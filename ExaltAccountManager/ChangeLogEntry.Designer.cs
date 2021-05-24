namespace ExaltAccountManager
{
    partial class ChangeLogEntry
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
            this.lHeadline = new System.Windows.Forms.Label();
            this.pLine = new System.Windows.Forms.Panel();
            this.lEntry = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(10, 8);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(313, 16);
            this.lHeadline.TabIndex = 0;
            this.lHeadline.Text = "v1.4 - \"Security improvements and QoL update\"";
            // 
            // pLine
            // 
            this.pLine.BackColor = System.Drawing.Color.Black;
            this.pLine.Location = new System.Drawing.Point(42, 25);
            this.pLine.Name = "pLine";
            this.pLine.Size = new System.Drawing.Size(383, 1);
            this.pLine.TabIndex = 1;
            // 
            // lEntry
            // 
            this.lEntry.AutoSize = true;
            this.lEntry.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEntry.Location = new System.Drawing.Point(10, 40);
            this.lEntry.MaximumSize = new System.Drawing.Size(405, 0);
            this.lEntry.Name = "lEntry";
            this.lEntry.Size = new System.Drawing.Size(41, 15);
            this.lEntry.TabIndex = 2;
            this.lEntry.Text = "- Entry";
            this.lEntry.UseMnemonic = false;
            // 
            // ChangeLogEntry
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.lEntry);
            this.Controls.Add(this.pLine);
            this.Controls.Add(this.lHeadline);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ChangeLogEntry";
            this.Size = new System.Drawing.Size(425, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.Panel pLine;
        private System.Windows.Forms.Label lEntry;
    }
}
