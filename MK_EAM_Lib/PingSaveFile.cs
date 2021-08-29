namespace MK_EAM_Lib
{
    [System.Serializable]
    public class PingSaveFile
    {
        public StartupPing startupPing;
        public bool serverDataOnStartup;
        public string accountName;
        public bool refreshServerdata;

        public PingSaveFile()
        {
            startupPing = StartupPing.All;
            serverDataOnStartup = false;
            accountName = string.Empty;
            refreshServerdata = false;
        }
    }

    public enum StartupPing
    {
        All,
        Favorites,
        Non
    }
}
