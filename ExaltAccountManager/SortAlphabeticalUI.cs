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
    public partial class SortAlphabeticalUI : UserControl
    {
        Color backHover = Color.FromArgb(25, 0, 0, 0);
        public bool useEmail = false;
        FrmMain frm;

        public SortAlphabeticalUI(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.ForeColor = p.Color = font;
                this.BackColor = pbAZ.BackColor = pbZA.BackColor = second;

                pbAZ.Image = Properties.Resources.alphabetical_sorting_az_white_24px;
                pbZA.Image = Properties.Resources.alphabetical_sorting_za_white_24px;

                backHover = Color.FromArgb(125, 75, 75, 75);
            }
            else
            {
                Color def = Color.FromArgb(255, 255, 255);
                Color second = Color.FromArgb(250, 250, 250);
                Color third = Color.FromArgb(230, 230, 230);
                Color font = Color.Black;

                this.ForeColor = p.Color = font;
                this.BackColor = pbAZ.BackColor = pbZA.BackColor = second;

                pbAZ.Image = Properties.Resources.alphabetical_sorting_az_24px;
                pbZA.Image = Properties.Resources.alphabetical_sorting_za_24px;

                backHover = Color.FromArgb(25, 0, 0, 0);
            }
            this.Invalidate();
        }

        Pen p = new Pen(Color.Black);

        private void SortAlphabeticalUI_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLines(p, new Point[] { topLeft, topRight, topRight, lowerRight, lowerRight, lowerLeft, lowerLeft, topLeft });
            e.Graphics.DrawLine(p, 0, pbAZ.Bottom, s.Width - 1, pbAZ.Bottom);
            //e.Graphics.DrawLine(p, topRight, lowerRight);
            //e.Graphics.DrawLine(p, lowerRight, lowerLeft);
            //e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        private void pbAZ_Click(object sender, EventArgs e)
        {
            frm.SortAccountsAlphabetical(true, useEmail);
        }

        private void pbZA_Click(object sender, EventArgs e)
        {
            frm.SortAccountsAlphabetical(false, useEmail);
        }

        private void pb_MouseEnter(object sender, EventArgs e)
        {
            (sender as PictureBox).BackColor = backHover;
        }

        private void pb_MouseLeave(object sender, EventArgs e)
        {
            (sender as PictureBox).BackColor = this.BackColor;
        }
    }
}
