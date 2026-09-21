#!/usr/bin/env bash
#
# Build, Sign, and Notarize the macOS App
#
# Two artifacts get notarized, because they are distributed separately:
#   - the .app bundle, repacked into the updater tarball after stapling
#   - the .dmg, for fresh downloads
# That means two round trips to Apple's notary service per run.
#
# Signing uses the Developer ID certificate in the login keychain; Tauri
# handles that via bundle.macOS.signingIdentity in tauri.conf.json.
#
# Notarization is a separate online service and needs an account credential.
# Preferred: store one once in the keychain and name the profile here.
#
#   xcrun notarytool store-credentials "EAM_NOTARY" \
#       --key ~/.appstoreconnect/private_keys/AuthKey_XXXXXXXXXX.p8 \
#       --key-id XXXXXXXXXX --issuer <issuer-uuid>
#
#   NOTARY_KEYCHAIN_PROFILE   name of that stored profile
#
# Fallback, if no profile is stored:
#
#   APPLE_ID        Apple ID used for notarization
#   APPLE_PASSWORD  app-specific password (appleid.apple.com -> Sign-In and Security)
#   APPLE_TEAM_ID   7LV36PT6D2
#
# The updater signature additionally needs TAURI_SIGNING_PRIVATE_KEY and
# TAURI_SIGNING_PRIVATE_KEY_PASSWORD.
#
# Usage: ./scripts/build-and-notarize.sh

set -euo pipefail

# Run from the repo root regardless of where the script was invoked from,
# since every path below is relative to it.
SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR/.."

# If the direnv shell hook did not run (non-interactive shell, IDE terminal,
# CI), pull the environment in explicitly.
# Always reload after the cd above, rather than only when the environment looks
# empty. direnv loads the NEAREST .envrc, so a shell started in a subdirectory
# can carry a partial environment — the notary profile but not the signing key,
# say — and a check for "some variable is set" would wrongly skip the reload.
if [ -f .envrc ] && command -v direnv >/dev/null 2>&1; then
    eval "$(direnv export bash 2>/dev/null || true)"
fi

# A stored keychain profile keeps every secret out of the environment, so it
# wins when present. Otherwise fall back to Apple ID + app-specific password.
if [ -n "${NOTARY_KEYCHAIN_PROFILE:-}" ]; then
    NOTARY_AUTH=(--keychain-profile "$NOTARY_KEYCHAIN_PROFILE")
    NOTARY_AUTH_DESC="keychain profile $NOTARY_KEYCHAIN_PROFILE"
else
    MISSING=""
    for VAR in APPLE_ID APPLE_PASSWORD APPLE_TEAM_ID; do
        if [ -z "${!VAR:-}" ]; then
            MISSING="$MISSING $VAR"
        fi
    done

    if [ -n "$MISSING" ]; then
        echo "❌ No notarization credentials found."
        echo "   NOTARY_KEYCHAIN_PROFILE is unset and these are missing:$MISSING"
        echo
        echo "💡 Recommended — store a credential once, then set only the profile name:"
        echo "     xcrun notarytool store-credentials \"EAM_NOTARY\" \\"
        echo "         --key ~/.appstoreconnect/private_keys/AuthKey_XXXXXXXXXX.p8 \\"
        echo "         --key-id XXXXXXXXXX --issuer <issuer-uuid>"
        echo "     echo 'export NOTARY_KEYCHAIN_PROFILE=\"EAM_NOTARY\"' >> .envrc && direnv allow"
        exit 1
    fi

    NOTARY_AUTH=(--apple-id "$APPLE_ID" --password "$APPLE_PASSWORD" --team-id "$APPLE_TEAM_ID")
    NOTARY_AUTH_DESC="Apple ID $APPLE_ID"
fi

if [ -z "${TAURI_SIGNING_PRIVATE_KEY:-}" ]; then
    echo "⚠️  TAURI_SIGNING_PRIVATE_KEY is not set — the updater artifacts will be unsigned."
else
    # Updater signing is the very last step of the build, so a bad key password
    # otherwise only surfaces after a full release compile. Test-sign first.
    echo "🔑 Checking updater signing key..."
    SIGN_PROBE="$(mktemp -t eam-sign-probe)"
    if npx --no-install tauri signer sign "$SIGN_PROBE" >/dev/null 2>&1; then
        echo "✅ Updater key and password match"
    else
        rm -f "$SIGN_PROBE" "$SIGN_PROBE.sig"
        echo "❌ Cannot sign with TAURI_SIGNING_PRIVATE_KEY."
        echo "   The key and TAURI_SIGNING_PRIVATE_KEY_PASSWORD do not match."
        echo "💡 Check .envrc — each export must be on its own line, and a value"
        echo "   containing \$ # & ? needs single quotes."
        exit 1
    fi
    rm -f "$SIGN_PROBE" "$SIGN_PROBE.sig"
fi

echo "🔨 Building Tauri app..."
npm run tauri build

APP_PATH="src-tauri/target/release/bundle/macos/Exalt Account Manager.app"

