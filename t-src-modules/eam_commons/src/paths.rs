///OS dependent fixed path.
/// 
///Windows: `C:\Users\USERNAME\AppData\Local\ExaltAccountManager\v4\`
/// 
///Mac: `~/Library/Application Support/ExaltAccountManager/v4/`
pub fn get_save_file_path() -> String {
    
    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    path.to_str().unwrap().to_string()
}