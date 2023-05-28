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
            }
        }

        public Point AimLocation
        {
            set
            {
                aimLocation = value;
                pbZoom.Refresh();
            }
        }
        private Point aimLocation = new Point(50, 50);

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
            AimLocation = new Point(
                this.Width / 2,
                this.Height / 2);
        }

        private void pbZoom_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.FromArgb(98, 0, 238), 1f))
            {
                e.Graphics.DrawLine(
                    p,
                    new Point(0, aimLocation.Y),
                    new Point(pbZoom.Width, aimLocation.Y)
                );
                e.Graphics.DrawLine(
                    p,
                    new Point(aimLocation.X, 0),
                    new Point(aimLocation.X, pbZoom.Height)
                );
            }
        }
    }
}
