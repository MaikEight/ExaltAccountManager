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
    public partial class FrmCharHost : Form
    {
        UIAccountViewer viewer;
        UICharacterViewer charUI;
        public FrmCharHost(UIAccountViewer _viewer, UICharacterViewer _charUI)
        {
            InitializeComponent();
            viewer = _viewer;
            charUI = _charUI;

            charUI.Visible = false;
            this.Controls.Add(charUI);
            charUI.Location = new Point(0, 0);

            ApplyTheme(viewer.GetDarkmode());

            bunifuTransition.Show(charUI, false);
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
            this.BackColor = second;
        }
    }
}
