import { Box, Collapse, IconButton, Tooltip, Typography } from "@mui/material";
import ComponentBox from "../ComponentBox";
import Character from "../Realm/Character";
import ItemCanvas from "../Realm/ItemCanvas";
import items from "../../assets/constants";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import { useEffect, useMemo, useState } from "react";
import GroupUI from "../GridComponents/GroupUI";
import useUserSettings from "../../hooks/useUserSettings";
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';
import useAccounts from "../../hooks/useAccounts";
import useSnack from "../../hooks/useSnack";
import KeyboardArrowLeftIcon from '@mui/icons-material/KeyboardArrowLeft';

function AccountView({ account }) {
    const [accountItemIds, setAccountItemIds] = useState([]);
    const [filteredAccountItemIds, setfilteredAccountItemIds] = useState([]);
    const [isRefreshingAccount, setIsRefreshingAccount] = useState(false);

    const { totalItems, filter, addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const { refreshData } = useAccounts();
    const collapsedFileds = useUserSettings().getByKeyAndSubKey('vaultPeeker', 'collapsedFileds');
    const { showSnackbar } = useSnack();

    const isDefaultCollapsed = !account ? false : collapsedFileds !== undefined ? collapsedFileds.accounts?.includes(account?.email) : false;

    useEffect(() => {
        if (account && account.account) {
            const itemIds = [];
            itemIds.push(...account.account.vault.itemIds);
            itemIds.push(...account.account.gifts.itemIds);
            itemIds.push(...account.account.material_storage.itemIds);
            itemIds.push(...account.account.temporary_gifts.itemIds);
            itemIds.push(...account.account.potions.itemIds);

            account.character.forEach((char) => {
                itemIds.push(...char.equipment);
            });
            const ids = [...new Set(itemIds)];
            setAccountItemIds(ids);
            setfilteredAccountItemIds(ids);
        }
    }, [account]);

    useEffect(() => {
        const id = `${account.email}_ACC`;
        addItemFilterCallback(id, (itemIds) => { setfilteredAccountItemIds(itemIds); }, accountItemIds);

        return () => {
            removeItemFilterCallback(id);
        };
    }, [accountItemIds]);

    const refreshAccountData = async (event) => {
        event.stopPropagation();

        if (!account || !account.email) return;
        setIsRefreshingAccount(true);

        const token = await refreshData(account.email);

        if (token) {
            showSnackbar("Refreshing finished");
        }

        setIsRefreshingAccount(false);
    };

    const boxTitle = useMemo(() => {
        return (
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 1, alignItems: 'center', justifyContent: 'space-between', width: '100%' }}>
                <Box sx={{ display: 'flex', flexDirection: 'row', gap: 1, alignItems: 'center' }}>
                    {account.group && <GroupUI group={account.group} />}
                    <Typography
                        variant="h6"
                        sx={{
                            fontWeight: 600,
                            textAlign: 'center',
                        }}
                    >
                        {account.name ? account.name : account.email}
                    </Typography>
                </Box>
                <Tooltip title="Refresh account">
                    <IconButton
                        disabled={isRefreshingAccount}
                        size="small"
                        onClick={refreshAccountData}
                    >
                        <RefreshOutlinedIcon />
                    </IconButton>
                </Tooltip>
            </Box>
        );
    }, [account, isRefreshingAccount]);

    if (!account || !filteredAccountItemIds || filteredAccountItemIds.length === 0) {
        return null;
    }

    return (
        <ComponentBox
            title={boxTitle}
            isCollapseable={true}
            defaultCollapsed={isDefaultCollapsed}
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
                    !(filter.characterType.enabled && filter.characterType.value === 3) &&
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
                    <StorageView vaultName={"vault"} canvasIdentifier={account.email + "_Vault"} title={<StorageViewTitle title="Vault" image="realm/vault_portal.png" sx={{ gap: 0.5 }} />} itemIds={account.account.vault.itemIds} totals={totalItems?.totals} />
                    {/* Gift Chest */}
                    <StorageView vaultName={"gift_chest"} canvasIdentifier={account.email + "_Gift"} title={<StorageViewTitle title="Gift Chest" image="realm/gift_chest.png" />} itemIds={account.account.gifts.itemIds} totals={totalItems?.totals} />
                    {/* Trade Chest */}
                    <StorageView vaultName={"trade_chest"} canvasIdentifier={account.email + "_Trade"} title={<StorageViewTitle title="Material Storage" image="realm/material_storage.png" />} itemIds={account.account.material_storage.itemIds} totals={totalItems?.totals} />
                    {/* Temporary Gifts */}
                    <StorageView vaultName={"temp_chest"} canvasIdentifier={account.email + "_Temp"} title={<StorageViewTitle title="Seasonal Spoils" image="realm/seasonal_spoils_chest.png" />} itemIds={account.account.temporary_gifts.itemIds} totals={totalItems?.totals} />
                    {/* Potion Storage */}
                    <StorageView vaultName={"potion_chest"} canvasIdentifier={account.email + "_Potion"} title={<StorageViewTitle title="Potion Storage" image="realm/potion_storage_small.png" />} itemIds={account.account.potions.itemIds} totals={totalItems?.totals} />
                </Box>
            }
        </ComponentBox>
    );
}

