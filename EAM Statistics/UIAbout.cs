using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_Statistics
{
    public partial class UIAbout : UserControl
    {
        FrmMain frm;
        public UIAbout(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            lVersion.Text = string.Format(lVersion.Text, frm.version);
            ApplyTheme(frm.useDarkmode);

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

        public void ApplyTheme(bool isDarkmode)
        {
            Color def = Color.FromArgb(255, 255, 255);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (isDarkmode)
            {
                def = Color.FromArgb(32, 32, 32);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            ApplyTheme(isDarkmode, def, second, third, font);
        }

        public void ApplyTheme(bool isDarkmode, Color def, Color second, Color third, Color font)
        {
            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;

            lProgramTitle.BackColor = lProgram.BackColor = pbProgram.BackColor =
            lVersionTitle.BackColor = lVersion.BackColor = pbVersion.BackColor =
            lDeveloperTitle.BackColor = pbDev.BackColor = pbDeveloper.BackColor =
            lWebsiteTitle.BackColor = linkWebsite.BackColor = pbWebsite.BackColor =
            lCopyrightTitle.BackColor = lCopyright.BackColor = pbCopyright.BackColor = isDarkmode ? Color.FromArgb(45, 20, 20, 20) : def;

            this.ForeColor = font;

            pbProgram.Image = isDarkmode ? Properties.Resources.ic_assessment_white_36dp : Properties.Resources.ic_assessment_black_36dp;
            pbVersion.Image = isDarkmode ? Properties.Resources.baseline_tag_white_36dp : Properties.Resources.baseline_tag_black_36dp;
            pbDev.Image = isDarkmode ? Properties.Resources.ic_code_white_36dp : Properties.Resources.ic_code_black_36dp;
            pbWebsite.Image = isDarkmode ? Properties.Resources.ic_public_white_36dp : Properties.Resources.ic_public_black_36dp;
            pbCopyright.Image = isDarkmode ? Properties.Resources.ic_copyright_white_36dp : Properties.Resources.ic_code_black_36dp;
            pbLogo.Image = isDarkmode ? Properties.Resources.baseline_assessment_white_96 : Properties.Resources.baseline_assessment_black_96;

            foreach (Panel p in this.Controls.OfType<Panel>())
            {
                foreach (MaterialPanel ui in p.Controls.OfType<MaterialPanel>())
                    ui.ApplyTheme(isDarkmode);
            }
            foreach (MaterialPanel ui in this.Controls.OfType<MaterialPanel>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialTextPanel ui in this.Controls.OfType<MaterialTextPanel>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialSimpelTextPanel ui in this.Controls.OfType<MaterialSimpelTextPanel>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialTopAccount ui in this.Controls.OfType<MaterialTopAccount>())
                ui.ApplyTheme(isDarkmode);
            foreach (MaterialRadarChars ui in this.Controls.OfType<MaterialRadarChars>())
                ui.ApplyTheme(isDarkmode);

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void linkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkWebsite.Text);
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

            lTool.Text = "E A M  Statistics";

            pbProgram.SizeMode =
            pbVersion.SizeMode =
            pbDev.SizeMode =
            pbWebsite.SizeMode =
            pbLogo.SizeMode =
            pbCopyright.SizeMode = PictureBoxSizeMode.Normal;

            ApplyTheme(frm.useDarkmode);
        }
    }
}
