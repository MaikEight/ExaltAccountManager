using ExaltAccountManager.UI.Elements;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public partial class UIModules : UserControl
    {
        FrmMain frm;

        EleHWID_Tool eleHWID_Tool = null;
        UIChangelog uiChangelog = null;
        UITokenViewer uiTokenViewer = null;
        UIImportExport uiImportExport = null;
        UIDailyLogins uiDailyLogins = null;

        public UIModules(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(null, null);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;

            pbInfoDaily.Image = pbInfoHWID.Image = frm.UseDarkmode ? Properties.Resources.ic_info_outline_white_24dp : Properties.Resources.ic_info_outline_black_24dp;

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in flow.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
            {
                shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;

                shadow.BorderColor = second;
                shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
            }
        }

        private void btnPingChecker_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(frm.pingCheckerExePath))
                {
                    ProcessStartInfo info = new ProcessStartInfo(frm.pingCheckerExePath);
                    Process p = new Process()
                    {
                        StartInfo = info
                    };
                    p.Start();
                }
                else
                    frm.ShowSnackbar("Failed to find the Ping Checker module.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
            catch
            {
                frm.ShowSnackbar("Failed to start the Ping Checker module!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void btnShowGameUpdater_Click(object sender, EventArgs e)
        {
            frm.btnGameUpdater_Click(null, null);
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(frm.statisticsExePath))
                {
                    ProcessStartInfo info = new ProcessStartInfo(frm.statisticsExePath);
                    Process p = new Process()
                    {
                        StartInfo = info
                    };
                    p.Start();
                }
                else
                    frm.ShowSnackbar("Failed to find the statistics module.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
            catch
            {
                frm.ShowSnackbar("Failed to start the statistics module!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void btnVaultPeeker_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(frm.vaultPeekerExePath))
                {
                    ProcessStartInfo info = new ProcessStartInfo(frm.vaultPeekerExePath);
                    Process p = new Process()
                    {
                        StartInfo = info
                    };
                    p.Start();
                }
                else
                    frm.ShowSnackbar("Failed to find the vault peeker module.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
            catch
            {
                frm.ShowSnackbar("Failed to start the vault peeker module!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void btnShowHWIDTool_Click(object sender, EventArgs e)
        {
            if (eleHWID_Tool == null)
                eleHWID_Tool = new EleHWID_Tool(frm);
            else
                eleHWID_Tool.CheckState();

            frm.ShowShadowForm(eleHWID_Tool);
        }

        private void btnTokenViewer_Click(object sender, EventArgs e)
        {
            if (uiTokenViewer == null)
                uiTokenViewer = new UITokenViewer(frm) { Dock = DockStyle.Fill };
            else
                uiTokenViewer.LoadDataGridView();

            frm.AddContrtolToPContent(uiTokenViewer, 9);
        }

        private void btnShowChangelog_Click(object sender, EventArgs e)
        {
            if (uiChangelog == null)
                uiChangelog = new UIChangelog(frm) { Dock = DockStyle.Fill };

            frm.AddContrtolToPContent(uiChangelog, 8);
        }

        private void btnDailyLogins_Click(object sender, EventArgs e)
        {
            if (uiDailyLogins == null)
                uiDailyLogins = new UIDailyLogins(frm) { Dock = DockStyle.Fill };

            frm.AddContrtolToPContent(uiDailyLogins, 11);
        }

        private void btnImportExport_Click(object sender, EventArgs e)
        {
            OpenImporter();
        }

        public void OpenImporter()
        {
            if (uiImportExport == null)
                uiImportExport = new UIImportExport(frm) { Dock = DockStyle.Fill };

            frm.AddContrtolToPContent(uiImportExport, 10);
        }
        
    }
}
