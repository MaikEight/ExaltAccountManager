using Bunifu.UI.WinForms;
using ExaltAccountManager.UI;
using ExaltAccountManager.UI.Elements;
using MK_EAM_Analytics;
using MK_EAM_General_Services_Lib;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using MK_EAM_General_Services_Lib.General.Responses;
using System.Drawing.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExaltAccountManager
{
    public sealed partial class FrmMain : Form
    {
        public readonly Version version = new Version(3, 2, 0);
        public const string GITHUB_PROJECT_URL = "https://github.com/MaikEight/ExaltAccountManager";
        public const string DISCORD_INVITE_URL = "https://discord.exalt-account-manager.eu";
        public string API_BASE_URL { get; internal set; } = "https://api.exalt-account-manager.eu/";

        public event EventHandler ThemeChanged;

        private System.Timers.Timer saveAccountsTimer;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public bool UseDarkmode
        {
            get => useDarkmode;
            set
            {
                useDarkmode = value;
                if (ThemeChanged != null)
                    ThemeChanged(this, new EventArgs());

                toolTip.SetToolTipIcon(btnSwitchDesign, useDarkmode ? Properties.Resources.ic_brightness_5_white_48dp : Properties.Resources.ic_brightness_4_black_48dp);
                toolTip.SetToolTipTitle(btnSwitchDesign, useDarkmode ? "Switch to daymode" : "Switch to darkmode");
                toolTip.SetToolTip(btnSwitchDesign, useDarkmode ? "Burn your eyes!" : "Come to the dark side, we have cookies!");
            }
        }
        private bool useDarkmode = false;

        private Size defaultMinimumsize = new Size(0, 0);

        public bool loading = false;
        private LogData lastLogData = new LogData(-1, "", LogEventType.EAMError, "") { time = new DateTime() };

        public BindingList<MK_EAM_Lib.AccountInfo> accounts = new BindingList<MK_EAM_Lib.AccountInfo>();
        public ServerDataCollection serverData
        {
            get => serverDataValue;
            set
            {
                serverDataValue = value;
                try
                {
                    File.WriteAllBytes(serverCollectionPath, ObjectToByteArray(serverData));
                }
                catch { }
                if (uiAccounts != null)
                    uiAccounts.LoadServers();

                if (uiOptions != null)
                    uiOptions.LoadServers();
            }
        }
        private ServerDataCollection serverDataValue = new ServerDataCollection();

        public OptionsData OptionsData
        {
            get => optionsDataValue;
            set
            {
                optionsDataValue = value;

                if (uiOptions != null)
                    uiOptions.ApplyOptions();
            }
        }
        private OptionsData optionsDataValue = new OptionsData();
        private bool drawConfigChangesIcon = false;
        public NotificationOptions notOpt = new NotificationOptions();

        private EAMNotificationMessageSaveFile notificationSaveFile = new EAMNotificationMessageSaveFile();
        public bool HasNewNews
        {
            get => hasNewNews;
            set
            {
                hasNewNews = value;
                btnNews.Invalidate();
            }
        }
        private bool hasNewNews = false;
        public DateTime LastNewsViewed { get; internal set; } = DateTime.MinValue;
        public DiscordUser DiscordUser { get; internal set; } = null;

        private GameUpdater gameUpdater { get; set; }

        private UIAccounts uiAccounts;
        private UIEAMNews uiEAMNews;
        private UIAddAccount uiAddAccounts;
        private UIModules uiModules;
        private UIOptions uiOptions;
        private UIHelp uiHelp;
        private UILogs uiLogs;
        private UIAbout uiAbout;
        private EleGameUpdater eleGameUpdater;
        private EleQNA eleEAMUpdate;
        private FrmShadowHost frmShadowHost;

        private UIState uiState
        {
            get => uiStateVal;
            set
            {
                #region Reset Image of old state

                switch (uiStateVal)
                {
                    case UIState.Accounts:
                        btnAccounts.Image = useDarkmode ? Properties.Resources.ic_people_outline_white_24dp : Properties.Resources.ic_people_outline_black_24dp;
                        break;
                    case UIState.News:
                        btnNews.Image = useDarkmode ? Properties.Resources.news_outline_white_24px : Properties.Resources.news_outline_black_24px;
                        break;
                    case UIState.AddAccount:
                        btnAddAccount.Image = useDarkmode ? Properties.Resources.add_user_white_outline_24px : Properties.Resources.add_user_outline_24px;
                        break;
                    case UIState.Modules:
                        btnModules.Image = useDarkmode ? Properties.Resources.dashboard_layout_outline_white_24px : Properties.Resources.dashboard_layout_outline_24px;
                        break;
                    case UIState.Options:
                        btnOptions.Image = useDarkmode ? Properties.Resources.settings_outline_white_24px : Properties.Resources.settings_outline_24px;
                        break;
                    case UIState.Help:
                        btnHelp.Image = useDarkmode ? Properties.Resources.ic_help_outline_white_24dp : Properties.Resources.ic_help_outline_black_24dp;
                        break;
                    case UIState.Logs:
                        btnLogs.Image = useDarkmode ? Properties.Resources.activity_history_outline_white_24px : Properties.Resources.activity_history_outline_24px;
                        break;
                    case UIState.AboutEAM:
                        btnAbout.Image = useDarkmode ? Properties.Resources.ic_info_outline_white_24dp : Properties.Resources.ic_info_outline_black_24dp;
                        break;
                    default:
                        this.MinimumSize = defaultMinimumsize;
                        break;
                }

                #endregion

                lastUiState = uiState;
                uiStateVal = value;

                #region Set Image of new State

                switch (uiStateVal)
                {
                    case UIState.Accounts:
                        btnAccounts.Image = useDarkmode ? Properties.Resources.ic_people_white_24dp : Properties.Resources.ic_people_black_24dp;
                        lTitle.Text = "Accounts";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Accounts);
                        break;
                    case UIState.News:
                        btnNews.Image = useDarkmode ? Properties.Resources.news_white_24px : Properties.Resources.news_black_24px;
                        lTitle.Text = "News";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.News);
                        break;
                    case UIState.AddAccount:
                        btnAddAccount.Image = useDarkmode ? Properties.Resources.add_user_white_24px : Properties.Resources.add_user_24px;
                        lTitle.Text = "Add Account";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Settings);
                        break;
                    case UIState.Modules:
                        btnModules.Image = useDarkmode ? Properties.Resources.dashboard_layout_white_24px : Properties.Resources.dashboard_layout_24px;
                        lTitle.Text = "Modules";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Modules);
                        break;
                    case UIState.Options:
                        btnOptions.Image = useDarkmode ? Properties.Resources.settings_white_24px : Properties.Resources.settings_24px;
                        lTitle.Text = "Options";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Settings);
                        break;
                    case UIState.Help:
                        btnHelp.Image = useDarkmode ? Properties.Resources.ic_help_white_24dp : Properties.Resources.ic_help_black_24dp;
                        lTitle.Text = "Help";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Help);
                        break;
                    case UIState.Logs:
                        btnLogs.Image = useDarkmode ? Properties.Resources.activity_history_white_24px : Properties.Resources.activity_history_24px;
                        lTitle.Text = "Logs";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Logs);
                        break;
                    case UIState.AboutEAM:
                        btnAbout.Image = useDarkmode ? Properties.Resources.ic_info_white_24dp : Properties.Resources.ic_info_black_24dp;
                        lTitle.Text = "About Exalt Account Manager";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.About);
                        break;
                    case UIState.Updater:
                        lTitle.Text = "Updater";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Updater);
                        break;
                    case UIState.Changelog:
                        lTitle.Text = "Changelog";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Changelog);
                        break;
                    case UIState.TokenViewer:
                        lTitle.Text = "Token Viewer";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.TokenViewer);
                        break;
                    case UIState.ImportExport:
                        lTitle.Text = "Im- & Export";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Accounts);
                        break;
                    case UIState.DailyLogin:
                        lTitle.Text = "Daily logins";

                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.DailyLogin);
                        break;
                    case UIState.DailyNotifications:
                        lTitle.Text = "Daily Login notification-settings";
                        break;
                    default:
                        this.MinimumSize = defaultMinimumsize;
                        DiscordHelper.UpdateMenu(DiscordHelper.Menu.Accounts);
                        break;
                }
                DiscordHelper.ApplyPresence();

                #endregion

                pSideBar.Visible = !(uiState == UIState.Changelog || uiState == UIState.TokenViewer || uiState == UIState.ImportExport || uiState == UIState.DailyLogin || uiState == UIState.DailyNotifications);
            }
        }
        private UIState uiStateVal = UIState.Accounts;
        private UIState lastUiState = UIState.Accounts;

        public Bunifu.UI.WinForms.BunifuSnackbar.Positions SnackbarPosition { get; set; } = Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight;

        #region Paths

        public static readonly string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        public readonly string optionsPath = Path.Combine(saveFilePath, "EAM.options");
        public readonly string accountsPath = Path.Combine(saveFilePath, "EAM.accounts");
        public readonly string accountOrdersPath = Path.Combine(saveFilePath, "EAM.accountOrders");
        public readonly string dailyLoginsPath = Path.Combine(saveFilePath, "EAM.DailyLoginsV2");
        public readonly string notificationOptionsPath = Path.Combine(saveFilePath, "EAM.NotificationOptions");
        public readonly string serverCollectionPath = Path.Combine(saveFilePath, "EAM.ServerCollection");
        public readonly string accountStatsPath = Path.Combine(saveFilePath, "Stats");
        public readonly string pathLogs = Path.Combine(saveFilePath, "EAM.Logs");
        public readonly string pathNews = Path.Combine(saveFilePath, "EAM.News");
        public readonly string pathDiscordPopups = Path.Combine(saveFilePath, "EAM.DiscordPopup");
        public readonly string lastUpdateCheckPath = Path.Combine(saveFilePath, "EAM.LastUpdateCheck");
        public readonly string lastNotificationCheckPath = Path.Combine(saveFilePath, "EAM.LastNotificationCheck");
        public readonly string forceHWIDFilePath = Path.Combine(saveFilePath, "EAM.HWID");
        public readonly string itemsSaveFilePath = Path.Combine(saveFilePath, "EAM.ItemsSaveFile");
        public readonly string privacyPolicyPath = Path.Combine(saveFilePath, "EAM_Privacy_Policy.txt");
        public readonly string activeVaultPeekerAccountsPath = Path.Combine(saveFilePath, "EAM.ActiveVaultPeekerAccounts");
        public readonly string getClientHWIDToolPath = Path.Combine(Application.StartupPath, "EAM_GetClientHWID");
        public readonly string pingCheckerExePath = Path.Combine(Application.StartupPath, "EAM PingChecker.exe");
        public readonly string statisticsExePath = Path.Combine(Application.StartupPath, "EAM Statistics.exe");
        public readonly string dailyServiceExePath = Path.Combine(Application.StartupPath, "DailyService", "EAM Daily Login Service.exe");
        public readonly string vaultPeekerExePath = Path.Combine(Application.StartupPath, "EAM Vault Peeker.exe");

        #endregion

        private string linkUpdate = string.Empty;

        #region Borderless Form Minimize On Taskbar Icon Click

        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }

        #endregion

        public FrmMain(string[] args)
        {
            #region Arguments

            if (args?.Length > 0)
            {
                switch (args[0])
                {
                    case "update":
                        {
                            if (args.Length >= 2)
                            {
                                string tempUpdatePath = args[1];
                                MK_EAM_Updater_Lib.Updater.MoveUpdateFiles(Application.StartupPath, tempUpdatePath);

                                System.Diagnostics.Process.Start(Application.ExecutablePath);
                                Environment.Exit(0);
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            #endregion

            InitializeComponent();

            defaultMinimumsize = this.MinimumSize = new Size(this.MinimumSize.Width, 576);
            lVersion.Text = $"EAM v{version} by Maik8";

            bool isNewInstall = false;

            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
                isNewInstall = true;
            }

            isNewInstall = (isNewInstall || (!File.Exists(accountsPath) && !File.Exists(optionsPath)));

            #region API Base URL

            try
            {
                API_BASE_URL = "https://api.exalt-account-manager.eu/";
                string fileName = Path.Combine(Application.StartupPath, "MK_EAM_API_DATA");
                if (File.Exists(fileName))
                    API_BASE_URL = File.ReadAllText(fileName);
            }
            catch { API_BASE_URL = "https://api.exalt-account-manager.eu/"; }

            #endregion

            #region LatestNewsViews

            try
            {
                LastNewsViewed = DateTime.MinValue;
                if (File.Exists(pathNews))
                {
                    long.TryParse(File.ReadAllText(pathNews), out long ticks);
                    LastNewsViewed = new DateTime(ticks);
                }
            }
            catch { LastNewsViewed = DateTime.MinValue; }

            #endregion

            if (!isNewInstall)
            {
                #region Options

                if (File.Exists(optionsPath))
                {
                    try
                    {
                        OptionsData = (OptionsData)ByteArrayToObject(File.ReadAllBytes(optionsPath));
                        UseDarkmode = OptionsData.useDarkmode;
                        SnackbarPosition = (Bunifu.UI.WinForms.BunifuSnackbar.Positions)OptionsData.snackbarPosition;

                        bool toSave = false;
                        if (OptionsData.discordOptions == null)
                        {
                            toSave = true;
                            OptionsData.discordOptions = new DiscordOptions() { ShowAccountNames = true, ShowMenus = true, ShowState = true };
                        }

                        if (OptionsData.analyticsOptions == null)
                        {
                            toSave = true;
                            OptionsData.analyticsOptions = new AnalyticsOptions() { Anonymization = false, OptOut = false };
                        }

                        if (toSave)
                        {
                            try
                            {
                                File.WriteAllBytes(optionsPath, ObjectToByteArray(OptionsData));
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        OptionsData = new OptionsData()
                        {
                            exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe"),
                            closeAfterConnection = false,
                            snackbarPosition = 8,
                            discordOptions = new DiscordOptions() { ShowAccountNames = true, ShowMenus = true, ShowState = true },
                            analyticsOptions = new AnalyticsOptions() { Anonymization = false, OptOut = false },
                        };
                        File.WriteAllBytes(optionsPath, ObjectToByteArray(OptionsData));
                    }
                    catch { }
                }

                if (OptionsData.discordOptions == null)
                {
                    OptionsData.discordOptions = new DiscordOptions() { ShowAccountNames = true, ShowMenus = true, ShowState = true };
                }

                if (!OptionsData.discordOptions.OptOut)
                {
                    DiscordHelper.OnDiscordConnectionChanged += DiscordHelper_OnDiscordConnectionChanged;
                    DiscordHelper.Initialize(OptionsData.discordOptions,
                                             this,
                                             autoEvents: false,
                                             _updateOnChange: false);
                }

                #endregion

                #region Accounts

                if (File.Exists(accountsPath))
                    LoadAccountInfos(accountsPath);

                #endregion

                #region NotificationOptions

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

                #endregion

                try
                {
                    if (!Directory.Exists(accountStatsPath))
                        Directory.CreateDirectory(accountStatsPath);
                }
                catch { }
            }

            LoadServerData();

            //Init
            ThemeChanged += ApplyTheme;
            ApplyTheme(this, null);

            uiAccounts = new UIAccounts(this)
            {
                Dock = DockStyle.Fill
            };
            pContent.Controls.Add(uiAccounts);
            uiStateVal = UIState.Accounts;

            bunifuForm.SubscribeControlsToDragEvents(new Control[] { lTitle, pHeader, lHeaderEAM, pbHeader }, true);
            bunifuForm.AllowHidingBottomRegion = true;

            saveAccountsTimer = new System.Timers.Timer() { Interval = 1000 };
            saveAccountsTimer.Elapsed += SaveAccountsTimer_Tick;

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
            if (notificationSaveFile.forceCheck || DateTime.Now.Date > notificationSaveFile.lastCheck.Date || notificationSaveFile.lastCheckWasStop)
                EAMNotificationMessage.GetEAMNotificationMessage(version.ToString(), (EAMNotificationMessage msg) => NotificationMessageInvoker(msg));

            this.Show();
        }

        private void DiscordHelper_OnDiscordConnectionChanged(object sender, bool isConnected)
        {
            if (File.Exists(pathDiscordPopups))
            {
                try
                {
                    DiscordPopupSettings settings = JsonConvert.DeserializeObject<DiscordPopupSettings>(File.ReadAllText(pathDiscordPopups));

                    if (settings.LastDiscordPopupResult == DiscordPopupSettings.DiscordpopupResult.Never ||
                        (DateTime.Now - settings.LastQuestion).TotalDays <= 7 ||
                        settings.LastDiscordPopupResult == DiscordPopupSettings.DiscordpopupResult.Yes)
                    {
                        return;
                    }
                }
                catch { }
            }

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(10000);

            if (isConnected && DiscordUser == null)
            {
                //Request discord user
                //If no user is found, ask if the user wants to create a connection
                ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
                {
                    CancellationToken token = (CancellationToken)obj;
                    try
                    {
                        Task<DiscordUser> task = GeneralServicesClient.Instance?.GetDiscordUser(GetAPIClientIdHash(false));
                        while (!task.IsCompleted)
                        {
                            if (token.IsCancellationRequested)
                            {
                                LogEvent(new MK_EAM_Lib.LogData(
                                    "EAM",
                                    MK_EAM_Lib.LogEventType.APIError,
                                    "Failed to fetch stored discord user data."));

                                return;
                            }

                            await Task.Delay(50, token);
                        }
                        DiscordUser = task.Result;


                        if (DiscordUser != null && DiscordUser.DiscordUserId.Equals("NotFound"))
                        { //No discord user found

                            DiscordUserConnectionInvoker(DiscordUser, token);                            
                        }
                        else if (DiscordUser != null)
                        {
                            File.WriteAllText(pathDiscordPopups, JsonConvert.SerializeObject(new DiscordPopupSettings()
                            {
                                LastQuestion = DateTime.Now,
                                LastDiscordPopupResult = DiscordPopupSettings.DiscordpopupResult.Yes
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvent(new MK_EAM_Lib.LogData(
                                   "EAM",
                                   MK_EAM_Lib.LogEventType.APIError,
                                   "Failed to fetch stored discord user data." + Environment.NewLine + "Exception: " + ex.Message));
                    }
                }), cancellationTokenSource.Token);
            }
        }

        private void DiscordUserConnectionInvoker(DiscordUser dcUser, CancellationToken token) => DiscordUserConnection(dcUser, token);
        private bool DiscordUserConnection(DiscordUser dcUser, CancellationToken token)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<DiscordUser, CancellationToken, bool>)DiscordUserConnection, dcUser, token);

            DiscordUser = null;
            using (FrmDiscordPopup popup = new FrmDiscordPopup(this))
            {
                popup.Location = new Point(this.Location.X + (this.Width - popup.Width) / 2, this.Location.Y + (this.Height - popup.Height) / 2);
                if (ShowShadowFormDialog(popup) == DialogResult.OK)
                {
                    FetchDiscordUser();
                    return true;
                }
            }

            return false;
        }

        private async void FetchDiscordUser()
        {
            ulong discordId = DiscordHelper.GetUserId();
            if (discordId == ulong.MaxValue)
            {
                LogEvent(new MK_EAM_Lib.LogData(
                    "EAM",
                    MK_EAM_Lib.LogEventType.EAMError,
                    "Failed to set discord user id."));
                return;
            }

            DiscordUser = await GeneralServicesClient.Instance?.PostDiscordUser(GetAPIClientIdHash(false), discordId.ToString());

        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            timerLoadUI.Start();
            gameUpdater = new GameUpdater(OptionsData.exePath, lastUpdateCheckPath);
            GameUpdater.Instance.OnUpdateRequired += UpdateRequiredInvoker;

            if (!OptionsData.analyticsOptions.OptOut)
            {
                new AnalyticsClient(API_BASE_URL + "v1/Analytics");
                AnalyticsClient.Instance?.StartSession(accounts.Count, GetAPIClientIdHash(), version);
            }

            _ = new GeneralServicesClient(API_BASE_URL);

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(10000);

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;
                try
                {
                    Task<Version> task = GeneralServicesClient.Instance?.GetLatestEamVersion();
                    while (!task.IsCompleted)
                    {
                        if (token.IsCancellationRequested)
                        {
                            OnAPIResponseLatestEamVersion(new Version(0, 0, 0));
                            LogEvent(new MK_EAM_Lib.LogData(
                                "EAM",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Failed to fetch latest EAM-Version."));

                            return;
                        }

                        await Task.Delay(50, token);
                    }
                    Version latestVersion = task.Result;
                    OnAPIResponseLatestEamVersion(latestVersion);
                }
                catch (Exception ex)
                {
                    LogEvent(new MK_EAM_Lib.LogData(
                               "EAM",
                               MK_EAM_Lib.LogEventType.APIError,
                               "Failed to fetch latest EAM-Version." + Environment.NewLine + "Exception: " + ex.Message));
                }
            }), cancellationTokenSource.Token);
        }

        private void OnAPIResponseLatestEamVersion(Version latestVersion)
        {
            if (latestVersion == null)
            {
                LogEvent(new MK_EAM_Lib.LogData(
                               "EAM",
                               MK_EAM_Lib.LogEventType.APIError,
                               "Fetched EAM-Version is null."));
                return;
            }

            if (version < latestVersion)
            {
                //Update needed!
                ShowEamUpdateNoticeInvoker();
            }
        }

        private void PerformEamUpdateInvoker(string releaselink, string directDownloadLink)
        {
            PerformEamUpdate(releaselink, directDownloadLink);
        }

        private bool PerformEamUpdate(string releaselink, string directDownloadLink)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<string, string, bool>)PerformEamUpdate, releaselink, directDownloadLink);

            try
            {
                string updaterPath = Path.Combine(Application.StartupPath, "EAM_Updater.exe");
                if (File.Exists(updaterPath))
                {
                    LogEvent(new MK_EAM_Lib.LogData(
                               "EAM",
                               MK_EAM_Lib.LogEventType.UpdateEAM,
                               "Starting EAM_Updater."));
                    ProcessStartInfo info = new ProcessStartInfo(updaterPath);
                    info.Arguments = directDownloadLink;
                    Process.Start(info);

                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                LogEvent(new MK_EAM_Lib.LogData(
                               "EAM",
                               MK_EAM_Lib.LogEventType.Error,
                               "Failed to start the EAM_Updater. Exception: " + ex.Message));

                ShowSnackbar("Failed to start the EAM_Updater.", BunifuSnackbar.MessageTypes.Error, 7500);
            }

            return false;
        }

        private void ShowEamUpdateNoticeInvoker()
        {
            ShowEamUpdateNotice();
        }

        private bool ShowEamUpdateNotice()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)ShowEamUpdateNotice);

            ExaltAccountManagerUpdateAvailable();

            return false;
        }


        public void ApplyTheme(object sender, EventArgs e)
        {
            #region ApplyTheme

            Color def = ColorScheme.GetColorDef(useDarkmode);
            Color second = ColorScheme.GetColorSecond(useDarkmode);
            Color third = ColorScheme.GetColorThird(useDarkmode);
            Color font = ColorScheme.GetColorFont(useDarkmode);

            this.BackColor = pContent.BackColor = def;
            pSideButtons.BackColor = pTop.BackColor = pbClose.BackColor = pbMinimize.BackColor = second;
            this.ForeColor = font;

            pbClose.Image = useDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
            pbMinimize.Image = UseDarkmode ? Properties.Resources.baseline_minimize_white_24dp : Properties.Resources.baseline_minimize_black_24dp;

            if (gameUpdater != null && GameUpdater.Instance.UpdateRequired)
                lVersion.ForeColor = UseDarkmode ? Color.Orange : Color.DarkOrange;

            #region Button Images

            btnAccounts.Image = useDarkmode ? Properties.Resources.ic_people_outline_white_24dp : Properties.Resources.ic_people_outline_black_24dp;
            btnNews.Image = useDarkmode ? Properties.Resources.news_outline_white_24px : Properties.Resources.news_outline_black_24px;
            btnAddAccount.Image = useDarkmode ? Properties.Resources.add_user_white_outline_24px : Properties.Resources.add_user_outline_24px;
            btnModules.Image = useDarkmode ? Properties.Resources.dashboard_layout_outline_white_24px : Properties.Resources.dashboard_layout_outline_24px;
            btnOptions.Image = useDarkmode ? Properties.Resources.settings_outline_white_24px : Properties.Resources.settings_outline_24px;
            btnHelp.Image = useDarkmode ? Properties.Resources.ic_help_outline_white_24dp : Properties.Resources.ic_help_outline_black_24dp;
            btnLogs.Image = useDarkmode ? Properties.Resources.activity_history_outline_white_24px : Properties.Resources.activity_history_outline_24px;
            btnAbout.Image = useDarkmode ? Properties.Resources.ic_info_outline_white_24dp : Properties.Resources.ic_info_outline_black_24dp;
            btnGameUpdater.Image = useDarkmode ? Properties.Resources.update_left_rotation_white_24px : Properties.Resources.update_left_rotation_24px;

            switch (uiStateVal)
            {
                case UIState.Accounts:
                    btnAccounts.Image = useDarkmode ? Properties.Resources.ic_people_white_24dp : Properties.Resources.ic_people_black_24dp;
                    break;
                case UIState.News:
                    btnNews.Image = useDarkmode ? Properties.Resources.news_white_24px : Properties.Resources.news_black_24px;
                    break;
                case UIState.AddAccount:
                    btnAddAccount.Image = useDarkmode ? Properties.Resources.add_user_white_24px : Properties.Resources.add_user_24px;
                    break;
                case UIState.Modules:
                    btnModules.Image = useDarkmode ? Properties.Resources.dashboard_layout_white_24px : Properties.Resources.dashboard_layout_24px;
                    break;
                case UIState.Options:
                    btnOptions.Image = useDarkmode ? Properties.Resources.settings_white_24px : Properties.Resources.settings_24px;
                    break;
                case UIState.Help:
                    btnHelp.Image = useDarkmode ? Properties.Resources.ic_help_white_24dp : Properties.Resources.ic_help_black_24dp;
                    break;
                case UIState.Logs:
                    btnLogs.Image = useDarkmode ? Properties.Resources.activity_history_white_24px : Properties.Resources.activity_history_24px;
                    break;
                case UIState.AboutEAM:
                    btnAbout.Image = useDarkmode ? Properties.Resources.ic_info_white_24dp : Properties.Resources.ic_info_black_24dp;
                    break;
                case UIState.Updater:
                    btnGameUpdater.Image = useDarkmode ? Properties.Resources.available_updates_white_24px : Properties.Resources.available_updates_24px;
                    break;
                default:
                    break;
            }

            #endregion

            #region Snackbar

            BunifuSnackbar.CustomizationOptions opt = new BunifuSnackbar.CustomizationOptions()
            {
                ActionBackColor = UseDarkmode ? Color.FromArgb(8, 8, 8) : Color.White,
                ActionBorderColor = UseDarkmode ? Color.FromArgb(15, 15, 15) : Color.White,
                ActionForeColor = UseDarkmode ? Color.FromArgb(170, 170, 170) : Color.Black,
                BackColor = UseDarkmode ? Color.FromArgb(8, 8, 8) : Color.White,
                BorderColor = UseDarkmode ? Color.FromArgb(15, 15, 15) : Color.White,
                ForeColor = UseDarkmode ? Color.FromArgb(170, 170, 170) : Color.Black,
                CloseIconColor = Color.FromArgb(246, 255, 237),
                ActionBorderRadius = 9,
            };

            snackbar.ErrorOptions = opt;
            snackbar.ErrorOptions.CloseIconColor = Color.FromArgb(255, 204, 199);
            snackbar.WarningOptions = opt;
            snackbar.WarningOptions.CloseIconColor = Color.FromArgb(255, 229, 143);
            snackbar.InformationOptions = opt;
            snackbar.InformationOptions.CloseIconColor = Color.FromArgb(145, 213, 255);
            snackbar.SuccessOptions = opt;
            snackbar.SuccessOptions.CloseIconColor = Color.FromArgb(246, 255, 237);

            #endregion

            toolTip.BackColor = def;
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = useDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);

            DiscordHelper.UpdateMenu(DiscordHelper.Menu.Accounts);

            #endregion
        }

        private void NotificationMessage(EAMNotificationMessage msg)
        {
            if (notificationSaveFile == null)
                notificationSaveFile = new EAMNotificationMessageSaveFile();

            if (!notificationSaveFile.knownIDs.Contains(msg.id))
            {
                QNA notMessage = new QNA();
                bool showUI = false;

                switch (msg.type)
                {
                    case EAMNotificationMessageType.None:
                        break;
                    case EAMNotificationMessageType.UpdateAvailable:
                        {
                            if (!OptionsData.searchUpdateNotification)
                                return;
                            showUI = true;
                            linkUpdate = msg.link;
                            notMessage = new QNA()
                            {
                                Question = "Exalt Account Manager Update available",
                                Answer = msg.message,
                                ButtonText = "Show release on Github",
                                ButtonImage = Properties.Resources.update_left_rotation_white_24px,
                                Type = QuestionType.Update,
                                Action = (object sender, EventArgs e) => System.Diagnostics.Process.Start(linkUpdate)
                            };
                            ExaltAccountManagerUpdateAvailable();

                            notificationSaveFile.forceCheck = true;
                        }
                        break;
                    case EAMNotificationMessageType.Message:
                        {
                            if (!OptionsData.searchWarnings)
                                return;

                            showUI = true;
                            linkUpdate = msg.link;

                            notMessage = new QNA()
                            {
                                Question = "Message",
                                Answer = msg.message,
                                Type = QuestionType.Message
                            };
                            if (!string.IsNullOrWhiteSpace(linkUpdate))
                            {
                                notMessage.ButtonText = "Open website";
                                notMessage.ButtonImage = Properties.Resources.ic_public_white_36dp;
                                notMessage.Action = (object sender, EventArgs e) => System.Diagnostics.Process.Start(linkUpdate);
                            }
                        }
                        break;
                    case EAMNotificationMessageType.Warning:
                        {
                            if (!OptionsData.searchWarnings)
                                return;

                            showUI = true;
                            linkUpdate = msg.link;

                            notMessage = new QNA()
                            {
                                Question = "Important warning!",
                                Answer = msg.message,
                                Type = QuestionType.Warning
                            };
                            if (!string.IsNullOrWhiteSpace(linkUpdate))
                            {
                                notMessage.ButtonText = "Open website";
                                notMessage.ButtonImage = Properties.Resources.ic_public_white_36dp;
                                notMessage.Action = (object sender, EventArgs e) => System.Diagnostics.Process.Start(linkUpdate);
                            }
                        }
                        break;
                    case EAMNotificationMessageType.Stop:
                        {
                            if (!OptionsData.deactivateKillswitch)
                                return;

                            foreach (System.Windows.Forms.Button btn in pSideButtons.Controls.OfType<System.Windows.Forms.Button>())
                            {
                                btn.Enabled = false;
                            }
                            pContent.Controls.Clear();

                            linkUpdate = msg.link;

                            notMessage = new QNA()
                            {
                                Question = "Important warning!",
                                Answer = msg.message,
                                Type = QuestionType.Stop
                            };
                            if (!string.IsNullOrWhiteSpace(linkUpdate))
                            {
                                notMessage.ButtonText = "Open website";
                                notMessage.ButtonImage = Properties.Resources.ic_public_white_36dp;
                                notMessage.Action = (object sender, EventArgs e) => System.Diagnostics.Process.Start(linkUpdate);
                            }

                            eleEAMUpdate = new EleQNA(this);
                            eleEAMUpdate.QNA = notMessage;
                            pContent.Controls.Add(eleEAMUpdate);
                            eleEAMUpdate.Location = new Point((pContent.Width - eleEAMUpdate.Width) / 2, (pContent.Height - eleEAMUpdate.Height) / 2);
                            eleEAMUpdate.Anchor = AnchorStyles.None;
                            return;
                        }
                    default:
                        break;
                }

                if (showUI)
                {
                    eleEAMUpdate = new EleQNA(this);
                    eleEAMUpdate.QNA = notMessage;
                    ShowShadowForm(eleEAMUpdate);
                }
            }
            notificationSaveFile.lastCheck = DateTime.Now;
            try
            {
                File.WriteAllBytes(lastNotificationCheckPath, ObjectToByteArray(notificationSaveFile));
            }
            catch
            {
                ShowSnackbar("Failed to save the notification save file.", BunifuSnackbar.MessageTypes.Error, 3000);
            }
        }

        private void ExaltAccountManagerUpdateAvailable()
        {
            lVersion.ForeColor = UseDarkmode ? Color.Orange : Color.DarkOrange;
            pUpdate.Visible = true;

            ShowSnackbar("New EAM-Version available.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 12500);
        }

        private void btnEAMUpdate_Click(object sender, EventArgs e)
        {
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(7500);

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;
                try
                {
                    Task<MK_EAM_General_Services_Lib.General.Responses.EAMReleaseInfoResponse> task = GeneralServicesClient.Instance?.GetEamReleaseInfo();
                    while (!task.IsCompleted)
                    {
                        if (token.IsCancellationRequested)
                        {
                            LogEvent(new MK_EAM_Lib.LogData(
                                "EAM",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Failed to fetch EAM release information."));

                            return;
                        }

                        await Task.Delay(50, token);
                    }
                    MK_EAM_General_Services_Lib.General.Responses.EAMReleaseInfoResponse response = task.Result;
                    if (response == null || response.ReleaseLink == null || response.ReleaseDownloadLink == null)
                    {
                        throw new ArgumentNullException(
                            "Params: " + response == null ?
                            "response" :
                            (response.ReleaseLink == null ?
                                "ReleaseLink" :
                                "" +
                             response.ReleaseDownloadLink == null ?
                                "ReleaseDownloadLink" :
                                ""), "Response from server is NULL or contains a NULL-Value.");
                    }
                    PerformEamUpdateInvoker(response.ReleaseLink, response.ReleaseDownloadLink);
                    return;
                }
                catch (Exception ex)
                {
                    LogEvent(new MK_EAM_Lib.LogData(
                               "EAM",
                               MK_EAM_Lib.LogEventType.APIError,
                               "Failed to fetch EAM release information, update process canceled." + Environment.NewLine + "Exception: " + ex.Message));

                    System.Diagnostics.Process.Start(GITHUB_PROJECT_URL);

                    MessageBox.Show(
                        "The automatic update failed, please download the update on GitHub.",
                        "An error occured",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }), cancellationTokenSource.Token);
        }

        private bool NotificationMessageInvoker(EAMNotificationMessage msg)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<EAMNotificationMessage, bool>)NotificationMessageInvoker, msg);

            NotificationMessage(msg);

            return false;
        }

        #region LoadServerData

        private void LoadServerData()
        {
            try
            {
                if (File.Exists(serverCollectionPath))
                    serverData = (ServerDataCollection)ByteArrayToObject(File.ReadAllBytes(serverCollectionPath));
            }
            catch
            {
                LogEvent(new LogData(-1, "EAM", LogEventType.EAMError, $"Failed to load server-data."));
                ShowSnackbar("Failed to load server-data, please refresh an account to get new data from deca.", BunifuSnackbar.MessageTypes.Warning, 10000);
            }
        }

        #endregion

        #region LoadAccountInfos

        private void LoadAccountInfos(string path, bool performSaveAfter = false)
        {
            try
            {
                loading = true;

                try
                {
                    AccountSaveFile saveFile = (AccountSaveFile)ByteArrayToObject(File.ReadAllBytes(path));
                    accounts = new BindingList<MK_EAM_Lib.AccountInfo>(AccountSaveFile.Decrypt(saveFile).OrderBy(a => a.orderID).ToList());
                }
                catch
                {
                    LogEvent(new LogData(-1, "EAM", LogEventType.EAMError, $"Failed to decrypt accounts."));
                    LogButtonBlink();
#pragma warning disable CS0612 

                    byte[] data = File.ReadAllBytes(path);
                    AesCryptographyService acs = new AesCryptographyService();
                    List<ExaltAccountManager.AccountInfo> accs = (List<ExaltAccountManager.AccountInfo>)ByteArrayToObject(acs.Decrypt(data));
                    accounts = new BindingList<MK_EAM_Lib.AccountInfo>();
                    for (int i = 0; i < accs.Count; i++)
                        accounts.Add(ExaltAccountManager.AccountInfo.Convert(accs[i]));
#pragma warning restore CS0612 

                    LogEvent(new LogData(-1, "EAM", LogEventType.AccountInfo, $"Found an old save file, converting it."));
                    if (!performSaveAfter)
                        File.Delete(path);
                    SaveAccounts();
                }

                bool foundOld = false;
                if (accounts.Min(a => a.orderID) <= 0)
                {
                    int maxID = accounts.Max(a => a.orderID);
                    maxID = maxID < 0 ? 0 : maxID;
                    for (int i = 0; i < accounts.Count; i++)
                    {
                        if (accounts[i].orderID <= 0)
                        {
                            accounts[i].orderID = ++maxID;
                            foundOld = true;
                        }
                    }
                    accounts = new BindingList<MK_EAM_Lib.AccountInfo>(accounts.OrderBy(a => a.orderID).ToList());
                }

                loading = false;

                if (foundOld || performSaveAfter)
                    SaveAccounts();
            }
            catch (Exception e) { string ex = e.Message; }
        }

        #endregion

        #region SaveAccounts

        public bool SaveAccounts()
        {
            try
            {
                AccountSaveFile saveFile = AccountSaveFile.Encrypt(new AccountSaveFile(), ObjectToByteArray(accounts.ToList() as List<MK_EAM_Lib.AccountInfo>));
                if (saveFile != null && string.IsNullOrEmpty(saveFile.error))
                {
                    File.WriteAllBytes(accountsPath, ObjectToByteArray(saveFile));
                    if (lastLogData.ID == -1 || lastLogData.eventType != LogEventType.SaveAccounts || (lastLogData.eventType == LogEventType.SaveAccounts && lastLogData.time < DateTime.Now.AddMinutes(-1)))
                        LogEvent(new LogData(-1, "EAM", LogEventType.SaveAccounts, $"Saving accounts."));
                }
                else
                {
                    LogEvent(new LogData(-1, "EAM", LogEventType.EAMError, $"Failed to encrypt accounts!"));
                    snackbar.Show(this, $"Failed to encrypt accounts!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                    return false;
                }
            }
            catch
            {
                LogEvent(new LogData(-1, "EAM", LogEventType.EAMError, $"Failed to save accounts!"));
                LogButtonBlink();
                ShowSnackbar($"Failed to save accounts!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
                return false;
            }
            return true;
        }

        public void SaveAccountsNeeded()
        {
            if (saveAccountsTimer.Enabled)
                saveAccountsTimer.Stop();

            saveAccountsTimer.Start();
        }

        private void SaveAccountsTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            saveAccountsTimer.Stop();
            SaveAccounts();
        }

        #endregion

        #region SaveOptions

        public void SaveOptions(OptionsData opt, bool showSnackbarNotification = false)
        {
            try
            {
                LogEvent(new LogData(-1, "EAM Options", LogEventType.SaveOptions, $"Saving new options."));
                byte[] data = ObjectToByteArray(opt);
                File.WriteAllBytes(optionsPath, data);

                if (showSnackbarNotification)
                    ShowSnackbar($"Options saved successfully!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000);
            }
            catch
            {
                LogEvent(new LogData(-1, "EAM Options", LogEventType.EAMError, $"Failed to save options!"));
                LogButtonBlink();
                ShowSnackbar($"Failed to save options!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        public void UpdateUIOptionsData()
        {
            if (OptionsData != null)
            {
                uiOptions?.ApplyOptions();
            }
        }

        public void UpdateHasConfigChangesUI(bool state)
        {
            drawConfigChangesIcon = state;

            btnOptions.Invalidate();
        }

        #endregion

        public void UpdateDataGridViewGroup(int index, Color clr)
        {
            if (uiAccounts != null)
            {
                uiAccounts.GetAccountInfo(index).Color = clr;
                uiAccounts.UpdateGroupCell(index);
            }
        }

        public void UpdateAddNewUserGroup(Color clr, bool none = false)
        {
            if (uiAddAccounts != null)
                uiAddAccounts.UpdateGroup(clr, none);
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
                    lastLogData = data;
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
                    lastLogData = data;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
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
                    using (var sha1 = System.Security.Cryptography.SHA1.Create())
                    {
                        ret = string.Join("", sha1.ComputeHash(Encoding.UTF8.GetBytes(concatStr)).Select(b => b.ToString("x2")));
                    }
                }
                catch
                {
                    LogEvent(new LogData(-1, "EAM", LogEventType.EAMError, $"Failed to get the DeviceUniqueIdentifier!"));
                    ShowSnackbar("Failed to get the DeviceUniqueIdentifier!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
                    LogButtonBlink();
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
                    LogEvent(new LogData(-1, "EAM", LogEventType.EAMError, $"Failed to load HWID from file, using alternative Methode!"));
                    LogButtonBlink();
                    return GetDeviceUniqueIdentifier(true);
                }
            }

            return ret;
        }

        public string GetAPIClientIdHash(bool isForAnalytics = true)
        {
            if (isForAnalytics && OptionsData.analyticsOptions.Anonymization)
                return "--ANONYMIZED--";

            return QuickHash(GetDeviceUniqueIdentifier(true) + System.Security.Principal.WindowsIdentity.GetCurrent().User.Value);
        }

        public string GetAnalyticsEmailHash(string email) => QuickHash(email);
        private string QuickHash(string secret)
        {
            using (var md5 = MD5.Create())
            {
                var secretBytes = Encoding.UTF8.GetBytes(secret);
                var secretHash = md5.ComputeHash(secretBytes);
                return new System.Numerics.BigInteger(secretHash.Reverse().ToArray()).ToString("x2");
            }
        }

        public string GetAccountStatsFilename(string email)
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

        public bool ShowSnackbar(string message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes msgType, int duration = 3000)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<string, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes, int, bool>)ShowSnackbar, message, msgType, duration);

            snackbar.Show(this, message, msgType, duration, null, SnackbarPosition);
            return false;
        }

        public void CloseAllSnackbars() => snackbar.Close();

        public bool AddAccount(MK_EAM_Lib.AccountInfo info)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<MK_EAM_Lib.AccountInfo, bool>)AddAccount, info);

            accounts.Add(info);

            SaveAndUpdateAccounts();

            if (!OptionsData.analyticsOptions.OptOut)
            {
                AnalyticsClient.Instance?.UpdateAmountOfAccounts(accounts.Count);
            }

            return false;
        }

        public void SaveAndUpdateAccounts()
        {
            SaveAccounts();

            if (uiAccounts != null)
            {
                uiAccounts.UpdateDatagridView();
            }
        }

        public void UpdateSideAccountUI()
        {
            if (uiAccounts != null)
            {
                uiAccounts.UpdateAccountToUI();
                uiAccounts.UpdateToggleToggleValues();
            }
        }

        public void SetActiveVaultPeekerAccount(string email, bool state)
        {
            if (state)
                uiAccounts.AddActiveVaultPeekerAccount(email);
            else
                uiAccounts.RemoveActiveVaultPeekerAccount(email);
        }

        public void LogButtonBlink()
        {
            btnLogs.BackColor = Color.Crimson;
            timerLogBlink.Start();
        }

        private void timerLogBlink_Tick(object sender, EventArgs e)
        {
            btnLogs.BackColor = pSideButtons.BackColor;
            timerLogBlink.Stop();
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;

            if (!OptionsData.analyticsOptions.OptOut)
            {
                _ = AnalyticsClient.Instance?.EndSeesion();
            }

            Environment.Exit(0);
        }

        private void pbClose_MouseDown(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Red;


        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_24dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = pTop.BackColor;
            pbClose.Image = UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
        }

        private void pbClose_MouseUp(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Crimson;

        #endregion

        #region Button Minimize

        private void pbMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private void pbMinimize_MouseDown(object sender, MouseEventArgs e) => pbMinimize.BackColor = UseDarkmode ? Color.FromArgb(200, 75, 75, 75) : Color.FromArgb(75, 50, 50, 50);
        private void pbMinimize_MouseUp(object sender, MouseEventArgs e) => pbMinimize.BackColor = UseDarkmode ? Color.FromArgb(125, 75, 75, 75) : Color.FromArgb(50, 50, 50, 50);
        private void pbMinimize_MouseEnter(object sender, EventArgs e) => pbMinimize.BackColor = UseDarkmode ? Color.FromArgb(125, 75, 75, 75) : Color.FromArgb(50, 50, 50, 50);
        private void pbMinimize_MouseLeave(object sender, EventArgs e) => pbMinimize.BackColor = pTop.BackColor;

        #endregion

        #region Menu Buttons

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Accounts)
            {
                pSideBar.Top = (sender as Control).Top + 5;
                pContent.Controls.Clear();
                pContent.Controls.Add(uiAccounts);

                uiState = UIState.Accounts;
            }
            lHeaderEAM.Focus();
        }

        private void btnNews_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.News)
            {
                pSideBar.Top = (sender as Control).Top + 5;

                if (uiEAMNews == null)
                    uiEAMNews = new UIEAMNews(this) { Dock = DockStyle.Fill };

                pContent.Controls.Clear();
                pContent.Controls.Add(uiEAMNews);
                SaveNewsViewed();

                uiState = UIState.News;
            }
            lHeaderEAM.Focus();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.AddAccount)
            {
                pSideBar.Top = (sender as Control).Top + 5;

                if (uiAddAccounts == null)
                    uiAddAccounts = new UIAddAccount(this);

                pContent.Controls.Clear();
                pContent.Controls.Add(uiAddAccounts);

                uiState = UIState.AddAccount;
            }
            lHeaderEAM.Focus();
        }

        private void btnModules_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Modules)
            {
                pSideBar.Top = (sender as Control).Top + 5;

                if (uiModules == null)
                    uiModules = new UIModules(this) { Dock = DockStyle.Fill };

                pContent.Controls.Clear();
                pContent.Controls.Add(uiModules);
                uiModules.Dock = DockStyle.Fill;

                uiState = UIState.Modules;
            }
            lHeaderEAM.Focus();
        }

        public void btnOptions_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Options)
            {
                pSideBar.Top = btnOptions.Top + 5;

                if (uiOptions == null)
                    uiOptions = new UIOptions(this) { Dock = DockStyle.Fill };

                pContent.Controls.Clear();
                pContent.Controls.Add(uiOptions);

                uiState = UIState.Options;
            }
            lHeaderEAM.Focus();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Help)
            {
                pSideBar.Top = (sender as Control).Top + 5;

                if (uiHelp == null)
                    uiHelp = new UIHelp(this) { Dock = DockStyle.Fill };

                pContent.Controls.Clear();
                pContent.Controls.Add(uiHelp);

                uiState = UIState.Help;
            }
            lHeaderEAM.Focus();
        }

        public void btnLogs_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Logs)
            {
                pSideBar.Top = (sender as Control).Top + 5;

                if (uiLogs == null)
                    uiLogs = new UILogs(this) { Dock = DockStyle.Fill };

                pContent.Controls.Clear();
                pContent.Controls.Add(uiLogs);

                uiState = UIState.Logs;
            }
            lHeaderEAM.Focus();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.AboutEAM)
            {
                pSideBar.Top = (sender as Control).Top + 5;

                if (uiAbout == null)
                    uiAbout = new UIAbout(this) { Dock = DockStyle.Fill };

                pContent.Controls.Clear();
                pContent.Controls.Add(uiAbout);

                uiState = UIState.AboutEAM;
            }
            lHeaderEAM.Focus();
        }

        public void btnGameUpdater_Click(object sender, EventArgs e)
        {
            if (uiState != UIState.Updater)
            {
                if (btnGameUpdater.Visible)
                {
                    uiState = UIState.Updater;

                    pSideBar.Top = btnGameUpdater.Top + 5;
                }

                if (eleGameUpdater == null)
                    eleGameUpdater = new EleGameUpdater(this);

                ShowShadowForm(eleGameUpdater);
                DiscordHelper.UpdateMenu(DiscordHelper.Menu.Updater);
                DiscordHelper.ApplyPresence();
            }
            lHeaderEAM.Focus();
        }

        private void btnAccounts_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Accounts)
                btnAccounts.Image = useDarkmode ? Properties.Resources.ic_people_white_24dp : Properties.Resources.ic_people_black_24dp;
        }

        private void btnAccounts_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Accounts)
                btnAccounts.Image = useDarkmode ? Properties.Resources.ic_people_outline_white_24dp : Properties.Resources.ic_people_outline_black_24dp;
        }

        private void btnNews_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.News)
                btnNews.Image = useDarkmode ? Properties.Resources.news_white_24px : Properties.Resources.news_black_24px;
        }

        private void btnNews_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.News)
                btnNews.Image = useDarkmode ? Properties.Resources.news_outline_white_24px : Properties.Resources.news_outline_black_24px;
        }

        private void btnAddAccount_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.AddAccount)
                btnAddAccount.Image = useDarkmode ? Properties.Resources.add_user_white_24px : Properties.Resources.add_user_24px;
        }

        private void btnAddAccount_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.AddAccount)
                btnAddAccount.Image = useDarkmode ? Properties.Resources.add_user_white_outline_24px : Properties.Resources.add_user_outline_24px;
        }

        private void btnModules_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Modules)
                btnModules.Image = useDarkmode ? Properties.Resources.dashboard_layout_white_24px : Properties.Resources.dashboard_layout_24px;
        }

        private void btnModules_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Modules)
                btnModules.Image = useDarkmode ? Properties.Resources.dashboard_layout_outline_white_24px : Properties.Resources.dashboard_layout_outline_24px;
        }

        private void btnOptions_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Options)
                btnOptions.Image = useDarkmode ? Properties.Resources.settings_white_24px : Properties.Resources.settings_24px;
        }

        private void btnOptions_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Options)
                btnOptions.Image = useDarkmode ? Properties.Resources.settings_outline_white_24px : Properties.Resources.settings_outline_24px;
        }

        private void btnHelp_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Help)
                btnHelp.Image = useDarkmode ? Properties.Resources.ic_help_white_24dp : Properties.Resources.ic_help_black_24dp;
        }

        private void btnHelp_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Help)
                btnHelp.Image = useDarkmode ? Properties.Resources.ic_help_outline_white_24dp : Properties.Resources.ic_help_outline_black_24dp;
        }

        private void btnLogs_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Logs)
                btnLogs.Image = useDarkmode ? Properties.Resources.activity_history_white_24px : Properties.Resources.activity_history_24px;
        }

        private void btnLogs_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.Logs)
                btnLogs.Image = useDarkmode ? Properties.Resources.activity_history_outline_white_24px : Properties.Resources.activity_history_outline_24px;
        }

        private void btnAbout_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.AboutEAM)
                btnAbout.Image = useDarkmode ? Properties.Resources.ic_info_white_24dp : Properties.Resources.ic_info_black_24dp;
        }

        private void btnAbout_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.AboutEAM)
                btnAbout.Image = useDarkmode ? Properties.Resources.ic_info_outline_white_24dp : Properties.Resources.ic_info_outline_black_24dp;
        }

        private void btnGameUpdater_MouseEnter(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.AboutEAM)
                btnGameUpdater.Image = useDarkmode ? Properties.Resources.available_updates_white_24px : Properties.Resources.available_updates_24px;
        }

        private void btnGameUpdater_MouseLeave(object sender, EventArgs e)
        {
            if (uiStateVal != UIState.AboutEAM)
                btnGameUpdater.Image = useDarkmode ? Properties.Resources.update_left_rotation_white_24px : Properties.Resources.update_left_rotation_24px;
        }

        #endregion

        public void btnSwitchDesign_Click(object sender, EventArgs e)
        {
            UseDarkmode = !UseDarkmode;

            OptionsData = new OptionsData()
            {
                exePath = OptionsData.exePath,
                closeAfterConnection = OptionsData.closeAfterConnection,

                serverToJoin = OptionsData.serverToJoin,

                searchRotmgUpdates = OptionsData.searchRotmgUpdates,
                searchUpdateNotification = OptionsData.searchUpdateNotification,
                searchWarnings = OptionsData.searchWarnings,
                deactivateKillswitch = OptionsData.deactivateKillswitch,

                useDarkmode = UseDarkmode
            };

            lHeaderEAM.Focus();

            SaveOptions(OptionsData);
        }

        private void timerLoadUI_Tick(object sender, EventArgs e)
        {
            timerLoadUI.Stop();

            if (uiAddAccounts == null)
                uiAddAccounts = new UIAddAccount(this);

            if (uiModules == null)
                uiModules = new UIModules(this) { Dock = DockStyle.Fill };

            if (uiOptions == null)
                uiOptions = new UIOptions(this) { Dock = DockStyle.Fill };

            if (OptionsData.searchRotmgUpdates)
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
                            checkForUpdate = true;
                    }
                    catch
                    {
                        checkForUpdate = true;
                    }
                }
                else
                    checkForUpdate = true;

                if (checkForUpdate)
                    GameUpdater.Instance.CheckForUpdate();
            }

            DiscordHelper.UpdateMenu(DiscordHelper.Menu.Accounts);
            DiscordHelper.ApplyPresence();

            var usr = DiscordHelper.GetUser();

            timerDiscordUpdater.Start();

            if (uiEAMNews == null)
                uiEAMNews = new UIEAMNews(this) { Dock = DockStyle.Fill };

            //Check if privacy policy exists & create if not
            if (!File.Exists(privacyPolicyPath) || (DateTime.Now - File.GetLastWriteTime(privacyPolicyPath)) > TimeSpan.FromDays(3))
            {
                #region Privacy Policy

                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Cancel();
                }

                cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(7500);

                ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
                {
                    CancellationToken token = (CancellationToken)obj;

                    try
                    {

                        Task<MK_EAM_General_Services_Lib.General.Responses.GetFileResponse> task =
                            GeneralServicesClient.Instance?.GetPrivacyPolicy();

                        while (!task.IsCompleted)
                        {
                            if (token.IsCancellationRequested)
                            {
                                LogEvent(new MK_EAM_Lib.LogData(
                                    "EAM",
                                    MK_EAM_Lib.LogEventType.APIError,
                                    "Failed to request privacy policy."));

                                return;
                            }

                            await Task.Delay(50, token);
                        }
                        MK_EAM_General_Services_Lib.General.Responses.GetFileResponse result = task.Result;

                        if (result != null && result.data != null)
                        {
                            File.WriteAllBytes(privacyPolicyPath, result.data);
                        }
                        else
                        {
                            LogEvent(new MK_EAM_Lib.LogData(
                                    "EAM",
                                    MK_EAM_Lib.LogEventType.APIError,
                                    "Failed to request privacy policy."));
                        }
                    }
                    catch
                    {
                        LogEvent(new MK_EAM_Lib.LogData(
                                    "EAM",
                                    MK_EAM_Lib.LogEventType.APIError,
                                    "Failed to request privacy policy."));
                    }
                }), cancellationTokenSource.Token);

                #endregion
            }

            timerLoadUI.Dispose();
            lHeaderEAM.Focus();
        }

        public void SaveNewsViewed()
        {
            LastNewsViewed = DateTime.Now;
            try
            {
                File.WriteAllText(pathNews, LastNewsViewed.Ticks.ToString());
                uiEAMNews?.UpdateHasNewNews();
            }
            catch { }
        }

        private bool UpdateRequired()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)UpdateRequired);

            btnGameUpdater.Visible = GameUpdater.Instance.UpdateRequired;

            if (GameUpdater.Instance.UpdateRequired)
            {
                pSideBar.Top += btnGameUpdater.Height + 2;

                ShowSnackbar("Game-update available.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 12500);
            }
            return false;
        }

        private void UpdateRequiredInvoker(object sender, EventArgs e) => UpdateRequired();

        public void AddContrtolToPContent(Control ctr, int newUIState)
        {
            uiState = (UIState)newUIState;

            pContent.Controls.Clear();
            pContent.Controls.Add(ctr);
        }

        public void OpenImporter() => uiModules.OpenImporter();

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
                    if (form == this || form.GetType().Name.Equals("FrmMain")) // Except a few ones that should not
                        continue;
                    form.Location = new Point(
                      form.Location.X + Location.X - m_PreviousLocation.X,
                      form.Location.Y + Location.Y - m_PreviousLocation.Y
                    );
                }
            }
            m_PreviousLocation = Location;
        }

        public DialogResult ShowShadowFormDialog(Form form)
        {
            if (frmShadowHost == null)
            {
                frmShadowHost = new FrmShadowHost(this);
                frmShadowHost.Size = new Size(this.Size.Width - 1, this.Size.Height - 25);
                frmShadowHost.Location = new Point(this.Left, this.Top + 24);
                frmShadowHost.Owner = this;
                frmShadowHost.TopLevel = true;
            }
            else
                frmShadowHost.RemoveControl();

            frmShadowHost.Show();
            DialogResult result = frmShadowHost.ShowFormDialog(form);

            RemoveShadowForm();
            return result;
        }

        public void ShowShadowForm(Control ctr)
        {
            if (frmShadowHost == null)
            {
                frmShadowHost = new FrmShadowHost(this);
                frmShadowHost.Size = new Size(this.Size.Width - 1, this.Size.Height - 25);
                frmShadowHost.Location = new Point(this.Left, this.Top + 24);
                frmShadowHost.Owner = this;
                frmShadowHost.TopLevel = true;
            }
            else
                frmShadowHost.RemoveControl();

            frmShadowHost.ShowControl(ctr);
            frmShadowHost.Show();
        }

        public void RemoveShadowForm()
        {
            if (uiState == UIState.Updater && GameUpdater.Instance.UpdateProgress > 0)
                return;

            if (frmShadowHost != null)
            {
                frmShadowHost.RemoveControl();
                frmShadowHost.Hide();
                this.BringToFront();

                if (uiState == UIState.Updater)
                {
                    uiState = lastUiState;

                    switch (uiState)
                    {
                        case UIState.Accounts:
                            pSideBar.Top = btnAccounts.Top + 5;
                            break;
                        case UIState.News:
                            pSideBar.Top = btnNews.Top + 5;
                            break;
                        case UIState.AddAccount:
                            pSideBar.Top = btnAddAccount.Top + 5;
                            break;
                        case UIState.Modules:
                            pSideBar.Top = btnModules.Top + 5;
                            break;
                        case UIState.Options:
                            pSideBar.Top = btnOptions.Top + 5;
                            break;
                        case UIState.Help:
                            pSideBar.Top = btnHelp.Top + 5;
                            break;
                        case UIState.Logs:
                            pSideBar.Top = btnLogs.Top + 5;
                            break;
                        case UIState.AboutEAM:
                            pSideBar.Top = btnAbout.Top + 5;
                            break;
                        case UIState.Updater:
                            uiState = lastUiState = UIState.Accounts;
                            pSideBar.Top = btnAccounts.Top + 5;
                            break;
                        default:
                            break;
                    }
                }
                uiState = uiStateVal;
            }
        }

        public void SwitchLlamaState(bool showLlama) => pbHeader.Image = showLlama ? Properties.Resources.llama : Properties.Resources.ic_account_balance_wallet_white_48dp;

        public void ShowEamLogoGif(string _url, Action<object, EventArgs> action)
        {
            string url = API_BASE_URL + _url;

            PictureBox pbDev = new PictureBox()
            {
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                Visible = false,
                Tag = "Dev"
            };
            pHeader.Controls.Add(pbDev);
            pbDev.BringToFront();

            pbDev.LoadAsync(url);
            pbDev.LoadCompleted += Execute;

            void Execute(object sender, EventArgs e)
            {
                pbDev.Visible = true;
                action?.Invoke(this, EventArgs.Empty);
                pbDev.LoadCompleted -= Execute;
            }
        }

        public void HideEamLogoGif()
        {
            PictureBox pb = pHeader.Controls.OfType<PictureBox>()
                                .Where(p => p.Tag.Equals("Dev"))
                                .FirstOrDefault();
            if (pb != null)
            {
                pb.Visible = false;
                pHeader.Controls.Remove(pb);
                pb.Image = null;
                pb.Dispose();
                pb = null;
            }
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (frmShadowHost != null)
            {
                frmShadowHost.Size = new Size(this.Size.Width - 1, this.Size.Height - 25);
                frmShadowHost.Invalidate();
            }
        }
        private enum UIState
        {
            Accounts = 0,
            AddAccount = 1,
            Modules = 2,
            Options = 3,
            Help = 4,
            Logs = 5,
            AboutEAM = 6,
            Updater = 7,
            Changelog = 8,
            TokenViewer = 9,
            ImportExport = 10,
            DailyLogin = 11,
            DailyNotifications = 12,
            News = 13,
        }

        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(ColorScheme.GetColorDef(useDarkmode)))
            {
                e.Graphics.DrawLine(p, 0, this.Height - 1, this.Width, this.Height - 1);
            }
        }

        private void pBottom_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(ColorScheme.GetColorSecond(useDarkmode)))
            {
                e.Graphics.DrawLine(p, 0, pBottom.Height - 1, pBottom.Width, pBottom.Height - 1);
            }
        }

        private void lVersion_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(ColorScheme.GetColorSecond(useDarkmode)))
            {
                e.Graphics.DrawLine(p, 0, lVersion.Height - 1, lVersion.Width, lVersion.Height - 1);
            }
        }

        private void timerDiscordUpdater_Tick(object sender, EventArgs e)
        {
            DiscordHelper.ApplyPresence();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            if (!OptionsData.analyticsOptions.OptOut)
            {
                _ = AnalyticsClient.Instance?.EndSeesion().Result;
            }
        }

        private void btnOptions_Paint(object sender, PaintEventArgs e)
        {
            if (!drawConfigChangesIcon)
                return;

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen pColor = new Pen(Color.FromArgb(98, 0, 238)))
            using (Pen pFont = new Pen(ColorScheme.GetColorFont(useDarkmode)))
            {
                e.Graphics.FillEllipse(pColor.Brush, btnOptions.Width - 52f, 6f, 26.5f, 26.5f);
                e.Graphics.DrawImage(Properties.Resources.ic_save_white_18dp, btnOptions.Width - 47, 11, 18, 18);
            }
        }

        private void btnNews_Paint(object sender, PaintEventArgs e)
        {
            if (!HasNewNews)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen pColor = new Pen(Color.Crimson))
            {
                e.Graphics.FillEllipse(pColor.Brush, 33f, 26f, 9f, 9f);
            }
        }
    }
}
