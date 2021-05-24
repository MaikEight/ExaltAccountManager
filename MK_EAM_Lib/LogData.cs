namespace MK_EAM_Lib
{
    [System.Serializable]
    public class LogData
    {
        public int ID;
        public System.DateTime time;
        public string sender;
        public LogEventType eventType;

        public string message;

        public LogData() { }

        public LogData(int _ID, string _sender, LogEventType _eventType, string _message) 
        {
            ID = _ID;
            sender = _sender;
            eventType = _eventType;
            message = _message;
            time = System.DateTime.Now;
        }
    }

    public enum LogEventType
    {
        Error,
        AddAccount,
        RemoveAccount,
        EditAccount,
        Login,
        WebRequest,
        EAMError,
        ServiceStart,
        ServiceDone,
        ServiceError,
        SaveOptions,
        SaveNotify,
        SaveAccounts,
        StopExalt,
        GetTaskData,
        InstallTask,
        UpdateTask,
        RemoveTask,
        TaskTime,
        Stats,
        StatsError,
        NoLogs,
        LogsError,
        SaveUIOrder
    }
}
