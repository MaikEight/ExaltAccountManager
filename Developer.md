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

## Prerequisites

- **Rust:** [Install the Rust](https://www.rust-lang.org/learn/get-started) toolchain and Cargo.
- **Node.js:** [Install Node.js](https://nodejs.org/en/download) for the React frontend.
- **Bun** (Optional): For faster dependency installation [Bun](https://bun.sh/) can also be used.
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
  
3. **Build the included binaries:**
   - Create a new folder called `IncludedBinaries` at `src-tauri` resulting in a path like: [src-tauri/IncludedBinaries](src-tauri/IncludedBinaries)
   - Build the `daily_auto_login_cli` sub-project.
        - Run `cargo build --release` in [t-src-modules/daily_auto_login_cli](t-src-modules/daily_auto_login_cli)
        - Copy and rename the resulting .exe from [t-src-modules\daily_auto_login_cli\target\release\daily_auto_login.exe](t-src-modules\daily_auto_login_cli\target\release\daily_auto_login.exe) to [src-tauri\IncludedBinaries\EAM_Daily_Auto_Login.exe](src-tauri\IncludedBinaries\EAM_Daily_Auto_Login.exe)
   - Copy the `EAM_Save_File_Converter.exe` from your known source into [src-tauri/IncludedBinaries](src-tauri/IncludedBinaries)
   - Build the [EAM_Task_Installer](t-src-modules\EAM_Task_Installer) in `Release` mode.
  
    If you have trouble building the `daily_auto_login_cli` or the `EAM_Task_Installer` you can just copy the current existing versions from [C:\Users\\%username%\AppData\Local\ExaltAccountManager\v4](C:\Users\\%username%\AppData\Local\ExaltAccountManager\v4) into the specified destinations. (Requires an installed EAM version)

4. **Run EAM in developer mode:**
    Run `npm run tauri dev` in the root of the project.
    
    This will take quite a while for the first time but eventually you should see a transparent Window pop up. This windows will after a short time display EAM.
    
    **Enjoy coding 🥳** 

## Recommendations

- We use [GitMojis](https://gitmoji.dev/) for commit messages, if you don't wish to do so, that's fine but expect your commits to be squashed upon merge.
- If possible use a GPG-Key to sign your commits.

## Bonus

- Press **F12** when EAM is focused to open the Developer console.
- Press **F5** or **Ctrl + R** to reload the window.
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