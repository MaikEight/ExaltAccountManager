
namespace ExaltAccountManager
{
    partial class FrmHWIDchanger
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHWIDchanger));
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            this.pTop = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.lHWIDHeadline = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnUseReal = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbHWID = new Bunifu.UI.WinForms.BunifuTextBox();
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Controls.Add(this.lHeadline);
            this.pTop.Controls.Add(this.pbClose);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(320, 24);
            this.pTop.TabIndex = 2;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.fingerprint_24px;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(24, 24);
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(26, 3);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(120, 19);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Change HWID";
            // 
            // pbClose
            // 
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(301, 1);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(18, 18);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClose.TabIndex = 0;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // lHWIDHeadline
            // 
            this.lHWIDHeadline.AutoSize = true;
            this.lHWIDHeadline.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHWIDHeadline.Location = new System.Drawing.Point(2, 31);
            this.lHWIDHeadline.Name = "lHWIDHeadline";
            this.lHWIDHeadline.Size = new System.Drawing.Size(82, 16);
            this.lHWIDHeadline.TabIndex = 0;
            this.lHWIDHeadline.Text = "HWID for {0}";
            // 
            // btnGenerate
            // 
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Image = global::ExaltAccountManager.Properties.Resources.fingerprint_scan_24px;
            this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerate.Location = new System.Drawing.Point(135, 84);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(180, 25);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate a random HWID";
            this.btnGenerate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnUseReal
            // 
            this.btnUseReal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUseReal.Image = global::ExaltAccountManager.Properties.Resources.fingerprint_24px;
            this.btnUseReal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUseReal.Location = new System.Drawing.Point(5, 84);
            this.btnUseReal.Name = "btnUseReal";
            this.btnUseReal.Size = new System.Drawing.Size(125, 25);
            this.btnUseReal.TabIndex = 1;
            this.btnUseReal.Text = "Use real HWID";
            this.btnUseReal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUseReal.UseVisualStyleBackColor = true;
            this.btnUseReal.Click += new System.EventHandler(this.btnUseReal_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Image = global::ExaltAccountManager.Properties.Resources.outline_save_black_18dp;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(5, 115);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(310, 25);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.MouseEnter += new System.EventHandler(this.btnSave_MouseEnter);
            this.btnSave.MouseLeave += new System.EventHandler(this.btnSave_MouseLeave);
            // 
            // tbHWID
            // 
            this.tbHWID.AcceptsReturn = false;
            this.tbHWID.AcceptsTab = false;
            this.tbHWID.AnimationSpeed = 200;
            this.tbHWID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbHWID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbHWID.BackColor = System.Drawing.Color.Transparent;
            this.tbHWID.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbHWID.BackgroundImage")));
            this.tbHWID.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.tbHWID.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.tbHWID.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.tbHWID.BorderColorIdle = System.Drawing.Color.Silver;
            this.tbHWID.BorderRadius = 1;
            this.tbHWID.BorderThickness = 1;
            this.tbHWID.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.tbHWID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbHWID.DefaultFont = new System.Drawing.Font("Segoe UI", 9.25F);
            this.tbHWID.DefaultText = "a99b9c7e7de21499287a1e48007a654bd1ffb153";
            this.tbHWID.FillColor = System.Drawing.Color.White;
            this.tbHWID.HideSelection = true;
            this.tbHWID.IconLeft = null;
            this.tbHWID.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbHWID.IconPadding = 10;
            this.tbHWID.IconRight = null;
            this.tbHWID.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.tbHWID.Lines = new string[] {
        "a99b9c7e7de21499287a1e48007a654bd1ffb153"};
            this.tbHWID.Location = new System.Drawing.Point(5, 49);
            this.tbHWID.MaximumSize = new System.Drawing.Size(310, 30);
            this.tbHWID.MaxLength = 40;
            this.tbHWID.MinimumSize = new System.Drawing.Size(310, 30);
            this.tbHWID.Modified = false;
            this.tbHWID.Multiline = false;
            this.tbHWID.Name = "tbHWID";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbHWID.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            stateProperties2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            stateProperties2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.tbHWID.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbHWID.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.Silver;
            stateProperties4.FillColor = System.Drawing.Color.White;
            stateProperties4.ForeColor = System.Drawing.Color.Empty;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.tbHWID.OnIdleState = stateProperties4;
            this.tbHWID.Padding = new System.Windows.Forms.Padding(3);
            this.tbHWID.PasswordChar = '\0';
            this.tbHWID.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.tbHWID.PlaceholderText = "hwid";
            this.tbHWID.ReadOnly = false;
            this.tbHWID.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbHWID.SelectedText = "";
            this.tbHWID.SelectionLength = 0;
            this.tbHWID.SelectionStart = 40;
            this.tbHWID.ShortcutsEnabled = true;
            this.tbHWID.Size = new System.Drawing.Size(310, 30);
            this.tbHWID.Style = Bunifu.UI.WinForms.BunifuTextBox._Style.Bunifu;
            this.tbHWID.TabIndex = 20;
            this.tbHWID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbHWID.TextMarginBottom = 0;
            this.tbHWID.TextMarginLeft = 3;
            this.tbHWID.TextMarginTop = 0;
            this.tbHWID.TextPlaceholder = "hwid";
            this.tbHWID.UseSystemPasswordChar = false;
            this.tbHWID.WordWrap = false;
            this.tbHWID.TextChanged += new System.EventHandler(this.tbHWID_TextChanged);
            // 
            // FrmHWIDchanger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(320, 145);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lHWIDHeadline);
            this.Controls.Add(this.btnUseReal);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbHWID);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmHWIDchanger";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "HWID Changer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmHWIDchanger_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.PictureBox pbClose;
        private Bunifu.UI.WinForms.BunifuTextBox tbHWID;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUseReal;
        private System.Windows.Forms.Label lHWIDHeadline;
        private System.Windows.Forms.Button btnGenerate;
    }
}