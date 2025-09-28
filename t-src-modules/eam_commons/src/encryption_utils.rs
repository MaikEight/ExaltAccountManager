extern crate winapi;

use base64::engine::general_purpose::STANDARD;
use base64::Engine;
use std::{io, ptr::null_mut, slice};

use winapi::ctypes::c_void;
use winapi::shared::minwindef::BOOL;
use winapi::um::dpapi::{CryptProtectData, CryptUnprotectData, CRYPTPROTECT_UI_FORBIDDEN};
use winapi::um::errhandlingapi::GetLastError;
use winapi::um::winbase::LocalFree;
use winapi::um::wincrypt::DATA_BLOB;
use zeroize::Zeroize;

fn last_os_error(msg: &str) -> io::Error {
    let code = unsafe { GetLastError() } as i32;
    io::Error::new(io::ErrorKind::Other, format!("{msg}: winerr={code}"))
}

pub fn encrypt_data(data: &str) -> Result<String, Box<dyn std::error::Error>> {
    let data_bytes = data.as_bytes();

    let mut in_blob = DATA_BLOB {
        cbData: data_bytes.len() as u32,
        pbData: data_bytes.as_ptr() as *mut _,
    };

    // Windows allocates this; we must LocalFree it after copying.
    let mut out_blob = DATA_BLOB {
        cbData: 0,
        pbData: null_mut(),
    };

    let ok: BOOL = unsafe {
        CryptProtectData(
            &mut in_blob,
            null_mut(),
            null_mut(),
            null_mut(),
            null_mut(),
            CRYPTPROTECT_UI_FORBIDDEN, 
            &mut out_blob,
        )
    };

    if ok == 0 {
        return Err(Box::new(last_os_error("CryptProtectData failed")));
    }

    let encrypted: Vec<u8> = unsafe {
        slice::from_raw_parts(out_blob.pbData, out_blob.cbData as usize).to_vec()
    };

    // Free Windows buffer
    if !out_blob.pbData.is_null() {
        unsafe { LocalFree(out_blob.pbData as *mut c_void) };
        out_blob.pbData = null_mut();
    }

    Ok(STANDARD.encode(&encrypted))
}

pub fn decrypt_data(data_b64: &str) -> Result<String, Box<dyn std::error::Error>> {
    if data_b64.is_empty() {
        return Ok(String::new());
    }

    let decoded = STANDARD.decode(data_b64)?;

    let mut in_blob = DATA_BLOB {
        cbData: decoded.len() as u32,
        pbData: decoded.as_ptr() as *mut _,
    };

    let mut out_blob = DATA_BLOB {
        cbData: 0,
        pbData: null_mut(),
    };

    let ok: BOOL = unsafe {
        CryptUnprotectData(
            &mut in_blob,
            null_mut(),
            null_mut(),
            null_mut(),
            null_mut(),
            CRYPTPROTECT_UI_FORBIDDEN,
            &mut out_blob,
        )
    };
    if ok == 0 {
        return Err(Box::new(last_os_error("CryptUnprotectData failed")));
    }

    // Copy plaintext out, then free the Windows buffer
    let mut plain: Vec<u8> = unsafe {
        slice::from_raw_parts(out_blob.pbData, out_blob.cbData as usize).to_vec()
    };

    if !out_blob.pbData.is_null() {
        unsafe { LocalFree(out_blob.pbData as *mut c_void) };
        out_blob.pbData = null_mut();
    }

    let s = String::from_utf8(plain.clone())?;
    plain.zeroize();

    Ok(s)
}
