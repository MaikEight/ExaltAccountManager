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
            this.radioOwnChoice = new Bunifu.UI.WinForms.BunifuRadioButton();
            this.pPercentage.SuspendLayout();
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
            this.lChoice.Size = new System.Drawing.Size(356, 45);
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
            this.pPercentage.Controls.Add(this.radioOwnChoice);
            this.pPercentage.Controls.Add(this.lResults);
            this.pPercentage.Dock = System.Windows.Forms.DockStyle.Right;
            this.pPercentage.Location = new System.Drawing.Point(356, 0);
            this.pPercentage.Name = "pPercentage";
            this.pPercentage.Size = new System.Drawing.Size(68, 45);
            this.pPercentage.TabIndex = 1;
            this.pPercentage.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.pPercentage.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.pPercentage.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            // 
            // lResults
            // 
            this.lResults.AutoSize = true;
            this.lResults.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lResults.Location = new System.Drawing.Point(2, 14);
            this.lResults.Name = "lResults";
            this.lResults.Size = new System.Drawing.Size(40, 17);
            this.lResults.TabIndex = 0;
            this.lResults.Text = "100%";
            this.lResults.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.lResults.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.lResults.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            // 
            // radioOwnChoice
            // 
            this.radioOwnChoice.AllowBindingControlLocation = false;
            this.radioOwnChoice.BackColor = System.Drawing.Color.Transparent;
            this.radioOwnChoice.BindingControlPosition = Bunifu.UI.WinForms.BunifuRadioButton.BindingControlPositions.Right;
            this.radioOwnChoice.BorderThickness = 1;
            this.radioOwnChoice.Checked = true;
            this.radioOwnChoice.Location = new System.Drawing.Point(45, 14);
            this.radioOwnChoice.Name = "radioOwnChoice";
            this.radioOwnChoice.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.radioOwnChoice.OutlineColorTabFocused = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.radioOwnChoice.OutlineColorUnchecked = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.radioOwnChoice.RadioColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(95)))), ((int)(((byte)(244)))));
            this.radioOwnChoice.RadioColorTabFocused = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.radioOwnChoice.Size = new System.Drawing.Size(18, 18);
            this.radioOwnChoice.TabIndex = 28;
            this.radioOwnChoice.Text = null;
            this.radioOwnChoice.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.radioOwnChoice.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.radioOwnChoice.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            // 
            // MiniNewsPollEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lChoice);
            this.Controls.Add(this.pPercentage);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MiniNewsPollEntry";
            this.Size = new System.Drawing.Size(424, 45);
            this.Click += new System.EventHandler(this.MiniNewsPollEntry_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MiniNewsPollEntry_Paint);
            this.MouseEnter += new System.EventHandler(this.MiniNewsPollEntry_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MiniNewsPollEntry_MouseLeave);
            this.pPercentage.ResumeLayout(false);
            this.pPercentage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lChoice;
        private System.Windows.Forms.Panel pPercentage;
        private System.Windows.Forms.Label lResults;
        private Bunifu.UI.WinForms.BunifuRadioButton radioOwnChoice;
    }
}
