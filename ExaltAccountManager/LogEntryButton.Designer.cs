
namespace ExaltAccountManager
{
    partial class LogEntryButton
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
            this.lEntries = new System.Windows.Forms.Label();
            this.btnShowMore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lEntries
            // 
            this.lEntries.AutoSize = true;
            this.lEntries.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEntries.Location = new System.Drawing.Point(20, 17);
            this.lEntries.Name = "lEntries";
            this.lEntries.Size = new System.Drawing.Size(185, 16);
            this.lEntries.TabIndex = 1;
            this.lEntries.Text = "{0} / {1} Log-Entries shown";
            // 
            // btnShowMore
            // 
            this.btnShowMore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnShowMore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnShowMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMore.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowMore.Location = new System.Drawing.Point(291, 9);
            this.btnShowMore.Name = "btnShowMore";
            this.btnShowMore.Size = new System.Drawing.Size(200, 31);
            this.btnShowMore.TabIndex = 2;
            this.btnShowMore.Text = "Load more entries";
            this.btnShowMore.UseVisualStyleBackColor = true;
            this.btnShowMore.Click += new System.EventHandler(this.btnShowMore_Click);
            // 
            // LogEntryButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lEntries);
            this.Controls.Add(this.btnShowMore);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LogEntryButton";
            this.Size = new System.Drawing.Size(781, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lEntries;
        private System.Windows.Forms.Button btnShowMore;
    }
}
