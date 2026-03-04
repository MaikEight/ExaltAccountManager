import { useState, useMemo, useCallback, useEffect } from "react";
import { Box, Typography, Chip, Avatar, AvatarGroup, Skeleton } from "@mui/material";
import { useTheme } from "@emotion/react";
import ComponentBox from "../../ComponentBox";
import CharacterGridV2 from "./CharacterGridV2";
import StorageContainerV2 from "./StorageContainerV2";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import { portrait } from "../../../utils/portraitUtils";
import useAccounts from "../../../hooks/useAccounts";
import EamIconButton from "../../EamIconButton";
import MenuOpenRoundedIcon from '@mui/icons-material/MenuOpenRounded';

/**
 * AccountViewV2 - Displays a single account's characters and storage
 * Used inside AccountsViewV2's Virtuoso list
 * 
 * @param {Object} props
 * @param {Object} props.accountData - Processed account data from VaultPeekerContext
 * @param {Function} props.onToggleCollapse - Callback when account is collapsed/expanded
 * @param {boolean} props.collapsed - Whether this account is collapsed
 */
function AccountViewV2({ accountData, onToggleCollapse, collapsed = false }) {
    const theme = useTheme();
    const { filter } = useVaultPeeker();
    const [characterPortraits, setCharacterPortraits] = useState([]);
    const { getAccountByEmail, loadAccountByEmail, setSelectedAccount } = useAccounts();

    // Extract data from accountData (comes from processAccountsData in context)
    const { email, name, group, characters, storageContainers, account } = accountData || {};

    // Count total items
    const totalItems = useMemo(() => {
        let count = 0;
        // Count character items
        characters?.forEach((char) => {
            if (char.equipment) count += char.equipment.filter((i) => i?.item_id !== -1).length;
            if (char.inventory) count += char.inventory.filter((i) => i?.item_id !== -1).length;
            if (char.backpacks) {
                char.backpacks.forEach((item) => {
                    if (item?.item_id !== -1) count++;
                });
            }
        });
        // Count storage items
        if (storageContainers) {
            Object.values(storageContainers).forEach((container) => {
                count += container.filter((i) => i?.item_id !== -1).length;
            });
        }
        return count;
    }, [characters, storageContainers]);

    const openWidgetBarWithAccount = useCallback(() => {
        let fullAccountData = getAccountByEmail(email);
        if (!fullAccountData) {
            console.warn(`Account with email ${email} not found in context.`);
            fullAccountData = loadAccountByEmail(email, true); // Force load if not found, as the getAccountByEmail would return null a second time..
            if (!fullAccountData) {
                console.error(`Failed to load account with email ${email}.`);
                return;
            }
        }
        setSelectedAccount(fullAccountData);
    }, [email, getAccountByEmail, loadAccountByEmail, setSelectedAccount]);

    // Generate character portraits using the portrait utility (like v1)
    useEffect(() => {
        if (!characters || characters.length === 0) {
            setCharacterPortraits([]);
            return;
        }

        const generatePortraits = async () => {
            const chars = [];
            // Max 4 characters for collapsed view
            for (let i = 0; i < characters.length && i < 4; i++) {
                const char = characters[i];
                chars.push({
                    type: char.char_class,
                    skin: char.texture,
                    tex1: char.tex1,
                    tex2: char.tex2,
                    adjust: false,
                    seasonal: char.seasonal,
                });
            }

            try {
                const portraits = await Promise.all(
                    chars.map((char) => portrait(char.type, char.skin, char.tex1, char.tex2, char.adjust))
                );
                // Pad with nulls if less than 4
                while (portraits.length < 4) {
                    portraits.push(null);
                }
                setCharacterPortraits(portraits.map((p, i) => ({
                    src: p,
                    seasonal: chars[i]?.seasonal || false,
                })));
            } catch (error) {
                console.error('Error generating portraits:', error);
                setCharacterPortraits([]);
            }
        };

        generatePortraits();
    }, [characters]);

    const handleHeaderClick = useCallback(() => {
        onToggleCollapse?.(!collapsed);
    }, [collapsed, onToggleCollapse]);

    if (!accountData) return null;

    const getAccountHeader = useMemo(() => {
        return (
            <Box
                onClick={handleHeaderClick}
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: 1,
                    p: 1,
                    cursor: 'pointer',
                    borderRadius: 1,
                    width: '100%',
                    '&:hover': {
                        backgroundColor: theme.palette.action.hover,
                    },
                }}
            >
                {/* Group Color Indicator */}
                {group && (
                    <Box
                        sx={{
                            width: 10,
                            height: 10,
                            borderRadius: '50%',
                            backgroundColor: group.color || theme.palette.primary.main,
                        }}
                    />
                )}

                {/* Account Name */}
                <Typography variant="subtitle1" fontWeight="bold" sx={{}}>
                    {name || email || 'Unknown Account'}
                </Typography>



                {/* Open WidgetBar Button */}
                <EamIconButton
                    icon={<MenuOpenRoundedIcon fontSize="medium" sx={{ ml: 0.75 }} />}
                    tooltip={'Open account details'}
                    tooltipDirection="right"
                    tooltipBackground="background.paperLight"
                    aria-label="Open Account Details"
                    onClick={(e) => {
                        e.stopPropagation();
                        openWidgetBarWithAccount();
                    }}
                />
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        gap: 0.5,
                        justifyContent: 'center',
                        mx: 'auto',
                    }}
                >
                    {/* Stats Chips */}
                    <Chip
                        label={`${characters?.length || 0} chars`}
                        size="small"
                        variant="outlined"
                        sx={{ fontSize: '0.75rem' }}
                    />
                    <Chip
                        label={`${totalItems} items`}
                        size="small"
                        variant="outlined"
                        sx={{ fontSize: '0.75rem' }}
                    />
                </Box>

                {/* Character Portraits */}
                {characterPortraits.length > 0 && (
                    <Box sx={{ display: 'flex', gap: 0.5, ml: 'auto', flexWrap: 'wrap' }}>
                        {characters?.length === 1 ? (
                            characterPortraits[0]?.src ? (
                                <Avatar
                                    src={characterPortraits[0].src}
                                    sx={{
                                        width: 34,
                                        height: 34,
                                        border: characterPortraits[0].seasonal
                                            ? `2px solid ${theme.palette.warning.main}`
                                            : 'none',
                                    }}
                                    variant="square"
                                />
                            ) : (
                                <Skeleton variant="rounded" width={34} height={34} />
                            )
                        ) : (
                            <AvatarGroup
                                total={characters?.length || 0}
                                max={5}
                                spacing="medium"
                                renderSurplus={(surplus) => (
                                    <Avatar
                                        key="surplus-avatar"
                                        sx={{
                                            backgroundColor: theme.palette.background.default,
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
                                        backgroundColor: theme.palette.background.paper,
                                        border: 'none',
                                    },
                                }}
                            >
                                {characterPortraits.map((portraitData, index) => {
                                    if (!portraitData?.src) {
                                        return (
                                            <Box key={`skeleton-${index}`}>
                                                <Skeleton variant="rounded" width={34} height={34} />
                                            </Box>
                                        );
                                    }
                                    return (
                                        <Avatar
                                            key={`avatar-${index}`}
                                            alt={`Character ${index + 1}`}
                                            src={portraitData.src}
                                            sx={{
                                                width: 34,
                                                height: 34,
                                                border: portraitData.seasonal
                                                    ? `2px solid ${theme.palette.warning.main}`
                                                    : 'none',
                                            }}
                                            variant="square"
                                        />
                                    );
                                })}
                            </AvatarGroup>
                        )}
                    </Box>
                )}
            </Box>
        );
    }, [name, email, characterPortraits, characters, totalItems, group, theme, handleHeaderClick, openWidgetBarWithAccount]);

    return (
        <ComponentBox
            sx={{
                width: '100%',
                mb: 0,
                mx: 0, // Override default margin
            }}
            title={getAccountHeader}
            isCollapseable
        >
            <Box sx={{ p: 1, pt: 0, }}>
                {/* Characters Grid */}
                {characters?.length > 0 && (
                    <Box sx={{ mb: 2 }}>
                        <CharacterGridV2 characters={characters} email={email} />
                    </Box>
                )}

                {/* Storage Containers */}
                {storageContainers && (
                    <Box
                        sx={{
                            display: 'grid',
                            gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
                            gap: 1,
                        }}
                    >
                        <StorageContainerV2
                            storageType="vault"
                            items={storageContainers.vault || []}
                            previewCount={16}
                        />
                        <StorageContainerV2
                            storageType="gifts"
                            items={storageContainers.gifts || []}
                            previewCount={16}
                        />
                        <StorageContainerV2
                            storageType="material_storage"
                            items={storageContainers.material_storage || []}
                            previewCount={16}
                        />
                        <StorageContainerV2
                            storageType="temporary_gifts"
                            items={storageContainers.temporary_gifts || []}
                            previewCount={16}
                        />
                        <StorageContainerV2
                            storageType="potions"
                            items={storageContainers.potions || []}
                            previewCount={16}
                        />
                    </Box>
                )}
            </Box>
        </ComponentBox >
    );
}

export default AccountViewV2;
