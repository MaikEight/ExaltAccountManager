use crate::diesel_functions::get_user_data_by_key;
use crate::diesel_setup::DbPool;
use crate::paths::get_default_game_path;
use futures::stream::{self, StreamExt};
use lazy_static::lazy_static;
use log::{info, warn, error};
use reqwest::header::{HeaderMap, HeaderValue, CONTENT_LENGTH, CONTENT_TYPE};
use roxmltree::Document;
use serde::Deserialize;
use std::fs;
use std::fs::File;
use std::io;
use std::io::Error;
use std::io::Read;
use std::io::Write;
use std::path::{Path, PathBuf};
use std::sync::{Arc, Mutex};
use thiserror::Error;
use tokio;
use tokio::io::AsyncReadExt;

const ROTMG_FILE_LIST_URL: &str = "/checksum.json";

#[cfg(target_os = "windows")]
const ROTMG_APP_INIT_URL: &str =
    "https://www.realmofthemadgod.com/app/init?platform=standalonewindows64&key=9KnJFxtTvLu2frXv";

#[cfg(target_os = "macos")]
const ROTMG_APP_INIT_URL: &str = "https://www.realmofthemadgod.com/app/init?platform=standaloneosxuniversal&key=9KnJFxtTvLu2frXv";

lazy_static! {
    static ref APP_INIT_DATA: Arc<Mutex<AppInitData>> = Arc::new(Mutex::new(AppInitData {
        build_id: "".to_string(),
        build_cdn: "".to_string(),
        build_hash: "".to_string(),
    }));
    static ref FILE_LIST: Arc<Mutex<Vec<FileData>>> = Arc::new(Mutex::new(Vec::new()));
    static ref GAME_FILES_TO_UPDATE: Arc<Mutex<Vec<FileData>>> = Arc::new(Mutex::new(Vec::new()));
}

