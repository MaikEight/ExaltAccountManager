use std::collections::HashMap;

pub async fn get_device_unique_identifier() -> Result<String, String> {
    use sha1::{Digest, Sha1};
    use wmi::{COMLibrary, Variant, WMIConnection};

    let com_con = COMLibrary::new().map_err(|e| e.to_string())?;
    let wmi_con = WMIConnection::new(com_con.into()).map_err(|e| e.to_string())?;

    let mut concat_str = String::new();

    let baseboard: Vec<HashMap<String, Variant>> = wmi_con
        .raw_query("SELECT * FROM Win32_BaseBoard")
        .map_err(|e| e.to_string())?;
    for obj in baseboard {
        if let Some(Variant::String(serial)) = obj.get("SerialNumber") {
            concat_str.push_str(&serial);
        }
    }

    let bios: Vec<HashMap<String, Variant>> = wmi_con
        .raw_query("SELECT * FROM Win32_BIOS")
        .map_err(|e| e.to_string())?;
    for obj in bios {
        if let Some(Variant::String(serial)) = obj.get("SerialNumber") {
            concat_str.push_str(&serial);
        }
    }

    let os: Vec<HashMap<String, Variant>> = wmi_con
        .raw_query("SELECT * FROM Win32_OperatingSystem")
        .map_err(|e| e.to_string())?;
    for obj in os {
        if let Some(Variant::String(serial)) = obj.get("SerialNumber") {
            concat_str.push_str(&serial);
        }
    }

    let mut hasher = Sha1::new();
    hasher.update(concat_str);
    let result = hasher.finalize();
    let hashed = format!("{:x}", result);

    Ok(hashed)
}