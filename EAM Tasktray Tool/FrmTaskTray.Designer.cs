
namespace EAM_Tasktray_Tool
{
    partial class FrmTaskTray
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTaskTray));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lHeader = new System.Windows.Forms.Label();
            this.lProgress = new System.Windows.Forms.Label();
            this.lText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.progressBar = new EAM_Tasktray_Tool.EAM_ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "30 / 100 Accounts done.";
            this.notifyIcon.BalloonTipTitle = "Exalt Account Manager Daily Login";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "EAM Daily Login\r\nClick to open progress.";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // lHeader
            // 
            this.lHeader.AutoSize = true;
            this.lHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeader.Location = new System.Drawing.Point(43, 7);
            this.lHeader.Name = "lHeader";
            this.lHeader.Size = new System.Drawing.Size(136, 21);
            this.lHeader.TabIndex = 0;
            this.lHeader.Text = "EAM Daily Login";
            // 
            // lProgress
            // 
            this.lProgress.Location = new System.Drawing.Point(5, 64);
            this.lProgress.Name = "lProgress";
            this.lProgress.Size = new System.Drawing.Size(240, 17);
            this.lProgress.TabIndex = 4;
            this.lProgress.Text = "No data available";
            this.lProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lText
            // 
            this.lText.Location = new System.Drawing.Point(5, 80);
            this.lText.Name = "lText";
            this.lText.Size = new System.Drawing.Size(240, 17);
            this.lText.TabIndex = 6;
            this.lText.Text = "Task is currently running, please wait...";
            this.lText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lText.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI Semilight", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(226, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 11);
            this.label2.TabIndex = 7;
            this.label2.Text = "v1.0";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbMinimize
            // 
            this.pbMinimize.Image = global::EAM_Tasktray_Tool.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(233, 1);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(16, 16);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMinimize.TabIndex = 5;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::EAM_Tasktray_Tool.Properties.Resources.ic_account_balance_wallet_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(1, 1);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(36, 36);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "EAM.DailyLogins";
            this.fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Changed);
            // 
            // timerClose
            // 
            this.timerClose.Interval = 15000;
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.progressBar.ForeColor = System.Drawing.Color.DodgerBlue;
            this.progressBar.Location = new System.Drawing.Point(5, 42);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(240, 23);
            this.progressBar.TabIndex = 3;
            // 
            // FrmTaskTray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(250, 100);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lText);
            this.Controls.Add(this.pbMinimize);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lProgress);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.lHeader);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmTaskTray";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "EAM Task Tray Tool";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTaskTray_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmTaskTray_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label lHeader;
        private System.Windows.Forms.PictureBox pbLogo;
        private EAM_ProgressBar progressBar;
        private System.Windows.Forms.Label lProgress;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.Label lText;
        private System.Windows.Forms.Label label2;
        private System.IO.FileSystemWatcher fileSystemWatcher;
        private System.Windows.Forms.Timer timerClose;
    }
}

