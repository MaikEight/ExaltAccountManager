
namespace ExaltAccountManager
{
    partial class FrmServerListChanger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmServerListChanger));
            this.dropServers = new Bunifu.UI.WinForms.BunifuDropdown();
            this.pTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lDropHeadline = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // dropServers
            // 
            this.dropServers.BackColor = System.Drawing.Color.Transparent;
            this.dropServers.BackgroundColor = System.Drawing.Color.White;
            this.dropServers.BorderColor = System.Drawing.Color.Silver;
            this.dropServers.BorderRadius = 1;
            this.dropServers.Color = System.Drawing.Color.Silver;
            this.dropServers.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.dropServers.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropServers.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dropServers.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dropServers.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.dropServers.DisabledIndicatorColor = System.Drawing.Color.DarkGray;
            this.dropServers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropServers.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.dropServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropServers.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropServers.FillDropDown = true;
            this.dropServers.FillIndicator = false;
            this.dropServers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropServers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dropServers.ForeColor = System.Drawing.Color.Black;
            this.dropServers.FormattingEnabled = true;
            this.dropServers.Icon = null;
            this.dropServers.IndicatorAlignment = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropServers.IndicatorColor = System.Drawing.Color.Gray;
            this.dropServers.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.dropServers.ItemBackColor = System.Drawing.Color.White;
            this.dropServers.ItemBorderColor = System.Drawing.Color.White;
            this.dropServers.ItemForeColor = System.Drawing.Color.Black;
            this.dropServers.ItemHeight = 26;
            this.dropServers.ItemHighLightColor = System.Drawing.Color.DodgerBlue;
            this.dropServers.ItemHighLightForeColor = System.Drawing.Color.White;
            this.dropServers.Items.AddRange(new object[] {
            "Last joined server",
            "EAM Default server (Set in Options)"});
            this.dropServers.ItemTopMargin = 3;
            this.dropServers.Location = new System.Drawing.Point(5, 52);
            this.dropServers.Name = "dropServers";
            this.dropServers.Size = new System.Drawing.Size(390, 32);
            this.dropServers.TabIndex = 0;
            this.dropServers.Text = null;
            this.dropServers.TextAlignment = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.dropServers.TextLeftMargin = 5;
            this.dropServers.SelectedIndexChanged += new System.EventHandler(this.dropServers_SelectedIndexChanged);
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Controls.Add(this.label1);
            this.pTop.Controls.Add(this.pbClose);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(400, 24);
            this.pTop.TabIndex = 1;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serverlist";
            // 
            // lDropHeadline
            // 
            this.lDropHeadline.AutoSize = true;
            this.lDropHeadline.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDropHeadline.Location = new System.Drawing.Point(2, 33);
            this.lDropHeadline.Name = "lDropHeadline";
            this.lDropHeadline.Size = new System.Drawing.Size(242, 16);
            this.lDropHeadline.TabIndex = 3;
            this.lDropHeadline.Text = "Choose the server {0} is going to join";
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.list_24px;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(24, 24);
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            // 
            // pbClose
            // 
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(381, 1);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(18, 18);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClose.TabIndex = 0;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // FrmServerListChanger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 89);
            this.Controls.Add(this.lDropHeadline);
            this.Controls.Add(this.pTop);
            this.Controls.Add(this.dropServers);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmServerListChanger";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmServerListChanger_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuDropdown dropServers;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label lDropHeadline;
    }
}
