namespace EAM_Tasktray_Tool
{
    partial class FrmTaskTrayTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTaskTrayTool));
            this.pTop = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pTopTop = new System.Windows.Forms.Panel();
            this.pbMinimize = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pHeadline = new System.Windows.Forms.Panel();
            this.lEAMHead = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.progressbar = new Bunifu.UI.WinForms.BunifuCircleProgress();
            this.pbProgress = new System.Windows.Forms.PictureBox();
            this.pbStart = new System.Windows.Forms.PictureBox();
            this.lStartTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lState = new System.Windows.Forms.Label();
            this.pbStatus = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.timerCheckForTask = new System.Windows.Forms.Timer(this.components);
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.shadowTaskInfo = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.spacerQuestion = new Bunifu.UI.WinForms.BunifuSeparator();
            this.lQuestion = new System.Windows.Forms.Label();
            this.shadowProgress = new Bunifu.UI.WinForms.BunifuShadowPanel();
            this.bunifuSeparator1 = new Bunifu.UI.WinForms.BunifuSeparator();
            this.lProgressBlock = new System.Windows.Forms.Label();
            this.pTop.SuspendLayout();
            this.pTopTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.pHeadline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProgress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            this.shadowTaskInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.shadowProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.panel2);
            this.pTop.Controls.Add(this.pTopTop);
            this.pTop.Controls.Add(this.pHeadline);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(223, 40);
            this.pTop.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(175, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(48, 3);
            this.panel2.TabIndex = 5;
            // 
            // pTopTop
            // 
            this.pTopTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pTopTop.Controls.Add(this.pbMinimize);
            this.pTopTop.Controls.Add(this.pbClose);
            this.pTopTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTopTop.Location = new System.Drawing.Point(175, 0);
            this.pTopTop.Name = "pTopTop";
            this.pTopTop.Size = new System.Drawing.Size(48, 24);
            this.pTopTop.TabIndex = 3;
            // 
            // pbMinimize
            // 
            this.pbMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbMinimize.Image = global::EAM_Tasktray_Tool.Properties.Resources.baseline_minimize_black_24dp;
            this.pbMinimize.Location = new System.Drawing.Point(0, 0);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new System.Drawing.Size(24, 24);
            this.pbMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMinimize.TabIndex = 3;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.Click += new System.EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseEnter += new System.EventHandler(this.pbMinimize_MouseEnter);
            this.pbMinimize.MouseLeave += new System.EventHandler(this.pbMinimize_MouseLeave);
            // 
            // pbClose
            // 
            this.pbClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbClose.Image = global::EAM_Tasktray_Tool.Properties.Resources.ic_close_black_24dp;
            this.pbClose.Location = new System.Drawing.Point(24, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbClose.TabIndex = 4;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // pHeadline
            // 
            this.pHeadline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.pHeadline.Controls.Add(this.lEAMHead);
            this.pHeadline.Controls.Add(this.label2);
            this.pHeadline.Controls.Add(this.pbLogo);
            this.pHeadline.Dock = System.Windows.Forms.DockStyle.Left;
            this.pHeadline.Location = new System.Drawing.Point(0, 0);
            this.pHeadline.Name = "pHeadline";
            this.pHeadline.Padding = new System.Windows.Forms.Padding(2);
            this.pHeadline.Size = new System.Drawing.Size(175, 40);
            this.pHeadline.TabIndex = 4;
            // 
            // lEAMHead
            // 
            this.lEAMHead.AutoSize = true;
            this.lEAMHead.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEAMHead.ForeColor = System.Drawing.Color.White;
            this.lEAMHead.Location = new System.Drawing.Point(59, 6);
            this.lEAMHead.Name = "lEAMHead";
            this.lEAMHead.Size = new System.Drawing.Size(97, 12);
            this.lEAMHead.TabIndex = 3;
            this.lEAMHead.Text = "Exalt Account Manager";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(43, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Notification Center";
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbLogo.Image = global::EAM_Tasktray_Tool.Properties.Resources.logo;
            this.pbLogo.Location = new System.Drawing.Point(2, 2);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(36, 36);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            // 
            // progressbar
            // 
            this.progressbar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.progressbar.Animated = false;
            this.progressbar.AnimationInterval = 1;
            this.progressbar.AnimationSpeed = 1;
            this.progressbar.BackColor = System.Drawing.Color.Transparent;
            this.progressbar.CircleMargin = 10;
            this.progressbar.Font = new System.Drawing.Font("Segoe UI", 35.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressbar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressbar.IsPercentage = true;
            this.progressbar.LineProgressThickness = 10;
            this.progressbar.LineThickness = 10;
            this.progressbar.Location = new System.Drawing.Point(43, 47);
            this.progressbar.Name = "progressbar";
            this.progressbar.ProgressAnimationSpeed = 200;
            this.progressbar.ProgressBackColor = System.Drawing.Color.White;
            this.progressbar.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.progressbar.ProgressColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(98)))), ((int)(((byte)(0)))), ((int)(((byte)(238)))));
            this.progressbar.ProgressEndCap = Bunifu.UI.WinForms.BunifuCircleProgress.CapStyles.Round;
            this.progressbar.ProgressFillStyle = Bunifu.UI.WinForms.BunifuCircleProgress.FillStyles.Solid;
            this.progressbar.ProgressStartCap = Bunifu.UI.WinForms.BunifuCircleProgress.CapStyles.Round;
            this.progressbar.SecondaryFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressbar.Size = new System.Drawing.Size(138, 138);
            this.progressbar.SubScriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SubScriptMargin = new System.Windows.Forms.Padding(5, -20, 0, 0);
            this.progressbar.SubScriptText = "";
            this.progressbar.SuperScriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressbar.SuperScriptMargin = new System.Windows.Forms.Padding(5, 50, 0, 0);
            this.progressbar.SuperScriptText = "%";
            this.progressbar.TabIndex = 17;
            this.progressbar.Text = "30";
            this.progressbar.TextMargin = new System.Windows.Forms.Padding(0);
            this.progressbar.Value = 30;
            this.progressbar.ValueByTransition = 30;
            this.progressbar.ValueMargin = new System.Windows.Forms.Padding(0);
            // 
            // pbProgress
            // 
            this.pbProgress.Image = global::EAM_Tasktray_Tool.Properties.Resources.loading_24px;
            this.pbProgress.Location = new System.Drawing.Point(12, 13);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(24, 24);
            this.pbProgress.TabIndex = 22;
            this.pbProgress.TabStop = false;
            this.pbProgress.Click += new System.EventHandler(this.pbProgress_Click);
            // 
            // pbStart
            // 
            this.pbStart.Image = global::EAM_Tasktray_Tool.Properties.Resources.ic_access_time_black_24dp;
            this.pbStart.Location = new System.Drawing.Point(12, 78);
            this.pbStart.Name = "pbStart";
            this.pbStart.Size = new System.Drawing.Size(24, 24);
            this.pbStart.TabIndex = 21;
            this.pbStart.TabStop = false;
            // 
            // lStartTime
            // 
            this.lStartTime.AutoSize = true;
            this.lStartTime.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStartTime.Location = new System.Drawing.Point(103, 81);
            this.lStartTime.Name = "lStartTime";
            this.lStartTime.Size = new System.Drawing.Size(49, 20);
            this.lStartTime.TabIndex = 20;
            this.lStartTime.Text = "00:00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(35, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 25);
            this.label7.TabIndex = 19;
            this.label7.Text = "Start:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(35, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 25);
            this.label5.TabIndex = 18;
            this.label5.Text = "Progress";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // lState
            // 
            this.lState.AutoSize = true;
            this.lState.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lState.ForeColor = System.Drawing.Color.SeaGreen;
            this.lState.Location = new System.Drawing.Point(103, 51);
            this.lState.Name = "lState";
            this.lState.Size = new System.Drawing.Size(68, 20);
            this.lState.TabIndex = 1;
            this.lState.Text = "Running";
            // 
            // pbStatus
            // 
            this.pbStatus.Image = global::EAM_Tasktray_Tool.Properties.Resources.ic_search_black_24dp;
            this.pbStatus.Location = new System.Drawing.Point(12, 49);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(24, 24);
            this.pbStatus.TabIndex = 0;
            this.pbStatus.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Status:";
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "EAM.DailyLoginsV2";
            this.fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Changed);
            this.fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Created);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "30 / 100 Accounts done.";
            this.notifyIcon.BalloonTipTitle = "Exalt Account Manager Daily Login";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "EAM Daily Login\r\nClick to open progress.";
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // timerCheckForTask
            // 
            this.timerCheckForTask.Interval = 2500;
            this.timerCheckForTask.Tick += new System.EventHandler(this.timerCheckForTask_Tick);
            // 
            // timerClose
            // 
            this.timerClose.Interval = 30000;
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // shadowTaskInfo
            // 
            this.shadowTaskInfo.BackColor = System.Drawing.Color.White;
            this.shadowTaskInfo.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadowTaskInfo.BorderRadius = 9;
            this.shadowTaskInfo.BorderThickness = 1;
            this.shadowTaskInfo.Controls.Add(this.pbInfo);
            this.shadowTaskInfo.Controls.Add(this.spacerQuestion);
            this.shadowTaskInfo.Controls.Add(this.lQuestion);
            this.shadowTaskInfo.Controls.Add(this.pbStart);
            this.shadowTaskInfo.Controls.Add(this.pbStatus);
            this.shadowTaskInfo.Controls.Add(this.lStartTime);
            this.shadowTaskInfo.Controls.Add(this.label3);
            this.shadowTaskInfo.Controls.Add(this.label7);
            this.shadowTaskInfo.Controls.Add(this.lState);
            this.shadowTaskInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.shadowTaskInfo.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadowTaskInfo.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadowTaskInfo.Location = new System.Drawing.Point(0, 40);
            this.shadowTaskInfo.Margin = new System.Windows.Forms.Padding(0);
            this.shadowTaskInfo.Name = "shadowTaskInfo";
            this.shadowTaskInfo.PanelColor = System.Drawing.Color.White;
            this.shadowTaskInfo.PanelColor2 = System.Drawing.Color.White;
            this.shadowTaskInfo.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadowTaskInfo.ShadowDept = 2;
            this.shadowTaskInfo.ShadowDepth = 4;
            this.shadowTaskInfo.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadowTaskInfo.ShadowTopLeftVisible = false;
            this.shadowTaskInfo.Size = new System.Drawing.Size(223, 114);
            this.shadowTaskInfo.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadowTaskInfo.TabIndex = 17;
            // 
            // pbInfo
            // 
            this.pbInfo.Image = global::EAM_Tasktray_Tool.Properties.Resources.ic_info_outline_black_24dp;
            this.pbInfo.Location = new System.Drawing.Point(12, 13);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(24, 24);
            this.pbInfo.TabIndex = 22;
            this.pbInfo.TabStop = false;
            // 
            // spacerQuestion
            // 
            this.spacerQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spacerQuestion.BackColor = System.Drawing.Color.Transparent;
            this.spacerQuestion.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("spacerQuestion.BackgroundImage")));
            this.spacerQuestion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.spacerQuestion.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.spacerQuestion.LineColor = System.Drawing.Color.Silver;
            this.spacerQuestion.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.spacerQuestion.LineThickness = 1;
            this.spacerQuestion.Location = new System.Drawing.Point(16, 36);
            this.spacerQuestion.Margin = new System.Windows.Forms.Padding(5);
            this.spacerQuestion.Name = "spacerQuestion";
            this.spacerQuestion.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.spacerQuestion.Size = new System.Drawing.Size(191, 10);
            this.spacerQuestion.TabIndex = 13;
            // 
            // lQuestion
            // 
            this.lQuestion.AutoSize = true;
            this.lQuestion.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lQuestion.Location = new System.Drawing.Point(35, 12);
            this.lQuestion.MaximumSize = new System.Drawing.Size(468, 0);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new System.Drawing.Size(163, 25);
            this.lQuestion.TabIndex = 12;
            this.lQuestion.Text = "Task informations";
            this.lQuestion.UseMnemonic = false;
            // 
            // shadowProgress
            // 
            this.shadowProgress.BackColor = System.Drawing.Color.White;
            this.shadowProgress.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.shadowProgress.BorderRadius = 9;
            this.shadowProgress.BorderThickness = 1;
            this.shadowProgress.Controls.Add(this.lProgressBlock);
            this.shadowProgress.Controls.Add(this.progressbar);
            this.shadowProgress.Controls.Add(this.pbProgress);
            this.shadowProgress.Controls.Add(this.label5);
            this.shadowProgress.Controls.Add(this.bunifuSeparator1);
            this.shadowProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shadowProgress.FillStyle = Bunifu.UI.WinForms.BunifuShadowPanel.FillStyles.Solid;
            this.shadowProgress.GradientMode = Bunifu.UI.WinForms.BunifuShadowPanel.GradientModes.Vertical;
            this.shadowProgress.Location = new System.Drawing.Point(0, 154);
            this.shadowProgress.Margin = new System.Windows.Forms.Padding(0);
            this.shadowProgress.Name = "shadowProgress";
            this.shadowProgress.PanelColor = System.Drawing.Color.White;
            this.shadowProgress.PanelColor2 = System.Drawing.Color.White;
            this.shadowProgress.ShadowColor = System.Drawing.Color.DarkGray;
            this.shadowProgress.ShadowDept = 2;
            this.shadowProgress.ShadowDepth = 4;
            this.shadowProgress.ShadowStyle = Bunifu.UI.WinForms.BunifuShadowPanel.ShadowStyles.Surrounded;
            this.shadowProgress.ShadowTopLeftVisible = false;
            this.shadowProgress.Size = new System.Drawing.Size(223, 197);
            this.shadowProgress.Style = Bunifu.UI.WinForms.BunifuShadowPanel.BevelStyles.Flat;
            this.shadowProgress.TabIndex = 18;
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuSeparator1.BackgroundImage")));
            this.bunifuSeparator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuSeparator1.DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.Silver;
            this.bunifuSeparator1.LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.Solid;
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(16, 37);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal;
            this.bunifuSeparator1.Size = new System.Drawing.Size(191, 10);
            this.bunifuSeparator1.TabIndex = 13;
            // 
            // lProgressBlock
            // 
            this.lProgressBlock.AutoSize = true;
            this.lProgressBlock.Font = new System.Drawing.Font("Segoe UI Black", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lProgressBlock.Location = new System.Drawing.Point(49, 84);
            this.lProgressBlock.Name = "lProgressBlock";
            this.lProgressBlock.Size = new System.Drawing.Size(126, 65);
            this.lProgressBlock.TabIndex = 23;
            this.lProgressBlock.Text = "N/A";
            this.lProgressBlock.Visible = false;
            // 
            // FrmTaskTrayTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(223, 351);
            this.ControlBox = false;
            this.Controls.Add(this.shadowProgress);
            this.Controls.Add(this.shadowTaskInfo);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmTaskTrayTool";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Notification Center";
            this.Load += new System.EventHandler(this.FrmTaskTrayTool_Load);
            this.pTop.ResumeLayout(false);
            this.pTopTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.pHeadline.ResumeLayout(false);
            this.pHeadline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProgress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            this.shadowTaskInfo.ResumeLayout(false);
            this.shadowTaskInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.shadowProgress.ResumeLayout(false);
            this.shadowProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel pHeadline;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lEAMHead;
        private System.Windows.Forms.Panel pTopTop;
        private System.Windows.Forms.PictureBox pbMinimize;
        private System.Windows.Forms.PictureBox pbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lState;
        private System.Windows.Forms.Label lStartTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private Bunifu.UI.WinForms.BunifuCircleProgress progressbar;
        private System.Windows.Forms.PictureBox pbStart;
        private System.Windows.Forms.PictureBox pbProgress;
        private System.IO.FileSystemWatcher fileSystemWatcher;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer timerCheckForTask;
        private System.Windows.Forms.Timer timerClose;
        private System.Windows.Forms.PictureBox pbClose;
        private Bunifu.UI.WinForms.BunifuShadowPanel shadowProgress;
        private Bunifu.UI.WinForms.BunifuSeparator bunifuSeparator1;
        private Bunifu.UI.WinForms.BunifuShadowPanel shadowTaskInfo;
        private Bunifu.UI.WinForms.BunifuSeparator spacerQuestion;
        private System.Windows.Forms.Label lQuestion;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Label lProgressBlock;
    }
}