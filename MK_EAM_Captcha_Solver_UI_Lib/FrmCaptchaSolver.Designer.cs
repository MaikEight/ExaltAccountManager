namespace MK_EAM_Captcha_Solver_UI_Lib
{
    partial class FrmCaptchaSolver
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCaptchaSolver));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges7 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges8 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.pTopContent = new System.Windows.Forms.Panel();
            this.pTop = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pHeader = new System.Windows.Forms.Panel();
            this.lBeta = new System.Windows.Forms.Label();
            this.lHeaderCaptchaAid = new System.Windows.Forms.Label();
            this.lEAMHead = new System.Windows.Forms.Label();
            this.pbHeader = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pMessage = new System.Windows.Forms.Panel();
            this.lMessage = new System.Windows.Forms.Label();
            this.pContent = new System.Windows.Forms.Panel();
            this.pCaptcha = new System.Windows.Forms.Panel();
            this.pActions = new System.Windows.Forms.Panel();
            this.lTriesLeft = new System.Windows.Forms.Label();
            this.btnReset = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnSubmit = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.pbCaptchaMain = new System.Windows.Forms.PictureBox();
            this.pbCaptchaQuestion = new System.Windows.Forms.PictureBox();
            this.pCaptchaText = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.formDock = new Bunifu.UI.WinForms.BunifuFormDock();
            this.pTopContent.SuspendLayout();
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.panel3.SuspendLayout();
            this.pHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            this.pMessage.SuspendLayout();
            this.pContent.SuspendLayout();
            this.pCaptcha.SuspendLayout();
            this.pActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptchaMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptchaQuestion)).BeginInit();
            this.pCaptchaText.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTopContent
            // 
            this.pTopContent.Controls.Add(this.pTop);
            this.pTopContent.Controls.Add(this.panel3);
            this.pTopContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTopContent.Location = new System.Drawing.Point(0, 0);
            this.pTopContent.Name = "pTopContent";
            this.pTopContent.Size = new System.Drawing.Size(300, 64);
            this.pTopContent.TabIndex = 0;
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pTop.Controls.Add(this.pbMinimize);
            this.pTop.Controls.Add(this.pbClose);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(175, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(125, 24);
            this.pTop.TabIndex = 3;
            // 
            // pbMinimize
            // 
            this.pbMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMinimize.Image = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(77, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMinimize.TabIndex = 10;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMinimize_MouseDown);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            this.pbMinimize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbMinimize_MouseUp);
            // 
            // pbClose
            // 
            this.pbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbClose.Image = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(101, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 9;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbClose_MouseDown);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            this.pbClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbClose_MouseUp);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.panel3.Controls.Add(this.pHeader);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(175, 64);
            this.panel3.TabIndex = 2;
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pHeader.Controls.Add(this.lBeta);
            this.pHeader.Controls.Add(this.lHeaderCaptchaAid);
            this.pHeader.Controls.Add(this.lEAMHead);
            this.pHeader.Controls.Add(this.pbHeader);
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(175, 64);
            this.pHeader.TabIndex = 4;
            // 
            // lBeta
            // 
            this.lBeta.AutoSize = true;
            this.lBeta.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lBeta.ForeColor = System.Drawing.Color.White;
            this.lBeta.Location = new System.Drawing.Point(142, 42);
            this.lBeta.Name = "lBeta";
            this.lBeta.Size = new System.Drawing.Size(25, 12);
            this.lBeta.TabIndex = 5;
            this.lBeta.Text = "BETA";
            // 
            // lHeaderCaptchaAid
            // 
            this.lHeaderCaptchaAid.AutoSize = true;
            this.lHeaderCaptchaAid.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeaderCaptchaAid.ForeColor = System.Drawing.Color.White;
            this.lHeaderCaptchaAid.Location = new System.Drawing.Point(60, 25);
            this.lHeaderCaptchaAid.Name = "lHeaderCaptchaAid";
            this.lHeaderCaptchaAid.Size = new System.Drawing.Size(104, 21);
            this.lHeaderCaptchaAid.TabIndex = 4;
            this.lHeaderCaptchaAid.Text = "Captcha Aid";
            // 
            // lEAMHead
            // 
            this.lEAMHead.AutoSize = true;
            this.lEAMHead.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEAMHead.ForeColor = System.Drawing.Color.White;
            this.lEAMHead.Location = new System.Drawing.Point(63, 14);
            this.lEAMHead.Name = "lEAMHead";
            this.lEAMHead.Size = new System.Drawing.Size(97, 12);
            this.lEAMHead.TabIndex = 2;
            this.lEAMHead.Text = "Exalt Account Manager";
            // 
            // pbHeader
            // 
            this.pbHeader.AllowFocused = false;
            this.pbHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbHeader.AutoSizeHeight = true;
            this.pbHeader.BorderRadius = 0;
            this.pbHeader.Image = ((System.Drawing.Image)(resources.GetObject("pbHeader.Image")));
            this.pbHeader.IsCircle = true;
            this.pbHeader.Location = new System.Drawing.Point(6, 6);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(48, 48);
            this.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbHeader.TabIndex = 3;
            this.pbHeader.TabStop = false;
            this.pbHeader.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Square;
            // 
            // pMessage
            // 
            this.pMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pMessage.Controls.Add(this.lMessage);
            this.pMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pMessage.Location = new System.Drawing.Point(0, 64);
            this.pMessage.Name = "pMessage";
            this.pMessage.Padding = new System.Windows.Forms.Padding(5);
            this.pMessage.Size = new System.Drawing.Size(300, 73);
            this.pMessage.TabIndex = 1;
            // 
            // lMessage
            // 
            this.lMessage.AutoEllipsis = true;
            this.lMessage.AutoSize = true;
            this.lMessage.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lMessage.Location = new System.Drawing.Point(5, 5);
            this.lMessage.Margin = new System.Windows.Forms.Padding(0);
            this.lMessage.MaximumSize = new System.Drawing.Size(290, 0);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(281, 63);
            this.lMessage.TabIndex = 8;
            this.lMessage.Text = "You need to solve a captcha to log in or refresh the data of the account: \r\n{0}";
            this.lMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lMessage.UseMnemonic = false;
            // 
            // pContent
            // 
            this.pContent.Controls.Add(this.pCaptcha);
            this.pContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContent.Location = new System.Drawing.Point(0, 137);
            this.pContent.Name = "pContent";
            this.pContent.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.pContent.Size = new System.Drawing.Size(300, 427);
            this.pContent.TabIndex = 2;
            this.pContent.Paint += new System.Windows.Forms.PaintEventHandler(this.pContent_Paint);
            // 
            // pCaptcha
            // 
            this.pCaptcha.Controls.Add(this.pActions);
            this.pCaptcha.Controls.Add(this.pbCaptchaMain);
            this.pCaptcha.Controls.Add(this.pbCaptchaQuestion);
            this.pCaptcha.Controls.Add(this.pCaptchaText);
            this.pCaptcha.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pCaptcha.Location = new System.Drawing.Point(5, 0);
            this.pCaptcha.Name = "pCaptcha";
            this.pCaptcha.Size = new System.Drawing.Size(290, 422);
            this.pCaptcha.TabIndex = 0;
            // 
            // pActions
            // 
            this.pActions.Controls.Add(this.lTriesLeft);
            this.pActions.Controls.Add(this.btnReset);
            this.pActions.Controls.Add(this.btnSubmit);
            this.pActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pActions.Location = new System.Drawing.Point(0, 381);
            this.pActions.Name = "pActions";
            this.pActions.Size = new System.Drawing.Size(290, 41);
            this.pActions.TabIndex = 3;
            this.pActions.Visible = false;
            // 
            // lTriesLeft
            // 
            this.lTriesLeft.AutoEllipsis = true;
            this.lTriesLeft.AutoSize = true;
            this.lTriesLeft.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTriesLeft.Location = new System.Drawing.Point(109, 5);
            this.lTriesLeft.Margin = new System.Windows.Forms.Padding(0);
            this.lTriesLeft.MaximumSize = new System.Drawing.Size(290, 0);
            this.lTriesLeft.Name = "lTriesLeft";
            this.lTriesLeft.Size = new System.Drawing.Size(62, 34);
            this.lTriesLeft.TabIndex = 26;
            this.lTriesLeft.Text = "Tries left\r\n{0}";
            this.lTriesLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lTriesLeft.UseMnemonic = false;
            // 
            // btnReset
            // 
            this.btnReset.AllowAnimations = true;
            this.btnReset.AllowMouseEffects = true;
            this.btnReset.AllowToggling = false;
            this.btnReset.AnimationSpeed = 200;
            this.btnReset.AutoGenerateColors = false;
            this.btnReset.AutoRoundBorders = false;
            this.btnReset.AutoSizeLeftIcon = true;
            this.btnReset.AutoSizeRightIcon = true;
            this.btnReset.BackColor = System.Drawing.Color.Transparent;
            this.btnReset.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnReset.ButtonText = "Reset";
            this.btnReset.ButtonTextMarginLeft = 0;
            this.btnReset.ColorContrastOnClick = 45;
            this.btnReset.ColorContrastOnHover = 45;
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges7.BottomLeft = true;
            borderEdges7.BottomRight = true;
            borderEdges7.TopLeft = true;
            borderEdges7.TopRight = true;
            this.btnReset.CustomizableEdges = borderEdges7;
            this.btnReset.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnReset.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnReset.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnReset.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnReset.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.IconLeft = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.back_to_white_36px_1;
            this.btnReset.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnReset.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnReset.IconMarginLeft = 11;
            this.btnReset.IconPadding = 6;
            this.btnReset.IconRight = null;
            this.btnReset.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReset.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnReset.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnReset.IconSize = 10;
            this.btnReset.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.IdleBorderRadius = 5;
            this.btnReset.IdleBorderThickness = 1;
            this.btnReset.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.IdleIconLeftImage = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.back_to_white_36px_1;
            this.btnReset.IdleIconRightImage = null;
            this.btnReset.IndicateFocus = false;
            this.btnReset.Location = new System.Drawing.Point(5, 5);
            this.btnReset.Name = "btnReset";
            this.btnReset.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnReset.OnDisabledState.BorderRadius = 5;
            this.btnReset.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnReset.OnDisabledState.BorderThickness = 1;
            this.btnReset.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnReset.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnReset.OnDisabledState.IconLeftImage = null;
            this.btnReset.OnDisabledState.IconRightImage = null;
            this.btnReset.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.onHoverState.BorderRadius = 5;
            this.btnReset.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnReset.onHoverState.BorderThickness = 1;
            this.btnReset.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnReset.onHoverState.IconLeftImage = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.back_to_white_36px;
            this.btnReset.onHoverState.IconRightImage = null;
            this.btnReset.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.OnIdleState.BorderRadius = 5;
            this.btnReset.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnReset.OnIdleState.BorderThickness = 1;
            this.btnReset.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnReset.OnIdleState.IconLeftImage = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.back_to_white_36px_1;
            this.btnReset.OnIdleState.IconRightImage = null;
            this.btnReset.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.OnPressedState.BorderRadius = 5;
            this.btnReset.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnReset.OnPressedState.BorderThickness = 1;
            this.btnReset.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnReset.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnReset.OnPressedState.IconLeftImage = null;
            this.btnReset.OnPressedState.IconRightImage = null;
            this.btnReset.Size = new System.Drawing.Size(98, 31);
            this.btnReset.TabIndex = 25;
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReset.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnReset.TextMarginLeft = 0;
            this.btnReset.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnReset.UseDefaultRadiusAndThickness = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.AllowAnimations = true;
            this.btnSubmit.AllowMouseEffects = true;
            this.btnSubmit.AllowToggling = false;
            this.btnSubmit.AnimationSpeed = 200;
            this.btnSubmit.AutoGenerateColors = false;
            this.btnSubmit.AutoRoundBorders = false;
            this.btnSubmit.AutoSizeLeftIcon = true;
            this.btnSubmit.AutoSizeRightIcon = true;
            this.btnSubmit.BackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSubmit.BackgroundImage")));
            this.btnSubmit.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnSubmit.ButtonText = "Submit";
            this.btnSubmit.ButtonTextMarginLeft = 0;
            this.btnSubmit.ColorContrastOnClick = 45;
            this.btnSubmit.ColorContrastOnHover = 45;
            this.btnSubmit.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges8.BottomLeft = true;
            borderEdges8.BottomRight = true;
            borderEdges8.TopLeft = true;
            borderEdges8.TopRight = true;
            this.btnSubmit.CustomizableEdges = borderEdges8;
            this.btnSubmit.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSubmit.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSubmit.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnSubmit.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnSubmit.Enabled = false;
            this.btnSubmit.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.IconLeft = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.Check_Circle_white_36px_1;
            this.btnSubmit.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSubmit.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnSubmit.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnSubmit.IconMarginLeft = 11;
            this.btnSubmit.IconPadding = 6;
            this.btnSubmit.IconRight = null;
            this.btnSubmit.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSubmit.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnSubmit.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnSubmit.IconSize = 10;
            this.btnSubmit.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.IdleBorderRadius = 5;
            this.btnSubmit.IdleBorderThickness = 1;
            this.btnSubmit.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.IdleIconLeftImage = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.Check_Circle_white_36px_1;
            this.btnSubmit.IdleIconRightImage = null;
            this.btnSubmit.IndicateFocus = false;
            this.btnSubmit.Location = new System.Drawing.Point(177, 5);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSubmit.OnDisabledState.BorderRadius = 5;
            this.btnSubmit.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnSubmit.OnDisabledState.BorderThickness = 1;
            this.btnSubmit.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnSubmit.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnSubmit.OnDisabledState.IconLeftImage = null;
            this.btnSubmit.OnDisabledState.IconRightImage = null;
            this.btnSubmit.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.onHoverState.BorderRadius = 5;
            this.btnSubmit.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnSubmit.onHoverState.BorderThickness = 1;
            this.btnSubmit.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.onHoverState.IconLeftImage = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.Check_Circle_white_36px;
            this.btnSubmit.onHoverState.IconRightImage = null;
            this.btnSubmit.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.OnIdleState.BorderRadius = 5;
            this.btnSubmit.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnSubmit.OnIdleState.BorderThickness = 1;
            this.btnSubmit.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.OnIdleState.IconLeftImage = global::MK_EAM_Captcha_Solver_UI_Lib.Properties.Resources.Check_Circle_white_36px_1;
            this.btnSubmit.OnIdleState.IconRightImage = null;
            this.btnSubmit.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.OnPressedState.BorderRadius = 5;
            this.btnSubmit.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnSubmit.OnPressedState.BorderThickness = 1;
            this.btnSubmit.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnSubmit.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.OnPressedState.IconLeftImage = null;
            this.btnSubmit.OnPressedState.IconRightImage = null;
            this.btnSubmit.Size = new System.Drawing.Size(108, 31);
            this.btnSubmit.TabIndex = 24;
            this.btnSubmit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSubmit.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnSubmit.TextMarginLeft = 0;
            this.btnSubmit.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnSubmit.UseDefaultRadiusAndThickness = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // pbCaptchaMain
            // 
            this.pbCaptchaMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbCaptchaMain.Location = new System.Drawing.Point(0, 91);
            this.pbCaptchaMain.Name = "pbCaptchaMain";
            this.pbCaptchaMain.Size = new System.Drawing.Size(290, 290);
            this.pbCaptchaMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCaptchaMain.TabIndex = 1;
            this.pbCaptchaMain.TabStop = false;
            this.pbCaptchaMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCaptchaMain_Paint);
            this.pbCaptchaMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCaptchaMain_MouseDown);
            // 
            // pbCaptchaQuestion
            // 
            this.pbCaptchaQuestion.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbCaptchaQuestion.Location = new System.Drawing.Point(0, 29);
            this.pbCaptchaQuestion.Name = "pbCaptchaQuestion";
            this.pbCaptchaQuestion.Size = new System.Drawing.Size(290, 62);
            this.pbCaptchaQuestion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbCaptchaQuestion.TabIndex = 0;
            this.pbCaptchaQuestion.TabStop = false;
            // 
            // pCaptchaText
            // 
            this.pCaptchaText.Controls.Add(this.label2);
            this.pCaptchaText.Dock = System.Windows.Forms.DockStyle.Top;
            this.pCaptchaText.Location = new System.Drawing.Point(0, 0);
            this.pCaptchaText.Name = "pCaptchaText";
            this.pCaptchaText.Size = new System.Drawing.Size(290, 29);
            this.pCaptchaText.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(74, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.MaximumSize = new System.Drawing.Size(290, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 21);
            this.label2.TabIndex = 9;
            this.label2.Text = "Find the following";
            this.label2.UseMnemonic = false;
            // 
            // formDock
            // 
            this.formDock.AllowFormDragging = true;
            this.formDock.AllowFormDropShadow = true;
            this.formDock.AllowFormResizing = false;
            this.formDock.AllowHidingBottomRegion = true;
            this.formDock.AllowOpacityChangesWhileDragging = false;
            this.formDock.BorderOptions.BottomBorder.BorderColor = System.Drawing.Color.Silver;
            this.formDock.BorderOptions.BottomBorder.BorderThickness = 1;
            this.formDock.BorderOptions.BottomBorder.ShowBorder = true;
            this.formDock.BorderOptions.LeftBorder.BorderColor = System.Drawing.Color.Silver;
            this.formDock.BorderOptions.LeftBorder.BorderThickness = 1;
            this.formDock.BorderOptions.LeftBorder.ShowBorder = true;
            this.formDock.BorderOptions.RightBorder.BorderColor = System.Drawing.Color.Silver;
            this.formDock.BorderOptions.RightBorder.BorderThickness = 1;
            this.formDock.BorderOptions.RightBorder.ShowBorder = true;
            this.formDock.BorderOptions.TopBorder.BorderColor = System.Drawing.Color.Silver;
            this.formDock.BorderOptions.TopBorder.BorderThickness = 1;
            this.formDock.BorderOptions.TopBorder.ShowBorder = true;
            this.formDock.ContainerControl = this;
            this.formDock.DockingIndicatorsColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(215)))), ((int)(((byte)(233)))));
            this.formDock.DockingIndicatorsOpacity = 0.5D;
            this.formDock.DockingOptions.DockAll = true;
            this.formDock.DockingOptions.DockBottomLeft = true;
            this.formDock.DockingOptions.DockBottomRight = true;
            this.formDock.DockingOptions.DockFullScreen = true;
            this.formDock.DockingOptions.DockLeft = true;
            this.formDock.DockingOptions.DockRight = true;
            this.formDock.DockingOptions.DockTopLeft = true;
            this.formDock.DockingOptions.DockTopRight = true;
            this.formDock.FormDraggingOpacity = 0.9D;
            this.formDock.ParentForm = this;
            this.formDock.ShowCursorChanges = true;
            this.formDock.ShowDockingIndicators = false;
            this.formDock.TitleBarOptions.AllowFormDragging = true;
            this.formDock.TitleBarOptions.BunifuFormDock = this.formDock;
            this.formDock.TitleBarOptions.DoubleClickToExpandWindow = false;
            this.formDock.TitleBarOptions.TitleBarControl = this.pTop;
            this.formDock.TitleBarOptions.UseBackColorOnDockingIndicators = false;
            // 
            // FrmCaptchaSolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(300, 564);
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.pMessage);
            this.Controls.Add(this.pTopContent);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmCaptchaSolver";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmCaptchaSolver";
            this.Activated += new System.EventHandler(this.FrmCaptchaSolver_Activated);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmCaptchaSolver_Paint);
            this.pTopContent.ResumeLayout(false);
            this.pTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.panel3.ResumeLayout(false);
            this.pHeader.ResumeLayout(false);
            this.pHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            this.pMessage.ResumeLayout(false);
            this.pMessage.PerformLayout();
            this.pContent.ResumeLayout(false);
            this.pCaptcha.ResumeLayout(false);
            this.pActions.ResumeLayout(false);
            this.pActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptchaMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptchaQuestion)).EndInit();
            this.pCaptchaText.ResumeLayout(false);
            this.pCaptchaText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTopContent;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pMessage;
        private System.Windows.Forms.Panel pContent;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pHeader;
        private System.Windows.Forms.Label lHeaderCaptchaAid;
        private System.Windows.Forms.Label lEAMHead;
        private Bunifu.UI.WinForms.BunifuPictureBox pbHeader;
        private System.Windows.Forms.Label lMessage;
        private System.Windows.Forms.Panel pCaptcha;
        private System.Windows.Forms.PictureBox pbCaptchaMain;
        private System.Windows.Forms.PictureBox pbCaptchaQuestion;
        private System.Windows.Forms.Panel pCaptchaText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pActions;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnSubmit;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnReset;
        private System.Windows.Forms.Label lTriesLeft;
        private System.Windows.Forms.Label lBeta;
        private Bunifu.UI.WinForms.BunifuFormDock formDock;
    }
}