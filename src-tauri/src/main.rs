//Prevents additional console window on Windows in release, DO NOT REMOVE!!
//#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

// FOR WINDOWS WITH TRANSPARENCY / BLUR / ACRYLIC
#![cfg_attr(
    all(not(debug_assertions), target_os = "windows"),
    windows_subsystem = "windows"
)]
use futures::stream::{self, StreamExt};
use serde::{Deserialize, Serialize};
use std::path::Path;
use std::path::PathBuf;
use tauri::Error;
use tauri::Manager;
use tokio::fs as tokio_fs;
use tokio::io::{AsyncReadExt, BufReader};
use window_vibrancy::apply_blur;
use std::fs::File;
use flate2::read::GzDecoder;
use walkdir::WalkDir;
use std::io::{Read, Write};
use reqwest::get;
use zip::read::ZipArchive;
// Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
// #[tauri::command]
// fn greet(name: &str) -> String {
//     format!("Hello, {}! You've been greeted from Rust!", name)
// }

extern crate dirs;

#[tauri::command]
fn get_save_file_path() -> String {
    //OS dependent fixed path
    //Windows: C:\Users\USERNAME\AppData\Local\ExaltAccountManager\v4\
    //Mac: /Users/USERNAME/Library/Application Support/ExaltAccountManager/v4/
    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    path.to_str().unwrap().to_string()
}

#[tauri::command]
fn combine_paths(path1: String, path2: String) -> Result<String, Error> {
    let mut path_buf = PathBuf::from(path1);
    path_buf.push(&path2);

    Ok(path_buf.to_string_lossy().to_string())
}

#[tauri::command]
fn start_application(
    application_path: String,
    start_parameters: String,
) -> Result<(), tauri::Error> {
    let mut command = match std::env::consts::OS {
        "windows" => {
            // Wrap the application path in quotes
            let mut cmd = std::process::Command::new("powershell");
            cmd.arg("-Command")
                .arg("Start-Process")
                .arg(format!("\"{}\"", &application_path))
                .arg(format!("\"{}\"", start_parameters));
            cmd
        }
        _ => {
            let mut cmd = std::process::Command::new(&application_path);
            cmd.arg(&start_parameters);
            cmd
        }
    };

    let output = command.output()?;
    if !output.status.success() {
        return Err(tauri::Error::from(std::io::Error::new(
            std::io::ErrorKind::Other,
            format!(
                "Error starting application: {}",
                String::from_utf8_lossy(&output.stderr)
            ),
        )));
    }

    Ok(())
}

#[derive(Debug, Deserialize, Serialize)]
struct GameData {
    file: String,
    checksum: String,
    permision: String,
    size: usize,
}

#[derive(serde::Deserialize)]
struct GetGameFilesToUpdateArgs {
    game_exe_path: String,
    game_files_data: String,
}

#[tauri::command]
async fn get_game_files_to_update(args: GetGameFilesToUpdateArgs) -> tauri::Result<Vec<GameData>> {
    let (game_exe_path, game_files_data) = (args.game_exe_path, args.game_files_data);

    let result = get_game_files_to_update_impl(game_exe_path, game_files_data).await;

    match result {
        Ok(files_to_update) => Ok(files_to_update),
        Err(e) => Err(Error::from(e)),
    }
}

async fn get_game_files_to_update_impl(
    game_exe_path: String,
    game_files_data: String,
) -> Result<Vec<GameData>, Error> {
    let game_root_path = get_game_root_path(game_exe_path);
    let game_files_data: Vec<GameData> = serde_json::from_str(&game_files_data)?;
    let mut files_to_update = Vec::new();

    let stream = stream::iter(game_files_data.into_iter().map(|game_data| {
        let game_root_path = game_root_path.clone();
        async move {
            let file_path = Path::new(&game_root_path).join(&game_data.file);
            if !file_path.exists() || game_data.checksum != get_md5_as_string(&file_path).await? {
                Ok(Some(game_data))
            } else {
                Ok(None)
            }
        }
    }));

    let results: Vec<_> = stream.buffer_unordered(10).collect().await;

    for result in results {
        match result {
            Ok(Some(game_data)) => files_to_update.push(game_data),
            Ok(None) => {}
            Err(e) => return Err(e),
        }
    }

    Ok(files_to_update)
}

async fn get_md5_as_string(path: &Path) -> Result<String, Error> {
    let mut file = BufReader::new(tokio_fs::File::open(path).await?);
    let mut buffer = Vec::new();
    file.read_to_end(&mut buffer).await?;
    let digest = md5::compute(&buffer);
    Ok(format!("{:x}", digest))
}

#[derive(Debug, Deserialize, Serialize)]
struct GameFileData {
    file: String,
    checksum: String,
    permision: String,
    size: usize,
    url: String,
}

#[derive(serde::Deserialize)]
struct PerformGameUpdateArgs {
    game_exe_path: String,
    game_files_data: String,
}

use std::sync::mpsc::channel;
use std::thread;

