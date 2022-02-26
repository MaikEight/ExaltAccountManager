using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAM_Vault_Peeker.UI
{
    public class ItemUI : PictureBox
    {
        FrmMain frm;
        public Item item;
        public int amount = 0;
        public bool filteredByType = false;
        public bool filteredByFeedpower = false;
        public bool filteredByTier = false;
        public bool filteredByName = false;
        public bool filteredBySelectedAccount = false;

        public event EventHandler ShowPreview;

        private Size imageSize = new Size(40, 40);
        public Size ImageSize
        {
            get => imageSize;
            set
            {
                imageSize = value;
                this.Invalidate();
            }
        }

        private Point imagePoint = new Point(3, 3);
        public Point ImagePoint
        {
            get => imagePoint;
            set
            {
                imagePoint = value;
                this.Invalidate();
            }
        }

        private bool hideDuplicates = false;
        public bool HideDuplicates
        {
            get => hideDuplicates;
            set
            {
                hideDuplicates = value;
                this.Invalidate();
            }
        }

        private bool highlighted = false;

        public bool Highlighted 
        {
            get => highlighted;
            set 
            {
                highlighted = value;
                this.Invalidate();
            }
        }

        public ItemUI(FrmMain _frm, Item _item)
        {
            this.Size = new Size(46, 46);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.Margin = new Padding(1, 1, 1, 1);

            frm = _frm;
            item = _item;
            if (item.id != 0)
                Image = GetImageByCoords(item.x, item.y);
            else
                Image = Properties.Resources.question_mark;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);
        }

        ~ItemUI()
        {
            ShowPreview = null;
        }

        public Point GetPointOnScreen() => this.PointToScreen(Point.Empty);/*this.PointToScreen(new Point(this.Right + 3, this.Top));*/

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.useDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.useDarkmode);
            Color third = ColorScheme.GetColorThird(frm.useDarkmode);
            Color font = ColorScheme.GetColorFont(frm.useDarkmode);

            this.ForeColor = font;
            this.BackColor = def;
            //BackColor = def;
        }

        private Bitmap GetImageByCoords(int x, int y)
        {
            if ((x != -1 && y != -1) &&  x + (Width - 6) <= frm.renders.Width && y + (Height - 6) <= frm.renders.Height)
            {
                Bitmap bmp = new Bitmap(Width, Height);
                var graphics = Graphics.FromImage(bmp);
                graphics.DrawImage(frm.renders, new Rectangle(0, 0, Width - 6, Height - 6), new Rectangle(x, y, Width - 6, Height - 6), GraphicsUnit.Pixel);
                graphics.Dispose();
                return bmp;
            }

            return Properties.Resources.question_mark;
        }

        bool mouseOver = false;

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseOver = true;

            if (ShowPreview != null)
                ShowPreview(this, new EventArgs());

            this.Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mouseOver = false;

            if (ShowPreview != null)
                ShowPreview(this, new EventArgs());

            this.Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            frm.ToggleItemHighlight(item.id, !Highlighted);

            base.OnClick(e);
        }

        private static void PaintTransparentBackground(Control c, PaintEventArgs e)
        {
            if (c.Parent == null || !Application.RenderWithVisualStyles)
                return;

            ButtonRenderer.DrawParentBackground(e.Graphics, c.ClientRectangle, c);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintTransparentBackground(this, e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //If Highlighted, draw Background in gold
            if (highlighted)
            {
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(200, 239, 200, 11)))
                using (Pen p2 = new Pen(Color.FromArgb(200, 254, 179, 10)))
                using (Pen p3 = new Pen(Color.FromArgb(200, 255, 196, 21)))
                {
                    e.Graphics.FillRectangle(sb, new Rectangle(new Point(3, 3), imageSize));

                    e.Graphics.DrawRectangle(p2, new Rectangle(new Point(3, 3), imageSize));
                    e.Graphics.DrawRectangle(p3, new Rectangle(new Point(4, 4), new Size(imageSize.Width - 2, imageSize.Height - 2)));
                }
            }

            //Draw Image
            if (Image != null)
            {
                RectangleF sourceRect = new RectangleF(0, 0, imageSize.Width, imageSize.Height);
                RectangleF destRect = new RectangleF(imagePoint.X, imagePoint.Y, imageSize.Width, imageSize.Height);
                e.Graphics.DrawImage(
                                        Image,
                                        destRect,
                                        sourceRect,
                                        GraphicsUnit.Pixel
                                    );
            }

            //Paint number
            if (hideDuplicates && amount > 1)
            {
                Size s = TextRenderer.MeasureText($"x{amount}", new Font("Segoe UI Semibold", 9f));
                using (Pen pe = new Pen(ColorScheme.GetColorFont(!frm.useDarkmode)))
                using (SolidBrush sb = new SolidBrush(ColorScheme.GetColorFont(frm.useDarkmode)))
                {
                    GraphicsPath p = new GraphicsPath();
                    p.AddString(
                        $"x{amount}",                           // text to draw
                        new FontFamily("Segoe UI Semibold"),    // font family
                        (int)FontStyle.Regular,                 // font style
                        e.Graphics.DpiY * 10f / 72f,            // em size
                        new Point(this.Width - (s.Width + 1), this.Height - 20), // location
                        new StringFormat());                    
                    e.Graphics.DrawPath(pe, p);
                    e.Graphics.FillPath(sb, p);
                }
            }

            if (!mouseOver) return;

            Color c = Color.FromArgb(98, 0, 238);
            using (Pen p = new Pen(c))
            {
                if (!frm.useDarkmode)
                {
                    p.Color = Color.FromArgb(100, c.R, c.G, c.B);
                    e.Graphics.DrawLines(p, new Point[] { new Point(2, 2), new Point(this.Width - 3, 2), new Point(this.Width - 3, this.Height - 3), new Point(2, this.Height - 3), new Point(2, 2) });
                    p.Color = Color.FromArgb(75, c.R, c.G, c.B);
                    e.Graphics.DrawLines(p, new Point[] { new Point(1, 1), new Point(this.Width - 2, 1), new Point(this.Width - 2, this.Height - 2), new Point(1, this.Height - 2), new Point(1, 1) });
                    p.Color = Color.FromArgb(25, c.R, c.G, c.B);
                    e.Graphics.DrawLines(p, new Point[] { new Point(0, 0), new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1), new Point(0, this.Height - 1), new Point(0, 0) });
                }
                else
                {
                    c = Color.FromArgb(128, 30, 255);
                    p.Color = Color.FromArgb(250, c.R, c.G, c.B);
                    e.Graphics.DrawLines(p, new Point[] { new Point(2, 2), new Point(this.Width - 3, 2), new Point(this.Width - 3, this.Height - 3), new Point(2, this.Height - 3), new Point(2, 2) });
                    p.Color = Color.FromArgb(200, c.R, c.G, c.B);
                    e.Graphics.DrawLines(p, new Point[] { new Point(1, 1), new Point(this.Width - 2, 1), new Point(this.Width - 2, this.Height - 2), new Point(1, this.Height - 2), new Point(1, 1) });
                    p.Color = Color.FromArgb(75, c.R, c.G, c.B);
                    e.Graphics.DrawLines(p, new Point[] { new Point(0, 0), new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1), new Point(0, this.Height - 1), new Point(0, 0) });
                }
            }
        }

    }
}
