# EAM Developer Quick Start

EAM consists of a Tauri/Rust backend in `src-tauri/src`, a React/Vite frontend
in `src`, and the existing EAM support modules under `t-src-modules`.

## Prerequisites

- Rust and Cargo
- Node.js
- Bun
- .NET Framework 4.8 for the existing Windows-only task installer project
- Tauri CLI (`cargo install tauri-cli`)

EAM no longer builds or ships a local Realm asset extractor. Item metadata,
40x40 item sprites, character stats, dungeon definitions, and fame bonuses come
from RotMGGameDataService.

## Setup

1. Clone EAM normally. The removed game-asset extractor submodule is not needed.
2. Run `npm i` or `bun i` in the repository root.
3. Build and link `t-src-modules/eam-commons-js` as described by that module.
4. Prepare the existing Windows helper binaries when working on those features.
5. Run `bun run tauri dev`.

Debug builds default to the development service at
`http://192.168.1.2:8090`. Override it before compiling when needed:

```powershell
$env:EAM_GAME_DATA_API_URL = 'https://game-data.example.com'
bun run tauri dev
```

Release builds have no private-network default. `EAM_GAME_DATA_API_URL` must be
set to the public HTTPS service URL when the Tauri binary is compiled.

On startup, EAM downloads the latest manifest or an available diff. It retains
the last-good manifest for offline startup and downloads content-addressed
sprites only when the UI needs them. Downloaded sprites are SHA-256 verified and
cached in application data. If neither the service nor a last-good manifest is
available, EAM still starts in degraded mode and represents unknown items with a
bundled question-mark sprite. An unavailable individual sprite uses the same
temporary placeholder without storing it in the rendered-item cache.

## Developer tools

- Windows: press F12 while EAM is focused.
- macOS: press Cmd+Shift+J or use Inspect.
- Reload with F5/Ctrl+R on Windows or Cmd+R on macOS.

## Production builds

Set `EAM_GAME_DATA_API_URL` to the public HTTPS endpoint, then run
`npm run tauri build`. Local unsigned builds may still need the signing and
updater settings in `src-tauri/tauri.conf.json` adjusted as described by the
Tauri documentation.