export default AccountView;

function StorageView({ vaultName, canvasIdentifier, title, itemIds, totals }) {
    const [filteredItemIds, setFilteredItemIds] = useState(itemIds);
    const { addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const settings = useUserSettings();
    const accSettings = settings.getByKeyAndSubKey('vaultPeeker', 'accountView');
    const [hideStorage, setHideStorage] = useState(false);

    useEffect(() => {
        const isHidden = accSettings?.hiddenVaults?.includes(vaultName);
        setHideStorage(isHidden);
    }, [vaultName]);

    useEffect(() => {
        setFilteredItemIds(itemIds);

        addItemFilterCallback(canvasIdentifier, (itemIds) => { setFilteredItemIds(itemIds); }, itemIds);
        return () => {
            removeItemFilterCallback(canvasIdentifier);
        };
    }, [itemIds]);

    const toggleStorage = () => {
        const isVisible = hideStorage;
        setHideStorage(!isVisible);

        if (isVisible) {
            const hiddenVaults = accSettings?.hiddenVaults?.filter((v) => v !== vaultName);
            settings.setByKeyAndSubKey('vaultPeeker', 'accountView', { ...accSettings, hiddenVaults: hiddenVaults });
            return;
        }

        settings.setByKeyAndSubKey('vaultPeeker', 'accountView', { ...accSettings, hiddenVaults: [...accSettings?.hiddenVaults, vaultName] });
    };

    if (!filteredItemIds || filteredItemIds.length === 0) return null;

    return (
        <Box
            sx={{
                mt: 1,
                display: 'flex',
                flexDirection: 'column',
                gap: 1,
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    gap: 1,
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    width: '100%',
                    cursor: 'pointer',
                }}
                onClick={() => {
                    toggleStorage();
                }}
            >
                <Box
                    sx={{
                        ...(hideStorage ? {
                            opacity: 0.5
                        } : {}),
                        transition: theme => theme.transitions.create('opacity'),
                    }}
                >
                    {title}
                </Box>
                <IconButton
                    sx={{
                        marginLeft: 'auto',
                        transition: 'transform 0.2s',
                        transform: hideStorage ? 'rotate(0deg)' : 'rotate(90deg)',
                    }}
                    size="small"
                >
                    <KeyboardArrowLeftIcon />
                </IconButton>
            </Box>
            <Collapse in={!hideStorage}>
                <ItemCanvas canvasIdentifier={canvasIdentifier} imgSrc="renders.png" itemIds={filteredItemIds} items={items} totals={totals} override={{ fillNumbers: false }} />
            </Collapse>
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