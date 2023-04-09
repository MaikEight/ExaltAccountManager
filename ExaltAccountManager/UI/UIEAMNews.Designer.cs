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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIEAMNews));
            this.pNews = new System.Windows.Forms.Panel();
            this.scrollbar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.pContent = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pNews
            // 
            this.pNews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pNews.Location = new System.Drawing.Point(0, 0);
            this.pNews.Name = "pNews";
            this.pNews.Size = new System.Drawing.Size(649, 530);
            this.pNews.TabIndex = 1;
            // 
            // scrollbar
            // 
            this.scrollbar.AllowCursorChanges = true;
            this.scrollbar.AllowHomeEndKeysDetection = false;
            this.scrollbar.AllowIncrementalClickMoves = true;
            this.scrollbar.AllowMouseDownEffects = true;
            this.scrollbar.AllowMouseHoverEffects = true;
            this.scrollbar.AllowScrollingAnimations = true;
            this.scrollbar.AllowScrollKeysDetection = true;
            this.scrollbar.AllowScrollOptionsMenu = true;
            this.scrollbar.AllowShrinkingOnFocusLost = false;
            this.scrollbar.BackgroundColor = System.Drawing.Color.Silver;
            this.scrollbar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("scrollbar.BackgroundImage")));
            this.scrollbar.BindingContainer = this;
            this.scrollbar.BorderColor = System.Drawing.Color.Silver;
            this.scrollbar.BorderRadius = 0;
            this.scrollbar.BorderThickness = 2;
            this.scrollbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollbar.DurationBeforeShrink = 2000;
            this.scrollbar.LargeChange = 10;
            this.scrollbar.Location = new System.Drawing.Point(669, 0);
            this.scrollbar.Margin = new System.Windows.Forms.Padding(7);
            this.scrollbar.Maximum = 100;
            this.scrollbar.MaximumSize = new System.Drawing.Size(8, 0);
            this.scrollbar.Minimum = 0;
            this.scrollbar.MinimumSize = new System.Drawing.Size(8, 0);
            this.scrollbar.MinimumThumbLength = 18;
            this.scrollbar.Name = "scrollbar";
            this.scrollbar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.scrollbar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.scrollbar.ScrollBarColor = System.Drawing.Color.Silver;
            this.scrollbar.ShrinkSizeLimit = 3;
            this.scrollbar.Size = new System.Drawing.Size(8, 550);
            this.scrollbar.SmallChange = 1;
            this.scrollbar.TabIndex = 11;
            this.scrollbar.ThumbColor = System.Drawing.Color.Gray;
            this.scrollbar.ThumbLength = 54;
            this.scrollbar.ThumbMargin = 1;
            this.scrollbar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Proportional;
            this.scrollbar.Value = 0;
            // 
            // pContent
            // 
            this.pContent.Controls.Add(this.pNews);
            this.pContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContent.Location = new System.Drawing.Point(10, 10);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(649, 530);
            this.pContent.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(659, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 550);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(649, 10);
            this.panel2.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 540);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(649, 10);
            this.panel3.TabIndex = 15;
            // 
            // UIEAMNews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.scrollbar);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIEAMNews";
            this.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.Size = new System.Drawing.Size(677, 550);
            this.pContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pNews;
        private Bunifu.UI.WinForms.BunifuVScrollBar scrollbar;
        private System.Windows.Forms.Panel pContent;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
    }
}
