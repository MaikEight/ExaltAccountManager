
namespace EAM_Statistics
{
    partial class MaterialRadarChars
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
            this.radarChart = new MKRadarChart.RadarChart();
            this.pChart = new System.Windows.Forms.Panel();
            this.pMain.SuspendLayout();
            this.pChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // pMain
            // 
            this.pMain.BackColor = System.Drawing.Color.Transparent;
            this.pMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pMain.BorderRadius = 5;
            this.pMain.BorderThickness = 1;
            this.pMain.Controls.Add(this.pChart);
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
            this.pMain.Size = new System.Drawing.Size(406, 381);
            this.pMain.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.pMain.TabIndex = 0;
            // 
            // lTitle
            // 
            this.lTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(24, 0);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(350, 29);
            this.lTitle.TabIndex = 3;
            this.lTitle.Text = "Title ";
            this.lTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(10, 10);
            this.lHeadline.MaximumSize = new System.Drawing.Size(180, 0);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(49, 13);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Headline";
            // 
            // radarChart
            // 
            this.radarChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radarChart.BackColor = System.Drawing.Color.White;
            this.radarChart.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radarChart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radarChart.Location = new System.Drawing.Point(28, 0);
            this.radarChart.Name = "radarChart";
            this.radarChart.Size = new System.Drawing.Size(317, 342);
            this.radarChart.TabIndex = 4;
            // 
            // pChart
            // 
            this.pChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pChart.Controls.Add(this.lTitle);
            this.pChart.Controls.Add(this.radarChart);
            this.pChart.Location = new System.Drawing.Point(5, 25);
            this.pChart.Name = "pChart";
            this.pChart.Size = new System.Drawing.Size(396, 345);
            this.pChart.TabIndex = 5;
            // 
            // MaterialRadarChars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pMain);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MaterialRadarChars";
            this.Size = new System.Drawing.Size(406, 381);
            this.pMain.ResumeLayout(false);
            this.pMain.PerformLayout();
            this.pChart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuShadowPanel pMain;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Label lHeadline;
        private MKRadarChart.RadarChart radarChart;
        private System.Windows.Forms.Panel pChart;
    }
}
