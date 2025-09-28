use base64::{engine::general_purpose::STANDARD, Engine};
use std::{error::Error, io};

use aes_gcm::{
    aead::{Aead, OsRng, rand_core::RngCore},
    Aes256Gcm, KeyInit, Nonce,
};
use zeroize::Zeroize;

use security_framework::passwords::{get_generic_password, set_generic_password};

const SERVICE: &str = "com.exaltaccountmanager.vault";
const ACCOUNT: &str = "master-key";

const V1_TAG: u8 = 0x01;
const NONCE_LEN: usize = 12;

fn get_or_create_master_key() -> Result<[u8; 32], Box<dyn Error>> {
    match get_generic_password(SERVICE, ACCOUNT) {
        Ok(bytes) => {
            if bytes.len() != 32 {
                return Err(format!("master key has unexpected size: {}", bytes.len()).into());
            }
            let mut key = [0u8; 32];
            key.copy_from_slice(&bytes);
            Ok(key)
        }
        Err(_) => {
            let mut key = [0u8; 32];
            OsRng.fill_bytes(&mut key);
            set_generic_password(SERVICE, ACCOUNT, &key)?;
            Ok(key)
        }
    }
}

pub fn encrypt_data(plaintext: &str) -> Result<String, Box<dyn Error>> {
    let mut key = get_or_create_master_key()?;
    let cipher = Aes256Gcm::new((&key).into());

    let mut nonce_bytes = [0u8; NONCE_LEN];
    OsRng.fill_bytes(&mut nonce_bytes);
    let nonce = Nonce::from_slice(&nonce_bytes);

    // Map aes_gcm::aead::Error -> io::Error so `?` can work with Box<dyn Error>
    let ct = cipher
        .encrypt(nonce, plaintext.as_bytes())
        .map_err(|_| io::Error::new(io::ErrorKind::Other, "aes-gcm encrypt failed"))?;

    let mut out = Vec::with_capacity(1 + NONCE_LEN + ct.len());
    out.push(V1_TAG);
    out.extend_from_slice(&nonce_bytes);
    out.extend_from_slice(&ct);

    key.zeroize();
    nonce_bytes.zeroize();

    Ok(STANDARD.encode(out))
}

pub fn decrypt_data(data_b64: &str) -> Result<String, Box<dyn Error>> {
    if data_b64.is_empty() {
        return Ok(String::new());
    }

    let mut blob = STANDARD.decode(data_b64)?;
    if blob.len() < 1 + NONCE_LEN + 16 {
        return Err("ciphertext too short".into());
    }
    if blob[0] != V1_TAG {
        return Err("unsupported ciphertext version".into());
    }

    let nonce = Nonce::from_slice(&blob[1 .. 1 + NONCE_LEN]);
    let ct = &blob[1 + NONCE_LEN ..];

    let mut key = get_or_create_master_key()?;
    let cipher = Aes256Gcm::new((&key).into());

    let pt = cipher
        .decrypt(nonce, ct)
        .map_err(|_| io::Error::new(io::ErrorKind::Other, "aes-gcm decrypt failed"))?;

    key.zeroize();
    blob.zeroize();

    let s = String::from_utf8(pt)?;
    Ok(s)
}
