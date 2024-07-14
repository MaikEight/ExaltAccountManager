import { Box, Typography } from "@mui/material";
import ComponentBox from "../ComponentBox";
import Character from "../Realm/Character";
import ItemCanvas from "../Realm/ItemCanvas";
import items from "../../assets/constants";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import { useEffect, useState } from "react";

function AccountView({ account }) {
    const { totalItems } = useVaultPeeker();

    return (
        <ComponentBox
            title={account.name ? account.name : account.email}
            isCollapseable={true}
            sx={{
                mx: 0,
            }}
            innerSx={{
                dispaly: 'flex',
                flexDirection: 'coulmn',
                gap: 1
            }}
        >
            {/* Characters */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    gap: 1,
                    flexWrap: 'wrap',
                }}
            >
                {
                    account.character &&
                    account.character.map((char, index) => {
                        return (
                            <Character charIdentifier={`${account.email}_${char.char_id}_${index}`} key={index} character={char} />
                        );
                    })
                }
            </Box>
            {
                account && account.account &&
                <Box>
                    {/* Vault */}
                    <StorageView canvasIdentifier={account.email + "_Vault"} title={<StorageViewTitle title="Vault" image="realm/vault_portal.png" sx={{gap: 0.5}}/>} itemIds={account.account.vault.itemIds} totals={totalItems?.totals} />
                    {/* Gift Chest */}
                    <StorageView canvasIdentifier={account.email + "_Gift"} title={<StorageViewTitle title="Gift Chest" image="realm/gift_chest.png"/>} itemIds={account.account.gifts.itemIds} totals={totalItems?.totals} />
                    {/* Trade Chest */}
                    <StorageView canvasIdentifier={account.email + "_Trade"} title={<StorageViewTitle title="Material Storage" image="realm/material_storage.png"/>} itemIds={account.account.material_storage.itemIds} totals={totalItems?.totals} />
                    {/* Temporary Gifts */}
                    <StorageView canvasIdentifier={account.email + "_Temp"} title={<StorageViewTitle title="Temporary Gifts" image="realm/chest.png"/>} itemIds={account.account.temporary_gifts.itemIds} totals={totalItems?.totals} />
                    {/* Potion Storage */}
                    <StorageView canvasIdentifier={account.email + "_Potion"} title={<StorageViewTitle title="Potion Storage" image="realm/potion_storage_small.png"/>} itemIds={account.account.potions.itemIds} totals={totalItems?.totals} />
                </Box>
            }
        </ComponentBox>
    );
}

export default AccountView;

function StorageView({ canvasIdentifier, title, itemIds, totals }) {
    const [filteredItemIds, setFilteredItemIds] = useState(itemIds);
    const { addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();

    useEffect(() => {
        setFilteredItemIds(itemIds);

        addItemFilterCallback(canvasIdentifier, (itemIds) => { setFilteredItemIds(itemIds); }, itemIds);
        return () => {
            removeItemFilterCallback(canvasIdentifier);
        };
    }, [itemIds]);

    if(!filteredItemIds || filteredItemIds.length === 0) return null;

    return (
        <Box
            sx={{
                mt: 1,
                display: 'flex',
                flexDirection: 'column',
                gap: 1,
            }}
        >
            {title}
            <ItemCanvas canvasIdentifier={canvasIdentifier} imgSrc="renders.png" itemIds={filteredItemIds} items={items} totals={totals} override={{ fillNumbers: false }} />
        </Box>
    );
}

function StorageViewTitle({ title, image, sx }) {
    return (
        <Box
            sx={{
                ml: 1,
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'end',
                gap: 1,
                ...sx
            }}
        >
            <img src={image} alt={title} style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
            <Typography variant="button">{title}</Typography>
        </Box>
    );
}