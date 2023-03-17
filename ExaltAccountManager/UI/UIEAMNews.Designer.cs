namespace ExaltAccountManager.UI
{
    partial class UIEAMNews
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
            this.pNews = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pNews
            // 
            this.pNews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pNews.Location = new System.Drawing.Point(10, 10);
            this.pNews.Name = "pNews";
            this.pNews.Size = new System.Drawing.Size(657, 530);
            this.pNews.TabIndex = 1;
            // 
            // UIEAMNews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pNews);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIEAMNews";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(677, 550);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pNews;
    }
}
