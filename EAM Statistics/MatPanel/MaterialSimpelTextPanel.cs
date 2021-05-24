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
    public partial class MaterialSimpelTextPanel : UserControl
    {
        public MaterialSimpelTextPanel()
        {
            InitializeComponent();
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
            this.ForeColor = font;

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        public void ChangeHeadline(string _headline) => lHeadline.Text = _headline;
        public void ChangeTitle(string _title, bool isSingleLine = false)
        {
            lTitle.Text = _title;
            if (isSingleLine)
            {
                lTitle.MaximumSize = new Size(0, 30);
                this.Width = lTitle.Right + 10;
            }
                this.Height = lTitle.Bottom + 15;
        }
        public void Init(string _headline, string _title, bool isSingleLine = false)
        {
            ChangeHeadline(_headline);
            ChangeTitle(_title, isSingleLine);
        }
    }
}
