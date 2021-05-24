using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EAM_Tasktray_Tool
{
    class EAM_ProgressBar : ProgressBar
    {
        public EAM_ProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // None... Helps control the flicker.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const int inset = 2; // A single inset value to control teh sizing of the inner rect.

            using (Image offscreenImage = new Bitmap(this.Width, this.Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.
                    rect.Width = (int)(rect.Width * ((double)this.Value / this.Maximum));
                    if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                    LinearGradientBrush brush = new LinearGradientBrush(rect, this.BackColor, this.ForeColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);

                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                }
            }
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    Rectangle rec = e.ClipRectangle;

        //    rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
        //    if (ProgressBarRenderer.IsSupported)
        //        ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
        //    rec.Height = rec.Height - 4;
        //    e.Graphics.FillRectangle(Brushes.White, 2, 2, rec.Width, rec.Height);
        //}
    }
}
