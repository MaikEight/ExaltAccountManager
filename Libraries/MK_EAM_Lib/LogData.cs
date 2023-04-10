using System.ComponentModel;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class LogData
    {
        [Browsable(false)]
        public int ID;
        
        [Browsable(false)]
        public System.DateTime time;

        [DisplayName("Date")]
        public string Date { get => time.ToString("dd.MM.yyyy"); }

        [DisplayName("Time")]
        public string Time { get => time.ToString("HH:mm"); }

        [DisplayName("Sender")]
        public string sender { get; set; } = string.Empty;

        [Browsable(false)]
        public LogEventType eventType;

        [DisplayName("EventType")]
        public string EventType { get => eventType.ToString(); }

        [DisplayName("LogEntry")]
        public string message { get; set; } = string.Empty;

        public LogData() { }

        public LogData(int _ID, string _sender, LogEventType _eventType, string _message)
        {
            ID = _ID;
            sender = _sender;
            eventType = _eventType;
            message = _message;
            time = System.DateTime.Now;
        }

        public LogData(string _sender, LogEventType _eventType, string _message)
        {
            ID = -1;
            sender = _sender;
            eventType = _eventType;
            message = _message;
            time = System.DateTime.Now;
        }
    }

    public class CSVLogData
    {
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Entry { get; set; } = string.Empty;

        public CSVLogData() { }
        public CSVLogData(LogData data) 
        {
            Date = data.Date;
            Time = data.Time;
            Sender = data.sender;
            EventType = data.EventType;
            Entry = data.message;
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
        SaveUIOrder,
        AccountInfo,
        PingError,
        PingInfo,
        GameUpdate,
        ServiceInfo,
        ServiceEnd,
        APIError,
        UpdateEAM,
    }
}
