extern crate winapi;

use winapi::um::dpapi::CryptProtectData;
use winapi::um::dpapi::CryptUnprotectData;
use winapi::um::wincrypt::DATA_BLOB;
use std::ptr::null_mut;
use std::slice;
use std::str;
use base64::Engine;
use base64::engine::general_purpose::STANDARD;


pub fn encrypt_data(data: &str) -> Result<String, Box<dyn std::error::Error>> {
    let data_bytes = data.as_bytes();

    let mut in_blob = DATA_BLOB {
        cbData: data_bytes.len() as u32,
        pbData: data_bytes.as_ptr() as *mut _,
    };

    let mut out_blob = DATA_BLOB {
        cbData: 0,
        pbData: null_mut(),
    };

    unsafe {
        CryptProtectData(&mut in_blob, null_mut(), null_mut(), null_mut(), null_mut(), 0, &mut out_blob);
    }

    // Convert the output blob to a byte slice
    let encrypted_data = unsafe { slice::from_raw_parts(out_blob.pbData, out_blob.cbData as usize) };

    // Encode the encrypted data as a Base64 string
    Ok(STANDARD.encode(encrypted_data))
}

pub fn decrypt_data(data: &str) -> Result<String, Box<dyn std::error::Error>> {
    //if the data is empty, return an empty string
    if data.is_empty() {
        return Ok("".to_string());
    }
    
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