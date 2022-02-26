namespace ExaltAccountManager.UI.Elements
{
    partial class EleTokenViewer
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleTokenViewer));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges3 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCharList = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnAccountVerify = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.lToken = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lEmail = new System.Windows.Forms.Label();
            this.lAccName = new System.Windows.Forms.Label();
            this.lexpirationTime = new System.Windows.Forms.Label();
            this.lCreationTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pbClose = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.btnCopy = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lQuestion = new System.Windows.Forms.Label();
            this.bunifuElipse = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.lLastState = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.shadow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadow.BorderRadius = 9;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.lLastState);
            this.shadow.Controls.Add(this.label10);
            this.shadow.Controls.Add(this.label8);
            this.shadow.Controls.Add(this.label7);
            this.shadow.Controls.Add(this.btnCharList);
            this.shadow.Controls.Add(this.btnAccountVerify);
            this.shadow.Controls.Add(this.lToken);
            this.shadow.Controls.Add(this.label11);
            this.shadow.Controls.Add(this.lEmail);
            this.shadow.Controls.Add(this.lAccName);
            this.shadow.Controls.Add(this.lexpirationTime);
            this.shadow.Controls.Add(this.lCreationTime);
            this.shadow.Controls.Add(this.label6);
            this.shadow.Controls.Add(this.label5);
            this.shadow.Controls.Add(this.label4);
            this.shadow.Controls.Add(this.label3);
            this.shadow.Controls.Add(this.label2);
            this.shadow.Controls.Add(this.spacerQuestion);
            this.shadow.Controls.Add(this.pbClose);
            this.shadow.Controls.Add(this.btnCopy);
            this.shadow.Controls.Add(this.label1);
            this.shadow.Controls.Add(this.lQuestion);
            this.shadow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shadow.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadow.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadow.Location = new System.Drawing.Point(0, 0);
            this.shadow.Margin = new System.Windows.Forms.Padding(0);
            this.shadow.Name = "shadow";
            this.shadow.PanelColor = System.Drawing.Color.White;
            this.shadow.PanelColor2 = System.Drawing.Color.White;
            this.shadow.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadow.ShadowDept = 2;
            this.shadow.ShadowDepth = 4;
            this.shadow.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadow.ShadowTopLeftVisible = false;
            this.shadow.Size = new System.Drawing.Size(500, 543);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(50, 475);
            this.label8.MaximumSize = new System.Drawing.Size(468, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(312, 13);
            this.label8.TabIndex = 40;
            this.label8.Text = "Notice: account/verify requires you to renew your token afterwards.";
            this.label8.UseMnemonic = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(16, 413);
            this.label7.MaximumSize = new System.Drawing.Size(468, 21);
            this.label7.MinimumSize = new System.Drawing.Size(468, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(468, 21);
            this.label7.TabIndex = 39;
            this.label7.Text = "Perform webrequests in your browser";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label7.UseMnemonic = false;
            // 
            // btnCharList
            // 
            this.btnCharList.AllowAnimations = true;
            this.btnCharList.AllowMouseEffects = true;
            this.btnCharList.AllowToggling = false;
            this.btnCharList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCharList.AnimationSpeed = 200;
            this.btnCharList.AutoGenerateColors = false;
            this.btnCharList.AutoRoundBorders = false;
            this.btnCharList.AutoSizeLeftIcon = true;
            this.btnCharList.AutoSizeRightIcon = true;
            this.btnCharList.BackColor = System.Drawing.Color.Transparent;
            this.btnCharList.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCharList.BackgroundImage")));
            this.btnCharList.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCharList.ButtonText = "char/list";
            this.btnCharList.ButtonTextMarginLeft = 0;
            this.btnCharList.ColorContrastOnClick = 45;
            this.btnCharList.ColorContrastOnHover = 45;
            this.btnCharList.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnCharList.CustomizableEdges = borderEdges1;
            this.btnCharList.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCharList.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnCharList.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnCharList.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnCharList.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnCharList.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCharList.ForeColor = System.Drawing.Color.White;
            this.btnCharList.IconLeft = global::ExaltAccountManager.Properties.Resources.list_white_24px_1;
            this.btnCharList.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCharList.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnCharList.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnCharList.IconMarginLeft = 11;
            this.btnCharList.IconPadding = 6;
            this.btnCharList.IconRight = null;
            this.btnCharList.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCharList.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnCharList.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnCharList.IconSize = 10;
            this.btnCharList.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.IdleBorderRadius = 5;
            this.btnCharList.IdleBorderThickness = 1;
            this.btnCharList.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.list_white_24px_1;
            this.btnCharList.IdleIconRightImage = null;
            this.btnCharList.IndicateFocus = false;
            this.btnCharList.Location = new System.Drawing.Point(50, 495);
            this.btnCharList.Name = "btnCharList";
            this.btnCharList.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnCharList.OnDisabledState.BorderRadius = 5;
            this.btnCharList.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCharList.OnDisabledState.BorderThickness = 1;
            this.btnCharList.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnCharList.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnCharList.OnDisabledState.IconLeftImage = null;
            this.btnCharList.OnDisabledState.IconRightImage = null;
            this.btnCharList.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.onHoverState.BorderRadius = 5;
            this.btnCharList.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCharList.onHoverState.BorderThickness = 1;
            this.btnCharList.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnCharList.onHoverState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.search_in_list_24px;
            this.btnCharList.onHoverState.IconRightImage = null;
            this.btnCharList.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.OnIdleState.BorderRadius = 5;
            this.btnCharList.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCharList.OnIdleState.BorderThickness = 1;
            this.btnCharList.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnCharList.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.list_white_24px_1;
            this.btnCharList.OnIdleState.IconRightImage = null;
            this.btnCharList.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.OnPressedState.BorderRadius = 5;
            this.btnCharList.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCharList.OnPressedState.BorderThickness = 1;
            this.btnCharList.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCharList.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnCharList.OnPressedState.IconLeftImage = null;
            this.btnCharList.OnPressedState.IconRightImage = null;
            this.btnCharList.Size = new System.Drawing.Size(400, 31);
            this.btnCharList.TabIndex = 38;
            this.btnCharList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCharList.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnCharList.TextMarginLeft = 0;
            this.btnCharList.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnCharList.UseDefaultRadiusAndThickness = true;
            this.btnCharList.Click += new System.EventHandler(this.btnCharList_Click);
            // 
            // btnAccountVerify
            // 
            this.btnAccountVerify.AllowAnimations = true;
            this.btnAccountVerify.AllowMouseEffects = true;
            this.btnAccountVerify.AllowToggling = false;
            this.btnAccountVerify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccountVerify.AnimationSpeed = 200;
            this.btnAccountVerify.AutoGenerateColors = false;
            this.btnAccountVerify.AutoRoundBorders = false;
            this.btnAccountVerify.AutoSizeLeftIcon = true;
            this.btnAccountVerify.AutoSizeRightIcon = true;
            this.btnAccountVerify.BackColor = System.Drawing.Color.Transparent;
            this.btnAccountVerify.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAccountVerify.BackgroundImage")));
            this.btnAccountVerify.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnAccountVerify.ButtonText = "account/verify";
            this.btnAccountVerify.ButtonTextMarginLeft = 0;
            this.btnAccountVerify.ColorContrastOnClick = 45;
            this.btnAccountVerify.ColorContrastOnHover = 45;
            this.btnAccountVerify.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnAccountVerify.CustomizableEdges = borderEdges2;
            this.btnAccountVerify.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnAccountVerify.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnAccountVerify.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnAccountVerify.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnAccountVerify.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnAccountVerify.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccountVerify.ForeColor = System.Drawing.Color.White;
            this.btnAccountVerify.IconLeft = global::ExaltAccountManager.Properties.Resources.unverified_account_24px;
            this.btnAccountVerify.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAccountVerify.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnAccountVerify.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnAccountVerify.IconMarginLeft = 11;
            this.btnAccountVerify.IconPadding = 6;
            this.btnAccountVerify.IconRight = null;
            this.btnAccountVerify.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAccountVerify.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnAccountVerify.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnAccountVerify.IconSize = 10;
            this.btnAccountVerify.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.IdleBorderRadius = 5;
            this.btnAccountVerify.IdleBorderThickness = 1;
            this.btnAccountVerify.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.unverified_account_24px;
            this.btnAccountVerify.IdleIconRightImage = null;
            this.btnAccountVerify.IndicateFocus = false;
            this.btnAccountVerify.Location = new System.Drawing.Point(50, 442);
            this.btnAccountVerify.Name = "btnAccountVerify";
            this.btnAccountVerify.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnAccountVerify.OnDisabledState.BorderRadius = 5;
            this.btnAccountVerify.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnAccountVerify.OnDisabledState.BorderThickness = 1;
            this.btnAccountVerify.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnAccountVerify.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnAccountVerify.OnDisabledState.IconLeftImage = null;
            this.btnAccountVerify.OnDisabledState.IconRightImage = null;
            this.btnAccountVerify.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.onHoverState.BorderRadius = 5;
            this.btnAccountVerify.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnAccountVerify.onHoverState.BorderThickness = 1;
            this.btnAccountVerify.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnAccountVerify.onHoverState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.verified_account_24px;
            this.btnAccountVerify.onHoverState.IconRightImage = null;
            this.btnAccountVerify.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.OnIdleState.BorderRadius = 5;
            this.btnAccountVerify.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnAccountVerify.OnIdleState.BorderThickness = 1;
            this.btnAccountVerify.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnAccountVerify.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.unverified_account_24px;
            this.btnAccountVerify.OnIdleState.IconRightImage = null;
            this.btnAccountVerify.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.OnPressedState.BorderRadius = 5;
            this.btnAccountVerify.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnAccountVerify.OnPressedState.BorderThickness = 1;
            this.btnAccountVerify.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnAccountVerify.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnAccountVerify.OnPressedState.IconLeftImage = null;
            this.btnAccountVerify.OnPressedState.IconRightImage = null;
            this.btnAccountVerify.Size = new System.Drawing.Size(400, 31);
            this.btnAccountVerify.TabIndex = 37;
            this.btnAccountVerify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccountVerify.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnAccountVerify.TextMarginLeft = 0;
            this.btnAccountVerify.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnAccountVerify.UseDefaultRadiusAndThickness = true;
            this.btnAccountVerify.Click += new System.EventHandler(this.btnAccountVerify_Click);
            // 
            // lToken
            // 
            this.lToken.AutoSize = true;
            this.lToken.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lToken.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lToken.Location = new System.Drawing.Point(16, 257);
            this.lToken.MaximumSize = new System.Drawing.Size(468, 0);
            this.lToken.MinimumSize = new System.Drawing.Size(468, 0);
            this.lToken.Name = "lToken";
            this.lToken.Size = new System.Drawing.Size(468, 102);
            this.lToken.TabIndex = 36;
            this.lToken.Text = resources.GetString("lToken.Text");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(13, 237);
            this.label11.MaximumSize = new System.Drawing.Size(476, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 20);
            this.label11.TabIndex = 35;
            this.label11.Text = "Token:";
            // 
            // lEmail
            // 
            this.lEmail.AutoSize = true;
            this.lEmail.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEmail.Location = new System.Drawing.Point(150, 102);
            this.lEmail.MaximumSize = new System.Drawing.Size(476, 0);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new System.Drawing.Size(79, 20);
            this.lEmail.TabIndex = 34;
            this.lEmail.Text = "00.00.0000";
            // 
            // lAccName
            // 
            this.lAccName.AutoSize = true;
            this.lAccName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAccName.Location = new System.Drawing.Point(150, 77);
            this.lAccName.MaximumSize = new System.Drawing.Size(476, 0);
            this.lAccName.Name = "lAccName";
            this.lAccName.Size = new System.Drawing.Size(79, 20);
            this.lAccName.TabIndex = 33;
            this.lAccName.Text = "00.00.0000";
            // 
            // lexpirationTime
            // 
            this.lexpirationTime.AutoSize = true;
            this.lexpirationTime.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lexpirationTime.Location = new System.Drawing.Point(150, 212);
            this.lexpirationTime.MaximumSize = new System.Drawing.Size(476, 0);
            this.lexpirationTime.Name = "lexpirationTime";
            this.lexpirationTime.Size = new System.Drawing.Size(79, 20);
            this.lexpirationTime.TabIndex = 32;
            this.lexpirationTime.Text = "00.00.0000";
            // 
            // lCreationTime
            // 
            this.lCreationTime.AutoSize = true;
            this.lCreationTime.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCreationTime.Location = new System.Drawing.Point(150, 187);
            this.lCreationTime.MaximumSize = new System.Drawing.Size(476, 0);
            this.lCreationTime.Name = "lCreationTime";
            this.lCreationTime.Size = new System.Drawing.Size(79, 20);
            this.lCreationTime.TabIndex = 31;
            this.lCreationTime.Text = "00.00.0000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 212);
            this.label6.MaximumSize = new System.Drawing.Size(476, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 20);
            this.label6.TabIndex = 30;
            this.label6.Text = "Expiration date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 132);
            this.label5.MaximumSize = new System.Drawing.Size(468, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 21);
            this.label5.TabIndex = 29;
            this.label5.Text = "Token";
            this.label5.UseMnemonic = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 51);
            this.label4.MaximumSize = new System.Drawing.Size(468, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 21);
            this.label4.TabIndex = 28;
            this.label4.Text = "Account";
            this.label4.UseMnemonic = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 187);
            this.label3.MaximumSize = new System.Drawing.Size(476, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 20);
            this.label3.TabIndex = 27;
            this.label3.Text = "Creation date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 102);
            this.label2.MaximumSize = new System.Drawing.Size(476, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 26;
            this.label2.Text = "Email:";
            // 
            // spacerQuestion
            // 
            this.spacerQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spacerQuestion.BackColor = System.Drawing.Color.Transparent;
            this.spacerQuestion.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("spacerQuestion.BackgroundImage")));
            this.spacerQuestion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.spacerQuestion.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.spacerQuestion.LineColor = System.Drawing.Color.Silver;
            this.spacerQuestion.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.spacerQuestion.LineThickness = 1;
            this.spacerQuestion.Location = new System.Drawing.Point(16, 36);
            this.spacerQuestion.Margin = new System.Windows.Forms.Padding(5);
            this.spacerQuestion.Name = "spacerQuestion";
            this.spacerQuestion.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.spacerQuestion.Size = new System.Drawing.Size(468, 10);
            this.spacerQuestion.TabIndex = 13;
            // 
            // pbClose
            // 
            this.pbClose.AllowFocused = false;
            this.pbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbClose.AutoSizeHeight = true;
            this.pbClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pbClose.BorderRadius = 9;
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_18dp;
            this.pbClose.IsCircle = true;
            this.pbClose.Location = new System.Drawing.Point(473, 9);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(18, 18);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClose.TabIndex = 25;
            this.pbClose.TabStop = false;
            this.pbClose.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // btnCopy
            // 
            this.btnCopy.AllowAnimations = true;
            this.btnCopy.AllowMouseEffects = true;
            this.btnCopy.AllowToggling = false;
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.AnimationSpeed = 200;
            this.btnCopy.AutoGenerateColors = false;
            this.btnCopy.AutoRoundBorders = false;
            this.btnCopy.AutoSizeLeftIcon = true;
            this.btnCopy.AutoSizeRightIcon = true;
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCopy.BackgroundImage")));
            this.btnCopy.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCopy.ButtonText = "Copy Token to clipboard";
            this.btnCopy.ButtonTextMarginLeft = 0;
            this.btnCopy.ColorContrastOnClick = 45;
            this.btnCopy.ColorContrastOnHover = 45;
            this.btnCopy.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges3.BottomLeft = true;
            borderEdges3.BottomRight = true;
            borderEdges3.TopLeft = true;
            borderEdges3.TopRight = true;
            this.btnCopy.CustomizableEdges = borderEdges3;
            this.btnCopy.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCopy.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnCopy.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnCopy.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnCopy.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnCopy.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.ForeColor = System.Drawing.Color.White;
            this.btnCopy.IconLeft = global::ExaltAccountManager.Properties.Resources.ic_content_copy_white_24dp;
            this.btnCopy.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnCopy.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnCopy.IconMarginLeft = 11;
            this.btnCopy.IconPadding = 6;
            this.btnCopy.IconRight = null;
            this.btnCopy.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnCopy.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnCopy.IconSize = 10;
            this.btnCopy.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.IdleBorderRadius = 5;
            this.btnCopy.IdleBorderThickness = 1;
            this.btnCopy.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_content_copy_white_24dp;
            this.btnCopy.IdleIconRightImage = null;
            this.btnCopy.IndicateFocus = false;
            this.btnCopy.Location = new System.Drawing.Point(50, 370);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnCopy.OnDisabledState.BorderRadius = 5;
            this.btnCopy.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCopy.OnDisabledState.BorderThickness = 1;
            this.btnCopy.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnCopy.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnCopy.OnDisabledState.IconLeftImage = null;
            this.btnCopy.OnDisabledState.IconRightImage = null;
            this.btnCopy.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.onHoverState.BorderRadius = 5;
            this.btnCopy.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCopy.onHoverState.BorderThickness = 1;
            this.btnCopy.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnCopy.onHoverState.IconLeftImage = null;
            this.btnCopy.onHoverState.IconRightImage = null;
            this.btnCopy.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.OnIdleState.BorderRadius = 5;
            this.btnCopy.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCopy.OnIdleState.BorderThickness = 1;
            this.btnCopy.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnCopy.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_content_copy_white_24dp;
            this.btnCopy.OnIdleState.IconRightImage = null;
            this.btnCopy.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.OnPressedState.BorderRadius = 5;
            this.btnCopy.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCopy.OnPressedState.BorderThickness = 1;
            this.btnCopy.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCopy.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnCopy.OnPressedState.IconLeftImage = null;
            this.btnCopy.OnPressedState.IconRightImage = null;
            this.btnCopy.Size = new System.Drawing.Size(400, 31);
            this.btnCopy.TabIndex = 24;
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCopy.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnCopy.TextMarginLeft = 0;
            this.btnCopy.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnCopy.UseDefaultRadiusAndThickness = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.MaximumSize = new System.Drawing.Size(476, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Accountname:";
            // 
            // lQuestion
            // 
            this.lQuestion.AutoSize = true;
            this.lQuestion.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(12, 12);
            this.lQuestion.MaximumSize = new System.Drawing.Size(468, 0);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(125, 25);
            this.lQuestion.TabIndex = 12;
            this.lQuestion.Text = "Token Viewer";
            this.lQuestion.UseMnemonic = false;
            // 
            // bunifuElipse
            // 
            this.bunifuElipse.ElipseRadius = 9;
            this.bunifuElipse.TargetControl = this.lToken;
            // 
            // lLastState
            // 
            this.lLastState.AutoSize = true;
            this.lLastState.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLastState.Location = new System.Drawing.Point(150, 162);
            this.lLastState.MaximumSize = new System.Drawing.Size(476, 0);
            this.lLastState.Name = "lLastState";
            this.lLastState.Size = new System.Drawing.Size(79, 20);
            this.lLastState.TabIndex = 42;
            this.lLastState.Text = "00.00.0000";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(13, 162);
            this.label10.MaximumSize = new System.Drawing.Size(476, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(135, 20);
            this.label10.TabIndex = 41;
            this.label10.Text = "Last Request State:";
            // 
            // EleTokenViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EleTokenViewer";
            this.Size = new System.Drawing.Size(500, 543);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private Bunifu.UI.WinForms.BunifuPictureBox pbClose;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnCopy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lQuestion;
        private System.Windows.Forms.Label lToken;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lEmail;
        private System.Windows.Forms.Label lAccName;
        private System.Windows.Forms.Label lexpirationTime;
        private System.Windows.Forms.Label lCreationTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnCharList;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnAccountVerify;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse;
        private System.Windows.Forms.Label lLastState;
        private System.Windows.Forms.Label label10;
    }
}
