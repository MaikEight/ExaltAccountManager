using MK_EAM_Captcha_Solver_UI_Lib;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public sealed partial class UIAddAccount : UserControl
    {
        private FrmMain frm;
        private ColorChanger colorChanger;
        private MK_EAM_Lib.AccountInfo info = new MK_EAM_Lib.AccountInfo() { Color = Color.Transparent };

        public UIAddAccount(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;

            colorChanger = new ColorChanger(frm) { Visible = false, accountIndex = -99 };
            this.Controls.Add(colorChanger);
            colorChanger.Location = new Point(lGroup.Right, lGroup.Top);
            colorChanger.BringToFront();

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;

            tbEmail.ResetColors();
            tbPassword.ResetColors();
            tbCustomAccountname.ResetColors();

            this.ForeColor =
            tbEmail.ForeColor = tbEmail.OnActiveState.ForeColor = tbEmail.OnDisabledState.ForeColor = tbEmail.OnHoverState.ForeColor = tbEmail.OnIdleState.ForeColor =
            tbPassword.ForeColor = tbPassword.OnActiveState.ForeColor = tbPassword.OnDisabledState.ForeColor = tbPassword.OnHoverState.ForeColor = tbPassword.OnIdleState.ForeColor =
            tbCustomAccountname.ForeColor = tbCustomAccountname.OnActiveState.ForeColor = tbCustomAccountname.OnDisabledState.ForeColor = tbCustomAccountname.OnHoverState.ForeColor = tbCustomAccountname.OnIdleState.ForeColor
            = font;

            shadowInfos.PanelColor = shadowInfos.BackColor = shadowInfos.PanelColor2 =
            shadowImporter.PanelColor = shadowImporter.BackColor = shadowImporter.PanelColor2 =
            tbEmail.BackColor = tbEmail.FillColor = tbEmail.OnIdleState.FillColor = tbEmail.OnHoverState.FillColor = tbEmail.OnActiveState.FillColor = tbEmail.OnDisabledState.FillColor =
            tbPassword.BackColor = tbPassword.FillColor = tbPassword.OnIdleState.FillColor = tbPassword.OnHoverState.FillColor = tbPassword.OnDisabledState.FillColor =
            tbCustomAccountname.BackColor = tbCustomAccountname.FillColor = tbCustomAccountname.OnIdleState.FillColor = tbCustomAccountname.OnHoverState.FillColor = tbCustomAccountname.OnDisabledState.FillColor
            = def;

            shadowInfos.BorderColor = 
            shadowImporter.BorderColor = second;
            shadowInfos.ShadowColor =
            shadowImporter.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            if (info.Color == Color.Transparent)
                pbGroup.Image = frm.UseDarkmode ? Properties.Resources.ic_block_white_18dp : Properties.Resources.ic_block_black_18dp;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
            this.Invalidate();
        }

        private bool toggleResetTime = false;
        private void toggleCustomAccountname_CheckedChanged(object sender, EventArgs e)
        {
            if (!transition.IsCompleted && !toggleResetTime)
            {
                toggleCustomAccountname.Checked = !toggleCustomAccountname.Checked;
                toggleResetTime = true;
                return;
            }
            else if (toggleResetTime)
            {
                toggleResetTime = false;
                return;
            }

            if (toggleCustomAccountname.Checked)
                transition.Show(tbCustomAccountname, true, Bunifu.UI.WinForms.BunifuAnimatorNS.Animation.VertSlide);
            else
                tbCustomAccountname.Visible = false;
            tbCustomAccountname.Clear();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (!btnAddAccount.Enabled) 
                return;

            btnAddAccount.Enabled = false;

            if (toggleCustomAccountname.Checked)
                info.name = tbCustomAccountname.Text;

            info.email = tbEmail.Text.Replace(" ", "");
            info.password = tbPassword.Text;

            if (string.IsNullOrEmpty(info.email) || string.IsNullOrEmpty(info.password))
            {
                frm.ShowSnackbar("E-Mail and password can't be empty!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);

                btnAddAccount.Enabled = true;
                return;
            }

            if (frm.accounts.Count > 0 && frm.accounts.Where(i => i.email.ToLower().Equals(info.email.ToLower())).Count() > 0)
            {
                frm.ShowSnackbar("The account is already in the list.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);

                btnAddAccount.Enabled = true;
                return;
            }

            loader.Visible = true;

            info.PerformWebrequest(frm, frm.LogEvent, "EAM", frm.accountStatsPath, frm.itemsSaveFilePath, frm.GetDeviceUniqueIdentifier(), !toggleCustomAccountname.Checked, true, RequestDone);
        }

        private void RequestDone(MK_EAM_Lib.AccountInfo _info)
        {
            if (_info.requestState == MK_EAM_Lib.AccountInfo.RequestState.Captcha)
            {
                bool success = ShowCaptcha(_info);
                if (!success)
                {
                    frm.ShowSnackbar("Captcha solving failed, please try again later.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                    ResetUI(false);
                }
                return;
            }

            if (string.IsNullOrEmpty(_info.name) || _info.requestState != MK_EAM_Lib.AccountInfo.RequestState.Success)
            {
                frm.ShowSnackbar("Email or password are not correct.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                ResetUI(false);
            }
            else if (_info.requestState == MK_EAM_Lib.AccountInfo.RequestState.Success)
            {
                _info.Color = info.Color;
                _info.performSave = !toggleLogin.Checked;

                frm.AddAccount(_info);
                ResetUI();
            }
        }

        private bool ShowCaptcha(MK_EAM_Lib.AccountInfo _info)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<MK_EAM_Lib.AccountInfo, bool>)ShowCaptcha, _info);

            return CaptchaSolverUiUtils.Show(_info, frm, frm.UseDarkmode, frm.LogEvent, "EAM", frm.accountStatsPath, frm.itemsSaveFilePath, frm.GetDeviceUniqueIdentifier(), !toggleCustomAccountname.Checked, true, RequestDone);
        }

        private bool ResetUI(bool success = true)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool, bool>)ResetUI, success);

            if (success)
            {
                tbEmail.Clear();
                tbPassword.Clear();
                tbCustomAccountname.Clear();
                toggleCustomAccountname.Checked =
                toggleLogin.Checked = false;
                pbGroup.Image = frm.UseDarkmode ? Properties.Resources.ic_block_white_18dp : Properties.Resources.ic_block_black_18dp;

                frm.ShowSnackbar($"Successfully added {info.email} to the accountlist.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                info = new MK_EAM_Lib.AccountInfo() { Color = Color.Transparent };
            }

            btnAddAccount.Enabled = true;
            loader.Visible = false;

            return false;
        }

        private void pbGroup_Click(object sender, EventArgs e)
        {
            colorChanger.accountIndex = -99;
            if (!colorChanger.Visible)
                transition.Show(colorChanger, true, Bunifu.UI.WinForms.BunifuAnimatorNS.Animation.HorizSlide);
            else
                colorChanger.Visible = false;
        }

        internal void UpdateGroup(Color clr, bool none = false)
        {
            info.Color = clr;
            pbGroup.Image = info.Group;
            pbGroup.SizeMode = none ? PictureBoxSizeMode.CenterImage : PictureBoxSizeMode.Zoom;

            if (none)
                pbGroup.Image = frm.UseDarkmode ? Properties.Resources.ic_block_white_18dp : Properties.Resources.ic_block_black_18dp;
        }

        private void btnImporter_Click(object sender, EventArgs e) => frm.OpenImporter();
    }
}
