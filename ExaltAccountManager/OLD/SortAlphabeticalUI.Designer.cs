
namespace ExaltAccountManager
{
    partial class SortAlphabeticalUI
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
            this.pbZA = new System.Windows.Forms.PictureBox();
            this.pbAZ = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbZA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAZ)).BeginInit();
            this.SuspendLayout();
            // 
            // pbZA
            // 
            this.pbZA.Image = global::ExaltAccountManager.Properties.Resources.alphabetical_sorting_za_24px;
            this.pbZA.Location = new System.Drawing.Point(1, 26);
            this.pbZA.Name = "pbZA";
            this.pbZA.Size = new System.Drawing.Size(24, 24);
            this.pbZA.TabIndex = 1;
            this.pbZA.TabStop = false;
            this.pbZA.Click += new System.EventHandler(this.pbZA_Click);
            this.pbZA.MouseEnter += new System.EventHandler(this.pb_MouseEnter);
            this.pbZA.MouseLeave += new System.EventHandler(this.pb_MouseLeave);
            // 
            // pbAZ
            // 
            this.pbAZ.Image = global::ExaltAccountManager.Properties.Resources.alphabetical_sorting_az_24px;
            this.pbAZ.Location = new System.Drawing.Point(1, 1);
            this.pbAZ.Name = "pbAZ";
            this.pbAZ.Size = new System.Drawing.Size(24, 24);
            this.pbAZ.TabIndex = 0;
            this.pbAZ.TabStop = false;
            this.pbAZ.Click += new System.EventHandler(this.pbAZ_Click);
            this.pbAZ.MouseEnter += new System.EventHandler(this.pb_MouseEnter);
            this.pbAZ.MouseLeave += new System.EventHandler(this.pb_MouseLeave);
            // 
            // SortAlphabeticalUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pbAZ);
            this.Controls.Add(this.pbZA);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.Name = "SortAlphabeticalUI";
            this.Size = new System.Drawing.Size(26, 51);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SortAlphabeticalUI_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pbZA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAZ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAZ;
        private System.Windows.Forms.PictureBox pbZA;
    }
}
