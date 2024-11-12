import { fetch } from '@tauri-apps/plugin-http';
import { EAM_USERS_API } from '../../constants';

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

function getProfileImageUrl(imageUrl) {
    return imageUrl;
}

async function patchUserLlama(id_token) {
    const myHeaders = {
        "Authorization": `Bearer ${id_token}`
    };

    const requestOptions = {
        method: "PATCH",
        headers: myHeaders,
        body: JSON.stringify({}) // Assuming an empty body as in the original axios call
    };

    try {
        const response = await fetch(`${EAM_USERS_API}/user/llama`, requestOptions);

        if (!response?.ok) {
            console.error(`HTTP error! status: ${response.status} - ${response.statusText}`);
            return { error: true, message: response?.statusText };
        }

        return response;
    } catch (e) {
        console.error(e);
        return { error: true, message: e?.message };
    }
}

export { getProfileImage, getProfileImageUrl, patchUserLlama };