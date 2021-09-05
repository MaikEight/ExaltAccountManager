using ExaltAccountManager;
using MK_EAM_Lib;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace EAM_Tasktray_Tool
{
    public partial class FrmTaskTray : Form
    {
        Pen p = new Pen(Color.Black);
        private static string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        string pathEAMConfig = Path.Combine(saveFilePath, "EAM.options");
        string pathDailyLoginsConfig = Path.Combine(saveFilePath, "EAM.DailyLogins");
        string pathNotificationConfig = Path.Combine(saveFilePath, "EAM.NotificationOptions");
        OptionsData options;
        NotificationOptions notOptions;
        DateTime lastUpdate;
        int lastErrors = 0;

        public FrmTaskTray()
        {
            InitializeComponent();
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;

            LoadEAMConfig();
            LoadNotificationConfig();
            fileSystemWatcher.Path = saveFilePath;
            fileSystemWatcher.Filter = "EAM.DailyLogins";
            UpdateDailyLogins();

            try
            {
                DailyLogins logins = (DailyLogins)ByteArrayToObject(File.ReadAllBytes(pathDailyLoginsConfig));
                timerClose.Interval = (int)((logins.logins.Count * (notOptions.joinTime + notOptions.killTime)) * 1.5f) * 1000;
                if (timerClose.Interval < 15000) timerClose.Interval = 15000;

                timerClose.Start();
            }
            catch { }
        }

        private void UpdateDailyLogins()
        {
            try
            {
                if (File.Exists(pathDailyLoginsConfig))
                {
                    DailyLogins logins = (DailyLogins)ByteArrayToObject(File.ReadAllBytes(pathDailyLoginsConfig));

                    if (logins.isDone)
                    {
                        if (notOptions.useNotifications && notOptions.showNotificationOnDone)
                        {
                            int amount = logins.logins.Where(o => (o.lastState == 1 && o.lastLogin.Date == DateTime.Now.Date)).Count();
                            int error = logins.logins.Where(o => (o.lastState == 2 && o.lastLogin.Date == DateTime.Now.Date)).Count();

                            string msg = $"Successfully logged into {amount} / {logins.logins.Count} accounts.";
                            if (error > 0)
                                msg = $"Successfully logged into {amount} / {logins.logins.Count} accounts. {error} failed!";

                            ShowNotification("Daily logins done", msg);
                        }

                        progressBar.Maximum = logins.logins.Count;
                        progressBar.Value = logins.logins.Count;
                        lProgress.Text = $"{progressBar.Value} / {progressBar.Maximum}";
                        lText.Text = "Daily login finished, closing tool soon";

                        //Exit Application in 15 seconds.    
                        timerClose.Stop();

                        timerClose.Interval = 15000;
                        timerClose.Start();

                        return;
                    }

                    if (lastUpdate == null || lastUpdate < logins.lastUpdate)
                    {
                        if (!lText.Visible)
                            lText.Visible = true;

                        int amount = logins.logins.Where(o => (o.lastState == 1 && o.lastLogin.Date == DateTime.Now.Date)).Count();

                        progressBar.Maximum = logins.logins.Count;
                        progressBar.Value = (amount <= progressBar.Maximum && amount >= 0) ? amount : (amount > progressBar.Maximum) ? progressBar.Maximum : 0;

                        lProgress.Text = $"{progressBar.Value} / {progressBar.Maximum}";

                        lastUpdate = logins.lastUpdate;

                        if (notOptions.useNotifications && notOptions.showNotificationOnError)
                        {
                            try
                            {
                                var errors = logins.logins.Where(o => (o.lastState == 2 && o.lastLogin.Date == DateTime.Now.Date));
                                if (errors.Count() > lastErrors)
                                {
                                    lastErrors = errors.Count();

                                    ShowNotification("Daily Login Error", $"Failed at account: {errors.Last().mail}", 3);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
        }

        private void ShowNotification(string title, string text, int icon = 0)
        {
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = text;
            notifyIcon.BalloonTipIcon = (ToolTipIcon)icon;

            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);
        }

        private void ApplyTheme()
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(255, 255, 255);
            Color font = Color.Black;

            progressBar.ForeColor = Color.FromArgb(32, 32, 32);
            progressBar.BackColor = Color.FromArgb(0, 0, 0);

            if (options.useDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
                p = new Pen(Color.White);

                progressBar.ForeColor = Color.Gainsboro;
                progressBar.BackColor = Color.Black;
            }

            this.BackColor = def;
            this.ForeColor = font;
            lText.ForeColor = lProgress.ForeColor = lHeader.ForeColor = font;

            if (options.useDarkmode)
            {
                pbLogo.Image = Properties.Resources.ic_account_balance_wallet_white_48dp;
            }
            else
            {
                pbLogo.Image = Properties.Resources.ic_account_balance_wallet_black_48dp;
            }

            this.Invalidate();
        }

        private void LoadEAMConfig()
        {
            try
            {
                if (File.Exists(pathEAMConfig))
                    options = (OptionsData)ByteArrayToObject(File.ReadAllBytes(pathEAMConfig));

                if (options != null)
                {
                    ApplyTheme();
                }
            }
            catch { }
        }

        private void LoadNotificationConfig()
        {
            try
            {
                if (File.Exists(pathNotificationConfig))
                    notOptions = (NotificationOptions)ByteArrayToObject(File.ReadAllBytes(pathNotificationConfig));
                else
                {
                    notOptions = new NotificationOptions();
                    File.WriteAllBytes(pathNotificationConfig, ObjectToByteArray(notOptions));
                }

                if (!notOptions.useTaskTrayTool) //WTF!?
                {
                    this.Close();
                    Environment.Exit(0);
                    return;
                }

                if (notOptions.useNotifications)
                {
                    if (notOptions.showNotificationOnStart)
                        ShowNotification("EAM Daily Login Started", " ", 0);

                    int daysLeft = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day;
                    if (daysLeft < 2)
                        ShowNotification("Monthly reminder", "Please keep in mind, that you need to claim your login-rewards manually till the end of this month.", 0);
                }
            }
            catch { }
        }

        public void ShowForm()
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Size.Width - (this.Width + 0), Screen.PrimaryScreen.WorkingArea.Size.Height - (this.Height + 0));
            this.TopMost = true;
        }

        public void HideForm()
        {
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
            Hide();
            notifyIcon.Visible = true;
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void FrmTaskTray_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        private void pbMinimize_Click(object sender, EventArgs e)
        {
            HideForm();
        }

        private void FrmTaskTray_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                notifyIcon.Visible = false;
                notifyIcon.Icon = null;
            }
            catch { }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);
        }

        public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            if (options != null && options.useDarkmode)
                pbMinimize.BackColor = Color.DimGray;
            else
                pbMinimize.BackColor = Color.DarkGray;
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e) => pbMinimize.BackColor = Color.Transparent;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            UpdateDailyLogins();
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            timerClose.Stop();
            Environment.Exit(0);
        }
    }
}
