namespace ExaltAccountManager
{
    partial class FrmDiscordPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDiscordPopup));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges4 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges5 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges6 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.pTop = new System.Windows.Forms.Panel();
            this.pTopBar = new System.Windows.Forms.Panel();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pHeader = new System.Windows.Forms.Panel();
            this.pbHeader = new System.Windows.Forms.PictureBox();
            this.lHeaderEAM = new System.Windows.Forms.Label();
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.btnNever = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnLater = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnYeah = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.lQuestion = new System.Windows.Forms.Label();
            this.lHeadline = new System.Windows.Forms.Label();
            this.pContent = new System.Windows.Forms.Panel();
            this.pTop.SuspendLayout();
            this.pTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.pHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            this.shadow.SuspendLayout();
            this.pContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.pTopBar);
            this.pTop.Controls.Add(this.pHeader);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(500, 64);
            this.pTop.TabIndex = 0;
            // 
            // pTopBar
            // 
            this.pTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pTopBar.Controls.Add(this.pbClose);
            this.pTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTopBar.Location = new System.Drawing.Point(175, 0);
            this.pTopBar.Name = "pTopBar";
            this.pTopBar.Size = new System.Drawing.Size(325, 24);
            this.pTopBar.TabIndex = 2;
            // 
            // pbClose
            // 
            this.pbClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(301, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 8;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbClose_MouseDown);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pHeader.Controls.Add(this.pbHeader);
            this.pHeader.Controls.Add(this.lHeaderEAM);
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(175, 64);
            this.pHeader.TabIndex = 1;
            // 
            // pbHeader
            // 
            this.pbHeader.Image = global::ExaltAccountManager.Properties.Resources.logo;
            this.pbHeader.Location = new System.Drawing.Point(8, 8);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(48, 48);
            this.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbHeader.TabIndex = 7;
            this.pbHeader.TabStop = false;
            // 
            // lHeaderEAM
            // 
            this.lHeaderEAM.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeaderEAM.ForeColor = System.Drawing.Color.White;
            this.lHeaderEAM.Location = new System.Drawing.Point(55, 2);
            this.lHeaderEAM.Name = "lHeaderEAM";
            this.lHeaderEAM.Size = new System.Drawing.Size(115, 60);
            this.lHeaderEAM.TabIndex = 8;
            this.lHeaderEAM.Text = "Exalt Account Manager";
            this.lHeaderEAM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shadow
            // 
            this.shadow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadow.BorderRadius = 9;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.btnNever);
            this.shadow.Controls.Add(this.btnLater);
            this.shadow.Controls.Add(this.btnYeah);
            this.shadow.Controls.Add(this.lQuestion);
            this.shadow.Controls.Add(this.lHeadline);
            this.shadow.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadow.Location = new System.Drawing.Point(10, 10);
            this.shadow.Name = "shadow";
            this.shadow.PanelColor = System.Drawing.Color.White;
            this.shadow.PanelColor2 = System.Drawing.Color.White;
            this.shadow.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadow.ShadowDept = 2;
            this.shadow.ShadowDepth = 4;
            this.shadow.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadow.ShadowTopLeftVisible = false;
            this.shadow.Size = new System.Drawing.Size(480, 225);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 21;
            // 
            // btnNever
            // 
            this.btnNever.AllowAnimations = true;
            this.btnNever.AllowMouseEffects = true;
            this.btnNever.AllowToggling = false;
            this.btnNever.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNever.AnimationSpeed = 200;
            this.btnNever.AutoGenerateColors = false;
            this.btnNever.AutoRoundBorders = false;
            this.btnNever.AutoSizeLeftIcon = true;
            this.btnNever.AutoSizeRightIcon = true;
            this.btnNever.BackColor = System.Drawing.Color.Transparent;
            this.btnNever.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNever.BackgroundImage")));
            this.btnNever.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnNever.ButtonText = "Don\'t ask me again";
            this.btnNever.ButtonTextMarginLeft = 0;
            this.btnNever.ColorContrastOnClick = 45;
            this.btnNever.ColorContrastOnHover = 45;
            this.btnNever.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges4.BottomLeft = true;
            borderEdges4.BottomRight = true;
            borderEdges4.TopLeft = true;
            borderEdges4.TopRight = true;
            this.btnNever.CustomizableEdges = borderEdges4;
            this.btnNever.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnNever.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnNever.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnNever.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnNever.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnNever.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNever.ForeColor = System.Drawing.Color.White;
            this.btnNever.IconLeft = global::ExaltAccountManager.Properties.Resources.ic_do_not_disturb_white_36dp;
            this.btnNever.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNever.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnNever.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnNever.IconMarginLeft = 11;
            this.btnNever.IconPadding = 6;
            this.btnNever.IconRight = null;
            this.btnNever.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNever.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnNever.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnNever.IconSize = 10;
            this.btnNever.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.IdleBorderRadius = 5;
            this.btnNever.IdleBorderThickness = 1;
            this.btnNever.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_do_not_disturb_white_36dp;
            this.btnNever.IdleIconRightImage = null;
            this.btnNever.IndicateFocus = false;
            this.btnNever.Location = new System.Drawing.Point(283, 178);
            this.btnNever.Name = "btnNever";
            this.btnNever.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnNever.OnDisabledState.BorderRadius = 5;
            this.btnNever.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnNever.OnDisabledState.BorderThickness = 1;
            this.btnNever.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnNever.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnNever.OnDisabledState.IconLeftImage = null;
            this.btnNever.OnDisabledState.IconRightImage = null;
            this.btnNever.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.onHoverState.BorderRadius = 5;
            this.btnNever.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnNever.onHoverState.BorderThickness = 1;
            this.btnNever.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnNever.onHoverState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_do_not_disturb_white_36dp;
            this.btnNever.onHoverState.IconRightImage = null;
            this.btnNever.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.OnIdleState.BorderRadius = 5;
            this.btnNever.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnNever.OnIdleState.BorderThickness = 1;
            this.btnNever.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnNever.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_do_not_disturb_white_36dp;
            this.btnNever.OnIdleState.IconRightImage = null;
            this.btnNever.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.OnPressedState.BorderRadius = 5;
            this.btnNever.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnNever.OnPressedState.BorderThickness = 1;
            this.btnNever.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnNever.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnNever.OnPressedState.IconLeftImage = null;
            this.btnNever.OnPressedState.IconRightImage = null;
            this.btnNever.Size = new System.Drawing.Size(179, 31);
            this.btnNever.TabIndex = 27;
            this.btnNever.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNever.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnNever.TextMarginLeft = 0;
            this.btnNever.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnNever.UseDefaultRadiusAndThickness = true;
            this.btnNever.Click += new System.EventHandler(this.btnNever_Click);
            // 
            // btnLater
            // 
            this.btnLater.AllowAnimations = true;
            this.btnLater.AllowMouseEffects = true;
            this.btnLater.AllowToggling = false;
            this.btnLater.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLater.AnimationSpeed = 200;
            this.btnLater.AutoGenerateColors = false;
            this.btnLater.AutoRoundBorders = false;
            this.btnLater.AutoSizeLeftIcon = true;
            this.btnLater.AutoSizeRightIcon = true;
            this.btnLater.BackColor = System.Drawing.Color.Transparent;
            this.btnLater.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLater.BackgroundImage")));
            this.btnLater.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnLater.ButtonText = "Maybe later";
            this.btnLater.ButtonTextMarginLeft = 0;
            this.btnLater.ColorContrastOnClick = 45;
            this.btnLater.ColorContrastOnHover = 45;
            this.btnLater.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges5.BottomLeft = true;
            borderEdges5.BottomRight = true;
            borderEdges5.TopLeft = true;
            borderEdges5.TopRight = true;
            this.btnLater.CustomizableEdges = borderEdges5;
            this.btnLater.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnLater.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnLater.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnLater.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnLater.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnLater.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLater.ForeColor = System.Drawing.Color.White;
            this.btnLater.IconLeft = global::ExaltAccountManager.Properties.Resources.delivery_time_white_24px;
            this.btnLater.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLater.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnLater.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnLater.IconMarginLeft = 11;
            this.btnLater.IconPadding = 6;
            this.btnLater.IconRight = null;
            this.btnLater.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLater.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnLater.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnLater.IconSize = 10;
            this.btnLater.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.IdleBorderRadius = 5;
            this.btnLater.IdleBorderThickness = 1;
            this.btnLater.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.delivery_time_white_24px;
            this.btnLater.IdleIconRightImage = null;
            this.btnLater.IndicateFocus = false;
            this.btnLater.Location = new System.Drawing.Point(16, 178);
            this.btnLater.Name = "btnLater";
            this.btnLater.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnLater.OnDisabledState.BorderRadius = 5;
            this.btnLater.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnLater.OnDisabledState.BorderThickness = 1;
            this.btnLater.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnLater.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnLater.OnDisabledState.IconLeftImage = null;
            this.btnLater.OnDisabledState.IconRightImage = null;
            this.btnLater.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.onHoverState.BorderRadius = 5;
            this.btnLater.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnLater.onHoverState.BorderThickness = 1;
            this.btnLater.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnLater.onHoverState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.delivery_time_white_24px;
            this.btnLater.onHoverState.IconRightImage = null;
            this.btnLater.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.OnIdleState.BorderRadius = 5;
            this.btnLater.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnLater.OnIdleState.BorderThickness = 1;
            this.btnLater.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnLater.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.delivery_time_white_24px;
            this.btnLater.OnIdleState.IconRightImage = null;
            this.btnLater.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.OnPressedState.BorderRadius = 5;
            this.btnLater.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnLater.OnPressedState.BorderThickness = 1;
            this.btnLater.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnLater.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnLater.OnPressedState.IconLeftImage = null;
            this.btnLater.OnPressedState.IconRightImage = null;
            this.btnLater.Size = new System.Drawing.Size(261, 31);
            this.btnLater.TabIndex = 26;
            this.btnLater.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLater.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnLater.TextMarginLeft = 0;
            this.btnLater.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnLater.UseDefaultRadiusAndThickness = true;
            this.btnLater.Click += new System.EventHandler(this.btnLater_Click);
            // 
            // btnYeah
            // 
            this.btnYeah.AllowAnimations = true;
            this.btnYeah.AllowMouseEffects = true;
            this.btnYeah.AllowToggling = false;
            this.btnYeah.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYeah.AnimationSpeed = 200;
            this.btnYeah.AutoGenerateColors = false;
            this.btnYeah.AutoRoundBorders = false;
            this.btnYeah.AutoSizeLeftIcon = true;
            this.btnYeah.AutoSizeRightIcon = true;
            this.btnYeah.BackColor = System.Drawing.Color.Transparent;
            this.btnYeah.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnYeah.BackgroundImage")));
            this.btnYeah.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnYeah.ButtonText = "Yeah, one connection please!";
            this.btnYeah.ButtonTextMarginLeft = 0;
            this.btnYeah.ColorContrastOnClick = 45;
            this.btnYeah.ColorContrastOnHover = 45;
            this.btnYeah.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges6.BottomLeft = true;
            borderEdges6.BottomRight = true;
            borderEdges6.TopLeft = true;
            borderEdges6.TopRight = true;
            this.btnYeah.CustomizableEdges = borderEdges6;
            this.btnYeah.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnYeah.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnYeah.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnYeah.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnYeah.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnYeah.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYeah.ForeColor = System.Drawing.Color.White;
            this.btnYeah.IconLeft = global::ExaltAccountManager.Properties.Resources.discord_new_outline_white_24px;
            this.btnYeah.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnYeah.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnYeah.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnYeah.IconMarginLeft = 11;
            this.btnYeah.IconPadding = 6;
            this.btnYeah.IconRight = null;
            this.btnYeah.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnYeah.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnYeah.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnYeah.IconSize = 10;
            this.btnYeah.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.IdleBorderRadius = 5;
            this.btnYeah.IdleBorderThickness = 1;
            this.btnYeah.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.discord_new_outline_white_24px;
            this.btnYeah.IdleIconRightImage = null;
            this.btnYeah.IndicateFocus = false;
            this.btnYeah.Location = new System.Drawing.Point(16, 141);
            this.btnYeah.Name = "btnYeah";
            this.btnYeah.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnYeah.OnDisabledState.BorderRadius = 5;
            this.btnYeah.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnYeah.OnDisabledState.BorderThickness = 1;
            this.btnYeah.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnYeah.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnYeah.OnDisabledState.IconLeftImage = null;
            this.btnYeah.OnDisabledState.IconRightImage = null;
            this.btnYeah.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.onHoverState.BorderRadius = 5;
            this.btnYeah.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnYeah.onHoverState.BorderThickness = 1;
            this.btnYeah.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnYeah.onHoverState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.discord_new_white_24px;
            this.btnYeah.onHoverState.IconRightImage = null;
            this.btnYeah.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.OnIdleState.BorderRadius = 5;
            this.btnYeah.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnYeah.OnIdleState.BorderThickness = 1;
            this.btnYeah.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnYeah.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.discord_new_outline_white_24px;
            this.btnYeah.OnIdleState.IconRightImage = null;
            this.btnYeah.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.OnPressedState.BorderRadius = 5;
            this.btnYeah.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnYeah.OnPressedState.BorderThickness = 1;
            this.btnYeah.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnYeah.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnYeah.OnPressedState.IconLeftImage = null;
            this.btnYeah.OnPressedState.IconRightImage = null;
            this.btnYeah.Size = new System.Drawing.Size(446, 31);
            this.btnYeah.TabIndex = 25;
            this.btnYeah.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnYeah.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnYeah.TextMarginLeft = 0;
            this.btnYeah.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnYeah.UseDefaultRadiusAndThickness = true;
            this.btnYeah.Click += new System.EventHandler(this.btnYeah_Click);
            // 
            // lQuestion
            // 
            this.lQuestion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(12, 41);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(456, 91);
            this.lQuestion.TabIndex = 21;
            this.lQuestion.Text = "Do you want to connect your discord account ({0}) with this EAM account?\r\nThis wo" +
    "uld grant you special roles and more on the Exalt Account Manager discord server" +
    ".";
            this.lQuestion.UseMnemonic = false;
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(12, 12);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(158, 21);
            this.lHeadline.TabIndex = 7;
            this.lHeadline.Text = "Update discord user";
            // 
            // pContent
            // 
            this.pContent.Controls.Add(this.shadow);
            this.pContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContent.Location = new System.Drawing.Point(0, 64);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(500, 245);
            this.pContent.TabIndex = 22;
            // 
            // FrmDiscordPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 309);
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmDiscordPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.pTop.ResumeLayout(false);
            this.pTopBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.pHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            this.pContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pHeader;
        private System.Windows.Forms.PictureBox pbHeader;
        private System.Windows.Forms.Label lHeaderEAM;
        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnYeah;
        private System.Windows.Forms.Label lQuestion;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.Panel pTopBar;
        private System.Windows.Forms.Panel pContent;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnNever;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnLater;
        private System.Windows.Forms.PictureBox pbClose;
    }
}