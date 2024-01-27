import { useSnackbar } from "notistack";

function useSnack() {
    const snackbar = useSnackbar();

    const showSnackbar = (message, variant = 'default') => {
        snackbar.enqueueSnackbar(message, { variant: variant, persist: false });
    };

    return { showSnackbar };
}

export default useSnack;