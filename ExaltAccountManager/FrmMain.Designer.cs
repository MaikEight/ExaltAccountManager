namespace ExaltAccountManager
{
    partial class FrmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Bunifu.UI.WinForms.BunifuAnimatorNS.Animation animation1 = new Bunifu.UI.WinForms.BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pTop = new System.Windows.Forms.Panel();
            this.lUpdateAvailable = new System.Windows.Forms.Label();
            this.pBox = new System.Windows.Forms.Panel();
            this.pbDarkmode = new ExaltAccountManager.RoundPictureBox();
            this.lDev = new System.Windows.Forms.Label();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.lVersion = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.pMain = new System.Windows.Forms.Panel();
            this.timerLlama = new System.Windows.Forms.Timer(this.components);
            this.timerLoadProcesses = new System.Windows.Forms.Timer(this.components);
            this.timerMoreToolsUI = new System.Windows.Forms.Timer(this.components);
            this.snackbar = new Bunifu.UI.WinForms.BunifuSnackbar(this.components);
            this.toolTip = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.lUpdateNeeded = new System.Windows.Forms.Label();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.header = new ExaltAccountManager.AccountUIHeader();
            this.transition = new Bunifu.UI.WinForms.BunifuTransition(this.components);
            this.timerShowMessage = new System.Windows.Forms.Timer(this.components);
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDarkmode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.lUpdateAvailable);
            this.pTop.Controls.Add(this.pBox);
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Controls.Add(this.lHeadline);
            this.transition.SetDecoration(this.pTop, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(600, 48);
            this.pTop.TabIndex = 0;
            this.toolTip.SetToolTip(this.pTop, "");
            this.toolTip.SetToolTipIcon(this.pTop, null);
            this.toolTip.SetToolTipTitle(this.pTop, "");
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // lUpdateAvailable
            // 
            this.lUpdateAvailable.AutoSize = true;
            this.lUpdateAvailable.BackColor = System.Drawing.Color.Transparent;
            this.lUpdateAvailable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.transition.SetDecoration(this.lUpdateAvailable, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lUpdateAvailable.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lUpdateAvailable.ForeColor = System.Drawing.Color.DarkOrange;
            this.lUpdateAvailable.Location = new System.Drawing.Point(58, 34);
            this.lUpdateAvailable.Name = "lUpdateAvailable";
            this.lUpdateAvailable.Size = new System.Drawing.Size(263, 14);
            this.lUpdateAvailable.TabIndex = 8;
            this.lUpdateAvailable.Text = "EAM-Update available! Click to open website";
            this.lUpdateAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.lUpdateAvailable, "");
            this.toolTip.SetToolTipIcon(this.lUpdateAvailable, null);
            this.toolTip.SetToolTipTitle(this.lUpdateAvailable, "");
            this.lUpdateAvailable.Visible = false;
            this.lUpdateAvailable.Click += new System.EventHandler(this.lUpdateAvailable_Click);
            // 
            // pBox
            // 
            this.pBox.Controls.Add(this.pbDarkmode);
            this.pBox.Controls.Add(this.lDev);
            this.pBox.Controls.Add(this.pbMinimize);
            this.pBox.Controls.Add(this.pbClose);
            this.pBox.Controls.Add(this.lVersion);
            this.transition.SetDecoration(this.pBox, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Location = new System.Drawing.Point(510, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(90, 48);
            this.pBox.TabIndex = 4;
            this.toolTip.SetToolTip(this.pBox, "");
            this.toolTip.SetToolTipIcon(this.pBox, null);
            this.toolTip.SetToolTipTitle(this.pBox, "");
            this.pBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // pbDarkmode
            // 
            this.pbDarkmode.BackColor = System.Drawing.Color.White;
            this.transition.SetDecoration(this.pbDarkmode, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbDarkmode.Image = global::ExaltAccountManager.Properties.Resources.ic_brightness_4_black_48dp;
            this.pbDarkmode.Location = new System.Drawing.Point(5, 0);
            this.pbDarkmode.Name = "pbDarkmode";
            this.pbDarkmode.Size = new System.Drawing.Size(32, 32);
            this.pbDarkmode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDarkmode.TabIndex = 6;
            this.pbDarkmode.TabStop = false;
            this.toolTip.SetToolTip(this.pbDarkmode, "");
            this.toolTip.SetToolTipIcon(this.pbDarkmode, null);
            this.toolTip.SetToolTipTitle(this.pbDarkmode, "");
            this.pbDarkmode.Click += new System.EventHandler(this.pbDarkmode_Click);
            this.pbDarkmode.MouseEnter += new System.EventHandler(this.pbDarkmode_MouseEnter);
            this.pbDarkmode.MouseLeave += new System.EventHandler(this.pbDarkmode_MouseLeave);
            // 
            // lDev
            // 
            this.lDev.AutoSize = true;
            this.transition.SetDecoration(this.lDev, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lDev.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDev.Location = new System.Drawing.Point(37, 33);
            this.lDev.Name = "lDev";
            this.lDev.Size = new System.Drawing.Size(54, 15);
            this.lDev.TabIndex = 5;
            this.lDev.Text = "by Maik8";
            this.toolTip.SetToolTip(this.lDev, "");
            this.toolTip.SetToolTipIcon(this.lDev, null);
            this.toolTip.SetToolTipTitle(this.lDev, "");
            this.lDev.Click += new System.EventHandler(this.lDev_Click);
            // 
            // pbMinimize
            // 
            this.transition.SetDecoration(this.pbMinimize, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbMinimize.Image = global::ExaltAccountManager.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(42, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMinimize.TabIndex = 1;
            this.pbMinimize.TabStop = false;
            this.toolTip.SetToolTip(this.pbMinimize, "");
            this.toolTip.SetToolTipIcon(this.pbMinimize, null);
            this.toolTip.SetToolTipTitle(this.pbMinimize, "");
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbClose
            // 
            this.transition.SetDecoration(this.pbClose, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(66, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 0;
            this.pbClose.TabStop = false;
            this.toolTip.SetToolTip(this.pbClose, "");
            this.toolTip.SetToolTipIcon(this.pbClose, null);
            this.toolTip.SetToolTipTitle(this.pbClose, "");
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.transition.SetDecoration(this.lVersion, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lVersion.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lVersion.Location = new System.Drawing.Point(3, 33);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(37, 15);
            this.lVersion.TabIndex = 6;
            this.lVersion.Text = "v2.2.2";
            this.toolTip.SetToolTip(this.lVersion, "");
            this.toolTip.SetToolTipIcon(this.lVersion, null);
            this.toolTip.SetToolTipTitle(this.lVersion, "");
            // 
            // pbLogo
            // 
            this.transition.SetDecoration(this.pbLogo, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_account_balance_wallet_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.toolTip.SetToolTip(this.pbLogo, "");
            this.toolTip.SetToolTipIcon(this.pbLogo, null);
            this.toolTip.SetToolTipTitle(this.pbLogo, "");
            this.pbLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pbLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.transition.SetDecoration(this.lHeadline, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(54, 11);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(243, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Exalt Account Manager";
            this.toolTip.SetToolTip(this.lHeadline, "");
            this.toolTip.SetToolTipIcon(this.lHeadline, null);
            this.toolTip.SetToolTipTitle(this.lHeadline, "");
            this.lHeadline.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.lHeadline.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // pMain
            // 
            this.transition.SetDecoration(this.pMain, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(0, 84);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(592, 591);
            this.pMain.TabIndex = 1;
            this.toolTip.SetToolTip(this.pMain, "");
            this.toolTip.SetToolTipIcon(this.pMain, null);
            this.toolTip.SetToolTipTitle(this.pMain, "");
            this.pMain.MouseEnter += new System.EventHandler(this.pMain_MouseEnter);
            // 
            // timerLlama
            // 
            this.timerLlama.Interval = 2500;
            this.timerLlama.Tick += new System.EventHandler(this.timerLlama_Tick);
            // 
            // timerLoadProcesses
            // 
            this.timerLoadProcesses.Interval = 5000;
            this.timerLoadProcesses.Tick += new System.EventHandler(this.timerLoadProcesses_Tick);
            // 
            // timerMoreToolsUI
            // 
            this.timerMoreToolsUI.Interval = 2;
            this.timerMoreToolsUI.Tick += new System.EventHandler(this.timerMoreToolsUI_Tick);
            // 
            // snackbar
            // 
            this.snackbar.AllowDragging = false;
            this.snackbar.AllowMultipleViews = true;
            this.snackbar.ClickToClose = true;
            this.snackbar.DoubleClickToClose = true;
            this.snackbar.DurationAfterIdle = 3000;
            this.snackbar.ErrorOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.ErrorOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.ErrorOptions.ActionBorderRadius = 1;
            this.snackbar.ErrorOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.ErrorOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.ErrorOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.ErrorOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.ErrorOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(199)))));
            this.snackbar.ErrorOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.ErrorOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.ErrorOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon")));
            this.snackbar.ErrorOptions.IconLeftMargin = 12;
            this.snackbar.FadeCloseIcon = false;
            this.snackbar.Host = Bunifu.UI.WinForms.BunifuSnackbar.Hosts.FormOwner;
            this.snackbar.InformationOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.InformationOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.InformationOptions.ActionBorderRadius = 1;
            this.snackbar.InformationOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.InformationOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.InformationOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.InformationOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.InformationOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
            this.snackbar.InformationOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.InformationOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.InformationOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon1")));
            this.snackbar.InformationOptions.IconLeftMargin = 12;
            this.snackbar.Margin = 10;
            this.snackbar.MaximumSize = new System.Drawing.Size(0, 0);
            this.snackbar.MaximumViews = 7;
            this.snackbar.MessageRightMargin = 15;
            this.snackbar.MinimumSize = new System.Drawing.Size(0, 0);
            this.snackbar.ShowBorders = false;
            this.snackbar.ShowCloseIcon = false;
            this.snackbar.ShowIcon = true;
            this.snackbar.ShowShadows = true;
            this.snackbar.SuccessOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.SuccessOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.SuccessOptions.ActionBorderRadius = 1;
            this.snackbar.SuccessOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.SuccessOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.SuccessOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.SuccessOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.SuccessOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(255)))), ((int)(((byte)(237)))));
            this.snackbar.SuccessOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.SuccessOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.SuccessOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon2")));
            this.snackbar.SuccessOptions.IconLeftMargin = 12;
            this.snackbar.ViewsMargin = 7;
            this.snackbar.WarningOptions.ActionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.WarningOptions.ActionBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.snackbar.WarningOptions.ActionBorderRadius = 1;
            this.snackbar.WarningOptions.ActionFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.snackbar.WarningOptions.ActionForeColor = System.Drawing.Color.Black;
            this.snackbar.WarningOptions.BackColor = System.Drawing.Color.White;
            this.snackbar.WarningOptions.BorderColor = System.Drawing.Color.White;
            this.snackbar.WarningOptions.CloseIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(143)))));
            this.snackbar.WarningOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.snackbar.WarningOptions.ForeColor = System.Drawing.Color.Black;
            this.snackbar.WarningOptions.Icon = ((System.Drawing.Image)(resources.GetObject("resource.Icon3")));
            this.snackbar.WarningOptions.IconLeftMargin = 12;
            this.snackbar.ZoomCloseIcon = true;
            // 
            // toolTip
            // 
            this.toolTip.Active = true;
            this.toolTip.AlignTextWithTitle = false;
            this.toolTip.AllowAutoClose = false;
            this.toolTip.AllowFading = true;
            this.toolTip.AutoCloseDuration = 5000;
            this.toolTip.BackColor = System.Drawing.SystemColors.Control;
            this.toolTip.BorderColor = System.Drawing.Color.Gainsboro;
            this.toolTip.ClickToShowDisplayControl = false;
            this.toolTip.ConvertNewlinesToBreakTags = true;
            this.toolTip.DisplayControl = null;
            this.toolTip.EntryAnimationSpeed = 150;
            this.toolTip.ExitAnimationSpeed = 50;
            this.toolTip.GenerateAutoCloseDuration = false;
            this.toolTip.IconMargin = 6;
            this.toolTip.InitialDelay = 0;
            this.toolTip.Name = "toolTip";
            this.toolTip.Opacity = 1D;
            this.toolTip.OverrideToolTipTitles = false;
            this.toolTip.Padding = new System.Windows.Forms.Padding(10);
            this.toolTip.ReshowDelay = 20;
            this.toolTip.ShowAlways = true;
            this.toolTip.ShowBorders = false;
            this.toolTip.ShowIcons = true;
            this.toolTip.ShowShadows = true;
            this.toolTip.Tag = null;
            this.toolTip.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.toolTip.TextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolTip.TextMargin = 2;
            this.toolTip.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolTip.TitleForeColor = System.Drawing.Color.Black;
            this.toolTip.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.toolTip.ToolTipTitle = null;
            // 
            // lUpdateNeeded
            // 
            this.lUpdateNeeded.AutoSize = true;
            this.lUpdateNeeded.BackColor = System.Drawing.Color.Transparent;
            this.lUpdateNeeded.Cursor = System.Windows.Forms.Cursors.Hand;
            this.transition.SetDecoration(this.lUpdateNeeded, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.lUpdateNeeded.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lUpdateNeeded.ForeColor = System.Drawing.Color.DarkOrange;
            this.lUpdateNeeded.Location = new System.Drawing.Point(317, 10);
            this.lUpdateNeeded.Name = "lUpdateNeeded";
            this.lUpdateNeeded.Size = new System.Drawing.Size(187, 28);
            this.lUpdateNeeded.TabIndex = 7;
            this.lUpdateNeeded.Text = "Your game needs an update!\r\nClick to open the game updater";
            this.lUpdateNeeded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.lUpdateNeeded, "");
            this.toolTip.SetToolTipIcon(this.lUpdateNeeded, null);
            this.toolTip.SetToolTipTitle(this.lUpdateNeeded, "");
            this.lUpdateNeeded.Visible = false;
            this.lUpdateNeeded.Click += new System.EventHandler(this.lUpdateNeeded_Click);
            this.lUpdateNeeded.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.lUpdateNeeded.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // scrollbar
            // 
            this.scrollbar.AllowCursorChanges = true;
            this.scrollbar.AllowHomeEndKeysDetection = false;
            this.scrollbar.AllowIncrementalClickMoves = true;
            this.scrollbar.AllowMouseDownEffects = true;
            this.scrollbar.AllowMouseHoverEffects = true;
            this.scrollbar.AllowScrollingAnimations = true;
            this.scrollbar.AllowScrollKeysDetection = true;
            this.scrollbar.AllowScrollOptionsMenu = false;
            this.scrollbar.AllowShrinkingOnFocusLost = false;
            this.scrollbar.BackgroundColor = System.Drawing.Color.Silver;
            this.scrollbar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("scrollbar.BackgroundImage")));
            this.scrollbar.BindingContainer = this.pMain;
            this.scrollbar.BorderColor = System.Drawing.Color.Silver;
            this.scrollbar.BorderRadius = 0;
            this.scrollbar.BorderThickness = 2;
            this.transition.SetDecoration(this.scrollbar, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.scrollbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(592, 48);
            this.scrollbar.Maximum = 100;
            this.scrollbar.MaximumSize = new System.Drawing.Size(8, 0);
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.Silver;
            this.scrollbar.ShrinkSizeLimit = 3;
            this.scrollbar.Size = new System.Drawing.Size(8, 627);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 0;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 61;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.toolTip.SetToolTip(this.scrollbar, "");
            this.toolTip.SetToolTipIcon(this.scrollbar, null);
            this.toolTip.SetToolTipTitle(this.scrollbar, "");
            this.scrollbar.Value = 0;
            this.scrollbar.Visible = false;
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.transition.SetDecoration(this.header, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.header.Location = new System.Drawing.Point(0, 48);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(592, 36);
            this.header.TabIndex = 8;
            this.toolTip.SetToolTip(this.header, "");
            this.toolTip.SetToolTipIcon(this.header, null);
            this.toolTip.SetToolTipTitle(this.header, "");
            // 
            // transition
            // 
            this.transition.AnimationType = Bunifu.UI.WinForms.BunifuAnimatorNS.AnimationType.VertSlide;
            this.transition.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 0F;
            animation1.TransparencyCoeff = 0F;
            this.transition.DefaultAnimation = animation1;
            this.transition.MaxAnimationTime = 500;
            this.transition.TimeStep = 0.03F;
            // 
            // timerShowMessage
            // 
            this.timerShowMessage.Interval = 1500;
            this.timerShowMessage.Tick += new System.EventHandler(this.timerShowMessage_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(600, 675);
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.header);
            this.Controls.Add(this.lUpdateNeeded);
            this.Controls.Add(this.scrollbar);
            this.Controls.Add(this.pTop);
            this.transition.SetDecoration(this, Bunifu.UI.WinForms.BunifuTransition.DecorationType.None);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.LocationChanged += new System.EventHandler(this.FrmMain_LocationChanged);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            this.pBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDarkmode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label lDev;
        private System.Windows.Forms.Timer timerLlama;
        private System.Windows.Forms.Label lVersion;
        public System.Windows.Forms.Timer timerLoadProcesses;
        private System.Windows.Forms.Timer timerMoreToolsUI;
        private RoundPictureBox pbDarkmode;
        public Bunifu.UI.WinForms.BunifuSnackbar snackbar;
        public Bunifu.UI.WinForms.BunifuToolTip toolTip;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private System.Windows.Forms.Label lUpdateNeeded;
        private AccountUIHeader header;
        private Bunifu.UI.WinForms.BunifuTransition transition;
        private System.Windows.Forms.Label lUpdateAvailable;
        private System.Windows.Forms.Timer timerShowMessage;
    }
}

