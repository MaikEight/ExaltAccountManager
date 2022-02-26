using MK_EAM_Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public partial class UIDailyLogins : UserControl
    {
        FrmMain frm;

        DailyLogins dailyLogins = new DailyLogins();

        public UIDailyLogins(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);

            if (File.Exists(frm.dailyLoginsPath))
            {
                try
                {
                    using (StreamReader file = File.OpenText(frm.dailyLoginsPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        dailyLogins = (DailyLogins)serializer.Deserialize(file, typeof(DailyLogins));
                    }
                }
                catch
                {
                    frm.LogEvent(new LogData(-1, "EAM Daily Logins", LogEventType.EAMError, "Failed to load daily logins data."));
                    frm.ShowSnackbar("Failed to load daily logins data.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                }
            }
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);


            this.BackColor = def;
            this.ForeColor =
            chartCanvas.ForeColor = chartCanvas.YAxesForeColor = chartCanvas.XAxesForeColor = chartCanvas.XAxesLabelForeColor = chartCanvas.YAxesLabelForeColor =
            font;
            bunifuCards.BackColor = chartCanvas.BackColor = second;

            chartCanvas.XAxesGridColor = chartCanvas.YAxesGridColor = frm.UseDarkmode ? Color.FromArgb(100, 128, 128, 128) : Color.FromArgb(100, 0, 0, 0);

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in pTop.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
                ApplyThemeToShadowPanel(shadow, ref def, ref second);
            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in pBottom.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
                ApplyThemeToShadowPanel(shadow, ref def, ref second);

            LoadUI();
        }

        private void ApplyThemeToShadowPanel(Bunifu.UI.WinForms.BunifuShadowPanel shadow, ref Color def, ref Color second)
        {
            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;

            shadow.BorderColor = second;
            shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
        }

        private void UIDailyLogins_Load(object sender, EventArgs e)
        {
            timerCheckForTask_Tick(timerCheckForTask, null);
            timerCheckForTask.Start();
        }

        private void LoadUI()
        {
            chartCanvas.Labels = GetDaysLabels();

            List<double> success = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
            List<double> failed = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };

            if (dailyLogins.DailyDatas.Count > 0)
            {
                dailyLogins.DailyDatas = dailyLogins.DailyDatas.OrderByDescending(x => x.Date).ToList();
                lLastRun.Text = $"Date:  {dailyLogins.DailyDatas[0].Date.ToString("dd.MM.yyyy")}{Environment.NewLine}Time: {dailyLogins.DailyDatas[0].StartTime.ToString("HH:mm")} - {dailyLogins.DailyDatas[0].EndTime.ToString("HH:mm")}{Environment.NewLine}Took: {Math.Floor(dailyLogins.DailyDatas[0].DurationInSeconds / 60d)} minutes";

                for (int i = 0; i < 7; i++)
                {
                    DailyData data = dailyLogins.DailyDatas.Where(d => d.Date.Date == DateTime.Now.AddDays(-i).Date).FirstOrDefault();
                    if (data == null)
                        continue;

                    for (int x = 0; x < data.AccountData.Count; x++)
                    {
                        if (data.AccountData[x].Success)
                            success[i]++;
                        else
                            failed[i]++;
                    }
                }

                lLastResults.Text = $"Last run: {success[0]} / {dailyLogins.DailyDatas[0].AccountData.Count} successfull";
                btnShowDetails.Enabled = true;
            }
            else
            {
                lLastRun.Text = "Never";
                lLastResults.Text = "N/A";
                //btnShowDetails.Enabled = false;
            }

            barChartSuccess.Data = success;
            barChartFailed.Data = failed;

            timerAnimate.Start();
        }

        private void timerAnimate_Tick(object sender, EventArgs e)
        {
            timerAnimate.Stop();

            barChartSuccess.TargetCanvas = chartCanvas;
            barChartFailed.TargetCanvas = chartCanvas;
            chartCanvas.Refresh();
        }

        private string[] GetDaysLabels()
        {
            string[] labels = new string[7];

            labels[0] = "Today";
            for (int i = 1; i < labels.Length; i++)
            {
                labels[i] = DateTime.Now.AddDays(-i).DayOfWeek.ToString();
            }

            return labels;
        }

        private void btnNotificationSettings_Click(object sender, EventArgs e)
        {
            frm.ShowShadowForm(new Elements.EleDailyLoginsNotificationsettings(frm));
        }

        private void btnTimingSettings_Click(object sender, EventArgs e)
        {
            frm.ShowShadowForm(new Elements.EleDailyLoginsTimingSettings(frm));
        }

        private void btnTaskScheduler_Click(object sender, EventArgs e)
        {
            frm.ShowShadowForm(new Elements.EleDailyLoginTaskInstaller(frm));
        }

        private void btnRunTaskNOW_Click(object sender, EventArgs e)
        {
            if (!btnRunTaskAll.Visible)
            {
                btnRunTaskAll.Location = new Point(12, 12);
                btnRunTaskAll.Visible = true;
                btnRunTaskAll.BringToFront();
                btnRunTaskNOW.Text = "Run task now";
                return;
            }

            StartTask();
        }

        private void btnRunTaskAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(frm.dailyLoginsPath))
                {
                    try
                    {
                        using (StreamReader file = File.OpenText(frm.dailyLoginsPath))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            dailyLogins = (DailyLogins)serializer.Deserialize(file, typeof(DailyLogins));
                        }
                    }
                    catch
                    {
                        frm.LogEvent(new LogData("EAM Daily Logins", LogEventType.EAMError, "Failed to load daily logins data."));
                        frm.ShowSnackbar("Failed to load daily logins data.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                    }
                }
                dailyLogins.RefreshAll = true;
                using (StreamWriter file = File.CreateText(frm.dailyLoginsPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, dailyLogins);
                }

                StartTask();
            }
            catch (Exception)
            {
                frm.LogEvent(new LogData("EAM Daily Logins", LogEventType.EAMError, "Failed to start task."));
                frm.ShowSnackbar("Failed to start task.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void StartTask()
        {
            timerCheckForTask.Stop();

            btnRunTaskAll.Enabled = false;
            btnRunTaskNOW.Enabled = false;
            try
            {
                if (File.Exists(frm.dailyServiceExePath))
                {
                    ProcessStartInfo info = new ProcessStartInfo()
                    {
                        FileName = frm.dailyServiceExePath,
                        Arguments = "force"
                    };
                    Process.Start(info);

                    frm.LogEvent(new LogData("EAM", LogEventType.ServiceStart, "Daily Login Service started manually."));
                    frm.ShowSnackbar("Daily Login Service started.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000);
                }
                else
                {
                    frm.LogEvent(new LogData("EAM", LogEventType.EAMError, "Failed to find the Daily Login Service executable."));
                    frm.ShowSnackbar("Failed to find the Daily Login Service executable.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                    btnRunTaskAll.Enabled = true;
                }
            }
            catch
            {
                frm.LogEvent(new LogData("EAM", LogEventType.EAMError, "Failed to start the Daily Login Service."));
                frm.ShowSnackbar("Failed to start the Daily Login Service.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                btnRunTaskAll.Enabled = true;
            }

            timerCheckForTask.Start();
        }

        private void btnShowLastDetails_Click(object sender, EventArgs e)
        {
            frm.ShowShadowForm(new Elements.EleDailyLoginsResultsList(frm));
        }

        private void timerCheckForTask_Tick(object sender, EventArgs e)
        {
            btnRunTaskNOW.Enabled =
            btnRunTaskAll.Enabled = !CheckIfTaskIsRunning();
        }

        private bool CheckIfTaskIsRunning()
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName == "EAM Daily Login Service")
                        return true;
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

        private void UIDailyLogins_ParentChanged(object sender, EventArgs e)
        {
            if ((sender as Control).Parent == null)
                timerCheckForTask.Stop();
            else
                timerCheckForTask.Start();
        }
    }
}
