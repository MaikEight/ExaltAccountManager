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
    public partial class FrmHelp : Form
    {
        List<string> questions = new List<string>()
        {
            $"Why does Exalt not start?",

            $"Exalt tells me something about a token...",

            $"The Daily login task fails to install.",

            $"The Daily login task is installed but won't start!",

            $"The Daily login task ran successfully but my accounts did not get the login-reward!",

            $"The account-list contains empty spots or does behave strange while dragging.",

            $"Some accounts seem to be corrupt / The Daily login didn't work for them."
        };

        List<string> answers = new List<string>()
        {
            $"If it for some reason does not start exalt, check if you have exalt installed. - if so, check if the path in the options is set correctly.{Environment.NewLine}{Environment.NewLine}" +
            $"If you already have started the game via the Exalt launcher, close all running Exalt sessions, close the Exalt Launcher and try again.",

            $"Try to use the \"Renew\"-Button to the right of the \"Play\"-Button, this will request a new token for that account.{Environment.NewLine}{Environment.NewLine}" +
            $"If it still does not work, delete and re-add the account.",

            $"Depending on the error-message I can help you with that, please write the error message you get here.{Environment.NewLine}" +
            $"You can also install it manually using the Windows Task Scheduler(mmc.exe).",

            $"This is a bit more tricky, try to go into the EAM folder > DailyService and start the \"EAM Daily Login Service.exe\" manually - does it work now?{Environment.NewLine}Do you have accounts checked to be used for the Daily-Login in the Account-list?{Environment.NewLine}Try to delete the EAM.DailyLogins file, open the ExaltAccountManager after that and uncheck-recheck one account (re-creats the EAM.DailyLogins file), after that start the \"EAM Daily Login Service.exe\" manually again.{Environment.NewLine}If it works now, check if the task is installed correctly - some AVs block that",

            $"Check if there is an update for the unity client available, since I start it (hidden), it won't login if it is outdated.{Environment.NewLine}You need to start the game once via the original launcher after each game-update.",

            $"Two options to fix this:{Environment.NewLine}A: Remove the accounts in question and re-add him.{Environment.NewLine}B: Delete the file \"EAM.accountOrders\" and restart the EAM.{Environment.NewLine}Note: This will reset your account-order to default.",

            $"Only fix there seems to be is to remove and re-add the accounts in question."
        };

        FrmMain frm;
        public FrmHelp(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            LoadUIs();

            ApplyTheme(frm.useDarkmode);
        }

        private void LoadUIs()
        {
            bool isSecond = false;
            for (int i = 0; i < questions.Count; i++)
            {
                HelpUI ui = new HelpUI(questions[i], answers[i], isSecond);
                ui.ApplyTheme(frm.useDarkmode);
                flow.Controls.Add(ui);
                flow.SetFlowBreak(ui, true);
                isSecond = !isSecond;
            }
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
                this.BackColor = third;
                pTop.BackColor = def;
                pBox.BackColor = def;

                lHeadline.ForeColor = font;

                pbLogo.Image = Properties.Resources.ic_live_help_white_36dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                p = new Pen(Color.White);

                //foreach (HelpUI ui in flow.Controls.OfType<HelpUI>())                
                //    ui.ApplyTheme(isDarkmode);                
            }
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
        private void FrmHelp_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
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

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }
    }
}
