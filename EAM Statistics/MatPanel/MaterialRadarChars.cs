using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MKRadarChart;

namespace EAM_Statistics
{
    public partial class MaterialRadarChars : UserControl
    {
        public MaterialRadarChars()
        {
            InitializeComponent();

            Init();
        }

        public MaterialRadarChars(string headline, string title)
        {
            InitializeComponent();

            Init();
            SetLabels(headline, title);            
        }

        public void Init()
        {
            //radarChart = new RadarChart();
            //pMain.Controls.Add(radarChart);
            int s = pChart.Height < pChart.Width ? pChart.Height : pChart.Width;
            radarChart.Size = new Size(s, s);
            radarChart.Location = new Point(28, 0);
            //radarChart.Location = new Point((((this.Width - 56) - radarChart.Width) / 2) + pMain.ShadowDepth, 25);
            //radarChart.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom);
            radarChart.SetHeadline("");
            radarChart.ChangeShowValueBallsDirectly(false);
            radarChart.maxDistanceToPoint = 75d;
            pChart.Controls.SetChildIndex(lTitle, 0);
        }

        public void SetLabels(string headline, string title)
        {
            lHeadline.Text = headline;
            lTitle.Text = title;
        }

        public void SetTitle(string title)
        {
            lTitle.Text = title;
        }

        public void DrawValues(int[] _values, Image[] _imgs, int pictureSize = 10, string[] names = null, bool _showValuesInToolTip = true)
        {
            radarChart.DrawValues(_values, _imgs, pictureSize, names, _showValuesInToolTip);
            radarChart.Location = new Point(((pChart.Width - radarChart.Width) / 2) + 12, 0);
        }

        public void ApplyTheme(bool useDarkmode)
        {
            Color def = Color.FromArgb(253, 253, 253);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (useDarkmode)
            {
                def = Color.FromArgb(30, 30, 30);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            pMain.PanelColor = pMain.PanelColor2 = def;
            pMain.ShadowColor = useDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            radarChart.ChangeTheme(def, second, font);
            radarChart.ChangeGridColor(useDarkmode ? Color.FromArgb(128, 200, 200, 200) : Color.FromArgb(128, 0, 0, 0));

            this.ForeColor = font;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }
    }
}
