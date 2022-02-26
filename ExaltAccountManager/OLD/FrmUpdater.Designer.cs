
namespace ExaltAccountManager
{
    partial class FrmUpdater
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
            Utilities.BunifuPages.BunifuAnimatorNS.Animation animation1 = new Utilities.BunifuPages.BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpdater));
            this.pTop = new System.Windows.Forms.Panel();
            this.lHeadline = new System.Windows.Forms.Label();
            this.pBox = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pages = new Bunifu.UI.WinForms.BunifuPages();
            this.pageInfo = new System.Windows.Forms.TabPage();
            this.btnInfo = new System.Windows.Forms.Button();
            this.pbState = new System.Windows.Forms.PictureBox();
            this.lStatus = new System.Windows.Forms.Label();
            this.pageUpdate = new System.Windows.Forms.TabPage();
            this.btnDone = new System.Windows.Forms.Button();
            this.lWait = new System.Windows.Forms.Label();
            this.lUpdateStatus = new System.Windows.Forms.Label();
            this.progressbar = new Bunifu.UI.WinForms.BunifuCircleProgress();
            this.pTop.SuspendLayout();
            this.pBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pages.SuspendLayout();
            this.pageInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).BeginInit();
            this.pageUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.lHeadline);
            this.pTop.Controls.Add(this.pBox);
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(250, 48);
            this.pTop.TabIndex = 2;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(50, 13);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(170, 23);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "ROTMG Updater";
            // 
            // pBox
            // 
            this.pBox.Controls.Add(this.label1);
            this.pBox.Controls.Add(this.pbMinimize);
            this.pBox.Controls.Add(this.pbClose);
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Location = new System.Drawing.Point(202, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(48, 48);
            this.pBox.TabIndex = 4;
            this.pBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pBox_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Beta";
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
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.below_black_48px;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            this.pbLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLogo_Paint);
            // 
            // pages
            // 
            this.pages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.pages.AllowTransitions = false;
            this.pages.Controls.Add(this.pageInfo);
            this.pages.Controls.Add(this.pageUpdate);
            this.pages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pages.Location = new System.Drawing.Point(0, 48);
            this.pages.Multiline = true;
            this.pages.Name = "pages";
            this.pages.Page = this.pageUpdate;
            this.pages.PageIndex = 1;
            this.pages.PageName = "pageUpdate";
            this.pages.PageTitle = "Update";
            this.pages.SelectedIndex = 0;
            this.pages.Size = new System.Drawing.Size(250, 152);
            this.pages.TabIndex = 17;
            animation1.AnimateOnlyDifferences = false;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 0F;
            animation1.TransparencyCoeff = 0F;
            this.pages.Transition = animation1;
            this.pages.TransitionType = Utilities.BunifuPages.BunifuAnimatorNS.AnimationType.Custom;
            // 
            // pageInfo
            // 
            this.pageInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pageInfo.Controls.Add(this.btnInfo);
            this.pageInfo.Controls.Add(this.pbState);
            this.pageInfo.Controls.Add(this.lStatus);
            this.pageInfo.Location = new System.Drawing.Point(4, 4);
            this.pageInfo.Name = "pageInfo";
            this.pageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.pageInfo.Size = new System.Drawing.Size(242, 124);
            this.pageInfo.TabIndex = 0;
            this.pageInfo.Text = "Info";
            this.pageInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmUpdater_Paint);
            // 
            // btnInfo
            // 
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfo.Image = global::ExaltAccountManager.Properties.Resources.ic_search_black_24dp;
            this.btnInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInfo.Location = new System.Drawing.Point(39, 95);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(173, 34);
            this.btnInfo.TabIndex = 5;
            this.btnInfo.Text = "Search for updates";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            this.btnInfo.MouseEnter += new System.EventHandler(this.btnInfo_MouseEnter);
            this.btnInfo.MouseLeave += new System.EventHandler(this.btnInfo_MouseLeave);
            // 
            // pbState
            // 
            this.pbState.Image = global::ExaltAccountManager.Properties.Resources.ic_hourglass_empty_black_36dp;
            this.pbState.Location = new System.Drawing.Point(40, 29);
            this.pbState.Name = "pbState";
            this.pbState.Size = new System.Drawing.Size(36, 36);
            this.pbState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbState.TabIndex = 4;
            this.pbState.TabStop = false;
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.Font = new System.Drawing.Font("Century Schoolbook", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatus.Location = new System.Drawing.Point(82, 26);
            this.lStatus.MinimumSize = new System.Drawing.Size(0, 46);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(126, 46);
            this.lStatus.TabIndex = 3;
            this.lStatus.Text = "Not searched\r\nfor updates...";
            this.lStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pageUpdate
            // 
            this.pageUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pageUpdate.Controls.Add(this.btnDone);
            this.pageUpdate.Controls.Add(this.lWait);
            this.pageUpdate.Controls.Add(this.lUpdateStatus);
            this.pageUpdate.Controls.Add(this.progressbar);
            this.pageUpdate.Location = new System.Drawing.Point(4, 4);
            this.pageUpdate.Name = "pageUpdate";
            this.pageUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.pageUpdate.Size = new System.Drawing.Size(242, 124);
            this.pageUpdate.TabIndex = 1;
            this.pageUpdate.Text = "Update";
            this.pageUpdate.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmUpdater_Paint);
            // 
            // btnDone
            // 
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDone.Image = global::ExaltAccountManager.Properties.Resources.ic_done_black_24dp;
            this.btnDone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDone.Location = new System.Drawing.Point(85, 208);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(81, 34);
            this.btnDone.TabIndex = 20;
            this.btnDone.Text = "Finish";
            this.btnDone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Visible = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lWait
            // 
            this.lWait.AutoSize = true;
            this.lWait.Font = new System.Drawing.Font("Century Schoolbook", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lWait.Location = new System.Drawing.Point(49, 33);
            this.lWait.Name = "lWait";
            this.lWait.Size = new System.Drawing.Size(152, 16);
            this.lWait.TabIndex = 19;
            this.lWait.Text = "Please wait for it to finish";
            // 
            // lUpdateStatus
            // 
            this.lUpdateStatus.Font = new System.Drawing.Font("Century Schoolbook", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lUpdateStatus.Location = new System.Drawing.Point(36, 10);
            this.lUpdateStatus.Name = "lUpdateStatus";
            this.lUpdateStatus.Size = new System.Drawing.Size(178, 23);
            this.lUpdateStatus.TabIndex = 18;
            this.lUpdateStatus.Text = "Update in progress";
            this.lUpdateStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressbar
            // 
            this.progressbar.Animated = false;
            this.progressbar.AnimationInterval = 1;
            this.progressbar.AnimationSpeed = 1;
            this.progressbar.BackColor = System.Drawing.Color.Transparent;
            this.progressbar.CircleMargin = 10;
            this.progressbar.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Bold);
            this.progressbar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressbar.IsPercentage = true;
            this.progressbar.LineProgressThickness = 10;
            this.progressbar.LineThickness = 10;
            this.progressbar.Location = new System.Drawing.Point(50, 52);
            this.progressbar.Name = "progressbar";
            this.progressbar.ProgressAnimationSpeed = 200;
            this.progressbar.ProgressBackColor = System.Drawing.Color.Gainsboro;
            this.progressbar.ProgressColor = System.Drawing.Color.DodgerBlue;
            this.progressbar.ProgressColor2 = System.Drawing.Color.DodgerBlue;
            this.progressbar.ProgressEndCap = Bunifu.UI.WinForms.BunifuCircleProgress.CapStyles.Round;
            this.progressbar.ProgressFillStyle = Bunifu.UI.WinForms.BunifuCircleProgress.FillStyles.Solid;
            this.progressbar.ProgressStartCap = Bunifu.UI.WinForms.BunifuCircleProgress.CapStyles.Round;
            this.progressbar.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.progressbar.Size = new System.Drawing.Size(150, 150);
            this.progressbar.SubScriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SubScriptMargin = new System.Windows.Forms.Padding(5, -20, 0, 0);
            this.progressbar.SubScriptText = "";
            this.progressbar.SuperScriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SuperScriptMargin = new System.Windows.Forms.Padding(5, 50, 0, 0);
            this.progressbar.SuperScriptText = "%";
            this.progressbar.TabIndex = 17;
            this.progressbar.Text = "30";
            this.progressbar.TextMargin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.progressbar.Value = 30;
            this.progressbar.ValueByTransition = 30;
            this.progressbar.ValueMargin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            // 
            // FrmUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(250, 200);
            this.Controls.Add(this.pages);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ROTMG Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmUpdater_Paint);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pBox.ResumeLayout(false);
            this.pBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pages.ResumeLayout(false);
            this.pageInfo.ResumeLayout(false);
            this.pageInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).EndInit();
            this.pageUpdate.ResumeLayout(false);
            this.pageUpdate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lHeadline;
        private Bunifu.UI.WinForms.BunifuPages pages;
        private System.Windows.Forms.TabPage pageInfo;
        private System.Windows.Forms.PictureBox pbState;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.TabPage pageUpdate;
        private Bunifu.UI.WinForms.BunifuCircleProgress progressbar;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Label lWait;
        private System.Windows.Forms.Label lUpdateStatus;
        private System.Windows.Forms.Label label1;
    }
}