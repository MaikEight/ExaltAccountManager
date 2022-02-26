
namespace ExaltAccountManager
{
    partial class UIWindowsTask
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
            this.lBack = new System.Windows.Forms.Label();
            this.lWaitMinutes = new System.Windows.Forms.Label();
            this.lHeader = new System.Windows.Forms.Label();
            this.btnInstallTask = new System.Windows.Forms.Button();
            this.tbWaitMinutes = new System.Windows.Forms.TextBox();
            this.tbResetTime = new System.Windows.Forms.TextBox();
            this.lTime = new System.Windows.Forms.Label();
            this.lIsInstalled = new System.Windows.Forms.Label();
            this.lInfo = new System.Windows.Forms.Label();
            this.btnUninstallTask = new System.Windows.Forms.Button();
            this.pbBack = new Bunifu.UI.WinForms.BunifuPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            this.SuspendLayout();
            // 
            // lBack
            // 
            this.lBack.AutoSize = true;
            this.lBack.Location = new System.Drawing.Point(-1, 23);
            this.lBack.Name = "lBack";
            this.lBack.Size = new System.Drawing.Size(33, 15);
            this.lBack.TabIndex = 18;
            this.lBack.Text = "Back";
            this.lBack.Click += new System.EventHandler(this.pbBack_Click);
            this.lBack.MouseEnter += new System.EventHandler(this.pbBack_MouseEnter);
            this.lBack.MouseLeave += new System.EventHandler(this.pbBack_MouseLeave);
            // 
            // lWaitMinutes
            // 
            this.lWaitMinutes.Location = new System.Drawing.Point(63, 31);
            this.lWaitMinutes.Name = "lWaitMinutes";
            this.lWaitMinutes.Size = new System.Drawing.Size(172, 48);
            this.lWaitMinutes.TabIndex = 20;
            this.lWaitMinutes.Text = "Minutes after the windows-login, the daily login will be executed.";
            // 
            // lHeader
            // 
            this.lHeader.AutoSize = true;
            this.lHeader.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeader.Location = new System.Drawing.Point(42, 3);
            this.lHeader.Name = "lHeader";
            this.lHeader.Size = new System.Drawing.Size(193, 20);
            this.lHeader.TabIndex = 19;
            this.lHeader.Text = "Windows Task Scheduler";
            // 
            // btnInstallTask
            // 
            this.btnInstallTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstallTask.Image = global::ExaltAccountManager.Properties.Resources.ic_build_black_18dp;
            this.btnInstallTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInstallTask.Location = new System.Drawing.Point(13, 220);
            this.btnInstallTask.Name = "btnInstallTask";
            this.btnInstallTask.Size = new System.Drawing.Size(220, 24);
            this.btnInstallTask.TabIndex = 21;
            this.btnInstallTask.Text = "Install Daily Login Task";
            this.btnInstallTask.UseVisualStyleBackColor = true;
            this.btnInstallTask.Click += new System.EventHandler(this.btnInstallTask_Click);
            // 
            // tbWaitMinutes
            // 
            this.tbWaitMinutes.Location = new System.Drawing.Point(13, 44);
            this.tbWaitMinutes.Name = "tbWaitMinutes";
            this.tbWaitMinutes.Size = new System.Drawing.Size(46, 20);
            this.tbWaitMinutes.TabIndex = 22;
            this.tbWaitMinutes.Text = "0";
            this.tbWaitMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbResetTime
            // 
            this.tbResetTime.Location = new System.Drawing.Point(14, 88);
            this.tbResetTime.Name = "tbResetTime";
            this.tbResetTime.Size = new System.Drawing.Size(45, 20);
            this.tbResetTime.TabIndex = 24;
            this.tbResetTime.Text = "06:00";
            this.tbResetTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lTime
            // 
            this.lTime.Location = new System.Drawing.Point(63, 83);
            this.lTime.Name = "lTime";
            this.lTime.Size = new System.Drawing.Size(172, 36);
            this.lTime.TabIndex = 23;
            this.lTime.Text = "Time the rotmg-login will count to the next day.";
            // 
            // lIsInstalled
            // 
            this.lIsInstalled.BackColor = System.Drawing.SystemColors.Control;
            this.lIsInstalled.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lIsInstalled.Location = new System.Drawing.Point(-5, 117);
            this.lIsInstalled.Name = "lIsInstalled";
            this.lIsInstalled.Size = new System.Drawing.Size(259, 20);
            this.lIsInstalled.TabIndex = 25;
            this.lIsInstalled.Text = "Daily-login task is installed!";
            this.lIsInstalled.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lInfo
            // 
            this.lInfo.Font = new System.Drawing.Font("Century Schoolbook", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lInfo.Location = new System.Drawing.Point(14, 141);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(221, 75);
            this.lInfo.TabIndex = 26;
            this.lInfo.Text = "The task can be found in the Windows Task Scheduler and deleted there manuall or " +
    "by clicking the button below. If you move the folder of this tool, Update the \r\n" +
    "task for it to work again.";
            // 
            // btnUninstallTask
            // 
            this.btnUninstallTask.BackColor = System.Drawing.Color.Transparent;
            this.btnUninstallTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUninstallTask.ForeColor = System.Drawing.Color.Crimson;
            this.btnUninstallTask.Location = new System.Drawing.Point(137, 188);
            this.btnUninstallTask.Name = "btnUninstallTask";
            this.btnUninstallTask.Size = new System.Drawing.Size(96, 24);
            this.btnUninstallTask.TabIndex = 27;
            this.btnUninstallTask.Text = "Uninstall Task";
            this.btnUninstallTask.UseVisualStyleBackColor = false;
            this.btnUninstallTask.Visible = false;
            this.btnUninstallTask.Click += new System.EventHandler(this.btnUninstallTask_Click);
            // 
            // pbBack
            // 
            this.pbBack.AllowFocused = false;
            this.pbBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbBack.AutoSizeHeight = true;
            this.pbBack.BorderRadius = 12;
            this.pbBack.Image = global::ExaltAccountManager.Properties.Resources.ic_arrow_back_black_24dp;
            this.pbBack.IsCircle = true;
            this.pbBack.Location = new System.Drawing.Point(3, 2);
            this.pbBack.Name = "pbBack";
            this.pbBack.Size = new System.Drawing.Size(24, 24);
            this.pbBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbBack.TabIndex = 28;
            this.pbBack.TabStop = false;
            this.pbBack.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Circle;
            this.pbBack.Click += new System.EventHandler(this.pbBack_Click);
            this.pbBack.MouseEnter += new System.EventHandler(this.pbBack_MouseEnter);
            this.pbBack.MouseLeave += new System.EventHandler(this.pbBack_MouseLeave);
            // 
            // UIWindowsTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pbBack);
            this.Controls.Add(this.btnUninstallTask);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.lIsInstalled);
            this.Controls.Add(this.tbResetTime);
            this.Controls.Add(this.lTime);
            this.Controls.Add(this.tbWaitMinutes);
            this.Controls.Add(this.btnInstallTask);
            this.Controls.Add(this.lWaitMinutes);
            this.Controls.Add(this.lHeader);
            this.Controls.Add(this.lBack);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Name = "UIWindowsTask";
            this.Size = new System.Drawing.Size(248, 250);
            this.Load += new System.EventHandler(this.UIWindowsTask_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lBack;
        private System.Windows.Forms.Label lWaitMinutes;
        private System.Windows.Forms.Label lHeader;
        private System.Windows.Forms.Button btnInstallTask;
        private System.Windows.Forms.TextBox tbWaitMinutes;
        private System.Windows.Forms.TextBox tbResetTime;
        private System.Windows.Forms.Label lTime;
        private System.Windows.Forms.Label lIsInstalled;
        private System.Windows.Forms.Label lInfo;
        private System.Windows.Forms.Button btnUninstallTask;
        private Bunifu.UI.WinForms.BunifuPictureBox pbBack;
    }
}
