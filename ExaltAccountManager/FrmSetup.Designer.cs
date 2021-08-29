
namespace ExaltAccountManager
{
    partial class FrmSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetup));
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState1 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState2 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState toggleState3 = new Bunifu.UI.WinForms.BunifuToggleSwitch.ToggleState();
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.lDarkmode = new System.Windows.Forms.Label();
            this.lText = new System.Windows.Forms.Label();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.seperator = new Bunifu.UI.WinForms.BunifuSeparator();
            this.toggleUseDarkmode = new Bunifu.UI.WinForms.BunifuToggleSwitch();
            this.btnLoadSaveFile = new System.Windows.Forms.Button();
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.pBox);
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Controls.Add(this.lHeadline);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(250, 48);
            this.pTop.TabIndex = 2;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            // 
            // pBox
            // 
            this.pBox.Controls.Add(this.pbMinimize);
            this.pBox.Controls.Add(this.pbClose);
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Location = new System.Drawing.Point(202, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(48, 48);
            this.pBox.TabIndex = 4;
            this.pBox.Visible = false;
            // 
            // pbMinimize
            // 
            this.pbMinimize.Image = global::ExaltAccountManager.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(0, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMinimize.TabIndex = 1;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.Visible = false;
            // 
            // pbClose
            // 
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(24, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 0;
            this.pbClose.TabStop = false;
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.maintenance_black_48px;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Gothic", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(62, 11);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(128, 23);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Quick setup";
            // 
            // lDarkmode
            // 
            this.lDarkmode.AutoSize = true;
            this.lDarkmode.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDarkmode.Location = new System.Drawing.Point(50, 61);
            this.lDarkmode.Name = "lDarkmode";
            this.lDarkmode.Size = new System.Drawing.Size(94, 16);
            this.lDarkmode.TabIndex = 5;
            this.lDarkmode.Text = "Use darkmode";
            // 
            // lText
            // 
            this.lText.AutoSize = true;
            this.lText.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lText.Location = new System.Drawing.Point(10, 106);
            this.lText.MaximumSize = new System.Drawing.Size(230, 0);
            this.lText.Name = "lText";
            this.lText.Size = new System.Drawing.Size(227, 48);
            this.lText.TabIndex = 9;
            this.lText.Text = "Did you already use an old version and want to import the old save files?";
            // 
            // btnNo
            // 
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Image = global::ExaltAccountManager.Properties.Resources.new_Outline_black_18px;
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo.Location = new System.Drawing.Point(10, 254);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(230, 32);
            this.btnNo.TabIndex = 10;
            this.btnNo.TabStop = false;
            this.btnNo.Text = "Start fresh";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.MouseEnter += new System.EventHandler(this.btnNo_MouseEnter);
            this.btnNo.MouseLeave += new System.EventHandler(this.btnNo_MouseLeave);
            // 
            // btnYes
            // 
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Image = global::ExaltAccountManager.Properties.Resources.ic_folder_open_black_18dp;
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnYes.Location = new System.Drawing.Point(10, 170);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(230, 32);
            this.btnYes.TabIndex = 8;
            this.btnYes.TabStop = false;
            this.btnYes.Text = "      Yes, choose path to old EAM version";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnYes.MouseEnter += new System.EventHandler(this.btnYes_MouseEnter);
            this.btnYes.MouseLeave += new System.EventHandler(this.btnYes_MouseLeave);
            // 
            // seperator
            // 
            this.seperator.BackColor = System.Drawing.Color.Transparent;
            this.seperator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("seperator.BackgroundImage")));
            this.seperator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seperator.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.seperator.LineColor = System.Drawing.Color.Silver;
            this.seperator.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.seperator.LineThickness = 1;
            this.seperator.Location = new System.Drawing.Point(10, 88);
            this.seperator.Name = "seperator";
            this.seperator.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.seperator.Size = new System.Drawing.Size(230, 5);
            this.seperator.TabIndex = 6;
            // 
            // toggleUseDarkmode
            // 
            this.toggleUseDarkmode.Animation = 5;
            this.toggleUseDarkmode.BackColor = System.Drawing.Color.Transparent;
            this.toggleUseDarkmode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toggleUseDarkmode.BackgroundImage")));
            this.toggleUseDarkmode.Checked = true;
            this.toggleUseDarkmode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toggleUseDarkmode.InnerCirclePadding = 3;
            this.toggleUseDarkmode.Location = new System.Drawing.Point(12, 60);
            this.toggleUseDarkmode.Name = "toggleUseDarkmode";
            this.toggleUseDarkmode.Size = new System.Drawing.Size(32, 18);
            this.toggleUseDarkmode.TabIndex = 3;
            this.toggleUseDarkmode.ThumbMargin = 3;
            toggleState1.BackColor = System.Drawing.Color.DarkGray;
            toggleState1.BackColorInner = System.Drawing.Color.White;
            toggleState1.BorderColor = System.Drawing.Color.DarkGray;
            toggleState1.BorderColorInner = System.Drawing.Color.White;
            toggleState1.BorderRadius = 17;
            toggleState1.BorderRadiusInner = 11;
            toggleState1.BorderThickness = 1;
            toggleState1.BorderThicknessInner = 1;
            this.toggleUseDarkmode.ToggleStateDisabled = toggleState1;
            toggleState2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            toggleState2.BackColorInner = System.Drawing.Color.White;
            toggleState2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            toggleState2.BorderColorInner = System.Drawing.Color.White;
            toggleState2.BorderRadius = 17;
            toggleState2.BorderRadiusInner = 11;
            toggleState2.BorderThickness = 1;
            toggleState2.BorderThicknessInner = 1;
            this.toggleUseDarkmode.ToggleStateOff = toggleState2;
            toggleState3.BackColor = System.Drawing.Color.Black;
            toggleState3.BackColorInner = System.Drawing.Color.White;
            toggleState3.BorderColor = System.Drawing.Color.Black;
            toggleState3.BorderColorInner = System.Drawing.Color.White;
            toggleState3.BorderRadius = 17;
            toggleState3.BorderRadiusInner = 11;
            toggleState3.BorderThickness = 1;
            toggleState3.BorderThicknessInner = 1;
            this.toggleUseDarkmode.ToggleStateOn = toggleState3;
            this.toggleUseDarkmode.Value = true;
            this.toggleUseDarkmode.CheckedChanged += new System.EventHandler<Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs>(this.toggleUseDarkmode_CheckedChanged);
            // 
            // btnLoadSaveFile
            // 
            this.btnLoadSaveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadSaveFile.Image = global::ExaltAccountManager.Properties.Resources.ic_import_export_black_24dp;
            this.btnLoadSaveFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadSaveFile.Location = new System.Drawing.Point(10, 212);
            this.btnLoadSaveFile.Name = "btnLoadSaveFile";
            this.btnLoadSaveFile.Size = new System.Drawing.Size(230, 32);
            this.btnLoadSaveFile.TabIndex = 11;
            this.btnLoadSaveFile.TabStop = false;
            this.btnLoadSaveFile.Text = "Import: exported- or muledump file  ";
            this.btnLoadSaveFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoadSaveFile.UseVisualStyleBackColor = true;
            this.btnLoadSaveFile.Click += new System.EventHandler(this.btnLoadSaveFile_Click);
            // 
            // FrmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 296);
            this.Controls.Add(this.btnLoadSaveFile);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.lText);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.seperator);
            this.Controls.Add(this.lDarkmode);
            this.Controls.Add(this.toggleUseDarkmode);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(250, 296);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 296);
            this.Name = "FrmSetup";
            this.Text = "EAM Setup";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmSetup_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lHeadline;
        private Bunifu.UI.WinForms.BunifuToggleSwitch toggleUseDarkmode;
        private System.Windows.Forms.Label lDarkmode;
        private Bunifu.UI.WinForms.BunifuSeparator seperator;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Label lText;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnLoadSaveFile;
    }
}