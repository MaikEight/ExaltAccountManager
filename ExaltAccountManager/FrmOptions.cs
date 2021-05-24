using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmOptions : Form
    {
        FrmMain frm;
        Pen p = new Pen(Color.Black);

        public FrmOptions(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            checkCloseAfterConnect.Checked = frm.closeAfterConnect;
            lPath.Text = frm.exePath;

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(-3f, -11f, lSave.Width + 6, lSave.Height + 22);
            Region rg = new Region(gp);
            lSave.Region = rg;

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
                lPath.BackColor = def;
                lPath.ForeColor = font;
                pTop.BackColor = def;
                pBox.BackColor = def;

                pbLogo.Image = Properties.Resources.ic_settings_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;

                btnChangeExePath.Image = Properties.Resources.ic_folder_open_white_18dp;
                btnAddMuledump.Image = Properties.Resources.ic_add_to_photos_white_18dp;
                btnChangeExePath.FlatAppearance.MouseOverBackColor = btnAddMuledump.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 225, 225, 225);
                p = new Pen(Color.White);
            }
        }

        private void checkCloseAfterConnect_CheckedChanged(object sender, EventArgs e)
        {
            frm.closeAfterConnect = checkCloseAfterConnect.Checked;
            SaveOptions();
        }

        private void btnChangeExePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "*.exe|*.exe";
            diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            diag.Title = "Select the RotMG Exalt.exe";
            diag.Multiselect = false;

            if (diag.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(diag.FileName))
                {
                    frm.exePath = diag.FileName;
                    lPath.Text = diag.FileName;
                    SaveOptions();
                }
            }
        }

        private void SaveOptions()
        {
            OptionsData opt = new OptionsData();
            opt.closeAfterConnection = frm.closeAfterConnect;
            opt.exePath = frm.exePath;
            frm.SaveOptions(opt);
        }

        #region Button Close / Minimize
        private void pbMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void pbClose_Click(object sender, EventArgs e) => this.Close();

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbClose.BackColor = Color.FromArgb(225, 50, 50);
            else
                pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e) => pbClose.BackColor = Color.Transparent;

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbMinimize.BackColor = Color.DimGray;
            else
                pbMinimize.BackColor = Color.DarkGray;
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e) => pbMinimize.BackColor = Color.Transparent;
        #endregion

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

        private void lSave_Click(object sender, EventArgs e)
        {
            SaveOptions();
            this.Close();
        }

        private void btnAddMuledump_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "*.js|*.js|*.*|*.*";
            diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            diag.Title = "Select your muledump accounts.js";
            diag.Multiselect = false;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(diag.FileName))
                {
                    string[] rows = File.ReadAllLines(diag.FileName);
                    bool active = false;
                    List<MuledumpAccounts> accs = new List<MuledumpAccounts>();

                    for (int i = 0; i < rows.Length; i++)
                    {
                        if (rows[i].TrimStart().StartsWith("accounts = {"))
                            active = true;
                        else if (active)
                        {
                            if (rows[i].TrimStart().StartsWith("}"))
                                break;
                            if (rows[i].TrimStart().StartsWith("'"))
                            {
                                string[] data = rows[i].Split('\'');
                                MuledumpAccounts acc = new MuledumpAccounts
                                {
                                    mail = data[1],
                                    password = data[3]
                                };
                                accs.Add(acc);
                            }
                        }
                    }

                    foreach (MuledumpAccounts a in accs)
                    {
                        AccountInfo i = new AccountInfo(a);

                        if (checkBoxAutoFill.Checked)
                        {
                            try
                            {
                                i = frm.GetAccountData(i);
                            }
                            catch { }
                        }

                        frm.accounts.Add(i);
                    }

                    frm.UpdateAccountInfos();
                    this.Close();
                }
            }
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

        private void FrmOptions_Paint(object sender, PaintEventArgs e)
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

        private void pbMinimize_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
        }

        private void pbClose_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
        }

        private void lSave_MouseEnter(object sender, EventArgs e)
        {
            lSave.BackColor = frm.useDarkmode ? Color.FromArgb(48, 48, 48) : Color.FromArgb(230, 230, 230);
        }

        private void lSave_MouseLeave(object sender, EventArgs e) => lSave.BackColor = Color.Transparent;

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }
    }

    [System.Serializable]
    public class OptionsData
    {
        public string exePath;
        public bool closeAfterConnection;
        public bool useDarkmode;
    }

    [System.Serializable]
    public class MuledumpAccounts
    {
        public string mail;
        public string password;
    }
}
