import items from "../assets/constants";

export function getItemById(itemId) {
    return items[itemId];
}

export function formatAccountDataFromCharListDatasets(charListDatasets) {
    if (!charListDatasets) {
        return null;
    }

    if (charListDatasets.length === 1) {
        return [formatAccountDataFromCharListDataset(charListDatasets[0])];
    }

    return charListDatasets.map((charListDataset) => formatAccountDataFromCharListDataset(charListDataset));
};

export function formatAccountDataFromCharListDataset(charListDataset) {
    if (!charListDataset) {
        return null;
    }

    const accountData = {
        email: charListDataset.email,
        account: {},
        character: [],
    };

    //Extract items from account
    const account = {
        vault: {
            itemIds: [],
            totals: {},
        },
        gifts: {
            itemIds: [],
            totals: {},
        },
        material_storage: {
            itemIds: [],
            totals: {},
        },
        temporary_gifts: {
            itemIds: [],
            totals: {},
        },
        potions: {
            itemIds: [],
            totals: {},
        },
    }

    const extracedVault = extractItemIdsFromValueString(charListDataset.account.vault, true, false);
    const extractedGifts = extractItemIdsFromValueString(charListDataset.account.gifts, true, false);
    const extractedMaterialStorage = extractItemIdsFromValueString(charListDataset.account.material_storage, true, false);
    const extractedTemporaryGifts = extractItemIdsFromValueString(charListDataset.account.temporary_gifts, true, false);
    const extractedPotions = extractItemIdsFromValueString(charListDataset.account.potions, true, false);

    extracedVault.forEach((itemId) => {
        account.vault.itemIds.push(itemId);
        account.vault.totals[itemId] = account.vault.totals[itemId] ? account.vault.totals[itemId] + 1 : 1;
    });
    account.vault.itemIds = Array.from(new Set(account.vault.itemIds));

    extractedGifts.forEach((itemId) => {
        account.gifts.itemIds.push(itemId);
        account.gifts.totals[itemId] = account.gifts.totals[itemId] ? account.gifts.totals[itemId] + 1 : 1;
    });
    account.gifts.itemIds = Array.from(new Set(account.gifts.itemIds));

    extractedMaterialStorage.forEach((itemId) => {
        account.material_storage.itemIds.push(itemId);
        account.material_storage.totals[itemId] = account.material_storage.totals[itemId] ? account.material_storage.totals[itemId] + 1 : 1;
    });
    account.material_storage.itemIds = Array.from(new Set(account.material_storage.itemIds));

    extractedTemporaryGifts.forEach((itemId) => {
        account.temporary_gifts.itemIds.push(itemId);
        account.temporary_gifts.totals[itemId] = account.temporary_gifts.totals[itemId] ? account.temporary_gifts.totals[itemId] + 1 : 1;
    });
    account.temporary_gifts.itemIds = Array.from(new Set(account.temporary_gifts.itemIds));

    extractedPotions.forEach((itemId) => {
        account.potions.itemIds.push(itemId);
        account.potions.totals[itemId] = account.potions.totals[itemId] ? account.potions.totals[itemId] + 1 : 1;
    });
    account.potions.itemIds = Array.from(new Set(account.potions.itemIds));

    accountData.account = account;


    charListDataset.character.forEach((character) => {
        accountData.character.push(formatCharacterDataFromCharListDataset(character));
    });

    //Extract character data
    accountData.character = accountData.character.filter((character) => character !== null && character.char_id !== undefined);

    return accountData;
}

export function formatCharacterDataFromCharListDataset(character) {
    if (!character) {
        return null;
    }
    if (character.char_id === undefined) {
        return null;
    }
    

    return {
        char_id: character.char_id,
        creation_date: character.creation_date,
        class: character.char_class,
        level: character.level,
        backpackSlots: character.backpack_slots,
        hasBelt: character.has3_quickslots === 1,
        equipment: extractItemIdsFromValueString(character.equipment, true, false),
        maxHp: character.max_hit_points,
        maxMp: character.max_magic_points,
        def: character.defense,
        spd: character.speed,
        dex: character.dexterity,
        vit: character.hp_regen,
        wis: character.mp_regen,
        atk: character.attack,
        exp: character.exp,
        fame: character.current_fame,
        seasonal: character.seasonal,
        dead: character.dead,
        tex1: character.tex1,
        tex2: character.tex2,
        texture: character.texture,
        crucibleActive: character.crucible_active,
        raw_pc_stats: character.pc_stats,
    };
}

