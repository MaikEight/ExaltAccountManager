using Bunifu.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_PingChecker
{
    public partial class UIOptions : UserControl
    {
        FrmMain frm;
        private bool isInit = false;
        private bool useDarkmode = false;

        public UIOptions(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            useDarkmode = frm.useDarkmode;
            isInit = true;

            dropAccounts.SelectedIndex = 0;

            for (int i = 0; i < frm.accounts.Count; i++)
                dropAccounts.Items.Add(frm.accounts[i].name);

            AddSaveFileToUI();
            ApplyTheme(useDarkmode, ColorScheme.GetColorDef(useDarkmode), ColorScheme.GetColorSecond(useDarkmode), ColorScheme.GetColorThird(useDarkmode), ColorScheme.GetColorFont(useDarkmode));
            isInit = false;
        }

        public void ApplyTheme(bool _useDarkmode, Color def, Color second, Color third, Color font)
        {
            useDarkmode = _useDarkmode;

            this.BackColor = def;
            //toolTip.BackColor = def;
            this.ForeColor =
            dropAccounts.ForeColor =
            dropAccounts.ItemForeColor = font;
            //toolTip.TitleForeColor = font;
            //toolTip.TextForeColor = useDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);
            //lRefresh.ForeColor = ColorScheme.GetColorThird(!useDarkmode);

            shadow.BackColor = shadow2.BackColor =
            shadow.PanelColor = shadow2.PanelColor =
            shadow.PanelColor2 = shadow2.PanelColor2 = def;

            shadow.ForeColor = shadow2.ForeColor =
            separator.LineColor = separator2.LineColor = font;

            dropAccounts.BackgroundColor =
            dropAccounts.ItemBackColor = def;

            BunifuSnackbar.CustomizationOptions opt = new BunifuSnackbar.CustomizationOptions()
            {
                ActionBackColor = !useDarkmode ? Color.White : Color.FromArgb(8, 8, 8),
                BackColor = !useDarkmode ? Color.White : Color.FromArgb(8, 8, 8),
                ActionBorderColor = !useDarkmode ? Color.White : Color.FromArgb(15, 15, 15),
                BorderColor = !useDarkmode ? Color.White : Color.FromArgb(15, 15, 15),
                ActionForeColor = !useDarkmode ? Color.Black : Color.FromArgb(170, 170, 170),
                ForeColor = !useDarkmode ? Color.Black : Color.FromArgb(170, 170, 170),
                CloseIconColor = Color.FromArgb(246, 255, 237)
            };

            snackbar.ErrorOptions = opt;
            snackbar.ErrorOptions.CloseIconColor = Color.FromArgb(255, 204, 199);
            snackbar.WarningOptions = opt;
            snackbar.WarningOptions.CloseIconColor = Color.FromArgb(255, 229, 143);
            snackbar.InformationOptions = opt;
            snackbar.InformationOptions.CloseIconColor = Color.FromArgb(145, 213, 255);
            snackbar.SuccessOptions = opt;
            snackbar.SuccessOptions.CloseIconColor = Color.FromArgb(246, 255, 237);
        }

        private void AddSaveFileToUI()
        {
            if (frm.pingSaveFile == null)
                frm.pingSaveFile = new MK_EAM_Lib.PingSaveFile();
            isInit = true;

            switch (frm.pingSaveFile.startupPing)
            {
                case MK_EAM_Lib.StartupPing.All:
                    radioPingAll.Checked = true;
                    radioPingFavs.Checked = false;
                    radioPingNon.Checked = false;
                    break;
                case MK_EAM_Lib.StartupPing.Favorites:
                    radioPingAll.Checked = false;
                    radioPingFavs.Checked = true;
                    radioPingNon.Checked = false;
                    break;
                case MK_EAM_Lib.StartupPing.Non:
                    radioPingAll.Checked = false;
                    radioPingFavs.Checked = false;
                    radioPingNon.Checked = true;
                    break;
                default:
                    break;
            }

            toggleServerDataOnStartup.Checked = frm.pingSaveFile.serverDataOnStartup;

            if (dropAccounts.Items.Contains(frm.pingSaveFile.accountName))
                dropAccounts.SelectedIndex = dropAccounts.Items.IndexOf(frm.pingSaveFile.accountName);

            toggleRefreshServerdata.Checked = frm.pingSaveFile.refreshServerdata;

            isInit = false;
        }

        private void radioPingAll_Click(object sender, EventArgs e)
        {
            if (isInit) return;

            frm.pingSaveFile.startupPing = MK_EAM_Lib.StartupPing.All;

            radioPingAll.Checked = true;
            radioPingFavs.Checked = false;
            radioPingNon.Checked = false;

            WriteSaveFile();
        }

        private void radioPingFavs_Click(object sender, EventArgs e)
        {
            if (isInit) return;

            frm.pingSaveFile.startupPing = MK_EAM_Lib.StartupPing.Favorites;

            radioPingAll.Checked = false;
            radioPingFavs.Checked = true;
            radioPingNon.Checked = false;

            WriteSaveFile();
        }

        private void radioPingNon_Click(object sender, EventArgs e)
        {
            if (isInit) return;

            frm.pingSaveFile.startupPing = MK_EAM_Lib.StartupPing.Non;

            radioPingAll.Checked = false;
            radioPingFavs.Checked = false;
            radioPingNon.Checked = true;

            WriteSaveFile();
        }

        private void toggleServerDataOnStartup_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (isInit) return;

            frm.pingSaveFile.serverDataOnStartup = toggleServerDataOnStartup.Checked;

            WriteSaveFile();
        }

        private void toggleRefreshServerdata_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (isInit) return;

            frm.pingSaveFile.refreshServerdata = toggleRefreshServerdata.Checked;

            WriteSaveFile();
        }

        private void WriteSaveFile()
        {
            if (frm.SavePingSaveFile())
            {
                snackbar.Show(frm, "Configuration saved successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1500, "X" ,BunifuSnackbar.Positions.BottomRight);
            }
            else
            {
                snackbar.Show(frm, "Failed to save configuration.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000, "X", BunifuSnackbar.Positions.BottomRight);
            }
        }

        private void dropAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            frm.pingSaveFile.accountName = dropAccounts.SelectedItem.ToString();

            WriteSaveFile();
        }
    }
}
