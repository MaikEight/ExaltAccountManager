using System.Drawing;
using System.Windows.Forms;

namespace EAM_Statistics
{
    public partial class TopAccountListEntry : UserControl
    {
        bool isSecond = false;
        bool isHeadline = false;
        Color back = Color.White;
        Color backHover = Color.White;
        public TopAccountListEntry(string name, string value, bool _isSecond, bool _isHeadline = false)
        {
            InitializeComponent();

            isSecond = _isSecond;
            isHeadline = _isHeadline;

            lName.Text = name;
            lValue.Text = value;
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
                third = Color.FromArgb(18, 18, 18);
                font = Color.White;
            }

            back = this.BackColor = isSecond ? second : isHeadline ? third : def;

            if (useDarkmode)
            {
                backHover = isSecond ? Color.FromArgb(200, 38, 38, 38) : Color.FromArgb(200, 45, 45, 45);
            }
            else
            {
                backHover = isSecond ? Color.FromArgb(200, 230, 230, 230) : Color.FromArgb(200, 240, 240, 240);
            }

            this.ForeColor = font;            
        }

        private void TopAccountListEntry_MouseEnter(object sender, System.EventArgs e)
        {
            if (isHeadline) return;
            this.BackColor = backHover;
            //this.BackColor = lName.BackColor = lValue.BackColor = backHover;
        }

        private void TopAccountListEntry_MouseLeave(object sender, System.EventArgs e)
        {
            if (isHeadline) return;
            this.BackColor = back;
            //this.BackColor = lName.BackColor = lValue.BackColor = back;
        }
    }
}
