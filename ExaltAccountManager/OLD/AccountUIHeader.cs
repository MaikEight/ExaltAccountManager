using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class AccountUIHeader : UserControl
    {
        public FrmMainOLD frm;

        public AccountUIHeader()
        {
            InitializeComponent();

            pbColor.Image = pbColor.InitialImage = pbColor.BackgroundImage = pbColor.ErrorImage = null;

            toolTip.SetToolTip(lAccountName, "Click to order the accounts by name alphabetically");
            toolTip.SetToolTip(lEmail, "Click to order the accounts by email alphabetically ");            
        }

        public void SetFrmMain(FrmMainOLD _frm)
        {
            frm = _frm;            
        }

        public void ApplyTheme(bool isDarkmode)
        {
            Color font = isDarkmode ? Color.White : Color.Black;

            pbFilterList.Image = isDarkmode ? Properties.Resources.ic_format_line_spacing_white_36dp : Properties.Resources.ic_format_line_spacing_black_36dp;
            pbColor.BackColor = isDarkmode ? Color.White : Color.Black;

            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = Color.FromArgb(225, font.R, font.G, font.B);

            toolTip.BackColor = isDarkmode ? Color.FromArgb(32, 32, 32) : Color.FromArgb(255, 255, 255);
        }

        public bool ScrollbarStateChanged(bool isShown)
        {
            if (isShown)
            {
                //pDaily.Width = 69;
                //pActions.Width = 65;
                pDaily.Width = 56;
                //pActions.Width = 78;
                pEmail.Width = 180;
            }
            else
            {
                pDaily.Width = 48;
                //pActions.Width = 86;
                pEmail.Width = 191;
            }
            return isShown;
        }

        private void lAccountName_Click(object sender, EventArgs e)
        {
            if (frm == null || frm.lockForm)
                return;

            frm.ShowMoreUI(true, true);
            frm.ShowSortAlphabeticalUI(pAccountName.Left + ((lAccountName.Width - 26) / 2) + 4, false);
        }

        private void lEmail_Click(object sender, EventArgs e)
        {
            if (frm == null || frm.lockForm)
                return;

            frm.ShowMoreUI(true, true);
            frm.ShowSortAlphabeticalUI(pEmail.Left + ((lEmail.Width - 26) / 2) + 6, true);
        }
    }
}
