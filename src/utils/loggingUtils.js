import { invoke } from '@tauri-apps/api/tauri';

async function logToAuditLog(sender, message, accountEmail = null) {
    if(!sender || !message) return;

    const logData = {
        id: null,
        time: "",
        sender: sender,
        message: message,
        accountEmail: accountEmail
    }

    console.info(`Logging to audit log: ${JSON.stringify(logData)}`);
    await invoke('log_to_audit_log', { log: logData });
}

async function getAuditLog() {
    return await invoke('get_all_audit_logs');
}

async function getAuditLogForAccount(accountEmail) {    
    return await invoke('get_audit_log_for_account', { accountEmail: accountEmail });
}

async function logToErrorLog(sender, message) {
    if(!sender || !message) return;

    const logData = {
        id: null,
        time: "",
        sender: sender,
        message: message
    }

    console.info(`Logging to error log: ${JSON.stringify(logData)}`);
    await invoke('log_to_error_log', { log: logData });
}

async function getErrorLog() {
    return await invoke('get_all_error_logs');
}

export { logToAuditLog, getAuditLog, getAuditLogForAccount, logToErrorLog, getErrorLog};