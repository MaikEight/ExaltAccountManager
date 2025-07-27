import KeyOffOutlinedIcon from '@mui/icons-material/KeyOffOutlined';
import HourglassEmptyOutlinedIcon from '@mui/icons-material/HourglassEmptyOutlined';
import ImageSearchOutlinedIcon from '@mui/icons-material/ImageSearchOutlined';
import GavelOutlinedIcon from '@mui/icons-material/GavelOutlined';
import LockPersonOutlinedIcon from '@mui/icons-material/LockPersonOutlined';
import ErrorOutlineOutlinedIcon from '@mui/icons-material/ErrorOutlineOutlined';
import CheckCircleOutlinedIcon from '@mui/icons-material/CheckCircleOutlined';
import VpnLockOutlinedIcon from '@mui/icons-material/VpnLockOutlined';
import SyncProblemOutlinedIcon from '@mui/icons-material/SyncProblemOutlined';

function getRequestState(charList) {
    if (!charList) {
        console.warn('charList is undefined or null');
        return "Error";
    }
    const hasErrors = charList?.Error !== undefined;

    if (hasErrors) {
        console.info('charList has error', charList.Error);
        const error = charList.Error?.toLowerCase();

        if (error.includes("passworderror")) {
            return "WrongPassword";
        } else if (error.includes("wait") || error.includes("try again later")) {
            return "TooManyRequests";
        } else if (error.includes("captchalock")) {
            return "Captcha";
        } else if (error.includes("suspended")) {
            return "AccountSuspended";
        } else if (error.includes("account in use")) {
            return "AccountInUse";
        } else {
            return "Error";
        }
    }

    return "Success";
}

function requestStateToMessage(requestState) {
    if (!requestState) {
        return null;
    }

    switch (requestState) {
        case "WrongPassword":
            return "Wrong password";
        case "TooManyRequests":
            return "Too many requests. Try again later.";
        case "Captcha":
            return "Captcha lock";
        case "AccountSuspended":
            return "Account suspended";
        case "AccountInUse":
            return "Account in use";
        case "Error":
            return "Error";
        case "Success":
            return "Success";
        case "RateLimitExceeded": //This is a EAM Rate limit error
            return "Rate limit exceeded";
        case "BGSyncError": //This is a background sync error
            return "Background sync error";
        default:
            return "Unknown Error";
    }
}

function requestStateToShortName(requestState) {
    if (!requestState) {
        return null;
    }

    switch (requestState) {
        case "WrongPassword":
            return "W. Pass";
        case "TooManyRequests":
            return "Cooldown";
        case "Captcha":
            return "Captcha";
        case "AccountSuspended":
            return "Banned";
        case "AccountInUse":
            return "In use";
        case "Error":
            return "Error";
        case "Success":
            return "Success";
        case "RateLimitExceeded": //This is a EAM Rate limit error
            return "Rate Limit";
        case "BGSyncError": //This is a background sync error
            return "BG Sync";
        default:
            return "Error";
    }
}

function requestStateToIcon(requestState, props) {
    switch (requestState) {
        case "WrongPassword":
            return (<KeyOffOutlinedIcon {...props} />);
        case "TooManyRequests":
            return (<HourglassEmptyOutlinedIcon {...props} />);
        case "Captcha":
            return (<ImageSearchOutlinedIcon {...props} />);
        case "AccountSuspended":
            return (<GavelOutlinedIcon {...props} />);
        case "AccountInUse":
            return (<LockPersonOutlinedIcon {...props} />);
        case "Error":
            return (<ErrorOutlineOutlinedIcon {...props} />);
        case "Success":
            return (<CheckCircleOutlinedIcon {...props} />);
        case "RateLimitExceeded":
            return (<VpnLockOutlinedIcon {...props} />);
        case "BGSyncError":
            return (<SyncProblemOutlinedIcon {...props} />);
        default:
            return (<ErrorOutlineOutlinedIcon {...props} />); // Default to error icon for unknown states
    }
}

function requestStateToColor(requestState) {
    switch (requestState) {
        case "WrongPassword":
            return "error";
        case "TooManyRequests":
            return "warning";
        case "Captcha":
            return "warning";
        case "AccountSuspended":
            return "error";
        case "AccountInUse":
            return "warning";
        case "Error":
            return "error";
        case "Success":
            return "primary";
        case "RateLimitExceeded":
            return "warning";
        case "BGSyncError":
            return "warning";
        default:
            return "error"; // Default to error color for unknown states
    }
}

export { getRequestState, requestStateToMessage, requestStateToIcon, requestStateToColor, requestStateToShortName };