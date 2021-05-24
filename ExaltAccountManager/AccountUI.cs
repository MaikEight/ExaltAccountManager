using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MK_EAM_Lib;
using System.Diagnostics;

namespace ExaltAccountManager
{
    public partial class AccountUI : UserControl
    {
        FrmMain frm;
        public AccountInfo accountInfo;
        public bool isSecond = false;
        Process process = null;
        public bool isRunning = false;
        private bool isCreating = true;
        public AccountUI(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(-3, -3, pbEdit.Width + 6, pbEdit.Height + 6);
            Region rg = new Region(gp);
            pbEdit.Region = rg;

            System.Drawing.Drawing2D.GraphicsPath gp2 = new System.Drawing.Drawing2D.GraphicsPath();
            gp2.AddEllipse(-3, -3, pbDelete.Width + 6, pbDelete.Height + 6);
            Region rg2 = new Region(gp2);
            pbDelete.Region = rg2;

            accountInfo = new AccountInfo();

            isCreating = false;
        }

        public AccountUI(FrmMain _frm, AccountInfo _accountInfo)
        {
            InitializeComponent();
            frm = _frm;
            accountInfo = _accountInfo;

            lAccountName.Text = accountInfo.name;
            lEmail.Text = accountInfo.email;
            checkBox.Checked = (accountInfo.performSave == null) ? false : accountInfo.performSave;

            System.Drawing.Drawing2D.GraphicsPath grp = new System.Drawing.Drawing2D.GraphicsPath();
            int h = pbEdit.Height;
            int w = pbEdit.Width;
            float dividerW = 6f;
            float dividerH = 6f;

            grp.AddPolygon(new PointF[]
            {
                new PointF(w / dividerW, 0),
                new PointF(w - (w / dividerW), 0),
                new PointF(w, h / dividerH),
                new PointF(w, h - (h / dividerH)),
                new PointF(w - (w / dividerW), h),
                new PointF(w / dividerW, h),
                new PointF(0, h - (h / dividerH)),
                new PointF(0, h / dividerH),
            });
            Region reg = new Region(grp);
            pbEdit.Region = reg;
            pbDelete.Region = reg;

            grp = new System.Drawing.Drawing2D.GraphicsPath();
            h = pbDragHandle.Height;
            w = pbDragHandle.Width;
            dividerW = 9.333f;
            dividerH = 6f;

            grp.AddPolygon(new PointF[]
            {
                new PointF(w / dividerW, 0),
                new PointF(w - (w / dividerW), 0),
                new PointF(w, h / dividerH),
                new PointF(w, h - (h / dividerH)),
                new PointF(w - (w / dividerW), h),
                new PointF(w / dividerW, h),
                new PointF(0, h - (h / dividerH)),
                new PointF(0, h / dividerH),
            });
            reg = new Region(grp);
            pbDragHandle.Region = reg;

            toolTip.SetToolTip(pbPlay, "Click to Play");
            toolTip.SetToolTip(pbGetNewToken, "Click to get a new Access-Token.\nThis may help you if you run into login-problems.");
            toolTip.SetToolTip(pbEdit, "Click to edit");
            toolTip.SetToolTip(pbDelete, "Click to delete");
            toolTip.SetToolTip(lEmail, "Click to copy the e-mail to clipboard.");
            toolTip.SetToolTip(lAccountName, "Click to copy the account-name to clipboard.");

            isCreating = false;
        }

        public void AddProcess(Process p)
        {
            process = p;
            process.EnableRaisingEvents = true;
            process.Exited += ProcessExit;
            isRunning = true;

            pbGetNewToken.Enabled = pbEdit.Enabled = pbDelete.Enabled = false;
            pbPlay.Image = frm.useDarkmode ? Properties.Resources.baseline_pause_circle_outline_white_36dp : Properties.Resources.baseline_pause_circle_outline_black_36dp;
            pbPlay.BackColor = frm.useDarkmode ? Color.FromArgb(0, 139, 169) : Color.FromArgb(0, 179, 219);

            toolTip.SetToolTip(pbPlay, "Click to close the runnig exalt instance.");
        }

        private void ProcessExit(object sender, EventArgs e)
        {
            process = null;
            isRunning = false;

            ProcessExitUI();
        }

