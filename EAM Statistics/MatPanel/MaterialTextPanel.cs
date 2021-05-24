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
    public partial class MaterialTextPanel : UserControl
    {
        public MaterialTextPanel()
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
            if (useDarkmode)
            {
                lHeadline.ForeColor = Color.Gainsboro;
                lTitle.ForeColor = Color.WhiteSmoke;
                this.ForeColor = Color.FromArgb(200,200,200);
            }
            else
            {
                lHeadline.ForeColor = Color.DimGray;
                lTitle.ForeColor = Color.FromArgb(25,25,25);
                this.ForeColor = Color.FromArgb(75, 75, 75);
            }

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        public void ChangeHeadline(string _headline) => lHeadline.Text = _headline;
        public void ChangeTitle(string _title) => lTitle.Text = _title;
        public void ChangeText(string _text) => lText.Text = _text;

        public void Init(string _headline, string _title, string _text)
        {
            ChangeHeadline(_headline);
            ChangeTitle(_title);
            ChangeText(_text);
        }
    }
}
