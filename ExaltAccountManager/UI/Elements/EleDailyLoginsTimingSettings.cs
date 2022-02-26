using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleDailyLoginsTimingSettings : UserControl
    {
        FrmMain frm;
        public EleDailyLoginsTimingSettings(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);

            LoadUI();
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            lQuestion.Focus();

            tbSecondsToJoinNexus.ResetColors();
            tbKill.ResetColors();

            this.BackColor =
            tbSecondsToJoinNexus.BackColor = tbSecondsToJoinNexus.FillColor = tbSecondsToJoinNexus.OnIdleState.FillColor = tbSecondsToJoinNexus.OnHoverState.FillColor = tbSecondsToJoinNexus.OnActiveState.FillColor = tbSecondsToJoinNexus.OnDisabledState.FillColor =
            tbKill.BackColor = tbKill.FillColor = tbKill.OnIdleState.FillColor = tbKill.OnHoverState.FillColor = tbKill.OnActiveState.FillColor = tbKill.OnDisabledState.FillColor =
            def;

            this.ForeColor =
            tbSecondsToJoinNexus.ForeColor = tbSecondsToJoinNexus.OnActiveState.ForeColor = tbSecondsToJoinNexus.OnDisabledState.ForeColor = tbSecondsToJoinNexus.OnHoverState.ForeColor = tbSecondsToJoinNexus.OnIdleState.ForeColor =
            tbKill.ForeColor = tbKill.OnActiveState.ForeColor = tbKill.OnDisabledState.ForeColor = tbKill.OnHoverState.ForeColor = tbKill.OnIdleState.ForeColor =
            font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
        }

        private void LoadUI()
        {
            NotificationOptions notOpt = GetNotificationOptions();

            tbSecondsToJoinNexus.Text = notOpt.joinTime.ToString();
            tbKill.Text = notOpt.killTime.ToString();
        }

        private NotificationOptions GetNotificationOptions()
        {
            NotificationOptions notOpt = new NotificationOptions();
            if (File.Exists(frm.notificationOptionsPath))
            {
                try
                {
                    notOpt = (NotificationOptions)frm.ByteArrayToObject(File.ReadAllBytes(frm.notificationOptionsPath));
                }
                catch
                {
                    frm.LogEvent(new LogData(-1, "EAM Timings", LogEventType.EAMError, "Failed to load the timing-settings."));
                    frm.ShowSnackbar("Failed to load the timing-settings.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);

                    try
                    {
                        notOpt = new NotificationOptions();
                        File.WriteAllBytes(frm.notificationOptionsPath, frm.ObjectToByteArray(notOpt));
                    }
                    catch { }
                }
            }
            else
            {
                try
                {
                    File.WriteAllBytes(frm.notificationOptionsPath, frm.ObjectToByteArray(notOpt));
                }
                catch { }
            }
            return notOpt;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            NotificationOptions notOpt = GetNotificationOptions();

            try
            {
                int.TryParse(tbSecondsToJoinNexus.Text, out int j);
                int.TryParse(tbKill.Text, out int k);

                notOpt.joinTime = (j > 0) ? j : 90;
                notOpt.killTime = (k > 0) ? k : 30;
            }
            catch { }

            try
            {
                File.WriteAllBytes(frm.notificationOptionsPath, frm.ObjectToByteArray(notOpt));

                frm.LogEvent(new LogData(-1, "EAM Timings", LogEventType.SaveNotify, "Saved new timing-settings."));
                frm.ShowSnackbar("Saved timing-settings successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);

                pbClose_Click(pbClose, null);
            }
            catch
            {
                frm.LogEvent(new LogData(-1, "EAM Timings", LogEventType.SaveNotify, "Failed to save the timing-settings."));
                frm.ShowSnackbar("Failed to save the timing-settings.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Back)
                e.SuppressKeyPress = !int.TryParse(Convert.ToString((char)e.KeyData), out int _);
        }
    }
}
