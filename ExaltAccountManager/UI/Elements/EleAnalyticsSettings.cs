using MK_EAM_Analytics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleAnalyticsSettings : UserControl
    {
        private FrmMain frm;
        public EleAnalyticsSettings(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;

            toggleAnonymize.Checked = frm.OptionsData.analyticsOptions.Anonymization;
            toggleSendAnalytics.Checked = !frm.OptionsData.analyticsOptions.OptOut;

            if (frm.OptionsData.analyticsOptions.OptOut || frm.OptionsData.analyticsOptions.Anonymization || AnalyticsClient.Instance == null)
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
        }
        Thread worker;
        private void btnRequestData_Click(object sender, EventArgs e)
        {
            btnRequestData.Enabled =
            btnDelete.Enabled = false;
            if (worker != null)
            {
                try
                {
                    worker.Abort();
                }
                catch { }
            }

            worker = new Thread(new ThreadStart(async () =>
            {

                var task = AnalyticsClient.Instance?.GetUserData(frm.GetAnalyticsClientIdHash());
                var data = await task;
                if (data != null)
                {
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    try
                    {
                        string path = System.IO.Path.Combine(Application.StartupPath, "AnalyticsUserData.txt");
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
                }
            }));
            worker.IsBackground = true;
            worker.Start();
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

            if (worker != null)
            {
                try
                {
                    worker.Abort();
                }
                catch { }
            }
            worker = new Thread(new ThreadStart(async () =>
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
                    var task = AnalyticsClient.Instance?.DeleteUser(frm.GetAnalyticsClientIdHash());
                    bool result = await task;
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
            }));
            worker.IsBackground = true;
            worker.Start();
            
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
    }
}
