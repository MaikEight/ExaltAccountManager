using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MK_EAM_Lib;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleDailyLoginsNotificationsettings : UserControl
    {
        private FrmMain frm;

        public EleDailyLoginsNotificationsettings(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);
            
            LoadUI();
        }

        private void LoadUI()
        {

            NotificationOptions notOpt = GetNotificationOptions();

            toggleUseNotifications.Checked = notOpt.useTaskTrayTool;
            toggleEvent.Checked = notOpt.useNotifications;
            toggleStart.Checked = notOpt.showNotificationOnStart;
            toggleFinished.Checked = notOpt.showNotificationOnDone;
            toggleError.Checked = notOpt.showNotificationOnError;

            toggleUseNotifications_CheckedChanged(toggleUseNotifications, null);
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
                    frm.LogEvent(new LogData(-1, "EAM Notifications", LogEventType.EAMError, "Failed to load the notification-settings."));
                    frm.ShowSnackbar("Failed to load the notification-settings.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);

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

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
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

            notOpt.useTaskTrayTool = toggleUseNotifications.Checked;
            notOpt.useNotifications = toggleEvent.Checked;
            notOpt.showNotificationOnStart = toggleStart.Checked;
            notOpt.showNotificationOnDone = toggleFinished.Checked;
            notOpt.showNotificationOnError = toggleError.Checked;

            try
            {
                File.WriteAllBytes(frm.notificationOptionsPath, frm.ObjectToByteArray(notOpt));

                frm.LogEvent(new LogData(-1, "EAM Notifications", LogEventType.SaveNotify, "Saved new notification-settings."));
                frm.ShowSnackbar("Saved notification-settings successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);

                pbClose_Click(pbClose, null);
            }
            catch 
            {
                frm.LogEvent(new LogData(-1, "EAM Notifications", LogEventType.SaveNotify, "Failed to save the notification-settings."));
                frm.ShowSnackbar("Failed to save the notification-settings.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void toggleUseNotifications_CheckedChanged(object sender, EventArgs e)
        {
            toggleEvent.Enabled =
            toggleStart.Enabled =
            toggleFinished.Enabled =
            toggleError.Enabled = toggleUseNotifications.Checked;
        }
    }
}
