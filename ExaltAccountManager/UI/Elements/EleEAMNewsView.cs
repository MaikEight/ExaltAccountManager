using MK_EAM_General_Services_Lib.News.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public partial class EleEAMNewsView : UserControl
    {
        public NewsData NewsData { get; internal set; }
        private FrmMain frm;
        private List<Control> controls = new List<Control>();

        public EleEAMNewsView(FrmMain _frm, NewsData newsData)
        {
            InitializeComponent();
            NewsData = newsData;
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (s, e) => frm.ThemeChanged -= ApplyTheme;
            ApplyTheme(frm, null);
            LoadUI();
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = second;
            this.ForeColor = font;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        public void LoadUI()
        {
            if (NewsData == null)
                return;

            lTitle.Text = NewsData.Title;
            lDate.Text = NewsData.Date.ToString("dd.MM.yyyy");

            Point p = new Point(10, spacer.Bottom + 10);

            NewsData.newsEntries = NewsData.newsEntries.OrderBy(e => e.OrderId).ToList();

            foreach (NewsEntry entry in NewsData.newsEntries)
            {
                switch (entry.Type)
                {
                    case NewsType.Text:
                        {
                            TextUIData data = (TextUIData)entry.UiData;
                            Label lbl = new Label()
                            {
                                MaximumSize = new Size(this.Width - 20, 10000),
                            };
                            lbl.Text = data.Text;
                            controls.Add(lbl);
                            this.Controls.Add(lbl);
                            lbl.Location = p;
                            p.Y = lbl.Bottom + 5;
                        }
                        break;
                    case NewsType.Headline:
                        {
                            TextUIData data = (TextUIData)entry.UiData;
                            Label lbl = new Label()
                            {
                                MaximumSize = new Size(this.Width - 20, 1000),
                                Font = new Font("Segoe UI", 12f, FontStyle.Bold)
                            };
                            lbl.Text = data.Text;
                            controls.Add(lbl);
                            this.Controls.Add(lbl);
                            lbl.Location = p;
                            p.Y = lbl.Bottom + 10;
                        }
                        break;
                    case NewsType.Image:
                        {
                            ImageUIData data = (ImageUIData)entry.UiData;

                            Size max = new Size(this.Width - 20, 0);
                            max.Width = data.MaxSize.Width == 0 ? max.Width : data.MaxSize.Width;
                            max.Height = data.MaxSize.Height == 0 ? max.Height : data.MaxSize.Height;

                            Size min = new Size(0, 0);
                            min.Width = data.MinSize.Width == 0 ? min.Width : data.MinSize.Width;
                            min.Height = data.MinSize.Height == 0 ? min.Height : data.MinSize.Height;

                            PictureBox pb = new PictureBox()
                            {
                                MaximumSize = max,
                                MinimumSize = min,
                                SizeMode = (PictureBoxSizeMode)data.PictureBoxSizeMode
                            };
                            pb.Width = this.Width - 20;
                            pb.Load(data.ImageUrl);
                            pb.Height = pb.Image.Width > this.Width - 20 ?(int)((double)pb.Image.Height / ((double)pb.Image.Width / ((double)this.Width - 20d)) ): 0;
                            controls.Add(pb);
                            this.Controls.Add(pb);
                            pb.Location = p;
                            p.Y = pb.Bottom + 5;
                        }
                        break;
                    case NewsType.Poll:
                        {
                            PollUIData data = (PollUIData)entry.UiData;

                        }
                        break;
                    default:
                        break;
                }
            }

            this.Height = p.Y + 10;
        }
    }
}
