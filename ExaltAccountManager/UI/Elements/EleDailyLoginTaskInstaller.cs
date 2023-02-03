using Microsoft.Win32.TaskScheduler;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleDailyLoginTaskInstaller : UserControl
    {
        private FrmMain frm;
        private bool isTaskInstalled = false;

        private string taskName = "Exalt Account Manager Daily Login Task";
        private string taskDesc = "The EAM Daily Login Task runs the current Unity client in the background (silent) and loggs throught each account in the list, that is checked for the daily login.";
        private string taskAuthor = "Maik8";
        private string taskpath = Path.Combine(Application.StartupPath, "DailyService", "EAM Daily Login Service.exe");

        public EleDailyLoginTaskInstaller(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);

            GetTaskData();
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            lHeadline.Focus();

            tbMinutesTillStart.ResetColors();
            tbNewDayTime.ResetColors();

            this.BackColor =
            tbMinutesTillStart.BackColor = tbMinutesTillStart.FillColor = tbMinutesTillStart.OnIdleState.FillColor = tbMinutesTillStart.OnHoverState.FillColor = tbMinutesTillStart.OnActiveState.FillColor = tbMinutesTillStart.OnDisabledState.FillColor =
            tbNewDayTime.BackColor = tbNewDayTime.FillColor = tbNewDayTime.OnIdleState.FillColor = tbNewDayTime.OnHoverState.FillColor = tbNewDayTime.OnActiveState.FillColor = tbNewDayTime.OnDisabledState.FillColor =
            def;

            this.ForeColor =
            tbMinutesTillStart.ForeColor = tbMinutesTillStart.OnActiveState.ForeColor = tbMinutesTillStart.OnDisabledState.ForeColor = tbMinutesTillStart.OnHoverState.ForeColor = tbMinutesTillStart.OnIdleState.ForeColor =
            tbNewDayTime.ForeColor = tbNewDayTime.OnActiveState.ForeColor = tbNewDayTime.OnDisabledState.ForeColor = tbNewDayTime.OnHoverState.ForeColor = tbNewDayTime.OnIdleState.ForeColor =
            font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            pbTaskState.Image = isTaskInstalled ? (frm.UseDarkmode ? Properties.Resources.ok_white_36px : Properties.Resources.ok_36px) : (frm.UseDarkmode ? Properties.Resources.ic_do_not_disturb_white_36dp : Properties.Resources.ic_do_not_disturb_black_36dp);
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e) => frm.RemoveShadowForm();

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_18dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = frm.UseDarkmode ? ColorScheme.GetColorSecond(frm.UseDarkmode) : ColorScheme.GetColorThird(frm.UseDarkmode);
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;
        }

        private void pbClose_MouseDown(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Red;

        #endregion

        private bool isTaskUpdate = false;
        private void btnEditTask_Click(object sender, EventArgs e)
        {
            try
            {
                using (TaskService service = new TaskService())
                {
                    if (!service.RootFolder.AllTasks.Any(t => t.Name == taskName))
                    {
                        //frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.GetTaskData, "Failed to get Task data."));
                        //frm.ShowSnackbar("Failed to get Task data.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                        frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.InstallTask, "Installing task..."));
                        lTaskState.ForeColor = Color.Green;
                        lTaskState.Text = "Installing...";
                        isTaskUpdate = false;

                        var task = service.NewTask();
                        task.RegistrationInfo.Description = taskDesc;
                        task.RegistrationInfo.Author = taskAuthor;

                        int de = 0;
                        int.TryParse(tbMinutesTillStart.Text, out de);

                        var logontrigger = new LogonTrigger
                        {
                            Delay = (de > 0) ? new TimeSpan(0, de, 0) : new TimeSpan(0),
                            StartBoundary = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 1, 0, 0),
                            Enabled = true,
                            UserId = Environment.UserName
                        };
                        task.Triggers.Add(logontrigger);

                        int triggerHour = 0;
                        int triggerMinute = 0;
                        bool triggerSuccess = false;

                        try
                        {
                            string[] triggerTexts = tbNewDayTime.Text.Split(':');
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
                                StartBoundary = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).AddDays(-1).Day, triggerHour, triggerMinute, 0),
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
                                btnUninstall.Visible = true;

                                frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.InstallTask, "Task successfully installed."));
                            }
                            catch (Exception ex)
                            {
                                frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.Error, "Task could not be installed correctly."));
                                frm.ShowSnackbar($"Task could not be installed correctly.{Environment.NewLine}Help can be found via discord or email.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 7500);
                            }
                        }
                        else
                        {
                            lTaskState.Text = "Executable NOT found.";
                            lTaskState.ForeColor = Color.Crimson;

                            frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.Error, "Task could not be installed correctly."));
                            frm.ShowSnackbar($"Task could not be installed correctly.{Environment.NewLine}Please ensure the executable is there and try again.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 7500);
                        }
                    }
                    else
                    {
                        //Task already installed, updating it...
                        frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.Error, "Updating Task ..."));
                        isTaskUpdate = true;
                        lTaskState.ForeColor = Color.Green;
                        lTaskState.Text = "Updateing...";
                        btnUninstall_Click(btnUninstall, null);
                    }
                }
            }
            catch (Exception ex) { }

            try
            {
                GetTaskData();
            }
            catch { }

            lTaskState.Focus();
        }

        /*
            ########################################################################
            ############################+--TASK-STUFF--+############################
            ########################################################################
        */
        private void GetTaskData()
        {
            using (TaskService service = new TaskService())
            {
                try
                {
                    if (!service.RootFolder.AllTasks.Any(t => t.Name == taskName))
                    {
                        isTaskInstalled = false;

                        //Does not exist!
                        lTaskState.Text = "The task is not installed.";
                        lTaskState.ForeColor = Color.Crimson;
                        btnEditTask.Text = "Install task";
                        btnUninstall.Visible = false;
                    }
                    else
                    {
                        isTaskInstalled = true;

                        lTaskState.Text = "The task is installed!";
                        lTaskState.ForeColor = Color.Green;
                        btnEditTask.Text = "Update Task with new timing values";
                        btnUninstall.Visible = true;

                        Microsoft.Win32.TaskScheduler.Task s = service.RootFolder.AllTasks.Where(t => t.Name == taskName).First();
                        bool foundTrigger = false;
                        foreach (var tr in s.Definition.Triggers)
                        {
                            if (tr.GetType().ToString().Contains("LogonTrigger"))
                            {
                                LogonTrigger ltr = tr as LogonTrigger;
                                tbMinutesTillStart.Text = ltr.Delay.Minutes.ToString();
                                foundTrigger = true;
                            }
                            else if (tr.GetType().ToString().Contains("DailyTrigger"))
                            {
                                DailyTrigger dtr = tr as DailyTrigger;
                                string h = (dtr.StartBoundary.Hour < 10) ? $"0{dtr.StartBoundary.Hour}" : dtr.StartBoundary.Hour.ToString();
                                string m = (dtr.StartBoundary.Minute < 10) ? $"0{dtr.StartBoundary.Minute}" : dtr.StartBoundary.Minute.ToString();
                                tbNewDayTime.Text = $"{h}:{m}";
                            }
                        }
                        if (!foundTrigger)
                        {
                            tbMinutesTillStart.Text = "-1";
                        }
                    }
                }
                catch
                {
                    frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.EAMError, "Failed to get Task data."));
                    frm.ShowSnackbar("Failed to get Task data.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                }
            }
            pbTaskState.Image = isTaskInstalled ? (frm.UseDarkmode ? Properties.Resources.ok_white_36px : Properties.Resources.ok_36px) : (frm.UseDarkmode ? Properties.Resources.ic_do_not_disturb_white_36dp : Properties.Resources.ic_do_not_disturb_black_36dp);
        }

        private void btnUninstall_Click(object sender, EventArgs e)
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
                            frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.RemoveTask, "Removing Task ..."));
                            Microsoft.Win32.TaskScheduler.Task s = service.RootFolder.AllTasks.Where(t => t.Name == taskName).First();
                            service.RootFolder.DeleteTask(taskName, false);

                            if (isTaskUpdate)
                            {
                                btnEditTask_Click(btnEditTask, null);
                            }
                        }
                        catch
                        {
                            frm.LogEvent(new MK_EAM_Lib.LogData(-1, "EAM Task", MK_EAM_Lib.LogEventType.RemoveTask, "Failed to remove Task."));
                            frm.ShowSnackbar("Task was not uninstalled correctly. Try to do it manually, via the Windows Task Scheduler.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 12500);                            
                        }
                    }
                    else
                    {
                        //Does not exist!?       
                        if (isTaskUpdate)
                        {
                            btnEditTask_Click(btnEditTask, null);
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
    }
}