export function extractRealmItemsFromCharListDatasets(charListDatasets) {
    if (!charListDatasets) {
        return {
            itemIds: [],
            totals: {},
        };
    }

    const realmItems = {
        itemIds: [],
        totals: {},
    };

    const extractedItems = charListDatasets.map((charListDataset) => extractRealmItemsFromCharListDataset(charListDataset));
    extractedItems.forEach((extractedItem) => {
        extractedItem.itemIds.forEach((itemId) => {
            realmItems.itemIds.push(itemId);
            realmItems.totals[itemId] = realmItems.totals[itemId] ?
                {
                    amount: realmItems.totals[itemId].amount + extractedItem.totals[itemId].amount,
                    location: {
                        ...realmItems.totals[itemId].location,
                        ...extractedItem.totals[itemId].location,
                    },
                }
                :
                extractedItem.totals[itemId];
        });
    });

    realmItems.itemIds = Array.from(new Set(realmItems.itemIds));
    if(realmItems.itemIds.includes(-1)) { 
        // Remove -1 from the itemIds array and add it to the end of the array
        // This is to ensure that -1 is always at the end of the array
        realmItems.itemIds = realmItems.itemIds.filter((itemId) => itemId !== -1).concat([-1]);
    }
    
    return realmItems;
}


export function extractRealmItemsFromCharListDataset(charListDataset) {
    if (!charListDataset) {
        return {
            itemIds: [],
            totals: {},
        };
    }

    const realmItems = {
        itemIds: [],
        totals: {},
    };

    const email = charListDataset.email;
    if (charListDataset.account) {
        // Extract items from account        
        const vaultItems = extractItemIdsFromValueString(charListDataset.account.vault, true, false);
        const giftsItems = extractItemIdsFromValueString(charListDataset.account.gifts, true, false);
        const materialStorageItems = extractItemIdsFromValueString(charListDataset.account.material_storage, true, false);
        const temporaryGiftsItems = extractItemIdsFromValueString(charListDataset.account.temporary_gifts, true, false);
        const potionsItems = extractItemIdsFromValueString(charListDataset.account.potions, true, false);

        const characterItems = [];
        charListDataset.character.forEach((character) => {
            characterItems.push(extractItemIdsFromValueString(character.equipment, true, false));
        });

        const allItems = new Set([...vaultItems, ...giftsItems, ...materialStorageItems, ...temporaryGiftsItems, ...potionsItems, ...characterItems.flat()]);
        realmItems.itemIds = Array.from(allItems);

        // Extract totals from account
        vaultItems.forEach((itemId) => {
            realmItems.totals[itemId] = realmItems.totals[itemId] ?
                {
                    ...realmItems.totals[itemId],
                    amount: realmItems.totals[itemId].amount + 1,
                }
                :
                {
                    amount: 1,
                    location: {},
                };

            realmItems.totals[itemId].location[email] =
                realmItems.totals[itemId].location[email] ?
                    {
                        ...realmItems.totals[itemId].location[email],
                        vault: realmItems.totals[itemId].location[email].vault + 1,
                    }
                    : {
                        vault: 1,
                        gift: 0,
                        materialStorage: 0,
                        temporaryGifts: 0,
                        potions: 0,
                        character: 0,
                    };
        });

        giftsItems.forEach((itemId) => {
            realmItems.totals[itemId] = realmItems.totals[itemId] ?
                {
                    ...realmItems.totals[itemId],
                    amount: realmItems.totals[itemId].amount + 1,
                }
                :
                {
                    amount: 1,
                    location: {},
                };
            realmItems.totals[itemId].location[email] =
                realmItems.totals[itemId].location[email] ?
                    {
                        ...realmItems.totals[itemId].location[email],
                        gift: realmItems.totals[itemId].location[email].gift + 1,
                    }
                    : {
                        vault: 0,
                        gift: 1,
                        materialStorage: 0,
                        temporaryGifts: 0,
                        potions: 0,
                        character: 0,
                    };
        });

        materialStorageItems.forEach((itemId) => {
            realmItems.totals[itemId] = realmItems.totals[itemId] ?
                {
                    ...realmItems.totals[itemId],
                    amount: realmItems.totals[itemId].amount + 1,
                }
                :
                {
                    amount: 1,
                    location: {},
                };
            realmItems.totals[itemId].location[email] =
                realmItems.totals[itemId].location[email] ?
                    {
                        ...realmItems.totals[itemId].location[email],
                        materialStorage: realmItems.totals[itemId].location[email].materialStorage + 1,
                    }
                    : {
                        vault: 0,
                        gift: 0,
                        materialStorage: 1,
                        temporaryGifts: 0,
                        potions: 0,
                        character: 0,
                    };
        });

        temporaryGiftsItems.forEach((itemId) => {
            realmItems.totals[itemId] = realmItems.totals[itemId] ?
                {
                    ...realmItems.totals[itemId],
                    amount: realmItems.totals[itemId].amount + 1,
                }
                :
                {
                    amount: 1,
                    location: {},
                };
            realmItems.totals[itemId].location[email] =
                realmItems.totals[itemId].location[email] ?
                    {
                        ...realmItems.totals[itemId].location[email],
                        temporaryGifts: realmItems.totals[itemId].location[email].temporaryGifts + 1,
                    }
                    : {
                        vault: 0,
                        gift: 0,
                        materialStorage: 0,
                        temporaryGifts: 1,
                        potions: 0,
                        character: 0,
                    };
        });

        potionsItems.forEach((itemId) => {
            realmItems.totals[itemId] = realmItems.totals[itemId] ?
                {
                    ...realmItems.totals[itemId],
                    amount: realmItems.totals[itemId].amount + 1,
                }
                :
                {
                    amount: 1,
                    location: {},
                };
            realmItems.totals[itemId].location[email] =
                realmItems.totals[itemId].location[email] ?
                    {
                        ...realmItems.totals[itemId].location[email],
                        potions: realmItems.totals[itemId].location[email].potions + 1,
                    }
                    : {
                        vault: 0,
                        gift: 0,
                        materialStorage: 0,
                        temporaryGifts: 0,
                        potions: 1,
                        character: 0,
                    };
        });

        characterItems.forEach((characterItem) => {
            characterItem.forEach((itemId) => {
                realmItems.totals[itemId] = realmItems.totals[itemId] ?
                    {
                        ...realmItems.totals[itemId],
                        amount: realmItems.totals[itemId].amount + 1,
                    }
                    :
                    {
                        amount: 1,
                        location: {},
                    };
                realmItems.totals[itemId].location[email] =
                    realmItems.totals[itemId].location[email] ?
                        {
                            ...realmItems.totals[itemId].location[email],
                            character: realmItems.totals[itemId].location[email].character + 1,
                        }
                        : {
                            vault: 0,
                            gift: 0,
                            materialStorage: 0,
                            temporaryGifts: 0,
                            potions: 0,
                            character: 1,
                        };
            });
        });
    }

    return realmItems;
}

function extractItemIdsFromValueString(valueString, removeTrackingId = true, removeEmptyValues = true) {
    if (!valueString) {
        return [];
    }
    let vaultItems = valueString.split(',');
    if (removeTrackingId) {
        vaultItems = vaultItems.map((item) => item.split('#')[0]);
    }

    if (removeEmptyValues) {
        vaultItems = vaultItems.filter((itemId) => itemId !== '-1');
    } else {
        // Sort the items so that -1 is at the end of the array
        vaultItems = vaultItems.sort((a, b) => {
            if (a === '-1' && b !== '-1') {
                return 1;
            } else if (a !== '-1' && b === '-1') {
                return -1;
            }
            return 0;
        });
    }

    return vaultItems.map((itemId) => !itemId.includes('#') ? parseInt(itemId, 10) : itemId);
}