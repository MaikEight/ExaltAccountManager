import { invoke } from "@tauri-apps/api/core";
import {
    getRuntimeSpriteHash,
    mergeRuntimeAssets,
    MISSING_ITEM_SPRITE_SOURCE,
} from "../assets/runtimeAssets";

const spritePromises = new Map();

/** Loads the current shared game-data manifest, using the last-good disk cache offline. */
export async function refreshRuntimeAssets(force = false) {
    const manifest = await invoke("refresh_asset_cache", { force });
    if (!mergeRuntimeAssets(manifest)) {
        throw new Error("The live game-data manifest was incomplete.");
    }
    if (manifest.clientCache?.warning) {
        console.warn("Using cached game data:", manifest.clientCache.warning);
    }
    return manifest;
}

/** Resolves one content-addressed sprite through Rust's verified disk cache. */
export function getRuntimeItemSpriteSource(item) {
    const spriteHash = getRuntimeSpriteHash(item);
    if (!spriteHash) {
        return Promise.resolve(MISSING_ITEM_SPRITE_SOURCE);
    }
    if (!spritePromises.has(spriteHash)) {
        const request = invoke("get_asset_sprite", { spriteHash })
            .catch((error) => {
                spritePromises.delete(spriteHash);
                console.warn(`Using the missing-item sprite for ${spriteHash}:`, error);
                return MISSING_ITEM_SPRITE_SOURCE;
            });
        spritePromises.set(spriteHash, request);
    }
    return spritePromises.get(spriteHash);
}
