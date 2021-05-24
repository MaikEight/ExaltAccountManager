using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class TokenViewerUI : UserControl
    {
        FrmTokenViewer frmViewer;
        AccountInfo info;
        string accountVerifyLink = "https://www.realmofthemadgod.com/account/verify?guid={0}&password={1}&clientToken={2}&game_net=Unity&play_platform=Unity&game_net_user_id=";
        string charListLink = "";

        bool isSecond = false;
        public TokenViewerUI(FrmTokenViewer _frmViewer, AccountInfo _info, bool isDarkmode, bool _isSecond = false)
        {
            InitializeComponent();
            frmViewer = _frmViewer;
            info = _info;
            isSecond = _isSecond;

            //pTop_Click(pTop, null);
            CloseView();
            LoadUI();

            ApplyTheme(isDarkmode);
        }

        private void LoadUI()
        {
            if (info != null)
            {
                try
                {
                    lName.Text = info.name;
                    lMail.Text = info.email;

                    if (info.accessToken != null && !string.IsNullOrEmpty(info.accessToken.token))
                    {
                        rtbAccessToken.Text = info.accessToken.token;
                        DateTime cTime = AccessToken.UnixTimeStampToDateTime(double.Parse(info.accessToken.creationTime));
                        lCreationTime.Text = $"{cTime.ToShortDateString()} {cTime.ToShortTimeString()}";
                        lValidFor.Text = $"{Math.Round((info.accessToken.validUntil - cTime).TotalHours, 2)} hours";
                        lValidUntil.Text = $"{info.accessToken.validUntil.ToShortDateString()} {info.accessToken.validUntil.ToShortTimeString()}";
                        accountVerifyLink = string.Format(accountVerifyLink, System.Net.WebUtility.UrlEncode(info.email), System.Net.WebUtility.UrlEncode(info.password), info.accessToken.clientToken);
                        charListLink = $"https://www.realmofthemadgod.com/char/list?do_login=true&accessToken={System.Net.WebUtility.UrlEncode(info.accessToken.token)}&game_net=Unity&play_platform=Unity&game_net_user_id=";
                    }
                    else
                    {
                        rtbAccessToken.Text = "No Token found!";
                        lCreationTime.Text = "";
                        lValidFor.Text = "";
                        lValidUntil.Text = "";

                        accountVerifyLink = "";
                        linkAccountVerify.Enabled = false;
                        charListLink = "";
                        linkCharList.Enabled = false;
                    }
                }
                catch
                {

                }                
            }
        }

        public void ApplyTheme(bool isDarkmode)
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(245, 245, 245);
            Color third = Color.FromArgb(240, 240, 240);
            Color font = Color.Black;

            if (isDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            this.ForeColor = font;
            this.BackColor = pTop.BackColor = pName.BackColor =
                isSecond ? second : def;
            pMain.BackColor = isSecond ? def : second;

            p = new Pen(font);
        }

        Pen p = new Pen(Color.Black);
        private void pMain_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topRight, topLeft);
            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerRight, lowerLeft);
            e.Graphics.DrawLine(p, topLeft, lowerLeft);
        }

        private void linkAccountVerify_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(!string.IsNullOrEmpty(accountVerifyLink))
                System.Diagnostics.Process.Start(accountVerifyLink);
        }

        private void linkCharList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(charListLink))
                System.Diagnostics.Process.Start(charListLink);
        }

        private void pTop_Click(object sender, EventArgs e)
        {
            frmViewer.OpenViewer(this);
            //if (!pMain.Visible)
            //{
            //    //Show

            //}
            //else
            //{
            //    //Hide

            //}
        }

        public void CloseView()
        {
            pMain.Visible = false;
            this.Height = 30;
        }

        public void OpenView()
        {
            pMain.Visible = true;
            this.Height = 225;
        }
    }
}
