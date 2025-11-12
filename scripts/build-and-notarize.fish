#!/usr/bin/env fish

# Build, Sign, and Notarize macOS App (Fish Shell Version)
# Run this script to build your app with proper signing and notarization

# Source the environment variables
source ./buildEnvKeys.fish

echo "🔨 Building Tauri app..."
npm run tauri build

# Get the app path
set APP_PATH "src-tauri/target/release/bundle/macos/Exalt Account Manager.app"

if not test -d "$APP_PATH"
    echo "❌ App not found at $APP_PATH"
    exit 1
end

echo "✅ App built successfully"

# The signing should already be done by Tauri, but let's verify
echo "🔍 Verifying app signature..."
codesign --verify --deep --strict --verbose=2 "$APP_PATH"

echo "🔍 Checking security features..."
spctl --assess --type exec --verbose "$APP_PATH"

# Find the DMG file - try multiple patterns
set DMG_FILES (ls src-tauri/target/release/bundle/dmg/*.dmg 2>/dev/null)
if test (count $DMG_FILES) -eq 0
    echo "❌ DMG not found"
    echo "🔍 Looking for DMG files in:"
    ls -la src-tauri/target/release/bundle/dmg/ 2>/dev/null
    exit 1
end
set DMG_FILE $DMG_FILES[1]

echo "📦 Found DMG: $DMG_FILE"

echo "🚀 Starting notarization process..."

# Submit for notarization
echo "Using Apple ID for notarization..."
xcrun notarytool submit "$DMG_FILE" \
    --apple-id "$APPLE_ID" \
    --password "$APPLE_PASSWORD" \
    --team-id "$APPLE_TEAM_ID" \
    --wait

if test $status -eq 0
    echo "📋 Stapling notarization to DMG..."
    xcrun stapler staple "$DMG_FILE"

    echo "✅ Verifying notarization..."
    xcrun stapler validate "$DMG_FILE"

    echo "🎉 Build, signing, and notarization complete!"
    echo "📁 Your notarized DMG is at: $DMG_FILE"
else
    echo "❌ Notarization failed"
    echo "💡 Check notarization history for details:"
    echo "xcrun notarytool history --apple-id '$APPLE_ID' --password '$APPLE_PASSWORD' --team-id '$APPLE_TEAM_ID'"
    exit 1
end