        private bool ProcessExitUI()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)ProcessExitUI);

            pbPlay.Image = frm.useDarkmode ? Properties.Resources.ic_play_circle_outline_white_36dp : Properties.Resources.ic_play_circle_outline_black_36dp;
            pbPlay.Enabled = pbGetNewToken.Enabled = pbEdit.Enabled = pbDelete.Enabled = true;
            pbPlay.BackColor = this.BackColor;
            toolTip.SetToolTip(pbPlay, "Click to Play");

            return false;
        }

        public void ApplyTheme(bool isDarkmode)
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (isDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            ApplyTheme(isDarkmode, def, second, third, font);
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            this.ForeColor = font;
            toolTip.ForeColor = font;
            if (!isSecond)
            {
                this.BackColor = def;
                toolTip.BackColor = second;
            }
            else
            {
                this.BackColor = second;
                toolTip.BackColor = def;
            }

            if (isDarkmode)
            {
                pbPlay.Image = isRunning ? Properties.Resources.baseline_pause_circle_outline_white_36dp : Properties.Resources.ic_play_circle_outline_white_36dp;
                pbEdit.Image = Properties.Resources.ic_edit_white_36dp;
                pbDelete.Image = Properties.Resources.ic_delete_forever_white_36dp;
                pbGetNewToken.Image = Properties.Resources.baseline_autorenew_white_36dp;
                pbDragHandle.Image = Properties.Resources.ic_drag_handle_white_36dp;
            }
            else
            {
                pbPlay.Image = isRunning ? Properties.Resources.baseline_pause_circle_outline_black_36dp : Properties.Resources.ic_play_circle_outline_black_36dp;
                pbEdit.Image = Properties.Resources.ic_edit_black_36dp;
                pbDelete.Image = Properties.Resources.ic_delete_forever_black_36dp;
                pbGetNewToken.Image = Properties.Resources.baseline_autorenew_black_36dp;
                pbDragHandle.Image = Properties.Resources.ic_drag_handle_black_36dp;
            }
            pbEdit.BackColor = pbDelete.BackColor = pbGetNewToken.BackColor =
            pDrag.BackColor = pbDragHandle.BackColor = this.BackColor;
            pbPlay.BackColor = !isRunning ? this.BackColor : isDarkmode ? Color.FromArgb(0, 139, 169) : Color.FromArgb(0, 179, 219);

            #region CheckBox

            checkBox.BackColor = Color.Transparent;
            checkBox.OnCheck.BorderColor = isDarkmode ? Color.White : Color.Black;
            checkBox.OnCheck.CheckBoxColor = isDarkmode ? Color.White : Color.Black;
            checkBox.OnCheck.CheckmarkColor = isDarkmode ? Color.Black : Color.White;

            checkBox.OnUncheck.BorderColor = isDarkmode ? Color.FromArgb(128, 250, 250, 250) : Color.FromArgb(128, 0, 0, 0);
            checkBox.OnUncheck.CheckBoxColor = pCheckBox.BackColor;
            checkBox.OnUncheck.CheckmarkColor = isDarkmode ? Color.Black : Color.White;

            checkBox.OnHoverChecked.BorderColor =
            checkBox.OnHoverChecked.CheckBoxColor = isDarkmode ? Color.FromArgb(200, 250, 250, 250) : Color.FromArgb(150, 0, 0, 0);
            checkBox.OnHoverChecked.CheckmarkColor = isDarkmode ? Color.Black : Color.White;

            checkBox.OnHoverUnchecked.BorderColor = isDarkmode ? Color.FromArgb(128, 250, 250, 250) : Color.FromArgb(128, 0, 0, 0);
            checkBox.OnHoverUnchecked.CheckBoxColor = isDarkmode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(220, 220, 220);
            checkBox.OnHoverUnchecked.CheckmarkColor = isDarkmode ? Color.Black : Color.White;

            #endregion
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            Control c = sender as Control;
            if (sender != pbPlay && (c.BackColor == Color.LimeGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
                return;

            if (isRunning && c == pbPlay)
            {
                c.BackColor = frm.useDarkmode ? Color.FromArgb(0, 179, 219) : Color.FromArgb(0, 209, 249);
                return;
            }
            if (frm.useDarkmode)
            {
                c.BackColor = Color.FromArgb(64, 64, 64);
            }
            else
            {
                c.BackColor = Color.FromArgb(220, 220, 220);
            }
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (sender != pbPlay && (c.BackColor == Color.LimeGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
                return;
            if (isRunning && c == pbPlay)
            {
                c.BackColor = frm.useDarkmode ? Color.FromArgb(0, 139, 169) : Color.FromArgb(0, 179, 219);
                return;
            }
            c.BackColor = this.BackColor;
        }

        private void pbPlay_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;
            try
            {
                if (!isRunning)
                    frm.Connect(accountInfo);
                else
                {
                    if (process != null)
                    {
                        pbPlay.Enabled = false;
                        try//StopExalt
                        {
                            frm.timerLoadProcesses.Stop();
                            frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM AccUI", LogEventType.StopExalt, $"Stopping instance from: {accountInfo.email}."));
                            while (frm.searchProcesses)
                            {
                                Application.DoEvents();
                            }

                            process.Kill();

                            frm.timerLoadProcesses.Start();
                        }
                        catch
                        {
                            frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM AccUI", LogEventType.EAMError, $"Failed to stopping Instance from: {accountInfo.email}."));
                            pbPlay.Enabled = true;

                            if (frm.timerLoadProcesses.Enabled)
                                frm.timerLoadProcesses.Start();
                        }
                    }
                }
            }
            catch
            {
                frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM AccUI", LogEventType.EAMError, $"Failed to start the game for: {accountInfo.email}."));
            }
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmAddAccount frmAddAccount = new FrmAddAccount(frm, accountInfo);
            frmAddAccount.StartPosition = FormStartPosition.Manual;
            frmAddAccount.Location = new Point(frm.Location.X + ((frm.Width - frmAddAccount.Width) / 2), frm.Location.Y + ((frm.Height - frmAddAccount.Height) / 2));
            frmAddAccount.Show(frm);

            frm.lockForm = true;
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmDeletePopUp frmDeletePopUp = new FrmDeletePopUp(frm, this);
            frmDeletePopUp.StartPosition = FormStartPosition.Manual;
            frmDeletePopUp.Location = new Point(frm.Location.X + ((frm.Width - frmDeletePopUp.Width) / 2), frm.Location.Y + ((frm.Height - frmDeletePopUp.Height) / 2));
            //frmDeletePopUp.Location = new Point((frm.Location.X + pbDelete.Left) - frmDeletePopUp.Width / 2, (frm.Location.Y + this.Top - this.Height) + frmDeletePopUp.Height / 2);
            frmDeletePopUp.Show(frm);

            frm.lockForm = true;
        }

        public void DeleteAccount()
        {
            frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM", LogEventType.RemoveAccount, $"Removing account: {accountInfo.email}."));
            frm.accounts.Remove(accountInfo);
            frm.RemoveAccountFromOrders(accountInfo.email);
            frm.UpdateAccountInfos();
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (frm.lockForm) return;

            Control c = sender as Control;
            if (sender != pbPlay && (c.BackColor == Color.LimeGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
                return;
            if (isRunning && c == pbPlay)
            {
                c.BackColor = frm.useDarkmode ? Color.Crimson : Color.IndianRed;
                return;
            }
            if (frm.useDarkmode)
            {
                c.BackColor = Color.FromArgb(128, 128, 128);
            }
            else
            {
                c.BackColor = Color.FromArgb(180, 180, 180);
            }
        }

        private void button_MouseUp(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (sender != pbPlay && (c.BackColor == Color.LimeGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
                return;
            if (isRunning && c == pbPlay)
            {
                c.BackColor = frm.useDarkmode ? Color.FromArgb(0, 179, 219) : Color.FromArgb(0, 209, 249);
                return;
            }
            if (frm.lockForm)
            {
                c.BackColor = this.BackColor;
            }
            else
            {
                c.BackColor = frm.useDarkmode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(220, 220, 220);
            }
        }

        private void pbGetNewToken_Click(object sender, EventArgs e)
        {
            try
            {
                frm.ShowMoreUI(true);
                if (frm.lockForm) return;

                pbGetNewToken.Enabled = false;
                timerResetGetToken.Start();
                frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM AccUI", LogEventType.EditAccount, $"Get new token for: {accountInfo.email}."));
                int index = frm.accounts.IndexOf(accountInfo);
                accountInfo = frm.GetAccountData(accountInfo, false);
                if (index >= 0)
                {
                    frm.accounts[index] = accountInfo;
                    frm.SaveAccounts();
                    pbGetNewToken.BackColor = frm.useDarkmode ? Color.LimeGreen : Color.PaleGreen;
                }
                else
                {
                    pbGetNewToken.BackColor = frm.useDarkmode ? Color.FromArgb(225, 50, 50) : Color.IndianRed;
                }
            }
            catch
            {
                frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM AccUI", LogEventType.EAMError, $"Failed to get new token for: {accountInfo.email}."));
                pbGetNewToken.BackColor = frm.useDarkmode ? Color.FromArgb(225, 50, 50) : Color.IndianRed;
            }

        }

        private void tooltip_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (frm.lockForm) return;

            e.DrawBackground();
            e.DrawBorder();
            e.DrawText(TextFormatFlags.VerticalCenter);
        }

        private void timerResetGetToken_Tick(object sender, EventArgs e)
        {
            timerResetGetToken.Stop();
            pbGetNewToken.Enabled = true;
            pbGetNewToken.Image = !frm.useDarkmode ? Properties.Resources.baseline_autorenew_black_36dp : Properties.Resources.baseline_autorenew_white_36dp;
            pbGetNewToken.BackColor = this.BackColor;
        }

        public event MouseEventHandler mouseDown = null;
        public event MouseEventHandler mouseMove = null;
        public event MouseEventHandler mouseUp = null;
        private void pbDragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.mouseDown != null)
                this.mouseDown(this, e);
        }

        private void pbDragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mouseMove != null)
                this.mouseMove(this, e);
        }

        private void pbDragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.mouseUp != null)
                this.mouseUp(this, e);
        }

        private void checkBox_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (frm.loading) return;

            if (frm.lockForm)
            {
                checkBox.CheckedChanged -= checkBox_CheckedChanged;
                checkBox.Checked = isCreating ? checkBox.Checked : !checkBox.Checked;
                checkBox.CheckedChanged += checkBox_CheckedChanged;
                return;
            }
            frm.ShowMoreUI(true);

            accountInfo.performSave = checkBox.Checked;
            frm.ChangeDailyLoginState(accountInfo);
        }

        public void ChangeScrollState(bool state)
        {
            pScroll.Visible = state;
        }

        private void lEmail_Click(object sender, EventArgs e) => Clipboard.SetText(accountInfo.email);

        private void lAccountName_Click(object sender, EventArgs e) => Clipboard.SetText(accountInfo.name);
    }
}
