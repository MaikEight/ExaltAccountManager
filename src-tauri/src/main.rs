//Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]


// FOR WINDOWS WITH TRANSPARENCY / BLUR / ACRYLIC
// #![cfg_attr(
//     all(not(debug_assertions), target_os = "windows"),
//     windows_subsystem = "windows"
//   )]  
//   use tauri::Manager;
//   use window_vibrancy::{apply_blur, apply_vibrancy, NSVisualEffectMaterial};


// Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
// #[tauri::command]
// fn greet(name: &str) -> String {
//     format!("Hello, {}! You've been greeted from Rust!", name)
// }

fn main() {
    tauri::Builder::default()
        //.invoke_handler(tauri::generate_handler![greet])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}

// FOR WINDOWS WITH TRANSPARENCY / BLUR / ACRYLIC
//   fn main() {
//     tauri::Builder::default()
//       .setup(|app| {
//         let window = app.get_window("main").unwrap();
  
//         #[cfg(target_os = "macos")]
//         apply_vibrancy(&window, NSVisualEffectMaterial::HudWindow, None, None)
//           .expect("Unsupported platform! 'apply_vibrancy' is only supported on macOS");
  
//         #[cfg(target_os = "windows")]
//         apply_blur(&window, Some((18, 18, 18, 125)))
//           .expect("Unsupported platform! 'apply_blur' is only supported on Windows");
  
//         Ok(())
//       })
//       .run(tauri::generate_context!())
//       .expect("error while running tauri application");
//   }
