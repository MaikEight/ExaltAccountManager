using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleDiscordUser : UserControl, IDisposeOfControl
    {
        private FrmMain frm;

        public bool DisposeOfControl => true;

        public EleDiscordUser(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) =>
            {
                frm.ThemeChanged -= ApplyTheme;
            };

            ApplyTheme(frm, null);

            LoadUI();
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
        }

        private void LoadUI()
        {
            if (frm == null || frm.DiscordUser == null)
            {
                pbClose_Click(pbClose, EventArgs.Empty);
                return;
            }

            lValues.Text = $"{frm.DiscordUser.AmountOfAccounts}{Environment.NewLine}" +
                           $"{frm.DiscordUser.AmountOfSessions}{Environment.NewLine}" +
                           $"{frm.DiscordUser.FirstSeen.ToString("dd.MM.yyyy HH:mm")}{Environment.NewLine}" +
                           $"{(int)(frm.DiscordUser.minutesOfEamUseTime / 60)}h {frm.DiscordUser.minutesOfEamUseTime % 60}m{Environment.NewLine}" +
                           $"{frm.DiscordUser.DiscordUserId}{Environment.NewLine}" +
                           $"{(frm.DiscordUser.EasterEggFound ? "Yeah!" : "No:(")} {Environment.NewLine}";
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e)
        {
            frm.RemoveShadowForm();
            this.Dispose();
        }

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
    }
}