#[derive(Error, Debug)]
pub enum UpdaterError {
    #[error("IO error: {0}")]
    Io(#[from] io::Error),
    #[error("Reqwest error: {0}")]
    Reqwest(#[from] reqwest::Error),
    #[error("Other error: {0}")]
    Other(String),
    #[error("SerdeJson error: {0}")]
    SerdeJson(serde_json::Error),
}

#[derive(Clone)]
pub struct AppInitData {
    build_id: String,
    build_cdn: String,
    build_hash: String,
}

#[derive(Clone, Deserialize)]
#[allow(dead_code)]
pub struct FileData {
    file: String,
    checksum: String,
    permision: String,
    size: usize,
}

#[derive(Clone, Deserialize)]
pub struct FilesResponse {
    files: Vec<FileData>,
}

pub async fn get_app_init_data() -> Result<AppInitData, String> {
    let app_init_data = APP_INIT_DATA.lock().unwrap().clone();
    if !app_init_data.build_cdn.is_empty() && !app_init_data.build_hash.is_empty() {
        return Ok(app_init_data.clone());
    }

    info!("Fetching app init data from RotMG servers...");

    let mut headers = HeaderMap::new();
    headers.insert(CONTENT_LENGTH, HeaderValue::from_static("0"));
    headers.insert(
        CONTENT_TYPE,
        HeaderValue::from_static("application/x-www-form-urlencoded"),
    );

    let client = reqwest::Client::new();
    let res = client
        .post(ROTMG_APP_INIT_URL)
        .headers(headers)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    let doc = Document::parse(&body).unwrap();
    let mut build_id: &str = &"";
    let mut build_cdn: &str = &"";
    let mut build_hash: &str = &"";

    for node in doc.descendants() {
        if node.has_tag_name("BuildId") {
            build_id = node.text().map(|s| s).unwrap();
        }
        if node.has_tag_name("BuildCDN") {
            build_cdn = node.text().map(|s| s).unwrap();
        }

        if node.has_tag_name("BuildHash") {
            build_hash = node.text().map(|s| s).unwrap();
        }
    }

    let data = AppInitData {
        build_id: build_id.to_string(),
        build_cdn: build_cdn.to_string(),
        build_hash: build_hash.to_string(),
    };

    let mut app_init_data_mut = APP_INIT_DATA.lock().unwrap();
    *app_init_data_mut = data.clone();

    if data.build_cdn.is_empty() || data.build_hash.is_empty() {
        error!("Failed to get app init data from RotMG servers.");
        return Err("Failed to get app init data from RotMG servers.".to_string());
    }
    info!("Successfully fetched app init data from RotMG servers.");
    Ok(data)
}

pub async fn get_game_file_list() -> Result<Vec<FileData>, UpdaterError> {
    let file_list = FILE_LIST.lock().unwrap().clone();
    if !file_list.is_empty() {
        return Ok(file_list.clone().to_vec());
    }

    info!("Fetching game file list from RotMG servers...");

    let data = get_app_init_data()
        .await
        .expect("Failed to get app init data.");

    let url = format!(
        "{}{}{}{}",
        data.build_cdn, data.build_hash, data.build_id, ROTMG_FILE_LIST_URL
    );

    let files_res = reqwest::get(&url).await;
    let files: Vec<FileData>;
    match files_res {
        Ok(response) => {
            let raw_json = response.text().await?;
            let files_response = serde_json::from_str::<FilesResponse>(&raw_json)
                .map_err(UpdaterError::SerdeJson)?;
            files = files_response.files;
        }
        Err(e) => {
            return Err(UpdaterError::from(e));
        }
    }

    let mut file_list_mut = FILE_LIST.lock().unwrap();
    *file_list_mut = files.clone();

    info!("Successfully fetched game file list from RotMG servers.");

    Ok(files)
}

fn get_game_exe_path_with_fallback(pool: &DbPool) -> String {
    match get_user_data_by_key(pool, "game_exe_path".to_string()) {
        Ok(game_exe_path_data) => game_exe_path_data.dataValue,
        Err(_) => {
            warn!("Failed to read game_exe_path from database, using default path");
            get_default_game_path()
        }
    }
}

pub fn get_game_files_to_update(
    pool: &DbPool,
    force_recheck: bool,
) -> Result<Vec<FileData>, UpdaterError> {
    info!("Checking for game files to update, force_recheck: {}", force_recheck);

    if force_recheck {
        clean_global_variables();
    } else {
        let game_files_locked: std::sync::MutexGuard<Vec<FileData>> =
            GAME_FILES_TO_UPDATE.lock().unwrap();
        let game_files = game_files_locked.clone();
        if !game_files.is_empty() {
            info!("Found cached game files to update. Found {} files.", game_files.len());
            return Ok(game_files.clone().to_vec());
        }
    }

    let game_exe_path = get_game_exe_path_with_fallback(pool);
    let files_res = tokio::runtime::Runtime::new()
        .unwrap()
        .block_on(get_game_file_list());
    let files = files_res.map_err(|e| UpdaterError::Other(e.to_string()))?;
    tokio::runtime::Runtime::new()
        .unwrap()
        .block_on(get_game_files_to_update_impl(game_exe_path, files))
}

pub fn perform_game_update(pool: &DbPool) -> Result<bool, UpdaterError> {
    info!("Starting game update...");

    let pool = Arc::new(pool.clone());
    let pool_clone = Arc::clone(&pool);
    let game_files_data: Vec<FileData> = get_game_files_to_update(&*pool_clone.clone(), false)
        .map_err(|e| UpdaterError::Other(e.to_string()))?;

    let res = tokio::runtime::Runtime::new()
        .unwrap()
        .block_on(perform_game_update_impl(&*pool_clone, game_files_data));

    match res {
        Ok(true) => {
            info!("Game update completed successfully.");
            return Ok(true);
        },
        Ok(false) => {
            warn!("Game update did not complete successfully.");
            return Ok(false);
        },
        Err(e) => {
            error!("Failed to perform game update: {}", e);
            return Err(UpdaterError::Other(e.to_string()));
        },
    }
}

async fn perform_game_update_impl(
    pool: &DbPool,
    game_files_data: Vec<FileData>,
) -> Result<bool, String> {
    let game_exe_path = get_game_exe_path_with_fallback(pool);

    let game_root_path = get_game_root_path(game_exe_path);

    let build_cdn = APP_INIT_DATA.lock().unwrap().build_cdn.clone();
    let build_hash = APP_INIT_DATA.lock().unwrap().build_hash.clone();

    for game_file_data in game_files_data {
        //1.0. Build the download url for the file
        let url = format!(
            "{}{}/rotmg-exalt-win-64/{}.gz",
            build_cdn, build_hash, game_file_data.file
        );

        // 1.1. download file to ram
        let file_data = download_file_to_ram(&url)
            .await
            .map_err(|e| e.to_string())?;

        // 1.2. unzip file
        let unzipped_data = unzip_gzip_file(file_data).map_err(|e| e.to_string())?;

        // 1.3. save file to game root path + fileName (field: file)
        let file_path = Path::new(&game_root_path).join(&game_file_data.file);

        // Create the directory if it does not exist
        if let Some(parent_dir) = file_path.parent() {
            fs::create_dir_all(parent_dir).map_err(|e| e.to_string())?;
        }

        save_file_to_disk(file_path, unzipped_data).map_err(|e| e.to_string())?;
    }

    clean_global_variables();

    Ok(true)
}

fn clean_global_variables() {
    let mut app_init_data_mut = APP_INIT_DATA.lock().unwrap();
    *app_init_data_mut = AppInitData {
        build_id: "".to_string(),
        build_cdn: "".to_string(),
        build_hash: "".to_string(),
    };

    let mut file_list_mut = FILE_LIST.lock().unwrap();
    *file_list_mut = Vec::new();
}

async fn download_file_to_ram(url: &str) -> Result<Vec<u8>, std::io::Error> {
    let response = reqwest::get(url)
        .await
        .map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?;

    if !response.status().is_success() {
        return Err(std::io::Error::new(
            std::io::ErrorKind::Other,
            "Failed to download file",
        ));
    }

    Ok(response
        .bytes()
        .await
        .map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?
        .to_vec())
}

fn save_file_to_disk(path: PathBuf, data: Vec<u8>) -> std::io::Result<()> {
    let mut file = File::create(path)?;
    file.write_all(&data)?;
    Ok(())
}

//Only for gz files
fn unzip_gzip_file(data: Vec<u8>) -> Result<Vec<u8>, std::io::Error> {
    let mut gz = flate2::read::GzDecoder::new(&data[..]);
    let mut unzipped_data = Vec::new();
    gz.read_to_end(&mut unzipped_data)
        .map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?;
    Ok(unzipped_data)
}

async fn get_game_files_to_update_impl(
    game_exe_path: String,
    game_files_data: Vec<FileData>,
) -> Result<Vec<FileData>, UpdaterError> {
    let game_root_path = get_game_root_path(game_exe_path);
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

    let mut game_files = GAME_FILES_TO_UPDATE.lock().unwrap();
    *game_files = files_to_update.clone();

    info!("Found {} files to update.", files_to_update.len());

    Ok(files_to_update)
}

async fn get_md5_as_string(path: &Path) -> Result<String, Error> {
    let mut file = tokio::fs::File::open(path).await?;
    let mut buffer = Vec::new();
    file.read_to_end(&mut buffer).await?;
    let digest = md5::compute(&buffer);
    Ok(format!("{:x}", digest))
}

fn get_game_root_path(game_exe_path: String) -> String {
    let mut path_buf = PathBuf::from(game_exe_path);
    path_buf.pop();
    return path_buf.to_string_lossy().to_string();
}
