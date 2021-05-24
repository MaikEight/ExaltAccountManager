namespace ExaltAccountManager
{
    partial class AccountUIHeader
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lEmail = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lAccountName = new System.Windows.Forms.Label();
            this.pActions = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pDaily = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pDrag = new System.Windows.Forms.Panel();
            this.pbFilterList = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pActions.SuspendLayout();
            this.pDaily.SuspendLayout();
            this.pDrag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFilterList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lEmail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(184, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(191, 36);
            this.panel2.TabIndex = 5;
            // 
            // lEmail
            // 
            this.lEmail.AutoSize = true;
            this.lEmail.Font = new System.Drawing.Font("Century Schoolbook", 11.25F);
            this.lEmail.Location = new System.Drawing.Point(6, 9);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new System.Drawing.Size(50, 18);
            this.lEmail.TabIndex = 1;
            this.lEmail.Text = "Email";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lAccountName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(34, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 36);
            this.panel1.TabIndex = 4;
            // 
            // lAccountName
            // 
            this.lAccountName.AutoSize = true;
            this.lAccountName.Font = new System.Drawing.Font("Century Schoolbook", 11.25F);
            this.lAccountName.Location = new System.Drawing.Point(3, 9);
            this.lAccountName.Name = "lAccountName";
            this.lAccountName.Size = new System.Drawing.Size(110, 18);
            this.lAccountName.TabIndex = 0;
            this.lAccountName.Text = "Account Name";
            // 
            // pActions
            // 
            this.pActions.Controls.Add(this.label2);
            this.pActions.Dock = System.Windows.Forms.DockStyle.Left;
            this.pActions.Location = new System.Drawing.Point(375, 0);
            this.pActions.Name = "pActions";
            this.pActions.Size = new System.Drawing.Size(86, 36);
            this.pActions.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Schoolbook", 11.25F);
            this.label2.Location = new System.Drawing.Point(22, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Actions";
            // 
            // pDaily
            // 
            this.pDaily.Controls.Add(this.label1);
            this.pDaily.Dock = System.Windows.Forms.DockStyle.Right;
            this.pDaily.Location = new System.Drawing.Point(502, 0);
            this.pDaily.Name = "pDaily";
            this.pDaily.Size = new System.Drawing.Size(48, 36);
            this.pDaily.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 11.25F);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 36);
            this.label1.TabIndex = 7;
            this.label1.Text = "Daily \r\nLogin";
            // 
            // pDrag
            // 
            this.pDrag.Controls.Add(this.pbFilterList);
            this.pDrag.Dock = System.Windows.Forms.DockStyle.Left;
            this.pDrag.Location = new System.Drawing.Point(0, 0);
            this.pDrag.Name = "pDrag";
            this.pDrag.Size = new System.Drawing.Size(34, 36);
            this.pDrag.TabIndex = 13;
            // 
            // pbFilterList
            // 
            this.pbFilterList.Image = global::ExaltAccountManager.Properties.Resources.ic_format_line_spacing_black_36dp;
            this.pbFilterList.Location = new System.Drawing.Point(2, 0);
            this.pbFilterList.Name = "pbFilterList";
            this.pbFilterList.Size = new System.Drawing.Size(28, 36);
            this.pbFilterList.TabIndex = 0;
            this.pbFilterList.TabStop = false;
            // 
            // AccountUIHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pDaily);
            this.Controls.Add(this.pActions);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pDrag);
            this.Name = "AccountUIHeader";
            this.Size = new System.Drawing.Size(550, 36);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pActions.ResumeLayout(false);
            this.pActions.PerformLayout();
            this.pDaily.ResumeLayout(false);
            this.pDaily.PerformLayout();
            this.pDrag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbFilterList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lEmail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lAccountName;
        private System.Windows.Forms.Panel pActions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pDaily;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pDrag;
        private System.Windows.Forms.PictureBox pbFilterList;
    }
}
