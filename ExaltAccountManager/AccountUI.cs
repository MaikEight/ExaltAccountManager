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
        public MK_EAM_Lib.AccountInfo accountInfo;
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

            accountInfo = new MK_EAM_Lib.AccountInfo();

            pbColor.Image = pbColor.InitialImage = pbColor.BackgroundImage = pbColor.ErrorImage = null;

            isCreating = false;
        }

        public AccountUI(FrmMain _frm, MK_EAM_Lib.AccountInfo _accountInfo)
        {
            InitializeComponent();
            frm = _frm;
            accountInfo = _accountInfo;

            lAccountName.Text = accountInfo.name;
            lEmail.Text = accountInfo.email;
            checkBox.Checked = accountInfo.performSave;

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

            toolTip.SetToolTip(pbColor, "Click to change the color of this account.");
            toolTip.SetToolTip(pbPlay, "Click to Play");
            toolTip.SetToolTip(pbServerlist, "Click to change the server for this account.");
            toolTip.SetToolTip(pbGetNewToken, "Click to get a new Access-Token.\nThis may help you if you run into login-problems.");
            toolTip.SetToolTip(pbHWID, "Click to change the HWID used for this account.");
            toolTip.SetToolTip(pbEdit, "Click to edit");
            toolTip.SetToolTip(pbDelete, "Click to delete");
            toolTip.SetToolTip(lEmail, "Click to copy the e-mail to clipboard.");
            toolTip.SetToolTip(lAccountName, "Click to copy the account-name to clipboard.");

            pbColor.Image = pbColor.InitialImage = pbColor.BackgroundImage = pbColor.ErrorImage = null;
            pbColor.BackColor = accountInfo.color;

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
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = Color.FromArgb(225, font.R, font.G, font.B);

            if (!isSecond)
            {
                this.BackColor = def;
                toolTip.BackColor = second;

                if (frm.screenshotMode)
                    this.ForeColor = lAccountName.BackColor = lEmail.BackColor = second;
            }
            else
            {
                this.BackColor = second;
                toolTip.BackColor = def;

                if (frm.screenshotMode)
                    this.ForeColor = lAccountName.BackColor = lEmail.BackColor = def;
            }

            if (isDarkmode)
            {
                pbPlay.Image = isRunning ? Properties.Resources.baseline_pause_circle_outline_white_36dp : Properties.Resources.ic_play_circle_outline_white_36dp;
                pbEdit.Image = Properties.Resources.outline_edit_white_36dp;
                pbDelete.Image = Properties.Resources.baseline_delete_outline_white_36dp;
                pbDragHandle.Image = Properties.Resources.ic_drag_handle_white_36dp;
                pbDrag.Image = Properties.Resources.outline_drag_indicator_white_36dp;
                pbServerlist.Image = Properties.Resources.list_white_13px;
                pbHWID.Image = Properties.Resources.fingerprint_white_15px;

                if (pbGetNewToken.BackColor == Color.PaleGreen)
                {
                    pbGetNewToken.BackColor = Color.ForestGreen;
                    pbGetNewToken.Image = Properties.Resources.outline_published_with_changes_white_36dp;
                }
                else if (pbGetNewToken.BackColor == Color.IndianRed)
                {
                    pbGetNewToken.BackColor = Color.FromArgb(225, 50, 50);
                    pbGetNewToken.Image = Properties.Resources.outline_sync_problem_white_36dp;
                }
                else
                {
                    pbGetNewToken.Image = Properties.Resources.baseline_autorenew_white_36dp;
                    pbGetNewToken.BackColor = this.BackColor;
                }
            }
            else
            {
                pbPlay.Image = isRunning ? Properties.Resources.baseline_pause_circle_outline_black_36dp : Properties.Resources.ic_play_circle_outline_black_36dp;
                pbEdit.Image = Properties.Resources.outline_edit_black_36dp;
                pbDelete.Image = Properties.Resources.baseline_delete_outline_black_36dp;
                pbDragHandle.Image = Properties.Resources.ic_drag_handle_black_36dp;
                pbDrag.Image = Properties.Resources.outline_drag_indicator_black_36dp;
                pbServerlist.Image = Properties.Resources.list_13px;
                pbHWID.Image = Properties.Resources.fingerprint_15px;

                if (pbGetNewToken.BackColor == Color.ForestGreen)
                {
                    pbGetNewToken.BackColor = Color.PaleGreen;
                    pbGetNewToken.Image = Properties.Resources.outline_published_with_changes_black_36dp;
                }
                else if (pbGetNewToken.BackColor == Color.FromArgb(225, 50, 50))
                {
                    pbGetNewToken.BackColor = Color.IndianRed;
                    pbGetNewToken.Image = Properties.Resources.outline_sync_problem_black_36dp;
                }
                else
                {
                    pbGetNewToken.Image = Properties.Resources.baseline_autorenew_black_36dp;
                    pbGetNewToken.BackColor = this.BackColor;
                }
            }
            pbEdit.BackColor = pbDelete.BackColor =
            pDrag.BackColor = pbDragHandle.BackColor =
            pbServerlist.BackColor = pbHWID.BackColor = this.BackColor;

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


            #region Removed - Causing too much lag while switching themes
            //toolTip.SetToolTipIcon(pbPlay, pbPlay.Image);
            //toolTip.SetToolTipIcon(pbColor, pbColor.Image);
            //toolTip.SetToolTipIcon(pbServerlist, frm.useDarkmode ? Properties.Resources.list_white_24px_1 : Properties.Resources.list_24px);
            //toolTip.SetToolTipIcon(pbGetNewToken, pbGetNewToken.Image);
            //toolTip.SetToolTipIcon(pbHWID, frm.useDarkmode ? Properties.Resources.fingerprint_white_24px : Properties.Resources.fingerprint_24px);
            //toolTip.SetToolTipIcon(pbEdit, pbEdit.Image);
            //toolTip.SetToolTipIcon(pbDelete, pbDelete.Image);
            #endregion
        }
        bool dragEntered = false;

        private void button_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            Control c = sender as Control;
            if (sender != pbPlay && (c.BackColor == Color.ForestGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
                return;

            if (c == pbPlay)
            {
                if (isRunning)
                {
                    c.BackColor = frm.useDarkmode ? Color.FromArgb(0, 179, 219) : Color.FromArgb(0, 209, 249);
                    return;
                }
                else
                {
                    pbPlay.Image = frm.useDarkmode ? Properties.Resources.outline_slow_motion_video_white_36dp : Properties.Resources.outline_slow_motion_video_black_36dp;
                }
            }
            else if (c == pbGetNewToken)
            {
                pbGetNewToken.Image = frm.useDarkmode ? Properties.Resources.baseline_autorenew_white_36dp_45G : Properties.Resources.baseline_autorenew_black_36dp_45G;
            }
            else if (c == pbDelete)
            {
                pbDelete.Image = frm.useDarkmode ? Properties.Resources.ic_delete_forever_white_36dp : Properties.Resources.ic_delete_forever_black_36dp;
            }
            else if (c == pbEdit)
            {
                pbEdit.Image = frm.useDarkmode ? Properties.Resources.ic_edit_white_36dp : Properties.Resources.ic_edit_black_36dp;
            }
            else if (c == pbDragHandle)
            {
                pbDrag.Visible = false;
                pbDrag.Width = 0;
                dragEntered = true;
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
            if (sender != pbPlay && (c.BackColor == Color.ForestGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
                return;
            if (c == pbPlay)
            {
                if (isRunning)
                {
                    c.BackColor = frm.useDarkmode ? Color.FromArgb(0, 139, 169) : Color.FromArgb(0, 179, 219);
                    return;
                }
                else
                    pbPlay.Image = frm.useDarkmode ? Properties.Resources.ic_play_circle_outline_white_36dp : Properties.Resources.ic_play_circle_outline_black_36dp;
            }
            else if (c == pbGetNewToken)
            {
                pbGetNewToken.Image = frm.useDarkmode ? Properties.Resources.baseline_autorenew_white_36dp : Properties.Resources.baseline_autorenew_black_36dp;
            }
            else if (c == pbDelete)
            {
                pbDelete.Image = frm.useDarkmode ? Properties.Resources.baseline_delete_outline_white_36dp : Properties.Resources.baseline_delete_outline_black_36dp;
            }
            else if (c == pbEdit)
            {
                pbEdit.Image = frm.useDarkmode ? Properties.Resources.outline_edit_white_36dp : Properties.Resources.outline_edit_black_36dp;
            }
            else if (c == pbDragHandle)
            {
                pbDrag.Visible = true;
                pbDrag.Width = 34;
                dragEntered = false;
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
                            frm.snackbar.Show(frm, $"Failed to stopping Instance from: {accountInfo.email}.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
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
                frm.snackbar.Show(frm, $"Failed to start the game for: {accountInfo.email}.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
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
            if (sender != pbPlay && (c.BackColor == Color.ForestGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
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
            if (sender != pbPlay && (c.BackColor == Color.ForestGreen || c.BackColor == Color.PaleGreen || c.BackColor == Color.FromArgb(225, 50, 50) || c.BackColor == Color.IndianRed))
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
                    pbGetNewToken.BackColor = frm.useDarkmode ? Color.ForestGreen : Color.PaleGreen;
                    pbGetNewToken.Image = frm.useDarkmode ? Properties.Resources.outline_published_with_changes_white_36dp : Properties.Resources.outline_published_with_changes_black_36dp;
                    pbGetNewToken.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pbGetNewToken.BackColor = frm.useDarkmode ? Color.FromArgb(225, 50, 50) : Color.IndianRed;
                    pbGetNewToken.Image = frm.useDarkmode ? Properties.Resources.outline_sync_problem_white_36dp : Properties.Resources.outline_sync_problem_black_36dp;
                }
            }
            catch
            {
                frm.LogEvent(new LogData(frm.logs.Count + 1, "EAM AccUI", LogEventType.EAMError, $"Failed to get new token for: {accountInfo.email}."));
                frm.snackbar.Show(frm, $"Failed to get new token for: {accountInfo.email}.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                pbGetNewToken.BackColor = frm.useDarkmode ? Color.FromArgb(225, 50, 50) : Color.IndianRed;
                pbGetNewToken.Image = frm.useDarkmode ? Properties.Resources.outline_sync_problem_black_36dp : Properties.Resources.outline_sync_problem_black_36dp;
            }

        }

        //private void tooltip_Draw(object sender, DrawToolTipEventArgs e)
        //{
        //    if (frm.lockForm) return;

        //    e.DrawBackground();
        //    e.DrawBorder();
        //    e.DrawText(TextFormatFlags.VerticalCenter);
        //}

        private void timerResetGetToken_Tick(object sender, EventArgs e)
        {
            timerResetGetToken.Stop();
            pbGetNewToken.Enabled = true;
            pbGetNewToken.Image = !frm.useDarkmode ? Properties.Resources.baseline_autorenew_black_36dp : Properties.Resources.baseline_autorenew_white_36dp;
            pbGetNewToken.BackColor = this.BackColor;
            pbGetNewToken.SizeMode = PictureBoxSizeMode.CenterImage;
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

            pDrag_MouseLeave(pDrag, null);
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

        private void lEmail_Click(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            Clipboard.SetText(accountInfo.email);
            frm.snackbar.Show(frm, "E-Mail copied to clipboard.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
        }

        private void lAccountName_Click(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            Clipboard.SetText(accountInfo.name);
            frm.snackbar.Show(frm, "Accountname copied to clipboard.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
        }

        private void pbDrag_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            pbDrag.Visible = false;
            pbDrag.Width = 0;
            pbDragHandle.Visible = true;
        }

        private void pbDrag_MouseLeave(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            if (!dragEntered)
                return;

            pbDrag.Visible = true;
            pbDrag.Width = 34;
            pbDragHandle.Visible = false;
        }

        private void pDrag_MouseLeave(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            frm.HideDragHandles();
        }

        private void pAccName_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            frm.HideDragHandles();
        }

        public void HideDragHandle()
        {
            dragEntered = false;

            pbDrag.Visible = true;
            pbDrag.Width = 34;
            pbDragHandle.Visible = false;
        }

        private void pbColor_MouseEnter(object sender, EventArgs e)
        {
            if (frm.lockForm) return;
            frm.ShowMoreUI(true);

            frm.HideDragHandles();
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            if (frm.lockForm) return;

            frm.ShowColorChangerUI(this);
        }

        public void ChangeColor(Color clr)
        {
            pbColor.BackColor = clr;
            frm.accounts[frm.accounts.IndexOf(accountInfo)].color = clr;
            frm.SaveAccounts();
        }

        private void pbServerlist_Click(object sender, EventArgs e)
        {
            frm.ShowServerListUI(this);
        }

        private void pbHWID_Click(object sender, EventArgs e)
        {
            //frm.ShowHWIDUI(this);
        }
    }
}
