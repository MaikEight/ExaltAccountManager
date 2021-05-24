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
    public partial class FrmAbout : Form
    {
        FrmMain frm;
        public FrmAbout(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            lVersion.Text = $"v{frm.version}";
            ApplyTheme(frm.useDarkmode);

            //System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            //gp.AddEllipse(-3f, -11f, pbDevName.Width + 6, pbDevName.Height + 22);
            //Region rg = new Region(gp);
            //pbDevName.Region = rg;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            int h = pbDevName.Height;
            int w = pbDevName.Width;
            float dividerW = 16f;
            float dividerH = 4f;

            gp.AddPolygon(new PointF[]
            {
                new PointF(w / dividerW, 0),
                new PointF(w - (w / dividerW), 0),
                new PointF(w, h / dividerH),
                new PointF(w, h - (h / dividerH)),
                new PointF(w - (w / dividerW), h),
                new PointF(w / dividerW, h),
                new PointF(0, h - (h / dividerH)),
                new PointF(0, h / dividerH),
            });
            Region rg = new Region(gp);
            pbDevName.Region = rg;
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

                pbLogo.Image = Properties.Resources.ic_info_outline_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;

                pbProgram.Image = Properties.Resources.ic_account_balance_wallet_white_36dp;
                pbVersion.Image = Properties.Resources.baseline_tag_white_36dp;
                pbDev.Image = Properties.Resources.ic_code_white_36dp;
                pbWeb.Image = Properties.Resources.ic_public_white_36dp;

                p = new Pen(Color.White);
            }
        }

        private void linkMPGH_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkMPGH.Text);
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

        private void FrmAbout_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
            e.Graphics.DrawLine(p, new Point(lPerms.Left + 15, lPerms.Top - 5), new Point(lPerms.Right - 15, lPerms.Top - 5));
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

        private void lDev_Click(object sender, EventArgs e)
        {
            frm.lDev_Click(null, e);
            pbDev.Image = Properties.Resources.llama;
            if (timerLlama.Enabled)
                timerLlama.Stop();
            timerLlama.Start();
        }

        private void timerLlama_Tick(object sender, EventArgs e)
        {
            timerLlama.Stop();
            pbDev.Image = (frm.useDarkmode) ? Properties.Resources.ic_code_white_36dp : Properties.Resources.ic_code_black_36dp;
        }

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }        
    }
}
