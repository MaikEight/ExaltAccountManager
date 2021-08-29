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
    public partial class FrmImportAskPass : Form
    {
        public string password = string.Empty;

        public FrmImportAskPass()
        {
            InitializeComponent();
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.BackColor = def;
                this.ForeColor = font;

                tbImportPassword.BackColor = tbImportPassword.OnIdleState.FillColor = tbImportPassword.OnActiveState.FillColor = tbImportPassword.OnDisabledState.FillColor = tbImportPassword.OnHoverState.FillColor = def;
                tbImportPassword.OnIdleState.BorderColor = font;
                tbImportPassword.Update();

                p.Color = font;
            }
        }

        Pen p = new Pen(Color.Black);
        private void FrmImportAskPass_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(p, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        private void btnImportOK_Click(object sender, EventArgs e)
        {
            password = tbImportPassword.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void tbImportPassword_TextChanged(object sender, EventArgs e)
        {
            btnImportOK.Enabled = tbImportPassword.TextLength > 3;
        }

        private void lPass_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLines(p, new PointF[] { new PointF(0, 0), new PointF(0, lPass.Height), new PointF(0, 0), new PointF(lPass.Width, 0), new PointF(lPass.Width - 1, 0), new PointF(lPass.Width -1, lPass.Height) });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
