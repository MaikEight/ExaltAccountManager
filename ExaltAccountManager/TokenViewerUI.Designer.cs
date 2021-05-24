
namespace ExaltAccountManager
{
    partial class TokenViewerUI
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
            this.pName = new System.Windows.Forms.Panel();
            this.lName = new System.Windows.Forms.Label();
            this.lMail = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.pMain = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.linkCharList = new System.Windows.Forms.LinkLabel();
            this.linkAccountVerify = new System.Windows.Forms.LinkLabel();
            this.lValidUntil = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lValidFor = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lCreationTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pAccessToken = new System.Windows.Forms.Panel();
            this.rtbAccessToken = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pName.SuspendLayout();
            this.pTop.SuspendLayout();
            this.pMain.SuspendLayout();
            this.pAccessToken.SuspendLayout();
            this.SuspendLayout();
            // 
            // pName
            // 
            this.pName.Controls.Add(this.lName);
            this.pName.Dock = System.Windows.Forms.DockStyle.Left;
            this.pName.Location = new System.Drawing.Point(0, 0);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(200, 30);
            this.pName.TabIndex = 0;
            this.pName.Click += new System.EventHandler(this.pTop_Click);
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(1, 5);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(113, 20);
            this.lName.TabIndex = 0;
            this.lName.Text = "AccountName";
            this.lName.Click += new System.EventHandler(this.pTop_Click);
            // 
            // lMail
            // 
            this.lMail.AutoSize = true;
            this.lMail.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMail.Location = new System.Drawing.Point(201, 5);
            this.lMail.Name = "lMail";
            this.lMail.Size = new System.Drawing.Size(60, 20);
            this.lMail.TabIndex = 1;
            this.lMail.Text = "E-Mail";
            this.lMail.Click += new System.EventHandler(this.pTop_Click);
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.pName);
            this.pTop.Controls.Add(this.lMail);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(425, 30);
            this.pTop.TabIndex = 2;
            this.pTop.Click += new System.EventHandler(this.pTop_Click);
            // 
            // pMain
            // 
            this.pMain.Controls.Add(this.label3);
            this.pMain.Controls.Add(this.label8);
            this.pMain.Controls.Add(this.linkCharList);
            this.pMain.Controls.Add(this.linkAccountVerify);
            this.pMain.Controls.Add(this.lValidUntil);
            this.pMain.Controls.Add(this.label7);
            this.pMain.Controls.Add(this.lValidFor);
            this.pMain.Controls.Add(this.label5);
            this.pMain.Controls.Add(this.lCreationTime);
            this.pMain.Controls.Add(this.label2);
            this.pMain.Controls.Add(this.pAccessToken);
            this.pMain.Controls.Add(this.label1);
            this.pMain.Location = new System.Drawing.Point(0, 30);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(425, 195);
            this.pMain.TabIndex = 3;
            this.pMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pMain_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Schoolbook", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(311, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "* = from creation time";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(277, 16);
            this.label8.TabIndex = 19;
            this.label8.Text = "Open webrequests in your webbrowser";
            // 
            // linkCharList
            // 
            this.linkCharList.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkCharList.AutoSize = true;
            this.linkCharList.ForeColor = System.Drawing.Color.Black;
            this.linkCharList.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkCharList.Location = new System.Drawing.Point(119, 173);
            this.linkCharList.MaximumSize = new System.Drawing.Size(160, 0);
            this.linkCharList.Name = "linkCharList";
            this.linkCharList.Size = new System.Drawing.Size(48, 15);
            this.linkCharList.TabIndex = 18;
            this.linkCharList.TabStop = true;
            this.linkCharList.Text = "char/list";
            this.linkCharList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCharList_LinkClicked);
            // 
            // linkAccountVerify
            // 
            this.linkAccountVerify.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkAccountVerify.AutoSize = true;
            this.linkAccountVerify.ForeColor = System.Drawing.Color.Black;
            this.linkAccountVerify.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkAccountVerify.Location = new System.Drawing.Point(14, 173);
            this.linkAccountVerify.MaximumSize = new System.Drawing.Size(160, 0);
            this.linkAccountVerify.Name = "linkAccountVerify";
            this.linkAccountVerify.Size = new System.Drawing.Size(80, 15);
            this.linkAccountVerify.TabIndex = 17;
            this.linkAccountVerify.TabStop = true;
            this.linkAccountVerify.Text = "account/verify";
            this.linkAccountVerify.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAccountVerify_LinkClicked);
            // 
            // lValidUntil
            // 
            this.lValidUntil.AutoSize = true;
            this.lValidUntil.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lValidUntil.Location = new System.Drawing.Point(308, 129);
            this.lValidUntil.Name = "lValidUntil";
            this.lValidUntil.Size = new System.Drawing.Size(108, 16);
            this.lValidUntil.TabIndex = 8;
            this.lValidUntil.Text = "01.01.2000 00:00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(294, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 16);
            this.label7.TabIndex = 7;
            this.label7.Text = "Valid until";
            // 
            // lValidFor
            // 
            this.lValidFor.AutoSize = true;
            this.lValidFor.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lValidFor.Location = new System.Drawing.Point(178, 129);
            this.lValidFor.Name = "lValidFor";
            this.lValidFor.Size = new System.Drawing.Size(53, 16);
            this.lValidFor.TabIndex = 6;
            this.lValidFor.Text = "15 days";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(164, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "Valid for*";
            // 
            // lCreationTime
            // 
            this.lCreationTime.AutoSize = true;
            this.lCreationTime.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCreationTime.Location = new System.Drawing.Point(20, 129);
            this.lCreationTime.Name = "lCreationTime";
            this.lCreationTime.Size = new System.Drawing.Size(108, 16);
            this.lCreationTime.TabIndex = 4;
            this.lCreationTime.Text = "01.01.2000 00:00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Creation Time";
            // 
            // pAccessToken
            // 
            this.pAccessToken.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pAccessToken.Controls.Add(this.rtbAccessToken);
            this.pAccessToken.Location = new System.Drawing.Point(17, 26);
            this.pAccessToken.Name = "pAccessToken";
            this.pAccessToken.Size = new System.Drawing.Size(400, 75);
            this.pAccessToken.TabIndex = 2;
            // 
            // rtbAccessToken
            // 
            this.rtbAccessToken.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbAccessToken.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbAccessToken.Location = new System.Drawing.Point(0, 0);
            this.rtbAccessToken.Name = "rtbAccessToken";
            this.rtbAccessToken.ReadOnly = true;
            this.rtbAccessToken.Size = new System.Drawing.Size(398, 73);
            this.rtbAccessToken.TabIndex = 1;
            this.rtbAccessToken.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current stored access-token";
            // 
            // TokenViewerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Margin = new System.Windows.Forms.Padding(2, 5, 0, 0);
            this.MaximumSize = new System.Drawing.Size(425, 225);
            this.MinimumSize = new System.Drawing.Size(425, 30);
            this.Name = "TokenViewerUI";
            this.Size = new System.Drawing.Size(425, 225);
            this.pName.ResumeLayout(false);
            this.pName.PerformLayout();
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pMain.ResumeLayout(false);
            this.pMain.PerformLayout();
            this.pAccessToken.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pName;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lMail;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.Label lValidUntil;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lValidFor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lCreationTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pAccessToken;
        private System.Windows.Forms.RichTextBox rtbAccessToken;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel linkCharList;
        private System.Windows.Forms.LinkLabel linkAccountVerify;
        private System.Windows.Forms.Label label3;
    }
}
