namespace ExaltAccountManager.UI.Elements.Mini
{
    partial class MiniAccountDataOrderEntry
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
            Bunifu.Framework.UI.BunifuElipse bunifuElipse;
            this.lEntryName = new System.Windows.Forms.Label();
            this.lValueExample = new System.Windows.Forms.Label();
            bunifuElipse = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.SuspendLayout();
            // 
            // bunifuElipse
            // 
            bunifuElipse.ElipseRadius = 9;
            bunifuElipse.TargetControl = this;
            // 
            // lEntryName
            // 
            this.lEntryName.AutoSize = true;
            this.lEntryName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lEntryName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEntryName.Location = new System.Drawing.Point(3, 1);
            this.lEntryName.Name = "lEntryName";
            this.lEntryName.Size = new System.Drawing.Size(91, 21);
            this.lEntryName.TabIndex = 44;
            this.lEntryName.Text = "EntryName";
            this.lEntryName.Click += new System.EventHandler(this.MiniAccountDataOrderEntry_Click);
            // 
            // lValueExample
            // 
            this.lValueExample.AutoSize = true;
            this.lValueExample.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lValueExample.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lValueExample.Location = new System.Drawing.Point(3, 22);
            this.lValueExample.Name = "lValueExample";
            this.lValueExample.Size = new System.Drawing.Size(78, 15);
            this.lValueExample.TabIndex = 45;
            this.lValueExample.Text = "ValueExample";
            this.lValueExample.Click += new System.EventHandler(this.MiniAccountDataOrderEntry_Click);
            // 
            // MiniAccountDataOrderEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lValueExample);
            this.Controls.Add(this.lEntryName);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.Name = "MiniAccountDataOrderEntry";
            this.Size = new System.Drawing.Size(154, 40);
            this.Click += new System.EventHandler(this.MiniAccountDataOrderEntry_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lEntryName;
        private System.Windows.Forms.Label lValueExample;
    }
}
