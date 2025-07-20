import { Avatar, AvatarGroup, Box, Collapse, IconButton, Paper, Skeleton, Tooltip, Typography, useTheme } from "@mui/material";
import ComponentBox from "../ComponentBox";
import Character from "../Realm/Character";
import ItemCanvas from "../Realm/ItemCanvas";
import items from "../../assets/constants";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import { useEffect, useMemo, useState } from "react";
import useUserSettings from "../../hooks/useUserSettings";
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';
import useAccounts from "../../hooks/useAccounts";
import useSnack from "../../hooks/useSnack";
import KeyboardArrowLeftIcon from '@mui/icons-material/KeyboardArrowLeft';
import { portrait } from "../../utils/portraitUtils";
import { GroupUI } from "../GridComponents/GroupUI";

function AccountView({ account }) {
    const [accountItemIds, setAccountItemIds] = useState([]);
    const [filteredAccountItemIds, setfilteredAccountItemIds] = useState([]);
    const [isRefreshingAccount, setIsRefreshingAccount] = useState(false);
    const [characterPortraits, setCharacterPortraits] = useState([-1, -1, -1, -1]);
    const [overrideTotals, setOverrideTotals] = useState({});

    const { totalItems, filter, addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const { refreshData } = useAccounts();
    const collapsedFileds = useUserSettings().getByKeyAndSubKey('vaultPeeker', 'collapsedFileds');
    const { showSnackbar } = useSnack();

    const isDefaultCollapsed = !account ? false : collapsedFileds !== undefined ? collapsedFileds.accounts?.includes(account?.email) : false;

    const extractOverrideTotals = () => {
        const ot = {};

        if (account && account.account) {
            const acc = account.account;
            const vault = acc.vault;
            const gifts = acc.gifts;
            const material_storage = acc.material_storage;
            const temporary_gifts = acc.temporary_gifts;
            const potions = acc.potions;

            const calculateSpace = (totals) => {
                const space = { free: 0, used: 0, total: 0 };
                if (totals) {
                    space.free = totals['-1']?.amount || 0;
                    space.total = Object.values(totals).reduce((acc, item) => {
                        if (item.id !== -1) {
                            return acc + item.amount;
                        }
                        return acc;
                    }, 0);
                    space.used = space.total - space.free;
                }

                return space;
            };

            if (vault.totals) {
                ot.vault = { totals: {}, space: { free: 0, used: 0, total: 0 } };
                Object.keys(vault.totals).forEach((key) => {
                    if (vault.totals[key] > 0) {
                        ot.vault.totals[key] = { amount: vault.totals[key] };
                    }
                });
                ot.vault.space = calculateSpace(ot.vault.totals);
            }

            if (gifts.totals) {
                ot.gifts = { totals: {} };
                Object.keys(gifts.totals).forEach((key) => {
                    if (gifts.totals[key] > 0) {
                        ot.gifts.totals[key] = { amount: gifts.totals[key] };
                    }
                });
                ot.gifts.space = calculateSpace(ot.gifts.totals);
            }

            if (material_storage.totals) {
                ot.material_storage = { totals: {} };
                Object.keys(material_storage.totals).forEach((key) => {
                    if (material_storage.totals[key] > 0) {
                        ot.material_storage.totals[key] = { amount: material_storage.totals[key] };
                    }
                });
                ot.material_storage.space = calculateSpace(ot.material_storage.totals);
            }

            if (temporary_gifts.totals) {
                ot.temporary_gifts = { totals: {} };
                Object.keys(temporary_gifts.totals).forEach((key) => {
                    if (temporary_gifts.totals[key] > 0) {
                        ot.temporary_gifts.totals[key] = { amount: temporary_gifts.totals[key] };
                    }
                });
                ot.temporary_gifts.space = calculateSpace(ot.temporary_gifts.totals);
            }

            if (potions.totals) {
                ot.potions = { totals: {} };
                Object.keys(potions.totals).forEach((key) => {
                    if (potions.totals[key] > 0) {
                        ot.potions.totals[key] = { amount: potions.totals[key] };
                    }
                });
                ot.potions.space = calculateSpace(ot.potions.totals);
            }
            return ot;
        }
        return null;
    }

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
            setOverrideTotals(extractOverrideTotals());

            const generatePortraits = async () => {
                const chars = [];
                //max 4 characters
                for (let i = 0; i < account.character.length && i < 4; i++) {
                    const char = account.character[i];
                    chars.push({
                        type: char.class,
                        skin: char.texture,
                        tex1: char.tex1,
                        tex2: char.tex2,
                        adjust: false,
                    });
                }

                const portraits = await Promise.all(chars.map((char) => portrait(char.type, char.skin, char.tex1, char.tex2, char.adjust)));
                if (portraits.length < 4) {
                    for (let i = portraits.length; i < 4; i++) {
                        portraits.push(null);
                    }
                }
                setCharacterPortraits(portraits);
            }
            generatePortraits();
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
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 1,
                        alignItems: 'center',
                        justifyContent: 'center',
                        height: 34,
                    }}
                >
                    {
                        account && account.character && account.character.length > 0 &&
                            account.character.length === 1 ?
                            characterPortraits?.[0] && characterPortraits[0] !== -1 ?
                                <Avatar
                                    src={characterPortraits?.[0] ?? null}
                                    sx={{ width: 34, height: 34 }}
                                    variant="square"
                                />
                                :
                                <Skeleton variant="rounded" width={34} height={34} />
                            :
                            <AvatarGroup
                                total={account.character.length}
                                max={5}
                                spacing="medium"
                                renderSurplus={(surplus) => (
                                    <Avatar
                                        key="surplus-avatar"
                                        sx={{
                                            backgroundColor: (theme) => theme.palette.background.default,
                                            pl: 0.35,
                                        }}
                                        variant="circular"
                                    >
                                        <Typography variant="subtitle2" sx={{ color: 'text.secondary' }}>
                                            +{surplus}
                                        </Typography>
                                    </Avatar>
                                )}
                                sx={{
                                    height: 34,
                                    '& .MuiAvatarGroup-avatar': {
                                        width: 34,
                                        height: 34,
                                        backgroundColor: (theme) => theme.palette.background.paper,
                                        border: 'none',
                                    },
                                }}
                            >
                                {characterPortraits.map((portrait, index) => {
                                    if (portrait === -1) {
                                        return (
                                            <Box key={`skeleton-${index}`}>
                                                <Skeleton variant="rounded" width={34} height={34} />
                                            </Box>
                                        );
                                    }
                                    if (portrait) {
                                        return (
                                            <Avatar
                                                key={`avatar-${index}`}
                                                alt={`Character ${index + 1}`}
                                                src={portrait}
                                                sx={{ width: 34, height: 34 }}
                                                variant="square"
                                            />
                                        );
                                    }
                                    return null;
                                })}
                            </AvatarGroup>
                    }
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
            </Box>
        );
    }, [account, isRefreshingAccount, characterPortraits]);

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
                    <StorageView vaultName={"vault"} canvasIdentifier={account.email + "_Vault"} title={<StorageViewTitle title="Vault" image="realm/vault_portal.png" sx={{ gap: 0.5 }} />} itemIds={account.account.vault.itemIds} totals={totalItems?.totals} overrideTotals={overrideTotals.vault} />
                    {/* Gift Chest */}
                    <StorageView vaultName={"gift_chest"} canvasIdentifier={account.email + "_Gift"} title={<StorageViewTitle title="Gift Chest" image="realm/gift_chest.png" />} itemIds={account.account.gifts.itemIds} totals={totalItems?.totals} overrideTotals={{ ...overrideTotals.gifts, hideUsed: true }} />
                    {/* Trade Chest */}
                    <StorageView vaultName={"trade_chest"} canvasIdentifier={account.email + "_Trade"} title={<StorageViewTitle title="Material Storage" image="realm/material_storage.png" />} itemIds={account.account.material_storage.itemIds} totals={totalItems?.totals} overrideTotals={overrideTotals.material_storage} />
                    {/* Temporary Gifts */}
                    <StorageView vaultName={"temp_chest"} canvasIdentifier={account.email + "_Temp"} title={<StorageViewTitle title="Seasonal Spoils" image="realm/seasonal_spoils_chest.png" />} itemIds={account.account.temporary_gifts.itemIds} totals={totalItems?.totals} overrideTotals={{ ...overrideTotals.temporary_gifts, hideUsed: true }} />
                    {/* Potion Storage */}
                    <StorageView vaultName={"potion_chest"} canvasIdentifier={account.email + "_Potion"} title={<StorageViewTitle title="Potion Storage" image="realm/potion_storage_small.png" />} itemIds={account.account.potions.itemIds} totals={totalItems?.totals} overrideTotals={overrideTotals.potions} />
                </Box>
            }
        </ComponentBox>
    );
}

