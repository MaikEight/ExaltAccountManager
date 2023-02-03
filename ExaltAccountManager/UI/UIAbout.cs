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
    public partial class UIAbout : UserControl
    {
        FrmMain frm;

        public UIAbout(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            lVesionNumber.Text = $"EAM v{frm.version}";

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;

            this.ForeColor = font;

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in this.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
            {
                shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;

                shadow.BorderColor = second;
                shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
            }

            pbVersion.Image = frm.UseDarkmode ? Properties.Resources.baseline_tag_white_36dp : Properties.Resources.baseline_tag_black_36dp;
            pbDevIcon.Image = pbLinkSourceCode.Image = frm.UseDarkmode ? Properties.Resources.ic_code_white_36dp : Properties.Resources.ic_code_black_36dp;
            pbBMAC.Image = frm.UseDarkmode ? Properties.Resources.bmc2 : Properties.Resources.bmac2;
            pbDiscord.Image = frm.UseDarkmode ? Properties.Resources.UI_Icon_SocialDiscord : Properties.Resources.UI_Icon_SocialDiscord_black1;
            pbEmail.Image = frm.UseDarkmode ? Properties.Resources.ic_email_white_48dp : Properties.Resources.ic_email_black_48dp;
            pbProgram.Image = pbEAM.Image = !frm.UseDarkmode ? Properties.Resources.logo : Properties.Resources._2;

            pbThanks.Image = frm.UseDarkmode ? Properties.Resources.birthday_cake_1_white_36px : Properties.Resources.birthday_cake_1_36px;
        }

        private void pbDev_Click(object sender, EventArgs e)
        {
            pbDevIcon.Image = Properties.Resources.llama;
            frm.SwitchLlamaState(true);
            DiscordHelper.SetLlamaState();
            timerLlama.Start();
        }

        private void pbLinkSourceCode_Click(object sender, EventArgs e) => linkSourceCode_LinkClicked(null, null);
        private void linkSourceCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start("https://github.com/MaikEight/ExaltAccountManager");
        private void pbBMAC_Click(object sender, EventArgs e) => linkBuyMeACoffe_LinkClicked(null, null);
        private void linkBuyMeACoffe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start("https://www.buymeacoffee.com/Maik8");
        private void pbDiscord_Click(object sender, EventArgs e) => linkDiscord_LinkClicked(null, null);
        private void linkDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start("https://discord.gg/VNfxgPqbJ7");
        private void pbEmail_Click(object sender, EventArgs e) => linkEmail_LinkClicked(null, null);
        private void linkEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start("mailto:mail@maik8.de");
        private void pbRealmeye_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://www.realmeye.com/forum/u/MaikEight");
        private void pbReddit_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://www.reddit.com/user/Maik85");
        private void linkRealmeye_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start("https://www.realmeye.com/forum/u/MaikEight");
        private void linkReddit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start("https://www.reddit.com/user/Maik85");

        private void pbEmail_MouseEnter(object sender, EventArgs e) => pbEmail.Image = frm.UseDarkmode ? Properties.Resources.ic_drafts_white_48dp : Properties.Resources.ic_drafts_black_48dp;        
        private void pbEmail_MouseLeave(object sender, EventArgs e) => pbEmail.Image = frm.UseDarkmode ? Properties.Resources.ic_email_white_48dp : Properties.Resources.ic_email_black_48dp;
        private void pbDiscord_MouseEnter(object sender, EventArgs e) => pbDiscord.Image = Properties.Resources.UI_Icon_Discord2;
        private void pbDiscord_MouseLeave(object sender, EventArgs e) => pbDiscord.Image = frm.UseDarkmode ? Properties.Resources.UI_Icon_SocialDiscord : Properties.Resources.UI_Icon_SocialDiscord_black1;
        private void pbLinkSourceCode_MouseEnter(object sender, EventArgs e) => pbLinkSourceCode.Image = frm.UseDarkmode ? Properties.Resources.source_code_white_36px : Properties.Resources.source_code_36px;
        private void pbLinkSourceCode_MouseLeave(object sender, EventArgs e) => pbLinkSourceCode.Image = frm.UseDarkmode ? Properties.Resources.ic_code_white_36dp : Properties.Resources.ic_code_black_36dp;

        private void pbBMAC_MouseEnter(object sender, EventArgs e) => pbBMAC.Image = frm.UseDarkmode ? Properties.Resources.bmc : Properties.Resources.bmac;
        private void pbBMAC_MouseLeave(object sender, EventArgs e) => pbBMAC.Image = frm.UseDarkmode ? Properties.Resources.bmc2 : Properties.Resources.bmac2;

        private void timerLlama_Tick(object sender, EventArgs e)
        {
            timerLlama.Stop();
            pbDevIcon.Image = frm.UseDarkmode ? Properties.Resources.ic_code_white_36dp : Properties.Resources.ic_code_black_36dp;
            frm.SwitchLlamaState(false);
        }

        private void pbReddit_MouseEnter(object sender, EventArgs e)
        {
            pbReddit.Size = new Size(36, 36);
            pbReddit.Location = new Point(pbReddit.Left -1, pbReddit.Top -1);
        }

        private void pbReddit_MouseLeave(object sender, EventArgs e)
        {
            pbReddit.Size = new Size(32, 32);
            pbReddit.Location = new Point(pbReddit.Left + 1, pbReddit.Top + 1);
        }

        private void pbRealmeye_MouseEnter(object sender, EventArgs e)
        {
            pbRealmeye.Size = new Size(40, 40);
            pbRealmeye.Location = new Point(pbRealmeye.Left - 2, pbRealmeye.Top - 2);
        }

        private void pbRealmeye_MouseLeave(object sender, EventArgs e)
        {
            pbRealmeye.Size = new Size(36, 36);
            pbRealmeye.Location = new Point(pbRealmeye.Left + 2, pbRealmeye.Top + 2);
        }

        private void pbThanks_MouseEnter(object sender, EventArgs e)
        {
            pbThanks.Image = frm.UseDarkmode ? Properties.Resources.birthday_cake_2_white_36px : Properties.Resources.birthday_cake_2_36px;
        }

        private void pbThanks_MouseLeave(object sender, EventArgs e)
        {
            pbThanks.Image = frm.UseDarkmode ? Properties.Resources.birthday_cake_1_white_36px : Properties.Resources.birthday_cake_1_36px;
        }
    }
}
