
namespace EAM_Statistics
{
    partial class TopAccountListEntry
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
            this.lName = new System.Windows.Forms.Label();
            this.lValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.BackColor = System.Drawing.Color.Transparent;
            this.lName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lName.Font = new System.Drawing.Font("Segoe UI Semibold", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(0, 0);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(56, 23);
            this.lName.TabIndex = 4;
            this.lName.Text = "Name";
            this.lName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lName.MouseEnter += new System.EventHandler(this.TopAccountListEntry_MouseEnter);
            this.lName.MouseLeave += new System.EventHandler(this.TopAccountListEntry_MouseLeave);
            // 
            // lValue
            // 
            this.lValue.AutoSize = true;
            this.lValue.BackColor = System.Drawing.Color.Transparent;
            this.lValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lValue.Font = new System.Drawing.Font("Segoe UI Semibold", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lValue.Location = new System.Drawing.Point(147, 0);
            this.lValue.Name = "lValue";
            this.lValue.Size = new System.Drawing.Size(73, 23);
            this.lValue.TabIndex = 5;
            this.lValue.Text = "9999999";
            this.lValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lValue.MouseEnter += new System.EventHandler(this.TopAccountListEntry_MouseEnter);
            this.lValue.MouseLeave += new System.EventHandler(this.TopAccountListEntry_MouseLeave);
            // 
            // TopAccountListEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lValue);
            this.Controls.Add(this.lName);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TopAccountListEntry";
            this.Size = new System.Drawing.Size(220, 25);
            this.MouseEnter += new System.EventHandler(this.TopAccountListEntry_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.TopAccountListEntry_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lValue;
    }
}
