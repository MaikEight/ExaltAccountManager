using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Linq;

namespace EAM_Task_Tools
{
    internal class Program
    {
        private const string taskName = "Exalt Account Manager Daily Login Task V2";
        private const string taskDesc = "The EAM Daily Login Task runs the current Unity client in the background (silent) and loggs throught each account in the list, that is checked for the daily login. For more information about EAM visit https://github.com/MaikEight/ExaltAccountManager";
        private const string taskAuthor = "Maik8";

        /// <summary>
        /// Exit codes:
        /// 0 - Success
        /// 1 - No installation mode or path to daily login application provided
        /// 2 - Invalid mode
        /// 3 - Error un-/installing task
        /// 4 - Invalid path to daily login application
        /// 5 - Task already installed
        /// 6 - Task not installed
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Please provide the installation mode and path to the daily login application as an argument.");
                Environment.Exit(1);
                return;
            }

            if (args[0] == "check")
            {
                bool isInstalled = IsEamTaskInstalled();
                Console.WriteLine(isInstalled ? "Task is installed." : "Task is not installed.");
                Environment.Exit(isInstalled ? 0 : 6);
            }
            
            string mode = args[0];
            switch (mode)
            {
                case "install":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please provide the installation mode and path to the daily login application as an argument.");
                        Environment.Exit(1);
                        return;
                    }
                    string path = args[1];
                    InstallEamTask(path);
                    break;
                case "uninstall":
                    UninstallEamTask();
                    break;
                default:
                    Console.WriteLine("Invalid mode. Please use 'install' or 'uninstall'.");
                    Environment.Exit(2);
                    break;
            }

        }

        private static bool IsEamTaskInstalled()
        {
            try
            {
                using (TaskService service = new TaskService())
                {
                    return service.RootFolder.AllTasks.Any(t => t.Name == taskName);
                }
            }
            catch
            {
                return false;
            }
        }

        private static void InstallEamTask(string taskpath)
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

                        int triggerHour = utcOffset.Hours > 0 ? utcOffset.Hours : 24 - utcOffset.Hours;
                        int triggerMinute = 0;
                        bool triggerSuccess = false;

                        try
                        {
                            triggerSuccess = (triggerHour >= 0 && triggerMinute >= 0);
                        }
                        catch
                        {
                            triggerSuccess = false;
                        }

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
                        }
                        if (File.Exists(taskpath))
                        {
                            try
                            {
                                task.Actions.Add(new ExecAction(taskpath, "", Path.GetDirectoryName(taskpath)));
                                service.RootFolder.RegisterTaskDefinition(taskName, task);
                                Console.WriteLine("Task installed successfully.");
                                Environment.Exit(0);
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

        private static void UninstallEamTask()
        {
            bool taskInstalled = IsEamTaskInstalled();
            if (!taskInstalled)
            {
                Console.WriteLine("Task not installed.");
                Environment.Exit(6);
            }

            using (TaskService service = new TaskService())
            {
                try
                {
                    if (service.RootFolder.AllTasks.Any(t => t.Name == taskName))
                    {
                        try
                        {
                            Console.WriteLine("Uninstalling task...");
                            Microsoft.Win32.TaskScheduler.Task s = service.RootFolder.AllTasks.Where(t => t.Name == taskName).First();
                            service.RootFolder.DeleteTask(taskName, false);

                            Console.WriteLine("Task uninstalled successfully.");
                            Environment.Exit(0);
                        }
                        catch
                        {
                            Console.WriteLine("Error uninstalling task.");
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
