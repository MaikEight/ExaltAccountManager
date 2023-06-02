namespace ExaltAccountManager
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.pLeftSide = new System.Windows.Forms.Panel();
            this.pSideButtons = new System.Windows.Forms.Panel();
            this.pUpdate = new System.Windows.Forms.Panel();
            this.pBottom = new System.Windows.Forms.Panel();
            this.lVersion = new System.Windows.Forms.Label();
            this.pSpacer = new System.Windows.Forms.Panel();
            this.pHeader = new System.Windows.Forms.Panel();
            this.lHeaderEAM = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.btnSwitchDesign = new System.Windows.Forms.Button();
            this.lTitle = new System.Windows.Forms.Label();
            this.pContent = new System.Windows.Forms.Panel();
            this.bunifuForm = new Bunifu.UI.WinForms.BunifuFormDock();
            this.snackbar = new Bunifu.UI.WinForms.BunifuSnackbar(this.components);
            this.timerLogBlink = new System.Windows.Forms.Timer(this.components);
            this.timerLoadUI = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.timerDiscordUpdater = new System.Windows.Forms.Timer(this.components);
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.btnEAMUpdate = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.pSideBar = new System.Windows.Forms.PictureBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnLogs = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnModules = new System.Windows.Forms.Button();
            this.btnAddAccount = new System.Windows.Forms.Button();
            this.btnNews = new System.Windows.Forms.Button();
            this.btnAccounts = new System.Windows.Forms.Button();
            this.btnGameUpdater = new System.Windows.Forms.Button();
            this.pbHeader = new System.Windows.Forms.PictureBox();
            this.pbShowDiscordUser = new System.Windows.Forms.PictureBox();
            this.pLeftSide.SuspendLayout();
            this.pSideButtons.SuspendLayout();
            this.pUpdate.SuspendLayout();
            this.pBottom.SuspendLayout();
            this.pHeader.SuspendLayout();
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSideBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShowDiscordUser)).BeginInit();
            this.SuspendLayout();
            // 
            // pLeftSide
            // 
            this.pLeftSide.Controls.Add(this.pSideButtons);
            this.pLeftSide.Controls.Add(this.pSpacer);
            this.pLeftSide.Controls.Add(this.pHeader);
            this.pLeftSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeftSide.Location = new System.Drawing.Point(0, 0);
            this.pLeftSide.Name = "pLeftSide";
            this.pLeftSide.Size = new System.Drawing.Size(175, 576);
            this.pLeftSide.TabIndex = 2;
            this.toolTip.SetToolTip(this.pLeftSide, "");
            this.toolTip.SetToolTipIcon(this.pLeftSide, null);
            this.toolTip.SetToolTipTitle(this.pLeftSide, "");
            // 
            // pSideButtons
            // 
            this.pSideButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pSideButtons.Controls.Add(this.pUpdate);
            this.pSideButtons.Controls.Add(this.pBottom);
            this.pSideButtons.Controls.Add(this.pSideBar);
            this.pSideButtons.Controls.Add(this.btnAbout);
            this.pSideButtons.Controls.Add(this.btnLogs);
            this.pSideButtons.Controls.Add(this.btnHelp);
            this.pSideButtons.Controls.Add(this.btnOptions);
            this.pSideButtons.Controls.Add(this.btnModules);
            this.pSideButtons.Controls.Add(this.btnAddAccount);
            this.pSideButtons.Controls.Add(this.btnNews);
            this.pSideButtons.Controls.Add(this.btnAccounts);
            this.pSideButtons.Controls.Add(this.btnGameUpdater);
            this.pSideButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSideButtons.Location = new System.Drawing.Point(0, 66);
            this.pSideButtons.Name = "pSideButtons";
            this.pSideButtons.Size = new System.Drawing.Size(175, 510);
            this.pSideButtons.TabIndex = 7;
            this.toolTip.SetToolTip(this.pSideButtons, "");
            this.toolTip.SetToolTipIcon(this.pSideButtons, null);
            this.toolTip.SetToolTipTitle(this.pSideButtons, "");
            // 
            // pUpdate
            // 
            this.pUpdate.Controls.Add(this.btnEAMUpdate);
            this.pUpdate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pUpdate.Location = new System.Drawing.Point(0, 464);
            this.pUpdate.Name = "pUpdate";
            this.pUpdate.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pUpdate.Size = new System.Drawing.Size(175, 31);
            this.pUpdate.TabIndex = 26;
            this.toolTip.SetToolTip(this.pUpdate, "");
            this.toolTip.SetToolTipIcon(this.pUpdate, null);
            this.toolTip.SetToolTipTitle(this.pUpdate, "");
            this.pUpdate.Visible = false;
            // 
            // pBottom
            // 
            this.pBottom.Controls.Add(this.lVersion);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(0, 495);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(175, 15);
            this.pBottom.TabIndex = 9;
            this.toolTip.SetToolTip(this.pBottom, "");
            this.toolTip.SetToolTipIcon(this.pBottom, null);
            this.toolTip.SetToolTipTitle(this.pBottom, "");
            this.pBottom.Paint += new System.Windows.Forms.PaintEventHandler(this.pBottom_Paint);
            // 
            // lVersion
            // 
            this.lVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lVersion.AutoSize = true;
            this.lVersion.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lVersion.Location = new System.Drawing.Point(3, 2);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(78, 13);
            this.lVersion.TabIndex = 7;
            this.lVersion.Text = "v3.0.0 by Maik8";
            this.toolTip.SetToolTip(this.lVersion, "");
            this.toolTip.SetToolTipIcon(this.lVersion, null);
            this.toolTip.SetToolTipTitle(this.lVersion, "");
            this.lVersion.Paint += new System.Windows.Forms.PaintEventHandler(this.lVersion_Paint);
            // 
            // pSpacer
            // 
            this.pSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSpacer.Location = new System.Drawing.Point(0, 64);
            this.pSpacer.Name = "pSpacer";
            this.pSpacer.Size = new System.Drawing.Size(175, 2);
            this.pSpacer.TabIndex = 6;
            this.toolTip.SetToolTip(this.pSpacer, "");
            this.toolTip.SetToolTipIcon(this.pSpacer, null);
            this.toolTip.SetToolTipTitle(this.pSpacer, "");
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pHeader.Controls.Add(this.pbHeader);
            this.pHeader.Controls.Add(this.lHeaderEAM);
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(175, 64);
            this.pHeader.TabIndex = 5;
            this.toolTip.SetToolTip(this.pHeader, "");
            this.toolTip.SetToolTipIcon(this.pHeader, null);
            this.toolTip.SetToolTipTitle(this.pHeader, "");
            // 
            // lHeaderEAM
            // 
            this.lHeaderEAM.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeaderEAM.ForeColor = System.Drawing.Color.White;
            this.lHeaderEAM.Location = new System.Drawing.Point(55, 2);
            this.lHeaderEAM.Name = "lHeaderEAM";
            this.lHeaderEAM.Size = new System.Drawing.Size(115, 60);
            this.lHeaderEAM.TabIndex = 5;
            this.lHeaderEAM.Text = "Exalt Account Manager";
            this.lHeaderEAM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.lHeaderEAM, "");
            this.toolTip.SetToolTipIcon(this.lHeaderEAM, null);
            this.toolTip.SetToolTipTitle(this.lHeaderEAM, "");
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pTop.Controls.Add(this.pbShowDiscordUser);
            this.pTop.Controls.Add(this.btnSwitchDesign);
            this.pTop.Controls.Add(this.pbMinimize);
            this.pTop.Controls.Add(this.pbClose);
            this.pTop.Controls.Add(this.lTitle);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(175, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(678, 24);
            this.pTop.TabIndex = 3;
            this.toolTip.SetToolTip(this.pTop, "");
            this.toolTip.SetToolTipIcon(this.pTop, null);
            this.toolTip.SetToolTipTitle(this.pTop, "");
            // 
            // btnSwitchDesign
            // 
            this.btnSwitchDesign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchDesign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSwitchDesign.Location = new System.Drawing.Point(526, -2);
            this.btnSwitchDesign.Name = "btnSwitchDesign";
            this.btnSwitchDesign.Size = new System.Drawing.Size(103, 28);
            this.btnSwitchDesign.TabIndex = 9;
            this.btnSwitchDesign.Text = "Switch Theme";
            this.toolTip.SetToolTip(this.btnSwitchDesign, "");
            this.toolTip.SetToolTipIcon(this.btnSwitchDesign, null);
            this.toolTip.SetToolTipTitle(this.btnSwitchDesign, "");
            this.btnSwitchDesign.UseVisualStyleBackColor = true;
            this.btnSwitchDesign.Click += new System.EventHandler(this.btnSwitchDesign_Click);
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(6, 3);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(78, 21);
            this.lTitle.TabIndex = 6;
            this.lTitle.Text = "Accounts";
            this.toolTip.SetToolTip(this.lTitle, "");
            this.toolTip.SetToolTipIcon(this.lTitle, null);
            this.toolTip.SetToolTipTitle(this.lTitle, "");
            this.lTitle.UseMnemonic = false;
            // 
            // pContent
            // 
            this.pContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pContent.Location = new System.Drawing.Point(175, 24);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(677, 551);
            this.pContent.TabIndex = 4;
            this.toolTip.SetToolTip(this.pContent, "");
            this.toolTip.SetToolTipIcon(this.pContent, null);
            this.toolTip.SetToolTipTitle(this.pContent, "");
            // 
            // bunifuForm
            // 
            this.bunifuForm.AllowFormDragging = true;
            this.bunifuForm.AllowFormDropShadow = true;
            this.bunifuForm.AllowFormResizing = true;
            this.bunifuForm.AllowHidingBottomRegion = true;
            this.bunifuForm.AllowOpacityChangesWhileDragging = false;
            this.bunifuForm.BorderOptions.BottomBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuForm.BorderOptions.BottomBorder.BorderThickness = 1;
            this.bunifuForm.BorderOptions.BottomBorder.ShowBorder = true;
            this.bunifuForm.BorderOptions.LeftBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuForm.BorderOptions.LeftBorder.BorderThickness = 1;
            this.bunifuForm.BorderOptions.LeftBorder.ShowBorder = true;
            this.bunifuForm.BorderOptions.RightBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuForm.BorderOptions.RightBorder.BorderThickness = 1;
            this.bunifuForm.BorderOptions.RightBorder.ShowBorder = true;
            this.bunifuForm.BorderOptions.TopBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuForm.BorderOptions.TopBorder.BorderThickness = 1;
            this.bunifuForm.BorderOptions.TopBorder.ShowBorder = true;
            this.bunifuForm.ContainerControl = this;
            this.bunifuForm.DockingIndicatorsColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(215)))), ((int)(((byte)(233)))));
            this.bunifuForm.DockingIndicatorsOpacity = 0.5D;
            this.bunifuForm.DockingOptions.DockAll = true;
            this.bunifuForm.DockingOptions.DockBottomLeft = true;
            this.bunifuForm.DockingOptions.DockBottomRight = true;
            this.bunifuForm.DockingOptions.DockFullScreen = true;
            this.bunifuForm.DockingOptions.DockLeft = true;
            this.bunifuForm.DockingOptions.DockRight = true;
            this.bunifuForm.DockingOptions.DockTopLeft = true;
            this.bunifuForm.DockingOptions.DockTopRight = true;
            this.bunifuForm.FormDraggingOpacity = 0.9D;
            this.bunifuForm.ParentForm = this;
            this.bunifuForm.ShowCursorChanges = true;
            this.bunifuForm.ShowDockingIndicators = true;
            this.bunifuForm.TitleBarOptions.AllowFormDragging = true;
            this.bunifuForm.TitleBarOptions.BunifuFormDock = this.bunifuForm;
            this.bunifuForm.TitleBarOptions.DoubleClickToExpandWindow = true;
            this.bunifuForm.TitleBarOptions.TitleBarControl = this.pTop;
            this.bunifuForm.TitleBarOptions.UseBackColorOnDockingIndicators = false;
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
            this.snackbar.MessageTopMargin = 0;
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
            // timerLogBlink
            // 
            this.timerLogBlink.Interval = 500;
            this.timerLogBlink.Tick += new System.EventHandler(this.timerLogBlink_Tick);
            // 
            // timerLoadUI
            // 
            this.timerLoadUI.Interval = 350;
            this.timerLoadUI.Tick += new System.EventHandler(this.timerLoadUI_Tick);
            // 
            // toolTip
            // 
            this.toolTip.Active = true;
            this.toolTip.AlignTextWithTitle = true;
            this.toolTip.AllowAutoClose = false;
            this.toolTip.AllowFading = true;
            this.toolTip.AutoCloseDuration = 5000;
            this.toolTip.BackColor = System.Drawing.SystemColors.Control;
            this.toolTip.BorderColor = System.Drawing.Color.Gainsboro;
            this.toolTip.ClickToShowDisplayControl = false;
            this.toolTip.ConvertNewlinesToBreakTags = true;
            this.toolTip.DisplayControl = null;
            this.toolTip.EntryAnimationSpeed = 350;
            this.toolTip.ExitAnimationSpeed = 200;
            this.toolTip.GenerateAutoCloseDuration = false;
            this.toolTip.IconMargin = 6;
            this.toolTip.InitialDelay = 0;
            this.toolTip.Name = "toolTip";
            this.toolTip.Opacity = 1D;
            this.toolTip.OverrideToolTipTitles = false;
            this.toolTip.Padding = new System.Windows.Forms.Padding(10);
            this.toolTip.ReshowDelay = 100;
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
            // timerDiscordUpdater
            // 
            this.timerDiscordUpdater.Interval = 5000;
            this.timerDiscordUpdater.Tick += new System.EventHandler(this.timerDiscordUpdater_Tick);
            // 
            // pbMinimize
            // 
            this.pbMinimize.Image = global::ExaltAccountManager.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(630, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMinimize.TabIndex = 8;
            this.pbMinimize.TabStop = false;
            this.toolTip.SetToolTip(this.pbMinimize, "");
            this.toolTip.SetToolTipIcon(this.pbMinimize, null);
            this.toolTip.SetToolTipTitle(this.pbMinimize, "");
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMinimize_MouseDown);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            this.pbMinimize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbMinimize_MouseUp);
            // 
            // pbClose
            // 
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(654, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 7;
            this.pbClose.TabStop = false;
            this.toolTip.SetToolTip(this.pbClose, "");
            this.toolTip.SetToolTipIcon(this.pbClose, null);
            this.toolTip.SetToolTipTitle(this.pbClose, "");
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbClose_MouseDown);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            this.pbClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbClose_MouseUp);
            // 
            // btnEAMUpdate
            // 
            this.btnEAMUpdate.AllowAnimations = true;
            this.btnEAMUpdate.AllowMouseEffects = true;
            this.btnEAMUpdate.AllowToggling = false;
            this.btnEAMUpdate.AnimationSpeed = 200;
            this.btnEAMUpdate.AutoGenerateColors = false;
            this.btnEAMUpdate.AutoRoundBorders = false;
            this.btnEAMUpdate.AutoSizeLeftIcon = true;
            this.btnEAMUpdate.AutoSizeRightIcon = true;
            this.btnEAMUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnEAMUpdate.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEAMUpdate.BackgroundImage")));
            this.btnEAMUpdate.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnEAMUpdate.ButtonText = "Update EAM";
            this.btnEAMUpdate.ButtonTextMarginLeft = 0;
            this.btnEAMUpdate.ColorContrastOnClick = 45;
            this.btnEAMUpdate.ColorContrastOnHover = 45;
            this.btnEAMUpdate.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnEAMUpdate.CustomizableEdges = borderEdges1;
            this.btnEAMUpdate.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnEAMUpdate.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnEAMUpdate.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnEAMUpdate.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnEAMUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEAMUpdate.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnEAMUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEAMUpdate.ForeColor = System.Drawing.Color.White;
            this.btnEAMUpdate.IconLeft = global::ExaltAccountManager.Properties.Resources._2;
            this.btnEAMUpdate.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEAMUpdate.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnEAMUpdate.IconLeftPadding = new System.Windows.Forms.Padding(5, 3, 1, 3);
            this.btnEAMUpdate.IconMarginLeft = 11;
            this.btnEAMUpdate.IconPadding = 4;
            this.btnEAMUpdate.IconRight = null;
            this.btnEAMUpdate.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEAMUpdate.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnEAMUpdate.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnEAMUpdate.IconSize = 10;
            this.btnEAMUpdate.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.IdleBorderRadius = 5;
            this.btnEAMUpdate.IdleBorderThickness = 1;
            this.btnEAMUpdate.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources._2;
            this.btnEAMUpdate.IdleIconRightImage = null;
            this.btnEAMUpdate.IndicateFocus = false;
            this.btnEAMUpdate.Location = new System.Drawing.Point(5, 0);
            this.btnEAMUpdate.Name = "btnEAMUpdate";
            this.btnEAMUpdate.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnEAMUpdate.OnDisabledState.BorderRadius = 5;
            this.btnEAMUpdate.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnEAMUpdate.OnDisabledState.BorderThickness = 1;
            this.btnEAMUpdate.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnEAMUpdate.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnEAMUpdate.OnDisabledState.IconLeftImage = null;
            this.btnEAMUpdate.OnDisabledState.IconRightImage = null;
            this.btnEAMUpdate.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.onHoverState.BorderRadius = 5;
            this.btnEAMUpdate.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnEAMUpdate.onHoverState.BorderThickness = 1;
            this.btnEAMUpdate.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnEAMUpdate.onHoverState.IconLeftImage = null;
            this.btnEAMUpdate.onHoverState.IconRightImage = null;
            this.btnEAMUpdate.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.OnIdleState.BorderRadius = 5;
            this.btnEAMUpdate.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnEAMUpdate.OnIdleState.BorderThickness = 1;
            this.btnEAMUpdate.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnEAMUpdate.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources._2;
            this.btnEAMUpdate.OnIdleState.IconRightImage = null;
            this.btnEAMUpdate.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.OnPressedState.BorderRadius = 5;
            this.btnEAMUpdate.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnEAMUpdate.OnPressedState.BorderThickness = 1;
            this.btnEAMUpdate.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnEAMUpdate.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnEAMUpdate.OnPressedState.IconLeftImage = null;
            this.btnEAMUpdate.OnPressedState.IconRightImage = null;
            this.btnEAMUpdate.Size = new System.Drawing.Size(165, 31);
            this.btnEAMUpdate.TabIndex = 25;
            this.btnEAMUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEAMUpdate.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnEAMUpdate.TextMarginLeft = 0;
            this.btnEAMUpdate.TextPadding = new System.Windows.Forms.Padding(13, 0, 15, 0);
            this.toolTip.SetToolTip(this.btnEAMUpdate, "");
            this.toolTip.SetToolTipIcon(this.btnEAMUpdate, null);
            this.toolTip.SetToolTipTitle(this.btnEAMUpdate, "");
            this.btnEAMUpdate.UseDefaultRadiusAndThickness = true;
            this.btnEAMUpdate.Click += new System.EventHandler(this.btnEAMUpdate_Click);
            // 
            // pSideBar
            // 
            this.pSideBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pSideBar.Location = new System.Drawing.Point(0, 3);
            this.pSideBar.Name = "pSideBar";
            this.pSideBar.Size = new System.Drawing.Size(5, 30);
            this.pSideBar.TabIndex = 5;
            this.pSideBar.TabStop = false;
            this.toolTip.SetToolTip(this.pSideBar, "");
            this.toolTip.SetToolTipIcon(this.pSideBar, null);
            this.toolTip.SetToolTipTitle(this.pSideBar, "");
            // 
            // btnAbout
            // 
            this.btnAbout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAbout.FlatAppearance.BorderSize = 0;
            this.btnAbout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Image = global::ExaltAccountManager.Properties.Resources.ic_info_outline_black_24dp2;
            this.btnAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.Location = new System.Drawing.Point(0, 320);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(0);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAbout.Size = new System.Drawing.Size(175, 40);
            this.btnAbout.TabIndex = 8;
            this.btnAbout.Text = "   About EAM";
            this.btnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnAbout, "");
            this.toolTip.SetToolTipIcon(this.btnAbout, null);
            this.toolTip.SetToolTipTitle(this.btnAbout, "");
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            this.btnAbout.MouseEnter += new System.EventHandler(this.btnAbout_MouseEnter);
            this.btnAbout.MouseLeave += new System.EventHandler(this.btnAbout_MouseLeave);
            // 
            // btnLogs
            // 
            this.btnLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogs.FlatAppearance.BorderSize = 0;
            this.btnLogs.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLogs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogs.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogs.Image = global::ExaltAccountManager.Properties.Resources.activity_history_outline_24px;
            this.btnLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogs.Location = new System.Drawing.Point(0, 280);
            this.btnLogs.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogs.Name = "btnLogs";
            this.btnLogs.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnLogs.Size = new System.Drawing.Size(175, 40);
            this.btnLogs.TabIndex = 7;
            this.btnLogs.Text = "   Logs";
            this.btnLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnLogs, "");
            this.toolTip.SetToolTipIcon(this.btnLogs, null);
            this.toolTip.SetToolTipTitle(this.btnLogs, "");
            this.btnLogs.UseVisualStyleBackColor = true;
            this.btnLogs.Click += new System.EventHandler(this.btnLogs_Click);
            this.btnLogs.MouseEnter += new System.EventHandler(this.btnLogs_MouseEnter);
            this.btnLogs.MouseLeave += new System.EventHandler(this.btnLogs_MouseLeave);
            // 
            // btnHelp
            // 
            this.btnHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.Image = global::ExaltAccountManager.Properties.Resources.ic_help_outline_black_24dp;
            this.btnHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHelp.Location = new System.Drawing.Point(0, 240);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnHelp.Size = new System.Drawing.Size(175, 40);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "   Help";
            this.btnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnHelp, "");
            this.toolTip.SetToolTipIcon(this.btnHelp, null);
            this.toolTip.SetToolTipTitle(this.btnHelp, "");
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            this.btnHelp.MouseEnter += new System.EventHandler(this.btnHelp_MouseEnter);
            this.btnHelp.MouseLeave += new System.EventHandler(this.btnHelp_MouseLeave);
            // 
            // btnOptions
            // 
            this.btnOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOptions.FlatAppearance.BorderSize = 0;
            this.btnOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnOptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptions.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptions.Image = global::ExaltAccountManager.Properties.Resources.settings_outline_24px;
            this.btnOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptions.Location = new System.Drawing.Point(0, 200);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(0);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnOptions.Size = new System.Drawing.Size(175, 40);
            this.btnOptions.TabIndex = 5;
            this.btnOptions.Text = "   Options";
            this.btnOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnOptions, "");
            this.toolTip.SetToolTipIcon(this.btnOptions, null);
            this.toolTip.SetToolTipTitle(this.btnOptions, "");
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            this.btnOptions.Paint += new System.Windows.Forms.PaintEventHandler(this.btnOptions_Paint);
            this.btnOptions.MouseEnter += new System.EventHandler(this.btnOptions_MouseEnter);
            this.btnOptions.MouseLeave += new System.EventHandler(this.btnOptions_MouseLeave);
            // 
            // btnModules
            // 
            this.btnModules.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnModules.FlatAppearance.BorderSize = 0;
            this.btnModules.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnModules.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnModules.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModules.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModules.Image = global::ExaltAccountManager.Properties.Resources.dashboard_layout_outline_24px;
            this.btnModules.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModules.Location = new System.Drawing.Point(0, 160);
            this.btnModules.Margin = new System.Windows.Forms.Padding(0);
            this.btnModules.Name = "btnModules";
            this.btnModules.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnModules.Size = new System.Drawing.Size(175, 40);
            this.btnModules.TabIndex = 4;
            this.btnModules.Text = "   Modules";
            this.btnModules.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModules.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnModules, "");
            this.toolTip.SetToolTipIcon(this.btnModules, null);
            this.toolTip.SetToolTipTitle(this.btnModules, "");
            this.btnModules.UseVisualStyleBackColor = true;
            this.btnModules.Click += new System.EventHandler(this.btnModules_Click);
            this.btnModules.MouseEnter += new System.EventHandler(this.btnModules_MouseEnter);
            this.btnModules.MouseLeave += new System.EventHandler(this.btnModules_MouseLeave);
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddAccount.FlatAppearance.BorderSize = 0;
            this.btnAddAccount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAddAccount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAddAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAccount.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAccount.Image = global::ExaltAccountManager.Properties.Resources.add_user_outline_24px;
            this.btnAddAccount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddAccount.Location = new System.Drawing.Point(0, 120);
            this.btnAddAccount.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAddAccount.Size = new System.Drawing.Size(175, 40);
            this.btnAddAccount.TabIndex = 3;
            this.btnAddAccount.Text = "   Add Account";
            this.btnAddAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnAddAccount, "");
            this.toolTip.SetToolTipIcon(this.btnAddAccount, null);
            this.toolTip.SetToolTipTitle(this.btnAddAccount, "");
            this.btnAddAccount.UseVisualStyleBackColor = true;
            this.btnAddAccount.Click += new System.EventHandler(this.btnAddAccount_Click);
            this.btnAddAccount.MouseEnter += new System.EventHandler(this.btnAddAccount_MouseEnter);
            this.btnAddAccount.MouseLeave += new System.EventHandler(this.btnAddAccount_MouseLeave);
            // 
            // btnNews
            // 
            this.btnNews.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNews.FlatAppearance.BorderSize = 0;
            this.btnNews.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNews.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNews.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNews.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNews.Image = global::ExaltAccountManager.Properties.Resources.news_outline_black_24px;
            this.btnNews.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNews.Location = new System.Drawing.Point(0, 80);
            this.btnNews.Margin = new System.Windows.Forms.Padding(0);
            this.btnNews.Name = "btnNews";
            this.btnNews.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnNews.Size = new System.Drawing.Size(175, 40);
            this.btnNews.TabIndex = 27;
            this.btnNews.Text = "   News";
            this.btnNews.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNews.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnNews, "");
            this.toolTip.SetToolTipIcon(this.btnNews, null);
            this.toolTip.SetToolTipTitle(this.btnNews, "");
            this.btnNews.UseVisualStyleBackColor = true;
            this.btnNews.Click += new System.EventHandler(this.btnNews_Click);
            this.btnNews.Paint += new System.Windows.Forms.PaintEventHandler(this.btnNews_Paint);
            this.btnNews.MouseEnter += new System.EventHandler(this.btnNews_MouseEnter);
            this.btnNews.MouseLeave += new System.EventHandler(this.btnNews_MouseLeave);
            // 
            // btnAccounts
            // 
            this.btnAccounts.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAccounts.FlatAppearance.BorderSize = 0;
            this.btnAccounts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccounts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccounts.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccounts.Image = global::ExaltAccountManager.Properties.Resources.ic_people_outline_black_24dp;
            this.btnAccounts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAccounts.Location = new System.Drawing.Point(0, 40);
            this.btnAccounts.Margin = new System.Windows.Forms.Padding(0);
            this.btnAccounts.Name = "btnAccounts";
            this.btnAccounts.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAccounts.Size = new System.Drawing.Size(175, 40);
            this.btnAccounts.TabIndex = 2;
            this.btnAccounts.Text = "   Accounts";
            this.btnAccounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAccounts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnAccounts, "");
            this.toolTip.SetToolTipIcon(this.btnAccounts, null);
            this.toolTip.SetToolTipTitle(this.btnAccounts, "");
            this.btnAccounts.UseVisualStyleBackColor = true;
            this.btnAccounts.Click += new System.EventHandler(this.btnAccounts_Click);
            this.btnAccounts.MouseEnter += new System.EventHandler(this.btnAccounts_MouseEnter);
            this.btnAccounts.MouseLeave += new System.EventHandler(this.btnAccounts_MouseLeave);
            // 
            // btnGameUpdater
            // 
            this.btnGameUpdater.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGameUpdater.FlatAppearance.BorderSize = 0;
            this.btnGameUpdater.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnGameUpdater.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnGameUpdater.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameUpdater.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGameUpdater.Image = global::ExaltAccountManager.Properties.Resources.update_left_rotation_24px;
            this.btnGameUpdater.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGameUpdater.Location = new System.Drawing.Point(0, 0);
            this.btnGameUpdater.Margin = new System.Windows.Forms.Padding(0);
            this.btnGameUpdater.Name = "btnGameUpdater";
            this.btnGameUpdater.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnGameUpdater.Size = new System.Drawing.Size(175, 40);
            this.btnGameUpdater.TabIndex = 1;
            this.btnGameUpdater.Text = "   Game Updater";
            this.btnGameUpdater.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGameUpdater.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnGameUpdater, "");
            this.toolTip.SetToolTipIcon(this.btnGameUpdater, null);
            this.toolTip.SetToolTipTitle(this.btnGameUpdater, "");
            this.btnGameUpdater.UseVisualStyleBackColor = true;
            this.btnGameUpdater.Visible = false;
            this.btnGameUpdater.Click += new System.EventHandler(this.btnGameUpdater_Click);
            this.btnGameUpdater.MouseEnter += new System.EventHandler(this.btnGameUpdater_MouseEnter);
            this.btnGameUpdater.MouseLeave += new System.EventHandler(this.btnGameUpdater_MouseLeave);
            // 
            // pbHeader
            // 
            this.pbHeader.Image = global::ExaltAccountManager.Properties.Resources.logo;
            this.pbHeader.Location = new System.Drawing.Point(8, 8);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(48, 48);
            this.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbHeader.TabIndex = 6;
            this.pbHeader.TabStop = false;
            this.toolTip.SetToolTip(this.pbHeader, "");
            this.toolTip.SetToolTipIcon(this.pbHeader, null);
            this.toolTip.SetToolTipTitle(this.pbHeader, "");
            // 
            // pbShowDiscordUser
            // 
            this.pbShowDiscordUser.Image = global::ExaltAccountManager.Properties.Resources.male_user_outline_black_24px;
            this.pbShowDiscordUser.Location = new System.Drawing.Point(497, 0);
            this.pbShowDiscordUser.Name = "pbShowDiscordUser";
            this.pbShowDiscordUser.Size = new System.Drawing.Size(28, 24);
            this.pbShowDiscordUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbShowDiscordUser.TabIndex = 11;
            this.pbShowDiscordUser.TabStop = false;
            this.toolTip.SetToolTip(this.pbShowDiscordUser, "");
            this.toolTip.SetToolTipIcon(this.pbShowDiscordUser, null);
            this.toolTip.SetToolTipTitle(this.pbShowDiscordUser, "");
            this.pbShowDiscordUser.Visible = false;
            this.pbShowDiscordUser.Click += new System.EventHandler(this.pbShowDiscordUser_Click);
            this.pbShowDiscordUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbShowDiscordUser_MouseDown);
            this.pbShowDiscordUser.MouseEnter += new System.EventHandler(this.pbShowDiscordUser_MouseEnter);
            this.pbShowDiscordUser.MouseLeave += new System.EventHandler(this.pbShowDiscordUser_MouseLeave);
            this.pbShowDiscordUser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbShowDiscordUser_MouseUp);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(853, 576);
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.pTop);
            this.Controls.Add(this.pLeftSide);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MinimumSize = new System.Drawing.Size(853, 526);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exalt Account Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.LocationChanged += new System.EventHandler(this.FrmMain_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmMain_Paint);
            this.pLeftSide.ResumeLayout(false);
            this.pSideButtons.ResumeLayout(false);
            this.pUpdate.ResumeLayout(false);
            this.pBottom.ResumeLayout(false);
            this.pBottom.PerformLayout();
            this.pHeader.ResumeLayout(false);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSideBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShowDiscordUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pLeftSide;
        private System.Windows.Forms.Panel pSideButtons;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnLogs;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnModules;
        private System.Windows.Forms.PictureBox pSideBar;
        private System.Windows.Forms.Button btnAddAccount;
        private System.Windows.Forms.Button btnAccounts;
        private System.Windows.Forms.Panel pSpacer;
        private System.Windows.Forms.Panel pHeader;
        private System.Windows.Forms.PictureBox pbHeader;
        private System.Windows.Forms.Label lHeaderEAM;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Panel pContent;
        private Bunifu.UI.WinForms.BunifuFormDock bunifuForm;
        private System.Windows.Forms.Button btnSwitchDesign;
        private System.Windows.Forms.Timer timerLogBlink;
        private Bunifu.UI.WinForms.BunifuSnackbar snackbar;
        private System.Windows.Forms.Timer timerLoadUI;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.Button btnGameUpdater;
        private Bunifu.UI.WinForms.BunifuToolTip toolTip;
        private System.Windows.Forms.Panel pBottom;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnEAMUpdate;
        private System.Windows.Forms.Panel pUpdate;
        private System.Windows.Forms.Timer timerDiscordUpdater;
        private System.Windows.Forms.Button btnNews;
        private System.Windows.Forms.PictureBox pbShowDiscordUser;
    }
}