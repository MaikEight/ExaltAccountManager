using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleHWID_Tool : UserControl
    {
        private FrmMain frm;

        private UIState uiState
        {
            get => uiStatevalue;
            set
            {
                uiStatevalue = value;
                switch (uiStatevalue)
                {
                    case UIState.NotInstalled:
                        {
                            lValue.Text = "The HWID-Tool is not installed.";
                            btnFunction.Enabled = true;
                            btnFunction.Text = "Download HWID-Tool";
                            pbState.Image = frm.UseDarkmode ? Properties.Resources.ic_do_not_disturb_white_36dp : Properties.Resources.ic_do_not_disturb_black_36dp;
                            if (timerBlinkAndCheck.Enabled) timerBlinkAndCheck.Stop();
                        }
                        break;
                    case UIState.Installing:
                        {
                            lValue.Text = "Downloading, please wait...";
                            btnFunction.Enabled = false;
                            btnFunction.Text = "Please wait";
                            pbState.Image = frm.UseDarkmode ? Properties.Resources.ic_do_not_disturb_white_36dp : Properties.Resources.ic_do_not_disturb_black_36dp;
                            if (timerBlinkAndCheck.Enabled) timerBlinkAndCheck.Stop();
                        }
                        break;
                    case UIState.Installed:
                        {
                            lValue.Text = "HWID-Tool is ready to run.";
                            btnFunction.Enabled = true;
                            btnFunction.Text = "Run HWID-Tool";
                            btnFunction.IconLeft = Properties.Resources.fingerprint_white_24px;
                            pbState.Image = frm.UseDarkmode ? Properties.Resources.outline_slow_motion_video_white_36dp : Properties.Resources.outline_slow_motion_video_black_36dp;
                            if (timerBlinkAndCheck.Enabled) timerBlinkAndCheck.Stop();
                        }
                        break;
                    case UIState.Executing:
                        {
                            lValue.Text = "HWID-Tool is running, please wait.";
                            btnFunction.Text = "Please wait";
                            btnFunction.IconLeft = Properties.Resources.fingerprint_white_24px;
                            btnFunction.Enabled = false;
                            pbState.Image = frm.UseDarkmode ? Properties.Resources.ic_hourglass_empty_white_36dp : Properties.Resources.ic_hourglass_empty_black_36dp;
                            timerBlinkAndCheck.Start();
                        }
                        break;
                    case UIState.Executed:
                        {
                            lValue.Text = "Your HWID is read out and saved.";
                            btnFunction.Text = "Copy your HWID to clipboard";
                            btnFunction.IconLeft = Properties.Resources.ic_content_copy_white_24dp;
                            btnFunction.Enabled = true;
                            pbState.Image = frm.UseDarkmode ? Properties.Resources.ok_white_36px : Properties.Resources.ok_36px;
                            if (timerBlinkAndCheck.Enabled) timerBlinkAndCheck.Stop();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private UIState uiStatevalue = UIState.None;

        private const string hwidToolURL = "https://github.com/MaikEight/EAM-GetClientHWID/releases/download/v1.0.0/EAM_GetClientHWID.zip";
        private bool isInstalled = false;

        public EleHWID_Tool(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            CheckState();

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);
        }

        public void CheckState()
        {
            isInstalled = CheckInstallation();
            if (File.Exists(frm.forceHWIDFilePath))
                uiState = UIState.Executed;
            else
                uiState = isInstalled ? UIState.Installed : UIState.NotInstalled;
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;

            pbHeader.Image = frm.UseDarkmode ? Properties.Resources.ic_fingerprint_white_48dp : Properties.Resources.ic_fingerprint_black_48dp;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            switch (uiState)
            {
                case UIState.NotInstalled:
                    pbState.Image = frm.UseDarkmode ? Properties.Resources.ic_do_not_disturb_white_36dp : Properties.Resources.ic_do_not_disturb_black_36dp;
                    break;
                case UIState.Installed:
                    pbState.Image = frm.UseDarkmode ? Properties.Resources.outline_slow_motion_video_white_36dp : Properties.Resources.outline_slow_motion_video_black_36dp;
                    break;
                case UIState.Executing:
                    pbState.Image = frm.UseDarkmode ? Properties.Resources.ic_hourglass_empty_white_36dp : Properties.Resources.ic_hourglass_empty_black_36dp;
                    break;
                case UIState.Executed:
                    pbState.Image = frm.UseDarkmode ? Properties.Resources.ok_white_36px : Properties.Resources.ok_36px;
                    break;
                default:
                    break;
            }
        }

        private bool CheckInstallation() => (Directory.Exists(frm.getClientHWIDToolPath) && File.Exists(Path.Combine(frm.getClientHWIDToolPath, "EAM GetClientHWID.exe")));

        private void btnFunction_Click(object sender, EventArgs e)
        {
            switch (uiState)
            {
                case UIState.NotInstalled:
                    {
                        if (frm.isMPGHVersion)
                        {
                            System.Diagnostics.Process.Start("https://www.mpgh.net/forum/forumdisplay.php?f=599");
                            frm.ShowSnackbar("Download the tool from MPGH and unzip it into the EAM-Programmpath.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 15000);
                        }
                        else
                        {
                            uiState = UIState.Installing;

                            if (!Directory.Exists(frm.getClientHWIDToolPath))
                                Directory.CreateDirectory(frm.getClientHWIDToolPath);

                            using (System.Net.WebClient client = new System.Net.WebClient())
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
                            uiState = isInstalled ? UIState.Installed : UIState.NotInstalled;
                        }
                    }
                    break;
                case UIState.Installed:
                    {
                        blinks = 0;
                        blinkState = false;
                        Process p = new Process()
                        {
                            StartInfo = new ProcessStartInfo(Path.Combine(frm.getClientHWIDToolPath, "EAM GetClientHWID.exe"))
                            {
                                Arguments = $"-batchmode {'{'}{FrmMain.saveFilePath}{'}'}"
                            }
                        };
                        p.Start();
                        uiState = UIState.Executing;
                    }
                    break;
                case UIState.Executed:
                    {
                        try
                        {
                            Clipboard.SetText(File.ReadAllText(frm.forceHWIDFilePath));
                            frm.ShowSnackbar("Copied your HWID to clipboard.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000);
                        }
                        catch
                        {
                            frm.ShowSnackbar("Failed to read HWID from file.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private enum UIState
        {
            None,
            NotInstalled,
            Installing,
            Installed,
            Executing,
            Executed
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e) => frm.RemoveShadowForm();

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

        private bool blinkState = false;
        private int blinks = 0;
        private void timerBlink_Tick(object sender, EventArgs e)
        {
            if (File.Exists(frm.forceHWIDFilePath))
            {
                timerBlinkAndCheck.Stop();

                uiState = UIState.Executed;
                frm.ShowSnackbar("HWID read and saved successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                return;
            }

            if (blinkState)
                pbState.Image = frm.UseDarkmode ? Properties.Resources.ic_hourglass_empty_white_36dp : Properties.Resources.ic_hourglass_empty_black_36dp;
            else
                pbState.Image = frm.UseDarkmode ? Properties.Resources.ic_hourglass_full_white_36dp : Properties.Resources.ic_hourglass_full_black_36dp;
            blinkState = !blinkState;

            if (++blinks > 20)
            {
                timerBlinkAndCheck.Stop();
                uiState = UIState.Installed;

                frm.ShowSnackbar("Coud not read HWID, please restart the EAM and try again.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                try
                {
                    List<Process> procs = new List<Process>();
                    foreach (var process in Process.GetProcesses())
                    {
                        try
                        {
                            if (process.ProcessName == "EAM GetClientHWID")
                                procs.Add(process);
                        }
                        catch (Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
                        {
                            // Intentionally empty - no security access to the process.                            
                        }
                        catch (InvalidOperationException)
                        {
                            // Intentionally empty - the process exited before getting details.                            
                        }
                        catch { }
                    }

                    while (procs.Count > 0)
                    {
                        try
                        {
                            procs[procs.Count - 1].Kill();
                            procs.RemoveAt(procs.Count - 1);
                        }
                        catch { procs.RemoveAt(procs.Count - 1); }
                    }
                }
                catch { }
            }
        }
    }
}
