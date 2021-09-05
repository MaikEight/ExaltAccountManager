using Bunifu.UI.WinForms;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using AccountInfo = MK_EAM_Lib.AccountInfo;

namespace ExaltAccountManager
{
    public partial class FrmMain : Form
    {
        public readonly Version version = new Version(2, 2, 3);
        bool openMoreUI = true;

        #region UIs

        OptionsUI optionenUI;
        MoreToolsUI moreUI = null;
        SortAlphabeticalUI sortAlphabeticalUI;
        public ColorChanger colorChanger;
        public FrmServerListChanger frmServerListChanger;

        //THE HWID Change does not work, sadly...
        //public FrmHWIDchanger frmHWIDchanger;

        #endregion

        public List<MK_EAM_Lib.AccountInfo> accounts = new List<MK_EAM_Lib.AccountInfo>();
        public List<AccountUI> accountUIs = new List<AccountUI>();
        public bool closeAfterConnect = false;
        public bool useDarkmode = false;
        public string serverToJoin = string.Empty;

        public bool[] notificationValues = new bool[] { true, true, true, true };

        #region Paths

        public string exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe");

        public static string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        public string optionsPath = Path.Combine(saveFilePath, "EAM.options");
        public string accountsPath = Path.Combine(saveFilePath, "EAM.accounts");
        public string accountOrdersPath = Path.Combine(saveFilePath, "EAM.accountOrders");
        public string dailyLoginsPath = Path.Combine(saveFilePath, "EAM.DailyLogins");
        public string notificationOptionsPath = Path.Combine(saveFilePath, "EAM.NotificationOptions");
        public string serverCollectionPath = Path.Combine(saveFilePath, "EAM.ServerCollection");
        public string accountStatsPath = Path.Combine(saveFilePath, "Stats");
        public string pathLogs = Path.Combine(Application.StartupPath, "EAM.Logs");
        public string lastUpdateCheckPath = Path.Combine(saveFilePath, "EAM.LastUpdateCheck");
        public string lastNotificationCheckPath = Path.Combine(saveFilePath, "EAM.LastNotificationCheck");
        public string forceHWIDFilePath = Path.Combine(saveFilePath, "EAM.HWID");
        public string pingCheckerExePath = Path.Combine(Application.StartupPath, "EAM PingChecker.exe");
        public string getClientHWIDToolPath = Path.Combine(Application.StartupPath, "EAM_GetClientHWID");

        private string[] flagPaths = new string[]
        {
             Path.Combine(Application.StartupPath, "flag.ScreenshotMode"),
             Path.Combine(Application.StartupPath, "flag.MPGH")
        };

        #endregion

        #region Flags

        public bool screenshotMode = false;
        public bool isMPGHVersion = false;

        #endregion

        public NotificationOptions notOpt;

        public bool loading = false;
        public List<LogData> logs = new List<LogData>();
        public AccountOrders accountOrders;
        public bool lockForm = false;
        public bool isNewInstall = false;
        public ServerDataCollection serverData;

        public string checksumJson = string.Empty;
        public string buildHash = string.Empty;
        public bool updateRequired = false;

        public EAMNotificationMessage msg;
        private string updateLink = string.Empty;
        private EAMNotificationMessageSaveFile notificationSaveFile;

        public FrmMain()
        {
            InitializeComponent();

            lVersion.Text = $"v{version}";
            toolTip.AlignTextWithTitle = true;

            moreUI = new MoreToolsUI(this)
            {
                Visible = false
            };
            this.Controls.Add(moreUI);
            header.SetFrmMain(this);

            sortAlphabeticalUI = new SortAlphabeticalUI(this);
            sortAlphabeticalUI.Visible = false;
            this.Controls.Add(sortAlphabeticalUI);
            sortAlphabeticalUI.Top = header.Bottom + 15;

            pMain.HorizontalScroll.Visible = false;
            pMain.VerticalScroll.Visible = false;
            pMain.AutoScroll = false;

            try
            {
                optionenUI = new OptionsUI(this);
                this.Controls.Add(optionenUI);
                optionenUI.Dock = DockStyle.Top;
                optionenUI.BringToFront();
                header.BringToFront();
                pMain.BringToFront();
                scrollbar.BringToFront();

                #region Load Flags

                for (int i = 0; i < flagPaths.Length; i++)
                {
                    try
                    {
                        if (File.Exists(flagPaths[i]))
                        {
                            switch (i)
                            {
                                case 0: //Screenshot Mode
                                    screenshotMode = true;
                                    break;
                                case 1: //isMPGH release
                                    isMPGHVersion = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch { }
                }

                #endregion

                if (!Directory.Exists(saveFilePath))
                {
                    Directory.CreateDirectory(saveFilePath);
                    isNewInstall = true;
                }

                if (isNewInstall || (!File.Exists(accountsPath) && !File.Exists(optionsPath)))
                {
                    isNewInstall = true;

                    SwitchDesign();

                    this.Show();
                    this.BringToFront();

                    lockForm = true;

                    FrmSetup frmSetup = new FrmSetup(this);
                    frmSetup.StartPosition = FormStartPosition.Manual;
                    frmSetup.Location = new Point(this.Location.X + ((this.Width - frmSetup.Width) / 2), this.Location.Y + ((this.Height - frmSetup.Height) / 2));
                    frmSetup.Show(this);
                }

                if (!isNewInstall)
                {
                    if (File.Exists(optionsPath))
                    {
                        try
                        {
                            OptionsData opt = (OptionsData)ByteArrayToObject(File.ReadAllBytes(optionsPath));
                            exePath = opt.exePath;
                            closeAfterConnect = opt.closeAfterConnection;
                            useDarkmode = !opt.useDarkmode;
                            serverToJoin = opt.serverToJoin;

                            notificationValues[0] = opt.searchRotmgUpdates;
                            notificationValues[1] = opt.searchUpdateNotification;
                            notificationValues[2] = opt.searchWarnings;
                            notificationValues[3] = opt.deactivateKillswitch;
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            OptionsData opt = new OptionsData()
                            {
                                exePath = exePath,
                                closeAfterConnection = false,
                                useDarkmode = !useDarkmode,
                                serverToJoin = string.Empty,
                                searchRotmgUpdates = notificationValues[0],
                                searchUpdateNotification = notificationValues[1],
                                searchWarnings = notificationValues[2],
                                deactivateKillswitch = notificationValues[3]
                            };
                            File.WriteAllBytes(optionsPath, ObjectToByteArray(opt));
                        }
                        catch { }
                    }
                    if (File.Exists(accountOrdersPath))
                    {
                        try
                        {
                            accountOrders = (AccountOrders)ByteArrayToObject(File.ReadAllBytes(accountOrdersPath));
                        }
                        catch { }
                    }

                    if (File.Exists(accountsPath))
                    {
                        LoadAccountInfos(accountsPath);
                    }
                    if (File.Exists(notificationOptionsPath))
                    {
                        try
                        {
                            notOpt = (NotificationOptions)ByteArrayToObject(File.ReadAllBytes(notificationOptionsPath));
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            notOpt = new NotificationOptions();
                            File.WriteAllBytes(notificationOptionsPath, ObjectToByteArray(notOpt));
                        }
                        catch { }
                    }

                    try
                    {
                        if (!Directory.Exists(accountStatsPath))
                            Directory.CreateDirectory(accountStatsPath);
                    }
                    catch { }
                }

                LoadServerData();

                LoadLogs();
                if (!isNewInstall)
                    SwitchDesign();
                pbDarkmode_MouseLeave(pbDarkmode, null);

                timerLoadProcesses_Tick(timerLoadProcesses, null);
                timerLoadProcesses.Start();

                this.BringToFront();

                colorChanger = new ColorChanger(this);
                colorChanger.Visible = false;
                pMain.Controls.Add(colorChanger);

                frmServerListChanger = new FrmServerListChanger(this);
                //frmHWIDchanger = new FrmHWIDchanger(this);

                Application.DoEvents();

                bool checkForNotification = true;
                notificationSaveFile = new EAMNotificationMessageSaveFile();
                if (File.Exists(lastNotificationCheckPath))
                {
                    try
                    {
                        notificationSaveFile = ByteArrayToObject(File.ReadAllBytes(lastNotificationCheckPath)) as EAMNotificationMessageSaveFile;
                    }
                    catch
                    {
                        notificationSaveFile = new EAMNotificationMessageSaveFile();
                    }
                }

                checkForNotification = notificationSaveFile.forceCheck || DateTime.Now.Date > notificationSaveFile.lastCheck.Date || notificationSaveFile.lastCheckWasStop;

                if ((notificationValues[1] || notificationValues[2] || notificationValues[3]) && checkForNotification)
                {
                    if (notificationSaveFile.lastCheckWasStop)
                    {
                        lockForm = true;
                        pMain.Enabled = false;
                    }

                    try
                    {
                        msg = EAMNotificationMessage.GetEAMNotificationMessage($"{version.Major}_{version.Minor}_{version.Build}");

                        timerShowMessage.Start();
                    }
                    catch
                    {
                        LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to get / save notification message"));
                        optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                    }
                }

                if (notificationValues[0])
                {
                    bool checkForUpdate = false;
                    if (File.Exists(lastUpdateCheckPath))
                    {
                        try
                        {
                            string[] rows = File.ReadAllLines(lastUpdateCheckPath);
                            DateTime date = new DateTime(long.Parse(rows[0]));
                            bool updateRequired = Convert.ToBoolean(rows[1]);

                            if (updateRequired)
                                checkForUpdate = true;
                            else if (date.Date.AddDays(1) < DateTime.Now)
                            {
                                checkForUpdate = true;
                            }
                        }
                        catch
                        {
                            checkForUpdate = true;
                        }
                    }
                    else
                        checkForUpdate = true;

                    if (checkForUpdate)
                    {
                        FrmUpdater updCheck = new FrmUpdater(this, true);
                    }
                }
            }
            catch (Exception ex)
            {
                loading = false;

                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed while loading! {ex.StackTrace}"));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
            }
        }

        public bool ShowGameUpdateRequired(bool required)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool, bool>)ShowGameUpdateRequired, required);

            lUpdateNeeded.Visible = required;

            return false;
        }

