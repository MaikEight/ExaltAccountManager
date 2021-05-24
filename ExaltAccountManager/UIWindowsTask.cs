using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using MK_EAM_Lib;

namespace ExaltAccountManager
{
    public partial class UIWindowsTask : UserControl
    {
        private string taskName = "Exalt Account Manager Daily Login Task";
        private string taskDesc = "The EAM Daily Login Task runs the current Unity client in the background (silent) and loggs throught each account in the list, that is checked for the daily login.";
        private string taskAuthor = "Maik8";
        private string taskpath = Path.Combine(Application.StartupPath, "DailyService", "EAM Daily Login Service.exe");

        FrmDailyLoginOptions frmLogins;
        private bool isTaskUpdate = false;

        public UIWindowsTask(FrmDailyLoginOptions _frmLogins)
        {
            InitializeComponent();
            frmLogins = _frmLogins;
            ApplyTheme(frmLogins._isDarkmode);

            GetTaskData();
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.ForeColor = font;
                this.BackColor = second;

                lHeader.ForeColor = lBack.ForeColor = lInfo.ForeColor = lIsInstalled.ForeColor = lTime.ForeColor = lWaitMinutes.ForeColor = font;

                lIsInstalled.BackColor = def;

                pbBack.Image = Properties.Resources.ic_arrow_back_white_24dp;

                btnInstallTask.Image = Properties.Resources.ic_build_white_18dp;
                btnInstallTask.FlatAppearance.MouseOverBackColor = btnUninstallTask.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 225, 225, 225);
            }
        }

        private void GetTaskData()
        {
            using (TaskService service = new TaskService())
            {
                try
                {
                    if (!service.RootFolder.AllTasks.Any(t => t.Name == taskName))
                    {
                        //Does not exist!
                        lIsInstalled.Text = "Daily-login task is NOT installed!";
                        lIsInstalled.ForeColor = Color.Crimson;
                        btnInstallTask.Text = "Install Daily Login Task";
                        btnUninstallTask.Visible = false;
                    }
                    else
                    {
                        lIsInstalled.Text = "Daily-login task is installed!";
                        lIsInstalled.ForeColor = Color.Green;
                        btnInstallTask.Text = "Update Task with new values above";
                        btnUninstallTask.Visible = true;

                        Microsoft.Win32.TaskScheduler.Task s = service.RootFolder.AllTasks.Where(t => t.Name == taskName).First();
                        bool foundTrigger = false;
                        foreach (var tr in s.Definition.Triggers)
                        {
                            if (tr.GetType().ToString().Contains("LogonTrigger"))
                            {
                                LogonTrigger ltr = tr as LogonTrigger;
                                tbWaitMinutes.Text = ltr.Delay.Minutes.ToString();
                                foundTrigger = true;
                            }
                            else if (tr.GetType().ToString().Contains("DailyTrigger"))
                            {
                                DailyTrigger dtr = tr as DailyTrigger;
                                string h = (dtr.StartBoundary.Hour < 10) ? $"0{dtr.StartBoundary.Hour}" : dtr.StartBoundary.Hour.ToString();
                                string m = (dtr.StartBoundary.Minute < 10) ? $"0{dtr.StartBoundary.Minute}" : dtr.StartBoundary.Minute.ToString();
                                tbResetTime.Text = $"{h}:{m}";
                            }
                        }
                        if (!foundTrigger)
                        {
                            tbWaitMinutes.Text = "-1";
                        }
                    }
                }
                catch 
                {
                    frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.GetTaskData, $"Failed to get Tasnk data."));
                }
            }
        }

        private void UIWindowsTask_Load(object sender, EventArgs e)
        {

        }

        private void btnInstallTask_Click(object sender, EventArgs e)
        {
            try
            {
                using (TaskService service = new TaskService())
                {
                    if (!service.RootFolder.AllTasks.Any(t => t.Name == taskName))
                    {
                        frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.InstallTask, $"Installing task..."));
                        lIsInstalled.ForeColor = Color.Green;
                        lIsInstalled.Text = "Installing...";
                        isTaskUpdate = false;

                        var task = service.NewTask();
                        task.RegistrationInfo.Description = taskDesc;
                        task.RegistrationInfo.Author = taskAuthor;

                        int de = 0;
                        int.TryParse(tbWaitMinutes.Text, out de);

                        var logontrigger = new LogonTrigger
                        {
                            Delay = (de > 0) ? new TimeSpan(0, de, 0) : new TimeSpan(0),
                            StartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-1).Day , 1, 0, 0),
                            Enabled = true,
                            UserId = Environment.UserName
                        };
                        task.Triggers.Add(logontrigger);

                        int triggerHour = 0;
                        int triggerMinute = 0;
                        bool triggerSuccess = false;

                        try
                        {
                            string[] triggerTexts = tbResetTime.Text.Split(':');
                            int.TryParse(triggerTexts[0], out triggerHour);
                            int.TryParse(triggerTexts[1], out triggerMinute);
                            triggerSuccess = (triggerHour >= 0 && triggerMinute >= 0);
                        }
                        catch
                        {
                            triggerSuccess = false;
                        }
                        DailyTrigger timeTrigger = null;
                        if (triggerSuccess)
                        {
                            timeTrigger = new DailyTrigger(1)
                            {
                                DaysInterval = 1,
                                StartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-1).Day, triggerHour, triggerMinute, 0),
                                Enabled = true
                            };
                            task.Triggers.Add(timeTrigger);
                        }
                        if (File.Exists(taskpath))
                        {
                            try
                            {
                                task.Actions.Add(new ExecAction(taskpath, "", Path.GetDirectoryName(taskpath)));
                                service.RootFolder.RegisterTaskDefinition(taskName, task);
                                btnUninstallTask.Visible = true;

                                frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.InstallTask, $"Task successfully installed."));

                                if (timeTrigger != null)
                                    frmLogins.SaveTimingData(timeTrigger.StartBoundary);
                                else                                
                                    frmLogins.SaveTimingData(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-1).Day, 0, 0, 1));                                
                            }
                            catch (Exception ex)
                            {
                                frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.Error, $"Task could NOT be installed correctly."));
                                MessageBox.Show(this, $"Task could NOT be installed correctly. Help can also be found at MPGH.net from Maik8.{Environment.NewLine}Error Code{Environment.NewLine}{ex.Message}", "Task installation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            lIsInstalled.Text = "Executable NOT found.";
                            lIsInstalled.ForeColor = Color.Crimson;

                            frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.Error, $"Task could NOT be installed correctly."));
                            MessageBox.Show(this, $"Task could NOT be installed correctly. The file {taskpath} could not be found. Please ensure it is there and try again. Help can also be found at MPGH.net from Maik8.", "Task installation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Task already installed, updating it...
                        frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.UpdateTask, $"Updating Task ..."));
                        isTaskUpdate = true;
                        lIsInstalled.ForeColor = Color.Green;
                        lIsInstalled.Text = "Updateing...";
                        btnUninstallTask_Click(btnUninstallTask, null);
                    }
                }
            }
            catch { }

            try
            {
                GetTaskData();
            }
            catch { }
        }

        private void btnUninstallTask_Click(object sender, EventArgs e)
        {
            bool updatedData = false;

            using (TaskService service = new TaskService())
            {
                try
                {
                    if (service.RootFolder.AllTasks.Any(t => t.Name == taskName))
                    {
                        try
                        {
                            frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.RemoveTask, $"Removing Task ..."));
                            Microsoft.Win32.TaskScheduler.Task s = service.RootFolder.AllTasks.Where(t => t.Name == taskName).First();
                            service.RootFolder.DeleteTask(taskName, false);

                            if (isTaskUpdate)
                            {
                                btnInstallTask_Click(btnInstallTask, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            frmLogins.LogEvent(new LogData(0, "EAM Task", LogEventType.Error, $"Failed to remove Task."));
                            MessageBox.Show(this, $"Task was NOT uninstalled correctly. Try to do it manually, via the Windows Task Scheduler. Help can also be found at MPGH.net from Maik8. {Environment.NewLine}Error Code{Environment.NewLine}{ex.Message}", "Task installation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Does not exist!?       
                        if (isTaskUpdate)
                        {
                            btnInstallTask_Click(btnInstallTask, null);
                        }
                    }
                    GetTaskData();
                    updatedData = true;
                }
                catch { }
            }
            try
            {
                if (!updatedData)
                    GetTaskData();
            }
            catch { }
        }

        private void pbBack_MouseEnter(object sender, EventArgs e)
        {
            if (frmLogins._isDarkmode)
                pbBack.BackColor = Color.DimGray;
            else
                pbBack.BackColor = Color.DarkGray;
        }

        private void pbBack_MouseLeave(object sender, EventArgs e) => pbBack.BackColor = Color.Transparent;

        private void pbBack_Click(object sender, EventArgs e) => frmLogins.CloseWindowsTask();
    }
}
