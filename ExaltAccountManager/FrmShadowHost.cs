using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public sealed partial class FrmShadowHost : Form
    {
        private FrmMain frm;
        private FrmControlHost frmHost;

        public FrmShadowHost(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);

            frmHost = new FrmControlHost(frm);
            frmHost.Owner = this;
            frmHost.TopLevel = true;
            frmHost.SizeChanged += FrmHost_SizeChanged;
            frmHost.Disposed += FrmHost_Disposed;
        }

        private void FrmHost_Disposed(object sender, EventArgs e)
        {
            frmHost.SizeChanged -= FrmHost_SizeChanged;
        }

        private void FrmHost_SizeChanged(object sender, EventArgs e)
        {
            frmHost.Location = new Point(this.Left + (((this.Width - 175) - frmHost.Width) / 2) + 175, this.Top + ((this.Height - frmHost.Height) / 2));
        }

        private void ApplyTheme(object sender, EventArgs e)
        {            
            this.BackColor = frm.UseDarkmode ? Color.FromArgb(48, 48, 48) : Color.Gray;            
        }

        public void ShowControl(Control ctr)
        {
            frmHost.ShowControl(ctr);
            frmHost.Location = new Point(this.Left + (((this.Width - 175) - frmHost.Width) / 2) + 175, this.Top + ((this.Height - frmHost.Height) / 2));
            frmHost.Show();
        }

        public DialogResult ShowFormDialog(Form form)
        {
            return form.ShowDialog(this);
        }

        public void RemoveControl()
        {
            frmHost.RemoveControl();
            frmHost.Hide();
        }

        private void FrmShadowHost_SizeChanged(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddPolygon(new PointF[]
            {
                new PointF(0, 40),
                new PointF(175, 40),
                new PointF(175, 0),
                new PointF(this.Width, 0),
                new PointF(this.Width, this.Height),
                new PointF(0, this.Height),
                new PointF(0, 40),
            });
            this.Region = new Region(gp);

            frmHost.Location = new Point(this.Left + (((this.Width - 175)- frmHost.Width) / 2) + 175, this.Top + ((this.Height - frmHost.Height) / 2));
        }

        private void FrmShadowHost_Click(object sender, EventArgs e)
        {
            frm.RemoveShadowForm();
        }
    }
}
