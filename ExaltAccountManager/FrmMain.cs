using Bunifu.UI.WinForms;
using ExaltAccountManager.UI;
using ExaltAccountManager.UI.Elements;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmMain : Form
    {
        public readonly Version version = new Version(3, 0, 0);
        public event EventHandler ThemeChanged;

        private System.Timers.Timer saveAccountsTimer;
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
        LogData lastLogData = new LogData(-1, "", LogEventType.EAMError, "") { time = new DateTime() };

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
        public NotificationOptions notOpt = new NotificationOptions();
        private EAMNotificationMessageSaveFile notificationSaveFile = new EAMNotificationMessageSaveFile();
        private GameUpdater gameUpdater { get; set; }

        private UIAccounts uiAccounts;
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
                        break;
                    case UIState.AddAccount:
                        btnAddAccount.Image = useDarkmode ? Properties.Resources.add_user_white_24px : Properties.Resources.add_user_24px;
                        lTitle.Text = "Add Account";
                        break;
                    case UIState.Modules:
                        btnModules.Image = useDarkmode ? Properties.Resources.dashboard_layout_white_24px : Properties.Resources.dashboard_layout_24px;
                        lTitle.Text = "Modules";
                        break;
                    case UIState.Options:
                        btnOptions.Image = useDarkmode ? Properties.Resources.settings_white_24px : Properties.Resources.settings_24px;
                        lTitle.Text = "Options";
                        break;
                    case UIState.Help:
                        btnHelp.Image = useDarkmode ? Properties.Resources.ic_help_white_24dp : Properties.Resources.ic_help_black_24dp;
                        lTitle.Text = "Help";
                        break;
                    case UIState.Logs:
                        btnLogs.Image = useDarkmode ? Properties.Resources.activity_history_white_24px : Properties.Resources.activity_history_24px;
                        lTitle.Text = "Logs";
                        break;
                    case UIState.AboutEAM:
                        btnAbout.Image = useDarkmode ? Properties.Resources.ic_info_white_24dp : Properties.Resources.ic_info_black_24dp;
                        lTitle.Text = "About Exalt Account Manager";
                        break;
                    case UIState.Updater:
                        lTitle.Text = "Updater";
                        break;
                    case UIState.Changelog:
                        lTitle.Text = "Changelog";
                        break;
                    case UIState.TokenViewer:
                        lTitle.Text = "Token Viewer";
                        break;
                    case UIState.ImportExport:
                        lTitle.Text = "Im- & Export";
                        break;
                    case UIState.DailyLogin:
                        lTitle.Text = "Daily logins";
                        break;
                    case UIState.DailyNotifications:
                        lTitle.Text = "Daily Login notification-settings";
                        break;
                    default:
                        this.MinimumSize = defaultMinimumsize;
                        break;
                }

                #endregion

                pSideBar.Visible = !(uiState == UIState.Changelog || uiState == UIState.TokenViewer || uiState == UIState.ImportExport || uiState == UIState.DailyLogin || uiState == UIState.DailyNotifications);
            }
        }
        private UIState uiStateVal = UIState.Accounts;
        private UIState lastUiState = UIState.Accounts;

        public Bunifu.UI.WinForms.BunifuSnackbar.Positions SnackbarPosition { get; set; } = Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight;

        #region Paths

        //public string exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe");

        public static string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        public string optionsPath = Path.Combine(saveFilePath, "EAM.options");
        public string accountsPath = Path.Combine(saveFilePath, "EAM.accounts");
        public string accountOrdersPath = Path.Combine(saveFilePath, "EAM.accountOrders");
        public string dailyLoginsPath = Path.Combine(saveFilePath, "EAM.DailyLoginsV2");
        public string notificationOptionsPath = Path.Combine(saveFilePath, "EAM.NotificationOptions");
        public string serverCollectionPath = Path.Combine(saveFilePath, "EAM.ServerCollection");
        public string accountStatsPath = Path.Combine(saveFilePath, "Stats");
        public string pathLogs = Path.Combine(saveFilePath, "EAM.Logs");
        public string lastUpdateCheckPath = Path.Combine(saveFilePath, "EAM.LastUpdateCheck");
        public string lastNotificationCheckPath = Path.Combine(saveFilePath, "EAM.LastNotificationCheck");
        public string forceHWIDFilePath = Path.Combine(saveFilePath, "EAM.HWID");
        public string itemsSaveFilePath = Path.Combine(saveFilePath, "EAM.ItemsSaveFile");
        public string activeVaultPeekerAccountsPath = Path.Combine(saveFilePath, "EAM.ActiveVaultPeekerAccounts");
        public string getClientHWIDToolPath = Path.Combine(Application.StartupPath, "EAM_GetClientHWID");
        public string pingCheckerExePath = Path.Combine(Application.StartupPath, "EAM PingChecker.exe");
        public string statisticsExePath = Path.Combine(Application.StartupPath, "EAM Statistics.exe");
        public string vaultPeekerExePath = Path.Combine(Application.StartupPath, "EAM Vault Peeker.exe");
        public string dailyServiceExePath = Path.Combine(Application.StartupPath, "DailyService", "EAM Daily Login Service.exe");

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

        private string linkUpdate = string.Empty;

        public FrmMain()
        {
            InitializeComponent();

            defaultMinimumsize = this.MinimumSize = new Size(this.MinimumSize.Width, 576);
            lVersion.Text = $"EAM v{version} by Maik8";

            bool isNewInstall = false;

            LoadFlags();

            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
                isNewInstall = true;
            }

            isNewInstall = (isNewInstall || (!File.Exists(accountsPath) && !File.Exists(optionsPath)));

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
                            snackbarPosition = 8
                        };
                        File.WriteAllBytes(optionsPath, ObjectToByteArray(OptionsData));
                    }
                    catch { }
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

        private void FrmMain_Load(object sender, EventArgs e)
        {
            timerLoadUI.Start();
            gameUpdater = new GameUpdater(OptionsData.exePath, lastUpdateCheckPath);
            GameUpdater.Instance.OnUpdateRequired += UpdateRequiredInvoker;
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
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
                BackColor = UseDarkmode ? Color.FromArgb(8, 8, 8) : Color.White,
                ActionBorderColor = UseDarkmode ? Color.FromArgb(15, 15, 15) : Color.White,
                BorderColor = UseDarkmode ? Color.FromArgb(15, 15, 15) : Color.White,
                ActionForeColor = UseDarkmode ? Color.FromArgb(170, 170, 170) : Color.Black,
                ForeColor = UseDarkmode ? Color.FromArgb(170, 170, 170) : Color.Black,
                CloseIconColor = Color.FromArgb(246, 255, 237)
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
        }

        private void NotificationMessage(EAMNotificationMessage msg)
        {
            if (notificationSaveFile == null)
                notificationSaveFile = new EAMNotificationMessageSaveFile();

            if (!notificationSaveFile.knownIDs.Contains(msg.id))
            {
                QNA notMessage = new QNA();
                bool showUI = false;
                notificationSaveFile.forceCheck = msg.forceShow;
                notificationSaveFile.lastCheckWasStop = msg.type == EAMNotificationMessageType.Stop;
                notificationSaveFile.knownIDs.Add(msg.id);
                switch (msg.type)
                {
                    case EAMNotificationMessageType.None:
                        break;
                    case EAMNotificationMessageType.UpdateAvailable:
                        {
                            if (!OptionsData.searchUpdateNotification)
                                return;
                            showUI = true;
                            linkUpdate = isMPGHVersion ? msg.linkM : msg.link;
                            notMessage = new QNA()
                            {
                                Question = "Exalt Account Manager Update available",
                                Answer = msg.message,
                                ButtonText = isMPGHVersion ? "Show release on MPGH" : "Show release on Github",
                                ButtonImage = Properties.Resources.update_left_rotation_white_24px,
                                Type = QuestionType.Update,
                                Action = (object sender, EventArgs e) => System.Diagnostics.Process.Start(linkUpdate)
                            };
                            GameUpdateAvailable();

                            notificationSaveFile.forceCheck = true;
                        }
                        break;
                    case EAMNotificationMessageType.Message:
                        {
                            if (!OptionsData.searchWarnings)
                                return;

                            showUI = true;
                            linkUpdate = isMPGHVersion ? msg.linkM : msg.link;

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
                            linkUpdate = isMPGHVersion ? msg.linkM : msg.link;

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

                            foreach (Button btn in pSideButtons.Controls.OfType<Button>())
                            {
                                btn.Enabled = false;
                            }
                            pContent.Controls.Clear();

                            linkUpdate = isMPGHVersion ? msg.linkM : msg.link;

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

        private void GameUpdateAvailable()
        {
            lVersion.ForeColor = UseDarkmode ? Color.Orange : Color.DarkOrange;
            pUpdate.Visible = true;
        }

        private void btnEAMUpdate_Click(object sender, EventArgs e)
        {
            if (eleEAMUpdate != null)
                eleEAMUpdate = new EleQNA(this);
            ShowShadowForm(eleEAMUpdate);
        }

        private bool NotificationMessageInvoker(EAMNotificationMessage msg)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<EAMNotificationMessage, bool>)NotificationMessageInvoker, msg);

            NotificationMessage(msg);

            return false;
        }

        #region Load Flags

        private void LoadFlags()
        {

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
                catch
                {
                    LogEvent(new LogData(-1, "EAM", LogEventType.EAMError, $"Failed to load flags."));
                }
            }

        }

        #endregion

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

                //if (msg != null && msg.type == EAMNotificationMessageType.UpdateAvailable)
                //{
                //    lUpdateAvailable.Visible = true;
                //    lVersion.ForeColor = useDarkmode ? Color.Orange : Color.DarkOrange;
                //    notificationSaveFile.forceCheck = true;
                //    updateLink = isMPGHVersion ? msg.linkM : msg.link;
                //}
            }
            catch
            {
                LogEvent(new LogData(-1, "EAM Options", LogEventType.EAMError, $"Failed to save options!"));
                LogButtonBlink();
                ShowSnackbar($"Failed to save options!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
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

            snackbar.Show(this, message, msgType, duration, "X", SnackbarPosition);
            //snackbar.Show(this, message, msgType, duration, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);

            return false;
        }

        public void CloseAllSnackbars() => snackbar.Close();

        public bool AddAccount(MK_EAM_Lib.AccountInfo info)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<MK_EAM_Lib.AccountInfo, bool>)AddAccount, info);

            accounts.Add(info);

            SaveAndUpdateAccounts();

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

        private void pbClose_Click(object sender, EventArgs e) => Environment.Exit(0);

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

        private void timerLaodUI_Tick(object sender, EventArgs e)
        {
            timerLoadUI.Stop();

            if (uiAddAccounts == null)
                uiAddAccounts = new UIAddAccount(this);

            if (uiModules == null)
                uiModules = new UIModules(this);

            if (uiOptions == null)
                uiOptions = new UIOptions(this);

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

            timerLoadUI.Dispose();
            lHeaderEAM.Focus();
        }

        private bool UpdateRequired()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)UpdateRequired);

            btnGameUpdater.Visible = GameUpdater.Instance.UpdateRequired;
            if (GameUpdater.Instance.UpdateRequired)
                ShowSnackbar("Game-update available.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 12500);

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
            }
        }

        public void SwitchLlamaState(bool showLlama) => pbHeader.Image = showLlama ? Properties.Resources.llama : Properties.Resources.ic_account_balance_wallet_white_48dp;

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
        }
    }
}
