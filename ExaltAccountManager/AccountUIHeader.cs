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
        public AccountUIHeader()
        {
            InitializeComponent();
        }

        public void ApplyTheme(bool isDarkmode)
        {
            pbFilterList.Image = isDarkmode ? Properties.Resources.ic_format_line_spacing_white_36dp : Properties.Resources.ic_format_line_spacing_black_36dp;
        }

        public void ScrollbarStateChanged(bool isShown)
        {
            if (isShown)
            {
                pDaily.Width = 69;
                pActions.Width = 65;
            }
            else
            {
                pDaily.Width = 48;
                pActions.Width = 86;
            }
        }
    }
}
