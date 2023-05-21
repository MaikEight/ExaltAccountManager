using MK_EAM_Analytics;
using MK_EAM_Captcha_Solver_UI_Lib;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public sealed partial class UIAccounts : UserControl
    {
        private FrmMain frm;

        private bool isInit = true;

        private ColorChanger colorChanger;
        private Elements.EleDeleteAccount eleDeleteAccount;

        private Dictionary<MK_EAM_Lib.AccountInfo, DateTime> dicLockRenewInfoToTime = new Dictionary<MK_EAM_Lib.AccountInfo, DateTime>();
        private Dictionary<MK_EAM_Lib.AccountInfo, Process> dicAccountsToProcesses = new Dictionary<MK_EAM_Lib.AccountInfo, Process>();
        private Dictionary<Process, MK_EAM_Lib.AccountInfo> dicProcessesToAccounts = new Dictionary<Process, MK_EAM_Lib.AccountInfo>();
        public List<string> activeVaultPeekerAccounts { get; internal set; } = new List<string>();

        private BindingSource bindingSource = new BindingSource();

        public UIAccounts(FrmMain _frm)
        {
            InitializeComponent();
            pbFilter.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            frm = _frm;

            colorChanger = new ColorChanger(frm) { Visible = false };
            this.Controls.Add(colorChanger);
            this.Controls.SetChildIndex(colorChanger, 0);

            eleDeleteAccount = new Elements.EleDeleteAccount(frm, this) { Visible = false };
            shadowActions.Controls.Add(eleDeleteAccount);
            eleDeleteAccount.Location = new Point(8, 8);
            eleDeleteAccount.BringToFront();

            try
            {
                activeVaultPeekerAccounts = (List<string>)frm.ByteArrayToObject(System.IO.File.ReadAllBytes(frm.activeVaultPeekerAccountsPath));
            }
            catch { }

            UpdateDatagridView();

            dataGridView.MouseWheel += dataGridView_MouseWheel;

            LoadServers();
            isInit = true;

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);

            timerLoadProcesses_Tick(timerLoadProcesses, null);
            timerLoadProcesses.Start();
        }

        private void UIAccounts_Load(object sender, EventArgs e)
        {
            isInit = true;

            radioSearchForAccount.Checked = true;
            radioSearchForEmail.Checked = !radioSearchForAccount.Checked;
            dropOrder.SelectedIndex = 0;

            if (dataGridView.Rows.Count > 0)
                dataGridView.Rows[0].Selected = true;

            isInit = false;

            this.Controls.Add(pbDragbox);
            pbDragbox.BringToFront();
            pbDragbox.Capture = false;
            pbDragbox.MouseMove += pbDragbox_MouseMove;
            pbDragbox.DragEnter += PbDragbox_DragEnter;

            Task.Run(() => SetToggleToggleValues());
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            FormsUtils.SuspendDrawing(this);

            tbSortQuerry.ResetColors();

            this.BackColor =
            pFilter.BackgroundColor = pToggleMenu.BackgroundColor =
            lAllowDND.BackColor = lOrderType.BackColor = lSearchString.BackColor = lSearchAccountName.BackColor = lSearchEmail.BackColor = lDNDWarning.BackColor =
            lToggleMenu.BackColor = lToggleWarning.BackColor = lToggleDailyLogin.BackColor = lToggleShowInVaultPeeker.BackColor =
            tbSortQuerry.BackColor = tbSortQuerry.FillColor = tbSortQuerry.OnIdleState.FillColor = tbSortQuerry.OnHoverState.FillColor = tbSortQuerry.OnActiveState.FillColor = tbSortQuerry.OnDisabledState.FillColor =
            def;

            lStartGame.ForeColor = lRenewToken.ForeColor = lEditAccount.ForeColor = lDeleteAccount.ForeColor =
            tbSortQuerry.ForeColor = tbSortQuerry.OnActiveState.ForeColor = tbSortQuerry.OnDisabledState.ForeColor = tbSortQuerry.OnHoverState.ForeColor = tbSortQuerry.OnIdleState.ForeColor =
            this.ForeColor = font;

            tbSortQuerry.ResetColors();
            btnCopyAccountName.Image = btnCopyEmail.Image = frm.UseDarkmode ? Properties.Resources.ic_content_copy_white_24dp : Properties.Resources.ic_content_copy_black_24dp;

            btnCopyAccountName.ColorContrastOnHover = btnCopyEmail.ColorContrastOnHover = frm.UseDarkmode ? 40 : 40;
            btnCopyAccountName.ColorContrastOnClick = btnCopyEmail.ColorContrastOnClick = frm.UseDarkmode ? 30 : 30;
            btnCopyAccountName.BackgroundColor = btnCopyEmail.BackgroundColor = frm.UseDarkmode ? second : third;

            pbFilter.Image = frm.UseDarkmode ? Properties.Resources.ic_filter_list_white_24dp : Properties.Resources.ic_filter_list_black_24dp;
            pbToggle.Image = frm.UseDarkmode ? Properties.Resources.toggle_off_White_24px : Properties.Resources.toggle_off_24px;
            pbFilter.BackColor = pbToggle.BackColor =
            bunifuCards.BackColor = pFilterParent.BackColor = pToggleMenuParent.BackColor = second;

            shadowInfos.PanelColor = shadowInfos.BackColor = shadowInfos.PanelColor2 = pAccountName.BackColor = pCopyAccountName.BackColor =
            shadowActions.PanelColor = shadowActions.BackColor = shadowActions.PanelColor2 = pEmail.BackColor = pCopyEmail.BackColor = def;

            shadowInfos.BorderColor = shadowActions.BorderColor = second;
            shadowInfos.ShadowColor = shadowActions.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            scrollbar.BorderColor = frm.UseDarkmode ? second : Color.Silver;
            scrollbar.BackgroundColor = frm.UseDarkmode ? def : third;
            scrollbar.ThumbColor = frm.UseDarkmode ? third : Color.Gray;

            dropServers.BackgroundColor =
            dropOrder.BackgroundColor = def;
            dropServers.ForeColor =
            dropOrder.ForeColor = font;
            dropServers.ItemBackColor =
            dropOrder.ItemBackColor = def;
            dropServers.ItemBorderColor =
            dropOrder.ItemBorderColor =
            dropServers.ItemForeColor =
            dropOrder.ItemForeColor = font;
            dropServers.BorderColor =
            dropOrder.BorderColor = third;
            dropServers.Invalidate();
            dropOrder.Invalidate();

            dataGridView.BackgroundColor = second;
            dataGridView.CurrentTheme.BackColor = frm.UseDarkmode ? Color.FromArgb(77, 10, 173) : Color.FromArgb(107, 40, 203);
            dataGridView.CurrentTheme.GridColor = dataGridView.GridColor = frm.UseDarkmode ? third : Color.WhiteSmoke;

            dataGridView.CurrentTheme.HeaderStyle.BackColor = dataGridView.CurrentTheme.HeaderStyle.SelectionBackColor = dataGridView.HeaderBackColor = frm.UseDarkmode ? Color.FromArgb(77, 10, 173) : Color.FromArgb(107, 40, 203);

            dataGridView.CurrentTheme.RowsStyle.BackColor = frm.UseDarkmode ? Color.FromArgb(126, 65, 214) : Color.FromArgb(176, 127, 246);//78, 12, 174
            dataGridView.CurrentTheme.AlternatingRowsStyle.BackColor = frm.UseDarkmode ? Color.FromArgb(106, 45, 194) : Color.FromArgb(156, 95, 244);

            dataGridView.ApplyTheme(dataGridView.CurrentTheme);

            toolTip.BackColor = def;
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = frm.UseDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);

            toolTip.SetToolTipIcon(btnToggleFilter, frm.UseDarkmode ? Properties.Resources.ic_filter_list_white_24dp : Properties.Resources.ic_filter_list_black_24dp);
            toolTip.SetToolTipIcon(btnToggleToggle, frm.UseDarkmode ? Properties.Resources.toggle_on_white_24px : Properties.Resources.toggle_on_24px);
            toolTip.SetToolTipIcon(btnCopyAccountName, frm.UseDarkmode ? Properties.Resources.ic_content_copy_white_24dp : Properties.Resources.ic_content_copy_black_24dp);
            toolTip.SetToolTipIcon(btnCopyEmail, frm.UseDarkmode ? Properties.Resources.ic_content_copy_white_24dp : Properties.Resources.ic_content_copy_black_24dp);
            toolTip.SetToolTipIcon(btnRenewToken, frm.UseDarkmode ? Properties.Resources.baseline_autorenew_white_36dp : Properties.Resources.baseline_autorenew_black_36dp);
            toolTip.SetToolTipIcon(lRenewToken, frm.UseDarkmode ? Properties.Resources.baseline_autorenew_white_36dp : Properties.Resources.baseline_autorenew_black_36dp);
            toolTip.SetToolTipIcon(btnEditAccount, frm.UseDarkmode ? Properties.Resources.outline_edit_white_36dp : Properties.Resources.outline_edit_black_36dp);
            toolTip.SetToolTipIcon(lEditAccount, frm.UseDarkmode ? Properties.Resources.outline_edit_white_36dp : Properties.Resources.outline_edit_black_36dp);
            toolTip.SetToolTipIcon(btnDeleteAccount, frm.UseDarkmode ? Properties.Resources.baseline_delete_outline_white_36dp : Properties.Resources.baseline_delete_outline_black_36dp);
            toolTip.SetToolTipIcon(lDeleteAccount, frm.UseDarkmode ? Properties.Resources.baseline_delete_outline_white_36dp : Properties.Resources.baseline_delete_outline_black_36dp);
            toolTip.SetToolTipIcon(dropServers, frm.UseDarkmode ? Properties.Resources.server_white_18px : Properties.Resources.server_18p);
            toolTip.SetToolTipIcon(toggleShowInVP, frm.UseDarkmode ? Properties.Resources.btn_icon_chest_1 : Properties.Resources.btn_icon_chest_2);
            toolTip.SetToolTipIcon(lShowInVP, frm.UseDarkmode ? Properties.Resources.btn_icon_chest_1 : Properties.Resources.btn_icon_chest_2);

            FormsUtils.ResumeDrawing(this);
        }

        public void UpdateToggleToggleValues()
                                => Task.Run(()
                                => SetToggleToggleValues());

        private void SetToggleToggleValues()
        {
            bool dl = (!frm.accounts.Select(a => a.performSave)
                                    .Contains(true));

            List<string> allEmails = frm.accounts.Select(a => a.Email).ToList();
            bool vp = true;
            for (int i = 0; i < allEmails.Count; i++)
            {
                if (!activeVaultPeekerAccounts.Contains(allEmails[i]))
                {
                    vp = false;
                    break;
                }
            }

            ToggleToggleToggles(dl, vp);
        }

        private bool ToggleToggleToggles(bool dl, bool vp)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool, bool, bool>)ToggleToggleToggles, dl, vp);

            isInit = true;

            toggleAllAccountsDailyLogins.Checked = dl;
            toggleAllAccountsShowInVaultPeeker.Checked = vp;

            isInit = false;

            return false;
        }

        public bool LoadServers()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)LoadServers);

            isInit = true;

            dropServers.Items.Clear();
            dropServers.Items.Add("EAM default server");
            dropServers.Items.Add("Last server (Deca default)");

            if (frm.serverData != null && frm.serverData.servers != null)
            {
                for (int i = 0; i < frm.serverData.servers.Count; i++)
                    dropServers.Items.Add(frm.serverData.servers[i].name);
            }

            isInit = false;

            return false;
        }

        public void UpdateAccountToUI()
        {
            if (dataGridView.SelectedRows.Count == 0 || isAddAccountToUI)
                return;

            AddAccountToUI(GetAccountInfo(dataGridView.SelectedRows[0].Index));
        }

        bool isAddAccountToUI = false;
        private void AddAccountToUI(MK_EAM_Lib.AccountInfo info)
        {
            if (isInit) return;
            isAddAccountToUI = true;

            int maxSize = pAccountName.Width - 25;
            float fSize = 15.75f;

            while (TextRenderer.MeasureText(info.name, new Font(lAccountName.Font.FontFamily, fSize, lAccountName.Font.Style)).Width > maxSize)
                fSize -= 0.5f;

            lAccountName.Font = new Font(lAccountName.Font.FontFamily, fSize, lAccountName.Font.Style);
            lAccountName.Text = info.name;

            maxSize = pEmail.Width - 25;
            fSize = 15.75f;
            while (TextRenderer.MeasureText(info.email, new Font(lAccountName.Font.FontFamily, fSize, lAccountName.Font.Style)).Width > maxSize)
                fSize -= 1f;

            lEmail.Font = new Font(lEmail.Font.FontFamily, fSize, lEmail.Font.Style);
            lEmail.Text = info.email;

            toggleLogin.Checked = info.performSave;
            toggleShowInVP.Checked = activeVaultPeekerAccounts.Contains(info.Email);

            btnRenewToken.Enabled = !dicLockRenewInfoToTime.ContainsKey(info);
            btnRenewToken.BackColor = btnRenewToken.BackgroundColor = btnRenewToken.BorderColor = btnRenewToken.Enabled ? Color.FromArgb(156, 95, 244) : (frm.UseDarkmode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(48, 48, 48));

            string item = info.serverName;
            if (item == null)
                item = string.Empty;
            if (dropServers.Items.Contains(item))
                dropServers.SelectedItem = item;
            else
            {
                if (item.Equals("Last"))
                    dropServers.SelectedIndex = 1;
                else
                    dropServers.SelectedIndex = 0;
            }

            btnPlay.Image = dicAccountsToProcesses.ContainsKey(info) ? Properties.Resources.OutlinePause_36px : Properties.Resources.OutlinePlay_36px;
            lStartGame.Text = dicAccountsToProcesses.ContainsKey(info) ? "Close the game" : "Start game";

            eleDeleteAccount.Visible = false;

            isAddAccountToUI = false;
        }

        #region Buttons

        #region Play Button

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                btnPlay.Enabled = false;

                try
                {
                    if (dicAccountsToProcesses.ContainsKey(GetAccountInfo(dataGridView.SelectedRows[0].Index)))
                    {
                        #region Running instance

                        try
                        {
                            timerLoadProcesses.Stop();
                            frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.StopExalt, $"Stopping exalt instance from: {GetAccountInfo(dataGridView.SelectedRows[0].Index).email}."));
                            while (searchProcesses)
                                Application.DoEvents();

                            dicAccountsToProcesses[GetAccountInfo(dataGridView.SelectedRows[0].Index)].Kill();

                            timerLoadProcesses.Start();
                        }
                        catch
                        {
                            frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to stopping exalt instance from: {GetAccountInfo(dataGridView.SelectedRows[0].Index).email}."));
                            frm.ShowSnackbar($"Failed to stopping exalt instance from: {GetAccountInfo(dataGridView.SelectedRows[0].Index).email}.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);

                            if (timerLoadProcesses.Enabled)
                                timerLoadProcesses.Start();
                        }

                        #endregion
                    }
                    else
                    {
                        #region Start game

                        try
                        {
                            if (System.IO.File.Exists(frm.OptionsData.exePath))
                            {
                                MK_EAM_Lib.AccountInfo info = GetAccountInfo(dataGridView.SelectedRows[0].Index);
                                DateTime cTime = new DateTime(2000, 1, 1);
                                if (info.accessToken != null)
                                {
                                    try
                                    {
                                        cTime = MK_EAM_Lib.AccessToken.UnixTimeStampToDateTime(Convert.ToDouble(info.accessToken.creationTime));
                                    }
                                    catch { cTime = new DateTime(2000, 1, 1); }
                                }
                                string hwid = frm.GetDeviceUniqueIdentifier();
                                if (frm.OptionsData.alwaysrefreshDataOnLogin || info.accessToken == null || cTime.Date < DateTime.Now.Date || info.accessToken.validUntil < DateTime.Now.AddHours(1) || string.IsNullOrEmpty(info.accessToken.clientToken) || !info.accessToken.clientToken.Equals(hwid))
                                {
                                    info.PerformWebrequest(frm, frm.LogEvent, "EAM AccUI", frm.accountStatsPath, frm.itemsSaveFilePath, hwid, false, true, StartGameForAccountInvoker);
                                }
                                else
                                {
                                    StartGameForAccount(info);
                                }
                            }
                            else
                            {
                                frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Login attempt failed, the game.exe was not found.{Environment.NewLine}Path: {frm.OptionsData.exePath}"));
                                frm.ShowSnackbar("Login attempt failed, the game.exe was not found.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
                                frm.LogButtonBlink();
                            }
                        }
                        catch (Exception ex)
                        {
                            frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Login attempt failed.{Environment.NewLine}Error:{ex.Message}"));
                            frm.ShowSnackbar("Login attempt failed.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
                            frm.LogButtonBlink();
                        }

                        #endregion
                    }
                }
                catch
                {
                    frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to start / stop exalt."));
                    frm.ShowSnackbar($"Failed to start / stop exalt.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                    frm.LogButtonBlink();
                }
                btnPlay.Enabled = true;
            }
        }

        private void StartGameForAccountInvoker(MK_EAM_Lib.AccountInfo _info) => StartGameForAccount(_info);

        private bool StartGameForAccount(MK_EAM_Lib.AccountInfo _info)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<MK_EAM_Lib.AccountInfo, bool>)StartGameForAccount, _info);

            if (_info.requestState == MK_EAM_Lib.AccountInfo.RequestState.Captcha)
            {
                CaptchaSolverUiUtils.Show(_info, frm, frm.UseDarkmode, frm.LogEvent, "EAM AccUI", frm.accountStatsPath, frm.itemsSaveFilePath, frm.GetDeviceUniqueIdentifier(), false, true, StartGameForAccountInvoker);
            }

            if (_info.requestState != MK_EAM_Lib.AccountInfo.RequestState.Success)
                return false;

            try
            {
                frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.Login, $"Start login into account: {_info.email}."));
                frm.ShowSnackbar($"Start login into account: {_info.email}.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000);

                string arguments = string.Format("\"data:{{platform:Deca,guid:{0},token:{1},tokenTimestamp:{2},tokenExpiration:{3},env:4,serverName:{4}}}\"",
                                   StringToBase64String(_info.email), StringToBase64String(_info.accessToken.token), StringToBase64String(_info.accessToken.creationTime), StringToBase64String(_info.accessToken.expirationTime), GetServerName(_info.serverName));

                ProcessStartInfo pinfo = new ProcessStartInfo();
                pinfo.FileName = frm.OptionsData.exePath;
                pinfo.Arguments = arguments;
                Process p = new Process();
                p.StartInfo = pinfo;
                p.Start();

                SaveLoginStats(_info);

                Task<bool> analyticsAddLogin = null;
                if (!frm.OptionsData.analyticsOptions.OptOut)
                {
                    analyticsAddLogin = AnalyticsClient.Instance?.AddLogin(frm.GetAnalyticsEmailHash(_info.Email), GetServerName(_info.serverName));
                }

                if (frm.OptionsData.closeAfterConnection)
                { 
                    //Hide the form during the closing process
                    frm.WindowState = FormWindowState.Minimized;
                    frm.ShowInTaskbar = false;

                    try
                    {
                        if (analyticsAddLogin != null)
                            analyticsAddLogin.Wait(5000);
                    }
                    catch { }
                    
                    Environment.Exit(0); 
                }
                else
                {
                    if (!dicAccountsToProcesses.ContainsKey(_info))
                    {
                        dicAccountsToProcesses.Add(_info, p);
                        dicProcessesToAccounts.Add(p, _info);

                        p.EnableRaisingEvents = true;
                        p.Exited += ProcessExit;

                        if (dataGridView.SelectedRows.Count > 0 && GetAccountInfo(dataGridView.SelectedRows[0].Index) == _info)
                        {
                            btnPlay.Image = Properties.Resources.OutlinePause_36px;
                            lStartGame.Text = "Close the game";
                        }
                    }
                }

                if (frm.OptionsData.discordOptions.ShowState)
                {
                    string state = frm.OptionsData.discordOptions.ShowAccountNames ? "Ingame as " + _info.name + " 🎮" : "Playing rotmg 🎮";
                    DiscordHelper.SetState(state);
                    DiscordHelper.ApplyPresence();
                }                
            }
            catch
            {
                frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to start exalt."));
                frm.ShowSnackbar($"Failed to start exalt.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                frm.LogButtonBlink();
            }

            return false;
        }
        private string StringToBase64String(string toEncode) => Convert.ToBase64String(StringToByteArray(toEncode));

        private byte[] StringToByteArray(string toConvert)
        {
            List<byte> bytes = new List<byte>();
            foreach (char c in toConvert)
                bytes.Add(Convert.ToByte(c));
            return bytes.ToArray();
        }

        private string GetServerName(string server)
        {
            if (string.IsNullOrEmpty(server))
                return frm.OptionsData.serverToJoin;
            else if (server.Equals("Last"))
                return string.Empty;
            else
                return server;
        }

        private void SaveLoginStats(MK_EAM_Lib.AccountInfo info, bool sendCharList = false)
        {
            try
            {
                string fileName = System.IO.Path.Combine(frm.accountStatsPath, frm.GetAccountStatsFilename(info.email));

                if (!System.IO.Directory.Exists(frm.accountStatsPath))
                    System.IO.Directory.CreateDirectory(frm.accountStatsPath);

                if (System.IO.File.Exists(fileName))
                {
                    try
                    {
                        StatsMain stats = (StatsMain)frm.ByteArrayToObject(System.IO.File.ReadAllBytes(fileName));
                        if (stats.logins == null)
                            stats.logins = new List<LoginStats>();
                        stats.logins.Add(new LoginStats() { time = DateTime.Now, isFromTask = false });
                        System.IO.File.WriteAllBytes(fileName, frm.ObjectToByteArray(stats));
                    }
                    catch (Exception)
                    {
                        frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to load / save login-stats."));
                        frm.ShowSnackbar("Failed to load / save login-stats.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                        frm.LogButtonBlink();
                    }
                }
                else
                {
                    StatsMain stats = new StatsMain() { email = info.email };
                    stats.logins = new List<LoginStats>() { new LoginStats() { time = DateTime.Now, isFromTask = false } };
                    System.IO.File.WriteAllBytes(fileName, frm.ObjectToByteArray(stats));
                }
            }
            catch
            {
                frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to save login-stats."));
                frm.ShowSnackbar("Failed to save login-stats.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                frm.LogButtonBlink();
            }
        }

        private void btnPlay_MouseEnter(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;
            //btnPlay.Image = dicAccountsToProcesses.ContainsKey(frm.accounts[dataGridView.SelectedRows[0].Index]) ? Properties.Resources.pause_36px : Properties.Resources.Play_36px_1;
            btnPlay.Image = dicAccountsToProcesses.ContainsKey(GetAccountInfo(dataGridView.SelectedRows[0].Index)) ? Properties.Resources.pause_36px : Properties.Resources.Play_36px_1;
            lStartGame.ForeColor = Color.FromArgb(85, 85, 85);
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;
            //btnPlay.Image = dicAccountsToProcesses.ContainsKey(frm.accounts[dataGridView.SelectedRows[0].Index]) ? Properties.Resources.OutlinePause_36px : Properties.Resources.OutlinePlay_36px;
            btnPlay.Image = dicAccountsToProcesses.ContainsKey(GetAccountInfo(dataGridView.SelectedRows[0].Index)) ? Properties.Resources.OutlinePause_36px : Properties.Resources.OutlinePlay_36px;
            lStartGame.ForeColor = this.ForeColor;
        }

        #endregion

        #region RenewToken Button

        private void btnRenewToken_Click(object sender, EventArgs e)
        {
            if (!btnRenewToken.Enabled || dataGridView.SelectedRows.Count == 0)
                return;

            btnRenewToken.Enabled = false;
            btnRenewToken.AllowAnimations = false;
            btnRenewToken.AnimationSpeed = 0;
            timerRenewChangeColor.Start();
            MK_EAM_Lib.AccountInfo info = GetAccountInfo(dataGridView.SelectedRows[0].Index);

            dicLockRenewInfoToTime.Add(info, DateTime.Now.AddSeconds(15));

            info.PerformWebrequest(frm, frm.LogEvent, "EAM AccUI", frm.accountStatsPath, frm.itemsSaveFilePath, frm.GetDeviceUniqueIdentifier(), string.IsNullOrEmpty(info.Name), true, RenewTokenFinished_Invoker);

            if (!timerReactivateRenewToken.Enabled)
                timerReactivateRenewToken.Start();
        }

        private void RenewTokenFinished_Invoker(MK_EAM_Lib.AccountInfo _info) => RenewTokenFinished(_info);

        private bool RenewTokenFinished(MK_EAM_Lib.AccountInfo _info)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<MK_EAM_Lib.AccountInfo, bool>)RenewTokenFinished, _info);

            if (_info.requestState == MK_EAM_Lib.AccountInfo.RequestState.Captcha)
            {
                bool result = CaptchaSolverUiUtils.Show(_info, frm, frm.UseDarkmode, frm.LogEvent, "EAM AccUI", frm.accountStatsPath, frm.itemsSaveFilePath, frm.GetDeviceUniqueIdentifier(), string.IsNullOrEmpty(_info.Name), true, RenewTokenFinished_Invoker);

                if (!result)
                {
                    frm.ShowSnackbar("Captcha failed, please try again.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                }
                
                return true;
            }

            if (_info.requestState == MK_EAM_Lib.AccountInfo.RequestState.Success)
                frm.ShowSnackbar("Data refreshed successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000);
            else
                frm.ShowSnackbar("Data refreshing failed.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);

            frm.SaveAccounts();
            GetAccountInfo(dataGridView.SelectedRows[0].Index);
            return false;
        }

        private void btnRenewToken_MouseEnter(object sender, EventArgs e)
        {
            if (!btnRenewToken.Enabled) return;
            btnRenewToken.Image = Properties.Resources.baseline_autorenew_white_36dp_45G;
            lRenewToken.ForeColor = Color.FromArgb(85, 85, 85);
        }

        private void btnRenewToken_MouseLeave(object sender, EventArgs e)
        {
            btnRenewToken.Image = Properties.Resources.baseline_autorenew_white_36dp;
            lRenewToken.ForeColor = this.ForeColor;
        }

        private void timerReactivateRenewToken_Tick(object sender, EventArgs e)
        {
            MK_EAM_Lib.AccountInfo[] accs = dicLockRenewInfoToTime.Keys.ToArray();

            foreach (MK_EAM_Lib.AccountInfo acc in accs)
            {
                if (dicLockRenewInfoToTime[acc] < DateTime.Now)
                {
                    dicLockRenewInfoToTime.Remove(acc);

                    if (GetAccountInfo(dataGridView.SelectedRows[0].Index).email.Equals(acc.email))
                    {
                        btnRenewToken.BackColor = btnRenewToken.BackgroundColor = btnRenewToken.BorderColor = Color.FromArgb(156, 95, 244);
                        btnRenewToken.Enabled = true;
                        btnRenewToken.AllowAnimations = true;
                        btnRenewToken.AnimationSpeed = 200;
                    }
                }
            }

            if (dicLockRenewInfoToTime.Count == 0)
                timerReactivateRenewToken.Stop();
        }

        private void timerRenewChangeColor_Tick(object sender, EventArgs e)
        {
            btnRenewToken.BackColor = btnRenewToken.BackgroundColor = btnRenewToken.BorderColor = frm.UseDarkmode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(48, 48, 48);
            timerRenewChangeColor.Stop();
        }

        #endregion

        #region EditAccount Button

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count <= 0)
                return;
            MK_EAM_Lib.AccountInfo acc = GetAccountInfo(dataGridView.SelectedRows[0].Index);
            frm.ShowShadowForm(new Elements.EleEditAccount(frm, acc, activeVaultPeekerAccounts.Contains(acc.Email)));
        }

        private void btnEditAccount_MouseEnter(object sender, EventArgs e)
        {
            btnEditAccount.Image = Properties.Resources.ic_edit_white_36dp;
            lEditAccount.ForeColor = Color.FromArgb(85, 85, 85);
        }

        private void btnEditAccount_MouseLeave(object sender, EventArgs e)
        {
            btnEditAccount.Image = Properties.Resources.outline_edit_white_36dp;
            lEditAccount.ForeColor = this.ForeColor;
        }

        #endregion

        #region DeleteAccount Button

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count <= 0 || eleDeleteAccount.Visible)
                return;

            //eleDeleteAccount.ShowUI(frm.accounts[dataGridView.SelectedRows[0].Index]);
            eleDeleteAccount.ShowUI(GetAccountInfo(dataGridView.SelectedRows[0].Index));
        }

        private void btnDeleteAccount_MouseEnter(object sender, EventArgs e)
        {
            btnDeleteAccount.Image = Properties.Resources.ic_delete_forever_white_36dp;
            btnDeleteAccount.BackgroundColor = btnDeleteAccount.BorderColor =
            lDeleteAccount.ForeColor = Color.Crimson;
        }

        private void btnDeleteAccount_MouseLeave(object sender, EventArgs e)
        {
            btnDeleteAccount.Image = Properties.Resources.baseline_delete_outline_white_36dp;
            lDeleteAccount.ForeColor = this.ForeColor;
            btnDeleteAccount.BackgroundColor = btnDeleteAccount.BorderColor = Color.FromArgb(156, 95, 244);

            timerResetDeleteButton.Start();
        }

        private void timerResetDeleteButton_Tick(object sender, EventArgs e)
        {
            timerResetDeleteButton.Stop();

            btnDeleteAccount.BackgroundColor = btnDeleteAccount.BorderColor = Color.FromArgb(156, 95, 244);
        }

        public void DeleteAccount(MK_EAM_Lib.AccountInfo _info, bool removeAccount)
        {
            if (removeAccount)
            {
                dataGridView.DataBindings.Clear();

                if (frm.accounts.Contains(_info))
                    frm.accounts.Remove(_info);

                frm.SaveAccounts();
                UpdateDatagridView();
            }

            eleDeleteAccount.Visible = false;
        }

        #endregion

        #endregion

        private void lEmail_SizeChanged(object sender, EventArgs e) => pCopyEmail.Width = Math.Max(pEmail.Width - lEmail.Width, 24);

        private void lAccountName_SizeChanged(object sender, EventArgs e) => pCopyAccountName.Width = Math.Max(pAccountName.Width - lAccountName.Width, 24);

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            if (dataGridView.SelectedRows.Count > 0)
                AddAccountToUI(GetAccountInfo(dataGridView.SelectedRows[0].Index));
            else
                AddAccountToUI(new MK_EAM_Lib.AccountInfo());
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (isInit || dataGridView.AllowDrop) return;

            if (e.ColumnIndex == 0) // Got Group
            {
                colorChanger.Location = new Point(bunifuCards.Left + pData.Left + dataGridView.Left + dataGridView.Columns[0].Width,
                                                  bunifuCards.Top + pData.Top + dataGridView.Top + (dataGridView.SelectedRows[0].Index * dataGridView.SelectedRows[0].Height));
                if (colorChanger.Top + colorChanger.Height > this.Height)
                    colorChanger.Top = this.Height - (colorChanger.Height + 3);
                colorChanger.ShowUI(dataGridView.SelectedRows[0].Index);
                if (!colorChanger.Visible)
                    transition.Show(colorChanger, true, Bunifu.UI.WinForms.BunifuAnimatorNS.Animation.HorizSlide);
            }
        }

        public void UpdateGroupCell(int index)
        {
            try
            {
                dataGridView.Rows[index].Cells[0].Value = frm.accounts[frm.accounts.IndexOf(GetAccountInfo(index))].Group;
            }
            catch { }
        }

        public void UpdateDatagridView()
        {
            isInit = true;

            dataGridView.Columns.Clear();
            dataGridView.DataBindings.Clear();
            bindingSource = new BindingSource();
            bindingSource.DataSource = frm.accounts;
            dataGridView.DataSource = bindingSource;
            timerEnsureDatagridUpdate.Start();
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (timerEnsureDatagridUpdate.Enabled)
                timerEnsureDatagridUpdate.Stop();

            if (dataGridView.Rows.Count > 0)
            {
                dataGridView.ClearSelection();
                dataGridView.Rows[0].Selected = true;
            }
            dataGridView.Columns[0].Width = 60;
            dataGridView.Columns[0].DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding() { Left = 20, Top = 0, Right = 0, Bottom = 0 } };

            UpdateScrollbarValue();

            isInit = false;
        }

        private void timerEnsureDatagridUpdate_Tick(object sender, EventArgs e)
        {
            dataGridView_DataBindingComplete(dataGridView, null);
            timerEnsureDatagridUpdate.Stop();
        }

        public void UpdateScrollbarValue()
        {
            scrollbar.SmallChange = 1;
            scrollbar.LargeChange = 2;
            scrollbar.Value = 0;

            int max = 1;
            if (dataGridView.Rows.Count > 1)
                max = (dataGridView.Rows.Count - dataGridView.DisplayedRowCount(false)) - 1;
            scrollbar.Maximum = max < 1 ? 1 : max;
        }

        private void scrollbar_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            if (isInit) return;

            dataGridView.FirstDisplayedScrollingRowIndex = scrollbar.Value;
            dataGridView.Update();
        }

        private void dataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;

            int movement = e.Delta / 120;

            if (dataGridView.FirstDisplayedScrollingRowIndex - movement >= 0 && dataGridView.FirstDisplayedScrollingRowIndex - movement < (dataGridView.Rows.Count - dataGridView.DisplayedRowCount(false)))
                scrollbar.Value = dataGridView.FirstDisplayedScrollingRowIndex -= movement;
        }

        private void btnCopyAccountName_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                string name = GetAccountInfo(dataGridView.SelectedRows[0].Index).name;
                if (!string.IsNullOrEmpty(name))
                {
                    Clipboard.SetText(name);
                    frm.ShowSnackbar("Accountname copied to clipboard.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information);
                }
                else
                    frm.ShowSnackbar("No Accountname found.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information);
            }
            else
                frm.ShowSnackbar("Please select an account first.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information);
        }

        private void btnCopyEmail_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                string email = GetAccountInfo(dataGridView.SelectedRows[0].Index).email;
                if (!string.IsNullOrEmpty(email))
                {
                    Clipboard.SetText(email);
                    frm.ShowSnackbar("E-Mail copied to clipboard.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information);
                }
                else
                    frm.ShowSnackbar("No E-Mail found.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information);
            }
            else
                frm.ShowSnackbar("Please select an account first.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information);
        }

        private void btnToggleFilter_Click(object sender, EventArgs e)
        {
            if (pFilter.Visible)
            {
                pFilterParent.Visible = false;
                dragOffset = new Point(45, 50);
            }
            else
            {
                transition.Show(pFilterParent, true, Bunifu.UI.WinForms.BunifuAnimatorNS.Animation.HorizSlide);
                dragOffset = new Point(45, 240);

                if (pToggleMenuParent.Visible)
                    pToggleMenuParent.Visible = false;
            }
        }

        private bool toggleLoginChange = false;
        private void toggleLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (isInit || isAddAccountToUI || dataGridView.SelectedRows.Count <= 0)
            {
                if (!toggleLoginChange)
                {
                    toggleLoginChange = true;
                    toggleLogin.Checked = !toggleLogin.Checked;
                    return;
                }
                toggleLoginChange = false;
                return;
            }

            GetAccountInfo(dataGridView.SelectedRows[0].Index).performSave = !toggleLogin.Checked;

            frm.SaveAccountsNeeded();

            Task.Run(() => SetToggleToggleValues());
        }

        private bool dropServerChange = true;
        private void dropServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit || isAddAccountToUI) return;

            if (dataGridView.SelectedRows.Count > 0)
            {
                switch (dropServers.SelectedIndex)
                {
                    case 0:
                        GetAccountInfo(dataGridView.SelectedRows[0].Index).serverName = string.Empty;
                        break;
                    case 1:
                        GetAccountInfo(dataGridView.SelectedRows[0].Index).serverName = "Last";
                        break;
                    default:
                        if (dropServers.SelectedItem != null)
                            GetAccountInfo(dataGridView.SelectedRows[0].Index).serverName = dropServers.SelectedItem.ToString();
                        break;
                }
                frm.SaveAccountsNeeded();
            }
            else
            {
                if (!dropServerChange)
                {
                    dropServerChange = true;
                    dropServers.SelectedIndex = 0;
                    frm.ShowSnackbar("Please select an account first.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information);
                    return;
                }
                dropServerChange = false;
            }
        }

        private void timerLoadProcesses_Tick(object sender, EventArgs e)
        {
            try
            {
                GetExaltProcesses();
            }
            catch
            {
                frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to load running processes."));
                frm.LogButtonBlink();
                frm.ShowSnackbar("Failed to load running processes.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
            }
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

                        try
                        {
                            MK_EAM_Lib.AccountInfo q = frm.accounts.Where(a => a.email == guid).FirstOrDefault();
                            if (!dicAccountsToProcesses.ContainsKey(q))
                            {
                                dicAccountsToProcesses.Add(q, process);
                                dicProcessesToAccounts.Add(process, q);

                                process.EnableRaisingEvents = true;
                                process.Exited += ProcessExit;

                                if (dataGridView.SelectedRows.Count > 0 && GetAccountInfo(dataGridView.SelectedRows[0].Index) == q)
                                {
                                    btnPlay.Image = Properties.Resources.OutlinePause_36px;
                                    lStartGame.Text = "Close the game";
                                }
                            }
                        }
                        catch { }
                    }
                }
                catch (Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
                {
                    // Intentionally empty - no security access to the process.
                    frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to get Exalt-Processes (access denied)!"));
                }
                catch (InvalidOperationException)
                {
                    // Intentionally empty - the process exited before getting details.
                    frm.LogEvent(new LogData(-1, "EAM AccUI", LogEventType.EAMError, $"Failed to get Exalt-Processes (process exited before getting details)!"));
                }
                catch { }
            }
            searchProcesses = false;
        }

        private void ProcessExit(object sender, EventArgs e)
        {
            try
            {
                Process p = sender as Process;
                p.Exited -= ProcessExit;

                if (p != null && dicProcessesToAccounts.ContainsKey(p))
                {
                    ProcessExitUI(dicProcessesToAccounts[p]);

                    if (dicAccountsToProcesses.ContainsKey(dicProcessesToAccounts[p]))
                        dicAccountsToProcesses.Remove(dicProcessesToAccounts[p]);
                    dicProcessesToAccounts.Remove(p);

                    p.Dispose();
                    p = null;
                }
            }
            catch { }
        }

        private bool ProcessExitUI(MK_EAM_Lib.AccountInfo _info)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<MK_EAM_Lib.AccountInfo, bool>)ProcessExitUI, _info);

            try
            {
                if (dataGridView.SelectedRows.Count > 0 && GetAccountInfo(dataGridView.SelectedRows[0].Index) == _info)
                {
                    btnPlay.Image = Properties.Resources.OutlinePlay_36px;
                    lStartGame.Text = "Start game";
                }

                if (frm.OptionsData.discordOptions.ShowAccountNames)
                {
                    if (DiscordHelper.LastState.Contains(_info.Name))
                    {
                        if (dicProcessesToAccounts.Keys.Count > 1)
                        {
                            foreach (var item in dicProcessesToAccounts)
                            {
                                if (item.Value.Email != _info.Email)
                                {
                                    string state = frm.OptionsData.discordOptions.ShowAccountNames ? "Ingame as " + _info.name + " 🎮" : "Playing rotmg 🎮";
                                    DiscordHelper.SetState(state);
                                    DiscordHelper.ApplyPresence();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            DiscordHelper.UpdateMenu(null);
                        }
                    }
                }
            }
            catch { }

            return false;
        }

        #region DragNDrop

        private bool startedDragnDrop = false;
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private DragDropEffects dropEffect = DragDropEffects.Move;
        private PictureBox pbDragbox = new PictureBox() { SizeMode = PictureBoxSizeMode.StretchImage, Visible = false, Capture = false };
        private Bitmap bmpRow;
        private Point dragOffset = new Point(45, 50);

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!startedDragnDrop || !dataGridView.AllowDrop) return;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    dropEffect = dataGridView.DoDragDrop(
                    dataGridView.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (!startedDragnDrop || !dataGridView.AllowDrop) return;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    dropEffect = dataGridView.DoDragDrop(
                    dataGridView.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void dataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!startedDragnDrop) return;            

            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dataGridView.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);

                startedDragnDrop = true;

                Size dgvSz = dataGridView.ClientSize;
                Rectangle RowRect = dataGridView.GetRowDisplayRectangle(rowIndexFromMouseDown, true);
                bmpRow = new Bitmap(RowRect.Width, RowRect.Height);
                using (Bitmap bmpDgv = new Bitmap(dgvSz.Width, dgvSz.Height))
                //using (Bitmap bmpRow = new Bitmap(RowRect.Width, RowRect.Height))
                {
                    dataGridView.DrawToBitmap(bmpDgv, new Rectangle(Point.Empty, dgvSz));
                    using (Graphics G = Graphics.FromImage(bmpRow))
                        G.DrawImage(bmpDgv, new Rectangle(Point.Empty,
                                    RowRect.Size), RowRect, GraphicsUnit.Pixel);

                    pbDragbox.Size = RowRect.Size;
                    pbDragbox.Image = bmpRow;

                    Point p = dataGridView.PointToClient(new Point(e.X, e.Y));
                    p.X += dragOffset.X;
                    p.Y += dragOffset.Y;
                    pbDragbox.Location = p;
                    pbDragbox.Visible = true;
                }
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            startedDragnDrop = false;
        }

        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (!startedDragnDrop || !dataGridView.AllowDrop) return;

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move && rowIndexFromMouseDown > -1 && rowIndexFromMouseDown < dataGridView.Rows.Count && rowIndexOfItemUnderMouseToDrop > -1 && rowIndexOfItemUnderMouseToDrop < dataGridView.Rows.Count)
            {
                BindingExtension.MoveIndex(frm.accounts, rowIndexFromMouseDown, rowIndexOfItemUnderMouseToDrop);
                dataGridView.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
            }
            dropEffect = DragDropEffects.Move;
            pbDragbox.Visible = false;
            startedDragnDrop = false;

            for (int i = 0; i < frm.accounts.Count; i++)
                frm.accounts[i].orderID = i + 1;

            frm.SaveAccountsNeeded();
            UpdateDatagridView();
        }

        private void dataGridView_DragOver(object sender, DragEventArgs e)
        {
            if (!startedDragnDrop || !dataGridView.AllowDrop) return;

            e.Effect = dropEffect;
            Point p = dataGridView.PointToClient(new Point(e.X, e.Y));
            p.X += dragOffset.X;
            p.Y += dragOffset.Y;
            pbDragbox.Location = p;

            this.Update();
        }

        private void dataGridView_DragLeave(object sender, EventArgs e)
        {
            pbDragbox.Visible = false;
        }

        private void dataGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (!startedDragnDrop || !dataGridView.AllowDrop) return;
            pbDragbox.Visible = true;
        }

        private void pbDragbox_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = dataGridView.PointToClient(new Point(e.X, e.Y));
            p.X += dragOffset.X;
            p.Y += dragOffset.Y;
            pbDragbox.Location = p;

            dataGridView.Update();
        }

        private void PbDragbox_DragEnter(object sender, DragEventArgs e)
        {
            Point p = dataGridView.PointToClient(new Point(e.X, e.Y));
            p.X += dragOffset.X;
            p.Y += dragOffset.Y;
            pbDragbox.Location = p;

            dataGridView.Update();
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            startedDragnDrop = false;
        }

        #endregion

        private void toggleAllowDragNDrop_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView.AllowDrop = toggleAllowDragNDrop.Checked;

            if (colorChanger.Visible)
                colorChanger.Visible = false;

            if (dataGridView.AllowDrop)
            {
                startedDragnDrop = false;
                rowIndexFromMouseDown = -1;
                rowIndexOfItemUnderMouseToDrop = -1;

                dataGridView.MouseMove += dataGridView_MouseMove;
                dataGridView.MouseUp += dataGridView_MouseUp;
                dataGridView.CellMouseDown += dataGridView_CellMouseDown;
                dataGridView.DoubleClick += dataGridView_DoubleClick;

                dataGridView.DragDrop += dataGridView_DragDrop;
                dataGridView.DragEnter += dataGridView_DragEnter;
                dataGridView.DragLeave += dataGridView_DragLeave;
                dataGridView.DragOver += dataGridView_DragOver;

                dataGridView.CellMouseClick -= dataGridView_CellMouseClick;
            }
            else
            {
                dataGridView.MouseMove -= dataGridView_MouseMove;
                dataGridView.MouseDown -= dataGridView_MouseDown;
                dataGridView.MouseUp -= dataGridView_MouseUp;
                dataGridView.DoubleClick -= dataGridView_DoubleClick;

                dataGridView.DragDrop -= dataGridView_DragDrop;
                dataGridView.DragEnter -= dataGridView_DragEnter;
                dataGridView.DragLeave -= dataGridView_DragLeave;
                dataGridView.DragOver -= dataGridView_DragOver;

                dataGridView.CellMouseClick += dataGridView_CellMouseClick;
            }
        }

        public MK_EAM_Lib.AccountInfo GetAccountInfo(int index)
        {
            if (!useQuerry)
                return frm.accounts[index];
            else
                return frm.accounts.Where(a => a.Email.Equals(dataGridView.Rows[index].Cells[2].Value.ToString())).FirstOrDefault();
        }

        #region Sort

        private void radioSearchForAccount_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e) { if (!isInit && tbSortQuerry.Text.Length > 0) tbSortQuerry_TextChanged(tbSortQuerry, null); }

        private void radioSearchForEmail_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e) { if (!isInit && tbSortQuerry.Text.Length > 0) tbSortQuerry_TextChanged(tbSortQuerry, null); }

        private bool useQuerry = false;
        private void tbSortQuerry_TextChanged(object sender, EventArgs e)
        {
            dataGridView.CurrentCell = null;
            isInit = true;

            dropOrder.SelectedIndex = 0;

            if (tbSortQuerry.Text.Length == 0)
            {
                dataGridView.DataBindings.Clear();
                dataGridView.DataSource = frm.accounts;
                useQuerry = false;
            }
            else
            {
                useQuerry = true;
                dataGridView.DataBindings.Clear();

                //dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(radioSearchForAccount.Checked ? frm.accounts.Where(a => a.Name.Contains(tbSortQuerry.Text)).ToList() : frm.accounts.Where(a => a.Email.Contains(tbSortQuerry.Text)).ToList());
                dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(radioSearchForAccount.Checked ? frm.accounts.Where(a => CultureInfo.CurrentUICulture.CompareInfo.IndexOf(a.Name, tbSortQuerry.Text, CompareOptions.IgnoreCase) >= 0).OrderBy(a => a.orderID).ToList() : frm.accounts.Where(a => CultureInfo.CurrentUICulture.CompareInfo.IndexOf(a.Email, tbSortQuerry.Text, CompareOptions.IgnoreCase) >= 0).OrderBy(a => a.orderID).ToList());
            }
            isInit = false;
            dataGridView.CurrentCell = null;
            if (dataGridView.Columns.Count == 0)
                return;
            dataGridView.Columns[0].Width = 58;
            dataGridView.Columns[0].DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding() { Left = 20, Top = 0, Right = 0, Bottom = 0 } };
            UpdateScrollbarValue();

            toggleAllowDragNDrop.Enabled = !useQuerry;
            if (useQuerry)
                toggleAllowDragNDrop.Checked = false;

            if (dataGridView.Rows.Count > 0)
                dataGridView.Rows[0].Selected = true;
        }

        #endregion

        private void dropOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit)
                return;
            /*  
                Default
                Group
                Accountname (A -> Z)
                Accountname (Z -> A)
                Email (A -> Z)
                Email (Z -> A)
                Daily Autologin (on)
                Daily Autologin (off)
            */
            dataGridView.DataBindings.Clear();

            switch (dropOrder.SelectedIndex)
            {
                case 0: // Default
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderBy(a => a.orderID).ToList());
                        useQuerry = false;
                    }
                    break;
                case 1: // Group
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderByDescending(a => a.Color.GetHue()).ToList());
                        useQuerry = true;
                    }
                    break;
                case 2: // Accountname (A -> Z)
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderBy(a => a.Name).ToList());
                        useQuerry = true;
                    }
                    break;
                case 3: // Accountname (Z -> A)
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderByDescending(a => a.Name).ToList());
                        useQuerry = true;
                    }
                    break;
                case 4: // Email (A -> Z)
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderBy(a => a.Email).ToList());
                        useQuerry = true;
                    }
                    break;
                case 5: // Email (Z -> A)
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderByDescending(a => a.Email).ToList());
                        useQuerry = true;
                    }
                    break;
                case 6: // Daily Autologin (on)
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderByDescending(a => a.performSave).ThenBy(a => a.orderID).ToList());
                        useQuerry = true;
                    }
                    break;
                case 7: // Daily Autologin (off)
                    {
                        dataGridView.DataSource = new BindingList<MK_EAM_Lib.AccountInfo>(frm.accounts.OrderBy(a => a.performSave).ThenBy(a => a.orderID).ToList());
                        useQuerry = true;
                    }
                    break;
                default:
                    break;
            }
            dataGridView.ClearSelection();

        }

        private void toggleShowInVP_CheckedChanged(object sender, EventArgs e)
        {
            if (isInit || isAddAccountToUI || dataGridView.SelectedRows.Count <= 0)
                return;

            MK_EAM_Lib.AccountInfo info = GetAccountInfo(dataGridView.SelectedRows[0].Index);
            if (info == null) return;

            if (activeVaultPeekerAccounts.Contains(info.Email))
                activeVaultPeekerAccounts.Remove(info.Email);
            else
                activeVaultPeekerAccounts.Add(info.Email);

            Task.Run(() => SetToggleToggleValues());

            try
            {
                System.IO.File.WriteAllBytes(frm.activeVaultPeekerAccountsPath, frm.ObjectToByteArray(activeVaultPeekerAccounts));
            }
            catch
            {
                frm.LogEvent(new LogData("EAM AccUI", LogEventType.EAMError, "Failed to save active Vault Peeker accounts."));
                frm.ShowSnackbar("Failed to save active Vault Peeker accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void btnToggleToggle_Click(object sender, EventArgs e)
        {
            if (pToggleMenu.Visible)
            {
                pToggleMenuParent.Visible = false;
                dragOffset = new Point(45, 50);
            }
            else
            {
                transition.Show(pToggleMenuParent, true, Bunifu.UI.WinForms.BunifuAnimatorNS.Animation.HorizSlide);
                dragOffset = new Point(45, 170);

                if (pFilterParent.Visible)
                    pFilterParent.Visible = false;
            }
        }

        private void btnToggleToggle_MouseEnter(object sender, EventArgs e)
        {
            btnToggleToggle.Image = Properties.Resources.toggle_on_white_24px;
        }

        private void btnToggleToggle_MouseLeave(object sender, EventArgs e)
        {
            btnToggleToggle.Image = Properties.Resources.toggle_off_White_24px;
        }

        private void toggleAllAccountsDailyLogins_CheckedChanged(object sender, EventArgs e)
        {
            if (isInit || isAddAccountToUI)
                return;

            for (int i = 0; i < frm.accounts.Count; i++)
                frm.accounts[i].performSave = !toggleAllAccountsDailyLogins.Checked;

            frm.SaveAccountsNeeded();
            dataGridView_SelectionChanged(dataGridView, null);
        }

        private void toggleAllAccountsShowInVaultPeeker_CheckedChanged(object sender, EventArgs e)
        {
            if (isInit || isAddAccountToUI)
                return;

            activeVaultPeekerAccounts.Clear();

            if (toggleAllAccountsShowInVaultPeeker.Checked)
                activeVaultPeekerAccounts = frm.accounts.Select(a => a.Email).ToList();

            try
            {
                System.IO.File.WriteAllBytes(frm.activeVaultPeekerAccountsPath, frm.ObjectToByteArray(activeVaultPeekerAccounts));
            }
            catch
            {
                frm.LogEvent(new LogData("EAM AccUI", LogEventType.EAMError, "Failed to save active Vault Peeker accounts."));
                frm.ShowSnackbar("Failed to save active Vault Peeker accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }

            dataGridView_SelectionChanged(dataGridView, null);
        }

        public void AddActiveVaultPeekerAccount(string email)
        {
            if (!activeVaultPeekerAccounts.Contains(email))
            {
                activeVaultPeekerAccounts.Add(email);
                Task.Run(() => SetToggleToggleValues());

                try
                {
                    System.IO.File.WriteAllBytes(frm.activeVaultPeekerAccountsPath, frm.ObjectToByteArray(activeVaultPeekerAccounts));
                }
                catch
                {
                    frm.LogEvent(new LogData("EAM AccUI", LogEventType.EAMError, "Failed to save active Vault Peeker accounts."));
                    frm.ShowSnackbar("Failed to save active Vault Peeker accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                }
            }
        }

        public void RemoveActiveVaultPeekerAccount(string email)
        {
            if (activeVaultPeekerAccounts.Contains(email))
            {
                activeVaultPeekerAccounts.Remove(email);
                Task.Run(() => SetToggleToggleValues());

                try
                {
                    System.IO.File.WriteAllBytes(frm.activeVaultPeekerAccountsPath, frm.ObjectToByteArray(activeVaultPeekerAccounts));
                }
                catch
                {
                    frm.LogEvent(new LogData("EAM AccUI", LogEventType.EAMError, "Failed to save active Vault Peeker accounts."));
                    frm.ShowSnackbar("Failed to save active Vault Peeker accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                }
            }
        }
    }

    public static class BindingExtension
    {
        public static void SwapItems<T>(this BindingList<T> list, int idxX, int idxY)
        {
            if (idxX != idxY)
            {
                T tmp = list[idxX];
                list[idxX] = list[idxY];
                list[idxY] = tmp;
            }
        }

        public static void MoveIndex<T>(this BindingList<T> list, int srcIdx, int destIdx)
        {
            if (srcIdx != destIdx)
            {
                T tmp = list[srcIdx];
                list.RemoveAt(srcIdx);
                list.Insert(destIdx, tmp);
            }
        }
    }
}
