
namespace ExaltAccountManager
{
    partial class FrmDailyLoginOptions
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
            this.pTop = new System.Windows.Forms.Panel();
            this.lHeadline = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pBox = new System.Windows.Forms.Panel();
            this.checkBoxUseTaskTrayTool = new System.Windows.Forms.CheckBox();
            this.checkBoxUseNotifications = new System.Windows.Forms.CheckBox();
            this.checkBoxShowNotificationOnStart = new System.Windows.Forms.CheckBox();
            this.checkBoxShowNotificationOnDone = new System.Windows.Forms.CheckBox();
            this.checkBoxShowNotificationOnError = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pSpacer = new System.Windows.Forms.Panel();
            this.btnRunTask = new System.Windows.Forms.Button();
            this.btnWindowsTask = new System.Windows.Forms.Button();
            this.btnTimings = new System.Windows.Forms.Button();
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
            this.pTop.Controls.Add(this.lHeadline);
            this.pTop.Controls.Add(this.label2);
            this.pTop.Controls.Add(this.pBox);
            this.pTop.Controls.Add(this.pbLogo);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(250, 48);
            this.pTop.TabIndex = 3;
            this.pTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pTop_Paint);
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.pTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // lHeadline
            // 
            this.lHeadline.AutoSize = true;
            this.lHeadline.Font = new System.Drawing.Font("Century Schoolbook", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHeadline.Location = new System.Drawing.Point(57, 9);
            this.lHeadline.Name = "lHeadline";
            this.lHeadline.Size = new System.Drawing.Size(127, 25);
            this.lHeadline.TabIndex = 2;
            this.lHeadline.Text = "Daily Login";
            this.lHeadline.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.lHeadline.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Schoolbook", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(161, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Beta";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseDown);
            this.label2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Drag_MouseMove);
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
            // checkBoxUseTaskTrayTool
            // 
            this.checkBoxUseTaskTrayTool.AutoSize = true;
            this.checkBoxUseTaskTrayTool.Location = new System.Drawing.Point(12, 82);
            this.checkBoxUseTaskTrayTool.Name = "checkBoxUseTaskTrayTool";
            this.checkBoxUseTaskTrayTool.Size = new System.Drawing.Size(117, 19);
            this.checkBoxUseTaskTrayTool.TabIndex = 4;
            this.checkBoxUseTaskTrayTool.Text = "Use Tasktray tool";
            this.checkBoxUseTaskTrayTool.UseVisualStyleBackColor = true;
            this.checkBoxUseTaskTrayTool.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxUseNotifications
            // 
            this.checkBoxUseNotifications.AutoSize = true;
            this.checkBoxUseNotifications.Location = new System.Drawing.Point(12, 105);
            this.checkBoxUseNotifications.Name = "checkBoxUseNotifications";
            this.checkBoxUseNotifications.Size = new System.Drawing.Size(154, 19);
            this.checkBoxUseNotifications.TabIndex = 5;
            this.checkBoxUseNotifications.Text = "Send Event Notifications";
            this.checkBoxUseNotifications.UseVisualStyleBackColor = true;
            this.checkBoxUseNotifications.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxShowNotificationOnStart
            // 
            this.checkBoxShowNotificationOnStart.AutoSize = true;
            this.checkBoxShowNotificationOnStart.Location = new System.Drawing.Point(12, 128);
            this.checkBoxShowNotificationOnStart.Name = "checkBoxShowNotificationOnStart";
            this.checkBoxShowNotificationOnStart.Size = new System.Drawing.Size(224, 19);
            this.checkBoxShowNotificationOnStart.TabIndex = 6;
            this.checkBoxShowNotificationOnStart.Text = "Send Notifications on Daily Login start";
            this.checkBoxShowNotificationOnStart.UseVisualStyleBackColor = true;
            this.checkBoxShowNotificationOnStart.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxShowNotificationOnDone
            // 
            this.checkBoxShowNotificationOnDone.AutoSize = true;
            this.checkBoxShowNotificationOnDone.Location = new System.Drawing.Point(12, 151);
            this.checkBoxShowNotificationOnDone.Name = "checkBoxShowNotificationOnDone";
            this.checkBoxShowNotificationOnDone.Size = new System.Drawing.Size(180, 19);
            this.checkBoxShowNotificationOnDone.TabIndex = 7;
            this.checkBoxShowNotificationOnDone.Text = "Send Notifications when done";
            this.checkBoxShowNotificationOnDone.UseVisualStyleBackColor = true;
            this.checkBoxShowNotificationOnDone.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxShowNotificationOnError
            // 
            this.checkBoxShowNotificationOnError.AutoSize = true;
            this.checkBoxShowNotificationOnError.Location = new System.Drawing.Point(12, 174);
            this.checkBoxShowNotificationOnError.Name = "checkBoxShowNotificationOnError";
            this.checkBoxShowNotificationOnError.Size = new System.Drawing.Size(166, 19);
            this.checkBoxShowNotificationOnError.TabIndex = 8;
            this.checkBoxShowNotificationOnError.Text = "Send Notifications on error";
            this.checkBoxShowNotificationOnError.UseVisualStyleBackColor = true;
            this.checkBoxShowNotificationOnError.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Schoolbook", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Notification settings";
            // 
            // pSpacer
            // 
            this.pSpacer.BackColor = System.Drawing.Color.Gainsboro;
            this.pSpacer.Location = new System.Drawing.Point(10, 199);
            this.pSpacer.Name = "pSpacer";
            this.pSpacer.Size = new System.Drawing.Size(228, 1);
            this.pSpacer.TabIndex = 11;
            // 
            // btnRunTask
            // 
            this.btnRunTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunTask.Image = global::ExaltAccountManager.Properties.Resources.ic_playlist_play_black_18dp;
            this.btnRunTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRunTask.Location = new System.Drawing.Point(12, 267);
            this.btnRunTask.Name = "btnRunTask";
            this.btnRunTask.Size = new System.Drawing.Size(224, 25);
            this.btnRunTask.TabIndex = 13;
            this.btnRunTask.Text = "Run Daily Login now";
            this.btnRunTask.UseVisualStyleBackColor = true;
            this.btnRunTask.Click += new System.EventHandler(this.btnRunTask_Click);
            // 
            // btnWindowsTask
            // 
            this.btnWindowsTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowsTask.Image = global::ExaltAccountManager.Properties.Resources.ic_settings_applications_black_18dp;
            this.btnWindowsTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWindowsTask.Location = new System.Drawing.Point(12, 237);
            this.btnWindowsTask.Name = "btnWindowsTask";
            this.btnWindowsTask.Size = new System.Drawing.Size(224, 25);
            this.btnWindowsTask.TabIndex = 12;
            this.btnWindowsTask.Text = "Windows Task Scheduler settings";
            this.btnWindowsTask.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWindowsTask.UseVisualStyleBackColor = true;
            this.btnWindowsTask.Click += new System.EventHandler(this.btnWindowsTask_Click);
            // 
            // btnTimings
            // 
            this.btnTimings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimings.Image = global::ExaltAccountManager.Properties.Resources.ic_access_time_black_18dp;
            this.btnTimings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTimings.Location = new System.Drawing.Point(12, 206);
            this.btnTimings.Name = "btnTimings";
            this.btnTimings.Size = new System.Drawing.Size(224, 25);
            this.btnTimings.TabIndex = 10;
            this.btnTimings.Text = "Timing settings";
            this.btnTimings.UseVisualStyleBackColor = true;
            this.btnTimings.Click += new System.EventHandler(this.btnTimings_Click);
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
            this.pbLogo.Image = global::ExaltAccountManager.Properties.Resources.ic_access_alarm_black_48dp;
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
            // FrmDailyLoginOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(250, 300);
            this.Controls.Add(this.btnRunTask);
            this.Controls.Add(this.btnWindowsTask);
            this.Controls.Add(this.pSpacer);
            this.Controls.Add(this.btnTimings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxShowNotificationOnError);
            this.Controls.Add(this.checkBoxShowNotificationOnDone);
            this.Controls.Add(this.checkBoxShowNotificationOnStart);
            this.Controls.Add(this.checkBoxUseNotifications);
            this.Controls.Add(this.checkBoxUseTaskTrayTool);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("Century Schoolbook", 7.875F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDailyLoginOptions";
            this.ShowInTaskbar = false;
            this.Text = "FrmDailyLoginOptions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Closing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmDailyLoginOptions_Paint);
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
        private System.Windows.Forms.CheckBox checkBoxUseTaskTrayTool;
        private System.Windows.Forms.CheckBox checkBoxUseNotifications;
        private System.Windows.Forms.CheckBox checkBoxShowNotificationOnStart;
        private System.Windows.Forms.CheckBox checkBoxShowNotificationOnDone;
        private System.Windows.Forms.CheckBox checkBoxShowNotificationOnError;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTimings;
        private System.Windows.Forms.Panel pSpacer;
        private System.Windows.Forms.Button btnWindowsTask;
        private System.Windows.Forms.Button btnRunTask;
    }
}