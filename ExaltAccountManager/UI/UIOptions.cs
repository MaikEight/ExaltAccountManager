using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public sealed partial class UIOptions : UserControl
    {
        private FrmMain frm;
        private bool isInit = true;

        public bool[] GameUpdateAndNotifications = { true, true, true, true };

        //private bool HasChanges 
        //{
        //    get => hasChanges;
        //    set
        //    {
        //        hasChanges = value;

        //        if (frm != null)
        //        {

        //        }
        //    }

        //}
        //private bool hasChanges = false;

        public UIOptions(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;

            btnSave.Anchor = lSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            
            LoadServers();

            ApplyTheme(frm, null);

            isInit = false;
        }

        private void UIOptions_Load(object sender, EventArgs e)
        {
            ApplyOptions();
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            tbPath.ResetColors();

            this.BackColor =
            tbPath.BackColor = tbPath.FillColor = tbPath.OnIdleState.FillColor = tbPath.OnHoverState.FillColor = tbPath.OnActiveState.FillColor = tbPath.OnDisabledState.FillColor =
            def;

            this.ForeColor =
            tbPath.ForeColor = tbPath.OnActiveState.ForeColor = tbPath.OnDisabledState.ForeColor = tbPath.OnHoverState.ForeColor = tbPath.OnIdleState.ForeColor =
            font;

            dropServers.BackgroundColor = def;
            dropServers.ForeColor = font;
            dropServers.ItemBackColor = def;
            dropServers.ItemBorderColor = font;
            dropServers.ItemForeColor = font;
            dropServers.BorderColor = third;
            dropServers.Invalidate();

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in this.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
            {
                shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
                shadow.BorderColor = shadow.BorderColor = second;
                shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
            }

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        public void ApplyOptions()
        {
            isInit = true;

            tbPath.Text = frm.OptionsData.exePath;

            if (frm.OptionsData.serverToJoin == null)
                frm.OptionsData.serverToJoin = string.Empty;
            if (dropServers.Items.Contains(frm.OptionsData.serverToJoin))
                dropServers.SelectedItem = frm.OptionsData.serverToJoin;
            else
            {
                if (dropServers.Items.Count == 0)
                    dropServers.Items.Add("Last server (Deca default)");
                //if (frm.OptionsData.serverToJoin.Equals("Last"))
                dropServers.SelectedIndex = 0;

            }
            toggleCloseEAMAftergameStart.Checked = frm.OptionsData.closeAfterConnection;

            GameUpdateAndNotifications = new bool[]
            {
                frm.OptionsData.searchRotmgUpdates,
                frm.OptionsData.searchUpdateNotification,
                frm.OptionsData.searchWarnings,
                frm.OptionsData.deactivateKillswitch
            };

            toggleUseDarkmode.Checked = frm.UseDarkmode;
            toggleAlwaysRefreshDataOnLogin.Checked = frm.OptionsData.alwaysrefreshDataOnLogin;

            isInit = false;
        }

        public bool LoadServers()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)LoadServers);

            isInit = true;

            dropServers.Items.Clear();
            dropServers.Items.Add("Last server (Deca default)");

            if (frm.serverData != null && frm.serverData.servers != null)
            {
                for (int i = 0; i < frm.serverData.servers.Count; i++)
                    dropServers.Items.Add(frm.serverData.servers[i].name);
            }

            isInit = false;

            return false;
        }

        private void HasChanges()
        {
            if (isInit)
                return;

            btnSave.Visible = true;

            frm.UpdateHasConfigChangesUI(true);
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            lHeadline.Focus();

            try
            {
                OpenFileDialog diag = new OpenFileDialog();
                diag.Filter = "*.exe|*.exe";
                diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                diag.Title = "Select the RotMG Exalt.exe";
                diag.Multiselect = false;

                if (diag.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(diag.FileName))
                        tbPath.Text = diag.FileName;
                }
            }
            catch
            {
                frm.ShowSnackbar("An error occured during the open file dialog.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frm.OptionsData = new OptionsData()
            {
                exePath = tbPath.Text,
                closeAfterConnection = toggleCloseEAMAftergameStart.Checked,
                serverToJoin = dropServers.SelectedItem.ToString().StartsWith("Last") ? "Last" : dropServers.SelectedItem.ToString(),

                alwaysrefreshDataOnLogin = toggleAlwaysRefreshDataOnLogin.Checked,

                searchRotmgUpdates = GameUpdateAndNotifications[0],
                searchUpdateNotification = GameUpdateAndNotifications[1],
                searchWarnings = GameUpdateAndNotifications[2],
                deactivateKillswitch = GameUpdateAndNotifications[3],

                useDarkmode = frm.UseDarkmode
            };

            frm.SaveOptions(frm.OptionsData, true);
            btnSave.Visible = false;
            frm.UpdateHasConfigChangesUI(false);
        }

        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            btnSave.Image = Properties.Resources.save_36px;
            lSave.Visible = true;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.Image = Properties.Resources.save_Outline_36px;
            lSave.Visible = false;
        }

        private void btnSetDefaultPath_Click(object sender, EventArgs e)
        {
            lHeadline.Focus();

            tbPath.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe");
        }

        private void toggleUseDarkmode_CheckedChanged(object sender, EventArgs e)
        {
            if (isInit)
                return;

            frm.btnSwitchDesign_Click(null, null);
        }

        private void tbPath_TextChanged(object sender, EventArgs e)
        {
            if (tbPath.Text.Contains("%username%") && !string.IsNullOrEmpty(Environment.UserName))
            {
                tbPath.Text = tbPath.Text.Replace("%username%", Environment.UserName);
                tbPath.SelectionStart = tbPath.Text.Length;
            }

            HasChanges();
        }

        private void btnSnackbar_Click(object sender, EventArgs e)
        {
            lHeadline.Focus();

            Elements.EleSnackbarOptions eleSnackbarOptions = new Elements.EleSnackbarOptions(frm);
            frm.ShowShadowForm(eleSnackbarOptions);
        }

        private void btnGameUpdaterAndNotificationSettings_Click(object sender, EventArgs e)
        {
            lHeadline.Focus();

            frm.ShowShadowForm(new Elements.EleGameUpdateAndNotifications(frm));
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            lHeadline.Focus();

            frm.ShowShadowForm(new Elements.EleDiscordSettings(frm));
        }

        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            lHeadline.Focus();

            frm.ShowShadowForm(new Elements.EleAnalyticsSettings(frm));
        }

        private void dropServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            HasChanges();
        }

        private void toggleCloseEAMAftergameStart_CheckedChanged(object sender, EventArgs e)
        {
            HasChanges();
        }

        private void toggleAlwaysRefreshDataOnLogin_CheckedChanged(object sender, EventArgs e)
        {
            HasChanges();
        }
    }
}
