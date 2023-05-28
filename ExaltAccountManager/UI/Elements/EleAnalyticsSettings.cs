using MK_EAM_Analytics;
using MK_EAM_General_Services_Lib;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleAnalyticsSettings : UserControl
    {
        private FrmMain frm;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        
        public EleAnalyticsSettings(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;

            toggleAnonymize.Checked = frm.OptionsData.analyticsOptions.Anonymization;
            toggleSendAnalytics.Checked = !frm.OptionsData.analyticsOptions.OptOut;

            if (frm.OptionsData.analyticsOptions.OptOut || 
                frm.OptionsData.analyticsOptions.Anonymization || 
                AnalyticsClient.Instance == null || 
                AnalyticsClient.Instance?.SessionId == Guid.Empty || 
                AnalyticsClient.Instance?.SessionId == Guid.Parse("45414D20-0000-6279-0000-204D61696B38"))
            {
                btnRequestData.Enabled =
                btnDelete.Enabled = false;
            }

            ApplyTheme(this, EventArgs.Empty);

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.ForeColor = lAnonymize.ForeColor = lSendData.ForeColor = lInfo.ForeColor = font;

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel shadow in this.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
            {
                shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
                shadow.BorderColor = shadow.BorderColor = second;
                shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
            }

            pbInfoDaily.Image = frm.UseDarkmode ? Properties.Resources.ic_info_outline_white_24dp : Properties.Resources.ic_info_outline_black_24dp;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;
        }

        private void toggleSendAnalytics_CheckedChanged(object sender, EventArgs e)
        {
            pbState.Image = toggleSendAnalytics.Checked ? Properties.Resources.icons8_smiling_48px : Properties.Resources.sad_48px;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frm.OptionsData.analyticsOptions.Anonymization = toggleAnonymize.Checked;
            frm.OptionsData.analyticsOptions.OptOut = !toggleSendAnalytics.Checked;

            frm.SaveOptions(frm.OptionsData, true);
            frm.UpdateUIOptionsData();

            if (frm.OptionsData.analyticsOptions.OptOut && AnalyticsClient.Instance?.SessionId != Guid.Empty)
            {
                AnalyticsClient.Instance.EndSeesion();
            }
        }

        private void btnRequestData_Click(object sender, EventArgs e)
        {
            btnRequestData.Enabled =
            btnDelete.Enabled = false;
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(7500);

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;

                try
                {
                    Task<MK_EAM_Analytics.Response.Data.User> task = AnalyticsClient.Instance?.GetUserData(frm.GetAPIClientIdHash());
                    while (!task.IsCompleted)
                    {
                        if (token.IsCancellationRequested)
                        {
                            frm.LogEvent(new MK_EAM_Lib.LogData(
                                "EAM News",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Failed to request own userdata"));

                            return;
                        }

                        await Task.Delay(50, token);
                    }
                    MK_EAM_Analytics.Response.Data.User result = task.Result;
                    if (result != null)
                    {
                        string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                        try
                        {
                            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AnalyticsUserData.txt");
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                            System.IO.File.WriteAllText(path, json);
                            Process.Start(path);
                        }
                        catch
                        {
                            MessageBox.Show(json, "Your Data (save mode)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        OnRequestedDataReceivedInvoker();
                        return;
                    }

                    frm.LogEvent(new MK_EAM_Lib.LogData(
                                "EAM News",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Either the process to fetch your data failed or no data could be found."));
                }
                catch { }

                MessageBox.Show("Either the process to fetch your data failed or no data could be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }), cancellationTokenSource.Token);
        }


        private void OnRequestedDataReceivedInvoker()
        {
            OnRequestedDataReceived();
        }

        private bool OnRequestedDataReceived()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)OnRequestedDataReceived);

            btnDelete.Enabled = true;

            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnRequestData.Enabled =
            btnDelete.Enabled = false;

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(7500);

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;

                try
                {
                    bool didDelete = false;

                    if (MessageBox.Show(
                    "Are you sure you want to delete all collected data?",
                    "Delete all data?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2)
                    == DialogResult.Yes)
                    {
                        didDelete = true;
                        //Send Delete
                        Task<bool> task = AnalyticsClient.Instance?.DeleteUser(frm.GetAPIClientIdHash());
                        while (!task.IsCompleted)
                        {
                            if (token.IsCancellationRequested)
                            {
                                didDelete = false;

                                frm.LogEvent(new MK_EAM_Lib.LogData(
                                    "EAM Analytics",
                                    MK_EAM_Lib.LogEventType.APIError,
                                    "Failed to delete own userdata"));

                                MessageBox.Show(
                                    "Failed to delete data. \nPlease try again later.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                return;
                            }

                            await Task.Delay(50, token);
                        }
                        bool result = task.Result;

                        if (result)
                        {
                            MessageBox.Show(
                                "All data has been deleted.",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            //Ask to turn of analytics
                            if (!frm.OptionsData.analyticsOptions.OptOut && MessageBox.Show("Do you want to disable sending of analytics data?", "Disable analytics?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                frm.OptionsData.analyticsOptions.OptOut = true;
                                frm.SaveOptions(frm.OptionsData, true);
                            }
                        }
                        else
                        {
                            didDelete = false;
                            MessageBox.Show(
                                "Failed to delete data. \nPlease try again later.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }

                    OnUserDeletedInvoker(didDelete);
                }
                catch { }
            }), cancellationTokenSource.Token);
        }

        private void OnUserDeletedInvoker(bool didDelete)
        {
            OnUserDeleted(didDelete);
        }

        private bool OnUserDeleted(bool didDelete)
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool, bool>)OnUserDeleted, didDelete);

            if (didDelete)
            {
                toggleAnonymize.Checked = frm.OptionsData.analyticsOptions.Anonymization;
                toggleSendAnalytics.Checked = !frm.OptionsData.analyticsOptions.OptOut;
            }

            btnRequestData.Enabled =
            btnDelete.Enabled = !didDelete;

            return false;
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

        private void btnViewPrivacyPolicy_Click(object sender, EventArgs e)
        {
            btnViewPrivacyPolicy.Enabled = false;

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(7500);

            ThreadPool.QueueUserWorkItem(new WaitCallback(async (object obj) =>
            {
                CancellationToken token = (CancellationToken)obj;

                try
                {

                    Task<MK_EAM_General_Services_Lib.General.Responses.GetFileResponse> task =
                        GeneralServicesClient.Instance?.GetPrivacyPolicy();

                    string path = frm.privacyPolicyPath;

                    while (!task.IsCompleted)
                    {
                        if (token.IsCancellationRequested)
                        {
                            frm.LogEvent(new MK_EAM_Lib.LogData(
                                "EAM Analytics",
                                MK_EAM_Lib.LogEventType.APIError,
                                "Failed to request privacy policy."));

                            if (File.Exists(path))
                            {
                                OpenPrivacyPolicy(path);
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Failed to open privacy policy.\nPlease try again later.\nIf this problem persists, please get in touch via Discord or email to get a copy of the privacy policy.\nE-Mail: mail@maik8.de",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }

                            return;
                        }

                        await Task.Delay(50, token);
                    }
                    MK_EAM_General_Services_Lib.General.Responses.GetFileResponse result = task.Result;

                    if (result != null && result.data != null)
                    {
                        
                        try
                        {
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            File.WriteAllBytes(path, result.data);

                            OpenPrivacyPolicy(path);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(
                                    "Failed to download the latest privacy policy.\nPlease try again later.\nIf this problem persists, please get in touch via Discord or email to get a copy of the privacy policy.\nE-Mail: mail@maik8.de",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                            if (File.Exists(path))
                                OpenPrivacyPolicy(path);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Failed to open privacy policy.\nPlease try again later.\nIf this problem persists, please get in touch via Discord or email to get a copy of the privacy policy.\nE-Mail: mail@maik8.de",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        if (File.Exists(path))
                            OpenPrivacyPolicy(path);
                    }

                    OnViewedPrivacyPolicyInvoker();
                }
                catch { }
            }), cancellationTokenSource.Token);
        }

        private void OnViewedPrivacyPolicyInvoker()
        {
            OnViewedPrivacyPolicy();
        }

        private bool OnViewedPrivacyPolicy()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)OnViewedPrivacyPolicy);

            btnViewPrivacyPolicy.Enabled = true;

            return false;
        }

        private void OpenPrivacyPolicy(string path)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "notepad.exe",
                Arguments = path
            });
        }
    }
}
