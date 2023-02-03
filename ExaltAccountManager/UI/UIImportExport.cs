using ExaltAccountManager.UI.Elements;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public sealed partial class UIImportExport : UserControl
    {
        private EleImport eleImport = null;
        private EleExport eleExport = null;

        private FrmMain frm;

        public UIImportExport(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);

            btnTabImport.PerformClick();
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;
        }

        private void UIImportExport_Load(object sender, EventArgs e)
        {
            btnTabImport.Focus();
        }

        private void btnTabImport_Click(object sender, EventArgs e)
        {
            if (eleImport == null)
                eleImport = new EleImport(frm) { Dock = DockStyle.Fill };

            this.pContent.Controls.Clear();
            this.pContent.Controls.Add(eleImport);
            eleImport.BringToFront();
        }

        private void btnTabExport_Click(object sender, EventArgs e)
        {
            if(eleExport == null)
                eleExport = new EleExport(frm, this) { Dock = DockStyle.Fill };

            this.pContent.Controls.Clear();
            this.pContent.Controls.Add(eleExport);
            eleExport.BringToFront();
        }
    }
}
