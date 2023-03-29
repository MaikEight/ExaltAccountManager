using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleEditAccount : UserControl
    {
        private FrmMain frm;
        private MK_EAM_Lib.AccountInfo accountInfo;
        private bool showInVP = false;
        private bool passwordShownState = false;

        public EleEditAccount(FrmMain _frm, MK_EAM_Lib.AccountInfo info, bool _showInVP)
        {
            this.Disposed += OnDispose;
            InitializeComponent();
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            accountInfo = info;
            showInVP = _showInVP;

            LoadServers();

            ApplyTheme(frm, null);

            AddAccountToUI();
        }

        private void AddAccountToUI()
        {
            tbAccountname.Text = accountInfo.Name;
            tbEmail.Text = accountInfo.Email;
            tbPassword.Text = accountInfo.password;
            toggleLogin.Checked = !accountInfo.performSave;
            toggleShowinVP.Checked = showInVP;

            string item = accountInfo.serverName;
            if (item == null)
                item = string.Empty;
            if (dropServers.Items.Contains(item))
                dropServers.SelectedItem = item;
            else
            {
                if (item.Equals("Last"))
                    dropServers.SelectedIndex = 1;
                else
                    dropServers.SelectedIndex = 0;
            }
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            tbAccountname.ResetColors();
            tbEmail.ResetColors();
            tbPassword.ResetColors();

            this.ForeColor =
            tbEmail.ForeColor = tbEmail.OnActiveState.ForeColor = tbEmail.OnDisabledState.ForeColor = tbEmail.OnHoverState.ForeColor = tbEmail.OnIdleState.ForeColor =
            tbPassword.ForeColor = tbPassword.OnActiveState.ForeColor = tbPassword.OnDisabledState.ForeColor = tbPassword.OnHoverState.ForeColor = tbPassword.OnIdleState.ForeColor =
            tbAccountname.ForeColor = tbAccountname.OnActiveState.ForeColor = tbAccountname.OnDisabledState.ForeColor = tbAccountname.OnHoverState.ForeColor = tbAccountname.OnIdleState.ForeColor
            = font;
            this.BackColor =
            tbEmail.BackColor = tbEmail.FillColor = tbEmail.OnIdleState.FillColor = tbEmail.OnHoverState.FillColor = tbEmail.OnActiveState.FillColor = tbEmail.OnDisabledState.FillColor =
            tbPassword.BackColor = tbPassword.FillColor = tbPassword.OnIdleState.FillColor = tbPassword.OnHoverState.FillColor = tbPassword.OnDisabledState.FillColor =
            tbAccountname.BackColor = tbAccountname.FillColor = tbAccountname.OnIdleState.FillColor = tbAccountname.OnHoverState.FillColor = tbAccountname.OnDisabledState.FillColor
            = def;

            dropServers.BackgroundColor =
            dropServers.ItemBackColor = def;
            dropServers.ForeColor =
            dropServers.ItemBorderColor =
            dropServers.ItemForeColor = font;
            dropServers.BorderColor = third;
            dropServers.Invalidate();

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;

            shadow.BorderColor = second;
            shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            toolTip.BackColor = def;
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = frm.UseDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);

            this.Invalidate();
        }

        private void LoadServers()
        {
            dropServers.Items.Clear();
            dropServers.Items.Add("EAM default server");
            dropServers.Items.Add("Last server (Deca default)");

            if (frm.serverData != null && frm.serverData.servers != null)
            {
                for (int i = 0; i < frm.serverData.servers.Count; i++)
                    dropServers.Items.Add(frm.serverData.servers[i].name);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int index = -1;
            try
            {
                index = frm.accounts.IndexOf(accountInfo);
                if (index < 0 || index > frm.accounts.Count)                
                    index = frm.accounts.IndexOf(frm.accounts.Where(a => a.email.Equals(accountInfo.email)).FirstOrDefault());

                if (index < 0 || index > frm.accounts.Count)
                    throw new Exception();
            }
            catch
            {
                frm.ShowSnackbar("Failed to save Account!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                frm.RemoveShadowForm();
            }

            accountInfo.name = tbAccountname.Text;
            accountInfo.password = tbPassword.Text;
            accountInfo.performSave = !toggleLogin.Checked;
            frm.SetActiveVaultPeekerAccount(accountInfo.Email, toggleShowinVP.Checked);

            switch (dropServers.SelectedIndex)
            {
                case 0:
                    accountInfo.serverName = string.Empty;
                    break;
                case 1:
                    accountInfo.serverName = "Last";
                    break;
                default:
                    if (dropServers.SelectedItem != null)
                        accountInfo.serverName = dropServers.SelectedItem.ToString();
                    break;
            }
            frm.accounts[index] = accountInfo;

            if (frm.SaveAccounts())
                frm.ShowSnackbar("Account saved successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000);
            frm.UpdateSideAccountUI();
            frm.RemoveShadowForm();            
        }

        public void OnDispose(object sender, EventArgs e)
        {
            frm.ThemeChanged -= ApplyTheme;
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e) 
        {
            frm.RemoveShadowForm(); 
            this.Dispose();
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_18dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = frm.UseDarkmode ? ColorScheme.GetColorSecond(frm.UseDarkmode) : ColorScheme.GetColorThird(frm.UseDarkmode);
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;
        }

        private void pbClose_MouseDown(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Red;

        #endregion
       
    }
}
