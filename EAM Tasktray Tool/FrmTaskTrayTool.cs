using ExaltAccountManager;
using MK_EAM_Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_Tasktray_Tool
{
    public partial class FrmTaskTrayTool : Form
    {
        public static string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        public string optionsPath = Path.Combine(saveFilePath, "EAM.options");
        public string dailyLoginsPath = Path.Combine(saveFilePath, "EAM.DailyLoginsV2");
        public string notificationOptionsPath = Path.Combine(saveFilePath, "EAM.NotificationOptions");

        private bool taskIsrunning = false;
        private bool taskFoundOnce = false;
        private bool UseDarkmode = false;
        private NotificationOptions notificationOptions;

        public FrmTaskTrayTool()
        {
            InitializeComponent();
            HideForm();

            try
            {
                OptionsData optionsData = (OptionsData)ByteArrayToObject(File.ReadAllBytes(optionsPath));
                UseDarkmode = optionsData.useDarkmode;
                ApplyTheme();
            }
            catch { }

            try
            {
                if (File.Exists(notificationOptionsPath))
                    notificationOptions = (NotificationOptions)ByteArrayToObject(File.ReadAllBytes(notificationOptionsPath));
                else
                    notificationOptions = new NotificationOptions();
            }
            catch
            {
                notificationOptions = new NotificationOptions();
            }

            if (notificationOptions.showNotificationOnStart)            
                ShowNotification("EAM Daily Login started", " ", 0);
            

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Size.Width - (this.Width + 2), Screen.PrimaryScreen.WorkingArea.Size.Height - (this.Height + 2));
            progressbar.Maximum = 100;

            fileSystemWatcher.Path = saveFilePath;
            fileSystemWatcher.Filter = "EAM.DailyLoginsV2";            
        }

        private void FrmTaskTrayTool_Load(object sender, EventArgs e)
        {
            LoadUI();

            timerCheckForTask.Start();
        }

        private void ApplyTheme()
        {
            Color def = ColorScheme.GetColorDef(UseDarkmode);
            Color second = ColorScheme.GetColorSecond(UseDarkmode);
            Color third = ColorScheme.GetColorThird(UseDarkmode);
            Color font = ColorScheme.GetColorFont(UseDarkmode);

            this.BackColor = 
            pTop.BackColor = def;
            pTopTop.BackColor = second;
            this.ForeColor = font;

            pbInfo.Image = UseDarkmode ? Properties.Resources.ic_info_outline_white_24dp : Properties.Resources.ic_info_outline_black_24dp;
            pbStatus.Image = UseDarkmode ? Properties.Resources.ic_search_white_24dp : Properties.Resources.ic_search_black_24dp;
            pbStart.Image = UseDarkmode ? Properties.Resources.ic_access_time_white_24dp : Properties.Resources.ic_access_time_black_24dp;
            pbProgress.Image = UseDarkmode ? Properties.Resources.loading_white_24px : Properties.Resources.loading_24px;

            pbMinimize.Image = UseDarkmode ? Properties.Resources.baseline_minimize_white_24dp : Properties.Resources.baseline_minimize_black_24dp;
            pbClose.Image = UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;

            progressbar.ForeColor = UseDarkmode ? Color.FromArgb(230, 230, 230) : Color.FromArgb(64, 64, 64);

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in this.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
            {
                shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
                shadow.BorderColor = shadow.BorderColor = second;
                shadow.ShadowColor = shadow.ShadowColor = UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
            }
            this.Invalidate();
        }

        private void LoadUI()
        {
            try
            {
                taskIsrunning = IsTaskRunning();

                lState.Text = taskIsrunning ? "Running" : taskFoundOnce ? "Exited" : "Not started";
                lState.ForeColor = taskIsrunning ? Color.SeaGreen : taskFoundOnce ? Color.DodgerBlue : Color.Crimson;

                if (taskFoundOnce && File.Exists(dailyLoginsPath))
                {
                    DailyData data = null;
                    using (StreamReader file = File.OpenText(dailyLoginsPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        DailyLogins dailyLogins = (DailyLogins)serializer.Deserialize(file, typeof(DailyLogins));
                        data = dailyLogins.DailyDatas.OrderByDescending(d => d.Date).FirstOrDefault();
                        if (data.Date.Date != DateTime.Now.Date)
                            data = null;
                    }
                    if (data != null)
                    {
                        lProgressBlock.Visible = false;
                        progressbar.Visible = true;

                        lStartTime.Text = data.StartTime.ToString("HH:mm");

                        double max = data.PlannedLogins > 0 ? (data.PlannedLogins >= data.AccountData.Count ? data.PlannedLogins : 1) : 1 > data.AccountData.Count ? 1 : data.AccountData.Count;
                        double amnt = data.AccountData.Count - 1;
                        amnt = amnt > 0 ? amnt : 1;
                        progressbar.ValueByTransition = ((int)Math.Min((amnt / max) * 100d, 100d));

                        if (data.AccountData.Count >= data.PlannedLogins && (taskFoundOnce && !taskIsrunning))
                        {
                            progressbar.ValueByTransition = progressbar.Maximum;
                            timerCheckForTask.Stop();

                            timerClose.Start();
                            if (notificationOptions.showNotificationOnDone)
                                ShowNotification("Daily Logins finished", $"{data.AccountData.Where(d => d.Success).Count()} / {data.AccountData.Count} logins are Successfull.", 0);
                        }
                    }
                    else
                    {
                        lStartTime.Text = "N/A";
                        progressbar.ValueByTransition = 0;
                        progressbar.Visible = false;
                        lProgressBlock.Visible = true;

                        if (notificationOptions.showNotificationOnError)
                            ShowNotification("Daily Logins failed to find data", "Failed to find data to display.", 0);
                    }
                }
                else
                {
                    lStartTime.Text = "N/A";
                    progressbar.ValueByTransition = 0;
                    progressbar.Visible = false;
                    lProgressBlock.Visible = true;
                }
            }
            catch { }
        }

        private bool IsTaskRunning()
        {
            foreach (var process in System.Diagnostics.Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName == "EAM Daily Login Service")
                    {
                        taskFoundOnce = true;
                        return true;
                    }
                }
                catch (System.ComponentModel.Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
                {
                    // Intentionally empty - no security access to the process.
                }
                catch (InvalidOperationException)
                {
                    // Intentionally empty - the process exited before getting details.
                }
                catch { }
            }
            return false;
        }

        private void ShowNotification(string title, string text, int icon = 0)
        {
            if (!notificationOptions.useNotifications)
                return;

            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = text;
            notifyIcon.BalloonTipIcon = (ToolTipIcon)icon;
            
            bool state = notifyIcon.Visible;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);

            if(!state)
                notifyIcon.Visible = false; 
        }

        private void pbMinimize_Click(object sender, EventArgs e)
        {
            HideForm();
        }

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            pbMinimize.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e)
        {
            pbMinimize.BackColor = pTopTop.BackColor;
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            LoadUI();
        }

        private void fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            LoadUI();
        }

        private void ShowForm()
        {
            this.WindowState = FormWindowState.Normal;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Size.Width - (this.Width + 2), Screen.PrimaryScreen.WorkingArea.Size.Height - (this.Height + 2));
            ShowInTaskbar = true;
            notifyIcon.Visible = false;

            pbMinimize_MouseLeave(null, null);
            timerCheckForTask.Start();
        }

        private void HideForm()
        {
            this.WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            notifyIcon.Visible = true;

            timerCheckForTask.Stop();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void timerCheckForTask_Tick(object sender, EventArgs e)
        {
            bool fO = taskFoundOnce;
            taskIsrunning = IsTaskRunning();

            lState.Text = taskIsrunning ? "Running" : taskFoundOnce ? "Exited" : "Not started";
            lState.ForeColor = taskIsrunning ? Color.SeaGreen : taskFoundOnce ? Color.DodgerBlue : Color.Crimson;

            if (!taskIsrunning || fO != taskFoundOnce)
                LoadUI();
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_24dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = pTopTop.BackColor;
            pbClose.Image = UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
        }

        public object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = (object)binForm.Deserialize(memStream);

                return obj;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pbProgress_Click(object sender, EventArgs e)
        {

        }
    }
}
