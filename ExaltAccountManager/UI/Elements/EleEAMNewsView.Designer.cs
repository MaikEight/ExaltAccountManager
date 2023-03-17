namespace ExaltAccountManager.UI.Elements
{
    partial class EleEAMNewsView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleEAMNewsView));
            this.lDate = new System.Windows.Forms.Label();
            this.lTitle = new System.Windows.Forms.Label();
            this.spacer = new Bunifu.UI.WinForms.BunifuSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pContent = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lDate
            // 
            this.lDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lDate.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDate.Location = new System.Drawing.Point(594, 5);
            this.lDate.Margin = new System.Windows.Forms.Padding(0);
            this.lDate.MaximumSize = new System.Drawing.Size(468, 0);
            this.lDate.MinimumSize = new System.Drawing.Size(76, 19);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(76, 19);
            this.lDate.TabIndex = 27;
            this.lDate.Text = "00.00.0000";
            this.lDate.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lDate.UseMnemonic = false;
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(0, 0);
            this.lTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lTitle.MaximumSize = new System.Drawing.Size(577, 0);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(44, 21);
            this.lTitle.TabIndex = 26;
            this.lTitle.Text = "Title";
            this.lTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lTitle.UseMnemonic = false;
            // 
            // spacer
            // 
            this.spacer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spacer.BackColor = System.Drawing.Color.Transparent;
            this.spacer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("spacer.BackgroundImage")));
            this.spacer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.spacer.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.spacer.LineColor = System.Drawing.Color.Silver;
            this.spacer.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.spacer.LineThickness = 1;
            this.spacer.Location = new System.Drawing.Point(0, 21);
            this.spacer.Margin = new System.Windows.Forms.Padding(0);
            this.spacer.Name = "spacer";
            this.spacer.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.spacer.Size = new System.Drawing.Size(667, 5);
            this.spacer.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lTitle);
            this.panel1.Controls.Add(this.spacer);
            this.panel1.Controls.Add(this.lDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(667, 32);
            this.panel1.TabIndex = 28;
            // 
            // pContent
            // 
            this.pContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pContent.Location = new System.Drawing.Point(5, 37);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(667, 100);
            this.pContent.TabIndex = 29;
            // 
            // EleEAMNewsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EleEAMNewsView";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(677, 146);
            this.SizeChanged += new System.EventHandler(this.EleEAMNewsView_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.UI.WinForms.BunifuSeparator spacer;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pContent;
    }
}
