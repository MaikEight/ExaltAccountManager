import { useSnackbar } from "notistack";

function useSnack() {
    const snackbar = useSnackbar();

    const showSnackbar = (message, variant = 'default', persist = false) => {
        return snackbar.enqueueSnackbar(message, { variant: variant, persist: persist });
    };

    const closeSnackbar = (key) => {
        snackbar.closeSnackbar(key);
    };

    return { showSnackbar, closeSnackbar };
}

export default useSnack;