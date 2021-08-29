namespace MK_EAM_Lib
{
    [System.Serializable]
    public class ExportSaveFile
    {
        public int version;
        public ExportType exportType;
        public byte[] data;
    }

    [System.Serializable]
    public class ExportAccounts
    {
        public System.Collections.Generic.List<AccountInfo> accounts;
        public AccountOrders accountOrders;
        public System.Collections.Generic.List<string> statsFileName;
        public System.Collections.Generic.List<byte[]> statsFileData;
    }

    [System.Serializable]
    public class ExportAll
    {
        public ExportAccounts exportAccounts;
        public NotificationOptions notificationOptions;
        public byte[] options;
        public DailyLogins dailyLogins;
        public PingSaveFile pingSaveFile;
    }

    public class ExportCSVAccount
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string color { get; set; }
        public int orderID { get; set; }

        public ExportCSVAccount() { }
        public ExportCSVAccount(AccountInfo info, int _orderID = -1, bool savePassword = false)
        {
            username = info.name;
            email = info.email;
            password = savePassword ? info.password : string.Empty;
            color = "#" + info.color.R.ToString("X2") + info.color.G.ToString("X2") + info.color.B.ToString("X2");
            orderID = _orderID;
        }
    }

    public enum ExportType
    {
        AccountsWithPassword,
        AccountsNoPassword,
        CompleteSaveFileWithPassword
    }
}
