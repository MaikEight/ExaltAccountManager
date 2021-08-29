namespace MK_EAM_Lib
{
    [System.Serializable]
    public class AccountInfo
    {
        public string name;
        public string email;
        public string password;

        public string customClientID = string.Empty;
        public string serverName = string.Empty;

        public System.Drawing.Color color = System.Drawing.Color.FromArgb(50, 128, 128, 128);

        public bool performSave;

        public AccessToken accessToken;

        public bool requestSuccessfull = true;

        public AccountInfo() { }
        public AccountInfo(MuledumpAccounts muledump)
        {
            name = muledump.mail;
            email = muledump.mail;
            password = muledump.password;
            performSave = false;
        }

        public AccountInfo(ExportCSVAccount exp) 
        {
            name = exp.username;
            password = exp.password;
            email = exp.email;
            color = System.Drawing.ColorTranslator.FromHtml(exp.color);
        }
    }
}
