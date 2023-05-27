using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public sealed partial class FrmControlHost : Form
    {
        private FrmMain frm;
        private Control ctrToHost = null;

        public FrmControlHost(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);
            this.Disposed += OnDisposed;
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            this.BackColor = ColorScheme.GetColorDef(frm.UseDarkmode);

            this.ForeColor = ColorScheme.GetColorFont(frm.UseDarkmode);
        }

        public void ShowControl(Control ctr)
        {
            if (ctrToHost != null)
                RemoveControl();

            ctrToHost = ctr;
            this.Size = new Size(ctrToHost.Size.Width + 9, ctrToHost.Size.Height + 9);
            this.Controls.Add(ctrToHost);
            ctrToHost.Dock = DockStyle.None;
            ctrToHost.Location = new Point(4, 4);
            ctrToHost.SizeChanged += CtrToHost_SizeChanged;
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            if (ctrToHost != null)
                ctrToHost.SizeChanged -= CtrToHost_SizeChanged;
        }

        private void CtrToHost_SizeChanged(object sender, EventArgs e)
        {
            ctrToHost.Location = new Point(4, 4);
            this.Size = new Size(ctrToHost.Size.Width + 9, ctrToHost.Size.Height + 9);
        }

        public void RemoveControl()
        {
            if (ctrToHost != null)
            {
                this.Controls.Remove(ctrToHost);
                ctrToHost.SizeChanged -= CtrToHost_SizeChanged;
                ctrToHost = null;
            }
        }
    }
}
