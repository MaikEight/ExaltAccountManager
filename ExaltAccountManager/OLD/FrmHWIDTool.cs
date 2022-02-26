using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmHWIDTool : Form
    {
        FrmMainOLD frm;

        private const string hwidToolURL = "https://github.com/MaikEight/EAM-GetClientHWID/releases/download/v1.0.0/EAM_GetClientHWID.zip";
        private bool isInstalled = false;

        public FrmHWIDTool(FrmMainOLD _frm)
        {
            InitializeComponent();
            frm = _frm;

            isInstalled = CheckInstallation();

            if (isInstalled)
            {
                lState.BackColor = Color.FromArgb(75, 50, 200, 50);
                lState.Text = "HWID-Tool installed";
                btnDownload.Text = "     Start HWID-Tool";
                btnDownload.TextAlign = ContentAlignment.MiddleCenter;
                btnDownload.Image = frm.useDarkmode ? Properties.Resources.fingerprint_white_24px : Properties.Resources.fingerprint_24px;
            }

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

                this.ForeColor = lState.ForeColor = font;
                this.BackColor = second;
                pTop.BackColor = def;
                pBox.BackColor = def;

                btnDownload.Image = isInstalled ? Properties.Resources.fingerprint_white_24px : Properties.Resources.below_white_24px;
                pbLogo.Image = Properties.Resources.ic_fingerprint_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;

                p = new Pen(Color.White);
            }
        }

        private bool CheckInstallation() => (Directory.Exists(frm.getClientHWIDToolPath) && File.Exists(Path.Combine(frm.getClientHWIDToolPath, "EAM GetClientHWID.exe")));

        private void btnDownload_Click(object sender, EventArgs e)
        {
            isInstalled = CheckInstallation();

            if (isInstalled)
            {
                lState.BackColor = Color.FromArgb(75, 50, 200, 50);
                lState.Text = "HWID-Tool installed";
                btnDownload.Text = "     Start HWID-Tool";
                btnDownload.TextAlign = ContentAlignment.MiddleCenter;
                btnDownload.Image = frm.useDarkmode ? Properties.Resources.fingerprint_white_24px : Properties.Resources.fingerprint_24px;
            }

            if (isInstalled)
            {
                btnDownload.Text = "Please wait";
                btnDownload.TextAlign = ContentAlignment.MiddleCenter;
                btnDownload.Enabled = false;

                ProcessStartInfo info = new ProcessStartInfo(Path.Combine(frm.getClientHWIDToolPath, "EAM GetClientHWID.exe"))
                {
                    Arguments = $"-batchmode {'{'}{FrmMainOLD.saveFilePath}{'}'}"
                };
                Process p = new Process()
                {
                    StartInfo = info
                };
                p.Start();

                System.Threading.Thread.Sleep(3000);

                if (File.Exists(frm.forceHWIDFilePath))
                    frm.snackbar.Show(frm, $"HWID updated successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                else
                    frm.snackbar.Show(frm, $"Coud not read HWID, please restart the EAM and try again.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 7500, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);

                this.Close();
            }
            else
            {
                if (frm.isMPGHVersion)
                {
                    System.Diagnostics.Process.Start("https://www.mpgh.net/forum/forumdisplay.php?f=599");
                    frm.snackbar.Show(this, $"Download the tool from MPGH and unzip it into the EAM-Programmpath.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 15000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                }
                else
                {
                    btnDownload.Text = "Installing...";
                    btnDownload.TextAlign = ContentAlignment.MiddleCenter;
                    btnDownload.Enabled = false;

                    if (!Directory.Exists(frm.getClientHWIDToolPath))
                        Directory.CreateDirectory(frm.getClientHWIDToolPath);

                    using (WebClient client = new WebClient())
                    {
                        string filePath = Path.Combine(frm.getClientHWIDToolPath, "download.zip");

                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        client.DownloadFile(hwidToolURL, filePath);
                        if (File.Exists(filePath))
                        {
                            System.IO.Compression.ZipFile.ExtractToDirectory(filePath, Application.StartupPath);
                            File.Delete(filePath);
                        }
                    }

                    isInstalled = CheckInstallation();

                    if (isInstalled)
                    {
                        lState.BackColor = Color.FromArgb(75, 50, 200, 50);
                        lState.Text = "HWID-Tool installed";
                        btnDownload.Text = "     Start HWID-Tool";
                        btnDownload.TextAlign = ContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        btnDownload.Text = "Download HWID-Tool ";
                        btnDownload.TextAlign = ContentAlignment.MiddleRight;
                    }

                    btnDownload.Enabled = true;
                }
            }
        }

        #region Button Close / Minimize

        private void pbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbClose.BackColor = Color.FromArgb(225, 50, 50);
            else
                pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbMinimize.BackColor = Color.DimGray;
            else
                pbMinimize.BackColor = Color.DarkGray;
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e)
        {
            pbMinimize.BackColor = Color.Transparent;
        }

        #endregion

        Pen p = new Pen(Color.Black);

        private void FrmHWIDTool_Paint(object sender, PaintEventArgs e)
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

        private void pbClose_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
        }

        private void pbMinimize_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);

            e.Graphics.DrawLine(p, topLeft, topRight);
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
        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }
    }
}
