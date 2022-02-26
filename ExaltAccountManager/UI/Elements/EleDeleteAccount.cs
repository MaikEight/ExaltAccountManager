using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleDeleteAccount : UserControl
    {
        FrmMain frm;
        UIAccounts uiAccounts;
        MK_EAM_Lib.AccountInfo accountInfo;

        public EleDeleteAccount(FrmMain _frm, UIAccounts _uiAccounts)
        {
            InitializeComponent();
            frm = _frm;
            uiAccounts = _uiAccounts;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            this.BackColor = ColorScheme.GetColorDef(frm.UseDarkmode);
            this.ForeColor = 
            lRemove.ForeColor = lKeep.ForeColor = ColorScheme.GetColorFont(frm.UseDarkmode);
        }

        public void ShowUI(MK_EAM_Lib.AccountInfo info)
        {
            accountInfo = info;
            lHint.Text = $"This will remove the account {info.email} from EAM.";
            this.Visible = true;
        }

        private void btnDeleteAccount_MouseEnter(object sender, EventArgs e)
        {
            btnDeleteAccount.Image = Properties.Resources.ic_delete_forever_white_36dp;
            lRemove.ForeColor = Color.Crimson;
        }

        private void btnDeleteAccount_MouseLeave(object sender, EventArgs e)
        {
            btnDeleteAccount.Image = Properties.Resources.baseline_delete_outline_white_36dp;
            lRemove.ForeColor = this.ForeColor;
        }

        private void btnBack_MouseEnter(object sender, EventArgs e)
        {
            btnBack.Image = Properties.Resources.u_turn_to_left_36px;
            lKeep.ForeColor = Color.FromArgb(85, 85, 85);
        }

        private void btnBack_MouseLeave(object sender, EventArgs e)
        {
            btnBack.Image = Properties.Resources.return_36_white;
            lKeep.ForeColor = this.ForeColor;
        }

        private void btnBack_Click(object sender, EventArgs e) => uiAccounts.DeleteAccount(accountInfo, false);

        private void btnDeleteAccount_Click(object sender, EventArgs e) => uiAccounts.DeleteAccount(accountInfo, true);
    }
}
