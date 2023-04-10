namespace ExaltAccountManager.UI.Elements
{
    partial class EleGameUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleGameUpdater));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges3 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.pbHeader = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.btnDone = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnCheckForUpdate = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnPerformUpdate = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.pbCheck = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.pbStatus = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.progressbar = new Bunifu.UI.WinForms.BunifuCircleProgress();
            this.lStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lLastCheck = new System.Windows.Forms.Label();
            this.lAnswer = new System.Windows.Forms.Label();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pbClose = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.shadow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadow.BorderRadius = 9;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.pbHeader);
            this.shadow.Controls.Add(this.btnDone);
            this.shadow.Controls.Add(this.btnCheckForUpdate);
            this.shadow.Controls.Add(this.btnPerformUpdate);
            this.shadow.Controls.Add(this.pbCheck);
            this.shadow.Controls.Add(this.pbStatus);
            this.shadow.Controls.Add(this.progressbar);
            this.shadow.Controls.Add(this.lStatus);
            this.shadow.Controls.Add(this.label2);
            this.shadow.Controls.Add(this.lLastCheck);
            this.shadow.Controls.Add(this.lAnswer);
            this.shadow.Controls.Add(this.spacerQuestion);
            this.shadow.Controls.Add(this.pbClose);
            this.shadow.Controls.Add(this.lHeadline);
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
            this.shadow.Size = new System.Drawing.Size(353, 171);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 17;
            // 
            // pbHeader
            // 
            this.pbHeader.AllowFocused = false;
            this.pbHeader.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbHeader.AutoSizeHeight = true;
            this.pbHeader.BorderRadius = 12;
            this.pbHeader.Image = global::ExaltAccountManager.Properties.Resources.baseline_autorenew_black_36dp;
            this.pbHeader.IsCircle = true;
            this.pbHeader.Location = new System.Drawing.Point(17, 13);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(24, 24);
            this.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbHeader.TabIndex = 33;
            this.pbHeader.TabStop = false;
            this.pbHeader.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            // 
            // btnDone
            // 
            this.btnDone.AllowAnimations = true;
            this.btnDone.AllowMouseEffects = true;
            this.btnDone.AllowToggling = false;
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.AnimationSpeed = 200;
            this.btnDone.AutoGenerateColors = false;
            this.btnDone.AutoRoundBorders = false;
            this.btnDone.AutoSizeLeftIcon = true;
            this.btnDone.AutoSizeRightIcon = true;
            this.btnDone.BackColor = System.Drawing.Color.Transparent;
            this.btnDone.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDone.BackgroundImage")));
            this.btnDone.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDone.ButtonText = "Update done!";
            this.btnDone.ButtonTextMarginLeft = 0;
            this.btnDone.ColorContrastOnClick = 45;
            this.btnDone.ColorContrastOnHover = 45;
            this.btnDone.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnDone.CustomizableEdges = borderEdges1;
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnDone.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDone.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnDone.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnDone.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnDone.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.IconLeft = global::ExaltAccountManager.Properties.Resources.ic_done_white_24dp;
            this.btnDone.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDone.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnDone.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnDone.IconMarginLeft = 11;
            this.btnDone.IconPadding = 6;
            this.btnDone.IconRight = null;
            this.btnDone.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDone.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnDone.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnDone.IconSize = 10;
            this.btnDone.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.IdleBorderRadius = 5;
            this.btnDone.IdleBorderThickness = 1;
            this.btnDone.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_done_white_24dp;
            this.btnDone.IdleIconRightImage = null;
            this.btnDone.IndicateFocus = false;
            this.btnDone.Location = new System.Drawing.Point(50, 288);
            this.btnDone.Name = "btnDone";
            this.btnDone.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDone.OnDisabledState.BorderRadius = 5;
            this.btnDone.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDone.OnDisabledState.BorderThickness = 1;
            this.btnDone.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnDone.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnDone.OnDisabledState.IconLeftImage = null;
            this.btnDone.OnDisabledState.IconRightImage = null;
            this.btnDone.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.onHoverState.BorderRadius = 5;
            this.btnDone.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDone.onHoverState.BorderThickness = 1;
            this.btnDone.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnDone.onHoverState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_done_white_24dp;
            this.btnDone.onHoverState.IconRightImage = null;
            this.btnDone.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.OnIdleState.BorderRadius = 5;
            this.btnDone.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDone.OnIdleState.BorderThickness = 1;
            this.btnDone.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnDone.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.ic_done_white_24dp;
            this.btnDone.OnIdleState.IconRightImage = null;
            this.btnDone.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.OnPressedState.BorderRadius = 5;
            this.btnDone.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDone.OnPressedState.BorderThickness = 1;
            this.btnDone.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnDone.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnDone.OnPressedState.IconLeftImage = null;
            this.btnDone.OnPressedState.IconRightImage = null;
            this.btnDone.Size = new System.Drawing.Size(253, 31);
            this.btnDone.TabIndex = 32;
            this.btnDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDone.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnDone.TextMarginLeft = 0;
            this.btnDone.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnDone.UseDefaultRadiusAndThickness = true;
            this.btnDone.Visible = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnCheckForUpdate
            // 
            this.btnCheckForUpdate.AllowAnimations = true;
            this.btnCheckForUpdate.AllowMouseEffects = true;
            this.btnCheckForUpdate.AllowToggling = false;
            this.btnCheckForUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckForUpdate.AnimationSpeed = 200;
            this.btnCheckForUpdate.AutoGenerateColors = false;
            this.btnCheckForUpdate.AutoRoundBorders = false;
            this.btnCheckForUpdate.AutoSizeLeftIcon = true;
            this.btnCheckForUpdate.AutoSizeRightIcon = true;
            this.btnCheckForUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckForUpdate.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCheckForUpdate.BackgroundImage")));
            this.btnCheckForUpdate.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCheckForUpdate.ButtonText = "Check for update";
            this.btnCheckForUpdate.ButtonTextMarginLeft = 0;
            this.btnCheckForUpdate.ColorContrastOnClick = 45;
            this.btnCheckForUpdate.ColorContrastOnHover = 45;
            this.btnCheckForUpdate.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnCheckForUpdate.CustomizableEdges = borderEdges2;
            this.btnCheckForUpdate.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCheckForUpdate.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnCheckForUpdate.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnCheckForUpdate.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnCheckForUpdate.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnCheckForUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckForUpdate.ForeColor = System.Drawing.Color.White;
            this.btnCheckForUpdate.IconLeft = global::ExaltAccountManager.Properties.Resources.baseline_autorenew_white_36dp;
            this.btnCheckForUpdate.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckForUpdate.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnCheckForUpdate.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnCheckForUpdate.IconMarginLeft = 11;
            this.btnCheckForUpdate.IconPadding = 6;
            this.btnCheckForUpdate.IconRight = null;
            this.btnCheckForUpdate.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCheckForUpdate.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnCheckForUpdate.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnCheckForUpdate.IconSize = 10;
            this.btnCheckForUpdate.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.IdleBorderRadius = 5;
            this.btnCheckForUpdate.IdleBorderThickness = 1;
            this.btnCheckForUpdate.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.baseline_autorenew_white_36dp;
            this.btnCheckForUpdate.IdleIconRightImage = null;
            this.btnCheckForUpdate.IndicateFocus = false;
            this.btnCheckForUpdate.Location = new System.Drawing.Point(50, 132);
            this.btnCheckForUpdate.Name = "btnCheckForUpdate";
            this.btnCheckForUpdate.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnCheckForUpdate.OnDisabledState.BorderRadius = 5;
            this.btnCheckForUpdate.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCheckForUpdate.OnDisabledState.BorderThickness = 1;
            this.btnCheckForUpdate.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnCheckForUpdate.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnCheckForUpdate.OnDisabledState.IconLeftImage = null;
            this.btnCheckForUpdate.OnDisabledState.IconRightImage = null;
            this.btnCheckForUpdate.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.onHoverState.BorderRadius = 5;
            this.btnCheckForUpdate.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCheckForUpdate.onHoverState.BorderThickness = 1;
            this.btnCheckForUpdate.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnCheckForUpdate.onHoverState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.baseline_autorenew_white_36dp_45G;
            this.btnCheckForUpdate.onHoverState.IconRightImage = null;
            this.btnCheckForUpdate.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.OnIdleState.BorderRadius = 5;
            this.btnCheckForUpdate.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCheckForUpdate.OnIdleState.BorderThickness = 1;
            this.btnCheckForUpdate.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnCheckForUpdate.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.baseline_autorenew_white_36dp;
            this.btnCheckForUpdate.OnIdleState.IconRightImage = null;
            this.btnCheckForUpdate.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.OnPressedState.BorderRadius = 5;
            this.btnCheckForUpdate.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnCheckForUpdate.OnPressedState.BorderThickness = 1;
            this.btnCheckForUpdate.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnCheckForUpdate.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnCheckForUpdate.OnPressedState.IconLeftImage = null;
            this.btnCheckForUpdate.OnPressedState.IconRightImage = null;
            this.btnCheckForUpdate.Size = new System.Drawing.Size(253, 31);
            this.btnCheckForUpdate.TabIndex = 28;
            this.btnCheckForUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCheckForUpdate.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnCheckForUpdate.TextMarginLeft = 0;
            this.btnCheckForUpdate.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnCheckForUpdate.UseDefaultRadiusAndThickness = true;
            this.btnCheckForUpdate.Click += new System.EventHandler(this.btnCheckForUpdate_Click);
            // 
            // btnPerformUpdate
            // 
            this.btnPerformUpdate.AllowAnimations = true;
            this.btnPerformUpdate.AllowMouseEffects = true;
            this.btnPerformUpdate.AllowToggling = false;
            this.btnPerformUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPerformUpdate.AnimationSpeed = 200;
            this.btnPerformUpdate.AutoGenerateColors = false;
            this.btnPerformUpdate.AutoRoundBorders = false;
            this.btnPerformUpdate.AutoSizeLeftIcon = true;
            this.btnPerformUpdate.AutoSizeRightIcon = true;
            this.btnPerformUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnPerformUpdate.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPerformUpdate.BackgroundImage")));
            this.btnPerformUpdate.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnPerformUpdate.ButtonText = "Update game";
            this.btnPerformUpdate.ButtonTextMarginLeft = 0;
            this.btnPerformUpdate.ColorContrastOnClick = 45;
            this.btnPerformUpdate.ColorContrastOnHover = 45;
            this.btnPerformUpdate.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges3.BottomLeft = true;
            borderEdges3.BottomRight = true;
            borderEdges3.TopLeft = true;
            borderEdges3.TopRight = true;
            this.btnPerformUpdate.CustomizableEdges = borderEdges3;
            this.btnPerformUpdate.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnPerformUpdate.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnPerformUpdate.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnPerformUpdate.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnPerformUpdate.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnPerformUpdate.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPerformUpdate.ForeColor = System.Drawing.Color.White;
            this.btnPerformUpdate.IconLeft = global::ExaltAccountManager.Properties.Resources.below_white_48px;
            this.btnPerformUpdate.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPerformUpdate.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnPerformUpdate.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnPerformUpdate.IconMarginLeft = 11;
            this.btnPerformUpdate.IconPadding = 6;
            this.btnPerformUpdate.IconRight = null;
            this.btnPerformUpdate.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPerformUpdate.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnPerformUpdate.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnPerformUpdate.IconSize = 10;
            this.btnPerformUpdate.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.IdleBorderRadius = 5;
            this.btnPerformUpdate.IdleBorderThickness = 1;
            this.btnPerformUpdate.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.below_white_48px;
            this.btnPerformUpdate.IdleIconRightImage = null;
            this.btnPerformUpdate.IndicateFocus = false;
            this.btnPerformUpdate.Location = new System.Drawing.Point(50, 170);
            this.btnPerformUpdate.Name = "btnPerformUpdate";
            this.btnPerformUpdate.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnPerformUpdate.OnDisabledState.BorderRadius = 5;
            this.btnPerformUpdate.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnPerformUpdate.OnDisabledState.BorderThickness = 1;
            this.btnPerformUpdate.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnPerformUpdate.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnPerformUpdate.OnDisabledState.IconLeftImage = null;
            this.btnPerformUpdate.OnDisabledState.IconRightImage = null;
            this.btnPerformUpdate.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.onHoverState.BorderRadius = 5;
            this.btnPerformUpdate.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnPerformUpdate.onHoverState.BorderThickness = 1;
            this.btnPerformUpdate.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnPerformUpdate.onHoverState.IconLeftImage = null;
            this.btnPerformUpdate.onHoverState.IconRightImage = null;
            this.btnPerformUpdate.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.OnIdleState.BorderRadius = 5;
            this.btnPerformUpdate.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnPerformUpdate.OnIdleState.BorderThickness = 1;
            this.btnPerformUpdate.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnPerformUpdate.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.below_white_48px;
            this.btnPerformUpdate.OnIdleState.IconRightImage = null;
            this.btnPerformUpdate.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.OnPressedState.BorderRadius = 5;
            this.btnPerformUpdate.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnPerformUpdate.OnPressedState.BorderThickness = 1;
            this.btnPerformUpdate.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnPerformUpdate.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnPerformUpdate.OnPressedState.IconLeftImage = null;
            this.btnPerformUpdate.OnPressedState.IconRightImage = null;
            this.btnPerformUpdate.Size = new System.Drawing.Size(253, 64);
            this.btnPerformUpdate.TabIndex = 24;
            this.btnPerformUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPerformUpdate.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnPerformUpdate.TextMarginLeft = 0;
            this.btnPerformUpdate.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnPerformUpdate.UseDefaultRadiusAndThickness = true;
            this.btnPerformUpdate.Visible = false;
            this.btnPerformUpdate.Click += new System.EventHandler(this.btnPerformUpdate_Click);
            // 
            // pbCheck
            // 
            this.pbCheck.AllowFocused = false;
            this.pbCheck.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbCheck.AutoSizeHeight = true;
            this.pbCheck.BorderRadius = 16;
            this.pbCheck.Image = global::ExaltAccountManager.Properties.Resources.baseline_autorenew_black_36dp;
            this.pbCheck.IsCircle = true;
            this.pbCheck.Location = new System.Drawing.Point(16, 54);
            this.pbCheck.Name = "pbCheck";
            this.pbCheck.Size = new System.Drawing.Size(32, 32);
            this.pbCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbCheck.TabIndex = 27;
            this.pbCheck.TabStop = false;
            this.pbCheck.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            // 
            // pbStatus
            // 
            this.pbStatus.AllowFocused = false;
            this.pbStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbStatus.AutoSizeHeight = true;
            this.pbStatus.BorderRadius = 16;
            this.pbStatus.Image = global::ExaltAccountManager.Properties.Resources.ic_beenhere_black_36dp;
            this.pbStatus.IsCircle = true;
            this.pbStatus.Location = new System.Drawing.Point(16, 92);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(32, 32);
            this.pbStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbStatus.TabIndex = 31;
            this.pbStatus.TabStop = false;
            this.pbStatus.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            // 
            // progressbar
            // 
            this.progressbar.Animated = false;
            this.progressbar.AnimationInterval = 1;
            this.progressbar.AnimationSpeed = 1;
            this.progressbar.BackColor = System.Drawing.Color.Transparent;
            this.progressbar.CircleMargin = 10;
            this.progressbar.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Bold);
            this.progressbar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressbar.IsPercentage = true;
            this.progressbar.LineProgressThickness = 10;
            this.progressbar.LineThickness = 10;
            this.progressbar.Location = new System.Drawing.Point(99, 132);
            this.progressbar.Name = "progressbar";
            this.progressbar.ProgressAnimationSpeed = 200;
            this.progressbar.ProgressBackColor = System.Drawing.Color.White;
            this.progressbar.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.progressbar.ProgressColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.progressbar.ProgressEndCap = Bunifu.UI.WinForms.BunifuCircleProgress.CapStyles.Round;
            this.progressbar.ProgressFillStyle = Bunifu.UI.WinForms.BunifuCircleProgress.FillStyles.Solid;
            this.progressbar.ProgressStartCap = Bunifu.UI.WinForms.BunifuCircleProgress.CapStyles.Round;
            this.progressbar.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.progressbar.Size = new System.Drawing.Size(150, 150);
            this.progressbar.SubScriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SubScriptMargin = new System.Windows.Forms.Padding(5, -20, 0, 0);
            this.progressbar.SubScriptText = "";
            this.progressbar.SuperScriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SuperScriptMargin = new System.Windows.Forms.Padding(5, 50, 0, 0);
            this.progressbar.SuperScriptText = "%";
            this.progressbar.TabIndex = 16;
            this.progressbar.Text = "30";
            this.progressbar.TextMargin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.progressbar.Value = 30;
            this.progressbar.ValueByTransition = 30;
            this.progressbar.ValueMargin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.progressbar.Visible = false;
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatus.Location = new System.Drawing.Point(158, 100);
            this.lStatus.MaximumSize = new System.Drawing.Size(476, 0);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(80, 20);
            this.lStatus.TabIndex = 30;
            this.lStatus.Text = "Up to date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(55, 99);
            this.label2.MaximumSize = new System.Drawing.Size(476, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 21);
            this.label2.TabIndex = 29;
            this.label2.Text = "Status:";
            // 
            // lLastCheck
            // 
            this.lLastCheck.AutoSize = true;
            this.lLastCheck.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLastCheck.Location = new System.Drawing.Point(158, 62);
            this.lLastCheck.MaximumSize = new System.Drawing.Size(476, 0);
            this.lLastCheck.Name = "lLastCheck";
            this.lLastCheck.Size = new System.Drawing.Size(48, 20);
            this.lLastCheck.TabIndex = 26;
            this.lLastCheck.Text = "Never";
            // 
            // lAnswer
            // 
            this.lAnswer.AutoSize = true;
            this.lAnswer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAnswer.Location = new System.Drawing.Point(55, 61);
            this.lAnswer.MaximumSize = new System.Drawing.Size(476, 0);
            this.lAnswer.Name = "lAnswer";
            this.lAnswer.Size = new System.Drawing.Size(95, 21);
            this.lAnswer.TabIndex = 17;
            this.lAnswer.Text = "Last Check:";
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
            this.spacerQuestion.Size = new System.Drawing.Size(321, 10);
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
            this.pbClose.Location = new System.Drawing.Point(326, 9);
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
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(38, 12);
            this.lHeadline.MaximumSize = new System.Drawing.Size(468, 0);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(152, 25);
            this.lHeadline.TabIndex = 12;
            this.lHeadline.Text = "ROTMG Updater";
            this.lHeadline.UseMnemonic = false;
            // 
            // EleGameUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EleGameUpdater";
            this.Size = new System.Drawing.Size(353, 171);
            this.Load += new System.EventHandler(this.EleGameUpdater_Load);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private Bunifu.UI.WinForms.BunifuPictureBox pbClose;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnPerformUpdate;
        private System.Windows.Forms.Label lAnswer;
        private System.Windows.Forms.Label lHeadline;
        private Bunifu.UI.WinForms.BunifuCircleProgress progressbar;
        private Bunifu.UI.WinForms.BunifuPictureBox pbStatus;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Label label2;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnCheckForUpdate;
        private Bunifu.UI.WinForms.BunifuPictureBox pbCheck;
        private System.Windows.Forms.Label lLastCheck;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnDone;
        private Bunifu.UI.WinForms.BunifuPictureBox pbHeader;
    }
}
