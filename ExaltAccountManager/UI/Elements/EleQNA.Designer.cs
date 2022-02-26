namespace ExaltAccountManager.UI.Elements
{
    partial class EleQNA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleQNA));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges5 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.shadowQNA = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.pbClose = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.btnFunction = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.lAnswer = new System.Windows.Forms.Label();
            this.lQuestion = new System.Windows.Forms.Label();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.shadowQNA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // shadowQNA
            // 
            this.shadowQNA.BackColor = System.Drawing.Color.White;
            this.shadowQNA.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadowQNA.BorderRadius = 9;
            this.shadowQNA.BorderThickness = 1;
            this.shadowQNA.Controls.Add(this.spacerQuestion);
            this.shadowQNA.Controls.Add(this.pbClose);
            this.shadowQNA.Controls.Add(this.btnFunction);
            this.shadowQNA.Controls.Add(this.lAnswer);
            this.shadowQNA.Controls.Add(this.lQuestion);
            this.shadowQNA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shadowQNA.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadowQNA.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadowQNA.Location = new System.Drawing.Point(0, 0);
            this.shadowQNA.Margin = new System.Windows.Forms.Padding(0);
            this.shadowQNA.Name = "shadowQNA";
            this.shadowQNA.PanelColor = System.Drawing.Color.White;
            this.shadowQNA.PanelColor2 = System.Drawing.Color.White;
            this.shadowQNA.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadowQNA.ShadowDept = 2;
            this.shadowQNA.ShadowDepth = 4;
            this.shadowQNA.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadowQNA.ShadowTopLeftVisible = false;
            this.shadowQNA.Size = new System.Drawing.Size(500, 138);
            this.shadowQNA.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadowQNA.TabIndex = 16;
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
            this.pbClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbClose_MouseDown);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
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
            this.btnFunction.ButtonText = "Button Text";
            this.btnFunction.ButtonTextMarginLeft = 0;
            this.btnFunction.ColorContrastOnClick = 45;
            this.btnFunction.ColorContrastOnHover = 45;
            this.btnFunction.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges5.BottomLeft = true;
            borderEdges5.BottomRight = true;
            borderEdges5.TopLeft = true;
            borderEdges5.TopRight = true;
            this.btnFunction.CustomizableEdges = borderEdges5;
            this.btnFunction.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnFunction.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnFunction.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnFunction.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnFunction.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Hover;
            this.btnFunction.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction.ForeColor = System.Drawing.Color.White;
            this.btnFunction.IconLeft = null;
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
            this.btnFunction.IdleIconLeftImage = null;
            this.btnFunction.IdleIconRightImage = null;
            this.btnFunction.IndicateFocus = false;
            this.btnFunction.Location = new System.Drawing.Point(50, 91);
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
            this.btnFunction.OnIdleState.IconLeftImage = null;
            this.btnFunction.OnIdleState.IconRightImage = null;
            this.btnFunction.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.OnPressedState.BorderRadius = 5;
            this.btnFunction.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnFunction.OnPressedState.BorderThickness = 1;
            this.btnFunction.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.btnFunction.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnFunction.OnPressedState.IconLeftImage = null;
            this.btnFunction.OnPressedState.IconRightImage = null;
            this.btnFunction.Size = new System.Drawing.Size(400, 31);
            this.btnFunction.TabIndex = 24;
            this.btnFunction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFunction.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnFunction.TextMarginLeft = 0;
            this.btnFunction.TextPadding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnFunction.UseDefaultRadiusAndThickness = true;
            this.btnFunction.Visible = false;
            this.btnFunction.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // lAnswer
            // 
            this.lAnswer.AutoSize = true;
            this.lAnswer.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAnswer.Location = new System.Drawing.Point(12, 50);
            this.lAnswer.MaximumSize = new System.Drawing.Size(476, 0);
            this.lAnswer.Name = "lAnswer";
            this.lAnswer.Size = new System.Drawing.Size(90, 20);
            this.lAnswer.TabIndex = 17;
            this.lAnswer.Text = "Answer-Text";
            // 
            // lQuestion
            // 
            this.lQuestion.AutoSize = true;
            this.lQuestion.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(12, 12);
            this.lQuestion.MaximumSize = new System.Drawing.Size(468, 0);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(89, 25);
            this.lQuestion.TabIndex = 12;
            this.lQuestion.Text = "Question";
            this.lQuestion.UseMnemonic = false;
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
            // EleQNA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadowQNA);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EleQNA";
            this.Size = new System.Drawing.Size(500, 138);
            this.shadowQNA.ResumeLayout(false);
            this.shadowQNA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadowQNA;
        private System.Windows.Forms.Label lAnswer;
        private System.Windows.Forms.Label lQuestion;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnFunction;
        private Bunifu.UI.WinForms.BunifuPictureBox pbClose;
    }
}
