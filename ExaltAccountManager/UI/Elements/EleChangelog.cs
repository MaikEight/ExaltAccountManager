using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleChangelog : UserControl
    {
        public ChangelogEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                LoadUI();
            }
        }
        private ChangelogEntry entry = new ChangelogEntry();

        FrmMain frm;

        public EleChangelog(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            lChangelog.ForeColor =
            this.ForeColor = font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            scrollbar.BorderColor = frm.UseDarkmode ? third : Color.Silver;
            scrollbar.BackgroundColor = frm.UseDarkmode ? def : third;
            scrollbar.ThumbColor = frm.UseDarkmode ? third : Color.Gray;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
        }

        private void LoadUI()
        {
            lVersion.Text = Entry.Version.ToString();
            lReleaseDate.Text = Entry.ReleaseDate.ToString("dd.MM.yyyy");
            lName.Text = Entry.Name;
            lChangelog.Text = Entry.Description;

            pContent.Height = lChangelog.Height;
            if (lChangelog.Height > 431)
            {
                scrollbar.Height = (this.Bottom - 17) - pContent.Top;
                scrollbar.Value = 0;
                scrollbar.BindTo(pContent);
                scrollbar.Enabled =
                scrollbar.Visible = true;
            }
            else
            {
                scrollbar.BindTo(pContent);
                scrollbar.Value = 0;
                scrollbar.Enabled =
                scrollbar.Visible = false;
            }

            this.Height = pContent.Bottom + 17;
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
