# EAM Developer Quick Start

EAM is a project with the following components:

- **Tauri Backend (Rust):**  
  Located in [src-tauri/src/main.rs](src-tauri/src/main.rs). This is the main backend of the application.

- **React Frontend (Vite + JS):**  
  See the source in [src/](src/). The frontend is built with Vite and React.

- **C# Sub-Project (.NET Framework 4.8) EAM_Task_Installer:**  
  This tool is in the C# project at [t-src-modules/EAM_Task_Installer/EAM_Task_Installer/Program.cs](t-src-modules/EAM_Task_Installer/EAM_Task_Installer/Program.cs).

- **C# Sub-Project (.Net Framework 4.8) EAM_Save_File_Converter:**  
  This tool is currently closed source.

- **Rust EAM_Plus_Lib:**  
  This is closed-source.   
  To still be able to compile, there is a mock-lib available at [t-src-modules/eam_plus_lib_mock](t-src-modules/eam_plus_lib_mock), just use it in the dependencies here [t-src-modules/eam_background_sync/Cargo.toml](t-src-modules/eam_background_sync/Cargo.toml)

## Prerequisites

- **Rust:** [Install the Rust](https://www.rust-lang.org/learn/get-started) toolchain and Cargo.
- **Node.js:** [Install Node.js](https://nodejs.org/en/download) for the React frontend.
- **Bun**: [Bun](https://bun.sh/) is used as our runtime.
- **.NET Framework 4.8:** Required for the C# sub-project. (Comes with windows)
- **Tauri CLI:** Install globally with `cargo install tauri-cli`.

## Setup Steps

1. **Install JavaScript Dependencies:**  
   - Run either `npm i` or `bun i` in the project root.
   - Run either `npm i` or `bun i` in [t-src-modules/eam-commons-js](t-src-modules/eam-commons-js)
  
2. **Link the eam-commons-js lib:**
   - Build the lib using `npm run build` in [t-src-modules/eam-commons-js]
   - Run either `npm link` or `bun link` in [t-src-modules/eam-commons-js](t-src-modules/eam-commons-js)
   - Run either `npm link eam-commons-js` or `bun link eam-commons-js` in the project root
  
3. **Build the included binaries (Windows only):**
   - Create a new folder called `IncludedBinaries` at `src-tauri` resulting in a path like: [src-tauri/IncludedBinaries](src-tauri/IncludedBinaries)
   - Copy the `EAM_Save_File_Converter.exe` from your known source into [src-tauri/IncludedBinaries](src-tauri/IncludedBinaries)
   - Build the [EAM_Task_Installer](t-src-modules\EAM_Task_Installer) in `Release` mode.
  
    If you have trouble building the `EAM_Task_Installer` you can just copy the current existing versions from your[C:\Users\\%username%\AppData\Local\ExaltAccountManager\v4](C:\Users\\%username%\AppData\Local\ExaltAccountManager\v4) into the specified destinations. (Requires an installed EAM version)

4. **Run EAM in developer mode:**
    Run `bun run tauri dev` in the root of the project.
    
    This will take quite a while for the first time but eventually you should see a transparent Window pop up. This windows will after a short time display EAM.
    
    **Enjoy coding 🥳** 

## Game data service

Item metadata, 40x40 item sprites, player stats and fame bonuses are not bundled
with EAM. They come from the [RotMGGameDataService](https://github.com/TadusPro/RotMGGameDataService),
which EAM queries on startup and caches in its application data directory.

There is nothing to set up: builds default to the public service at
`https://game-assets.api.exaltaccountmanager.com`. You do **not** need a local
instance for normal development.

To point EAM at a service instance of your own, set `EAM_GAME_DATA_API_URL`:

```powershell
$env:EAM_GAME_DATA_API_URL = 'http://127.0.0.1:8090'
bun run tauri dev
```

- **Debug builds** read the variable at runtime, so switching endpoints only
  needs a restart, not a rebuild.
- **Release builds** bake in whatever was set when the binary was compiled and
  ignore the runtime environment.
- `http://` is accepted by debug builds only. Release builds require HTTPS.

A locally hosted service needs its PostgreSQL connection configured and at least
one published build, otherwise `/api/v1/builds/latest` answers `503` and EAM
falls back to placeholder sprites. See that repository's own documentation.

### Bundled snapshot

A snapshot of the game data ships with the installer, in
[src-tauri/resources/game-data](src-tauri/resources/game-data). EAM seeds an
empty cache from it, so a **fresh install** whose first contact with the service
fails still shows real items instead of placeholders. An existing install never
touches it, because it already retains its last-good manifest. The service
supersedes the snapshot on the first successful refresh, including by a diff
against it.

Refresh it before cutting a release:

```powershell
./src-tauri/resources/game-data/update-snapshot.ps1
```

The script verifies the manifest against the hash the service publishes, stores
both files compressed, and both are committed. They are declared in
`tauri.conf.json`, and **the Rust build fails if either is missing**, so they
cannot be left out of a checkout.

If the snapshot, the service and a previously cached manifest are all
unavailable, EAM still starts and shows a question-mark placeholder for every
item.

## Recommendations

- We use [GitMojis](https://gitmoji.dev/) for commit messages, if you don't wish to do so, that's fine but expect your commits to be squashed upon merge.
- If possible use a GPG-Key to sign your commits.

## Bonus

- Opening the developer tools
  - **Windows**: Press **F12** when EAM is focused to open the Developer console.
  - **macOS**: Right-Click -> Inspect or press **CMD + Shift + J**
- Reload the page
  - **Windows**: Press **F5** or **Ctrl + R** to reload the window.
  - **macOS**: Right-Click -> Reload or open the **developer tools** and press **CMD + R**
- The **LocalStorage** and **SessionStorage** contain data.

## Building for Production

At the moment building for production is not done via GitHub actions since we use a Code Signing Certificate that is stored on a local SmartCard. If you wish to build your own version please do so by modifying the [src-tauri\tauri.conf.json](src-tauri\tauri.conf.json) file.

Remove the following properties in order to be able to build:

- `bundle.windows.certificateThumbprint` This is the code signing certificates thumbprint
- `bundle.windows.digestAlgorithm` This is the digest Algorithem for code signing
- `bundle.windows.timestampUrl` This is the timestamp server used for code signing
- `bundle.plugins.updater.endpoints` This are the endpoints for EAM-Updates. If you build your own version you need to either host your own or remove all update servers.
- `bundle.plugins.updater.pubkey` This is the public key of the updates, since your build requires it's own private key you need to either remove this property or have your own key-pair.  

More informations can be found at the Tauri documentation [https://tauri.app/reference/config/](https://tauri.app/reference/config/)  
When ready, use `npm run tauri build` in the root of the project.

Release builds resolve the game data service at compile time. Leaving
`EAM_GAME_DATA_API_URL` unset is fine and uses the public endpoint; set it only
when a build should target a different service.