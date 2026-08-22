use std::path::PathBuf;

///OS dependent fixed path.
/// 
///Windows: `C:\Users\USERNAME\AppData\Local\ExaltAccountManager\v4\`
/// 
///Mac: `~/Library/Application Support/ExaltAccountManager/v4/`
pub fn get_save_file_path() -> String {    
    let path = get_save_file_path_buf();
    path.to_str().unwrap().to_string()
}

///OS dependent fixed path.
/// 
///Windows: `C:\Users\USERNAME\AppData\Local\ExaltAccountManager\v4\`
/// 
///Mac: `~/Library/Application Support/ExaltAccountManager/v4/`
pub fn get_save_file_path_buf() -> PathBuf {    
    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    path
}

///OS dependent default game path.
/// 
///Windows: `C:\Users\USERNAME\Documents\RealmOfTheMadGod\Production\RotMG Exalt.exe`
/// 
///Mac: `~/RealmOfTheMadGod/Production/RotMGExalt.app`
pub fn get_default_game_path() -> String {
    #[cfg(target_os = "windows")]
    {
        //C:\Users\USERNAME\Documents\RealmOfTheMadGod\Production\RotMG Exalt.exe
        let mut path = dirs::document_dir().unwrap();
        path.push("RealmOfTheMadGod");
        path.push("Production");
        path.push("RotMG Exalt.exe");
        path.to_str().unwrap().to_string()
    }
    #[cfg(target_os = "macos")]
    {
        //~/RealmOfTheMadGod/Production/RotMGExalt.app
        let mut path = dirs::home_dir().unwrap();
        path.push("RealmOfTheMadGod");
        path.push("Production");
        path.push("RotMGExalt.app");
        path.to_str().unwrap().to_string()
    }
}

///OS dependent default launcher path.
///
/// EAM starts the official launcher (not the game executable) to comply with
/// DECA's ToS. The launcher is a separate application from the game.
///
///Windows: `C:\Users\USERNAME\Documents\RealmOfTheMadGod\Launcher\RotMG Exalt Launcher.exe`
///
///Mac: `~/RealmOfTheMadGod/Launcher/RotMG Exalt Launcher.app`
pub fn get_default_launcher_path() -> String {
    #[cfg(target_os = "windows")]
    {
        // The launcher is a standalone install (its own installer), placed under
        // Program Files: `...\RotMG Exalt Launcher\RotMG Exalt Launcher.exe`.
        let program_files =
            std::env::var("ProgramFiles").unwrap_or_else(|_| "C:\\Program Files".to_string());
        let mut path = PathBuf::from(program_files);
        path.push("RotMG Exalt Launcher");
        path.push("RotMG Exalt Launcher.exe");
        path.to_str().unwrap().to_string()
    }
    #[cfg(target_os = "macos")]
    {
        // The launcher is installed as a standalone app via a .dmg.
        String::from("/Applications/RotMG Exalt Launcher/RotMG Exalt Launcher.app")
    }
    #[cfg(not(any(target_os = "windows", target_os = "macos")))]
    {
        String::new()
    }
}