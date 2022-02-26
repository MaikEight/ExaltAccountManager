using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class OptionsUI : UserControl
    {
        FrmMainOLD frm;

        public OptionsUI(FrmMainOLD _frm)
        {
            InitializeComponent();
            frm = _frm;
        }

        public void ApplyTheme(Color third, Color font)
        {
            this.BackColor = pOptionen.BackColor = lOptionen.BackColor = pAddAccount.BackColor = lAdd.BackColor = pDailyLogins.BackColor = lDaily.BackColor = pLog.BackColor = lLog.BackColor = third;
            this.ForeColor = font;
        }

        public void ShowOptions(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);

            if (frm.lockForm) return;

            //show them!
            FrmOptions frmOptions = new FrmOptions(frm);
            frmOptions.StartPosition = FormStartPosition.Manual;
            frmOptions.Location = new Point(frm.Location.X + ((frm.Width - frmOptions.Width) / 2), frm.Location.Y + ((frm.Height - frmOptions.Height) / 2));
            frmOptions.Show(frm);

            frm.lockForm = true;
        }

        public void AddAccount(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            //Show UI!
            FrmAddAccount frmAddAccount = new FrmAddAccount(frm);
            frmAddAccount.StartPosition = FormStartPosition.Manual;
            frmAddAccount.Location = new Point(frm.Location.X + ((frm.Width - frmAddAccount.Width) / 2), frm.Location.Y + ((frm.Height - frmAddAccount.Height) / 2));
            frmAddAccount.Show(frm);

            frm.lockForm = true;
        }

        private void addAccount_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            if (frm.useDarkmode)
            {
                pAddAccount.BackColor = 
                lAdd.BackColor = Color.FromArgb(48, 48, 48);
            }
            else
            {
                pAddAccount.BackColor = 
                lAdd.BackColor = Color.WhiteSmoke;
            }
        }

        private void addAccount_MouseLeave(object sender, EventArgs e)
        {
            pAddAccount.BackColor = 
            lAdd.BackColor = Color.Transparent;
        }

        private void options_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            if (frm.useDarkmode)
            {
                pOptionen.BackColor = 
                lOptionen.BackColor = Color.FromArgb(48, 48, 48);
            }
            else
            {
                pOptionen.BackColor = 
                lOptionen.BackColor = Color.WhiteSmoke;
            }
        }

        private void options_MouseLeave(object sender, EventArgs e)
        {
            pOptionen.BackColor = 
            lOptionen.BackColor = Color.Transparent;
        }

        private void lDaily_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            //Show them!
            FrmDailyLoginOptions frmDailyLogin = new FrmDailyLoginOptions(frm, frm.notOpt);
            frmDailyLogin.StartPosition = FormStartPosition.Manual;
            frmDailyLogin.Location = new Point(frm.Location.X + ((frm.Width - frmDailyLogin.Width) / 2), frm.Location.Y + ((frm.Height - frmDailyLogin.Height) / 2));
            frmDailyLogin.Show(frm);

            frm.lockForm = true;
        }

        private void lDaily_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            if (frm.useDarkmode)
            {
                pDailyLogins.BackColor = 
                lDaily.BackColor = Color.FromArgb(48, 48, 48);
            }
            else
            {
                pDailyLogins.BackColor = 
                lDaily.BackColor = Color.WhiteSmoke;
            }
        }

        private void lDaily_MouseLeave(object sender, EventArgs e)
        {
            pDailyLogins.BackColor = 
            lDaily.BackColor = Color.Transparent;
        }

        [STAThread]
        private void lLog_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);

            if (!allowLogClick) return;
            if (frm.lockForm) return;

            allowLogClick = false;

            //Show them!
            FrmLogViewer frmLogViewer = new FrmLogViewer(frm);
            frmLogViewer.StartPosition = FormStartPosition.Manual;
            frmLogViewer.Location = new Point(frm.Location.X - ((frmLogViewer.Width - frm.Width) / 2), frm.Location.Y - ((frmLogViewer.Height - frm.Height) / 2));
            frmLogViewer.Show();

            //var secondFormThread = new System.Threading.Thread(() => Application.Run(new FrmLogViewer(frm) { StartPosition = FormStartPosition.Manual, Location = new Point(frm.Location.X - ((800 - frm.Width) / 2), frm.Location.Y - ((500 - frm.Height) / 2)) }));
            //secondFormThread.Start();
            timerLogLock.Start();
        }

        private void lLog_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            if (frm.useDarkmode)
            {
                pLog.BackColor =
                lLog.BackColor = Color.FromArgb(48, 48, 48);
            }
            else
            {
                pLog.BackColor = 
                lLog.BackColor = Color.WhiteSmoke;
            }
        }

        private void lLog_MouseLeave(object sender, EventArgs e)
        {
            pLog.BackColor = 
            lLog.BackColor = Color.Transparent;
        }

        public void BlinkLog(Color clr)
        {
            pLog.BackColor = lLog.BackColor = clr;
            if (timerLogNews.Enabled)
                timerLogNews_Tick(timerLogNews, null);
            timerLogNews.Start();
        }

        private void timerLogNews_Tick(object sender, EventArgs e)
        {
            timerLogNews.Stop();
            pLog.BackColor = lLog.BackColor = this.BackColor;
        }

        private void lMore_Click(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            frm.ShowMoreUI();
        }

        private void lMore_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            if (frm.useDarkmode)
            {
                pMore.BackColor = 
                lMore.BackColor = Color.FromArgb(48, 48, 48);
            }
            else
            {
                pMore.BackColor = 
                lMore.BackColor = Color.WhiteSmoke;
            }
        }

        private void lMore_MouseLeave(object sender, EventArgs e)
        {
            pMore.BackColor = 
            lMore.BackColor = Color.Transparent;
        }

        private void lHelp_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            //Show them!
            FrmHelp frmHelp = new FrmHelp(frm);
            frmHelp.StartPosition = FormStartPosition.Manual;
            frmHelp.Location = new Point(frm.Location.X + ((frm.Width - frmHelp.Width) / 2), frm.Location.Y + ((frm.Height - frmHelp.Height) / 2));
            frmHelp.Show(frm);

            frm.lockForm = true;
        }

        private void lHelp_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            if (frm.useDarkmode)
            {
                pHelp.BackColor =
                lHelp.BackColor = Color.FromArgb(48, 48, 48);
            }
            else
            {
                pHelp.BackColor =
                lHelp.BackColor = Color.WhiteSmoke;
            }
        }

        private void lHelp_MouseLeave(object sender, EventArgs e)
        {
            pHelp.BackColor = 
            lHelp.BackColor = Color.Transparent;
        }

        bool allowLogClick = true;
        private void timerLogLock_Tick(object sender, EventArgs e)
        {
            allowLogClick = true;
            timerLogLock.Stop();
        }       
    }
}
