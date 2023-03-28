using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EAM_Updater
{
    public partial class FrmMain : Form
    {
        private readonly Version version = new Version(1, 0, 0);
        private const string BACKUP_LINK = "https://github.com/MaikEight/ExaltAccountManager/releases/latest";
        private string[] arguments = null;
        public FrmMain(string[] args)
        {
            InitializeComponent();
            lVersion.Text = string.Format(lVersion.Text, version.ToString());

            if (args == null || args.Length == 0)
            {
                MessageBox.Show("No arguments provided, aborting...");
                Environment.Exit(1);
                return;
            }
            arguments = args;

            Thread updateThread = new Thread(UpdateThread);
            updateThread.IsBackground = true;
            updateThread.Start();
        }

        private void UpdateThread()
        {
            if (arguments.Length == 1)
            {
                bool restartRequired = MK_EAM_Updater_Lib.Updater.PerformUpdate(Application.StartupPath, arguments[0]);

                if (restartRequired)
                {
                    Process.Start(Application.ExecutablePath, $"{arguments[0]} ${MK_EAM_Updater_Lib.Updater.tempFilePath}");
                    Environment.Exit(0);
                }
                else
                {
                    MK_EAM_Updater_Lib.Updater.CleanupTemp(MK_EAM_Updater_Lib.Updater.tempFilePath);

                    string eamPath = Path.Combine(Application.StartupPath, "ExaltAccountManager.exe");                    
                    if (File.Exists(eamPath))
                    {
                        Process.Start(eamPath, "update");
                    }
                    Environment.Exit(0);
                }
            }
            if (arguments.Length >= 2)
            {
                int tries = 0;
                if (arguments.Length == 3)
                {
                    int.TryParse(arguments[2], out tries);
                }
                tries++;
                if (tries >= 3)
                {
                    MessageBox.Show("Failed to update, please update manually.");
                    Process.Start(BACKUP_LINK);
                    Environment.Exit(1);
                    return;
                }
                bool restartRequired = MK_EAM_Updater_Lib.Updater.MoveUpdateFiles(arguments[0], arguments[1]);

                if (restartRequired)
                {
                    Process.Start(Application.StartupPath, $"{arguments[0]} ${MK_EAM_Updater_Lib.Updater.tempFilePath} ${tries.ToString()}");
                    Environment.Exit(0);
                    return;
                }
                MK_EAM_Updater_Lib.Updater.CleanupTemp(MK_EAM_Updater_Lib.Updater.tempFilePath);
                Process.Start(Path.Combine(Application.StartupPath, "ExaltAccountManager.exe"), "update");
                Environment.Exit(0);
            }
        }
    }
}
