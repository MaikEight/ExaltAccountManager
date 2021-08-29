using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmServerListChanger : Form
    {
        AccountUI ui;
        FrmMain frm;
        bool isInit = false;

        public FrmServerListChanger(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            LoadServers();
            ApplyTheme();
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
            this.ForeColor = dropServers.ForeColor = font;
            dropServers.ItemBackColor = dropServers.BackgroundColor = def;
            dropServers.ItemForeColor = font;

            pbLogo.Image = frm.useDarkmode ? Properties.Resources.list_white_24px_1 : Properties.Resources.list_24px;
            pbClose.Image = frm.useDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;

            p.Color = font;
        }

        public void ShowUI(AccountUI _ui)
        {
            isInit = true;
            ui = _ui;
            string item = string.Empty;

            if (ui != null)
            {
                item = ui.accountInfo.serverName;

                if (!frm.screenshotMode)
                    lDropHeadline.Text = $"Choose the server {ui.accountInfo.name} is going to join";
                else
                    lDropHeadline.Text = $"Choose the server AccountNameHere is going to join";
            }
            else
            {
                lDropHeadline.Text = $"Choose the EAM default server";
                item = frm.serverToJoin;
            }
            if (item == null)
                item = string.Empty;

            if (dropServers.Items.Contains(item))
                dropServers.SelectedItem = item;
            else
            {
                if (item.Equals("Last"))
                    dropServers.SelectedIndex = 1;
                else
                    dropServers.SelectedIndex = 0;
            }
            isInit = false;
        }

        public void LoadServers()
        {
            dropServers.Items.Clear();
            dropServers.Items.Add("EAM default server(Set in Options)");
            dropServers.Items.Add("Last server (Deca default)");

            if (frm.serverData != null)
            {
                for (int i = 0; i < frm.serverData.servers.Count; i++)
                    dropServers.Items.Add(frm.serverData.servers[i].name);
            }
        }

        Pen p = new Pen(Color.Black);
        private void FrmServerListChanger_Paint(object sender, PaintEventArgs e)
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

        private void dropServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            if (ui != null)
            {
                switch (dropServers.SelectedIndex)
                {
                    case 0:
                        ui.accountInfo.serverName = string.Empty;
                        break;
                    case 1:
                        ui.accountInfo.serverName = "Last";
                        break;
                    default:
                        ui.accountInfo.serverName = dropServers.SelectedItem.ToString();
                        break;
                }
                frm.accounts[frm.accounts.IndexOf(ui.accountInfo)].serverName = ui.accountInfo.serverName;
                frm.SaveAccounts();
            }
            else
            {
                isInit = true;
                if (dropServers.SelectedIndex == 0)
                    dropServers.SelectedIndex = 1;
                isInit = false;

                frm.serverToJoin = dropServers.SelectedItem.ToString();
            }
            pbClose_Click(pbClose, null);
        }

        private void pbClose_Click(object sender, EventArgs e) => this.Close();
        private void pbClose_MouseEnter(object sender, EventArgs e) => pbClose.BackColor = frm.useDarkmode ? Color.FromArgb(225, 50, 50) : Color.IndianRed;
        private void pbClose_MouseLeave(object sender, EventArgs e) => pbClose.BackColor = pTop.BackColor;

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            if (ui != null)
                frm.lockForm = false;
        }
    }
}
