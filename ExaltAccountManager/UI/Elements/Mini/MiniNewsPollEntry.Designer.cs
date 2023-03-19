namespace ExaltAccountManager.UI.Elements.Mini
{
    partial class MiniNewsPollEntry
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
            this.lChoice = new System.Windows.Forms.Label();
            this.pPercentage = new System.Windows.Forms.Panel();
            this.lResults = new System.Windows.Forms.Label();
            this.pbOwnChoice = new System.Windows.Forms.PictureBox();
            this.pPercentage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOwnChoice)).BeginInit();
            this.SuspendLayout();
            // 
            // lChoice
            // 
            this.lChoice.BackColor = System.Drawing.Color.Transparent;
            this.lChoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lChoice.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lChoice.Location = new System.Drawing.Point(0, 0);
            this.lChoice.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lChoice.Name = "lChoice";
            this.lChoice.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lChoice.Size = new System.Drawing.Size(314, 46);
            this.lChoice.TabIndex = 0;
            this.lChoice.Text = "label1";
            this.lChoice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lChoice.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.lChoice.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.lChoice.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            // 
            // pPercentage
            // 
            this.pPercentage.BackColor = System.Drawing.Color.Transparent;
            this.pPercentage.Controls.Add(this.pbOwnChoice);
            this.pPercentage.Controls.Add(this.lResults);
            this.pPercentage.Dock = System.Windows.Forms.DockStyle.Right;
            this.pPercentage.Location = new System.Drawing.Point(314, 0);
            this.pPercentage.Name = "pPercentage";
            this.pPercentage.Size = new System.Drawing.Size(110, 46);
            this.pPercentage.TabIndex = 1;
            this.pPercentage.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.pPercentage.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.pPercentage.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            // 
            // lResults
            // 
            this.lResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lResults.AutoSize = true;
            this.lResults.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lResults.Location = new System.Drawing.Point(44, 14);
            this.lResults.Name = "lResults";
            this.lResults.Size = new System.Drawing.Size(40, 17);
            this.lResults.TabIndex = 0;
            this.lResults.Text = "100%";
            this.lResults.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.lResults.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.lResults.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            // 
            // pbOwnChoice
            // 
            this.pbOwnChoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbOwnChoice.Image = global::ExaltAccountManager.Properties.Resources.Checkmark_black_28px;
            this.pbOwnChoice.Location = new System.Drawing.Point(9, 9);
            this.pbOwnChoice.Name = "pbOwnChoice";
            this.pbOwnChoice.Size = new System.Drawing.Size(28, 28);
            this.pbOwnChoice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbOwnChoice.TabIndex = 29;
            this.pbOwnChoice.TabStop = false;
            this.pbOwnChoice.Visible = false;
            // 
            // MiniNewsPollEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lChoice);
            this.Controls.Add(this.pPercentage);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MiniNewsPollEntry";
            this.Size = new System.Drawing.Size(424, 46);
            this.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MiniNewsPollEntry_Paint);
            this.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            this.pPercentage.ResumeLayout(false);
            this.pPercentage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOwnChoice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lChoice;
        private System.Windows.Forms.Panel pPercentage;
        private System.Windows.Forms.Label lResults;
        private System.Windows.Forms.PictureBox pbOwnChoice;
    }
}
