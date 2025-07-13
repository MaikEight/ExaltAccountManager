import { useSnackbar } from "notistack";

function useSnack() {
    const snackbar = useSnackbar();

    const showSnackbar = (message, variant = 'default', persist = false) => {
        snackbar.enqueueSnackbar(message, { variant: variant, persist: persist });
    };

    return { showSnackbar };
}

export default useSnack;