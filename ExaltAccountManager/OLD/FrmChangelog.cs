using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmChangelog : Form
    {
        List<string> changelogsHeadline = new List<string>()
        {
            "v2.5.0 - Muledump Replica: Vault Peeker, UI changes and more",
            "v2.2.3 - Hotfix: Daily Auto Login",
            "v2.2.2 - Ping checker, Security improvements and more",
            "v2.0 - Statistics, Changelog, Token viewer + QoL improvements",
            "v1.4 - \"Security improvements and QoL update\"",
            "v1.3 - The Daily Autologin Update!",
            "v1.2 - Bug fixes and darkmode",
            "v1.1 - Muledump import",
            "v1.0 - Exalt Account Manager"
        };

        List<string> changelogs = new List<string>()
        {
            $"VAULT PEEKER{Environment.NewLine}{Environment.NewLine}" +
            $"- Basicly a Muledump replica with less functionality.{Environment.NewLine}" +
            $"   Please give me feedback on what to change / add.{Environment.NewLine}{Environment.NewLine}" +
            $"MISCELLANEOUS{Environment.NewLine}{Environment.NewLine}" +
            $"- Slightly improved the scroll performance of the EAM.",

            $"HOTFIX{Environment.NewLine}{Environment.NewLine}" +
            $"- Fixed the Daily Auto Login task to work again.{Environment.NewLine}" +
            $"- Fixed a small bug in the Task-Installer.{Environment.NewLine}{Environment.NewLine}" +
            $"   Sorry for the inconvenience!{Environment.NewLine}{Environment.NewLine}" +
            $"ADD / EDIT ACCOUNT{Environment.NewLine}{Environment.NewLine}" +
            $"- Performance improvements by adding / editing accounts.{Environment.NewLine}" +
            $"- Added a loading UI.",

            $"ADDED A PING CHECKER MODULE{Environment.NewLine}{Environment.NewLine}" +
            $"- Ping every server and compare your ping time with the others.{Environment.NewLine}" +
            $"- Set favorites to quickly get an overview of the important servers.{Environment.NewLine}{Environment.NewLine}" +
            $"GAME UPDATER{Environment.NewLine}{Environment.NewLine}" +
            $"- Added a simple way to update or repair the game when needed.{Environment.NewLine}" +
            $"- It automatically checks once a day and will let you know if there{Environment.NewLine}   is an update available.{Environment.NewLine}{Environment.NewLine}" +
            $"SECURITY IMPROVEMENTS{Environment.NewLine}{Environment.NewLine}" +
            $"- The old AES-Key/IV implementation was a bit... dirty, so I switched{Environment.NewLine}" +
            $"   from AES 128 to DPAPI (Windows Data Protection API) wich is {Environment.NewLine}   quite superior and more secure.{Environment.NewLine}{Environment.NewLine}" +
            $"Instant SETUP{Environment.NewLine}{Environment.NewLine}" +
            $"- Import data from an old version in the blink of an eye!{Environment.NewLine}{Environment.NewLine}" +
            $"STATISTICS MODULE{Environment.NewLine}{Environment.NewLine}" +
            $"- Added a switch to differentiate between base and total fame for the      \"radar chart\" and the \"best class\".{Environment.NewLine}" +
            $"- Fixed the radar chart not rendering with new datasets.{Environment.NewLine}" +
            $"       Known bug: Old datasets do not render.{Environment.NewLine}" +
            $"                          > Just refresh them by \"re-new\" the token.{Environment.NewLine}{Environment.NewLine}" +
            $"IMPORT & EXPORT{Environment.NewLine}{Environment.NewLine}" +
            $"- Added a way to export and import data into this tool.{Environment.NewLine}" +
            $"  Currently accepted formats are: .EAMexport, .CSV and muledump files.{Environment.NewLine}{Environment.NewLine}" +
            $"UI-IMPROVEMENTS{Environment.NewLine}{Environment.NewLine}" +
            $"- Added grouping colors for each account to set them apart.{Environment.NewLine}" +
            $"- Added a nice snackbar (sadly it's nothing to eat).{Environment.NewLine}" +
            $"- Added / switched some images for more responsiveness.{Environment.NewLine}" +
            $"- Added an alphabetical sorting menu for accountName or email.{Environment.NewLine}" +
            $"     Just click the List-Titles (Account Name and Email) to show it.{Environment.NewLine}" +
            $"- Changed some colors.{Environment.NewLine}" +
            $"- Switched the scrollbars to custom ones, fixing the \"Jump to top\"-bug.{Environment.NewLine}" +
            $"- Switched the checkboxes to toggles.{Environment.NewLine}" +
            $"- Optimized the Drag & Drop performance.",

            $"ADDED A STATISTICS MODULE{Environment.NewLine}{Environment.NewLine}" +
            $"- With each login the tool collects data that can be presented in a unique      and beautiful way here.{Environment.NewLine}" +
            $"- Started logging specific data of some webrequest:{Environment.NewLine}" +
            $"     Account/verify:{Environment.NewLine}" +
            $"        Account name, class stats (best level/fame), total fame, fame.{Environment.NewLine}" +
            $"     Char/list:{Environment.NewLine}" +
            $"        All character related data.{Environment.NewLine}{Environment.NewLine}" +
            $"CHANGELOG{Environment.NewLine}{Environment.NewLine}" +
            $"- Added an easy way to keep track of the development of this tool.{Environment.NewLine}{Environment.NewLine}" +
            $"TOKEN VIEWER{Environment.NewLine}{Environment.NewLine}" +
            $"- Added a UI to view the current tokens of the accounts to help when{Environment.NewLine}   errors occure.{Environment.NewLine}{Environment.NewLine}" +
            $"BUG FIXES{Environment.NewLine}{Environment.NewLine}" +
            $"- Fixed the order of the log-entries, wich could slip into wrong order at{Environment.NewLine}   times.{Environment.NewLine}{Environment.NewLine}" +
            $"HELP UI{Environment.NewLine}{Environment.NewLine}" +
            $"- Added a simple way to get some quick troubleshooting tips.{Environment.NewLine}{Environment.NewLine}" +
            $"GUI-LOG{Environment.NewLine}{Environment.NewLine}" +
            $"- Changed to load only 50 entries at once to reduce the starting time.{Environment.NewLine}" +
            $"- Added a button to load 50 more entries (if there are more to load) at the{Environment.NewLine}   bottom of the list.{Environment.NewLine}" +
            $"   NOTE: It can take up to a few seconds to close when about 400 entries{Environment.NewLine}              are shown.{Environment.NewLine}{Environment.NewLine}" +
            $"MISCELLANEOUS{Environment.NewLine}{Environment.NewLine}" +
            $"- Added the functionality to order accounts via drag & drop.{Environment.NewLine}" +
            $"- Copy the account-name & e-mail to clipboard by clicking on it.{Environment.NewLine}" +
            $"- Run the daily login task manually by clicking the button for it, in case{Environment.NewLine}   something went wrong and you want to restart it.{Environment.NewLine}" +
            $"- Improved UI/UX here and there.{Environment.NewLine}" +
            $"- Synced llamas - they tend to wander of, please be carefull.",

            $"PROCESS DETECTION{Environment.NewLine}{Environment.NewLine}" +
            $"- Detecting running Exalt instances and shows the appropriate account as    logged in.{Environment.NewLine}{Environment.NewLine}" +
            $"LOGIN-TOKENS{Environment.NewLine}{Environment.NewLine}" +
            $"- Create login-tokens at runtime (New Login feature).{Environment.NewLine}{Environment.NewLine}" +
            $"   Special THANKS to DIA4A for helping me with the tokens!{Environment.NewLine}{Environment.NewLine}" +
            $"GUI-LOG{Environment.NewLine}{Environment.NewLine}" +
            $"- Added a graphical log for better error-handling.",

            $"AUTOMATIC NICKNAME DETECTION{Environment.NewLine}{Environment.NewLine}" +
            $"- Automaticly get the Account name (nickname) if you leave the username    blank.{Environment.NewLine}" +
            $"- Automaticly get the Account name (nickname) of muledump import if{Environment.NewLine}   you check the checkbox for it during import.{Environment.NewLine}{Environment.NewLine}" +
            $"AUTOMATIC DAILY LOGIN{Environment.NewLine}{Environment.NewLine}" +
            $"- Implemented the Auto Daily Login function.{Environment.NewLine}" +
            $"  NOTE: Use the menu for it in the Options-Bar and read the instructions in{Environment.NewLine}              the download-post if needed.",

            $"DARKMODE{Environment.NewLine}{Environment.NewLine}" +
            $"- Use the new darkmode to not burn your eyes!{Environment.NewLine}{Environment.NewLine}" +
            $"BUG FIXES{Environment.NewLine}{Environment.NewLine}" +
            $"- Fixed some annoying insects (bugs).",

            $"MULEDUMP IMPORT{Environment.NewLine}{Environment.NewLine}" +
            $"- Add accounts from a muledump formated file.",

            $"INITIAL RELEASE{Environment.NewLine}{Environment.NewLine}" +
            $"- Quickly open exalt with different accounts.{Environment.NewLine}" +
            $"- Open multiple instances of Exalt at once with the click of a button!{Environment.NewLine}" +
            $"- AES 128 encrypted save-file.",
        };

        List<string> releaseDates = new List<string>()
        {
            "99.99.9999",
            "05.09.2021",
            "28.08.2021",
            "20.04.2021",
            "11.03.2021",
            "05.02.2021",
            "12.06.2020",
            "24.05.2020",
            "19.05.2020"
        };


        FrmMainOLD frm;
        public FrmChangelog(FrmMainOLD _frm)
        {
            InitializeComponent();
            frm = _frm;

            ApplyTheme(frm.useDarkmode);

            LoadUIs();
        }

        private void LoadUIs()
        {
            bool isSecond = false;
            for (int i = 0; i < changelogsHeadline.Count; i++)
            {
                ChangeLogEntry entry = new ChangeLogEntry(changelogsHeadline[i], changelogs[i], releaseDates[i], frm.useDarkmode, isSecond);
                flow.Controls.Add(entry);
                flow.SetFlowBreak(entry, true);
                isSecond = !isSecond;
            }
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
                this.BackColor = third;
                pTop.BackColor = def;
                pBox.BackColor = def;

                lHeadline.ForeColor = font;

                pbLogo.Image = Properties.Resources.baseline_history_edu_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
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

        Pen p = new Pen(Color.Black);
        private void FrmChangelog_Paint(object sender, PaintEventArgs e)
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

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }
    }
}
