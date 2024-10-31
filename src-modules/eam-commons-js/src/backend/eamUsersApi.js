import { fetch } from '@tauri-apps/plugin-http';

async function getProfileImage(imageUrl) {
    const response = await fetch(
        getProfileImageUrl(imageUrl),
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Content-Length': '0'
            },
        });

    if (!response.ok) {
        console.log(`HTTP error! status: ${response.status}-${response}`);
        return;
    }

    const imageData = await response.arrayBuffer();
    const base64String = arrayBufferToBase64(imageData);
    return base64String;
}

function arrayBufferToBase64(buffer) {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    const len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

function getProfileImageUrl(imageUrl) 
{ 
    return imageUrl;
    // return `https://user.exaltaccountmanager.com/get-image.php?imageUrl=${imageUrl}&origin=ExaltAccountManager`;
}

export { getProfileImage, getProfileImageUrl };