using ExaltAccountManager.UI.Elements;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public sealed partial class UITokenViewer : UserControl
    {
        private FrmMain frm;
        private EleTokenViewer eleTokenViewer;

        public UITokenViewer(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            LoadDataGridView();
            dataGridView.MouseWheel += dataGridView_MouseWheel;

            eleTokenViewer = new EleTokenViewer(frm);

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);
        }

        public void LoadDataGridView()
        {
            dataGridView.DataBindings.Clear();
            
            dataGridView.DataSource = frm.accounts;
            dataGridView.Columns[0].Width = 100;
            dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void UIChangelog_Load(object sender, EventArgs e)
        {
            scrollbar.SmallChange = 1;
            scrollbar.LargeChange = 2;
            scrollbar.Value = 0;
            if (dataGridView.Rows.Count > 1)
                scrollbar.Maximum = (dataGridView.Rows.Count - 1) / 2;
            else
                scrollbar.Maximum = 1;

            if (dataGridView.Rows.Count > 0)
            {
                dataGridView.Columns[0].Width = 60;
                dataGridView.Columns[0].DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding() { Left = 20, Top = 0, Right = 0, Bottom = 0 } };
            }
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;

            this.ForeColor = font;

            bunifuCards.BackColor = second;

            scrollbar.BorderColor = frm.UseDarkmode ? third : Color.Silver;
            scrollbar.BackgroundColor = frm.UseDarkmode ? def : third;
            scrollbar.ThumbColor = frm.UseDarkmode ? third : Color.Gray;

            dataGridView.BackgroundColor = second;
            dataGridView.CurrentTheme.BackColor = frm.UseDarkmode ? Color.FromArgb(77, 10, 173) : Color.FromArgb(107, 40, 203);
            dataGridView.CurrentTheme.GridColor = dataGridView.GridColor = frm.UseDarkmode ? third : Color.WhiteSmoke;

            dataGridView.CurrentTheme.HeaderStyle.BackColor = dataGridView.CurrentTheme.HeaderStyle.SelectionBackColor = dataGridView.HeaderBackColor = frm.UseDarkmode ? Color.FromArgb(77, 10, 173) : Color.FromArgb(107, 40, 203);

            dataGridView.CurrentTheme.RowsStyle.BackColor = frm.UseDarkmode ? Color.FromArgb(126, 65, 214) : Color.FromArgb(176, 127, 246);//78, 12, 174
            dataGridView.CurrentTheme.AlternatingRowsStyle.BackColor = frm.UseDarkmode ? Color.FromArgb(106, 45, 194) : Color.FromArgb(156, 95, 244);

            dataGridView.ApplyTheme(dataGridView.CurrentTheme);
        }

        private void scrollbar_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            dataGridView.FirstDisplayedScrollingRowIndex = scrollbar.Value;
        }

        private void dataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;
            int movement = e.Delta / 120;
            if (dataGridView.FirstDisplayedScrollingRowIndex - movement >= 0 && dataGridView.FirstDisplayedScrollingRowIndex - movement < dataGridView.Rows.Count)
                scrollbar.Value = dataGridView.FirstDisplayedScrollingRowIndex -= movement;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                eleTokenViewer.AccountInfo = frm.accounts[e.RowIndex];
                frm.ShowShadowForm(eleTokenViewer);
            }
            catch { }
        }
    }    
}