export default AccountView;

function StorageView({ vaultName, canvasIdentifier, title, itemIds, totals, overrideTotals }) {
    const [filteredItemIds, setFilteredItemIds] = useState(itemIds);
    const { addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const settings = useUserSettings();
    const accSettings = settings.getByKeyAndSubKey('vaultPeeker', 'accountView');
    const [hideStorage, setHideStorage] = useState(false);
    const theme = useTheme();

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
            const hiddenVaults = accSettings?.hiddenVaults?.filter((v) => v !== vaultName) ?? [];
            settings.setByKeyAndSubKey('vaultPeeker', 'accountView', { ...accSettings, hiddenVaults: hiddenVaults });
            return;
        }

        settings.setByKeyAndSubKey('vaultPeeker', 'accountView', { ...accSettings, hiddenVaults: [...(accSettings?.hiddenVaults ?? null), vaultName].filter((v) => v !== null) });
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
                        transition: theme.transitions.create('opacity'),
                    }}
                >
                    {title}
                </Box>
                <Box>
                    {
                        overrideTotals?.space &&
                        <Tooltip
                            slots={{
                                tooltip: Paper,
                            }}
                            slotProps={{
                                tooltip: {
                                    sx: {
                                        p: 0.25,
                                        borderRadius: `${theme.shape.borderRadius}px`,
                                        backgroundColor: theme.palette.background.paperLight,
                                        height: 'fit-content',
                                        width: 'fit-content',
                                    },
                                }
                            }}
                            title={
                                <Box
                                    sx={{
                                        m: 0,
                                        p: 1,
                                        px: 1.5,
                                        display: 'flex',
                                        flexDirection: 'column',
                                        gap: 0.5,
                                        alignItems: 'center',
                                        backgroundColor: theme.palette.background.default,
                                        borderRadius: `${theme.shape.borderRadius - 2}px`,
                                        height: '100%',
                                        width: '100%',
                                    }}
                                >
                                    <Typography variant="body1">
                                        Storage Space
                                    </Typography>
                                    <Typography variant="body2">
                                        Free: {overrideTotals?.space?.free}
                                    </Typography>
                                    <Typography variant="body2">
                                        Used: {overrideTotals?.space?.used}
                                    </Typography>
                                    <Typography variant="body2">
                                        Total: {overrideTotals?.space?.total}
                                    </Typography>
                                </Box>
                            }
                        >
                            <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                                {
                                    overrideTotals?.hideUsed ?
                                        `${overrideTotals?.space?.total}`
                                        :
                                        `${overrideTotals?.space?.used} / ${overrideTotals?.space?.total}`
                                }
                            </Typography>
                        </Tooltip>
                    }
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
            </Box>
            <Collapse in={!hideStorage}>
                <ItemCanvas
                    canvasIdentifier={canvasIdentifier}
                    imgSrc="renders.png"
                    itemIds={filteredItemIds}
                    items={items}
                    totals={totals}
                    overrideTotals={overrideTotals.totals}
                    override={{ fillNumbers: true }}
                    overrideItemImages={
                        {
                            '-1': {
                                imgSrc: theme.palette.mode === "dark" ? 'realm/itemSlot.png' : 'realm/itemSlot_light.png',
                                size: 50,
                                padding: 0,
                            }
                        }
                    }
                />
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