
namespace EAM_Statistics
{
    partial class MaterialSimpelTextPanel
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
            this.pMain = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.lTitle = new System.Windows.Forms.Label();
            this.lHeadline = new System.Windows.Forms.Label();
            this.pMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pMain
            // 
            this.pMain.BackColor = System.Drawing.Color.Transparent;
            this.pMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pMain.BorderRadius = 5;
            this.pMain.BorderThickness = 1;
            this.pMain.Controls.Add(this.lTitle);
            this.pMain.Controls.Add(this.lHeadline);
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.pMain.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.pMain.Location = new System.Drawing.Point(0, 0);
            this.pMain.Margin = new System.Windows.Forms.Padding(4);
            this.pMain.Name = "pMain";
            this.pMain.PanelColor = System.Drawing.Color.White;
            this.pMain.PanelColor2 = System.Drawing.Color.White;
            this.pMain.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pMain.ShadowDept = 2;
            this.pMain.ShadowDepth = 2;
            this.pMain.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.pMain.ShadowTopLeftVisible = false;
            this.pMain.Size = new System.Drawing.Size(200, 100);
            this.pMain.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.pMain.TabIndex = 0;
            // 
            // lTitle
            // 
            this.lTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(10, 39);
            this.lTitle.MaximumSize = new System.Drawing.Size(180, 0);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(60, 30);
            this.lTitle.TabIndex = 1;
            this.lTitle.Text = "Title ";
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(10, 10);
            this.lHeadline.MaximumSize = new System.Drawing.Size(180, 0);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(49, 13);
            this.lHeadline.TabIndex = 0;
            this.lHeadline.Text = "Headline";
            // 
            // MaterialSimpelTextPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pMain);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MaterialSimpelTextPanel";
            this.Size = new System.Drawing.Size(200, 100);
            this.pMain.ResumeLayout(false);
            this.pMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel pMain;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Label lHeadline;
    }
}
