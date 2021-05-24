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
        }        

        bool clicked = false;
        private void btnShowMore_Click(object sender, EventArgs e)
        {
            if (clicked) return;
            
            clicked = true;
            frm.ShowMore();
        }
    }
}
