import { invoke } from '@tauri-apps/api/tauri';

async function logToAuditLog(sender, message, accountEmail = null) {
    if(!sender || !message) return;

    const logData = {
        id: null,
        time: "",
        sender: "" + sender,
        message: "" + message,
        accountEmail: accountEmail ? "" + accountEmail : null
    }

    console.info(`Logging to audit log: ${JSON.stringify(logData)}`);
    await invoke('log_to_audit_log', { log: logData })
        .catch((err) => {
            console.error('logToAuditLog', err);
        });
}

async function getAuditLog() {
    return await invoke('get_all_audit_logs')
        .catch((err) => {
            console.error('getAuditLog', err);
        });
}

async function getAuditLogForAccount(accountEmail) {    
    return await invoke('get_audit_log_for_account', { accountEmail: accountEmail })
        .catch((err) => {
            console.error('getAuditLogForAccount', err);
        });
}

async function logToErrorLog(sender, message) {
        if(!sender || !message) return;

    const logData = {
        id: null,
        time: "",
        sender: "" + sender,
        message: "" + message
    }

    await invoke('log_to_error_log', { log: logData })
        .catch((err) => {
            console.error('logToErrorLog', err);
        });
}

async function getErrorLog() {
    return await invoke('get_all_error_logs')
        .catch((err) => {
            console.error('getErrorLog', err);
        });
}

export { logToAuditLog, getAuditLog, getAuditLogForAccount, logToErrorLog, getErrorLog};