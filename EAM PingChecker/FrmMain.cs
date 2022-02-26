using ExaltAccountManager;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccountInfo = MK_EAM_Lib.AccountInfo;

namespace EAM_PingChecker
{
    public partial class FrmMain : Form
    {
        public readonly Version version = new Version(1, 1, 0);

        public bool useDarkmode = false;
        public static string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");
        public string serverCollectionPath = Path.Combine(saveFilePath, "EAM.ServerCollection");
        public string pingSaveFilePath = Path.Combine(saveFilePath, "EAM.PingSaveFile");
        public string serverFavoritesPath = Path.Combine(saveFilePath, "EAM.ServerFavorites");
        public string optionsPath = Path.Combine(saveFilePath, "EAM.options");
        public string accountsPath = Path.Combine(saveFilePath, "EAM.accounts");
        public string pathLogs = Path.Combine(saveFilePath, "EAM.Logs");
        public string accountStatsPath = Path.Combine(saveFilePath, "Stats");
        public string itemsSaveFilePath = Path.Combine(saveFilePath, "EAM.ItemsSaveFile");
        public ServerFavorites favorites;

        public PingSaveFile pingSaveFile;
        public ServerDataCollection serverData;
        public List<MK_EAM_Lib.AccountInfo> accounts = new List<MK_EAM_Lib.AccountInfo>();

        UIDashboard uiDashboard;
        UIOptions uiOptions;
        UIAbout uiAbout;

        public FrmMain()
        {
            InitializeComponent();

            LoadOptions();
            LoadAccountInfos(accountsPath);
            LoadPingSaveFile();
            LoadServerData();
            LoadServerFavorites();
            SwitchDesign(false);

            if (pingSaveFile.startupPing == StartupPing.Non)
            {
                lPingText.Text = $"Initializing...{Environment.NewLine}Please wait for it to finish.";
            }

            this.Show();
            Application.DoEvents(); //Ensure the form is shown before the pings start.
            bool hasAccount = (!string.IsNullOrEmpty(pingSaveFile.accountName) && !pingSaveFile.accountName.Equals("I don't need this feature"));
            if (serverData != null)
            {
                if (hasAccount && (DateTime.Now - serverData.collectionDate).TotalMinutes >= 30d && (pingSaveFile.serverDataOnStartup || pingSaveFile.refreshServerdata))
                {
                    timerRefreshData.Start();
                    timerRefreshData_Tick(null, null);
                }
                else if (hasAccount && pingSaveFile.refreshServerdata)
                {
                    int interval = (int)(1800 - (DateTime.Now - serverData.collectionDate).TotalSeconds) * 1000;

                    if (interval < 0)
                        timerRefreshData_Tick(null, null);
                    else
                        timerRefreshData.Interval = interval;
                }
            }

            uiDashboard = new UIDashboard(this);

            pMain.Controls.Remove(shadowLoading);
            pMain.Controls.Add(uiDashboard);

            shadowLoading.Dispose();
            shadowLoading = null;
        }

        private void LoadServerData()
        {
            try
            {
                if (File.Exists(serverCollectionPath))
                {
                    serverData = (ServerDataCollection)ByteArrayToObject(File.ReadAllBytes(serverCollectionPath));
                }
                else if((pingSaveFile.refreshServerdata || pingSaveFile.serverDataOnStartup) && (!string.IsNullOrEmpty(pingSaveFile.accountName) && !pingSaveFile.accountName.Equals("I don't need this feature")))
                    timerRefreshData_Tick(null, null);                
            }
            catch { }
        }

        public void LoadAccountInfos(string path)
        {
            try
            {

                try
                {
                    AccountSaveFile saveFile = (AccountSaveFile)ByteArrayToObject(File.ReadAllBytes(path));
                    accounts = AccountSaveFile.Decrypt(saveFile);
                }
                catch
                {
                    LogEvent(new LogData(-1, "EAM Ping", LogEventType.PingError, $"Failed to decrypt accounts."));
                    LogEvent(new LogData(-1, "EAM Ping", LogEventType.PingError, $"Failed to load AccountInfos."));
                }
            }
            catch (Exception e) { string ex = e.Message; }
        }


        private void LoadServerFavorites()
        {
            try
            {
                if (File.Exists(serverFavoritesPath))
                {
                    favorites = (ServerFavorites)ByteArrayToObject(File.ReadAllBytes(serverFavoritesPath));
                }
                else
                {
                    favorites = new ServerFavorites()
                    {
                        favorites = new List<string>()
                    };
                }
            }
            catch { }
        }

