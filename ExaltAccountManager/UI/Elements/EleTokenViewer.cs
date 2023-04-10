using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleTokenViewer : UserControl
    {
        public MK_EAM_Lib.AccountInfo AccountInfo 
        {
            get => accountInfo;
            set
            {
                accountInfo = value;
                LoadUI();
            }
        }
        private MK_EAM_Lib.AccountInfo accountInfo;

        private FrmMain frm;

        public EleTokenViewer(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

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

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            lToken.BackColor = second;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
        }

        private void LoadUI()
        {
            lAccName.Text = AccountInfo.Name;
            lEmail.Text = AccountInfo.Email;
            lLastState.Text = MK_EAM_Lib.AccountInfo.RequestStateToString(AccountInfo.requestState);

            if (AccountInfo.accessToken == null)
            {
                lCreationTime.Text = string.Empty;
                lexpirationTime.Text = string.Empty;
                lToken.Text = "No token found.";

                btnCopy.Enabled =
                btnCharList.Enabled = false;
                return;
            }
            else
            {
                btnCopy.Enabled =
                btnCharList.Enabled = true;
            }

            lCreationTime.Text = $"{MK_EAM_Lib.AccessToken.UnixTimeStampToDateTime(Convert.ToDouble(AccountInfo.accessToken.creationTime)).ToString("HH:mm dd.MM.yyyy")} | Raw: {AccountInfo.accessToken.creationTime}";
            lexpirationTime.Text = $"{AccountInfo.accessToken.validUntil.ToString("HH:mm dd.MM.yyyy")} | Raw: {AccountInfo.accessToken.expirationTime}";
            lToken.Text = AccountInfo.accessToken.token;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (AccountInfo.accessToken == null) return;

            Clipboard.SetText(AccountInfo.accessToken.token);
            frm.ShowSnackbar("Token copied to clipboard.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000);
        }

        private void btnAccountVerify_Click(object sender, EventArgs e)
        {
            string webPath = string.Format("https://www.realmofthemadgod.com/account/verify?guid={0}&password={1}&clientToken={2}&game_net=Unity&play_platform=Unity&game_net_user_id=", WebUtility.UrlEncode(AccountInfo.Email), WebUtility.UrlEncode(AccountInfo.password), frm.GetDeviceUniqueIdentifier());

            System.Diagnostics.Process.Start(webPath);
        }

        private void btnCharList_Click(object sender, EventArgs e)
        {
            if (AccountInfo.accessToken == null) return;

            string webPath = $"https://www.realmofthemadgod.com/char/list?do_login=false&accessToken={WebUtility.UrlEncode(AccountInfo.accessToken.token)}&game_net=Unity&play_platform=Unity&game_net_user_id=&muleDump=true&__source=jakcodex-v965";

            System.Diagnostics.Process.Start(webPath);
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
    }
}
