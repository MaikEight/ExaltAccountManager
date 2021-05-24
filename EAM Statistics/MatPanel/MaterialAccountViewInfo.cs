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

namespace EAM_Statistics.MatPanel
{
    public partial class MaterialAccountViewInfo : UserControl
    {
        StatsMain stats;
        UIAccountView view;
        public MaterialAccountViewInfo(UIAccountView _view, StatsMain _stats, int num)
        {
            InitializeComponent();
            stats = _stats;
            view = _view;

            lAccounts.Text = string.Format(lAccounts.Text, num + 1);
            LoadUI();
            ApplyTheme(view.GetDarkmode());
        }

        private void LoadUI()
        {
            if (stats == null) return;

            lAccName.Text = view.GetAccountName(stats.email);
            lAccChars.Text = string.Format(lAccChars.Text, stats.charList.Count > 0 ? stats.charList[stats.charList.Count - 1].chars.Count : 0);
            int amountOf8 = 0;
            int amountOfAlive = 0;
            for (int i = 0; i < (stats.charList.Count > 0 ? stats.charList[stats.charList.Count - 1].chars.Count : 0); i++)
            {
                stats.charList[stats.charList.Count - 1].chars[i].CalculateXof8();
                if (stats.charList[stats.charList.Count - 1].chars[i].xOf8 == 8)
                    amountOf8++;
                amountOfAlive += stats.charList[stats.charList.Count - 1].chars[i].fame;
            }
            lAcc8of8.Text = string.Format(lAcc8of8.Text, amountOf8);
            lAccAliveFame.Text = string.Format(lAccAliveFame.Text, amountOfAlive);
            lAccTotalFame.Text = string.Format(lAccTotalFame.Text, stats.stats.Count > 0 ? stats.stats[stats.stats.Count - 1].totalFame : 0);
        }

        public void ApplyTheme(bool useDarkmode)
        {
            Color def = Color.FromArgb(253, 253, 253);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (useDarkmode)
            {
                def = Color.FromArgb(30, 30, 30);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            //pMain.PanelColor = pMain.PanelColor2 = def;
            //pMain.ShadowColor = useDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
            lAccounts.BackColor = lAccName.BackColor = lAccChars.BackColor = lAcc8of8.BackColor = lAccAliveFame.BackColor = lAccTotalFame.BackColor = def;
            mtpAccount.ApplyTheme(useDarkmode);
            this.ForeColor = font;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            lAccName.Focus();

            view.OpenAccountViewer(stats);
        }
    }
}
