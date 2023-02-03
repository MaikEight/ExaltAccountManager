using MK_EAM_Discord_Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ExaltAccountManager
{
    public static class DiscordHelper
    {
        private const string DEFAULT_STATE = "EAM by Maik8";
        private const string APPLICATION_ID = "1069308775854526506";

        public static DateTime Timestamp
        {
            get => timestamp;
            set
            {
                timestamp = value;

                Discord.Timestamp = timestamp;
            }
        }
        private static DateTime timestamp = DateTime.Now;

        private readonly static DateTime startupTime = DateTime.Now;
        private static DiscordOptions discordOptions;

        private static bool statisticsOpen = false;
        private static bool pingCheckerOpen = false;
        private static bool vaultPeekerOpen = false;

        private static Menu menu;
        private static string lastState = "Starting up...✨";
        
        private static string imageKey { get => frm.UseDarkmode ? "eam_darkmode" : "eam_lightmode"; }

        private static FrmMain frm;
        private static bool updateOnChange = true;
        
        public static void Initialize(DiscordOptions _discordOptions, FrmMain _frm, bool autoEvents = false, bool _updateOnChange = true)
        {
            discordOptions = _discordOptions;
            frm = _frm;
            frm.ThemeChanged += Frm_ThemeChanged;
            updateOnChange = _updateOnChange;

            Discord.Initialize(APPLICATION_ID, autoEvents: autoEvents);
            Discord.UpdateOnChange = false;
            Discord.AddButton("Get Exalt Account Manager here", "https://github.com/MaikEight/ExaltAccountManager/releases/latest");
            
            Discord.Timestamp = startupTime;
            Discord.UseTimestamp = true;
            Discord.Details = "The better rotmg-launcher!💪";
            SetState(discordOptions.ShowState ? "Starting up...✨" : DEFAULT_STATE);
            Discord.LargeImageKey = imageKey;
            Discord.LargeImageText = "Exalt Account Manager";
            Discord.UpdateOnChange = updateOnChange;
            Discord.ApplyPresence();
        }

        private static void Frm_ThemeChanged(object sender, EventArgs e)
        {
            Discord.LargeImageKey = imageKey;
            Discord.ApplyPresence();
        }

        public static void SetLlamaState()
        {
            Discord.State = lastState = "Found the Llama! 🦙";
            Discord.ApplyPresence();
        }

        public static void ApplyPresence() => Discord.ApplyPresence();

        private static string GetLastState()
        {
            return lastState;
        }

        private static void SetState(string state)
        {
            if (statisticsOpen || pingCheckerOpen || vaultPeekerOpen)
            {
                lastState = state;
                return;
            }

            Discord.State = lastState = state;
        }        

        #region Statistics

        public static void OpenedStatistics(bool setOpen = true)
        {
            if (setOpen)
                statisticsOpen = true;

            if (discordOptions.ShowState)
            {
                Discord.State = "Viewing cool statistics 📊";
                Discord.SmallImageKey = "statistics_white";
                Discord.SmallImageText = "Statistics";

                if (!updateOnChange)
                    ApplyPresence();
            }
        }

        public static void ClosedStatistics()
        {
            statisticsOpen = false;

            if (discordOptions.ShowState)
            {
                if (pingCheckerOpen)
                {
                    OpenedPingChecker(false);
                    return;
                }

                if (vaultPeekerOpen)
                {
                    OpenedVaultPeeker(false);
                    return;
                }

                Discord.State = GetLastState();
                Discord.SmallImageKey = null;
                Discord.SmallImageText = null;

                if (!updateOnChange)
                    ApplyPresence();
            }
        }

        #endregion

        #region Ping Checker

        public static void OpenedPingChecker(bool setOpen = true)
        {
            if (setOpen)
                pingCheckerOpen = true;

            if (discordOptions.ShowState)
            {
                Discord.State = "Abusing servers to get ping times ⏲";
                Discord.SmallImageKey = "ping_checker_white";
                Discord.SmallImageText = "Ping Checker";
            }
            if (!updateOnChange)
                ApplyPresence();
        }

        public static void ClosedPingChecker()
        {
            pingCheckerOpen = false;

            if (discordOptions.ShowState)
            {
                if (statisticsOpen)
                {
                    OpenedStatistics(false);
                    return;
                }

                if (vaultPeekerOpen)
                {
                    OpenedVaultPeeker(false);
                    return;
                }

                Discord.State = GetLastState();
                Discord.SmallImageKey = null;
                Discord.SmallImageText = null;

                if (!updateOnChange)
                    ApplyPresence();
            }
        }

        #endregion

        #region Vault Peeker

        public static void OpenedVaultPeeker(bool setOpen = true)
        {
            if (setOpen)
                vaultPeekerOpen = true;

            if (discordOptions.ShowState)
            {
                Discord.State = "Peeking into vaults 🗝";
                Discord.SmallImageKey = "vault_peeker";
                Discord.SmallImageText = "Vault Peeker";

                if (!updateOnChange)
                    ApplyPresence();
            }
        }

        public static void ClosedVaultPeeker()
        {
            vaultPeekerOpen = false;

            if (discordOptions.ShowState)
            {
                if (statisticsOpen)
                {
                    OpenedStatistics(false);
                    return;
                }

                if (pingCheckerOpen)
                {
                    OpenedPingChecker(false);
                    return;
                }

                Discord.State = GetLastState();
                Discord.SmallImageKey = null;
                Discord.SmallImageText = null;

                if (!updateOnChange)
                    ApplyPresence();
            }
        }

        #endregion


        #region ProcessWatcher
        
        private static Dictionary<Process, string> processes = new Dictionary<Process, string>();
        public static Process AddProcessToWatchlist(ProcessStartInfo info, string type)
        {
            Process p = new Process()
            {
                StartInfo = info
            };
            processes.Add(p, type);
            p.EnableRaisingEvents = true;
            p.Exited += P_Exited;

            return p;
        }

        private static void P_Exited(object sender, EventArgs e)
        {
            var p = (Process)sender;
            p.Exited -= P_Exited;
            
            string key = processes[p];
            
            processes.Remove(p);

            if (!processes.Values.Contains(key))
            {
                switch (key)
                {
                    case "statistics":
                        statisticsOpen = false;
                        ClosedStatistics();
                        break;
                    case "pingChecker":
                        pingCheckerOpen = false;
                        ClosedPingChecker();
                        break;
                    case "vaultPeeker":
                        vaultPeekerOpen = false;
                        ClosedVaultPeeker();
                        break;
                    default:
                        break;
                }
            }
            p.Dispose();
        }

        #endregion


        #region Menus

        public static void UpdateMenu(Menu _menu)
        {
            menu = _menu;            
            Discord.LargeImageKey = imageKey;            

            if (discordOptions.ShowState)
            {
                switch (menu)
                {
                    case Menu.Accounts:
                        Discord.Details = "The better rotmg-launcher!💪";
                        SetState(discordOptions.ShowMenus ? "Selecting accounts 📁" : DEFAULT_STATE);
                        break;
                    case Menu.Settings:
                        SetState(discordOptions.ShowMenus ? "Playing with settings ⚙" : DEFAULT_STATE);
                        break;
                    case Menu.Modules:
                        SetState(discordOptions.ShowMenus ? "Deciding what to do 🙈" : DEFAULT_STATE);
                        break;
                    case Menu.Updater:
                        SetState(discordOptions.ShowMenus ? "Updating realm 🔄" : DEFAULT_STATE);
                        break;
                    case Menu.Help:
                        SetState(discordOptions.ShowMenus ? "In need of help 🆘" : DEFAULT_STATE);
                        break;
                    case Menu.Changelog:
                        SetState(discordOptions.ShowMenus ? "Reading about eam-changes 📰" : DEFAULT_STATE);
                        break;
                    case Menu.TokenViewer:
                        SetState(discordOptions.ShowMenus ? "Viewing tokens 🗝" : DEFAULT_STATE);
                        break;
                    case Menu.DailyLogin:
                        SetState(discordOptions.ShowMenus ? "Viewing the Daily auto Login 📅" : DEFAULT_STATE);
                        break;
                    case Menu.Logs:
                        SetState(discordOptions.ShowMenus ? "Living in the past ⏳" : DEFAULT_STATE);
                        break;
                    case Menu.About:
                        SetState(discordOptions.ShowMenus ? "Reading about EAM 📖" : DEFAULT_STATE);
                        break;
                    case Menu.Llama:
                        SetState(discordOptions.ShowMenus ? "Llama! 🦙" : DEFAULT_STATE);
                        Discord.LargeImageKey = imageKey;
                        break;
                }
                return;
            }
            SetState(DEFAULT_STATE);

            if (!updateOnChange)
                ApplyPresence();
        }

        #endregion

        public enum Menu
        {
            Accounts,
            Settings,
            Modules,
            Updater,
            Help,
            Changelog,
            TokenViewer,
            DailyLogin,
            Logs,
            About,
            Llama
        }
    }
}
