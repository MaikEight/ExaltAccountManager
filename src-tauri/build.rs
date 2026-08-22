fn main() {
    println!("cargo:rerun-if-env-changed=EAM_GAME_DATA_API_URL");
    tauri_build::build()
}
