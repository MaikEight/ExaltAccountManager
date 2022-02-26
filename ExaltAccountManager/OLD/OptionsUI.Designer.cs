namespace ExaltAccountManager
{
    partial class OptionsUI
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
            this.pOptionen = new System.Windows.Forms.Panel();
            this.lOptionen = new System.Windows.Forms.Label();
            this.pAddAccount = new System.Windows.Forms.Panel();
            this.lAdd = new System.Windows.Forms.Label();
            this.pDailyLogins = new System.Windows.Forms.Panel();
            this.lDaily = new System.Windows.Forms.Label();
            this.pLog = new System.Windows.Forms.Panel();
            this.lLog = new System.Windows.Forms.Label();
            this.timerLogNews = new System.Windows.Forms.Timer(this.components);
            this.pMore = new System.Windows.Forms.Panel();
            this.lMore = new System.Windows.Forms.Label();
            this.timerLogLock = new System.Windows.Forms.Timer(this.components);
            this.pHelp = new System.Windows.Forms.Panel();
            this.lHelp = new System.Windows.Forms.Label();
            this.pOptionen.SuspendLayout();
            this.pAddAccount.SuspendLayout();
            this.pDailyLogins.SuspendLayout();
            this.pLog.SuspendLayout();
            this.pMore.SuspendLayout();
            this.pHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // pOptionen
            // 
            this.pOptionen.Controls.Add(this.lOptionen);
            this.pOptionen.Dock = System.Windows.Forms.DockStyle.Left;
            this.pOptionen.Location = new System.Drawing.Point(0, 0);
            this.pOptionen.Name = "pOptionen";
            this.pOptionen.Size = new System.Drawing.Size(90, 24);
            this.pOptionen.TabIndex = 0;
            this.pOptionen.Click += new System.EventHandler(this.ShowOptions);
            this.pOptionen.MouseEnter += new System.EventHandler(this.options_MouseEnter);
            this.pOptionen.MouseLeave += new System.EventHandler(this.options_MouseLeave);
            // 
            // lOptionen
            // 
            this.lOptionen.AutoSize = true;
            this.lOptionen.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lOptionen.Location = new System.Drawing.Point(13, 3);
            this.lOptionen.Name = "lOptionen";
            this.lOptionen.Size = new System.Drawing.Size(64, 18);
            this.lOptionen.TabIndex = 1;
            this.lOptionen.Text = "Options";
            this.lOptionen.Click += new System.EventHandler(this.ShowOptions);
            this.lOptionen.MouseEnter += new System.EventHandler(this.options_MouseEnter);
            this.lOptionen.MouseLeave += new System.EventHandler(this.options_MouseLeave);
            // 
            // pAddAccount
            // 
            this.pAddAccount.Controls.Add(this.lAdd);
            this.pAddAccount.Dock = System.Windows.Forms.DockStyle.Left;
            this.pAddAccount.Location = new System.Drawing.Point(90, 0);
            this.pAddAccount.Name = "pAddAccount";
            this.pAddAccount.Size = new System.Drawing.Size(125, 24);
            this.pAddAccount.TabIndex = 1;
            this.pAddAccount.Click += new System.EventHandler(this.AddAccount);
            this.pAddAccount.MouseEnter += new System.EventHandler(this.addAccount_MouseEnter);
            this.pAddAccount.MouseLeave += new System.EventHandler(this.addAccount_MouseLeave);
            // 
            // lAdd
            // 
            this.lAdd.AutoSize = true;
            this.lAdd.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAdd.Location = new System.Drawing.Point(13, 3);
            this.lAdd.Name = "lAdd";
            this.lAdd.Size = new System.Drawing.Size(98, 18);
            this.lAdd.TabIndex = 1;
            this.lAdd.Text = "Add Account";
            this.lAdd.Click += new System.EventHandler(this.AddAccount);
            this.lAdd.MouseEnter += new System.EventHandler(this.addAccount_MouseEnter);
            this.lAdd.MouseLeave += new System.EventHandler(this.addAccount_MouseLeave);
            // 
            // pDailyLogins
            // 
            this.pDailyLogins.Controls.Add(this.lDaily);
            this.pDailyLogins.Dock = System.Windows.Forms.DockStyle.Left;
            this.pDailyLogins.Location = new System.Drawing.Point(215, 0);
            this.pDailyLogins.Name = "pDailyLogins";
            this.pDailyLogins.Size = new System.Drawing.Size(115, 24);
            this.pDailyLogins.TabIndex = 2;
            this.pDailyLogins.Click += new System.EventHandler(this.lDaily_Click);
            this.pDailyLogins.MouseEnter += new System.EventHandler(this.lDaily_MouseEnter);
            this.pDailyLogins.MouseLeave += new System.EventHandler(this.lDaily_MouseLeave);
            // 
            // lDaily
            // 
            this.lDaily.AutoSize = true;
            this.lDaily.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDaily.Location = new System.Drawing.Point(9, 3);
            this.lDaily.Name = "lDaily";
            this.lDaily.Size = new System.Drawing.Size(97, 18);
            this.lDaily.TabIndex = 1;
            this.lDaily.Text = "Daily Logins";
            this.lDaily.Click += new System.EventHandler(this.lDaily_Click);
            this.lDaily.MouseEnter += new System.EventHandler(this.lDaily_MouseEnter);
            this.lDaily.MouseLeave += new System.EventHandler(this.lDaily_MouseLeave);
            // 
            // pLog
            // 
            this.pLog.Controls.Add(this.lLog);
            this.pLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLog.Location = new System.Drawing.Point(330, 0);
            this.pLog.Name = "pLog";
            this.pLog.Size = new System.Drawing.Size(61, 24);
            this.pLog.TabIndex = 3;
            this.pLog.Click += new System.EventHandler(this.lLog_Click);
            this.pLog.MouseEnter += new System.EventHandler(this.lLog_MouseEnter);
            this.pLog.MouseLeave += new System.EventHandler(this.lLog_MouseLeave);
            // 
            // lLog
            // 
            this.lLog.AutoSize = true;
            this.lLog.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLog.Location = new System.Drawing.Point(10, 3);
            this.lLog.Name = "lLog";
            this.lLog.Size = new System.Drawing.Size(41, 18);
            this.lLog.TabIndex = 1;
            this.lLog.Text = "Logs";
            this.lLog.UseMnemonic = false;
            this.lLog.Click += new System.EventHandler(this.lLog_Click);
            this.lLog.MouseEnter += new System.EventHandler(this.lLog_MouseEnter);
            this.lLog.MouseLeave += new System.EventHandler(this.lLog_MouseLeave);
            // 
            // timerLogNews
            // 
            this.timerLogNews.Interval = 1000;
            this.timerLogNews.Tick += new System.EventHandler(this.timerLogNews_Tick);
            // 
            // pMore
            // 
            this.pMore.Controls.Add(this.lMore);
            this.pMore.Dock = System.Windows.Forms.DockStyle.Right;
            this.pMore.Location = new System.Drawing.Point(481, 0);
            this.pMore.Name = "pMore";
            this.pMore.Size = new System.Drawing.Size(69, 24);
            this.pMore.TabIndex = 4;
            this.pMore.Click += new System.EventHandler(this.lMore_Click);
            this.pMore.MouseEnter += new System.EventHandler(this.lMore_MouseEnter);
            this.pMore.MouseLeave += new System.EventHandler(this.lMore_MouseLeave);
            // 
            // lMore
            // 
            this.lMore.AutoSize = true;
            this.lMore.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMore.Location = new System.Drawing.Point(6, 3);
            this.lMore.Name = "lMore";
            this.lMore.Size = new System.Drawing.Size(57, 18);
            this.lMore.TabIndex = 1;
            this.lMore.Text = "More...";
            this.lMore.UseMnemonic = false;
            this.lMore.Click += new System.EventHandler(this.lMore_Click);
            this.lMore.MouseEnter += new System.EventHandler(this.lMore_MouseEnter);
            this.lMore.MouseLeave += new System.EventHandler(this.lMore_MouseLeave);
            // 
            // timerLogLock
            // 
            this.timerLogLock.Interval = 1500;
            this.timerLogLock.Tick += new System.EventHandler(this.timerLogLock_Tick);
            // 
            // pHelp
            // 
            this.pHelp.Controls.Add(this.lHelp);
            this.pHelp.Dock = System.Windows.Forms.DockStyle.Left;
            this.pHelp.Location = new System.Drawing.Point(391, 0);
            this.pHelp.Name = "pHelp";
            this.pHelp.Size = new System.Drawing.Size(61, 24);
            this.pHelp.TabIndex = 5;
            // 
            // lHelp
            // 
            this.lHelp.AutoSize = true;
            this.lHelp.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHelp.Location = new System.Drawing.Point(10, 3);
            this.lHelp.Name = "lHelp";
            this.lHelp.Size = new System.Drawing.Size(43, 18);
            this.lHelp.TabIndex = 1;
            this.lHelp.Text = "Help";
            this.lHelp.UseMnemonic = false;
            this.lHelp.Click += new System.EventHandler(this.lHelp_Click);
            this.lHelp.MouseEnter += new System.EventHandler(this.lHelp_MouseEnter);
            this.lHelp.MouseLeave += new System.EventHandler(this.lHelp_MouseLeave);
            // 
            // OptionsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.pHelp);
            this.Controls.Add(this.pMore);
            this.Controls.Add(this.pLog);
            this.Controls.Add(this.pDailyLogins);
            this.Controls.Add(this.pAddAccount);
            this.Controls.Add(this.pOptionen);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Name = "OptionsUI";
            this.Size = new System.Drawing.Size(550, 24);
            this.pOptionen.ResumeLayout(false);
            this.pOptionen.PerformLayout();
            this.pAddAccount.ResumeLayout(false);
            this.pAddAccount.PerformLayout();
            this.pDailyLogins.ResumeLayout(false);
            this.pDailyLogins.PerformLayout();
            this.pLog.ResumeLayout(false);
            this.pLog.PerformLayout();
            this.pMore.ResumeLayout(false);
            this.pMore.PerformLayout();
            this.pHelp.ResumeLayout(false);
            this.pHelp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pOptionen;
        private System.Windows.Forms.Label lOptionen;
        private System.Windows.Forms.Panel pAddAccount;
        private System.Windows.Forms.Label lAdd;
        private System.Windows.Forms.Panel pDailyLogins;
        private System.Windows.Forms.Label lDaily;
        private System.Windows.Forms.Panel pLog;
        private System.Windows.Forms.Label lLog;
        private System.Windows.Forms.Timer timerLogNews;
        private System.Windows.Forms.Panel pMore;
        private System.Windows.Forms.Label lMore;
        private System.Windows.Forms.Timer timerLogLock;
        private System.Windows.Forms.Panel pHelp;
        private System.Windows.Forms.Label lHelp;
    }
}
