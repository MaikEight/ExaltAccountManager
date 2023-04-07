using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public sealed partial class UIChangelog : UserControl
    {
        private FrmMain frm;
        private Elements.EleChangelog eleChangelog;

        private readonly BindingList<ChangelogEntry> changelogEntries = new BindingList<ChangelogEntry>()
        {
            new ChangelogEntry()
            {
              ReleaseDate = new DateTime(2023, 04, 09),
              Version = new Version(3, 1, 0),
              Name = "Captcha Support, News, installer, bug fixes and much more",
              Description = $"<b>CAPTCHA AID</b>{Environment.NewLine}{Environment.NewLine}" +
                            $"- Added support for captcha solving.{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;This is NO automation tool.{Environment.NewLine}" +
                            $"ℹ️ The Captcha Aid tool is still in beta, please report feedback.{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>INSTALLER</b>{Environment.NewLine}" +
                            $"- Added a new simple installer for EAM.{Environment.NewLine}{Environment.NewLine}" + 
                            $"<b>EAM UPDATER</b>{Environment.NewLine}" +
                            $"- Added an updater to EAM, no manuall downloads needed anymore!{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>EAM NEWS</b>{Environment.NewLine}" +
                            $"- Added a new system display custom news.{Environment.NewLine}" +
                            $"- Added multiple components to display.{Environment.NewLine}"+ 
                            $"- Added Polls to ask questions to you, the community.{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>DISCORD INTEGRATION</b>{Environment.NewLine}" +
                            $"- Added Discord-RPC support.{Environment.NewLine}" +
                            $"- Supports live-state tracking.{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>DAILY LOGINS</b>{Environment.NewLine}" +
                            $"- Changed the \"Run manually\" panel to use a toggle instead.{Environment.NewLine}" +
                            $"- Fixed the Graph to actually work now... hopefully{Environment.NewLine}" +
                            $"- Fixed the \"Last Run\" and \"Results\" panels to display information.{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;Thanks to <b>Ykao</b> for pointing me to this.{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>OPTIONS</b>{Environment.NewLine}" +
                            $"- Added a new toggle that allows to always refresh the data on login.{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;Big thanks to <b>Tadus</b> for reporting and testing.{Environment.NewLine}" +
                            $"- Added new buttons to allow for changing notification and privacy settings.{Environment.NewLine}" +
                            $"- Added an indicator for unsaved changes.{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>SECURITY</b>{Environment.NewLine}" +
                            $"- Finally got a code signing certificate to sign EAM-Files.{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;This should remove the \"Windows SmartScreen\" (on most systems).{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>ANALYTICS</b>{Environment.NewLine}" +
                            $"- Added anonym analytics for the EAM-usage.{Environment.NewLine}" +
                            $"- Any data collected is anonym and does NOT contain login informations.{Environment.NewLine}" +
                            $"- All collected data is sent encrypted (SSL) to the server.{Environment.NewLine}" +
                            $"- All collected data is used for improving EAM and it's features.{Environment.NewLine}" +   
                            $"- You can choose to send no clientId or opt-out in the options.{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>EAM MICROSERVICE</b>{Environment.NewLine}" +
                            $"- Added an REST-API that is running on a VPS.{Environment.NewLine}" +
                            $"ℹ️ The API is used to collect EAM-analytics data, provide notification messages,{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;eam-update-checks, character-stats-updates (soon™),{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;vault peeker-item-updates and more!{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>IMPORTER</b>{Environment.NewLine}" +
                            $"- Added a brief Error-Message, if the import failed.{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;This avoids the UI to become stuck at \"Importing, please wait...\".{Environment.NewLine}{Environment.NewLine}" +
                            $"<b>BUG FIXES</b>{Environment.NewLine}" +
                            $"- Fixed a critical bug during opening of the daily login menu on windows server.{Environment.NewLine}" +
                            $"- Fixed the taskbar-icon not minimizing EAM-windows.{Environment.NewLine}" +
                            $"- Fixed the button-sidebar not moving when the game-update button is added.{Environment.NewLine}" +
                            $"- Fixed a bug of the Notification Center that spamed Windows-Notifications.{Environment.NewLine}" +
                            $"- Fixed a crashing bug that occured when copying an empty accountname / email {Environment.NewLine}" +
                            $"- Fixed a bug that mad the task installer not working properly.{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;Thanks to <b>arhippa</b> for reporting the issue.{Environment.NewLine}" +
                            $"- Fixed a bug when importing .csv with missing headers.{Environment.NewLine}" +
                            $"- Fixed a bug that made the \"Daily Autologin\"-Toggle being reversed.{Environment.NewLine}" +
                            $"&nbsp;&nbsp;&nbsp;Thanks to <b>Ykao</b> for reporting the issue.{Environment.NewLine}" +
                            $"- Removed the \"White line\" at the bottom of EAM during darkmode."
            },
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2022, 02, 26),
                Version = new Version(3, 0, 0),                       
                Name = "Complete overhaul, Muledump Replica: Vault Peeker and more",
                Description = $"<b>COMPLETE OVERHAUL</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Completely changed the UI.{Environment.NewLine}" +
                              $"- Re-Wrote most of the Code.{Environment.NewLine}" +
                              $"- Improved Account-Handling.{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>ACCOUNTS</b>{Environment.NewLine}" +
                              $"- Added a new Filter & Grouping Option.{Environment.NewLine}" +
                              $"- Mass-toggle the \"Daily Login\" or \"Show in Vault Peeker\" option.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;Vault Peeker is limited to 50 accounts.{Environment.NewLine}" +
                              $"- Improved performance a lot.{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>MODULES</b>{Environment.NewLine}" +
                              $"- Simple collection of all important tools.{Environment.NewLine}{Environment.NewLine}" +                              
                              $"<b>IMPORT & EXPORT</b>{Environment.NewLine}" +
                              $"- Added a custom Importer for text-based formats.{Environment.NewLine}" +
                              $"- Added a custom Exporter, to export your accounts as needed.{Environment.NewLine}" +
                              $"- Improved UI{Environment.NewLine}" +
                              $"- Improved Performance{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>VAULT PEEKER MODULE</b>{Environment.NewLine}" +
                              $"- Basically a Muledump replica with less functionality.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;<b>Please give me feedback on what to change / add.</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>DAILY LOGIN SERVICE</b>{Environment.NewLine}" +
                              $"- Re-done most of it from scratch.{Environment.NewLine}" +
                              $"- Run manually with a \"refresh all token\" option via the menu.{Environment.NewLine}" +
                              $"- Get accountname if it's empty.{Environment.NewLine}" +
                              $"- Added the game-updater.{Environment.NewLine}" +
                              $"- Improved performance.{Environment.NewLine}" +
                              $"- Improved reliability.{Environment.NewLine}" +
                              $"- Improved statistics data collection.{Environment.NewLine}" +
                              $"- Improved logging.{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>NOTIFICATION CENTER</b>" + $" | former known as Tasktray-tool.{Environment.NewLine}{Environment.NewLine}" +
                              $"- Improved UI.{Environment.NewLine}" +
                              $"- Fixed all known bugs.{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>STATISTICS MODULE</b>{Environment.NewLine}" +
                              $"- Smaller UI improvements.{Environment.NewLine}" +
                              $"- Added support for the Kensei-class.{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>PING CHECKER MODULE</b>{Environment.NewLine}" +
                              $"- Ping-Speed improved.{Environment.NewLine}" +
                              $"- Smaller UI improvements.{Environment.NewLine}" +
                              $"- Added support for the Kensei-class.{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>MISCELLANEOUS</b>{Environment.NewLine}" +
                              $"- New Logo & Icon!{Environment.NewLine}" +
                              $"- Added an option to change the snackbar position."},
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2021, 09, 05),
                Version = new Version(2, 2, 3),
                Name = "Hotfix: Daily Auto Login",
                Description = $"<b>HOTFIX</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Fixed the Daily Auto Login task to work again.{Environment.NewLine}" +
                              $"- Fixed a small bug in the Task-Installer.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;Sorry for the inconvenience!{Environment.NewLine}{Environment.NewLine}" +
                              $"<b>ADD / EDIT ACCOUNT</b>{Environment.NewLine}" +
                              $"- Performance improvements by adding / editing accounts.{Environment.NewLine}" +
                              $"- Added a loading UI."},
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2021, 08, 29),
                Version = new Version(2, 2, 2),
                Name = "Ping checker, Security improvements and more",
                Description = $"<b>ADDED A PING CHECKER MODULE</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Ping every server and compare your ping time with the others.{Environment.NewLine}" +
                              $"- Set favorites to quickly get an overview of the important servers.{Environment.NewLine}<br>" +
                              $"<b>GAME UPDATER</b><br>" +
                              $"- Added a simple way to update or repair the game when needed.{Environment.NewLine}" +
                              $"- It automatically checks once a day and will let you know if there is an update available.{Environment.NewLine}<br>" +
                              $"<b>SECURITY IMPROVEMENTS</b>{Environment.NewLine}" +
                              $"- The old AES-Key/IV implementation was a bit... dirty, so I switched from AES 128 to DPAPI (Windows Data Protection API) wich is quite superior and more secure.{Environment.NewLine}<br>" +
                              $"<b>INSTANT SETUP</b>{Environment.NewLine}" +
                              $"- Import data from an old version in the blink of an eye!{Environment.NewLine}<br>" +
                              $"<b>STATISTICS MODULE</b>{Environment.NewLine}" +
                              $"- Added a switch to differentiate between base and total fame for the \"radar chart\" and the \"best class\".{Environment.NewLine}" +
                              $"- Fixed the radar chart not rendering with new datasets.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;Known bug: Old datasets do not render.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;> Just refresh them by \"re-new\" the token.{Environment.NewLine}<br>" +
                              $"<b>IMPORT & EXPORT</b>{Environment.NewLine}" +
                              $"- Added a way to export and import data into this tool.{Environment.NewLine}" +
                              $"  Currently accepted formats are: .EAMexport, .CSV and muledump files.{Environment.NewLine}<br>" +
                              $"<b>UI-IMPROVEMENTS</b>{Environment.NewLine}" +
                              $"- Added grouping colors for each account to set them apart.{Environment.NewLine}" +
                              $"- Added a nice snackbar (sadly it's nothing to eat).{Environment.NewLine}" +
                              $"- Added / switched some images for more responsiveness.{Environment.NewLine}" +
                              $"- Added an alphabetical sorting menu for accountName or email.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;Just click the List-Titles (Account Name and Email) to show it.{Environment.NewLine}" +
                              $"- Changed some colors.{Environment.NewLine}" +
                              $"- Switched the scrollbars to custom ones, fixing the \"Jump to top\"-bug.{Environment.NewLine}" +
                              $"- Switched the checkboxes to toggles.{Environment.NewLine}" +
                              $"- Optimized the Drag & Drop performance.",},
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2021, 04, 20),
                Version = new Version(2, 0, 0),
                Name = "Statistics, Changelog, Token viewer + QoL improvements",
                Description = $"<b>ADDED A STATISTICS MODULE</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- With each login the tool collects data that can be presented in a unique{Environment.NewLine}&nbsp;&nbsp;and beautiful way here.{Environment.NewLine}" +
                              $"- Started logging specific data of some webrequest:{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;Account/verify:{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Account name, class stats (best level/fame), total fame, fame.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;Char/list:{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;All character related data.{Environment.NewLine}<br>" +
                              $"<b>CHANGELOG</b>{Environment.NewLine}" +
                              $"- Added an easy way to keep track of the development of this tool.{Environment.NewLine}<br>" +
                              $"<b>TOKEN VIEWER</b>{Environment.NewLine}" +
                              $"- Added a UI to view the current tokens of the accounts to help when errors occure.{Environment.NewLine}<br>" +
                              $"<b>BUG FIXES</b>{Environment.NewLine}" +
                              $"- Fixed the order of the log-entries, wich could slip into wrong order at times.{Environment.NewLine}<br>" +
                              $"<b>HELP UI</b>{Environment.NewLine}" +
                              $"- Added a simple way to get some quick troubleshooting tips.{Environment.NewLine}<br>" +
                              $"<b>GUI-LOG</b>{Environment.NewLine}" +
                              $"- Changed to load only 50 entries at once to reduce the starting time.{Environment.NewLine}" +
                              $"- Added a button to load 50 more entries (if there are more to load) at the{Environment.NewLine}&nbsp;&nbsp;bottom of the list.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;NOTE: It can take up to a few seconds to close when about 400 entries are shown.{Environment.NewLine}<br>" +
                              $"<b>MISCELLANEOUS</b>{Environment.NewLine}" +
                              $"- Added the functionality to order accounts via drag & drop.{Environment.NewLine}" +
                              $"- Copy the account-name & e-mail to clipboard by clicking on it.{Environment.NewLine}" +
                              $"- Run the daily login task manually by clicking the button for it, in case something{Environment.NewLine}&nbsp;&nbsp;went wrong and you want to restart it.{Environment.NewLine}" +
                              $"- Improved UI/UX here and there.{Environment.NewLine}" +
                              $"- Synced llamas - they tend to wander of, please be carefull."},
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2021, 03, 11),
                Version = new Version(1, 4, 0),
                Name = "Security improvements and QoL update",
                Description = $"<b>PROCESS DETECTION</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Detecting running Exalt instances and shows the appropriate account as logged in.{Environment.NewLine}<br>" +
                              $"<b>LOGIN-TOKENS</b>{Environment.NewLine}" +
                              $"- Create login-tokens at runtime (New Login feature).{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;Special THANKS to DIA4A for helping me with the tokens!{Environment.NewLine}<br>" +
                              $"<b>GUI-LOG</b>{Environment.NewLine}" +
                              $"- Added a graphical log for better error-handling."},
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2021, 02, 05),
                Version = new Version(1, 3, 0),
                Name = "The Daily Autologin Update!",
                Description = $"<b>AUTOMATIC NICKNAME DETECTION</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Automaticly get the Account name (nickname) if you leave the username blank.{Environment.NewLine}" +
                              $"- Automaticly get the Account name (nickname) of muledump import if you check the checkbox for it during import.{Environment.NewLine}<br>" +
                              $"<b>AUTOMATIC DAILY LOGIN</b>{Environment.NewLine}" +
                              $"- Implemented the Auto Daily Login function.{Environment.NewLine}" +
                              $"&nbsp;&nbsp;&nbsp;NOTE: Use the menu for it in the Options-Bar and read the instructions in the post if &nbsp;&nbsp;&nbsp;needed.",},
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2020, 06, 12),
                Version = new Version(1, 2, 0),
                Name = "Bug fixes and darkmode ",
                Description = $"<b>DARKMODE</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Use the new darkmode to not burn your eyes!{Environment.NewLine}<br>" +
                              $"<b>BUG FIXES</b>{Environment.NewLine}" +
                              $"- Fixed some annoying insects (bugs)." },
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2020, 05, 24),
                Version = new Version(1, 1, 0),
                Name = "Muledump import",
                Description = $"<b>MULEDUMP IMPORT</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Add accounts from a muledump formated file." },
            new ChangelogEntry() {
                ReleaseDate = new DateTime(2020, 05, 19),
                Version = new Version(1, 0, 0),
                Name = "Exalt Account Manager",
                Description = $"<b>INITIAL RELEASE</b>{Environment.NewLine}{Environment.NewLine}" +
                              $"- Quickly open exalt with different accounts.{Environment.NewLine}" +
                              $"- Open multiple instances of Exalt at once with the click of a button!{Environment.NewLine}" +
                              $"- AES 128 encrypted save-file." }
        };

        private bool isFirstClick = true;
        public UIChangelog(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            dataGridView.DataSource = changelogEntries;
            dataGridView.Columns[0].Width = 100;
            dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridView.MouseWheel += dataGridView_MouseWheel;
            eleChangelog = new Elements.EleChangelog(frm);
            

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);
        }
        private void UIChangelog_Load(object sender, EventArgs e)
        {
            scrollbar.SmallChange = 1;
            scrollbar.LargeChange = 2;
            scrollbar.Value = 0;
            if (dataGridView.Rows.Count > 1)
                scrollbar.Maximum = (dataGridView.Rows.Count - 1) / 2;
            else
                scrollbar.Maximum = 1;

            if (dataGridView.Rows.Count > 0)
            {
                dataGridView.Columns[0].Width = 75;
                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[2].Width = 100;
                dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            eleChangelog.Entry = changelogEntries[changelogEntries.Count - 1];

        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;

            this.ForeColor = font;

            bunifuCards.BackColor = second;

            scrollbar.BorderColor = frm.UseDarkmode ? third : Color.Silver;
            scrollbar.BackgroundColor = frm.UseDarkmode ? def : third;
            scrollbar.ThumbColor = frm.UseDarkmode ? third : Color.Gray;

            dataGridView.BackgroundColor = second;
            dataGridView.CurrentTheme.BackColor = frm.UseDarkmode ? Color.FromArgb(77, 10, 173) : Color.FromArgb(107, 40, 203);
            dataGridView.CurrentTheme.GridColor = dataGridView.GridColor = frm.UseDarkmode ? third : Color.WhiteSmoke;

            dataGridView.CurrentTheme.HeaderStyle.BackColor = dataGridView.CurrentTheme.HeaderStyle.SelectionBackColor = dataGridView.HeaderBackColor = frm.UseDarkmode ? Color.FromArgb(77, 10, 173) : Color.FromArgb(107, 40, 203);

            dataGridView.CurrentTheme.RowsStyle.BackColor = frm.UseDarkmode ? Color.FromArgb(126, 65, 214) : Color.FromArgb(176, 127, 246);//78, 12, 174
            dataGridView.CurrentTheme.AlternatingRowsStyle.BackColor = frm.UseDarkmode ? Color.FromArgb(106, 45, 194) : Color.FromArgb(156, 95, 244);

            dataGridView.ApplyTheme(dataGridView.CurrentTheme);
        }

        private void scrollbar_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            dataGridView.FirstDisplayedScrollingRowIndex = scrollbar.Value;
        }

        private void dataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;
            int movement = e.Delta / 120;
            if (dataGridView.FirstDisplayedScrollingRowIndex - movement >= 0 && dataGridView.FirstDisplayedScrollingRowIndex - movement < dataGridView.Rows.Count)
                scrollbar.Value = dataGridView.FirstDisplayedScrollingRowIndex -= movement;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                eleChangelog.Entry = changelogEntries[e.RowIndex];
                frm.ShowShadowForm(eleChangelog);

                if (isFirstClick)
                {
                    isFirstClick = false;
                    frm.RemoveShadowForm();

                    eleChangelog.Entry = changelogEntries[e.RowIndex];
                    frm.ShowShadowForm(eleChangelog);
                }
            }
            catch { }
        }
    }

    public sealed class ChangelogEntry
    {
        public Version Version { get; set; } = new Version();
        public string Name { get; set; } = string.Empty;
        [Browsable(false)]
        public string Description { get; set; } = string.Empty;
        [DisplayName("Date")]
        public DateTime ReleaseDate { get; set; } = DateTime.MinValue;
    }
}
