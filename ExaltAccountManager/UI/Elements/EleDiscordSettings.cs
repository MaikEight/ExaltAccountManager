using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleDiscordSettings : UserControl
    {
        private FrmMain frm;

        public EleDiscordSettings(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;

            toggleUseDiscord.Checked = !frm.OptionsData.discordOptions.OptOut;           

            toggleShowAccountNames.Checked = frm.OptionsData.discordOptions.ShowAccountNames;
            toggleShowMenus.Checked = frm.OptionsData.discordOptions.ShowMenus;
            toggleDiscord.Checked = frm.OptionsData.discordOptions.ShowState;

            ApplyTheme(this, EventArgs.Empty);

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.ForeColor = lSearchForRotmgUpdates.ForeColor = lGetEAMUpdateNotifications.ForeColor = lGetMessagesAndWarnings.ForeColor =
            font;

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in this.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
            {
                shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
                shadow.BorderColor = shadow.BorderColor = second;
                shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
            }

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            frm.OptionsData.discordOptions.OptOut = !toggleUseDiscord.Checked;
            frm.OptionsData.discordOptions.ShowAccountNames = toggleShowAccountNames.Checked;
            frm.OptionsData.discordOptions.ShowMenus = toggleShowMenus.Checked;
            frm.OptionsData.discordOptions.ShowState = toggleDiscord.Checked;

            frm.SaveOptions(frm.OptionsData, true);
            frm.UpdateUIOptionsData();

            frm.RemoveShadowForm();
        }

        private void toggleUseDiscord_CheckedChanged(object sender, EventArgs e)
        {
            pCover.Visible = !toggleUseDiscord.Checked;
        }
    }
}
