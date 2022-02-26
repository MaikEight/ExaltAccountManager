namespace ExaltAccountManager.UI.Elements
{
    partial class EleHWID_Tool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleHWID_Tool));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.btnFunction = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.pbState = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lValue = new System.Windows.Forms.Label();
            this.lStatus = new System.Windows.Forms.Label();
            this.pbHeader = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pbClose = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.timerBlinkAndCheck = new System.Windows.Forms.Timer(this.components);
            this.shadow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadow.BorderRadius = 9;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.btnFunction);
            this.shadow.Controls.Add(this.pbState);
            this.shadow.Controls.Add(this.lValue);
            this.shadow.Controls.Add(this.lStatus);
            this.shadow.Controls.Add(this.pbHeader);
            this.shadow.Controls.Add(this.lHeadline);
            this.shadow.Controls.Add(this.spacerQuestion);
            this.shadow.Controls.Add(this.pbClose);
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
            this.shadow.Size = new System.Drawing.Size(444, 142);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 17;
            // 
            // btnFunction
            // 
            this.btnFunction.AllowAnimations = true;
            this.btnFunction.AllowMouseEffects = true;
            this.btnFunction.AllowToggling = false;
            this.btnFunction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFunction.AnimationSpeed = 200;
            this.btnFunction.AutoGenerateColors = false;
            this.btnFunction.AutoRoundBorders = false;
            this.btnFunction.AutoSizeLeftIcon = true;
            this.btnFunction.AutoSizeRightIcon = true;
            this.btnFunction.BackColor = System.Drawing.Color.Transparent;
            this.btnFunction.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFunction.BackgroundImage")));
            this.btnFunction.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnFunction.ButtonText = "Download HWID-Tool";
            this.btnFunction.ButtonTextMarginLeft = 0;
            this.btnFunction.ColorContrastOnClick = 45;
            this.btnFunction.ColorContrastOnHover = 45;
            this.btnFunction.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnFunction.CustomizableEdges = borderEdges2;
            this.btnFunction.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnFunction.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnFunction.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnFunction.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnFunction.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnFunction.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction.ForeColor = System.Drawing.Color.White;
            this.btnFunction.IconLeft = global::ExaltAccountManager.Properties.Resources.below_white_24px;
            this.btnFunction.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFunction.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnFunction.IconLeftPadding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.btnFunction.IconMarginLeft = 11;
            this.btnFunction.IconPadding = 6;
            this.btnFunction.IconRight = null;
            this.btnFunction.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnFunction.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnFunction.IconSize = 10;
            this.btnFunction.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.IdleBorderRadius = 5;
            this.btnFunction.IdleBorderThickness = 1;
            this.btnFunction.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.IdleIconLeftImage = global::ExaltAccountManager.Properties.Resources.below_white_24px;
            this.btnFunction.IdleIconRightImage = null;
            this.btnFunction.IndicateFocus = false;
            this.btnFunction.Location = new System.Drawing.Point(50, 94);
            this.btnFunction.Name = "btnFunction";
            this.btnFunction.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnFunction.OnDisabledState.BorderRadius = 5;
            this.btnFunction.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnFunction.OnDisabledState.BorderThickness = 1;
            this.btnFunction.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnFunction.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnFunction.OnDisabledState.IconLeftImage = null;
            this.btnFunction.OnDisabledState.IconRightImage = null;
            this.btnFunction.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.onHoverState.BorderRadius = 5;
            this.btnFunction.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnFunction.onHoverState.BorderThickness = 1;
            this.btnFunction.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnFunction.onHoverState.IconLeftImage = null;
            this.btnFunction.onHoverState.IconRightImage = null;
            this.btnFunction.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.OnIdleState.BorderRadius = 5;
            this.btnFunction.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnFunction.OnIdleState.BorderThickness = 1;
            this.btnFunction.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnFunction.OnIdleState.IconLeftImage = global::ExaltAccountManager.Properties.Resources.below_white_24px;
            this.btnFunction.OnIdleState.IconRightImage = null;
            this.btnFunction.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.OnPressedState.BorderRadius = 5;
            this.btnFunction.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnFunction.OnPressedState.BorderThickness = 1;
            this.btnFunction.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnFunction.OnPressedState.IconLeftImage = null;
            this.btnFunction.OnPressedState.IconRightImage = null;
            this.btnFunction.Size = new System.Drawing.Size(344, 31);
            this.btnFunction.TabIndex = 24;
            this.btnFunction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFunction.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnFunction.TextMarginLeft = 0;
            this.btnFunction.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnFunction.UseDefaultRadiusAndThickness = true;
            this.btnFunction.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // pbState
            // 
            this.pbState.AllowFocused = false;
            this.pbState.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbState.AutoSizeHeight = true;
            this.pbState.BorderRadius = 16;
            this.pbState.Image = global::ExaltAccountManager.Properties.Resources.ic_do_not_disturb_black_36dp;
            this.pbState.IsCircle = true;
            this.pbState.Location = new System.Drawing.Point(16, 54);
            this.pbState.Name = "pbState";
            this.pbState.Size = new System.Drawing.Size(32, 32);
            this.pbState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbState.TabIndex = 38;
            this.pbState.TabStop = false;
            this.pbState.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            // 
            // lValue
            // 
            this.lValue.AutoSize = true;
            this.lValue.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lValue.Location = new System.Drawing.Point(121, 62);
            this.lValue.MaximumSize = new System.Drawing.Size(476, 0);
            this.lValue.Name = "lValue";
            this.lValue.Size = new System.Drawing.Size(215, 20);
            this.lValue.TabIndex = 37;
            this.lValue.Text = "The HWID-Tool is not installed.";
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatus.Location = new System.Drawing.Point(55, 61);
            this.lStatus.MaximumSize = new System.Drawing.Size(476, 0);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(61, 21);
            this.lStatus.TabIndex = 36;
            this.lStatus.Text = "Status:";
            // 
            // pbHeader
            // 
            this.pbHeader.AllowFocused = false;
            this.pbHeader.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbHeader.AutoSizeHeight = true;
            this.pbHeader.BorderRadius = 12;
            this.pbHeader.Image = global::ExaltAccountManager.Properties.Resources.ic_fingerprint_black_48dp;
            this.pbHeader.IsCircle = true;
            this.pbHeader.Location = new System.Drawing.Point(17, 13);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(24, 24);
            this.pbHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbHeader.TabIndex = 35;
            this.pbHeader.TabStop = false;
            this.pbHeader.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(38, 12);
            this.lHeadline.MaximumSize = new System.Drawing.Size(468, 0);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(107, 25);
            this.lHeadline.TabIndex = 34;
            this.lHeadline.Text = "HWID-Tool";
            this.lHeadline.UseMnemonic = false;
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
            this.spacerQuestion.Size = new System.Drawing.Size(412, 10);
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
            this.pbClose.Location = new System.Drawing.Point(417, 9);
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
            // timerBlinkAndCheck
            // 
            this.timerBlinkAndCheck.Interval = 250;
            this.timerBlinkAndCheck.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // EleHWID_Tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EleHWID_Tool";
            this.Size = new System.Drawing.Size(444, 142);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private Bunifu.UI.WinForms.BunifuPictureBox pbClose;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnFunction;
        private Bunifu.UI.WinForms.BunifuPictureBox pbHeader;
        private System.Windows.Forms.Label lHeadline;
        private Bunifu.UI.WinForms.BunifuPictureBox pbState;
        private System.Windows.Forms.Label lValue;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Timer timerBlinkAndCheck;
    }
}
