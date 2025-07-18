using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Linq;

namespace EAM_Task_Tools
{
    internal sealed class Program
    {
        private static readonly Version version = new Version(1, 1, 0);
        private const string taskNameV1 = "Exalt Account Manager Daily Login Task";
        private const string taskNameV2 = "Exalt Account Manager Daily Login Task V2";
        private const string taskName = "Exalt Account Manager Daily Login Task V3";
        private const string taskDesc = "The EAM Daily Login Task runs the current Unity client in the background (silent) and loggs throught each account in the list, that is checked for the daily login. For more information about EAM visit https://github.com/MaikEight/ExaltAccountManager";
        private const string taskAuthor = "Maik8";

        private const string availableModes = "check, checkV1, checkV2, checkForOldVersions, install, uninstall, uninstallV1, uninstallV2, uninstallOldVersions, manual";
        /// <summary>
        /// Exit codes:
        /// 0 - Success
        /// 1 - No mode provided
        /// 2 - Invalid mode
        /// 3 - Error un-/installing task
        /// 4 - Invalid path to daily login application
        /// 5 - Task already installed
        /// 6 - Task not installed
        /// 7 - Permission denied
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "help" };
            }

            string mode = args[0];
            switch (mode)
            {
                case "check":
                    bool isInstalled = IsEamTaskInstalled(taskName);
                    Console.WriteLine(isInstalled ? "Task is installed." : "Task is not installed.");
                    Environment.Exit(isInstalled ? 0 : 6);
                    break;
                case "checkV1":
                    {
                        bool isInstalledV1 = IsEamTaskInstalled(taskNameV1);
                        Console.WriteLine(isInstalledV1 ? "Task V1 is installed." : "Task is not installed.");
                        Environment.Exit(isInstalledV1 ? 0 : 6);
                    }
                    break;
                case "checkV2":
                    {
                        bool isInstalledV2 = IsEamTaskInstalled(taskNameV2);
                        Console.WriteLine(isInstalledV2 ? "Task V2 is installed." : "Task is not installed.");
                        Environment.Exit(isInstalledV2 ? 0 : 6);
                    }
                    break;
                case "checkForOldVersions":
                    {
                        bool isInstalledV1 = IsEamTaskInstalled(taskNameV1);
                        bool isInstalledV2 = IsEamTaskInstalled(taskNameV2);
                        Console.WriteLine((isInstalledV1 || isInstalledV2) ? "Old Task versions are installed." : "Old Task versions are not installed.");
                        Environment.Exit((isInstalledV1 || isInstalledV2) ? 0 : 6);
                    }
                    break;
                case "install":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please provide the installation mode and path to the daily login application as an argument. You may also provide arguments for the tasked application execution.");
                        Environment.Exit(1);
                        return;
                    }
                    string path = args[1];
                    string arguments = args.Length > 2 ? args[2] : "";
                    bool ignoreFileCheck = args.Length > 3 && args[3] == "ignoreFileCheck";
                    InstallEamTask(path, arguments, ignoreFileCheck);
                    break;
                case "uninstall":
                    UninstallEamTask();
                    Environment.Exit(0);
                    break;
                case "uninstallV1":
                    UninstallEamTask(taskNameV1);
                    Environment.Exit(0);
                    break;
                case "uninstallV2":
                    UninstallEamTask(taskNameV2);
                    Environment.Exit(0);
                    break;
                case "uninstallOldVersions":
                    {
                        bool isInstalledV1 = IsEamTaskInstalled(taskNameV1);
                        if (isInstalledV1)
                        {
                            UninstallEamTask(taskNameV1, skipInstalledCheck: true);
                        }
                        bool isInstalledV2 = IsEamTaskInstalled(taskNameV2);
                        if (isInstalledV2)
                        {
                            UninstallEamTask(taskNameV2, skipInstalledCheck: true);
                        }
                        Environment.Exit(0);
                    }
                    break;
                case "manual":
                    System.Diagnostics.Process.Start("taskschd.msc");
                    Environment.Exit(0);
                    break;
                case "help":
                    Console.WriteLine("EAM Task Tools v" + version.ToString() + " by Maik8");
                    Console.WriteLine("Please provide the installation mode and path to the daily login application as an argument.");
                    Console.WriteLine("Usage: EAM_Task_Tools.exe <mode> <args>");
                    Console.WriteLine("Modes: " + availableModes);
                    Environment.Exit(1);
                    break;
                default:
                    Console.WriteLine("Invalid mode. Please provide a valid mode");
                    Console.WriteLine("Modes: " + availableModes);
                    Environment.Exit(2);
                    break;
            }

        }

        private static bool IsEamTaskInstalled(string _taskName = taskName)
        {
            try
            {
                using (TaskService service = new TaskService())
                {
                    return service.RootFolder.AllTasks.Any(t => t.Name == _taskName);
                }
            }
            catch
            {
                return false;
            }
        }

        private static void InstallEamTask(string taskpath, string arguments = "", bool ignoreFileCheck = false)
        {
            try
            {
                bool taskInstalled = IsEamTaskInstalled();
                if (taskInstalled)
                {
                    Console.WriteLine("Task already installed.");
                    Environment.Exit(5);
                }

                using (TaskService service = new TaskService())
                {
                    if (!service.RootFolder.AllTasks.Any(t => t.Name == taskName))
                    {
                        Console.WriteLine("Installing task...");

                        var task = service.NewTask();
                        task.RegistrationInfo.Description = taskDesc;
                        task.RegistrationInfo.Author = taskAuthor;

                        int minutesAfterLoginDelay = 5;

                        var logontrigger = new LogonTrigger
                        {
                            Delay = (minutesAfterLoginDelay > 0) ? new TimeSpan(0, minutesAfterLoginDelay, 0) : new TimeSpan(0),
                            StartBoundary = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 1, 0, 0),
                            Enabled = true,
                            UserId = Environment.UserName
                        };
                        task.Triggers.Add(logontrigger);

                        //Get the difference between the current time and the UTC time
                        DateTime now = DateTime.Now;
                        TimeSpan utcOffset = now - now.ToUniversalTime();

                        // Adjust the trigger hour calculation
                        int triggerHour = (24 + utcOffset.Hours) % 24;
                        int triggerMinute = 1;
                        bool triggerSuccess = triggerHour >= 0 && triggerMinute >= 0;

                        DailyTrigger timeTrigger = null;
                        if (triggerSuccess)
                        {
                            DateTime startBoundary = new DateTime(
                                DateTime.Now.AddDays(-2).Year,
                                DateTime.Now.AddDays(-2).Month,
                                DateTime.Now.AddDays(-2).Day,
                                triggerHour,
                                triggerMinute,
                                10);

                            timeTrigger = new DailyTrigger(1)
                            {
                                DaysInterval = 1,
                                StartBoundary = startBoundary,
                                Enabled = true
                            };
                            task.Triggers.Add(timeTrigger);

                            task.Settings.AllowDemandStart = true;
                            task.Settings.AllowHardTerminate = true;
                            task.Settings.DisallowStartIfOnBatteries = false;
                            task.Settings.IdleSettings.StopOnIdleEnd = false;
                            task.Settings.IdleSettings.RestartOnIdle = false;
                            task.Settings.MultipleInstances = TaskInstancesPolicy.Parallel;
                            task.Settings.RunOnlyIfNetworkAvailable = true;
                            task.Settings.StopIfGoingOnBatteries = false;
                            task.Settings.WakeToRun = true;
                            task.Settings.StartWhenAvailable = true;
                            task.Settings.Enabled = true;
                        }
                        if (ignoreFileCheck || File.Exists(taskpath))
                        {
                            try
                            {
                                task.Actions.Add(new ExecAction(taskpath, arguments, Path.GetDirectoryName(taskpath)));
                                service.RootFolder.RegisterTaskDefinition(taskName, task);
                                Console.WriteLine("Task installed successfully.");
                                Environment.Exit(0);
                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                Console.WriteLine("Error: Access denied. Please run the application as administrator: " + ex.Message);
                                Environment.Exit(7);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error installing task: " + ex.Message);
                                Environment.Exit(3);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: The provided path to the daily login application is invalid.");
                            Environment.Exit(4);
                        }
                    }
                    else
                    {
                        //Task already installed
                        Console.WriteLine("Task already installed.");
                        Environment.Exit(5);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error installing task: " + ex.Message);
                Environment.Exit(3);
            }
        }

        private static void UninstallEamTask(string _taskName = taskName, bool skipInstalledCheck = false)
        {
            bool taskInstalled = IsEamTaskInstalled(_taskName);
            if (!taskInstalled)
            {
                Console.WriteLine("Task not installed.");
                Environment.Exit(6);
            }

            using (TaskService service = new TaskService())
            {
                try
                {
                    if (service.RootFolder.AllTasks.Any(t => t.Name == _taskName))
                    {
                        try
                        {
                            Console.WriteLine("Uninstalling task...");
                            service.RootFolder.DeleteTask(_taskName, false);

                            Console.WriteLine("Task uninstalled successfully.");                            
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            Console.WriteLine("Error: Access denied. Please run the application as administrator: " + ex.Message);
                            Environment.Exit(7);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error uninstalling task: " + ex.Message);
                            Environment.Exit(3);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error uninstalling task.");
                    Environment.Exit(3);
                }
            }
        }
    }
}
