using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ExaltAccountManager
{
    public sealed partial class FrmDiscordPopup : Form
    {
        FrmMain frm;

        public FrmDiscordPopup(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            lQuestion.Text = string.Format(lQuestion.Text, DiscordHelper.GetUserName());

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) =>
            {
                frm.ThemeChanged -= ApplyTheme;
            };
            ApplyTheme(this, EventArgs.Empty);

            this.BringToFront();
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            #region ApplyTheme

            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.ForeColor = font;
            this.BackColor = pTop.BackColor = def;

            pTopBar.BackColor = second;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;

            shadow.BorderColor = second;
            shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
            pbClose.BackColor = pTopBar.BackColor;
            #endregion
        }

        private void btnYeah_Click(object sender, EventArgs e)
        {
            SaveSettings(new DiscordPopupSettings()
            {
                LastQuestion = DateTime.Now,
                LastDiscordPopupResult = DiscordPopupSettings.DiscordpopupResult.Yes,
            });

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnLater_Click(object sender, EventArgs e)
        {
            SaveSettings(new DiscordPopupSettings()
            {
                LastQuestion = DateTime.Now,
                LastDiscordPopupResult = DiscordPopupSettings.DiscordpopupResult.No,
            });

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnNever_Click(object sender, EventArgs e)
        {
            SaveSettings(new DiscordPopupSettings()
            {
                LastQuestion = DateTime.Now,
                LastDiscordPopupResult = DiscordPopupSettings.DiscordpopupResult.Never,
            });           
            
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void SaveSettings(DiscordPopupSettings settings)
        {
            try
            {
                System.IO.File.WriteAllText(frm.pathDiscordPopups, JsonConvert.SerializeObject(settings));
            }
            catch
            {
                frm.LogEvent(
                    new MK_EAM_Lib.LogData(
                        "Discord Popup",
                        MK_EAM_Lib.LogEventType.Error,
                        "Failed to save DiscordPopupSettings to file")
                    );
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Close();
        }

        private void pbClose_MouseDown(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Red;


        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_24dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = pTopBar.BackColor;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_24dp : Properties.Resources.ic_close_black_24dp;
        }
    }
}
