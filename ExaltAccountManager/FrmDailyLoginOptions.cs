using MK_EAM_Lib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmDailyLoginOptions : Form
    {
        public FrmMain frm { get; }
        NotificationOptions opt;

        Pen p = new Pen(Color.Black);
        bool startup = true;
        public bool _isDarkmode = false;

        UIDailyLoginsTimings uiTimings;
        UIWindowsTask uIWindowsTask;
        public FrmDailyLoginOptions(FrmMain _frm, NotificationOptions _opt)
        {
            InitializeComponent();
            frm = _frm;
            opt = _opt;

            LoadOptions();
            ApplyTheme(frm.useDarkmode);
            startup = false;
        }

        private void LoadOptions()
        {
            if (opt == null)
                opt = new NotificationOptions();

            checkBoxUseTaskTrayTool.Checked = opt.useTaskTrayTool;
            checkBoxUseNotifications.Checked = opt.useNotifications;
            checkBoxShowNotificationOnStart.Checked = opt.showNotificationOnStart;
            checkBoxShowNotificationOnDone.Checked = opt.showNotificationOnDone;
            checkBoxShowNotificationOnError.Checked = opt.showNotificationOnError;
        }

        public void ApplyTheme(bool isDarkmode)
        {
            _isDarkmode = isDarkmode;

            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.ForeColor = font;
                this.BackColor = second;
                pTop.BackColor = def;
                pBox.BackColor = def;

                checkBoxUseTaskTrayTool.ForeColor = checkBoxShowNotificationOnDone.ForeColor = checkBoxShowNotificationOnError.ForeColor = checkBoxShowNotificationOnStart.ForeColor = checkBoxUseNotifications.ForeColor = font;

                pbLogo.Image = Properties.Resources.ic_access_alarm_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;

                btnTimings.Image = Properties.Resources.ic_access_time_white_18dp;
                btnWindowsTask.Image = Properties.Resources.ic_settings_applications_white_18dp;
                btnRunTask.Image = Properties.Resources.ic_playlist_play_white_18dp;

                btnTimings.FlatAppearance.MouseOverBackColor = btnWindowsTask.FlatAppearance.MouseOverBackColor = btnRunTask.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 225, 225, 225);

                p = new Pen(Color.White);
            }
        }

        #region Button Close / Minimize
        private void pbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbClose.BackColor = Color.FromArgb(225, 50, 50);
            else
                pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbMinimize.BackColor = Color.DimGray;
            else
                pbMinimize.BackColor = Color.DarkGray;
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e)
        {
            pbMinimize.BackColor = Color.Transparent;
        }
        #endregion

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (startup) return;

            opt = new NotificationOptions()
            {
                useTaskTrayTool = checkBoxUseTaskTrayTool.Checked,
                useNotifications = checkBoxUseNotifications.Checked,
                showNotificationOnStart = checkBoxShowNotificationOnStart.Checked,
                showNotificationOnDone = checkBoxShowNotificationOnDone.Checked,
                showNotificationOnError = checkBoxShowNotificationOnError.Checked
            };

            frm.SaveNotificationoptions(opt);
        }

        private void FrmDailyLoginOptions_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        private void pTop_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
        }

        private void pbClose_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
        }

        private void pbMinimize_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);

            e.Graphics.DrawLine(p, topLeft, topRight);
        }

        private void pbLogo_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        private void btnTimings_Click(object sender, EventArgs e)
        {
            if (uiTimings == null)
            {
                uiTimings = new UIDailyLoginsTimings(this, opt);
            }

            if (!this.Controls.Contains(uiTimings))
            {
                this.Controls.Add(uiTimings);
                uiTimings.Location = new Point(1, pTop.Height);
                this.Controls.SetChildIndex(uiTimings, 0);
            }
        }

        public void CloseTimings()
        {
            if (uiTimings != null && this.Controls.Contains(uiTimings))
            {
                this.Controls.Remove(uiTimings);
            }
        }

        public void SaveTimingData(int joinTime, int killTime) => frm.SaveTimingData(joinTime, killTime);
        public void SaveTimingData(DateTime execTime) => frm.SaveTimingData(execTime);

        private void btnWindowsTask_Click(object sender, EventArgs e)
        {
            if (uIWindowsTask == null)
            {
                uIWindowsTask = new UIWindowsTask(this);
            }

            if (!this.Controls.Contains(uIWindowsTask))
            {
                this.Controls.Add(uIWindowsTask);
                uIWindowsTask.Location = new Point(1, pTop.Height);
                this.Controls.SetChildIndex(uIWindowsTask, 0);
            }
        }

        public void LogEvent(LogData data)
        {
            data.ID = frm.logs.Count - 1;
            frm.LogEvent(data);
        }

        public void CloseWindowsTask()
        {
            if (uIWindowsTask != null && this.Controls.Contains(uIWindowsTask))
            {
                this.Controls.Remove(uIWindowsTask);
            }
        }


        #region Drag Form
        private Point MouseDownLocation;
        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                this.Location = new Point(e.X + this.Left - MouseDownLocation.X, e.Y + this.Top - MouseDownLocation.Y);
        }
        #endregion

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }

        private void btnRunTask_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName == "EAM Daily Login Service")
                    {
                        return;
                    }
                }
                catch (Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
                {
                    // Intentionally empty - no security access to the process.
                    LogEvent(new LogData(0, "EAM", LogEventType.EAMError, $"Failed to get Task-Processes (access  denied)!"));
                    return;
                }
                catch (InvalidOperationException)
                {
                    // Intentionally empty - the process exited before getting details.
                    LogEvent(new LogData(0, "EAM", LogEventType.EAMError, $"Failed to get Task-Processes (process exited before getting details)!"));
                    return;
                }
                catch { }
            }

            //1
            DailyLogins logins = null;
            try
            {
                if (File.Exists(frm.dailyLoginsPath))
                {
                    logins = (DailyLogins)frm.ByteArrayToObject(File.ReadAllBytes(frm.dailyLoginsPath));
                    logins.lastUpdate = new DateTime(2000, 1, 1);
                }
                else
                {
                    logins = new DailyLogins();
                    logins.logins = new System.Collections.Generic.List<DailyData>();
                }
                File.WriteAllBytes(frm.dailyLoginsPath, frm.ObjectToByteArray(logins));
            }
            catch { }

            //2
            string taskpath = Path.Combine(Application.StartupPath, "DailyService", "EAM Daily Login Service.exe");
            if (File.Exists(taskpath))
            {
                ProcessStartInfo info = new ProcessStartInfo(taskpath);
                info.FileName = taskpath;
                info.WorkingDirectory = Path.GetDirectoryName(taskpath);

                Process p = new Process();
                p.StartInfo = info;
                p.Start();
            }
            else
                return;

            //3
            btnRunTask.Enabled = false;
        }
    }
}