if [ ! -d "$APP_PATH" ]; then
    echo "❌ App not found at $APP_PATH"
    exit 1
fi

echo "✅ App built successfully"

# Tauri signs the bundle during the build; verify that it actually happened.
echo "🔍 Verifying app signature..."
codesign --verify --deep --strict --verbose=2 "$APP_PATH"

# Informational only: Gatekeeper necessarily rejects the app here with
# "source=Unnotarized Developer ID", because notarization happens below.
# Exit code 3 must not abort the run.
echo "🔍 Checking Gatekeeper status (pre-notarization)..."
spctl --assess --type exec --verbose "$APP_PATH" 2>&1 | sed 's/^/   /' || true

APP_VERSION="$(node -p "require('./src-tauri/tauri.conf.json').version")"
UPDATER_TAR="src-tauri/target/release/bundle/macos/Exalt Account Manager.app.tar.gz"

# The updater ships the .app itself, not the DMG, so the bundle needs its own
# notarization pass. Stapling it here means an update installs and launches
# even with no network to reach Apple's notary service.
echo "🚀 Notarizing the app bundle ($NOTARY_AUTH_DESC)..."
APP_ZIP_DIR="$(mktemp -d)"
ditto -c -k --keepParent "$APP_PATH" "$APP_ZIP_DIR/app.zip"

if ! xcrun notarytool submit "$APP_ZIP_DIR/app.zip" "${NOTARY_AUTH[@]}" --wait; then
    rm -rf "$APP_ZIP_DIR"
    echo "❌ Notarization of the app bundle failed"
    exit 1
fi
rm -rf "$APP_ZIP_DIR"

echo "📋 Stapling notarization to the app..."
xcrun stapler staple "$APP_PATH"
xcrun stapler validate "$APP_PATH"

# Stapling writes the ticket into the bundle, so re-check that the signature
# still seals correctly rather than assuming it does.
echo "🔍 Re-verifying app signature after stapling..."
codesign --verify --deep --strict --verbose=2 "$APP_PATH"

# Rebuild the updater tarball, which the build produced from the app as it was
# BEFORE stapling and therefore has no ticket inside.
if [ -f "$UPDATER_TAR" ]; then
    echo "📦 Repacking updater artifact from the stapled app..."
    tar czf "$UPDATER_TAR" --no-mac-metadata --no-xattrs \
        -C "$(dirname "$APP_PATH")" "$(basename "$APP_PATH")"

    if [ -n "${TAURI_SIGNING_PRIVATE_KEY:-}" ]; then
        # --app-version reproduces the "version:" field that `tauri build` puts
        # in the signature's trusted comment; without it requireSignedVersion
        # updaters reject the artifact.
        echo "🔑 Re-signing updater artifact (version $APP_VERSION)..."
        rm -f "$UPDATER_TAR.sig"
        npx --no-install tauri signer sign --app-version "$APP_VERSION" "$UPDATER_TAR" >/dev/null
        echo "✅ Updater artifact repacked and re-signed"
    else
        echo "⚠️  Updater artifact repacked but NOT re-signed — its old .sig is now stale."
    fi
fi

# Pick the most recently written DMG, so a stale build from an older version
# can never be notarized by accident.
# The trailing `|| true` matters: once enough DMGs accumulate, `head` closes
# the pipe while `ls` is still writing, and pipefail would abort the script.
DMG_FILE="$(find src-tauri/target/release/bundle/dmg -maxdepth 1 -name '*.dmg' -print0 2>/dev/null \
    | xargs -0 ls -t 2>/dev/null \
    | head -n 1 || true)"

if [ -z "$DMG_FILE" ]; then
    echo "❌ DMG not found"
    echo "🔍 Looking for DMG files in:"
    ls -la src-tauri/target/release/bundle/dmg/ 2>/dev/null || true
    exit 1
fi

echo "📦 Found DMG: $DMG_FILE"

echo "🚀 Starting notarization process..."
echo "Authenticating with $NOTARY_AUTH_DESC..."

if xcrun notarytool submit "$DMG_FILE" "${NOTARY_AUTH[@]}" --wait; then

    echo "📋 Stapling notarization to DMG..."
    xcrun stapler staple "$DMG_FILE"

    echo "✅ Verifying notarization..."
    xcrun stapler validate "$DMG_FILE"

    echo "🎉 Build, signing, and notarization complete!"
    echo "📁 Your notarized DMG is at: $DMG_FILE"
else
    echo "❌ Notarization failed"
    echo "💡 Check the notarization history for details:"
    if [ -n "${NOTARY_KEYCHAIN_PROFILE:-}" ]; then
        echo "   xcrun notarytool history --keychain-profile \"$NOTARY_KEYCHAIN_PROFILE\""
    else
        echo "   xcrun notarytool history --apple-id \"\$APPLE_ID\" --password \"\$APPLE_PASSWORD\" --team-id \"\$APPLE_TEAM_ID\""
    fi
    exit 1
fi
