using MK_EAM_Lib;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmAddAccount : Form
    {
        AccountInfo info = new AccountInfo();
        FrmMain frm;
        bool addNew = true;
        int index = -1;
        string startEmail = string.Empty;

        public FrmAddAccount(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            lSave.Location = new Point((this.Width - lSave.Width) / 2, lSave.Location.Y);

            ChangeLSaveText("Save");

            ApplyTheme(frm.useDarkmode);
        }

        public FrmAddAccount(FrmMain _frm, AccountInfo _info)
        {
            InitializeComponent();
            frm = _frm;
            info = _info;
            addNew = false;
            lHeadline.Text = "Edit Account";
            lSave.Location = new Point((this.Width - lSave.Width) / 2, lSave.Location.Y);

            ChangeLSaveText("Save");

            tbName.Text = info.name;
            tbEmail.Text = info.email;
            tbPassword.Text = info.password;
            startEmail = info.email;

            index = frm.accounts.IndexOf(info);
            tbEmail.Focus();
            ApplyTheme(frm.useDarkmode);
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
                pTop.BackColor = def;
                pBox.BackColor = def;

                tbName.ForeColor = font;
                tbName.BackColor = def;

                tbEmail.ForeColor = font;
                tbEmail.BackColor = def;

                tbPassword.ForeColor = font;
                tbPassword.BackColor = def;

                pbLogo.Image = Properties.Resources.ic_person_add_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                p = new Pen(Color.White);
            }
        }

        private void lSave_MouseEnter(object sender, EventArgs e)
        {
            if (isWaiting) return;

            lSave.BackColor = frm.useDarkmode ? Color.FromArgb(48, 48, 48) : Color.FromArgb(230, 230, 230);
        }

        private void lSave_MouseLeave(object sender, EventArgs e)
        {
            if (isWaiting) return;

            lSave.BackColor = this.BackColor;
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

        bool isWaiting = false;
        private void lSave_Click(object sender, EventArgs e)
        {
            if (isWaiting) return;

            info.name = tbName.Text;
            info.email = tbEmail.Text;
            info.password = tbPassword.Text;

            if (string.IsNullOrEmpty(info.email) || string.IsNullOrEmpty(info.password))
            {
                ChangeLSaveText("E-Mail and password can't be empty!", 1);

                //lSave.Text = "E-Mail and password can't be empty!";
                //lSave.Location = new Point((this.Width - lSave.Width) / 2, lSave.Location.Y);
                lSave.BackColor = frm.useDarkmode ? Color.FromArgb(128, 200, 50, 50) : Color.FromArgb(175, 225, 0, 0);
                lInfo.Visible = false;
                isWaiting = true;
                timerResetSaveLabel.Start();
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(info.name))
                    info = frm.GetAccountData(info);
                else
                    info = frm.GetAccountData(info, false);

                if (string.IsNullOrEmpty(info.name) || !info.requestSuccessfull)
                {
                    ChangeLSaveText("Wrong E-Mail or password!", 1);
                    //lSave.Text = "Wrong E-Mail or password!";
                    //lSave.Location = new Point((this.Width - lSave.Width) / 2, lSave.Location.Y);
                    lSave.BackColor = frm.useDarkmode ? Color.FromArgb(128, 200, 50, 50) : Color.FromArgb(175, 225, 0, 0);
                    lInfo.Visible = false;
                    isWaiting = true;
                    timerResetSaveLabel.Start();
                    return;
                }

                int emailsInUse = frm.accounts.Where(o => o.email.Equals(info.email)).Count();
                if (startEmail.Equals(info.email))
                    emailsInUse--;
                if (emailsInUse > 0)
                {
                    ChangeLSaveText("E-Mail is already in use!", 1);
                    lSave.BackColor = frm.useDarkmode ? Color.FromArgb(128, 200, 50, 50) : Color.FromArgb(175, 225, 0, 0);
                    lInfo.Visible = false;
                    isWaiting = true;
                    timerResetSaveLabel.Start();
                    return;
                }

                if (addNew)
                {
                    frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM ADD", LogEventType.AddAccount, $"Adding new account: {info.email}"));
                    frm.accounts.Add(info);
                    frm.AddAccountToOrders(info.email);
                }
                else
                {
                    frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM Edit", LogEventType.EditAccount, $"Account edited: {info.email}"));
                    frm.accounts[index] = info;
                }

                frm.UpdateAccountInfos();
                this.Close();
            }
            catch (Exception)
            {
                ChangeLSaveText("Error occured!", 2);
                lSave.BackColor = frm.useDarkmode ? Color.FromArgb(128, 200, 50, 50) : Color.FromArgb(175, 225, 0, 0);
                //lSave.Location = new Point((this.Width - lSave.Width) / 2, lSave.Location.Y);
                isWaiting = true;
                timerResetSaveLabel.Start();
                frm.LogEvent(new LogData(frm.logs.Count + 1, (addNew) ? "EAM Add" : "EAM Edit", LogEventType.EAMError, $"Error while {((addNew) ? "adding a new" : "editing an")} account."));
            }
        }

        #region Drag Form
        private Point MouseDownLocation;
        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                this.Location = new Point(e.X + this.Left - MouseDownLocation.X, e.Y + this.Top - MouseDownLocation.Y);
        }
        #endregion

        private void timerResetSaveLabel_Tick(object sender, EventArgs e)
        {
            timerResetSaveLabel.Stop();
            ChangeLSaveText("Save");
            //lSave.Text = "Save";
            //lSave.Location = new Point((this.Width - lSave.Width) / 2, lSave.Location.Y);
            lInfo.Visible = true;
            isWaiting = false;

            lSave.BackColor = this.BackColor;
        }

        private void ChangeLSaveText(string text, int regionNumber = 0)
        {
            lSave.Text = text;
            lSave.Location = new Point((this.Width - lSave.Width) / 2, lSave.Location.Y);

            switch (regionNumber)
            {
                case 0:
                    {
                        System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                        gp.AddEllipse(-2f, -8f, lSave.Width + 4, lSave.Height + 16);
                        Region rg = new Region(gp);
                        lSave.Region = rg;
                    }
                    break;
                case 1:
                    {
                        System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                        gp.AddEllipse(-4.5f, -24f, lSave.Width + 9, lSave.Height + 48);
                        Region rg = new Region(gp);
                        lSave.Region = rg;
                    }
                    break;
                case 2:
                    {
                        System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                        gp.AddEllipse(-2.5f, -25f, lSave.Width + 5, lSave.Height + 50);
                        Region rg = new Region(gp);
                        lSave.Region = rg;
                    }
                    break;
                default:
                    break;
            }
        }

        Pen p = new Pen(Color.Black);
        private void FrmAddAccount_Paint(object sender, PaintEventArgs e)
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

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                lSave_Click(lSave, new EventArgs());
        }

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }
    }
}