        private void LoadOptions()
        {
            if (File.Exists(optionsPath))
            {
                try
                {
                    OptionsData opt = (OptionsData)ByteArrayToObject(File.ReadAllBytes(optionsPath));
                    useDarkmode = opt.useDarkmode;
                }
                catch { }
            }
            else
            {
                try
                {
                    OptionsData opt = new OptionsData()
                    {
                        exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe"),
                        closeAfterConnection = false,
                        useDarkmode = !useDarkmode
                    };
                    File.WriteAllBytes(optionsPath, ObjectToByteArray(opt));
                }
                catch { }
            }
        }

        private void LoadPingSaveFile()
        {
            if (File.Exists(pingSaveFilePath))
            {
                try
                {
                    pingSaveFile = (PingSaveFile)ByteArrayToObject(File.ReadAllBytes(pingSaveFilePath));
                }
                catch { }
            }
            else
            {
                try
                {
                    pingSaveFile = new PingSaveFile();
                    File.WriteAllBytes(pingSaveFilePath, ObjectToByteArray(pingSaveFile));
                }
                catch { }
            }
        }

        public bool SavePingSaveFile()
        {
            try
            {
                if (pingSaveFile == null)
                    pingSaveFile = new PingSaveFile();
                File.WriteAllBytes(pingSaveFilePath, ObjectToByteArray(pingSaveFile));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SaveServerFavorites()
        {
            try
            {
                File.WriteAllBytes(serverFavoritesPath, ObjectToByteArray(favorites));
            }
            catch { }
        }

        public void SwitchDesign(bool performSwitch = true)
        {
            if (performSwitch)
                useDarkmode = !useDarkmode;

            Color def = ColorScheme.GetColorDef(useDarkmode);
            Color second = ColorScheme.GetColorSecond(useDarkmode);
            Color third = ColorScheme.GetColorThird(useDarkmode);
            Color font = ColorScheme.GetColorFont(useDarkmode);

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            ApplyTheme(useDarkmode, def, second, third, font);

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            this.BackColor = def;
            if (shadowLoading != null)
            {
                shadowLoading.BackColor =
                shadowLoading.PanelColor =
                shadowLoading.PanelColor2 = def;

                shadowLoading.ForeColor = font;

            }

            this.ForeColor = font;

            pTop.BackColor = pRight.BackColor = pbMinimize.BackColor = pbClose.BackColor = second;
            pSide.BackColor = second;
            pMain.BackColor = def;
            pSpacer.BackColor = third;

            if (isDarkmode)
            {
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;

                //Buttons
                btnDashboard.Image = Properties.Resources.ic_dashboard_white_24dp;
                btnOptions.Image = Properties.Resources.ic_settings_applications_white_24dp;
                btnAbout.Image = Properties.Resources.ic_info_outline_white_24dp;
            }
            else
            {
                pbMinimize.Image = Properties.Resources.baseline_minimize_black_24dp;
                pbClose.Image = Properties.Resources.ic_close_black_24dp;

                //Buttons
                btnDashboard.Image = Properties.Resources.ic_dashboard_black_24dp;
                btnOptions.Image = Properties.Resources.ic_settings_applications_black_24dp;
                btnAbout.Image = Properties.Resources.ic_info_outline_black_24dp;
            }

            if (uiDashboard != null) uiDashboard.ApplyTheme(isDarkmode, def, second, third, font);
            if (uiOptions != null) uiOptions.ApplyTheme(isDarkmode, def, second, third, font);
            if (uiAbout != null) uiAbout.ApplyTheme(isDarkmode, def, second, third, font);

            this.Update();
        }

        #region Button Close / Minimize

        private void pbMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        //private void pbClose_Click(object sender, EventArgs e) => Environment.Exit(0);
        private void pbClose_Click(object sender, EventArgs e) => Environment.Exit(0);

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_24dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = pRight.BackColor;
            if (!useDarkmode)
                pbClose.Image = Properties.Resources.ic_close_black_24dp;
        }

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            pbMinimize.BackColor = useDarkmode ? Color.FromArgb(128, 105, 105, 105) : Color.FromArgb(128, 169, 169, 169);
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e) => pbMinimize.BackColor = pRight.BackColor;

        #endregion

        private void btnSwitchDesign_Click(object sender, EventArgs e)
        {
            SwitchDesign();
        }

        public object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }

        public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if (pMain.Controls.Contains(uiDashboard))
                return;

            pSideBar.Location = new Point(0, btnDashboard.Top + 3);
            pMain.Controls.Clear();
            pMain.Controls.Add(uiDashboard);
            lTitle.Text = "Dashboard";
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            if (uiOptions != null && pMain.Controls.Contains(uiOptions))
                return;

            pSideBar.Location = new Point(0, btnOptions.Top + 3);
            pMain.Controls.Clear();

            if (uiOptions == null)
                uiOptions = new UIOptions(this);

            pMain.Controls.Add(uiOptions);
            lTitle.Text = "Options";
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            if (uiAbout != null && pMain.Controls.Contains(uiAbout))
                return;

