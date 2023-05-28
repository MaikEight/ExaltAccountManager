namespace ExaltAccountManager.UI.Elements
{
    partial class EleDiscordUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleDiscordUser));
            this.shadow = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.pbClose = new Bunifu.UI.WinForms.BunifuPictureBox();
            this.lHeadlines = new System.Windows.Forms.Label();
            this.lQuestion = new System.Windows.Forms.Label();
            this.lValues = new System.Windows.Forms.Label();
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
            this.shadow.Controls.Add(this.lValues);
            this.shadow.Controls.Add(this.spacerQuestion);
            this.shadow.Controls.Add(this.pbClose);
            this.shadow.Controls.Add(this.lHeadlines);
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
            this.shadow.Size = new System.Drawing.Size(500, 183);
            this.shadow.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadow.TabIndex = 16;
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
            this.pbClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbClose_MouseDown);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // lHeadlines
            // 
            this.lHeadlines.AutoSize = true;
            this.lHeadlines.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadlines.Location = new System.Drawing.Point(12, 50);
            this.lHeadlines.MaximumSize = new System.Drawing.Size(476, 0);
            this.lHeadlines.Name = "lHeadlines";
            this.lHeadlines.Size = new System.Drawing.Size(180, 120);
            this.lHeadlines.TabIndex = 17;
            this.lHeadlines.Text = "Amount of Accounts\r\nAmount of EAM-Sessions\r\nFirst seen\r\nEAM runtime\r\nDiscord User" +
    " ID\r\nSecret Found ";
            // 
            // lQuestion
            // 
            this.lQuestion.AutoSize = true;
            this.lQuestion.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(12, 12);
            this.lQuestion.MaximumSize = new System.Drawing.Size(468, 0);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(139, 25);
            this.lQuestion.TabIndex = 12;
            this.lQuestion.Text = "EAM User data";
            this.lQuestion.UseMnemonic = false;
            // 
            // lValues
            // 
            this.lValues.AutoSize = true;
            this.lValues.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lValues.Location = new System.Drawing.Point(198, 50);
            this.lValues.MaximumSize = new System.Drawing.Size(476, 0);
            this.lValues.Name = "lValues";
            this.lValues.Size = new System.Drawing.Size(0, 20);
            this.lValues.TabIndex = 26;
            // 
            // EleDiscordUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.shadow);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EleDiscordUser";
            this.Size = new System.Drawing.Size(500, 183);
            this.shadow.ResumeLayout(false);
            this.shadow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadow;
        private System.Windows.Forms.Label lHeadlines;
        private System.Windows.Forms.Label lQuestion;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private Bunifu.UI.WinForms.BunifuPictureBox pbClose;
        private System.Windows.Forms.Label lValues;
    }
}
