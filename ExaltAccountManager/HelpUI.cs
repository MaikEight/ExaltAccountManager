using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class HelpUI : UserControl
    {
        Image openArrow = Properties.Resources.ic_arrow_drop_down_black_36dp;
        Image closeArrow = Properties.Resources.ic_arrow_drop_up_black_36dp;
        private bool useDarkmode = false;
        bool isSecond = false;
        public HelpUI()
        {
            InitializeComponent();
        }

        public HelpUI(string question, string answer, bool _isSecond = false)
        {
            InitializeComponent();
            isSecond = _isSecond;

            LoadUI(question, answer);
        }

        Pen p = new Pen(Color.Black);
        public void ApplyTheme(bool isDarkmode)
        {
            useDarkmode = isDarkmode;
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.ForeColor = font;
                this.BackColor =
                pbOpen.BackColor = isSecond ? def : second;

                openArrow = Properties.Resources.ic_arrow_drop_down_white_36dp;
                closeArrow = Properties.Resources.ic_arrow_drop_up_white_36dp;
                
                pbOpen.Image = pbOpen.Image == Properties.Resources.ic_arrow_drop_down_black_36dp ? closeArrow : openArrow;

                p = new Pen(Color.White);
            }
            else if (isSecond) this.BackColor = Color.White;
        }

        private void LoadUI(string question, string answer)
        {
            lQuestion.Text = question;
            lAnswer.Text = answer;

            openHeight = this.Height = pAnswer.Top + lAnswer.Bottom + 5;

            OpenUI();
        }

        int openHeight = 36;

        bool isOpen = true;
        private void OpenUI()
        {
            if (isOpen)
            {
                this.Height = pQuestion.Height;
            }
            else
            {
                this.Height = openHeight;
            }
            isOpen = !isOpen;
            pbOpen.Image = isOpen ? closeArrow : openArrow;
        }

        private void pAnswer_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);

            e.Graphics.DrawLine(p, topRight, topLeft);
        }

        private void pbOpen_Click(object sender, EventArgs e)
        {
            OpenUI();
        }

        private void pbOpen_MouseEnter(object sender, EventArgs e)
        {
            pbOpen.BackColor = useDarkmode ? Color.FromArgb(225, 50, 50, 50) : Color.FromArgb(75, 175, 175, 175);
        }

        private void pbOpen_MouseLeave(object sender, EventArgs e)
        {
            pbOpen.BackColor = pQuestion.BackColor;
        }
    }
}
