
namespace EAM_PingChecker
{
    partial class UIAbout
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
            this.shadowLoading = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lPingText = new System.Windows.Forms.Label();
            this.pbProgram = new System.Windows.Forms.PictureBox();
            this.lServerName = new System.Windows.Forms.Label();
            this.bunifuShadowPanel1 = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lVersion = new System.Windows.Forms.Label();
            this.pbVersion = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bunifuShadowPanel2 = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.pbDeveloper = new System.Windows.Forms.PictureBox();
            this.pbDev = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bunifuShadowPanel3 = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.linkWebsite = new System.Windows.Forms.LinkLabel();
            this.pbWebsite = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bunifuShadowPanel4 = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lCopyright = new System.Windows.Forms.Label();
            this.pbCopyright = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lTool = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.timerReset = new System.Windows.Forms.Timer(this.components);
            this.shadowLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProgram)).BeginInit();
            this.bunifuShadowPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVersion)).BeginInit();
            this.bunifuShadowPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeveloper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDev)).BeginInit();
            this.bunifuShadowPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebsite)).BeginInit();
            this.bunifuShadowPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // shadowLoading
            // 
            this.shadowLoading.BackColor = System.Drawing.Color.White;
            this.shadowLoading.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadowLoading.BorderRadius = 9;
            this.shadowLoading.BorderThickness = 1;
            this.shadowLoading.Controls.Add(this.lPingText);
            this.shadowLoading.Controls.Add(this.pbProgram);
            this.shadowLoading.Controls.Add(this.lServerName);
            this.shadowLoading.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadowLoading.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadowLoading.Location = new System.Drawing.Point(3, 3);
            this.shadowLoading.Name = "shadowLoading";
            this.shadowLoading.PanelColor = System.Drawing.Color.White;
            this.shadowLoading.PanelColor2 = System.Drawing.Color.White;
            this.shadowLoading.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.shadowLoading.ShadowDept = 2;
            this.shadowLoading.ShadowDepth = 4;
            this.shadowLoading.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadowLoading.ShadowTopLeftVisible = false;
            this.shadowLoading.Size = new System.Drawing.Size(365, 80);
            this.shadowLoading.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadowLoading.TabIndex = 3;
            // 
            // lPingText
            // 
            this.lPingText.AutoSize = true;
            this.lPingText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPingText.Location = new System.Drawing.Point(57, 38);
            this.lPingText.Name = "lPingText";
            this.lPingText.Size = new System.Drawing.Size(291, 21);
            this.lPingText.TabIndex = 7;
            this.lPingText.Text = "Exalt Account Manager Ping Checker";
            // 
            // pbProgram
            // 
            this.pbProgram.Image = global::EAM_PingChecker.Properties.Resources.time_black_36px;
            this.pbProgram.Location = new System.Drawing.Point(13, 31);
            this.pbProgram.Name = "pbProgram";
            this.pbProgram.Size = new System.Drawing.Size(36, 36);
            this.pbProgram.TabIndex = 11;
            this.pbProgram.TabStop = false;
            // 
            // lServerName
            // 
            this.lServerName.AutoSize = true;
            this.lServerName.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F);
            this.lServerName.Location = new System.Drawing.Point(10, 11);
            this.lServerName.Name = "lServerName";
            this.lServerName.Size = new System.Drawing.Size(61, 13);
            this.lServerName.TabIndex = 6;
            this.lServerName.Text = "Information";
            // 
            // bunifuShadowPanel1
            // 
            this.bunifuShadowPanel1.BackColor = System.Drawing.Color.White;
            this.bunifuShadowPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel1.BorderRadius = 9;
            this.bunifuShadowPanel1.BorderThickness = 1;
            this.bunifuShadowPanel1.Controls.Add(this.lVersion);
            this.bunifuShadowPanel1.Controls.Add(this.pbVersion);
            this.bunifuShadowPanel1.Controls.Add(this.label2);
            this.bunifuShadowPanel1.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.bunifuShadowPanel1.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.bunifuShadowPanel1.Location = new System.Drawing.Point(3, 86);
            this.bunifuShadowPanel1.Name = "bunifuShadowPanel1";
            this.bunifuShadowPanel1.PanelColor = System.Drawing.Color.White;
            this.bunifuShadowPanel1.PanelColor2 = System.Drawing.Color.White;
            this.bunifuShadowPanel1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel1.ShadowDept = 2;
            this.bunifuShadowPanel1.ShadowDepth = 4;
            this.bunifuShadowPanel1.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.bunifuShadowPanel1.ShadowTopLeftVisible = false;
            this.bunifuShadowPanel1.Size = new System.Drawing.Size(365, 80);
            this.bunifuShadowPanel1.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.bunifuShadowPanel1.TabIndex = 4;
            // 
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.lVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lVersion.Location = new System.Drawing.Point(57, 38);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(195, 21);
            this.lVersion.TabIndex = 7;
            this.lVersion.Text = "Ping Checker version {0}";
            // 
            // pbVersion
            // 
            this.pbVersion.Image = global::EAM_PingChecker.Properties.Resources.baseline_tag_black_36dp;
            this.pbVersion.Location = new System.Drawing.Point(13, 31);
            this.pbVersion.Name = "pbVersion";
            this.pbVersion.Size = new System.Drawing.Size(36, 36);
            this.pbVersion.TabIndex = 11;
            this.pbVersion.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F);
            this.label2.Location = new System.Drawing.Point(10, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Version";
            // 
            // bunifuShadowPanel2
            // 
            this.bunifuShadowPanel2.BackColor = System.Drawing.Color.White;
            this.bunifuShadowPanel2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel2.BorderRadius = 9;
            this.bunifuShadowPanel2.BorderThickness = 1;
            this.bunifuShadowPanel2.Controls.Add(this.pbDeveloper);
            this.bunifuShadowPanel2.Controls.Add(this.pbDev);
            this.bunifuShadowPanel2.Controls.Add(this.label4);
            this.bunifuShadowPanel2.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.bunifuShadowPanel2.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.bunifuShadowPanel2.Location = new System.Drawing.Point(3, 169);
            this.bunifuShadowPanel2.Name = "bunifuShadowPanel2";
            this.bunifuShadowPanel2.PanelColor = System.Drawing.Color.White;
            this.bunifuShadowPanel2.PanelColor2 = System.Drawing.Color.White;
            this.bunifuShadowPanel2.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel2.ShadowDept = 2;
            this.bunifuShadowPanel2.ShadowDepth = 4;
            this.bunifuShadowPanel2.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.bunifuShadowPanel2.ShadowTopLeftVisible = false;
            this.bunifuShadowPanel2.Size = new System.Drawing.Size(365, 80);
            this.bunifuShadowPanel2.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.bunifuShadowPanel2.TabIndex = 5;
            // 
            // pbDeveloper
            // 
            this.pbDeveloper.Image = global::EAM_PingChecker.Properties.Resources.Logo_NameOnly_3_Big;
            this.pbDeveloper.Location = new System.Drawing.Point(61, 31);
            this.pbDeveloper.Name = "pbDeveloper";
            this.pbDeveloper.Size = new System.Drawing.Size(138, 36);
            this.pbDeveloper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDeveloper.TabIndex = 12;
            this.pbDeveloper.TabStop = false;
            this.pbDeveloper.Click += new System.EventHandler(this.pbDeveloper_Click);
            // 
            // pbDev
            // 
            this.pbDev.Image = global::EAM_PingChecker.Properties.Resources.ic_code_black_36dp;
            this.pbDev.Location = new System.Drawing.Point(13, 31);
            this.pbDev.Name = "pbDev";
            this.pbDev.Size = new System.Drawing.Size(36, 36);
            this.pbDev.TabIndex = 11;
            this.pbDev.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F);
            this.label4.Location = new System.Drawing.Point(10, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Developer";
            // 
            // bunifuShadowPanel3
            // 
            this.bunifuShadowPanel3.BackColor = System.Drawing.Color.White;
            this.bunifuShadowPanel3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel3.BorderRadius = 9;
            this.bunifuShadowPanel3.BorderThickness = 1;
            this.bunifuShadowPanel3.Controls.Add(this.linkWebsite);
            this.bunifuShadowPanel3.Controls.Add(this.pbWebsite);
            this.bunifuShadowPanel3.Controls.Add(this.label3);
            this.bunifuShadowPanel3.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.bunifuShadowPanel3.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.bunifuShadowPanel3.Location = new System.Drawing.Point(3, 252);
            this.bunifuShadowPanel3.Name = "bunifuShadowPanel3";
            this.bunifuShadowPanel3.PanelColor = System.Drawing.Color.White;
            this.bunifuShadowPanel3.PanelColor2 = System.Drawing.Color.White;
            this.bunifuShadowPanel3.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel3.ShadowDept = 2;
            this.bunifuShadowPanel3.ShadowDepth = 4;
            this.bunifuShadowPanel3.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.bunifuShadowPanel3.ShadowTopLeftVisible = false;
            this.bunifuShadowPanel3.Size = new System.Drawing.Size(365, 80);
            this.bunifuShadowPanel3.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.bunifuShadowPanel3.TabIndex = 6;
            // 
            // linkWebsite
            // 
            this.linkWebsite.ActiveLinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkWebsite.AutoSize = true;
            this.linkWebsite.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkWebsite.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkWebsite.Location = new System.Drawing.Point(58, 32);
            this.linkWebsite.MaximumSize = new System.Drawing.Size(220, 0);
            this.linkWebsite.Name = "linkWebsite";
            this.linkWebsite.Size = new System.Drawing.Size(210, 32);
            this.linkWebsite.TabIndex = 13;
            this.linkWebsite.TabStop = true;
            this.linkWebsite.Text = "https://github.com/MaikEight/ExaltAccountManager";
            this.linkWebsite.VisitedLinkColor = System.Drawing.Color.Purple;
            // 
            // pbWebsite
            // 
            this.pbWebsite.Image = global::EAM_PingChecker.Properties.Resources.ic_public_black_36dp;
            this.pbWebsite.Location = new System.Drawing.Point(13, 31);
            this.pbWebsite.Name = "pbWebsite";
            this.pbWebsite.Size = new System.Drawing.Size(36, 36);
            this.pbWebsite.TabIndex = 11;
            this.pbWebsite.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F);
            this.label3.Location = new System.Drawing.Point(10, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Website";
            // 
            // bunifuShadowPanel4
            // 
            this.bunifuShadowPanel4.BackColor = System.Drawing.Color.White;
            this.bunifuShadowPanel4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel4.BorderRadius = 9;
            this.bunifuShadowPanel4.BorderThickness = 1;
            this.bunifuShadowPanel4.Controls.Add(this.lCopyright);
            this.bunifuShadowPanel4.Controls.Add(this.pbCopyright);
            this.bunifuShadowPanel4.Controls.Add(this.label5);
            this.bunifuShadowPanel4.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.bunifuShadowPanel4.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.bunifuShadowPanel4.Location = new System.Drawing.Point(3, 335);
            this.bunifuShadowPanel4.Name = "bunifuShadowPanel4";
            this.bunifuShadowPanel4.PanelColor = System.Drawing.Color.White;
            this.bunifuShadowPanel4.PanelColor2 = System.Drawing.Color.White;
            this.bunifuShadowPanel4.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bunifuShadowPanel4.ShadowDept = 2;
            this.bunifuShadowPanel4.ShadowDepth = 4;
            this.bunifuShadowPanel4.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.bunifuShadowPanel4.ShadowTopLeftVisible = false;
            this.bunifuShadowPanel4.Size = new System.Drawing.Size(610, 137);
            this.bunifuShadowPanel4.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.bunifuShadowPanel4.TabIndex = 7;
            // 
            // lCopyright
            // 
            this.lCopyright.AutoSize = true;
            this.lCopyright.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCopyright.Location = new System.Drawing.Point(57, 37);
            this.lCopyright.Name = "lCopyright";
            this.lCopyright.Size = new System.Drawing.Size(499, 84);
            this.lCopyright.TabIndex = 12;
            this.lCopyright.Text = "This tool is owned and ©opyrighted\r\nby Maik \"Maik8\" Kühne\r\n\r\nPublishing this tool" +
    " without explicit permissions is not tolerated";
            // 
            // pbCopyright
            // 
            this.pbCopyright.Image = global::EAM_PingChecker.Properties.Resources.ic_copyright_black_36dp;
            this.pbCopyright.Location = new System.Drawing.Point(13, 31);
            this.pbCopyright.Name = "pbCopyright";
            this.pbCopyright.Size = new System.Drawing.Size(36, 36);
            this.pbCopyright.TabIndex = 11;
            this.pbCopyright.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F);
            this.label5.Location = new System.Drawing.Point(10, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Disclaimer";
            // 
            // lTool
            // 
            this.lTool.Font = new System.Drawing.Font("Segoe UI Black", 41F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTool.Location = new System.Drawing.Point(85, 500);
            this.lTool.Name = "lTool";
            this.lTool.Size = new System.Drawing.Size(543, 86);
            this.lTool.TabIndex = 15;
            this.lTool.Text = "EAM  Ping Checker";
            this.lTool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::EAM_PingChecker.Properties.Resources.time_black_96px;
            this.pbLogo.Location = new System.Drawing.Point(-2, 495);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(96, 96);
            this.pbLogo.TabIndex = 16;
            this.pbLogo.TabStop = false;
            // 
            // timerReset
            // 
            this.timerReset.Interval = 2500;
            this.timerReset.Tick += new System.EventHandler(this.timerReset_Tick);
            // 
            // UIAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.lTool);
            this.Controls.Add(this.bunifuShadowPanel4);
            this.Controls.Add(this.bunifuShadowPanel3);
            this.Controls.Add(this.bunifuShadowPanel2);
            this.Controls.Add(this.bunifuShadowPanel1);
            this.Controls.Add(this.shadowLoading);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIAbout";
            this.Size = new System.Drawing.Size(620, 600);
            this.shadowLoading.ResumeLayout(false);
            this.shadowLoading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProgram)).EndInit();
            this.bunifuShadowPanel1.ResumeLayout(false);
            this.bunifuShadowPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVersion)).EndInit();
            this.bunifuShadowPanel2.ResumeLayout(false);
            this.bunifuShadowPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeveloper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDev)).EndInit();
            this.bunifuShadowPanel3.ResumeLayout(false);
            this.bunifuShadowPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebsite)).EndInit();
            this.bunifuShadowPanel4.ResumeLayout(false);
            this.bunifuShadowPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel shadowLoading;
        private System.Windows.Forms.Label lPingText;
        private System.Windows.Forms.Label lServerName;
        private System.Windows.Forms.PictureBox pbProgram;
        private Bunifu.UI.WinForms.BunifuShadowPanel bunifuShadowPanel1;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.PictureBox pbVersion;
        private System.Windows.Forms.Label label2;
        private Bunifu.UI.WinForms.BunifuShadowPanel bunifuShadowPanel2;
        private System.Windows.Forms.PictureBox pbDev;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pbDeveloper;
        private Bunifu.UI.WinForms.BunifuShadowPanel bunifuShadowPanel3;
        private System.Windows.Forms.PictureBox pbWebsite;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkWebsite;
        private Bunifu.UI.WinForms.BunifuShadowPanel bunifuShadowPanel4;
        private System.Windows.Forms.PictureBox pbCopyright;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lCopyright;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lTool;
        private System.Windows.Forms.Timer timerReset;
    }
}
