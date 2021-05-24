
namespace ExaltAccountManager
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.pTop = new System.Windows.Forms.Panel();
            this.pBox = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lHeadline = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lVersion = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.linkMPGH = new System.Windows.Forms.LinkLabel();
            this.timerLlama = new System.Windows.Forms.Timer(this.components);
            this.lPerms = new System.Windows.Forms.Label();
            this.pbDevName = new System.Windows.Forms.PictureBox();
            this.pbWeb = new System.Windows.Forms.PictureBox();
            this.pbDev = new System.Windows.Forms.PictureBox();
            this.pbVersion = new System.Windows.Forms.PictureBox();
            this.pbProgram = new System.Windows.Forms.PictureBox();
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDevName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbWeb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProgram)).BeginInit();
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
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.Paint += new System.Windows.Forms.PaintEventHandler(this.pbClose_Paint);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_info_outline_black_48dp;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(54, 9);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(71, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "About";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(55, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Exalt Account Manager";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Program name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(47, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Version";
            // 
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.lVersion.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lVersion.Location = new System.Drawing.Point(55, 128);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(53, 20);
            this.lVersion.TabIndex = 8;
            this.lVersion.Text = "v1.5.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(47, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Developer";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(47, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Website";
            // 
            // linkMPGH
            // 
            this.linkMPGH.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkMPGH.AutoSize = true;
            this.linkMPGH.ForeColor = System.Drawing.Color.Black;
            this.linkMPGH.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkMPGH.Location = new System.Drawing.Point(56, 227);
            this.linkMPGH.MaximumSize = new System.Drawing.Size(160, 0);
            this.linkMPGH.Name = "linkMPGH";
            this.linkMPGH.Size = new System.Drawing.Size(159, 30);
            this.linkMPGH.TabIndex = 16;
            this.linkMPGH.TabStop = true;
            this.linkMPGH.Text = "https://www.mpgh.net/forum/member.php?u=1465031";
            this.linkMPGH.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMPGH_LinkClicked);
            // 
            // timerLlama
            // 
            this.timerLlama.Interval = 2500;
            this.timerLlama.Tick += new System.EventHandler(this.timerLlama_Tick);
            // 
            // lPerms
            // 
            this.lPerms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lPerms.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPerms.Location = new System.Drawing.Point(5, 272);
            this.lPerms.MinimumSize = new System.Drawing.Size(240, 100);
            this.lPerms.Name = "lPerms";
            this.lPerms.Size = new System.Drawing.Size(240, 161);
            this.lPerms.TabIndex = 17;
            this.lPerms.Text = resources.GetString("lPerms.Text");
            this.lPerms.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lPerms.UseMnemonic = false;
            // 
            // pbDevName
            // 
            this.pbDevName.Image = global::ExaltAccountManager.Properties.Resources.Logo_NameOnly;
            this.pbDevName.Location = new System.Drawing.Point(59, 178);
            this.pbDevName.Name = "pbDevName";
            this.pbDevName.Size = new System.Drawing.Size(92, 22);
            this.pbDevName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDevName.TabIndex = 18;
            this.pbDevName.TabStop = false;
            this.pbDevName.Click += new System.EventHandler(this.lDev_Click);
            // 
            // pbWeb
            // 
            this.pbWeb.Image = global::ExaltAccountManager.Properties.Resources.ic_public_black_36dp;
            this.pbWeb.Location = new System.Drawing.Point(12, 212);
            this.pbWeb.Name = "pbWeb";
            this.pbWeb.Size = new System.Drawing.Size(36, 36);
            this.pbWeb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbWeb.TabIndex = 13;
            this.pbWeb.TabStop = false;
            // 
            // pbDev
            // 
            this.pbDev.Image = global::ExaltAccountManager.Properties.Resources.ic_code_black_36dp;
            this.pbDev.Location = new System.Drawing.Point(12, 162);
            this.pbDev.Name = "pbDev";
            this.pbDev.Size = new System.Drawing.Size(36, 36);
            this.pbDev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbDev.TabIndex = 10;
            this.pbDev.TabStop = false;
            // 
            // pbVersion
            // 
            this.pbVersion.Image = global::ExaltAccountManager.Properties.Resources.baseline_tag_black_36dp;
            this.pbVersion.Location = new System.Drawing.Point(12, 112);
            this.pbVersion.Name = "pbVersion";
            this.pbVersion.Size = new System.Drawing.Size(36, 36);
            this.pbVersion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVersion.TabIndex = 7;
            this.pbVersion.TabStop = false;
            // 
            // pbProgram
            // 
            this.pbProgram.Image = global::ExaltAccountManager.Properties.Resources.ic_account_balance_wallet_black_36dp;
            this.pbProgram.Location = new System.Drawing.Point(12, 62);
            this.pbProgram.Name = "pbProgram";
            this.pbProgram.Size = new System.Drawing.Size(36, 36);
            this.pbProgram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbProgram.TabIndex = 4;
            this.pbProgram.TabStop = false;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(250, 440);
            this.Controls.Add(this.pbDevName);
            this.Controls.Add(this.lPerms);
            this.Controls.Add(this.linkMPGH);
            this.Controls.Add(this.pbWeb);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pbDev);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pbVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lVersion);
            this.Controls.Add(this.pbProgram);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmAbout";
            this.Text = "About EAM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmAbout_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDevName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbWeb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProgram)).EndInit();
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
        private System.Windows.Forms.PictureBox pbProgram;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.PictureBox pbDev;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pbWeb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkMPGH;
        private System.Windows.Forms.Timer timerLlama;
        private System.Windows.Forms.Label lPerms;
        private System.Windows.Forms.PictureBox pbDevName;
    }
}