using CsvHelper.Configuration;
using System.ComponentModel;
using System.Globalization;

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
        [System.Obsolete] public AccountOrders accountOrders;
        public System.Collections.Generic.List<string> statsFileName;
        public System.Collections.Generic.List<byte[]> statsFileData;
    }

    [System.Serializable]
    public class ExportAll
    {
        public ExportAccounts exportAccounts;
        public NotificationOptions notificationOptions;
        public byte[] options;
        public DailyLoginsOLD dailyLogins;
        public PingSaveFile pingSaveFile;
    }

    public class ExportCSVAccount
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string color { get; set; }
        [Browsable(false)]
        public int orderID { get; set; }

        public ExportCSVAccount() { }
        public ExportCSVAccount(AccountInfo info, bool savePassword = false, int _orderID = -1)
        {
            username = info.name;
            email = info.email;
            password = savePassword ? info.password : string.Empty;
            color = "#" + info.Color.R.ToString("X2") + info.Color.G.ToString("X2") + info.Color.B.ToString("X2");
            orderID = _orderID;
        }
    }

    public sealed class ExportCSVAccountMap : ClassMap<ExportCSVAccount>
    {
        public ExportCSVAccountMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.orderID).Ignore();
        }
    }

    public enum ExportType
    {
        AccountsWithPassword,
        AccountsNoPassword,
        CompleteSaveFileWithPassword
    }
}
