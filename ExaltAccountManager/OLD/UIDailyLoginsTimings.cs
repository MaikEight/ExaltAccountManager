using MK_EAM_Lib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class UIDailyLoginsTimings : UserControl
    {
        FrmDailyLoginOptions frmLogins;
        NotificationOptions opt;

        public UIDailyLoginsTimings(FrmDailyLoginOptions _frmLogins, NotificationOptions _opt)
        {
            InitializeComponent();

            frmLogins = _frmLogins;
            opt = _opt;

            if (opt != null)
            {
                tbJointTime.Text = opt.joinTime.ToString();
                tbKillTime.Text = opt.killTime.ToString();
            }

            ApplyTheme(frmLogins._isDarkmode);
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

                lStoJoin.ForeColor = lStillKill.ForeColor = lBack.ForeColor = font;

                pbBack.Image = Properties.Resources.ic_arrow_back_white_24dp;
                pSpacer.BackColor = Color.White;

                btnSave.Image = Properties.Resources.ic_save_white_18dp;
                btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 225, 225, 225);
            }
        }

        private void pbBack_MouseEnter(object sender, EventArgs e)
        {
            if (frmLogins._isDarkmode)
                pbBack.BackColor = Color.DimGray;
            else
                pbBack.BackColor = Color.DarkGray;
        }

        private void pbBack_MouseLeave(object sender, EventArgs e) => pbBack.BackColor = Color.Transparent;

        private void pbBack_Click(object sender, EventArgs e) => frmLogins.CloseTimings();

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                frmLogins.LogEvent(new LogData(0, "EAM Time", LogEventType.TaskTime, $"Saving task timings."));

                int.TryParse(tbJointTime.Text, out int j);
                int.TryParse(tbKillTime.Text, out int k);

                frmLogins.SaveTimingData((j > 0) ? j : 90, (k > 0) ? k : 30);
                frmLogins.CloseTimings();
            }
            catch 
            {
                frmLogins.LogEvent(new LogData(0, "EAM Time", LogEventType.Error, $"Failed to save task timings."));
            }
        }
    }
}
