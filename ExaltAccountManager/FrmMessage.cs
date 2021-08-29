using MK_EAM_Lib;
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
    public partial class FrmMessage : Form
    {
        FrmMain frm;

        public FrmMessage(FrmMain _frm, EAMNotificationMessage msg)
        {
            InitializeComponent();
            frm = _frm;

            lMessage.Text = msg.message;

            if (frm.isMPGHVersion ? !string.IsNullOrEmpty(msg.linkM) : !string.IsNullOrEmpty(msg.link))
            {
                link.Visible = true;
                link.Text = frm.isMPGHVersion ? msg.linkM : msg.link;

                link.Top = lMessage.Bottom + 6;
                this.Height = link.Bottom + 12;
            }
            else
                this.Height = lMessage.Bottom + 12;

            ApplyTheme(frm.useDarkmode);            

            switch (msg.type)
            {
                case EAMNotificationMessageType.UpdateAvailable:
                    {
                        timerClose_Tick(null, null);
                    }
                    break;
                case EAMNotificationMessageType.Warning:
                    {
                        timerClose.Stop();
                        timerClose.Interval = 5000;
                        lCloseBlock.Text = $"Close button will be{Environment.NewLine}available after 5 seconds.";
                        timerClose.Start();
                    }
                    break;
                case EAMNotificationMessageType.Stop:
                    {
                        lHeadline.ForeColor = Color.Crimson;
                        timerClose.Stop();
                        lCloseBlock.Text = "";
                        this.FormClosing -= Frm_Closing;
                        this.FormClosing += Frm_ClosingSTOP;
                    }
                    break;
                default:
                    timerClose.Start();
                    break;
            }

            this.TopMost = true;
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
                this.BackColor = second;
                pTop.BackColor = def;
                pBox.BackColor = def;

                pbLogo.Image = Properties.Resources.ic_new_releases_white_48dp;
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
        private void FrmMessage_Paint(object sender, PaintEventArgs e)
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

        private void Frm_ClosingSTOP(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = true;
            e.Cancel = true;
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            timerClose.Stop();

            lCloseBlock.Visible = false;
            lCloseBlock.Dispose();

            pbClose.Enabled = true;
            pBox.BringToFront();

            this.Invalidate();
        }

        private void link_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(link.Text);
        }
    }
}
