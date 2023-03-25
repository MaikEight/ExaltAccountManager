using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EAM_PingChecker
{
    public partial class UIAbout : UserControl
    {
        FrmMain frm;

        public UIAbout(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            lVersion.Text = string.Format(lVersion.Text, frm.version);

            linkWebsite.Text = "https://github.com/MaikEight/ExaltAccountManager";

            ApplyTheme(frm.useDarkmode, ColorScheme.GetColorDef(frm.useDarkmode), ColorScheme.GetColorSecond(frm.useDarkmode), ColorScheme.GetColorThird(frm.useDarkmode), ColorScheme.GetColorFont(frm.useDarkmode));

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            int h = pbDeveloper.Height;
            int w = pbDeveloper.Width;
            float dividerW = 16f;
            float dividerH = 4f;

            gp.AddPolygon(new PointF[]
            {
                new PointF(w / dividerW, 0),
                new PointF(w - (w / dividerW), 0),
                new PointF(w, h / dividerH),
                new PointF(w, h - (h / dividerH)),
                new PointF(w - (w / dividerW), h),
                new PointF(w / dividerW, h),
                new PointF(0, h - (h / dividerH)),
                new PointF(0, h / dividerH),
            });
            Region rg = new Region(gp);
            pbDeveloper.Region = rg;
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;
            //lProgramTitle.BackColor = lProgram.BackColor = pbProgram.BackColor =
            //lVersionTitle.BackColor = lVersion.BackColor = pbVersion.BackColor =
            //lDeveloperTitle.BackColor = pbDev.BackColor = pbDeveloper.BackColor =
            //lWebsiteTitle.BackColor = linkWebsite.BackColor = pbWebsite.BackColor =
            //lCopyrightTitle.BackColor = lCopyright.BackColor = pbCopyright.BackColor = isDarkmode ? Color.FromArgb(45, 20, 20, 20) : def;

            this.ForeColor = font;

            pbProgram.Image = isDarkmode ? Properties.Resources.time_white_36px : Properties.Resources.time_black_36px;
            pbVersion.Image = isDarkmode ? Properties.Resources.baseline_tag_white_36dp : Properties.Resources.baseline_tag_black_36dp;
            pbDev.Image = isDarkmode ? Properties.Resources.ic_code_white_36dp : Properties.Resources.ic_code_black_36dp;
            pbWebsite.Image = isDarkmode ? Properties.Resources.ic_public_white_36dp : Properties.Resources.ic_public_black_36dp;
            pbCopyright.Image = isDarkmode ? Properties.Resources.ic_copyright_white_36dp : Properties.Resources.ic_copyright_black_36dp;
            pbLogo.Image = isDarkmode ? Properties.Resources.time_white_96px : Properties.Resources.time_black_96px;

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel ui in this.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
                ui.BackColor = ui.PanelColor = ui.PanelColor2 = def;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void pbDeveloper_Click(object sender, EventArgs e)
        {
            lTool.Text = "< > Maik8 </>";

            pbProgram.Image =
            pbVersion.Image =
            pbDev.Image =
            pbWebsite.Image =
            pbLogo.Image =
            pbCopyright.Image = Properties.Resources.llama;

            pbProgram.SizeMode =
            pbVersion.SizeMode =
            pbDev.SizeMode =
            pbWebsite.SizeMode =
            pbLogo.SizeMode =
            pbCopyright.SizeMode = PictureBoxSizeMode.Zoom;

            timerReset.Start();
        }

        private void timerReset_Tick(object sender, EventArgs e)
        {
            timerReset.Stop();

            lTool.Text = "EAM  Ping Checker";

            pbProgram.SizeMode =
            pbVersion.SizeMode =
            pbDev.SizeMode =
            pbWebsite.SizeMode =
            pbLogo.SizeMode =
            pbCopyright.SizeMode = PictureBoxSizeMode.Normal;

            ApplyTheme(frm.useDarkmode, ColorScheme.GetColorDef(frm.useDarkmode), ColorScheme.GetColorSecond(frm.useDarkmode), ColorScheme.GetColorThird(frm.useDarkmode), ColorScheme.GetColorFont(frm.useDarkmode));
        }
    }
}
