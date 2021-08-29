using MK_EAM_Lib;
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
        string savedServerToJoin = string.Empty;

        public FrmOptions(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            savedServerToJoin = frm.serverToJoin;

            toggleCloseAfterConnect.Checked = frm.closeAfterConnect;
            toggleRotmgUpdates.Checked = frm.notificationValues[0];
            toggleEAMUpdate.Checked = frm.notificationValues[1];
            toggleMessages.Checked = frm.notificationValues[2];
            toggleKillswitch.Checked = frm.notificationValues[3];

            lPath.Text = frm.exePath;
            lDefaultServer.Text = (string.IsNullOrEmpty(frm.serverToJoin) || frm.serverToJoin.Equals("Last")) ? "Last server (Deca default)" : frm.serverToJoin;

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(-3f, -11f, lSave.Width + 6, lSave.Height + 22);
            Region rg = new Region(gp);
            lSave.Region = rg;

            toolTip.SetToolTipIcon(pbHelp, Properties.Resources.ic_help_outline_black_36dp);
            toolTip.SetToolTipTitle(pbHelp, "Notifications");
            toolTip.SetToolTip(pbHelp, $"{Environment.NewLine}<b>Messages</b>{Environment.NewLine}" +
                                       $"Informations about potential risks.{Environment.NewLine}{Environment.NewLine}" +
                                       $"<b>Warnings</b>{Environment.NewLine}" +
                                       $"Informations about bans that may occured due to EAM.{Environment.NewLine}{Environment.NewLine}" +
                                       $"<b>Killswitch</b>{Environment.NewLine}" +
                                       $"The killswitch will be activated if there are proven bans caused by the EAM.{Environment.NewLine}" +
                                       $"<b>DON'T DEACTIVATE THIS UNLESS YOU KNOW WHAT YOU ARE DOING!</b>");

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
                lPath.BackColor = lDefaultServer.BackColor = def;
                lPath.ForeColor = lDefaultServer.ForeColor = font;
                pTop.BackColor = def;
                pBox.BackColor = def;

                pbLogo.Image = Properties.Resources.ic_settings_white_48dp;
                pbHelp.Image = Properties.Resources.ic_help_outline_white_36dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;

                btnChangeExePath.Image = Properties.Resources.ic_folder_open_white_18dp;

                btnChangeServer.Image = Properties.Resources.list_white_24px_1;
                btnChangeExePath.FlatAppearance.MouseOverBackColor = btnChangeServer.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 225, 225, 225);

                toggleCloseAfterConnect.ForeColor =
                toggleCloseAfterConnect.ToggleStateOn.BorderColor =
                toggleCloseAfterConnect.ToggleStateOn.BackColor =
                toggleRotmgUpdates.ForeColor =
                toggleRotmgUpdates.ToggleStateOn.BorderColor =
                toggleRotmgUpdates.ToggleStateOn.BackColor =
                toggleEAMUpdate.ForeColor =
                toggleEAMUpdate.ToggleStateOn.BorderColor =
                toggleEAMUpdate.ToggleStateOn.BackColor =
                toggleMessages.ForeColor =
                toggleMessages.ToggleStateOn.BorderColor =
                toggleMessages.ToggleStateOn.BackColor =
                toggleKillswitch.ForeColor =
                toggleKillswitch.ToggleStateOn.BorderColor =
                toggleKillswitch.ToggleStateOn.BackColor = font;

                toggleCloseAfterConnect.ToggleStateOn.BorderColorInner =
                toggleCloseAfterConnect.ToggleStateOn.BackColorInner =
                toggleRotmgUpdates.ToggleStateOn.BorderColorInner =
                toggleRotmgUpdates.ToggleStateOn.BackColorInner =
                toggleEAMUpdate.ToggleStateOn.BorderColorInner =
                toggleEAMUpdate.ToggleStateOn.BackColorInner =
                toggleMessages.ToggleStateOn.BorderColorInner =
                toggleMessages.ToggleStateOn.BackColorInner =
                toggleKillswitch.ToggleStateOn.BorderColorInner =
                toggleKillswitch.ToggleStateOn.BackColorInner = def;

                toggleMessages.ToggleStateOff.BackColor = toggleMessages.ToggleStateOff.BorderColor = Color.Orange;

                toolTip.TitleForeColor = font;
                toolTip.TextForeColor = Color.FromArgb(225, font.R, font.G, font.B);
                toolTip.BackColor = def;
                toolTip.SetToolTipIcon(pbHelp, Properties.Resources.ic_help_outline_white_36dp);

                p = new Pen(Color.White);
            }
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
                    lPath.Text = diag.FileName;
                }
            }
        }

        private void SaveOptions()
        {
            frm.exePath = lPath.Text;
            frm.notificationValues = new bool[]
            {
                toggleRotmgUpdates.Checked,
                toggleEAMUpdate.Checked,
                toggleMessages.Checked,
                toggleKillswitch.Checked
            };
            OptionsData opt = new OptionsData()
            {
                closeAfterConnection = toggleCloseAfterConnect.Checked,
                exePath = frm.exePath,
                serverToJoin = frm.serverToJoin,
                useDarkmode = frm.useDarkmode,
                searchRotmgUpdates = toggleRotmgUpdates.Checked,
                searchUpdateNotification = toggleEAMUpdate.Checked,
                searchWarnings = toggleMessages.Checked,
                deactivateKillswitch = toggleKillswitch.Checked
            };
            frm.SaveOptions(opt, true);

            savedServerToJoin = (frm.serverToJoin.Equals("Last server (Deca default)")) ? "Last" : frm.serverToJoin;
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
                        MK_EAM_Lib.AccountInfo i = new MK_EAM_Lib.AccountInfo(a);

                        //if (checkBoxAutoFill.Checked)
                        //{
                        //    try
                        //    {
                        //        i = frm.GetAccountData(i);
                        //    }
                        //    catch { }
                        //}

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
            frm.serverToJoin = savedServerToJoin;
        }

        private void toggleCloseAfterConnect_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            //frm.closeAfterConnect = toggleCloseAfterConnect.Checked;
            //SaveOptions();
        }

        private void btnChangeExePath_MouseEnter(object sender, EventArgs e)
        {
            btnChangeExePath.Image = frm.useDarkmode ? Properties.Resources.ic_folder_white_18dp : Properties.Resources.ic_folder_black_18dp;
        }

        private void btnChangeExePath_MouseLeave(object sender, EventArgs e)
        {
            btnChangeExePath.Image = frm.useDarkmode ? Properties.Resources.ic_folder_open_white_18dp : Properties.Resources.ic_folder_open_black_18dp;
        }

        private void btnChangeServer_Click(object sender, EventArgs e)
        {
            frm.ShowServerListUI(null, this.Left + btnChangeServer.Left, this.Top + btnChangeServer.Top / 2);
            lDefaultServer.Text = frm.serverToJoin;
        }

        private void lCloseAfter_Click(object sender, EventArgs e) => toggleCloseAfterConnect.Checked = !toggleCloseAfterConnect.Checked;

        private void lRotmgUpdates_Click(object sender, EventArgs e) => toggleRotmgUpdates.Checked = !toggleRotmgUpdates.Checked;

        private void lEAMUpdate_Click(object sender, EventArgs e) => toggleEAMUpdate.Checked = !toggleEAMUpdate.Checked;

        private void lMessages_Click(object sender, EventArgs e) => toggleMessages.Checked = !toggleMessages.Checked;

        private void lKillswitch_Click(object sender, EventArgs e) => toggleKillswitch.Checked = !toggleKillswitch.Checked;
    }

    [System.Serializable]
    public class OptionsData
    {
        public string exePath = string.Empty;
        public bool closeAfterConnection = false;
        public bool useDarkmode = true;
        public string serverToJoin = "Last";

        public bool searchRotmgUpdates = true;
        public bool searchUpdateNotification = true;
        public bool searchWarnings = true;
        public bool deactivateKillswitch = true;
    }
}
