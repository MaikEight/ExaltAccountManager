using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class MoreToolsUI : UserControl
    {
        FrmMain frm;
        Color backHover = Color.FromArgb(25, 0, 0, 0);

        public MoreToolsUI(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.ForeColor = pLeft.BackColor = pRight.BackColor = pBottom.BackColor = pTop.BackColor = font;
                this.BackColor = second;

                pChangelog.BackColor = lChangelog.BackColor = pbChangelog.BackColor = 
                pStatistics.BackColor = lStatistics.BackColor = pbStatistics.BackColor = 
                pAbout.BackColor = lAbout.BackColor = pbAbout.BackColor = 
                pTokenViewer.BackColor = lTokenViewer.BackColor = pbTokenViewer.BackColor =
                pPingChecker.BackColor = lPingChecker.BackColor = pbPingChecker.BackColor =
                pImExport.BackColor = lImExport.BackColor = pbImExport.BackColor =
                pUpdater.BackColor = lUpdater.BackColor = pbUpdater.BackColor =
                pHWIDTool.BackColor = lHWIDTool.BackColor = pbHWIDTool.BackColor = this.BackColor;

                pbStatistics.Image = Properties.Resources.ic_assessment_white_24dp;
                pbPingChecker.Image = Properties.Resources.time_36px;
                pbChangelog.Image = Properties.Resources.baseline_history_edu_white_48dp;
                pbAbout.Image = Properties.Resources.ic_info_outline_white_24dp;
                pbTokenViewer.Image = Properties.Resources.ic_visibility_white_24dp;
                pbImExport.Image = Properties.Resources.ic_import_export_white_24dp;
                pbUpdater.Image = Properties.Resources.below_white_24px;
                pbHWIDTool.Image = Properties.Resources.fingerprint_white_24px;

                backHover = Color.FromArgb(125, 75, 75, 75);
            }
            else
            {
                Color def = Color.FromArgb(255, 255, 255);
                Color second = Color.FromArgb(250, 250, 250);
                Color third = Color.FromArgb(230, 230, 230);
                Color font = Color.Black;
                
                this.ForeColor = pLeft.BackColor = pRight.BackColor = pBottom.BackColor = pTop.BackColor = font;
                this.BackColor = second;

                pChangelog.BackColor = lChangelog.BackColor = pbChangelog.BackColor =
                pStatistics.BackColor = lStatistics.BackColor = pbStatistics.BackColor = 
                pAbout.BackColor = lAbout.BackColor = pbAbout.BackColor = 
                pTokenViewer.BackColor = lTokenViewer.BackColor = pbTokenViewer.BackColor =
                pPingChecker.BackColor = lPingChecker.BackColor = pbPingChecker.BackColor =
                pImExport.BackColor = lImExport.BackColor = pbImExport.BackColor = 
                pUpdater.BackColor = lUpdater.BackColor = pbUpdater.BackColor =
                pHWIDTool.BackColor = lHWIDTool.BackColor = pbHWIDTool.BackColor = this.BackColor;

                pbStatistics.Image = Properties.Resources.ic_assessment_black_24dp;
                pbPingChecker.Image = Properties.Resources.time_black_36px;
                pbChangelog.Image = Properties.Resources.baseline_history_edu_black_48dp;
                pbAbout.Image = Properties.Resources.ic_info_outline_black_24dp;
                pbTokenViewer.Image = Properties.Resources.ic_visibility_black_24dp;
                pbImExport.Image = Properties.Resources.ic_import_export_black_24dp;
                pbUpdater.Image = Properties.Resources.below_black_24px;
                pbHWIDTool.Image = Properties.Resources.fingerprint_24px;

                backHover = Color.FromArgb(25, 0, 0, 0);
            }
            this.Invalidate();            
        }

        private void pStatistics_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            //Statistics FrmMain Size: 795; 624
            try
            {
                
                //var secondFormThread = new System.Threading.Thread(() => Application.Run(new EAM_Statistics.FrmMain { StartPosition = FormStartPosition.Manual, Location = new Point(frm.Location.X + ((frm.Width - 795) / 2), frm.Location.Y + ((frm.Height - 624) / 2)) }));
                var secondFormThread = new System.Threading.Thread(() => StartStatistics());
                secondFormThread.Start();
            }
            catch
            {
                frm.snackbar.Show(frm, $"Failed to start the Statistics Module!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }
        }

        [STAThread]
        private void StartStatistics()
        {
            try
            {
                EAM_Statistics.FrmMain form = new EAM_Statistics.FrmMain()
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = new Point(frm.Location.X + ((frm.Width - 795) / 2),
                    frm.Location.Y + ((frm.Height - 624) / 2))
                };
                Application.Run(form);
            }
            catch { }
        }

        //[STAThread]
        //private void StartPingChecker()
        //{
        //    try
        //    {
        //        using (EAM_PingChecker.FrmMain form = new EAM_PingChecker.FrmMain()
        //        {
        //            StartPosition = FormStartPosition.CenterParent
        //        })
        //        {
        //            Application.Run(form);
        //        }
        //    }
        //    catch { }
        //}

        private void pChangelog_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmChangelog frmChangelog = new FrmChangelog(frm);
            frmChangelog.StartPosition = FormStartPosition.Manual;
            frmChangelog.Location = new Point(frm.Location.X + ((frm.Width - frmChangelog.Width) / 2), frm.Location.Y + ((frm.Height - frmChangelog.Height) / 2));
            frmChangelog.Show(frm);

            frm.lockForm = true;
        }

        private void pChangelog_MouseEnter(object sender, EventArgs e)
        {
            pChangelog.BackColor = backHover;
            lChangelog.BackColor = pbChangelog.BackColor = Color.Transparent;
        }

        private void pChangelog_MouseLeave(object sender, EventArgs e)
        {
            pChangelog.BackColor = lChangelog.BackColor = pbChangelog.BackColor = this.BackColor;
        }

        private void pStatistics_MouseEnter(object sender, EventArgs e)
        {
            pStatistics.BackColor = backHover;
            lStatistics.BackColor = pbStatistics.BackColor = Color.Transparent;
        }

        private void pStatistics_MouseLeave(object sender, EventArgs e)
        {
            pStatistics.BackColor = lStatistics.BackColor = pbStatistics.BackColor = this.BackColor;
        }

        private void pAbout_MouseEnter(object sender, EventArgs e)
        {
            pAbout.BackColor = backHover;
            lAbout.BackColor = pbAbout.BackColor = Color.Transparent;
        }

        private void pAbout_MouseLeave(object sender, EventArgs e)
        {
            pAbout.BackColor = lAbout.BackColor = pbAbout.BackColor = this.BackColor;
        }

        private void pAbout_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmAbout frmAbout = new FrmAbout(frm);
            frmAbout.StartPosition = FormStartPosition.Manual;
            frmAbout.Location = new Point(frm.Location.X + ((frm.Width - frmAbout.Width) / 2), frm.Location.Y + ((frm.Height - frmAbout.Height) / 2));
            frmAbout.Show(frm);

            frm.lockForm = true;
        }

        private void pTokenViewer_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmTokenViewer frmTokenViewer = new FrmTokenViewer(frm);
            frmTokenViewer.StartPosition = FormStartPosition.Manual;
            frmTokenViewer.Location = new Point(frm.Location.X + ((frm.Width - frmTokenViewer.Width) / 2), frm.Location.Y + ((frm.Height - frmTokenViewer.Height) / 2));
            frmTokenViewer.Show(frm);

            frm.lockForm = true;
        }

        private void pTokenViewer_MouseEnter(object sender, EventArgs e)
        {
            pTokenViewer.BackColor = backHover;
            lTokenViewer.BackColor = pbTokenViewer.BackColor = Color.Transparent;
        }

        private void pTokenViewer_MouseLeave(object sender, EventArgs e)
        {
            pTokenViewer.BackColor = lTokenViewer.BackColor = pbTokenViewer.BackColor = this.BackColor;
        }

        private void pPingChecker_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            try
            {
                //var secondFormThread = new System.Threading.Thread(() => StartPingChecker());
                //secondFormThread.Start();
                if (System.IO.File.Exists(frm.pingCheckerExePath))
                {
                    ProcessStartInfo info = new ProcessStartInfo(frm.pingCheckerExePath);
                    Process p = new Process()
                    {
                        StartInfo = info
                    };
                    p.Start();
                }
            }
            catch
            {
                frm.snackbar.Show(frm, $"Failed to start the Ping Checker Module!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            }
        }

        private void pPingChecker_MouseEnter(object sender, EventArgs e)
        {
            pPingChecker.BackColor = backHover;
            lPingChecker.BackColor = pbPingChecker.BackColor = Color.Transparent;
        }

        private void pPingChecker_MouseLeave(object sender, EventArgs e)
        {
            pPingChecker.BackColor = lPingChecker.BackColor = pbPingChecker.BackColor = this.BackColor;
        }

        private void pImExport_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmImExport frmImExport = new FrmImExport(frm);
            frmImExport.StartPosition = FormStartPosition.Manual;
            frmImExport.Location = new Point(frm.Location.X + ((frm.Width - frmImExport.Width) / 2), frm.Location.Y + ((frm.Height - frmImExport.Height) / 2));
            frmImExport.Show(frm);

            frm.lockForm = true;
        }

        private void pImExport_MouseEnter(object sender, EventArgs e)
        {
            pImExport.BackColor = backHover;
            lImExport.BackColor = pbImExport.BackColor = Color.Transparent;
        }

        private void pImExport_MouseLeave(object sender, EventArgs e)
        {
            pImExport.BackColor = lImExport.BackColor = pbImExport.BackColor = this.BackColor;
        }

        public void pUpdater_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmUpdater frmUpdater = new FrmUpdater(frm);
            frmUpdater.StartPosition = FormStartPosition.Manual;
            frmUpdater.Location = new Point(frm.Location.X + ((frm.Width - frmUpdater.Width) / 2), frm.Location.Y + ((frm.Height - frmUpdater.Height) / 2));
            frmUpdater.Show(frm);

            frm.lockForm = true;
        }

        private void pUpdater_MouseEnter(object sender, EventArgs e)
        {
            pUpdater.BackColor = backHover;
            lUpdater.BackColor = pbUpdater.BackColor = Color.Transparent;
        }

        private void pUpdater_MouseLeave(object sender, EventArgs e)
        {
            pUpdater.BackColor = lUpdater.BackColor = pbUpdater.BackColor = this.BackColor;            
        }

        private void pHWIDTool_Click(object sender, EventArgs e)
        {
            frm.ShowMoreUI(true);
            if (frm.lockForm) return;

            FrmHWIDTool frmHWID = new FrmHWIDTool(frm);
            frmHWID.StartPosition = FormStartPosition.Manual;
            frmHWID.Location = new Point(frm.Location.X + ((frm.Width - frmHWID.Width) / 2), frm.Location.Y + ((frm.Height - frmHWID.Height) / 2));
            frmHWID.Show(frm);

            frm.lockForm = true;
        }

        private void pHWIDTool_MouseEnter(object sender, EventArgs e)
        {
            pHWIDTool.BackColor = backHover;
            lHWIDTool.BackColor = pbHWIDTool.BackColor = Color.Transparent;
        }

        private void pHWIDTool_MouseLeave(object sender, EventArgs e)
        {
            pHWIDTool.BackColor = lHWIDTool.BackColor = pbHWIDTool.BackColor = this.BackColor;
        }
    }
}
