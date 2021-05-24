namespace ExaltAccountManager
{
    partial class FrmMain
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.lDev = new System.Windows.Forms.Label();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.lVersion = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.pMain = new System.Windows.Forms.Panel();
            this.timerLlama = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timerLoadProcesses = new System.Windows.Forms.Timer(this.components);
            this.timerMoreToolsUI = new System.Windows.Forms.Timer(this.components);
            this.header = new ExaltAccountManager.AccountUIHeader();
            this.pbDarkmode = new ExaltAccountManager.RoundPictureBox();
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDarkmode)).BeginInit();
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
            this.pTop.Size = new System.Drawing.Size(580, 48);
            this.pTop.TabIndex = 0;
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // pBox
            // 
            this.pBox.Controls.Add(this.pbDarkmode);
            this.pBox.Controls.Add(this.lDev);
            this.pBox.Controls.Add(this.pbMinimize);
            this.pBox.Controls.Add(this.pbClose);
            this.pBox.Controls.Add(this.lVersion);
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Location = new System.Drawing.Point(490, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(90, 48);
            this.pBox.TabIndex = 4;
            this.pBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // lDev
            // 
            this.lDev.AutoSize = true;
            this.lDev.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDev.Location = new System.Drawing.Point(37, 33);
            this.lDev.Name = "lDev";
            this.lDev.Size = new System.Drawing.Size(54, 15);
            this.lDev.TabIndex = 5;
            this.lDev.Text = "by Maik8";
            this.toolTip.SetToolTip(this.lDev, "The Developer!");
            this.lDev.Click += new System.EventHandler(this.lDev_Click);
            // 
            // pbMinimize
            // 
            this.pbMinimize.Image = global::ExaltAccountManager.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(42, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMinimize.TabIndex = 1;
            this.pbMinimize.TabStop = false;
            this.toolTip.SetToolTip(this.pbMinimize, "Minimize");
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbClose
            // 
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(66, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 0;
            this.pbClose.TabStop = false;
            this.toolTip.SetToolTip(this.pbClose, "Close");
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.lVersion.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lVersion.Location = new System.Drawing.Point(3, 33);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(37, 15);
            this.lVersion.TabIndex = 6;
            this.lVersion.Text = "v1.4.2";
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_account_balance_wallet_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pbLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(54, 9);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(243, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Exalt Account Manager";
            this.lHeadline.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.lHeadline.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // pMain
            // 
            this.pMain.AutoScroll = true;
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(0, 84);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(580, 591);
            this.pMain.TabIndex = 1;
            // 
            // timerLlama
            // 
            this.timerLlama.Interval = 2500;
            this.timerLlama.Tick += new System.EventHandler(this.timerLlama_Tick);
            // 
            // toolTip
            // 
            this.toolTip.OwnerDraw = true;
            this.toolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.toolTip_Draw);
            // 
            // timerLoadProcesses
            // 
            this.timerLoadProcesses.Interval = 5000;
            this.timerLoadProcesses.Tick += new System.EventHandler(this.timerLoadProcesses_Tick);
            // 
            // timerMoreToolsUI
            // 
            this.timerMoreToolsUI.Interval = 2;
            this.timerMoreToolsUI.Tick += new System.EventHandler(this.timerMoreToolsUI_Tick);
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(0, 48);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(580, 36);
            this.header.TabIndex = 2;
            // 
            // pbDarkmode
            // 
            this.pbDarkmode.BackColor = System.Drawing.Color.White;
            this.pbDarkmode.Image = global::ExaltAccountManager.Properties.Resources.ic_brightness_4_black_48dp;
            this.pbDarkmode.Location = new System.Drawing.Point(5, 0);
            this.pbDarkmode.Name = "pbDarkmode";
            this.pbDarkmode.Size = new System.Drawing.Size(32, 32);
            this.pbDarkmode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDarkmode.TabIndex = 6;
            this.pbDarkmode.TabStop = false;
            this.toolTip.SetToolTip(this.pbDarkmode, "Switch to darkmode!");
            this.pbDarkmode.Click += new System.EventHandler(this.pbDarkmode_Click);
            this.pbDarkmode.MouseEnter += new System.EventHandler(this.pbDarkmode_MouseEnter);
            this.pbDarkmode.MouseLeave += new System.EventHandler(this.pbDarkmode_MouseLeave);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(580, 675);
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.header);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.LocationChanged += new System.EventHandler(this.FrmMain_LocationChanged);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            this.pBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDarkmode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label lDev;
        private System.Windows.Forms.Timer timerLlama;
        private AccountUIHeader header;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.Timer timerLoadProcesses;
        private System.Windows.Forms.Timer timerMoreToolsUI;
        private RoundPictureBox pbDarkmode;
    }
}

