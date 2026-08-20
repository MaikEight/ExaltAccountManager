export const getVaultItemSlotImage = (theme) => (
    theme.palette.vaultPeeker?.itemSlotImage
    ?? (theme.palette.mode === 'dark'
        ? '/realm/itemSlot.png'
        : '/realm/itemSlot_light.png')
);
