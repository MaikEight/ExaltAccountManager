//C# Code
// var entropy = new byte[20];
// using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
//    rng.GetBytes(save.entropy);
// var accountsData = ProtectedData.Protect(accounts, entropy, DataProtectionScope.CurrentUser);

// Rust Code
extern crate winapi;

use winapi::um::dpapi::CryptUnprotectData;
use winapi::um::wincrypt::DATA_BLOB;
use std::ptr::null_mut;
use std::slice;
use std::str;
use base64::Engine;
use base64::engine::general_purpose::STANDARD;

pub fn decrypt_data(data: &str) -> Result<String, Box<dyn std::error::Error>> {
    // Decode the Base64 string back into bytes
    let decoded_data = STANDARD.decode(data)?;

    let mut in_blob = DATA_BLOB {
        cbData: decoded_data.len() as u32,
        pbData: decoded_data.as_ptr() as *mut _,
    };

    let mut decrypted_blob = DATA_BLOB {
        cbData: 0,
        pbData: null_mut(),
    };

    unsafe {
        CryptUnprotectData(&mut in_blob, null_mut(), null_mut(), null_mut(), null_mut(), 0, &mut decrypted_blob);
    }

    // Convert the decrypted blob to a byte slice
    let decrypted_data = unsafe { slice::from_raw_parts(decrypted_blob.pbData, decrypted_blob.cbData as usize) };

    // Convert the byte slice to a string
    let decrypted_string = str::from_utf8(decrypted_data)?;

    Ok(decrypted_string.to_string())
}