using MK_EAM_Lib;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleDailyLoginsResultsList : UserControl
    {
        private FrmMain frm;
        private bool isInit = false;

        private BindingSource bindingSource = new BindingSource();
        private BindingList<DailyData> bindingListResults = new BindingList<DailyData>();
        private BindingList<DailyAccountData> bindingListDetails = new BindingList<DailyAccountData>();

        public EleDailyLoginsResultsList(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            dataGridView.MouseWheel += dataGridView_MouseWheel;

            ApplyTheme(frm, null);

            if (File.Exists(frm.dailyLoginsPath))
            {
                using (StreamReader file = File.OpenText(frm.dailyLoginsPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    bindingListResults = new BindingList<DailyData>(((DailyLogins)serializer.Deserialize(file, typeof(DailyLogins))).DailyDatas.OrderByDescending(d => d.Date).ToList());
                }
            }           

            LoadUIResults();            
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            this.BackColor = def;
            this.ForeColor = font;

            pbClose.BackColor = frm.UseDarkmode ? second : third;
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;

            shadow.PanelColor = shadow.BackColor = shadow.PanelColor2 = def;
            shadow.BorderColor = shadow.BorderColor = second;
            shadow.ShadowColor = shadow.ShadowColor = frm.UseDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);

            scrollbar.BorderColor = frm.UseDarkmode ? second : Color.Silver;
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

        bool showDetails = false;
        private void LoadUIResults()
        {
            showDetails = false;
            dataGridView.Columns.Clear();
            dataGridView.DataBindings.Clear();
            bindingSource.DataSource = bindingListResults;
            dataGridView.DataSource = bindingSource;

            lHint.Visible = true;
        }

        private void LoadUIDetails(int index)
        {
            showDetails = true;
            bindingListDetails = new BindingList<DailyAccountData>(bindingListResults[index].AccountData);

            dataGridView.Columns.Clear();
            dataGridView.DataBindings.Clear();
            bindingSource.DataSource = bindingListDetails;
            dataGridView.DataSource = bindingSource;

            lHint.Visible = false;
            btnBack.Visible = true;
        }

        public void UpdateScrollbarValue()
        {
            scrollbar.SmallChange = 1;
            scrollbar.LargeChange = 2;
            scrollbar.Value = 0;

            int max = 1;
            if (dataGridView.Rows.Count > 1)
                max = (dataGridView.Rows.Count - dataGridView.DisplayedRowCount(false)) - 1;
            scrollbar.Maximum = max < 1 ? 1 : max;
        }

        private void scrollbar_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            if (isInit) return;

            dataGridView.FirstDisplayedScrollingRowIndex = scrollbar.Value;
            dataGridView.Update();
        }

        private void dataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;

            int movement = e.Delta / 120;
            if (dataGridView.FirstDisplayedScrollingRowIndex - movement >= 0 && dataGridView.FirstDisplayedScrollingRowIndex - movement < (dataGridView.Rows.Count - dataGridView.DisplayedRowCount(false)))
                scrollbar.Value = dataGridView.FirstDisplayedScrollingRowIndex -= movement;
        }

        #region Button Close

        private void pbClose_Click(object sender, EventArgs e) => frm.RemoveShadowForm();

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Crimson;
            pbClose.Image = Properties.Resources.ic_close_white_18dp;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = frm.UseDarkmode ? ColorScheme.GetColorSecond(frm.UseDarkmode) : ColorScheme.GetColorThird(frm.UseDarkmode);
            pbClose.Image = frm.UseDarkmode ? Properties.Resources.ic_close_white_18dp : Properties.Resources.ic_close_black_18dp;
        }

        private void pbClose_MouseDown(object sender, MouseEventArgs e) => pbClose.BackColor = Color.Red;

        #endregion

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateScrollbarValue();
            if (dataGridView.Rows.Count == 0)
                return;
            if (!showDetails)
            {
                dataGridView.Columns[0].DefaultCellStyle.Format = $"dd.MM.yyyy a't' HH:mm";
                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
            else
            {
                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView.Columns[1].CellTemplate.Style.Padding = new Padding(3, 0, 0, 0);
                dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView.Columns[2].CellTemplate.Style.Padding = new Padding(3, 0, 0, 0);
                dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView.Columns[3].CellTemplate.Style.Padding = new Padding(3, 0, 0, 0);
                dataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            lHint.Visible =
            btnBack.Visible = false;

            LoadUIResults();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (showDetails)
                return;
            if (dataGridView.SelectedRows.Count > 0)
                LoadUIDetails(dataGridView.SelectedRows[0].Index);
        }
    }
}
