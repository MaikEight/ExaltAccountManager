using ExaltAccountManager.UI.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public partial class UIHelp : UserControl
    {
        private static FrmMain frm;

        Elements.EleQNA eleQNA;

        BindingList<QNA> questions = new BindingList<QNA>()
        {
            new QNA() { Question = "Why does Exalt not start?",
                        Answer = $"If it for some reason does not start exalt, check if you have exalt installed. - if so, check if the path in the options is set correctly.{Environment.NewLine}{Environment.NewLine}" +
                                 $"If you already have started the game via the Exalt launcher, close all running Exalt sessions, close the Exalt Launcher and try again.",
                        Type = QuestionType.Help },
            new QNA() { Question = "Exalt tells me something about a token...",
                        Answer= $"Try to use the \"Renew\"-Button right under the \"Play\"-Button, this will request a new token for that account.{Environment.NewLine}" +
                                $"If it still does not work, delete and Re-Add the account.{Environment.NewLine}{Environment.NewLine}" +
                                $"If none of your accounts work, try to use the \"HWID Tool\".{Environment.NewLine}",
                        ButtonImage = Properties.Resources.fingerprint_white_24px,
                        ButtonText = "Show HWID-Tool",
                        Action = Button_HWID_Click,
                        Type = QuestionType.Help },
            new QNA() { Question = "The Daily login task fails to install.",
                        Answer = $"Start the EAM with administrator privileges and try again.{Environment.NewLine}" +
                                 $"If that won't work, I can help you with that, please write me the error message you get here via discord or email.{Environment.NewLine}" +
                                 $"You can also install the task manually using the Windows Task Scheduler (mmc.exe).",
                        Type = QuestionType.DailyLogin },
            new QNA() { Question = "The Daily login task is installed but won't start, what can I do?",
                        Answer = $"First, check the Logs if it really did not start, as it always leaves an entry there on startup.{Environment.NewLine}" +
                                 $"Second, check if you have accounts opted-in for the daily auto login.",
                        ButtonImage = Properties.Resources.activity_history_outline_white_24px,
                        ButtonText = "Show Logs",
                        Action = Button_Logs_Click,
                        Type = QuestionType.DailyLogin },
            new QNA() { Question = "The Daily login task ran successfully but my accounts did not get the login-reward, why?",
                        Answer = $"Check the Logs, if the task did report any errors.{Environment.NewLine}{Environment.NewLine}" +
                                 $"Hint: You still need to login once at the end of the month to claim your rewards!",
                        ButtonImage = Properties.Resources.activity_history_outline_white_24px,
                        ButtonText = "Show Logs",
                        Action = Button_Logs_Click,
                        Type = QuestionType.DailyLogin },
            new QNA() { Question = "Can I get banned for using this?",
                        Answer= $"No, not that I am aware of.{Environment.NewLine}{Environment.NewLine}" +
                                $"However, I won't take responsibility for if it does happen!{Environment.NewLine}{Environment.NewLine}" +
                                $"To somewhat ensure you are on the safe side, there is a notification-system built-in, that warns you about potential risks, as soon as they occure.{Environment.NewLine}" +
                                $"The last release got approved by the deca-team (a user asked them, if it is allowed to use the EAM).{Environment.NewLine}{Environment.NewLine}" +
                                $"Ensure you have the Notifications, warnings and the killswitch enabled in the options!",
                        ButtonImage = Properties.Resources.settings_outline_white_24px,
                        ButtonText = "Show Options",
                        Action = Button_Options_Click,
                        Type = QuestionType.Account },
            new QNA() { Question = "Where are the save files located?",
                        Answer = $"All your accounts and save files are located here:{Environment.NewLine}" +
                                 $"{FrmMain.saveFilePath}{Environment.NewLine}",
                        ButtonImage = Properties.Resources.ic_folder_open_white_18dp,
                        ButtonText = "Open save file folder",
                        Action = (object sender, EventArgs e) => System.Diagnostics.Process.Start(FrmMain.saveFilePath),
                        Type = QuestionType.Account},
            new QNA() { Question = "Are my accounts saved on my PC only?",
                        Answer = $"Yes, of course! This tool saves your account details encrypted (via WDP-API) in the save file folder only.{Environment.NewLine}" +
                                 $"There is no data send to any third-party-service, except the official deca-servers.",
                        Type = QuestionType.Account},
            new QNA() { Question = "Can I support this project?",
                        Answer = $"Of course you can!{Environment.NewLine}" +
                                 $"You can donate some money via BuyMeACoffee (Button below).{Environment.NewLine}" +
                                 $"If you can't or don't want to donate money, you are also welcome to leave me some feedback via discord or email." +
                                 $"Thank you for using EAM!",
                        ButtonImage = Properties.Resources.bmc,
                        ButtonText = "Open \"Buy me a Coffee\"",
                        Action = (object sender, EventArgs e) => System.Diagnostics.Process.Start("https://www.buymeacoffee.com/Maik8"),
                        Type = QuestionType.Miscellaneous }
        };

        public UIHelp(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            questions = new BindingList<QNA>(questions.OrderBy(x => x.Type).ToList());
            dataGridView.DataSource = questions;
            dataGridView.Columns[0].Width = 100;
            dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridView.MouseWheel += dataGridView_MouseWheel;

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);
            eleQNA = new Elements.EleQNA(frm);
        }

        private void UIHelp_Load(object sender, EventArgs e)
        {
            scrollbar.SmallChange = 1;
            scrollbar.LargeChange = 2;
            scrollbar.Value = 0;
            scrollbar.Maximum = (questions.Count - 1) / 2;
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;

            this.ForeColor = font;

            shadowContact.PanelColor = shadowContact.BackColor = shadowContact.PanelColor2 = pLinkButtons.BackColor = def;
            shadowContact.BorderColor = shadowContact.BorderColor = second;
            shadowContact.ShadowColor = shadowContact.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            bunifuCards.BackColor = second;

            pbEmail.Image = frm.UseDarkmode ? Properties.Resources.ic_email_white_48dp : Properties.Resources.ic_email_black_48dp;
            pbMailMini.Image = frm.UseDarkmode ? Properties.Resources.ic_email_white_24dp : Properties.Resources.ic_email_black_24dp;

            pbDiscord.Image = pbDiscordMini.Image = frm.UseDarkmode ? Properties.Resources.UI_Icon_SocialDiscord : Properties.Resources.UI_Icon_SocialDiscord_black1;

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

        #region Contact

        private void pbEmail_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("mailto:mail@maik8.de");

        private void pbEmail_MouseEnter(object sender, EventArgs e)
        {
            pbEmail.Image = frm.UseDarkmode ? Properties.Resources.ic_drafts_white_48dp : Properties.Resources.ic_drafts_black_48dp;
            pbMailMini.Image = frm.UseDarkmode ? Properties.Resources.ic_drafts_white_24dp : Properties.Resources.ic_drafts_black_24dp;
        }

        private void pbEmail_MouseLeave(object sender, EventArgs e)
        {
            pbEmail.Image = frm.UseDarkmode ? Properties.Resources.ic_email_white_48dp : Properties.Resources.ic_email_black_48dp;
            pbMailMini.Image = frm.UseDarkmode ? Properties.Resources.ic_email_white_24dp : Properties.Resources.ic_email_black_24dp;
        }

        private void pbDiscord_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://discord.gg/VNfxgPqbJ7");

        private void pbDiscord_MouseEnter(object sender, EventArgs e)
        {
            (sender as Bunifu.UI.WinForms.BunifuPictureBox).Image = Properties.Resources.UI_Icon_Discord2;
        }

        private void pbDiscord_MouseLeave(object sender, EventArgs e)
        {
            (sender as Bunifu.UI.WinForms.BunifuPictureBox).Image = frm.UseDarkmode ? Properties.Resources.UI_Icon_SocialDiscord : Properties.Resources.UI_Icon_SocialDiscord_black1;
        }

        bool contactIsMinimize = false;
        private void btnContactMinimize_Click(object sender, EventArgs e)
        {
            contactIsMinimize = !contactIsMinimize;

            lHelpText.Visible = pLinkButtons.Visible = spacorContact.Visible = !contactIsMinimize;
            pbMailMini.Visible = pbDiscordMini.Visible = contactIsMinimize;

            if (contactIsMinimize)
            {
                shadowContact.Height = 54;
                pbMailMini.Top = pbDiscordMini.Top = 15;
            }
            else
                shadowContact.Height = 170;

            btnContactMinimize.Image = contactIsMinimize ? Properties.Resources.ic_arrow_drop_down_white_36dp : Properties.Resources.ic_arrow_drop_up_white_36dp;
        }

        #endregion

        private static void Button_HWID_Click(object sender, EventArgs e)
        {
            frm.ShowShadowForm(new EleHWID_Tool(frm));
        }

        private static void Button_Options_Click(object sender, EventArgs e)
        {
            frm.btnOptions_Click(frm, null);
            frm.RemoveShadowForm();
        }

        private static void Button_Logs_Click(object sender, EventArgs e)
        {
            frm.btnLogs_Click(frm, null);
            frm.RemoveShadowForm();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                eleQNA.QNA = questions[e.RowIndex];
                frm.ShowShadowForm(eleQNA);
            }
            catch { }
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
    }

    public class QNA
    {
        public QuestionType Type { get; set; } = QuestionType.Help;
        public string Question { get; set; } = string.Empty;
        [Browsable(false)]
        public string Answer { get; set; } = string.Empty;
        [Browsable(false)]
        public string ButtonText { get; set; } = string.Empty;
        [Browsable(false)]
        public Action<object, EventArgs> Action { get; set; } = null;
        [Browsable(false)]
        public Image ButtonImage { get; set; } = null;
    }

    public enum QuestionType
    {
        Miscellaneous = 0,
        Account = 1,
        Bug = 2,
        DailyLogin = 3,
        Help = 4,
        Ping = 5,
        Statistics = 6,
        Update = 7,
        Message,
        Warning,
        Stop,
    }
}
