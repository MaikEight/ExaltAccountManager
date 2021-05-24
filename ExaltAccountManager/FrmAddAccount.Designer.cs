namespace ExaltAccountManager
{
    partial class FrmAddAccount
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddAccount));
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lSave = new System.Windows.Forms.Label();
            this.timerResetSaveLabel = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lInfo = new System.Windows.Forms.Label();
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.pBox);
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Controls.Add(this.lHeadline);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(250, 48);
            this.pTop.TabIndex = 1;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // pBox
            // 
            this.pBox.Controls.Add(this.pbMinimize);
            this.pBox.Controls.Add(this.pbClose);
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Location = new System.Drawing.Point(202, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(48, 48);
            this.pBox.TabIndex = 4;
            this.pBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pBox_Paint);
            this.pBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // pbMinimize
            // 
            this.pbMinimize.Image = global::ExaltAccountManager.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(0, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMinimize.TabIndex = 1;
            this.pbMinimize.TabStop = false;
            this.toolTip.SetToolTip(this.pbMinimize, "Minimize");
            this.pbMinimize.Visible = false;
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMinimize_Paint);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbClose
            // 
            this.pbClose.Image = global::ExaltAccountManager.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(24, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.TabIndex = 0;
            this.pbClose.TabStop = false;
            this.toolTip.SetToolTip(this.pbClose, "Close");
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.Paint += new System.Windows.Forms.PaintEventHandler(this.pbClose_Paint);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_person_add_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            this.pbLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pbLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(54, 9);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(135, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Add Account";
            this.lHeadline.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.lHeadline.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(40, 93);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(175, 20);
            this.tbName.TabIndex = 3;
            this.toolTip.SetToolTip(this.tbName, "Name used only inside this tool.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username*";
            this.toolTip.SetToolTip(this.label1, "Name used only inside this tool.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "E-Mail";
            this.toolTip.SetToolTip(this.label2, "E-Mail of the ROTMG-Account");
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(40, 153);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(175, 20);
            this.tbEmail.TabIndex = 1;
            this.toolTip.SetToolTip(this.tbEmail, "E-Mail of the ROTMG-Account");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password";
            this.toolTip.SetToolTip(this.label3, "Password of the ROTMG-Account");
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(40, 213);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(175, 20);
            this.tbPassword.TabIndex = 2;
            this.toolTip.SetToolTip(this.tbPassword, "Password of the ROTMG-Account");
            this.tbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPassword_KeyDown);
            // 
            // lSave
            // 
            this.lSave.AutoSize = true;
            this.lSave.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSave.Location = new System.Drawing.Point(95, 247);
            this.lSave.MaximumSize = new System.Drawing.Size(240, 0);
            this.lSave.Name = "lSave";
            this.lSave.Size = new System.Drawing.Size(59, 25);
            this.lSave.TabIndex = 8;
            this.lSave.Text = "Save";
            this.lSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.lSave, "Save!");
            this.lSave.Click += new System.EventHandler(this.lSave_Click);
            this.lSave.MouseEnter += new System.EventHandler(this.lSave_MouseEnter);
            this.lSave.MouseLeave += new System.EventHandler(this.lSave_MouseLeave);
            // 
            // timerResetSaveLabel
            // 
            this.timerResetSaveLabel.Interval = 2000;
            this.timerResetSaveLabel.Tick += new System.EventHandler(this.timerResetSaveLabel_Tick);
            // 
            // lInfo
            // 
            this.lInfo.AutoSize = true;
            this.lInfo.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lInfo.Location = new System.Drawing.Point(10, 284);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(230, 15);
            this.lInfo.TabIndex = 9;
            this.lInfo.Text = "* = leave empty for auto-fill via webrequest";
            this.toolTip.SetToolTip(this.lInfo, "Name used only inside this tool.");
            // 
            // FrmAddAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(250, 300);
            this.Controls.Add(this.lSave);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.pTop);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAddAccount";
            this.ShowInTaskbar = false;
            this.Text = "FrmAddAccount";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmAddAccount_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lHeadline;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lSave;
        private System.Windows.Forms.Timer timerResetSaveLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lInfo;
    }
}