#[tauri::command]
fn perform_game_update(args: PerformGameUpdateArgs) -> Result<(), String> {
    let (tx, rx) = channel();

    thread::spawn(move || {
        let result = perform_game_update_impl(args);

        tx.send(result).unwrap();
    });

    match rx.recv().unwrap() {
        Ok(_) => Ok(()),
        Err(e) => Err(e),
    }
}

fn perform_game_update_impl(args: PerformGameUpdateArgs) -> Result<(), String> {
    let (game_exe_path, game_files_data) = (args.game_exe_path, args.game_files_data);
    let game_root_path = get_game_root_path(game_exe_path);
    let game_files_data: Vec<GameFileData> = serde_json::from_str(&game_files_data)
        .map_err(|e| e.to_string())?;

    for game_file_data in game_files_data {
        // 1.1. download file to ram
        let file_data = download_file_to_ram(&game_file_data.url)
            .map_err(|e| e.to_string())?;

        // 1.2. unzip file
        let unzipped_data = unzip_file(file_data)
            .map_err(|e| e.to_string())?;

        // 1.3. save file to game root path + fileName (field: file)
        let file_path = Path::new(&game_root_path).join(&game_file_data.file);
        save_file_to_disk(file_path, unzipped_data)
            .map_err(|e| e.to_string())?;        
    }

    Ok(())
}


fn download_file_to_ram(url: &str) -> Result<Vec<u8>, std::io::Error> {
    let response = reqwest::blocking::get(url).map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?;

    if !response.status().is_success() {
        return Err(std::io::Error::new(std::io::ErrorKind::Other, "Failed to download file"));
    }

    Ok(response.bytes().map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?.to_vec())
}

fn save_file_to_disk(path: PathBuf, data: Vec<u8>) -> std::io::Result<()> {
    let mut file = File::create(path)?;
    file.write_all(&data)?;
    Ok(())
}

fn unzip_file(data: Vec<u8>) -> Result<Vec<u8>, std::io::Error> {
    let mut gz = flate2::read::GzDecoder::new(&data[..]);
    let mut unzipped_data = Vec::new();
    gz.read_to_end(&mut unzipped_data).map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?;
    Ok(unzipped_data)
}

#[tauri::command]
fn get_temp_folder_path_with_creation(sub_path: String) -> String {
    let mut path_buf = get_temp_folder_path();
    path_buf.push_str(&sub_path);
    let _ = create_folder(path_buf.clone());
    path_buf
}

#[tauri::command]
fn create_folder(path: String) -> Result<(), Error> {
    std::fs::create_dir_all(path)?;
    Ok(())
}

#[tauri::command]
fn unpack_and_move_game_update_files(temp_folder: String, game_exe_path: String) -> Result<(), String> {
    /*
        1. get game root path
        2. unpack files to game path (including sub-folders), overwrite existing files
        3. delete temp folder
    */
    let game_root_path = get_game_root_path(game_exe_path);
    for entry in WalkDir::new(temp_folder.clone()).into_iter().filter_map(|e| e.ok()) {
        if entry.path().extension().unwrap_or_default() == "gz" {
            let temp_file_path = entry.path();
            let game_file_path = Path::new(&game_root_path).join(temp_file_path.strip_prefix(&temp_folder).unwrap()).with_extension("");
            let temp_file = File::open(temp_file_path).map_err(|e| e.to_string())?;
            let mut gz = GzDecoder::new(temp_file);
            let mut game_file = File::create(game_file_path).map_err(|e| e.to_string())?;
            std::io::copy(&mut gz, &mut game_file).map_err(|e| e.to_string())?;
        }
    }
    Ok(())
}

#[tauri::command]
fn get_temp_folder_path() -> String {
    let mut path_buf = PathBuf::from(std::env::temp_dir());
    path_buf.push("ExaltAccountManager");
    path_buf.to_string_lossy().to_string()
}

// fn main() {
//     tauri::Builder::default()
//         .invoke_handler(tauri::generate_handler![
//             get_save_file_path,
//             combine_paths,
//             start_application
//         ])
//         .run(tauri::generate_context!())
//         .expect("error while running tauri application");
// }

// FOR WINDOWS WITH TRANSPARENCY / BLUR / ACRYLIC
fn main() {
    tauri::Builder::default()
        .setup(|app| {
            let window = app.get_window("main").unwrap();

            #[cfg(target_os = "macos")]
            apply_vibrancy(&window, NSVisualEffectMaterial::HudWindow, None, None)
                .expect("Unsupported platform! 'apply_vibrancy' is only supported on macOS");

            #[cfg(target_os = "windows")]
            apply_blur(&window, Some((18, 18, 18, 125)))
                .expect("Unsupported platform! 'apply_blur' is only supported on Windows");

            Ok(())
        })
        .invoke_handler(tauri::generate_handler![
            get_save_file_path,
            combine_paths,
            start_application,
            get_game_files_to_update,
            get_temp_folder_path,
            get_temp_folder_path_with_creation,
            create_folder,
            unpack_and_move_game_update_files,
            perform_game_update
        ])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}

//Helper function to get the path to the application directory
fn get_game_root_path(game_exe_path: String) -> String {
    let mut path_buf = PathBuf::from(game_exe_path);
    path_buf.pop();
    return path_buf.to_string_lossy().to_string();
}
