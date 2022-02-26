namespace ExaltAccountManager.UI.Elements
{
    partial class EleChangelog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleChangelog));
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.pContent = new System.Windows.Forms.Panel();
            this.lChangelog = new Bunifu.UI.WinForms.BunifuLabel();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.lReleaseDate = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.pbClose = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lVersion = new System.Windows.Forms.Label();
            this.shadow.SuspendLayout();
            this.pContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // shadow
            // 
            this.shadow.BackColor = System.Drawing.Color.White;
            this.shadow.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadow.BorderRadius = 9;
            this.shadow.BorderThickness = 1;
            this.shadow.Controls.Add(this.scrollbar);
            this.shadow.Controls.Add(this.pContent);
            this.shadow.Controls.Add(this.spacerQuestion);
            this.shadow.Controls.Add(this.lReleaseDate);
            this.shadow.Controls.Add(this.lName);
            this.shadow.Controls.Add(this.pbClose);
            this.shadow.Controls.Add(this.lVersion);
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
            this.shadow.Size = new System.Drawing.Size(600, 218);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 17;
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
            this.scrollbar.AllowScrollOptionsMenu = true;
            this.scrollbar.AllowShrinkingOnFocusLost = false;
            this.scrollbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollbar.BackgroundColor = System.Drawing.Color.Silver;
            this.scrollbar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("scrollbar.BackgroundImage")));
            this.scrollbar.BindingContainer = null;
            this.scrollbar.BorderColor = System.Drawing.Color.Silver;
            this.scrollbar.BorderRadius = 0;
            this.scrollbar.BorderThickness = 2;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(582, 57);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(7);
            this.scrollbar.Maximum = 100;
            this.scrollbar.MaximumSize = new System.Drawing.Size(8, 0);
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumSize = new System.Drawing.Size(8, 20);
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.Silver;
            this.scrollbar.ShrinkSizeLimit = 3;
            this.scrollbar.Size = new System.Drawing.Size(8, 20);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 29;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 18;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.scrollbar.Value = 0;
            this.scrollbar.Visible = false;
            // 
            // pContent
            // 
            this.pContent.Controls.Add(this.lChangelog);
            this.pContent.Location = new System.Drawing.Point(12, 57);
            this.pContent.MaximumSize = new System.Drawing.Size(572, 431);
            this.pContent.MinimumSize = new System.Drawing.Size(572, 0);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(572, 149);
            this.pContent.TabIndex = 30;
            // 
            // lChangelog
            // 
            this.lChangelog.AllowParentOverrides = false;
            this.lChangelog.AutoEllipsis = false;
            this.lChangelog.Cursor = System.Windows.Forms.Cursors.Default;
            this.lChangelog.CursorType = System.Windows.Forms.Cursors.Default;
            this.lChangelog.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lChangelog.Location = new System.Drawing.Point(4, 0);
            this.lChangelog.MaximumSize = new System.Drawing.Size(570, 0);
            this.lChangelog.Name = "lChangelog";
            this.lChangelog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lChangelog.Size = new System.Drawing.Size(72, 20);
            this.lChangelog.TabIndex = 28;
            this.lChangelog.Text = "Changelog";
            this.lChangelog.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.lChangelog.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
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
            this.spacerQuestion.Location = new System.Drawing.Point(16, 47);
            this.spacerQuestion.Margin = new System.Windows.Forms.Padding(5);
            this.spacerQuestion.Name = "spacerQuestion";
            this.spacerQuestion.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.spacerQuestion.Size = new System.Drawing.Size(568, 5);
            this.spacerQuestion.TabIndex = 13;
            // 
            // lReleaseDate
            // 
            this.lReleaseDate.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lReleaseDate.Location = new System.Drawing.Point(16, 33);
            this.lReleaseDate.MaximumSize = new System.Drawing.Size(468, 0);
            this.lReleaseDate.MinimumSize = new System.Drawing.Size(76, 19);
            this.lReleaseDate.Name = "lReleaseDate";
            this.lReleaseDate.Size = new System.Drawing.Size(76, 19);
            this.lReleaseDate.TabIndex = 27;
            this.lReleaseDate.Text = "00.00.0000";
            this.lReleaseDate.UseMnemonic = false;
            // 
            // lName
            // 
            this.lName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(96, 6);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(471, 42);
            this.lName.TabIndex = 26;
            this.lName.Text = "Name";
            this.lName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lName.UseMnemonic = false;
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
            this.pbClose.Location = new System.Drawing.Point(573, 9);
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
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.lVersion.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lVersion.Location = new System.Drawing.Point(14, 11);
            this.lVersion.MaximumSize = new System.Drawing.Size(468, 0);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(80, 21);
            this.lVersion.TabIndex = 12;
            this.lVersion.Text = "00.00.00";
            this.lVersion.UseMnemonic = false;
            // 
            // EleChangelog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(600, 0);
            this.Name = "EleChangelog";
            this.Size = new System.Drawing.Size(600, 218);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            this.pContent.ResumeLayout(false);
            this.pContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private Bunifu.UI.WinForms.BunifuPictureBox pbClose;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lReleaseDate;
        private Bunifu.UI.WinForms.BunifuLabel lChangelog;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private System.Windows.Forms.Panel pContent;
    }
}
