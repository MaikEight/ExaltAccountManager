using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmHWIDchanger : Form
    {
        FrmMain frm;
        private Random random = new Random();
        private string originalHWID = string.Empty;
        private AccountUI ui = null;

        public FrmHWIDchanger(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;
            originalHWID = frm.GetDeviceUniqueIdentifier();
            ApplyTheme();
        }

        public void ShowUI(AccountUI _ui)
        {
            ui = _ui;

            tbHWID.Text = (string.IsNullOrEmpty(ui.accountInfo.customClientID) || ui.accountInfo.customClientID.Equals(originalHWID)) ? originalHWID : ui.accountInfo.customClientID;
            lHeadline.Focus();
            //lHWIDHeadline.Text = $"HWID for AccountNameHere";
            lHWIDHeadline.Text = $"HWID for {ui.accountInfo.name}";
        }

        public void ApplyTheme()
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (frm.useDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            this.BackColor = second;
            pTop.BackColor = pbClose.BackColor = pbLogo.BackColor = third;
            this.ForeColor = tbHWID.ForeColor = font;
            tbHWID.BackColor = tbHWID.OnIdleState.FillColor = tbHWID.OnActiveState.FillColor = tbHWID.OnDisabledState.FillColor = tbHWID.OnHoverState.FillColor = def;
            tbHWID.OnIdleState.BorderColor = font;
            tbHWID.Update();
            btnGenerate.FlatAppearance.MouseOverBackColor = btnUseReal.FlatAppearance.MouseOverBackColor = btnSave.FlatAppearance.MouseOverBackColor = frm.useDarkmode ? Color.FromArgb(25, 225, 225, 225) : Color.FromArgb(50, 128, 128, 128);
            btnGenerate.Image = frm.useDarkmode ? Properties.Resources.fingerprint_scan_white_24px_1 : Properties.Resources.fingerprint_scan_24px;
            btnUseReal.Image = frm.useDarkmode ? Properties.Resources.fingerprint_white_24px : Properties.Resources.fingerprint_24px;
            btnSave.Image = frm.useDarkmode ? Properties.Resources.save_18p : Properties.Resources.outline_save_black_18dp;
            pbLogo.Image = frm.useDarkmode ? Properties.Resources.list_white_24px_1 : Properties.Resources.list_24px;
            pbClose.Image = frm.useDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;

            p.Color = font;
        }


        private void btnUseReal_Click(object sender, EventArgs e)
        {
            tbHWID.Text = originalHWID;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            using (var sha1 = SHA1.Create())
            {
                tbHWID.Text = string.Join("", sha1.ComputeHash(Encoding.UTF8.GetBytes(RandomString(16))).Select(b => b.ToString("x2")));
            }
        }
        
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ui == null)
            {
                // Should I add a fakeHWID
                //if (File.Exists(frm.optionsPath))
                //{
                //    try
                //    {
                //        OptionsData opt = (OptionsData)frm.ByteArrayToObject(File.ReadAllBytes(frm.optionsPath));
                //        opt.defaultHWID = (tbHWID.Text.Equals(originalHWID) ? string.Empty : tbHWID.Text);
                //        File.WriteAllBytes(frm.optionsPath, frm.ObjectToByteArray(opt));
                //    }
                //    catch { }
                //}
            }
            else
            {
                ui.accountInfo.customClientID = (tbHWID.Text.Equals(originalHWID) ? string.Empty : tbHWID.Text);
                
                frm.accounts[frm.accounts.IndexOf(ui.accountInfo)] = ui.accountInfo = frm.GetAccountData(ui.accountInfo);
                frm.SaveAccounts();
            }
            this.Close();
        }

        private void btnSave_MouseEnter(object sender, EventArgs e) => btnSave.Image = frm.useDarkmode ? Properties.Resources.ic_save_white_18dp : Properties.Resources.ic_save_black_18dp;
        private void btnSave_MouseLeave(object sender, EventArgs e) => btnSave.Image = frm.useDarkmode ? Properties.Resources.save_18p : Properties.Resources.outline_save_black_18dp;

        Pen p = new Pen(Color.Black);
        private void FrmHWIDchanger_Paint(object sender, PaintEventArgs e)
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

            e.Graphics.DrawLine(p, topRight, topLeft);
            e.Graphics.DrawLine(p, topLeft, lowerLeft);
            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
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

        private void Frm_Closing(object sender, FormClosingEventArgs e) => frm.lockForm = false;

        private void pbClose_Click(object sender, EventArgs e) => this.Close();
        private void pbClose_MouseEnter(object sender, EventArgs e) => pbClose.BackColor = frm.useDarkmode ? Color.FromArgb(225, 50, 50) : Color.IndianRed;
        private void pbClose_MouseLeave(object sender, EventArgs e) => pbClose.BackColor = pTop.BackColor;

        private void tbHWID_TextChanged(object sender, EventArgs e) => btnUseReal.Enabled = !tbHWID.Text.Equals(originalHWID);
    }
}
