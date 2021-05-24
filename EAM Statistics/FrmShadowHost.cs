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
    public partial class FrmShadowHost : Form
    {
        UIAccountViewer viewer;
        public FrmShadowHost(UIAccountViewer _viewer)
        {
            InitializeComponent();
            viewer = _viewer;

            ApplyTheme(viewer.GetDarkmode());            
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                this.BackColor = Color.FromArgb(48, 48, 48);
            }
            else
            {
                this.BackColor = Color.LightGray;
            }
        }

        private void FrmShadowHost_Click(object sender, EventArgs e)
        {
            viewer.CloseCharacterForms();
        }
    }
}
