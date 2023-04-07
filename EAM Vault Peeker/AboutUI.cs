using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_Vault_Peeker
{
    public partial class AboutUI : UserControl
    {
        FrmMain frm;

        public AboutUI(FrmMain _frm)
        {
            InitializeComponent();

            frm = _frm;
            lVersion.Text = string.Format(lVersion.Text, frm.version);

            if (!System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, "flag.MPGH")))
            {
                linkWebsite.Text = "https://github.com/MaikEight/ExaltAccountManager";
            }
            ApplyTheme(null, null);

            frm.ThemeChanged += ApplyTheme;

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
        public void ApplyTheme(object sender, EventArgs e)
        {
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = ColorScheme.GetColorDef(frm.useDarkmode);

            this.ForeColor = ColorScheme.GetColorFont(frm.useDarkmode);

            pbProgram.Image = frm.useDarkmode ? Properties.Resources.btn_icon_chest_1 : Properties.Resources.btn_icon_chest_2;
            pbVersion.Image = frm.useDarkmode ? Properties.Resources.baseline_tag_white_36dp : Properties.Resources.baseline_tag_black_36dp;
            pbDev.Image = frm.useDarkmode ? Properties.Resources.ic_code_white_36dp : Properties.Resources.ic_code_black_36dp;
            pbWebsite.Image = frm.useDarkmode ? Properties.Resources.ic_public_white_36dp : Properties.Resources.ic_public_black_36dp;
            pbInfo.Image = frm.useDarkmode ? Properties.Resources.ic_info_outline_white_36dp : Properties.Resources.ic_info_outline_black_36dp;
            pbLogo.Image = frm.useDarkmode ? Properties.Resources.btn_icon_chest_1 : Properties.Resources.btn_icon_chest_2;

            foreach (Bunifu.UI.WinForms.BunifuShadowPanel ui in this.Controls.OfType<Bunifu.UI.WinForms.BunifuShadowPanel>())
                ui.BackColor = ui.PanelColor = ui.PanelColor2 = this.BackColor;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void pbDeveloper_Click(object sender, EventArgs e)
        {
            lTool.Text = "<> Maik8 </>";

            pbProgram.Image =
            pbVersion.Image =
            pbDev.Image =
            pbWebsite.Image =
            pbLogo.Image =
            pbInfo.Image = Properties.Resources.llama;

            pbVersion.SizeMode =
            pbDev.SizeMode =
            pbWebsite.SizeMode =
            pbInfo.SizeMode = PictureBoxSizeMode.Zoom;

            timerReset.Start();
        }

        private void timerReset_Tick(object sender, EventArgs e)
        {
            timerReset.Stop();

            lTool.Text = "EAM Vault Peeker";

            pbVersion.SizeMode =
            pbDev.SizeMode =
            pbWebsite.SizeMode =
            pbInfo.SizeMode = PictureBoxSizeMode.Normal;

            ApplyTheme(null, null);
        }
    }
}
