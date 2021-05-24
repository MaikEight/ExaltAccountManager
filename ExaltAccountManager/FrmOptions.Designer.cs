namespace ExaltAccountManager
{
    partial class FrmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOptions));
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.lHeadline = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lPath = new System.Windows.Forms.Label();
            this.checkCloseAfterConnect = new System.Windows.Forms.CheckBox();
            this.lSave = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxAutoFill = new System.Windows.Forms.CheckBox();
            this.btnAddMuledump = new System.Windows.Forms.Button();
            this.btnChangeExePath = new System.Windows.Forms.Button();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
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
            this.pTop.TabIndex = 2;
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
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(81, 9);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(89, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Options";
            this.lHeadline.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.lHeadline.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path: RotMG Exalt.exe";
            // 
            // lPath
            // 
            this.lPath.AutoSize = true;
            this.lPath.BackColor = System.Drawing.Color.White;
            this.lPath.Font = new System.Drawing.Font("Century Schoolbook", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.lPath.Location = new System.Drawing.Point(12, 81);
            this.lPath.MaximumSize = new System.Drawing.Size(225, 0);
            this.lPath.Name = "lPath";
            this.lPath.Size = new System.Drawing.Size(223, 32);
            this.lPath.TabIndex = 4;
            this.lPath.Text = "Path leading to the Exalt.exe, NOT the launcher.";
            // 
            // checkCloseAfterConnect
            // 
            this.checkCloseAfterConnect.AutoSize = true;
            this.checkCloseAfterConnect.BackColor = System.Drawing.Color.Transparent;
            this.checkCloseAfterConnect.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkCloseAfterConnect.Location = new System.Drawing.Point(12, 178);
            this.checkCloseAfterConnect.Name = "checkCloseAfterConnect";
            this.checkCloseAfterConnect.Size = new System.Drawing.Size(152, 18);
            this.checkCloseAfterConnect.TabIndex = 6;
            this.checkCloseAfterConnect.Text = "Close after exalt start";
            this.toolTip.SetToolTip(this.checkCloseAfterConnect, "Close this tool after starting an Exalt Session.");
            this.checkCloseAfterConnect.UseVisualStyleBackColor = false;
            this.checkCloseAfterConnect.CheckedChanged += new System.EventHandler(this.checkCloseAfterConnect_CheckedChanged);
            // 
            // lSave
            // 
            this.lSave.AutoSize = true;
            this.lSave.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSave.Location = new System.Drawing.Point(95, 266);
            this.lSave.Name = "lSave";
            this.lSave.Size = new System.Drawing.Size(59, 25);
            this.lSave.TabIndex = 9;
            this.lSave.Text = "Save";
            this.toolTip.SetToolTip(this.lSave, "Save!");
            this.lSave.Click += new System.EventHandler(this.lSave_Click);
            this.lSave.MouseEnter += new System.EventHandler(this.lSave_MouseEnter);
            this.lSave.MouseLeave += new System.EventHandler(this.lSave_MouseLeave);
            // 
            // checkBoxAutoFill
            // 
            this.checkBoxAutoFill.AutoSize = true;
            this.checkBoxAutoFill.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxAutoFill.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAutoFill.Location = new System.Drawing.Point(12, 238);
            this.checkBoxAutoFill.Name = "checkBoxAutoFill";
            this.checkBoxAutoFill.Size = new System.Drawing.Size(228, 18);
            this.checkBoxAutoFill.TabIndex = 11;
            this.checkBoxAutoFill.Text = "Autofill Names (Muledump import)";
            this.toolTip.SetToolTip(this.checkBoxAutoFill, "Close this tool after starting an Exalt Session.");
            this.checkBoxAutoFill.UseVisualStyleBackColor = false;
            // 
            // btnAddMuledump
            // 
            this.btnAddMuledump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMuledump.Image = global::ExaltAccountManager.Properties.Resources.ic_add_to_photos_black_18dp;
            this.btnAddMuledump.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddMuledump.Location = new System.Drawing.Point(12, 203);
            this.btnAddMuledump.Name = "btnAddMuledump";
            this.btnAddMuledump.Size = new System.Drawing.Size(214, 32);
            this.btnAddMuledump.TabIndex = 10;
            this.btnAddMuledump.TabStop = false;
            this.btnAddMuledump.Text = "Add muledump accounts";
            this.toolTip.SetToolTip(this.btnAddMuledump, "Add accounts from a muledump formated file.");
            this.btnAddMuledump.UseVisualStyleBackColor = true;
            this.btnAddMuledump.Click += new System.EventHandler(this.btnAddMuledump_Click);
            // 
            // btnChangeExePath
            // 
            this.btnChangeExePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeExePath.Image = global::ExaltAccountManager.Properties.Resources.ic_folder_open_black_18dp;
            this.btnChangeExePath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChangeExePath.Location = new System.Drawing.Point(12, 143);
            this.btnChangeExePath.Name = "btnChangeExePath";
            this.btnChangeExePath.Size = new System.Drawing.Size(214, 32);
            this.btnChangeExePath.TabIndex = 5;
            this.btnChangeExePath.TabStop = false;
            this.btnChangeExePath.Text = "Change path";
            this.btnChangeExePath.UseVisualStyleBackColor = true;
            this.btnChangeExePath.Click += new System.EventHandler(this.btnChangeExePath_Click);
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
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_settings_black_48dp;
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
            // FrmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(250, 300);
            this.Controls.Add(this.checkBoxAutoFill);
            this.Controls.Add(this.btnAddMuledump);
            this.Controls.Add(this.lSave);
            this.Controls.Add(this.checkCloseAfterConnect);
            this.Controls.Add(this.btnChangeExePath);
            this.Controls.Add(this.lPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmOptions";
            this.ShowInTaskbar = false;
            this.Text = "FrmOptions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmOptions_Paint);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lPath;
        private System.Windows.Forms.Button btnChangeExePath;
        private System.Windows.Forms.CheckBox checkCloseAfterConnect;
        private System.Windows.Forms.Label lSave;
        private System.Windows.Forms.Button btnAddMuledump;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBoxAutoFill;
    }
}