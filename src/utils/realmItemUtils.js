import items from "../assets/constants";

export function getItemById(itemId) {
    return items[itemId];
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
        const vaultItems = extractItemIdsFromValueString(charListDataset.account.vault);
        const giftsItems = extractItemIdsFromValueString(charListDataset.account.gifts);
        const materialStorageItems = extractItemIdsFromValueString(charListDataset.account.material_storage);
        const temporaryGiftsItems = extractItemIdsFromValueString(charListDataset.account.temporary_gifts);
        const potionsItems = extractItemIdsFromValueString(charListDataset.account.potions);

        const allItems = new Set([...vaultItems, ...giftsItems, ...materialStorageItems, ...temporaryGiftsItems, ...potionsItems]);
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
                        potions: 0
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
                        potions: 0
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
                        potions: 0
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
                        potions: 0
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
                        potions: 1
                    };
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
    }

    return vaultItems.map((itemId) => !itemId.includes('#') ? parseInt(itemId, 10) : itemId);
}