        public void SetAccountOrders(AccountOrders o) => accountOrders = o;

        public void LoadAccountInfos(string path, bool performSaveAfter = false)
        {
            try
            {
                loading = true;

                try
                {
                    AccountSaveFile saveFile = (AccountSaveFile)ByteArrayToObject(File.ReadAllBytes(path));
                    accounts = AccountSaveFile.Decrypt(saveFile);
                }
                catch
                {
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to decrypt accounts."));

#pragma warning disable CS0612 

                    byte[] data = File.ReadAllBytes(path);
                    AesCryptographyService acs = new AesCryptographyService();
                    List<ExaltAccountManager.AccountInfo> accs = (List<ExaltAccountManager.AccountInfo>)ByteArrayToObject(acs.Decrypt(data));
                    accounts = new List<MK_EAM_Lib.AccountInfo>();
                    for (int i = 0; i < accs.Count; i++)
                        accounts.Add(ExaltAccountManager.AccountInfo.Convert(accs[i]));

#pragma warning restore CS0612 

                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.AccountInfo, $"Found an old save file, converting it."));
                    if (!performSaveAfter)
                        File.Delete(path);
                    SaveAccounts();
                }

                UpdateAccountInfos();

                loading = false;

                if (performSaveAfter)
                    SaveAccounts();
            }
            catch (Exception e) { string ex = e.Message; }
        }

        private void LoadServerData()
        {
            try
            {
                if (File.Exists(serverCollectionPath))
                {
                    serverData = (ServerDataCollection)ByteArrayToObject(File.ReadAllBytes(serverCollectionPath));
                }
            }
            catch { }
        }

        public void ShowColorChangerUI(AccountUI ui, bool hideOnly = false)
        {
            if (colorChanger.ui == ui || hideOnly)
            {
                colorChanger.ui = null;
                colorChanger.Visible = false;
                return;
            }

            colorChanger.ShowUI(ui);
            colorChanger.Location = new Point(ui.Location.X + 56, ui.Location.Y);
            if (colorChanger.Top > pMain.Height - colorChanger.Height)
                colorChanger.Top = pMain.Height - colorChanger.Height;
            pMain.Controls.SetChildIndex(colorChanger, 0);
            colorChanger.Visible = true;
        }

        public void ShowServerListUI(AccountUI ui, int x = -1, int y = -1)
        {
            if (lockForm && (x == -1 || y == -1)) return;

            if (frmServerListChanger == null)
            {
                frmServerListChanger = new FrmServerListChanger(this);
            }

            lockForm = true;
            frmServerListChanger.ShowUI(ui);
            if (ui != null)
                frmServerListChanger.Location = new Point(this.Location.X + ui.Location.X + 186, this.Location.Y + ui.Location.Y + ui.Height + pMain.Top);
            else if (x > -1 || y > -1)
                frmServerListChanger.Location = new Point(x + ((250 - frmServerListChanger.Width) / 2), y + ((345 - frmServerListChanger.Height) / 2));
            else
                frmServerListChanger.Location = new Point(this.Location.X + ((this.Width - frmServerListChanger.Width) / 2), this.Location.Y + ((this.Height - frmServerListChanger.Height) / 2));

            if (frmServerListChanger.Top > (this.Bottom - frmServerListChanger.Height) - 2)
                frmServerListChanger.Top = (this.Bottom - frmServerListChanger.Height) - 2;

            frmServerListChanger.ShowDialog();
        }

        //public void ShowHWIDUI(AccountUI ui)
        //{
        //    if (lockForm) return;

        //    lockForm = true;
        //    frmHWIDchanger.ShowUI(ui);
        //    if (ui != null)
        //        frmHWIDchanger.Location = new Point(this.Location.X + ui.Location.X + 75, this.Location.Y + ui.Location.Y + ui.Height + pMain.Top);
        //    else
        //        frmHWIDchanger.Location = new Point(this.Location.X + ((this.Width - frmHWIDchanger.Width) / 2), this.Location.Y + ((this.Height - frmHWIDchanger.Height) / 2));

        //    if (frmHWIDchanger.Top > (this.Bottom - frmHWIDchanger.Height) - 2)
        //        frmHWIDchanger.Top = (this.Bottom - frmHWIDchanger.Height) - 2;

        //    frmHWIDchanger.ShowDialog();
        //}

        public void ShowMoreUI(bool hideOnly = false, bool dontHideSort = false)
        {
            if (hideOnly)
            {
                if (colorChanger.Visible)
                {
                    colorChanger.ui = null;
                    colorChanger.Visible = false;
                }

                if (sortAlphabeticalUI.Visible && !dontHideSort)
                    sortAlphabeticalUI.Visible = false;
            }

            if (openMoreUI)
            {
                if (hideOnly) return;

                this.Controls.SetChildIndex(moreUI, 0);

                openMoreUI = true;
                ////top -> down
                //moreUI.Location = new Point((this.Width - moreUI.Width) - 3, optionenUI.Bottom - moreUI.Height);
                //Right -> left
                moreUI.Location = new Point(this.Width, optionenUI.Bottom);

                moreUI.Visible = true;
                timerMoreToolsUI.Start();
            }
            else
            {
                //Hide
                //moreUI.Visible = false;
                openMoreUI = false;
                timerMoreToolsUI.Start();
            }

            if (colorChanger.Visible)
            {
                colorChanger.ui = null;
                colorChanger.Visible = false;
            }

            if (sortAlphabeticalUI.Visible && !dontHideSort)
                sortAlphabeticalUI.Visible = false;
        }

        private void LoadLogs()
        {
            if (File.Exists(pathLogs))
            {
                try
                {
                    logs = (List<LogData>)ByteArrayToObject(File.ReadAllBytes(pathLogs));
                }
                catch { }
            }
        }

        public void LogEvent(LogData data)
        {
            if (File.Exists(pathLogs))
            {
                try
                {
                    logs = (List<LogData>)ByteArrayToObject(File.ReadAllBytes(pathLogs));
                    if (logs == null)
                        logs = new List<LogData>();
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
                    if (logs == null)
                        logs = new List<LogData>();
                    data.ID = 0;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
        }

        public MK_EAM_Lib.AccountInfo GetAccountData(MK_EAM_Lib.AccountInfo info, bool getName = true)
        {
            try
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.WebRequest, $"Sending \"account/verify\" for {info.email}."));
                string uniqueID = GetDeviceUniqueIdentifier();
                //uniqueID = (string.IsNullOrEmpty(info.customClientID) || info.customClientID.Equals(uniqueID)) ? uniqueID : info.customClientID;

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
                    info.requestSuccessfull = false;
                    return info;
                }
                else
                    info.requestSuccessfull = true;

                if (getName && responseData.Contains("<Account>"))
                {
                    info.name = responseData.Substring(responseData.IndexOf("<Name>") + 6, responseData.IndexOf("</Name>") - 6 - responseData.IndexOf("<Name>"));
                }

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
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to get stats for {info.email}."));
                    optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                }
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Webrequest for {info.email} failed."));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
            }
            return info;
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

        private CharListStats GetCharacterStatsFromRequest(string charList)
        {
            CharListStats s = new CharListStats();
            try
            {
                serverData = ServerDataCollection.CreateNewCollection(charList);
                File.WriteAllBytes(serverCollectionPath, ObjectToByteArray(serverData));
                if (frmServerListChanger != null)
                    frmServerListChanger.LoadServers();
            }
            catch (Exception)
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to parse / save servers."));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
            }

            try
            {
                string[] chars = Regex.Split(charList, ("</Char>"));
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
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to parse CharList."));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
            }
            return s;
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
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to save stats for {info.email}."));
                    optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
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
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to save stats for {info.email}."));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
            }
        }

        private void pbDarkmode_Click(object sender, EventArgs e)
        {
            ShowMoreUI(true);
            if (lockForm) return;

            SwitchDesign();
            OptionsData opt = new OptionsData()
            {
                exePath = exePath,
                closeAfterConnection = false,
                useDarkmode = useDarkmode,
                serverToJoin = string.Empty,
                searchRotmgUpdates = notificationValues[0],
                searchUpdateNotification = notificationValues[1],
                searchWarnings = notificationValues[2],
                deactivateKillswitch = notificationValues[3]
            };
            SaveOptions(opt);
        }

        public void SaveOptions(OptionsData opt, bool showSnackbarNotification = false)
        {
            try
            {
                if (logs.Count == 0 || logs[logs.Count - 1].eventType != LogEventType.SaveOptions || (logs[logs.Count - 1].eventType == LogEventType.SaveOptions && logs[logs.Count - 1].time < DateTime.Now.AddMinutes(-1)))
                    LogEvent(new LogData(logs.Count + 1, "EAM Options", LogEventType.SaveOptions, $"Saving new options."));
                byte[] data = ObjectToByteArray(opt);
                File.WriteAllBytes(optionsPath, data);

                if (showSnackbarNotification)
                    snackbar.Show(this, $"Options saved successfully!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);

                if (msg != null && msg.type == EAMNotificationMessageType.UpdateAvailable)
                {
                    lUpdateAvailable.Visible = true;
                    lVersion.ForeColor = useDarkmode ? Color.Orange : Color.DarkOrange;
                    notificationSaveFile.forceCheck = true;
                    updateLink = isMPGHVersion ? msg.linkM : msg.link;
                }
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM Options", LogEventType.EAMError, $"Failed to save options!"));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                snackbar.Show(this, $"Failed to save options!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }
        }

        public void SaveTimingData(int joinTime, int killTime)
        {
            if (notOpt != null)
            {
                notOpt.joinTime = joinTime;
                notOpt.killTime = killTime;
            }
            else
                notOpt = new NotificationOptions() { joinTime = joinTime, killTime = killTime };

            SaveNotificationoptions(notOpt);
        }

        public void SaveTimingData(DateTime execTime)
        {
            if (notOpt != null)
            {
                notOpt.execTime = execTime;
            }
            else
                notOpt = new NotificationOptions() { execTime = execTime };

            SaveNotificationoptions(notOpt);
        }

        public void SwitchDesign()
        {
            useDarkmode = !useDarkmode;
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (useDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            ApplyTheme(useDarkmode, def, second, third, font);
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            FormsUtils.SuspendDrawing(this);

            this.ForeColor = font;
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = Color.FromArgb(225, font.R, font.G, font.B);

            header.BackColor = second;
            optionenUI.ApplyTheme(third, font);
            moreUI.ApplyTheme(isDarkmode);
            sortAlphabeticalUI.ApplyTheme(isDarkmode);
            pMain.BackColor =
            pTop.BackColor =
            lUpdateNeeded.BackColor =
            lUpdateAvailable.BackColor =
            pBox.BackColor = def;
            lDev.ForeColor = font;
            pbDarkmode.BackColor = pBox.BackColor;
            header.ApplyTheme(isDarkmode);

            toolTip.BackColor = def;

            //pbDarkmode.SizeMode = (isDarkmode) ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.CenterImage;
            BunifuSnackbar.CustomizationOptions opt;

            if (isDarkmode)
            {
                pbDarkmode.Image = Properties.Resources.ic_brightness_5_white_48dp;
                pbLogo.Image = Properties.Resources.ic_account_balance_wallet_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;

                toolTip.SetToolTipIcon(pbDarkmode, pbDarkmode.Image);
                toolTip.SetToolTipTitle(pbDarkmode, "Switch to daymode");
                toolTip.SetToolTip(pbDarkmode, $"Burn your eyes!");

                opt = new BunifuSnackbar.CustomizationOptions()
                {
                    ActionBackColor = Color.FromArgb(8, 8, 8),
                    BackColor = Color.FromArgb(8, 8, 8),
                    ActionBorderColor = Color.FromArgb(15, 15, 15),
                    BorderColor = Color.FromArgb(15, 15, 15),
                    ActionForeColor = Color.FromArgb(170, 170, 170),
                    ForeColor = Color.FromArgb(170, 170, 170),
                    CloseIconColor = Color.FromArgb(246, 255, 237)
                };

                scrollbar.BorderColor = second;
                scrollbar.BackgroundColor = def;
                scrollbar.ThumbColor = third;

                lUpdateNeeded.ForeColor = lUpdateAvailable.ForeColor = Color.Orange;
            }
            else
            {
                pbDarkmode.Image = Properties.Resources.ic_brightness_4_black_48dp;
                pbLogo.Image = Properties.Resources.ic_account_balance_wallet_black_48dp;
                pbClose.Image = Properties.Resources.ic_close_black_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_black_24dp;

                toolTip.SetToolTipIcon(pbDarkmode, pbDarkmode.Image);
                toolTip.SetToolTipTitle(pbDarkmode, "Switch to darkmode");
                toolTip.SetToolTip(pbDarkmode, $"Come to the dark side, we have cookies!");

                opt = new BunifuSnackbar.CustomizationOptions()
                {
                    ActionBackColor = Color.White,
                    BackColor = Color.White,
                    ActionBorderColor = Color.White,
                    BorderColor = Color.White,
                    ActionForeColor = Color.Black,
                    ForeColor = Color.Black,
                    CloseIconColor = Color.FromArgb(246, 255, 237)
                };

                scrollbar.BorderColor = Color.Silver;
                scrollbar.BackgroundColor = def;
                scrollbar.ThumbColor = Color.Gray;

                lUpdateNeeded.ForeColor = lUpdateAvailable.ForeColor = Color.DarkOrange;
            }

            if (updateRequired)
                lVersion.ForeColor = useDarkmode ? Color.Orange : Color.DarkOrange;

            snackbar.ErrorOptions = opt;
            snackbar.ErrorOptions.CloseIconColor = Color.FromArgb(255, 204, 199);
            snackbar.WarningOptions = opt;
            snackbar.WarningOptions.CloseIconColor = Color.FromArgb(255, 229, 143);
            snackbar.InformationOptions = opt;
            snackbar.InformationOptions.CloseIconColor = Color.FromArgb(145, 213, 255);
            snackbar.SuccessOptions = opt;
            snackbar.SuccessOptions.CloseIconColor = Color.FromArgb(246, 255, 237);

            foreach (AccountUI ui in accountUIs)
                ui.ApplyTheme(useDarkmode, def, second, third, font);

            if (colorChanger != null)
                colorChanger.ApplyTheme();
            if (frmServerListChanger != null)
                frmServerListChanger.ApplyTheme();
            //if (frmHWIDchanger != null)
            //    frmHWIDchanger.ApplyTheme();

            FormsUtils.ResumeDrawing(this);

            this.Update();

            pbDarkmode_MouseEnter(pbDarkmode, null);
        }

        public void AddAccountToOrders(string mail)
        {
            int nextID = 0;
            if (accountOrders == null)
            {
                accountOrders = new AccountOrders();
            }
            else if (accountOrders.orderData == null)
                accountOrders.orderData = new List<OrderData>();
            else if (accountOrders.orderData.Count > 0)
                nextID = accountOrders.orderData.Max(o => o.index) + 1;

            accountOrders.orderData.Add(new OrderData() { email = mail, index = nextID });
            SaveAccountOrders();
        }

        public void HideDragHandles(AccountUI ui = null)
        {
            for (int i = 0; i < accountUIs.Count; i++)
            {
                if (ui != null && ui == accountUIs[i])
                    continue;

                accountUIs[i].HideDragHandle();
            }
        }

        public void RemoveAccountFromOrders(string mail)
        {
            try
            {
                OrderData data = accountOrders.orderData.Where(o => o.email.Equals(mail)).First();
                if (data != null)
                {
                    accountOrders.orderData.Remove(data);
                    for (int i = 0; i < accountOrders.orderData.Count; i++)
                    {
                        if (accountOrders.orderData[i].index > data.index)
                            accountOrders.orderData[i].index--;
                    }

                    if (uiPoints.Count > 0)
                        uiPoints.RemoveAt(uiPoints.Count - 1);
                    OrderUIs();
                }
            }
            catch { }
            SaveAccountOrders();
        }

        public void SaveAccountOrders()
        {
            try
            {
                File.WriteAllBytes(accountOrdersPath, ObjectToByteArray(accountOrders));

                if (logs.Count == 0 || logs[logs.Count - 1].eventType != LogEventType.SaveUIOrder || (logs[logs.Count - 1].eventType == LogEventType.SaveUIOrder && logs[logs.Count - 1].time < DateTime.Now.AddMinutes(-1)))
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.SaveUIOrder, $"Saving account-orders."));
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to save new account-orders!"));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                snackbar.Show(this, $"Failed to save new account-orders!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }
        }

        #region DragAccountUIs

        public Point dragMouseDownLocation;
        private List<Point> uiPoints = new List<Point>();

        #region MouseDown

        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (lockForm) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                dragMouseDownLocation = e.Location;
                pMain.SuspendLayout();
                HideDragHandles(sender as AccountUI);
            }
        }

        #endregion

        #region MouseMove

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (lockForm) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                AccountUI dragUI = sender as AccountUI;
                int top = e.Y + dragUI.Top - MouseDownLocation.Y;
                if (top < 0) top = 0;
                else if (top > (pMain.Height - dragUI.Height) + 5) top = (pMain.Height - dragUI.Height) + 5;
                dragUI.Top = top;

                dragUI.BringToFront();
                //pMain.Invalidate();
                pMain.Update();
                foreach (AccountUI ui in pMain.Controls.OfType<AccountUI>())
                    ui.Update();
            }
        }

        #endregion

        #region MouseUp

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (lockForm) return;
            pMain.ResumeLayout();
            OrderUIs();

            SaveAccountOrders();
        }

        #endregion

        #endregion

        #region AccountOrders

        private void OrderUIs(bool doSort = true)
        {
            //pMain.AutoScroll = pMain.VerticalScroll.Enabled = pMain.HorizontalScroll.Enabled =
            //pMain.VerticalScroll.Visible = pMain.HorizontalScroll.Visible = false;            
            if (doSort)
                SortUIs();

            FormsUtils.SuspendDrawing(pMain);

            List<AccountUI> uis = pMain.Controls.OfType<AccountUI>().ToList();
            int offset = (scrollbar.Value < scrollbar.Maximum - 17 ? scrollbar.Value : scrollbar.Value + ((scrollbar.Maximum - 17) - scrollbar.Value));
            for (int i = 0; i < uis.Count; i++)
            {
                Point point = new Point(uis[i].Location.X, uis[i].Location.Y + offset);
                if (uiPoints.Contains(point))
                {
                    accountOrders.orderData.Where(o => o.email.Equals(uis[i].accountInfo.email)).First().index = uiPoints.IndexOf(point);
                    uis[i].isSecond = uiPoints.IndexOf(point) % 2 == 1;
                    uis[i].ApplyTheme(useDarkmode);
                }
            }

            FormsUtils.ResumeDrawing(pMain);
        }

        private void SortUIs()
        {
            List<int> yValues = new List<int>(); //used to save and sort the ui Y-positions.
            List<AccountUI> temp = new List<AccountUI>();
            List<AccountUI> temp2 = new List<AccountUI>();
            temp.AddRange(pMain.Controls.OfType<AccountUI>());
            foreach (AccountUI uis in temp)
                yValues.Add(uis.Location.Y);

            yValues.Sort();
            temp2.AddRange(temp);

            FormsUtils.SuspendDrawing(pMain);

            for (int i = 0; i < yValues.Count; i++)
            {
                AccountUI ui = GetUIsWithYValue(yValues[i], temp2); //Gets the ui with Y-Values.
                if (ui.Name.Equals("FALSE")) //If the Value is not Found, a ui with the name "FALSE" is returned.
                {
                    ui.Name = "AccountUI";
                    return; //Stops
                }
                ui.Location = uiPoints[i]; //sets the location of the ui to the Points[i]-value.
            }
            int offset = (scrollbar.Value < scrollbar.Maximum - 17 ? scrollbar.Value : scrollbar.Value + ((scrollbar.Maximum - 17) - scrollbar.Value));
            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Top -= offset; //apply offset
            }

            FormsUtils.ResumeDrawing(pMain);
        }

        private AccountUI GetUIsWithYValue(int value, List<AccountUI> temp2)
        {
            foreach (var ui in temp2)
            {
                if (ui.Location.Y == value)
                {
                    temp2.Remove(ui);
                    return ui;
                }
            }

            AccountUI u = new AccountUI(this);
            u.Name = "FALSE";
            return u;
        }

        #endregion

        public void UpdateAccountInfos()
        {
            #region Create colors

            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(240, 240, 240);
            Color font = Color.Black;

            if (useDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            #endregion

            #region Check for existing AccountOrders - v1.4 and older or Imported accounts do not have any -> create one if missing

            if (accountOrders == null)
            {
                //create new
                accountOrders = new AccountOrders();
                for (int i = 0; i < accounts.Count; i++)
                    accountOrders.orderData.Add(new OrderData() { email = accounts[i].email, index = i });
                SaveAccountOrders();
            }

            #endregion

            #region Check Accounts and AccountOrders

            try
            {
                List<OrderData> oData = accountOrders.orderData.Where(o => accounts.Where(a => a.email.Equals(o.email)).Count() == 0).ToList();
                if (oData.Count > 0)
                {
                    for (int i = 0; i < oData.Count; i++)
                        accountOrders.orderData.Remove(oData[i]);
                    SaveAccountOrders();
                }
            }
            catch { }

            #endregion

            FormsUtils.SuspendDrawing(pMain);
            FormsUtils.SuspendDrawing(this);

            #region Delete current AccountUIs

            accountUIs.Clear();
            List<AccountUI> temp = new List<AccountUI>();
            temp.AddRange(pMain.Controls.OfType<AccountUI>());

            for (int i = 0; i < temp.Count; i++)
                pMain.Controls.Remove(temp[i]);
            for (int i = 0; i < temp.Count; i++)
                temp[i].Dispose();
            temp.Clear();


            #endregion

            bool color = false;

            #region Create AccountUIs

            for (int i = 0; i < accounts.Count; i++)
            {
                AccountUI ui = new AccountUI(this, accounts[i]);

                ui.mouseDown += DragHandle_MouseDown;
                ui.mouseMove += DragHandle_MouseMove;
                ui.mouseUp += DragHandle_MouseUp;

                pMain.Controls.Add(ui);
                ui.Dock = DockStyle.Top;
                ui.BringToFront();
                if (color)
                    ui.BackColor = Color.FromArgb(250, 250, 250);
                ui.isSecond = color;

                temp.Add(ui);
                accountUIs.Add(ui);

                color = !color;
            }

            #endregion

            int h = temp.Count > 0 ? temp.Count * temp[temp.Count - 1].Height : 0;

            #region SortAccountUIs

            List<OrderData> sortedData = accountOrders.orderData.OrderBy(o => o.index).ToList();
            List<Point> points = new List<Point>();
            uiPoints.Clear();
            for (int i = 0; i < sortedData.Count; i++)
            {
                try
                {
                    AccountUI ui = temp.Where(o => o.accountInfo.email.Equals(sortedData[i].email)).First();
                    ui.BringToFront();
                    ui.Dock = DockStyle.None;
                    ui.Location = new Point(0, i * ui.Height);
                    uiPoints.Add(ui.Location);
                    ui.isSecond = i % 2 == 1;
                    ui.ApplyTheme(useDarkmode, def, second, third, font);
                }
                catch { }
            }

            #endregion

            scrollbar.Visible = header.ScrollbarStateChanged(h > pMain.Height);
            scrollbar.BindTo(pMain);

            for (int i = 0; i < temp.Count; i++)
                temp[i].ChangeScrollState(h > pMain.Height);

            FormsUtils.ResumeDrawing(pMain);
            FormsUtils.ResumeDrawing(this);

            dragMouseDownLocation = new Point(0, 0);

            //Stops saving the accounts on startup
            if (loading) return;
            SaveAccounts();
        }

        public void AddNewAccountInfo(MK_EAM_Lib.AccountInfo info)
        {
            accounts.Add(info);
            AddAccountToOrders(info.email);

            #region Create colors

            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(240, 240, 240);
            Color font = Color.Black;

            if (useDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            #endregion

            FormsUtils.SuspendDrawing(pMain);

            AccountUI ui = new AccountUI(this, info);

            ui.mouseDown += DragHandle_MouseDown;
            ui.mouseMove += DragHandle_MouseMove;
            ui.mouseUp += DragHandle_MouseUp;

            pMain.Controls.Add(ui);
            ui.Dock = DockStyle.Top;
            ui.SendToBack();

            if (accountUIs.Count % 2 == 1)
            {
                ui.BackColor = Color.FromArgb(250, 250, 250);
                ui.isSecond = true;
            }
            else
                ui.isSecond = false;

            ui.Dock = DockStyle.None;
            ui.Location = new Point(0, accountUIs.Count * ui.Height);
            uiPoints.Add(ui.Location);
            ui.isSecond = accountUIs.Count % 2 == 1;
            ui.ApplyTheme(useDarkmode, def, second, third, font);

            accountUIs.Add(ui);
            int h = accountUIs.Count > 0 ? accountUIs.Count * accountUIs[accountUIs.Count - 1].Height : 0;
            scrollbar.Visible = header.ScrollbarStateChanged(h > pMain.Height);
            scrollbar.BindTo(pMain);

            for (int i = 0; i < accountUIs.Count; i++)
                accountUIs[i].ChangeScrollState(h > pMain.Height);

            FormsUtils.ResumeDrawing(pMain);

            dragMouseDownLocation = new Point(0, 0);
            SaveAccounts();
        }

        public bool searchProcesses = false;
        private void GetExaltProcesses()
        {
            searchProcesses = true;
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName == "RotMG Exalt")
                    {
                        string arg = ProcADV.GetCommandLine(process);
                        string guid = arg.Substring(arg.IndexOf(",guid:") + 6, arg.Length - (arg.IndexOf(",guid:") + 6));
                        guid = guid.Substring(0, guid.IndexOf(",token:"));
                        guid = Encoding.UTF8.GetString(Convert.FromBase64String(guid));

                        foreach (AccountUI ui in pMain.Controls.OfType<AccountUI>())
                        {
                            if (ui.accountInfo.email.Equals(guid))
                            {
                                if (ui.isRunning)
                                    break;

                                ui.AddProcess(process);
                                break;
                            }
                        }
                    }
                }
                catch (Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
                {
                    // Intentionally empty - no security access to the process.
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to get Exalt-Processes (access  denied)!"));
                }
                catch (InvalidOperationException)
                {
                    // Intentionally empty - the process exited before getting details.
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to get Exalt-Processes (process exited before getting details)!"));
                }
                catch { }
            }
            searchProcesses = false;
        }

        public void ChangeDailyLoginState(MK_EAM_Lib.AccountInfo acc)
        {
            if (loading) return;

            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].email.Equals(acc.email))
                    accounts[i] = acc;
            }
            SaveAccounts();

            try
            {
                DailyLogins logins = null;
                try
                {
                    if (File.Exists(dailyLoginsPath))
                        logins = (DailyLogins)ByteArrayToObject(File.ReadAllBytes(dailyLoginsPath));
                }
                catch { }

                if (logins != null)
                {
                    List<DailyData> idsToRemove = new List<DailyData>();
                    List<int> idsToAdd = new List<int>();

                    for (int i = 0; i < accounts.Count; i++)
                    {
                        bool found = false;
                        for (int x = 0; x < logins.logins.Count; x++)
                        {
                            if (accounts[i].email.Equals(logins.logins[x].mail))
                            {
                                if (!accounts[i].performSave)
                                    idsToRemove.Add(logins.logins[x]);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            if (accounts[i].performSave)
                                idsToAdd.Add(i);
                        }
                    }

                    for (int i = 0; i < idsToRemove.Count; i++)
                        logins.logins.Remove(idsToRemove[i]);

                    for (int i = 0; i < idsToAdd.Count; i++)
                        logins.logins.Add(new DailyData() { mail = accounts[idsToAdd[i]].email });
                }
                else
                {
                    logins = new DailyLogins();
                    for (int i = 0; i < accounts.Count; i++)
                    {
                        if (accounts[i].performSave)
                            logins.logins.Add(new DailyData() { mail = accounts[i].email });
                    }
                }
                File.WriteAllBytes(dailyLoginsPath, ObjectToByteArray(logins));
            }
            catch { }
        }

        public void SaveAccounts()
        {
            try
            {
                AccountSaveFile saveFile = AccountSaveFile.Encrypt(new AccountSaveFile(), ObjectToByteArray(accounts));
                if (saveFile != null && string.IsNullOrEmpty(saveFile.error))
                {
                    File.WriteAllBytes(accountsPath, ObjectToByteArray(saveFile));
                    if (logs.Count == 0 || logs[logs.Count - 1].eventType != LogEventType.SaveAccounts || (logs[logs.Count - 1].eventType == LogEventType.SaveAccounts && logs[logs.Count - 1].time < DateTime.Now.AddMinutes(-1)))
                        LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.SaveAccounts, $"Saving accounts."));
                }
                else
                {
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to encrypt accounts!"));
                    snackbar.Show(this, $"Failed to encrypt accounts!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                    throw new Exception();
                }
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to save accounts!"));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                snackbar.Show(this, $"Failed to save accounts!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }
        }

        public void Connect(MK_EAM_Lib.AccountInfo accountInfo)
        {
            try
            {
                if (File.Exists(exePath))
                {
                    if (accountInfo.accessToken != null)
                    {
                        DateTime cTime = new DateTime(2000, 1, 1);
                        try
                        {
                            cTime = MK_EAM_Lib.AccessToken.UnixTimeStampToDateTime(Convert.ToDouble(accountInfo.accessToken.creationTime));
                        }
                        catch { cTime = new DateTime(2000, 1, 1); }
                        string hwid = GetDeviceUniqueIdentifier();
                        if (cTime.Date < DateTime.Now.Date || accountInfo.accessToken.validUntil < DateTime.Now || string.IsNullOrEmpty(accountInfo.accessToken.clientToken) || !accountInfo.accessToken.clientToken.Equals((string.IsNullOrEmpty(accountInfo.customClientID) || accountInfo.customClientID.Equals(hwid)) ? hwid : accountInfo.customClientID))
                        {
                            int index = accounts.IndexOf(accountInfo);
                            accountInfo = GetAccountData(accountInfo, false);
                            if (index >= 0)
                            {
                                accounts[index] = accountInfo;
                            }
                            SaveAccounts();
                        }
                    }
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.Login, $"Start login into account: {accountInfo.email}."));
                    snackbar.Show(this, $"Start login into account: {accountInfo.email}.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);

                    string arguments = string.Format("\"data:{{platform:Deca,guid:{0},token:{1},tokenTimestamp:{2},tokenExpiration:{3},env:4,serverName:{4}}}\"",
                                       StringToBase64String(accountInfo.email), StringToBase64String(accountInfo.accessToken.token), StringToBase64String(accountInfo.accessToken.creationTime), StringToBase64String(accountInfo.accessToken.expirationTime), GetServerName(accountInfo.serverName));

                    ProcessStartInfo pinfo = new ProcessStartInfo();
                    pinfo.FileName = exePath;
                    pinfo.Arguments = arguments;
                    Process p = new Process();
                    p.StartInfo = pinfo;
                    p.Start();

                    SaveLoginStats(accountInfo);

                    if (closeAfterConnect)
                        Environment.Exit(0);
                    else
                    {
                        foreach (AccountUI ui in pMain.Controls.OfType<AccountUI>())
                        {
                            if (!ui.isRunning && ui.accountInfo.email.Equals(accountInfo.email))
                            {
                                ui.AddProcess(p);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Login attempt failed, the game.exe was not found."));
                    snackbar.Show(this, "Login attempt failed, the game.exe was not found.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                    optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                }
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Login attempt failed: object broken, try re-adding the account."));
                snackbar.Show(this, "Login attempt failed: object broken, try re-adding the account.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
            }
        }

        private string GetServerName(string server)
        {
            if (string.IsNullOrEmpty(server))
                return serverToJoin;
            else if (server.Equals("Last"))
                return string.Empty;
            else
                return server;
        }

        private void SaveLoginStats(MK_EAM_Lib.AccountInfo info, bool sendCharList = false)
        {
            try
            {
                string fileName = Path.Combine(accountStatsPath, GetAccountStatsFilename(info.email));

                if (!Directory.Exists(accountStatsPath))
                    Directory.CreateDirectory(accountStatsPath);

                if (File.Exists(fileName))
                {
                    try
                    {
                        StatsMain stats = (StatsMain)ByteArrayToObject(File.ReadAllBytes(fileName));
                        if (stats.logins == null)
                            stats.logins = new List<LoginStats>();
                        stats.logins.Add(new LoginStats() { time = DateTime.Now, isFromTask = false });
                        File.WriteAllBytes(fileName, ObjectToByteArray(stats));
                    }
                    catch (Exception)
                    {
                        LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to load / save login-stats."));
                        snackbar.Show(this, "Failed to load / save login-stats.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                        optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                    }
                }
                else
                {
                    StatsMain stats = new StatsMain() { email = info.email };
                    stats.logins = new List<LoginStats>() { new LoginStats() { time = DateTime.Now, isFromTask = false } };
                    File.WriteAllBytes(fileName, ObjectToByteArray(stats));
                }
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to save login-stats."));
                snackbar.Show(this, "Failed to save login-stats.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
            }
        }

        private string StringToBase64String(string toEncode) => Convert.ToBase64String(StringToByteArray(toEncode));

        public void SaveNotificationoptions(NotificationOptions o)
        {
            try
            {
                if (logs.Count == 0 || logs[logs.Count - 1].eventType != LogEventType.SaveNotify || (logs[logs.Count - 1].eventType == LogEventType.SaveNotify && logs[logs.Count - 1].time < DateTime.Now.AddMinutes(-1)))
                    LogEvent(new LogData(logs.Count + 1, "EAM Notify", LogEventType.SaveNotify, $"Saving notification-options."));
                notOpt = o;
                File.WriteAllBytes(notificationOptionsPath, ObjectToByteArray(o));
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM Notify", LogEventType.EAMError, $"Failed to save notification-options!"));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                snackbar.Show(this, "Failed to save notification-options!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }
        }

        private byte[] StringToByteArray(string toConvert)
        {
            List<byte> bytes = new List<byte>();

            foreach (char c in toConvert)
            {
                bytes.Add(Convert.ToByte(c));
            }
            return bytes.ToArray();
        }

        public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = (object)binForm.Deserialize(memStream);

                return obj;
            }
        }

        public void CloseFrmLog(FrmLogViewer logViewer)
        {
            logViewer.Close();
        }

        #region Button Close / Minimize
        private void pbMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void pbClose_Click(object sender, EventArgs e) => Environment.Exit(0);

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = useDarkmode ? Color.FromArgb(225, 50, 50) : Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e) => pbClose.BackColor = Color.Transparent;

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            pbMinimize.BackColor = useDarkmode ? Color.DimGray : Color.DarkGray;
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e) => pbMinimize.BackColor = Color.Transparent;
        #endregion

        #region Drag Form
        private Point MouseDownLocation;
        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                this.Location = new Point(e.X + this.Left - MouseDownLocation.X, e.Y + this.Top - MouseDownLocation.Y);
        }
        #endregion

        private void timerLlama_Tick(object sender, EventArgs e)
        {
            timerLlama.Stop();
            pbLogo.Image = (useDarkmode) ? Properties.Resources.ic_account_balance_wallet_white_48dp : Properties.Resources.ic_account_balance_wallet_black_48dp;
        }

        public void lDev_Click(object sender, EventArgs e)
        {
            if (lockForm && sender != null) return;

            pbLogo.Image = Properties.Resources.llama;
            if (timerLlama.Enabled)
                timerLlama.Stop();
            timerLlama.Start();
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
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to parse ClientAccessData!"));
                snackbar.Show(this, "Failed to parse ClientAccessData!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }

            return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
        }

        public string GetDeviceUniqueIdentifier(bool fileFailed = false)
        {
            string ret = string.Empty;

            if (!File.Exists(forceHWIDFilePath) || fileFailed)
            {
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

                catch
                {
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to get the DeviceUniqueIdentifier!"));
                    optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                    snackbar.Show(this, "Failed to get the DeviceUniqueIdentifier!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                }
            }
            else
            {
                try
                {
                    ret = File.ReadAllText(forceHWIDFilePath);
                }
                catch
                {
                    LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to load HWID from file, using alternative Methode!"));
                    optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                    return GetDeviceUniqueIdentifier(true);
                }
            }

            return ret;
        }

        private void timerLoadProcesses_Tick(object sender, EventArgs e)
        {
            try
            {
                GetExaltProcesses();
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, $"Failed to load running processes."));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                snackbar.Show(this, "Failed to load running processes.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }
        }

        private void timerMoreToolsUI_Tick(object sender, EventArgs e)
        {
            //Top -> Down
            //if (openMoreUI)
            //{
            //    int h = moreUI.Top + 10;
            //    if (h >= optionenUI.Bottom)
            //    {
            //        h = optionenUI.Bottom;
            //        timerMoreToolsUI.Stop();
            //    }
            //    moreUI.Location = new Point(moreUI.Left, h);
            //}
            //else
            //{
            //    int h = moreUI.Top - 10;
            //    if (h <= optionenUI.Bottom - moreUI.Height)
            //    {
            //        h = optionenUI.Bottom - moreUI.Height;
            //        timerMoreToolsUI.Stop();
            //        moreUI.Visible = false;
            //    }
            //    moreUI.Location = new Point(moreUI.Left, h);
            //}

            //Right -> Left
            if (openMoreUI)
            {
                int h = moreUI.Left - 12;
                if (h <= this.Width - moreUI.Width)
                {
                    h = this.Width - moreUI.Width;
                    h -= 3;
                    timerMoreToolsUI.Stop();
                    openMoreUI = false;
                }
                moreUI.Location = new Point(h, moreUI.Top);
            }
            else
            {
                int h = moreUI.Left + 25;
                if (h >= this.Width)
                {
                    h = this.Width;
                    timerMoreToolsUI.Stop();
                    openMoreUI = true;
                }
                moreUI.Location = new Point(h, moreUI.Top);
            }
        }

        private Point m_PreviousLocation = new Point(int.MinValue, int.MinValue);
        private void FrmMain_LocationChanged(object sender, EventArgs e)
        {
            // All open child forms to be moved
            Form[] formsToAdjust = Application
              .OpenForms
              .OfType<Form>()
              .ToArray();

            // If the main form has been moved...
            if (m_PreviousLocation.X != int.MinValue)
            {
                foreach (var form in formsToAdjust) //... move all child froms aw well
                {
                    if (form == this || form.GetType().Name.Equals("FrmLogViewer") || form.GetType().Name.Equals("FrmMain")) // Except a few ones that should not
                        continue;
                    form.Location = new Point(
                      form.Location.X + Location.X - m_PreviousLocation.X,
                      form.Location.Y + Location.Y - m_PreviousLocation.Y
                    );
                }
            }
            m_PreviousLocation = Location;
        }

        private void pbDarkmode_MouseEnter(object sender, EventArgs e)
        {
            if (lockForm) return;

            pbDarkmode.BackColor = useDarkmode ? Color.FromArgb(225, 50, 50, 50) : Color.FromArgb(75, 175, 175, 175);
        }

        private void pbDarkmode_MouseLeave(object sender, EventArgs e)
        {
            if (lockForm) return;

            pbDarkmode.BackColor = pBox.BackColor;
        }

        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (lockForm) return;

            e.DrawBackground();
            e.DrawBorder();
            SolidBrush toolTipBrushForeColor = new SolidBrush(this.ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(e.ToolTipText, new Font(this.Font.FontFamily, 8f, FontStyle.Regular), toolTipBrushForeColor, e.Bounds, sf);
        }

        private void pMain_MouseEnter(object sender, EventArgs e) => HideDragHandles();

        private void lUpdateNeeded_Click(object sender, EventArgs e)
        {
            moreUI.pUpdater_Click(null, null);
        }

        public void ShowSortAlphabeticalUI(int left, bool useEmail)
        {
            if (!transition.IsCompleted)
                return;

            sortAlphabeticalUI.Left = left;
            sortAlphabeticalUI.BringToFront();

            if (sortAlphabeticalUI.Visible && sortAlphabeticalUI.useEmail != useEmail)
                sortAlphabeticalUI.Visible = false;

            sortAlphabeticalUI.useEmail = useEmail;

            if (sortAlphabeticalUI.Visible)
                transition.HideSync(sortAlphabeticalUI, true);
            else
                transition.ShowSync(sortAlphabeticalUI, true);
        }

        public void SortAccountsAlphabetical(bool az, bool useEmail = false)
        {
            try
            {
                if (az)
                {
                    accountOrders.orderData = useEmail ? accountOrders.orderData.OrderBy(o => o.email).ToList() : accountOrders.orderData = accountOrders.orderData.OrderBy(o => accounts.Where(a => a.email.Equals(o.email)).First().name).ToList();
                }
                else
                {
                    accountOrders.orderData = useEmail ? accountOrders.orderData.OrderByDescending(o => o.email).ToList() : accountOrders.orderData = accountOrders.orderData.OrderByDescending(o => accounts.Where(a => a.email.Equals(o.email)).First().name).ToList();
                }
            }
            catch
            {
                LogEvent(new LogData(logs.Count + 1, "EAM", LogEventType.EAMError, "Failed to order accounts."));
                optionenUI.BlinkLog((useDarkmode) ? Color.Crimson : Color.IndianRed);
                snackbar.Show(this, "Failed to order accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }

            for (int i = 0; i < accountOrders.orderData.Count; i++)
                accountOrders.orderData[i].index = i;

            FormsUtils.SuspendDrawing(pMain);

            List<AccountUI> uis = pMain.Controls.OfType<AccountUI>().ToList();
            int offset = (scrollbar.Value < scrollbar.Maximum - 17 ? scrollbar.Value : scrollbar.Value + ((scrollbar.Maximum - 17) - scrollbar.Value));
            for (int i = 0; i < uis.Count; i++)
            {
                Point point = new Point(uis[i].Location.X, uis[i].Location.Y + offset);
                if (uiPoints.Contains(point))
                {
                    int x = accountOrders.orderData.Where(o => o.email.Equals(uis[i].accountInfo.email)).First().index;
                    uis[i].Location = uiPoints[x];
                    uis[i].isSecond = x % 2 == 1;
                    uis[i].ApplyTheme(useDarkmode);
                }
            }

            FormsUtils.ResumeDrawing(pMain);

            SaveAccountOrders();

            transition.HideSync(sortAlphabeticalUI, true);
        }

        private void timerShowMessage_Tick(object sender, EventArgs e)
        {
            timerShowMessage.Stop();

            if (notificationSaveFile == null)
                notificationSaveFile = new EAMNotificationMessageSaveFile();

            if (msg.forceShow || !notificationSaveFile.knownIDs.Contains(msg.id) || notificationSaveFile.lastCheckWasStop)
            {
                if (!notificationSaveFile.knownIDs.Contains(msg.id))
                    notificationSaveFile.knownIDs.Add(msg.id);
                bool wasStop = notificationSaveFile.lastCheckWasStop;
                bool frmShown = false;

                notificationSaveFile.forceCheck =
                notificationSaveFile.lastCheckWasStop = false;

                //Show Message
                switch (msg.type)
                {
                    case EAMNotificationMessageType.None:
                        break;
                    case EAMNotificationMessageType.UpdateAvailable:
                        {
                            if (!notificationValues[1])
                                return;
                            lUpdateAvailable.Visible = true;
                            lVersion.ForeColor = useDarkmode ? Color.Orange : Color.DarkOrange;
                            notificationSaveFile.forceCheck = true;
                            updateLink = isMPGHVersion ? msg.linkM : msg.link;

                            FrmMessage frmMsg = new FrmMessage(this, msg);
                            frmMsg.StartPosition = FormStartPosition.Manual;
                            frmMsg.Location = new Point(this.Location.X + ((this.Width - frmMsg.Width) / 2), this.Location.Y + ((this.Height - frmMsg.Height) / 2));
                            frmMsg.Show(this);
                            frmShown = true;
                        }
                        break;
                    case EAMNotificationMessageType.Message:
                        {
                            if (!notificationValues[2])
                                return;
                            lockForm = true;
                            FrmMessage frmMsg = new FrmMessage(this, msg);
                            frmMsg.StartPosition = FormStartPosition.Manual;
                            frmMsg.Location = new Point(this.Location.X + ((this.Width - frmMsg.Width) / 2), this.Location.Y + ((this.Height - frmMsg.Height) / 2));
                            frmMsg.Show(this);
                            frmShown = true;
                        }
                        break;
                    case EAMNotificationMessageType.Warning:
                        {
                            if (!notificationValues[2])
                                return;
                            lockForm = true;

                            FrmMessage frmMsg = new FrmMessage(this, msg);
                            frmMsg.StartPosition = FormStartPosition.Manual;
                            frmMsg.Location = new Point(this.Location.X + ((this.Width - frmMsg.Width) / 2), this.Location.Y + ((this.Height - frmMsg.Height) / 2));
                            frmMsg.Show(this);
                            frmShown = true;
                        }
                        break;
                    case EAMNotificationMessageType.Stop:
                        {
                            if (!notificationValues[3])
                                return;

                            lockForm = true;
                            pMain.Enabled = false;
                            notificationSaveFile.forceCheck = true;
                            notificationSaveFile.lastCheckWasStop = true;

                            FrmMessage frmMsg = new FrmMessage(this, msg);
                            frmMsg.StartPosition = FormStartPosition.Manual;
                            frmMsg.Location = new Point(this.Location.X + ((this.Width - frmMsg.Width) / 2), this.Location.Y + ((this.Height - frmMsg.Height) / 2));
                            frmMsg.Show(this);
                            frmShown = true;
                        }
                        break;
                    default:
                        break;
                }
                File.WriteAllBytes(lastNotificationCheckPath, ObjectToByteArray(notificationSaveFile));

                if (wasStop && msg.type != EAMNotificationMessageType.Stop)
                {
                    lockForm = frmShown;
                    pMain.Enabled = true;
                }
            }
        }

        private void lUpdateAvailable_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(updateLink);
        }
    }

    public static class ProcADV
    {
        public static string GetCommandLine(this Process process)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
                using (ManagementObjectCollection objects = searcher.Get())
                {
                    return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
                }
            }
            catch { }

            return "";
        }
    }

    [Obsolete]
    [System.Serializable]
    public class AccountInfo
    {
        public string name;
        public string email;
        public string password;

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

        public static MK_EAM_Lib.AccountInfo Convert(AccountInfo info)
        {
            return new MK_EAM_Lib.AccountInfo()
            {
                name = info.name,
                email = info.email,
                password = info.password,
                performSave = info.performSave,
                accessToken = AccessToken.Convert(info.accessToken),
                requestSuccessfull = info.requestSuccessfull
            };
        }
    }
    [Obsolete]
    [System.Serializable]
    public class AccessToken
    {
        public string token;
        public string creationTime;
        public string expirationTime;
        public string clientToken;
        public System.DateTime validUntil;

        public AccessToken() { }

        public static MK_EAM_Lib.AccessToken Convert(ExaltAccountManager.AccessToken t)
        {
            return new MK_EAM_Lib.AccessToken()
            {
                token = t.token,
                creationTime = t.creationTime,
                expirationTime = t.expirationTime,
                clientToken = t.clientToken,
                validUntil = t.validUntil
            };
        }
    }
}