            pSideBar.Location = new Point(0, btnAbout.Top + 3);

            pMain.Controls.Clear();

            if (uiAbout == null)
                uiAbout = new UIAbout(this);

            pMain.Controls.Add(uiAbout);
            lTitle.Text = "About";
        }

        public void LogEvent(LogData data)
        {
            if (File.Exists(pathLogs))
            {
                try
                {
                    List<LogData> logs = new List<LogData>();
                    logs = (List<LogData>)ByteArrayToObject(File.ReadAllBytes(pathLogs));
                    data.ID = logs.Count - 1;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
            else
            {
                try
                {
                    List<LogData> logs = new List<LogData>();
                    data.ID = 0;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
        }

        public void timerRefreshData_Tick(object sender, EventArgs e)
        {
            if (pingSaveFile.accountName.Equals("I don't need this feature"))
                return;
            try
            {
                LoadAccountInfos(accountsPath);
                AccountInfo acc = accounts.Where(a => a.name.Equals(pingSaveFile.accountName)).First();
                if (acc != null)
                {
                    //acc = GetAccountData(acc);
                    acc.PerformWebrequest(this, LogEvent, "Ping Checker", accountStatsPath, itemsSaveFilePath, GetDeviceUniqueIdentifier(), string.IsNullOrEmpty(acc.Name), true, SaveAccounts);
                }
                else
                {
                    //Account not found!
                    LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Couldn't find the auto-refresh account: {pingSaveFile.accountName}."));
                }
            }
            catch { }            
        }

        public void SaveAccounts(AccountInfo _info = null)
        {
            try
            {
                if (_info.requestState != AccountInfo.RequestState.Success)
                    return;
                AccountSaveFile saveFile = AccountSaveFile.Encrypt(new AccountSaveFile(), ObjectToByteArray(accounts.ToList() as List<MK_EAM_Lib.AccountInfo>));
                if (saveFile != null && string.IsNullOrEmpty(saveFile.error))
                {
                    File.WriteAllBytes(accountsPath, ObjectToByteArray(saveFile));
                    LogEvent(new LogData(-1, "Ping Checker", LogEventType.SaveAccounts, $"Saving accounts."));
                }
                else
                {
                    LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to encrypt accounts!"));                    
                    throw new Exception();
                }
            }
            catch
            {
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to save accounts!"));                
            }

            UpdateServerUI();
        }

        private bool UpdateServerUI()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)UpdateServerUI);

            if (uiDashboard != null)
                uiDashboard.UpdateServer();

            if (!pingSaveFile.refreshServerdata)
            {
                timerRefreshData.Stop();
            }
            else
            {
                timerRefreshData.Stop();
                timerRefreshData.Interval = 1800000;
                timerRefreshData.Start();
            }
            return false;
        }

        public MK_EAM_Lib.AccountInfo GetAccountData(MK_EAM_Lib.AccountInfo info)
        {
            try
            {
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.WebRequest, $"Sending \"account/verify\" for {info.email}."));
                string uniqueID = GetDeviceUniqueIdentifier();

                string webPath = string.Format("https://www.realmofthemadgod.com/account/verify?guid={0}&password={1}&clientToken={2}&game_net=Unity&play_platform=Unity&game_net_user_id=", WebUtility.UrlEncode(info.email), WebUtility.UrlEncode(info.password), uniqueID);
                string responseData = string.Empty;

                WebRequest request = WebRequest.Create(webPath);
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseData = reader.ReadToEnd();
                }
                response.Close();

                if (responseData.Contains("<Error>"))
                {
                    //Error out
                    info.requestState = MK_EAM_Lib.AccountInfo.RequestState.Error;
                    return info;
                }
                else
                    info.requestState = MK_EAM_Lib.AccountInfo.RequestState.Success;

                Tuple<string, string, string> tup = GetClientAccessData(responseData);

                info.accessToken = new MK_EAM_Lib.AccessToken(tup.Item1, tup.Item2, tup.Item3, uniqueID);
                try
                {
                    string link = $"https://www.realmofthemadgod.com/char/list?do_login=true&accessToken={WebUtility.UrlEncode(info.accessToken.token)}&game_net=Unity&play_platform=Unity&game_net_user_id=";
                    request = WebRequest.Create(link);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    response = request.GetResponse();
                    string charList = "";
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        charList = reader.ReadToEnd();
                    }
                    response.Close();
                    SaveAccountStats(info, responseData, charList);
                }
                catch
                {
                    LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to get stats for {info.email}."));
                }
            }
            catch
            {
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Webrequest for {info.email} failed."));
            }
            return info;
        }

        public Tuple<string, string, string> GetClientAccessData(string resp)
        {
            try
            {
                string token = "";
                string timestamp = "";
                string expiration = "";

                if (resp.Contains("<AccessToken>"))
                {
                    token = resp.Substring(resp.IndexOf("<AccessToken>") + 13, resp.Length - (resp.IndexOf("<AccessToken>") + 13));
                    token = token.Substring(0, token.IndexOf("</AccessToken>"));
                }
                if (resp.Contains("<AccessTokenTimestamp>"))
                {
                    timestamp = resp.Substring(resp.IndexOf("<AccessTokenTimestamp>") + 22, resp.Length - (resp.IndexOf("<AccessTokenTimestamp>") + 22));
                    timestamp = timestamp.Substring(0, timestamp.IndexOf("</AccessTokenTimestamp>"));
                }
                if (resp.Contains("<AccessTokenExpiration>"))
                {
                    expiration = resp.Substring(resp.IndexOf("<AccessTokenExpiration>") + 23, resp.Length - (resp.IndexOf("<AccessTokenExpiration>") + 23));
                    expiration = expiration.Substring(0, expiration.IndexOf("</AccessTokenExpiration>"));
                }

                return new Tuple<string, string, string>(token, timestamp, expiration);
            }
            catch
            {
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to parse ClientAccessData!"));
                //snackbar.Show(this, "Failed to parse ClientAccessData!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }

            return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
        }

        private void SaveAccountStats(MK_EAM_Lib.AccountInfo info, string request, string charList)
        {
            try
            {
                if (!Directory.Exists(accountStatsPath))
                    Directory.CreateDirectory(accountStatsPath);

                string fileName = GetAccountStatsFilename(info.email);

                if (string.IsNullOrEmpty(fileName))
                {
                    LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to save stats for {info.email}."));
                    return;
                }

                AccountStats stats = new AccountStats(info.email, request);
                CharListStats s = GetCharacterStatsFromRequest(charList);

                StatsMain main = new StatsMain();
                string path = Path.Combine(accountStatsPath, fileName);
                if (File.Exists(path))
                {
                    try
                    {
                        main = (StatsMain)ByteArrayToObject(File.ReadAllBytes(path));
                    }
                    catch { main.email = info.email; }
                }
                else
                {
                    main.email = info.email;
                }

                main.stats.Add(stats);
                main.charList.Add(s);

                File.WriteAllBytes(path, ObjectToByteArray(main));
            }
            catch
            {
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to save stats for {info.email}."));
            }
        }

        private CharListStats GetCharacterStatsFromRequest(string charList)
        {
            CharListStats s = new CharListStats();
            try
            {
                serverData = ServerDataCollection.CreateNewCollection(charList);
                File.WriteAllBytes(serverCollectionPath, ObjectToByteArray(serverData));
            }
            catch (Exception)
            {
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to parse / save servers."));
            }

            try
            {
                string[] chars = System.Text.RegularExpressions.Regex.Split(charList, ("</Char>"));
                if (chars.Length > 0)
                    chars[0] = chars[0].Substring(chars[0].IndexOf("<Char id=\""), chars[0].Length - chars[0].IndexOf("<Char id=\""));

                for (int i = 0; i < chars.Length - 1; i++)
                {
                    CharacterStats st = new CharacterStats(chars[i]);
                    s.chars.Add(st);
                }
            }
            catch
            {
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to parse CharList."));
            }
            return s;
        }

        private string GetAccountStatsFilename(string email)
        {
            try
            {
                string fileName = email;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    fileName = fileName.Replace(c.ToString(), "");
                fileName = fileName.Replace(".", "");
                fileName = $"{fileName}.EAMstats";

                return fileName;
            }
            catch { }
            return string.Empty;
        }

        public string GetDeviceUniqueIdentifier()
        {
            string ret = string.Empty;

            string concatStr = string.Empty;
            try
            {
                using (ManagementObjectSearcher searcherBb = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
                {
                    foreach (var obj in searcherBb.Get())
                    {
                        concatStr += (string)obj.Properties["SerialNumber"].Value ?? string.Empty;
                    }
                }
                using (ManagementObjectSearcher searcherBios = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS"))
                {
                    foreach (var obj in searcherBios.Get())
                    {
                        concatStr += (string)obj.Properties["SerialNumber"].Value ?? string.Empty;
                    }
                }
                using (ManagementObjectSearcher searcherOs = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcherOs.Get())
                    {
                        concatStr += (string)obj.Properties["SerialNumber"].Value ?? string.Empty;
                    }
                }
                using (var sha1 = SHA1.Create())
                {
                    ret = string.Join("", sha1.ComputeHash(Encoding.UTF8.GetBytes(concatStr)).Select(b => b.ToString("x2")));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LogEvent(new LogData(-1, "Ping Checker", LogEventType.PingError, $"Failed to get the DeviceUniqueIdentifier!"));
                //snackbar.Show(this, "Failed to get the DeviceUniqueIdentifier!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }

            return ret;
        }
    }
}
