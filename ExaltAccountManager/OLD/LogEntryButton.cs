using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class LogEntryButton : UserControl
    {
        FrmLogViewer frm;
        public LogEntryButton(FrmLogViewer _frm, int entries, int maxEntries)
        {
            InitializeComponent();
            frm = _frm;

            lEntries.Text = string.Format(lEntries.Text, entries, maxEntries);

            if (frm.isDarkmode)
            {
                this.BackColor = Color.FromArgb(0, 0, 0);
                this.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(230, 230, 230);
                this.ForeColor = Color.Black;
            }
            btnShowMore.Image = frm.isDarkmode ? Properties.Resources.ic_rotate_right_white_24dp : Properties.Resources.ic_rotate_right_black_24dp;
        }        

        bool clicked = false;
        private void btnShowMore_Click(object sender, EventArgs e)
        {
            if (clicked) return;
            
            clicked = true;
            frm.ShowMore();
        }

        private void btnShowMore_MouseEnter(object sender, EventArgs e) => btnShowMore.Image = frm.isDarkmode ? Properties.Resources.ic_refresh_white_24dp : Properties.Resources.ic_refresh_black_24dp;

        private void btnShowMore_MouseLeave(object sender, EventArgs e) => btnShowMore.Image = frm.isDarkmode ? Properties.Resources.ic_rotate_right_white_24dp : Properties.Resources.ic_rotate_right_black_24dp;
    }
}
