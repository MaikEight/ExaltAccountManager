using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MK_EAM_Captcha_Solver_UI_Lib
{
    public partial class FrmZoom : Form
    {
        public Image Image
        {
            set
            {
                pbZoom.Image = value;
                if (value == null) return;
                scale = (float)pbZoom.Width / (float)value.Width ;
            }
        }

        public PointF AimLocation
        {
            set
            {
                aimLocation = value;
                pbZoom.Refresh();
            }
        }
        private PointF aimLocation = new PointF(50, 50);
        private float scale = 1f;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public FrmZoom()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 11, 11));
            AimLocation = new PointF(
                this.Width / 2f,
                this.Height / 2f);
        }

        private void pbZoom_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.FromArgb(98, 0, 238), 1f))
            {
                e.Graphics.DrawLine(
                    p,
                    new PointF(0, aimLocation.Y * scale),
                    new PointF(pbZoom.Width, aimLocation.Y * scale)
                );
                e.Graphics.DrawLine(
                    p,
                    new PointF(aimLocation.X * scale, 0),
                    new PointF(aimLocation.X * scale, pbZoom.Height)
                );
            }
        }
    }
}
