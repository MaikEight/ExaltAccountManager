
namespace EAM_Statistics
{
    partial class UIDashboard
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
            this.lAccountsHeadline = new System.Windows.Forms.Label();
            this.lAmountOfAccounts = new System.Windows.Forms.Label();
            this.lAmountOfChars = new System.Windows.Forms.Label();
            this.lCharactersHeadline = new System.Windows.Forms.Label();
            this.pAccount = new System.Windows.Forms.Panel();
            this.pCharacter = new System.Windows.Forms.Panel();
            this.pTotalFame = new System.Windows.Forms.Panel();
            this.lOverallTotalChange = new System.Windows.Forms.Label();
            this.lTotalFameHeadline = new System.Windows.Forms.Label();
            this.lTotalFame = new System.Windows.Forms.Label();
            this.pTotalAliveFame = new System.Windows.Forms.Panel();
            this.lTotalAliveChange = new System.Windows.Forms.Label();
            this.lTotalAliveFameHeadline = new System.Windows.Forms.Label();
            this.lTotalAliveFame = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lTotalLoginsChange = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lTotalLogins = new System.Windows.Forms.Label();
            this.materialPanel2 = new EAM_Statistics.MaterialPanel();
            this.materialPanel1 = new EAM_Statistics.MaterialPanel();
            this.topAccountLiveFame = new EAM_Statistics.MaterialTopAccount();
            this.topAccountFame = new EAM_Statistics.MaterialTopAccount();
            this.mtpTotalFame = new EAM_Statistics.MaterialPanel();
            this.mtpChars = new EAM_Statistics.MaterialPanel();
            this.mtpAccounts = new EAM_Statistics.MaterialPanel();
            this.mtpInfo = new EAM_Statistics.MaterialTextPanel();
            this.pAccount.SuspendLayout();
            this.pCharacter.SuspendLayout();
            this.pTotalFame.SuspendLayout();
            this.pTotalAliveFame.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lAccountsHeadline
            // 
            this.lAccountsHeadline.AutoSize = true;
            this.lAccountsHeadline.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAccountsHeadline.Location = new System.Drawing.Point(10, 11);
            this.lAccountsHeadline.Name = "lAccountsHeadline";
            this.lAccountsHeadline.Size = new System.Drawing.Size(49, 13);
            this.lAccountsHeadline.TabIndex = 2;
            this.lAccountsHeadline.Text = "Accounts";
            // 
            // lAmountOfAccounts
            // 
            this.lAmountOfAccounts.AutoSize = true;
            this.lAmountOfAccounts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAmountOfAccounts.Location = new System.Drawing.Point(9, 32);
            this.lAmountOfAccounts.Name = "lAmountOfAccounts";
            this.lAmountOfAccounts.Size = new System.Drawing.Size(155, 21);
            this.lAmountOfAccounts.TabIndex = 3;
            this.lAmountOfAccounts.Text = "{0} Accounts found";
            // 
            // lAmountOfChars
            // 
            this.lAmountOfChars.AutoSize = true;
            this.lAmountOfChars.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAmountOfChars.Location = new System.Drawing.Point(9, 32);
            this.lAmountOfChars.Name = "lAmountOfChars";
            this.lAmountOfChars.Size = new System.Drawing.Size(165, 21);
            this.lAmountOfChars.TabIndex = 6;
            this.lAmountOfChars.Text = "{0} Characters found";
            // 
            // lCharactersHeadline
            // 
            this.lCharactersHeadline.AutoSize = true;
            this.lCharactersHeadline.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCharactersHeadline.Location = new System.Drawing.Point(10, 11);
            this.lCharactersHeadline.Name = "lCharactersHeadline";
            this.lCharactersHeadline.Size = new System.Drawing.Size(54, 13);
            this.lCharactersHeadline.TabIndex = 5;
            this.lCharactersHeadline.Text = "Character";
            // 
            // pAccount
            // 
            this.pAccount.Controls.Add(this.lAccountsHeadline);
            this.pAccount.Controls.Add(this.lAmountOfAccounts);
            this.pAccount.Controls.Add(this.mtpAccounts);
            this.pAccount.Location = new System.Drawing.Point(210, 5);
            this.pAccount.Name = "pAccount";
            this.pAccount.Size = new System.Drawing.Size(200, 70);
            this.pAccount.TabIndex = 7;
            // 
            // pCharacter
            // 
            this.pCharacter.Controls.Add(this.lCharactersHeadline);
            this.pCharacter.Controls.Add(this.lAmountOfChars);
            this.pCharacter.Controls.Add(this.mtpChars);
            this.pCharacter.Location = new System.Drawing.Point(415, 5);
            this.pCharacter.Name = "pCharacter";
            this.pCharacter.Size = new System.Drawing.Size(200, 70);
            this.pCharacter.TabIndex = 8;
            // 
            // pTotalFame
            // 
            this.pTotalFame.Controls.Add(this.lOverallTotalChange);
            this.pTotalFame.Controls.Add(this.lTotalFameHeadline);
            this.pTotalFame.Controls.Add(this.lTotalFame);
            this.pTotalFame.Controls.Add(this.mtpTotalFame);
            this.pTotalFame.Location = new System.Drawing.Point(210, 80);
            this.pTotalFame.Name = "pTotalFame";
            this.pTotalFame.Size = new System.Drawing.Size(200, 83);
            this.pTotalFame.TabIndex = 9;
            // 
            // lOverallTotalChange
            // 
            this.lOverallTotalChange.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lOverallTotalChange.Location = new System.Drawing.Point(13, 57);
            this.lOverallTotalChange.Name = "lOverallTotalChange";
            this.lOverallTotalChange.Size = new System.Drawing.Size(110, 13);
            this.lOverallTotalChange.TabIndex = 14;
            this.lOverallTotalChange.Text = "{0} since last week";
            this.lOverallTotalChange.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lTotalFameHeadline
            // 
            this.lTotalFameHeadline.AutoSize = true;
            this.lTotalFameHeadline.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalFameHeadline.Location = new System.Drawing.Point(10, 11);
            this.lTotalFameHeadline.Name = "lTotalFameHeadline";
            this.lTotalFameHeadline.Size = new System.Drawing.Size(90, 13);
            this.lTotalFameHeadline.TabIndex = 2;
            this.lTotalFameHeadline.Text = "Overall total fame";
            // 
            // lTotalFame
            // 
            this.lTotalFame.AutoSize = true;
            this.lTotalFame.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalFame.Location = new System.Drawing.Point(9, 32);
            this.lTotalFame.Name = "lTotalFame";
            this.lTotalFame.Size = new System.Drawing.Size(114, 21);
            this.lTotalFame.TabIndex = 3;
            this.lTotalFame.Text = "{0} total fame";
            // 
            // pTotalAliveFame
            // 
            this.pTotalAliveFame.Controls.Add(this.lTotalAliveChange);
            this.pTotalAliveFame.Controls.Add(this.lTotalAliveFameHeadline);
            this.pTotalAliveFame.Controls.Add(this.lTotalAliveFame);
            this.pTotalAliveFame.Controls.Add(this.materialPanel1);
            this.pTotalAliveFame.Location = new System.Drawing.Point(415, 80);
            this.pTotalAliveFame.Name = "pTotalAliveFame";
            this.pTotalAliveFame.Size = new System.Drawing.Size(200, 83);
            this.pTotalAliveFame.TabIndex = 12;
            // 
            // lTotalAliveChange
            // 
            this.lTotalAliveChange.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalAliveChange.Location = new System.Drawing.Point(13, 57);
            this.lTotalAliveChange.Name = "lTotalAliveChange";
            this.lTotalAliveChange.Size = new System.Drawing.Size(110, 13);
            this.lTotalAliveChange.TabIndex = 15;
            this.lTotalAliveChange.Text = "{0} since last week";
            this.lTotalAliveChange.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lTotalAliveFameHeadline
            // 
            this.lTotalAliveFameHeadline.AutoSize = true;
            this.lTotalAliveFameHeadline.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalAliveFameHeadline.Location = new System.Drawing.Point(10, 11);
            this.lTotalAliveFameHeadline.Name = "lTotalAliveFameHeadline";
            this.lTotalAliveFameHeadline.Size = new System.Drawing.Size(80, 13);
            this.lTotalAliveFameHeadline.TabIndex = 2;
            this.lTotalAliveFameHeadline.Text = "Total alive fame";
            // 
            // lTotalAliveFame
            // 
            this.lTotalAliveFame.AutoSize = true;
            this.lTotalAliveFame.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalAliveFame.Location = new System.Drawing.Point(9, 32);
            this.lTotalAliveFame.Name = "lTotalAliveFame";
            this.lTotalAliveFame.Size = new System.Drawing.Size(115, 21);
            this.lTotalAliveFame.TabIndex = 3;
            this.lTotalAliveFame.Text = "{0} alive fame";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lTotalLoginsChange);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lTotalLogins);
            this.panel1.Controls.Add(this.materialPanel2);
            this.panel1.Location = new System.Drawing.Point(210, 168);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 83);
            this.panel1.TabIndex = 13;
            // 
            // lTotalLoginsChange
            // 
            this.lTotalLoginsChange.AutoSize = true;
            this.lTotalLoginsChange.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalLoginsChange.Location = new System.Drawing.Point(8, 57);
            this.lTotalLoginsChange.Name = "lTotalLoginsChange";
            this.lTotalLoginsChange.Size = new System.Drawing.Size(98, 13);
            this.lTotalLoginsChange.TabIndex = 15;
            this.lTotalLoginsChange.Text = "+{0} since last week";
            this.lTotalLoginsChange.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total logins";
            // 
            // lTotalLogins
            // 
            this.lTotalLogins.AutoSize = true;
            this.lTotalLogins.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTotalLogins.Location = new System.Drawing.Point(9, 32);
            this.lTotalLogins.Name = "lTotalLogins";
            this.lTotalLogins.Size = new System.Drawing.Size(148, 21);
            this.lTotalLogins.TabIndex = 3;
            this.lTotalLogins.Text = "{0} logins via EAM";
            // 
            // materialPanel2
            // 
            this.materialPanel2.BackColor = System.Drawing.Color.Transparent;
            this.materialPanel2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.materialPanel2.Location = new System.Drawing.Point(0, 0);
            this.materialPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.materialPanel2.Name = "materialPanel2";
            this.materialPanel2.Size = new System.Drawing.Size(200, 83);
            this.materialPanel2.TabIndex = 1;
            // 
            // materialPanel1
            // 
            this.materialPanel1.BackColor = System.Drawing.Color.Transparent;
            this.materialPanel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.materialPanel1.Location = new System.Drawing.Point(0, 0);
            this.materialPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.materialPanel1.Name = "materialPanel1";
            this.materialPanel1.Size = new System.Drawing.Size(200, 83);
            this.materialPanel1.TabIndex = 1;
            // 
            // topAccountLiveFame
            // 
            this.topAccountLiveFame.BackColor = System.Drawing.Color.Transparent;
            this.topAccountLiveFame.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.topAccountLiveFame.Location = new System.Drawing.Point(5, 155);
            this.topAccountLiveFame.Margin = new System.Windows.Forms.Padding(2);
            this.topAccountLiveFame.MinimumSize = new System.Drawing.Size(200, 175);
            this.topAccountLiveFame.Name = "topAccountLiveFame";
            this.topAccountLiveFame.Size = new System.Drawing.Size(200, 175);
            this.topAccountLiveFame.TabIndex = 11;
            // 
            // topAccountFame
            // 
            this.topAccountFame.BackColor = System.Drawing.Color.Transparent;
            this.topAccountFame.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.topAccountFame.Location = new System.Drawing.Point(5, 334);
            this.topAccountFame.Margin = new System.Windows.Forms.Padding(2);
            this.topAccountFame.MinimumSize = new System.Drawing.Size(200, 175);
            this.topAccountFame.Name = "topAccountFame";
            this.topAccountFame.Size = new System.Drawing.Size(200, 175);
            this.topAccountFame.TabIndex = 10;
            // 
            // mtpTotalFame
            // 
            this.mtpTotalFame.BackColor = System.Drawing.Color.Transparent;
            this.mtpTotalFame.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.mtpTotalFame.Location = new System.Drawing.Point(0, 0);
            this.mtpTotalFame.Margin = new System.Windows.Forms.Padding(2);
            this.mtpTotalFame.Name = "mtpTotalFame";
            this.mtpTotalFame.Size = new System.Drawing.Size(200, 83);
            this.mtpTotalFame.TabIndex = 1;
            // 
            // mtpChars
            // 
            this.mtpChars.BackColor = System.Drawing.Color.Transparent;
            this.mtpChars.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.mtpChars.Location = new System.Drawing.Point(0, 0);
            this.mtpChars.Margin = new System.Windows.Forms.Padding(2);
            this.mtpChars.Name = "mtpChars";
            this.mtpChars.Size = new System.Drawing.Size(200, 70);
            this.mtpChars.TabIndex = 4;
            // 
            // mtpAccounts
            // 
            this.mtpAccounts.BackColor = System.Drawing.Color.Transparent;
            this.mtpAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.mtpAccounts.Location = new System.Drawing.Point(0, 0);
            this.mtpAccounts.Margin = new System.Windows.Forms.Padding(2);
            this.mtpAccounts.Name = "mtpAccounts";
            this.mtpAccounts.Size = new System.Drawing.Size(200, 70);
            this.mtpAccounts.TabIndex = 1;
            // 
            // mtpInfo
            // 
            this.mtpInfo.BackColor = System.Drawing.Color.Transparent;
            this.mtpInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.mtpInfo.Location = new System.Drawing.Point(5, 5);
            this.mtpInfo.Margin = new System.Windows.Forms.Padding(2);
            this.mtpInfo.Name = "mtpInfo";
            this.mtpInfo.Size = new System.Drawing.Size(200, 146);
            this.mtpInfo.TabIndex = 0;
            // 
            // UIDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pTotalAliveFame);
            this.Controls.Add(this.topAccountLiveFame);
            this.Controls.Add(this.topAccountFame);
            this.Controls.Add(this.pTotalFame);
            this.Controls.Add(this.pCharacter);
            this.Controls.Add(this.pAccount);
            this.Controls.Add(this.mtpInfo);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIDashboard";
            this.Size = new System.Drawing.Size(620, 600);
            this.pAccount.ResumeLayout(false);
            this.pAccount.PerformLayout();
            this.pCharacter.ResumeLayout(false);
            this.pCharacter.PerformLayout();
            this.pTotalFame.ResumeLayout(false);
            this.pTotalFame.PerformLayout();
            this.pTotalAliveFame.ResumeLayout(false);
            this.pTotalAliveFame.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialTextPanel mtpInfo;
        private MaterialPanel mtpAccounts;
        private System.Windows.Forms.Label lAccountsHeadline;
        private System.Windows.Forms.Label lAmountOfAccounts;
        private MaterialPanel mtpChars;
        private System.Windows.Forms.Label lAmountOfChars;
        private System.Windows.Forms.Label lCharactersHeadline;
        private System.Windows.Forms.Panel pAccount;
        private System.Windows.Forms.Panel pCharacter;
        private System.Windows.Forms.Panel pTotalFame;
        private System.Windows.Forms.Label lTotalFameHeadline;
        private System.Windows.Forms.Label lTotalFame;
        private MaterialPanel mtpTotalFame;
        private MaterialTopAccount topAccountFame;
        private MaterialTopAccount topAccountLiveFame;
        private System.Windows.Forms.Panel pTotalAliveFame;
        private System.Windows.Forms.Label lTotalAliveFameHeadline;
        private System.Windows.Forms.Label lTotalAliveFame;
        private MaterialPanel materialPanel1;
        private System.Windows.Forms.Label lOverallTotalChange;
        private System.Windows.Forms.Label lTotalAliveChange;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lTotalLoginsChange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lTotalLogins;
        private MaterialPanel materialPanel2;
    }
}
