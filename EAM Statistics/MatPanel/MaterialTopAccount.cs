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
    public partial class MaterialTopAccount : UserControl
    {
        bool isDarkmode = false;
        bool isCharacter = false;
        public MaterialTopAccount()
        {
            InitializeComponent();
            dataGridView.MultiSelect = false;
        }

        public MaterialTopAccount(string headline, string title, string[] names, string[] values, bool _isCharacter = false)
        {
            InitializeComponent();
            isCharacter = _isCharacter;
            dataGridView.MultiSelect = false;

            Init(headline, title, names, values, _isCharacter);
        }

        public void Init(string headline, string title, string[] names, string[] values, bool _isCharacter = false)
        {
            isCharacter = _isCharacter;

            lHeadline.Text = headline;
            lTitle.Text = title;

            if (names == null || values == null || ((names.Length < values.Length) ? names.Length : values.Length) <= 0)
            {
                //flow.Visible = false;
                this.Height = lTitle.Bottom + 10;
                return;
            }
            if (!isCharacter)
            {
                dataGridView.Columns[0].HeaderText = names[0];
                dataGridView.Columns[1].HeaderText = values[0];

                for (int i = 1; i < names.Length; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView);
                    row.SetValues(new string[] { names[i], values[i] });
                    dataGridView.Rows.Add(row);
                }
            }
            else
            {
                dataGridView.Columns[0].HeaderText = names[0];
                dataGridView.Columns[0].Width -= 10;
                dataGridView.Columns[1].HeaderText = values[0];

                DataGridViewColumn col = new DataGridViewColumn(dataGridView.Columns[1].CellTemplate);
                col.HeaderText = "X/8";
                col.Width = 10;

                dataGridView.Columns.Insert(1, col);
                for (int i = 1; i < names.Length; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView);
                    string n = names[i].Substring(0, names[i].Length - 3);
                    string x8 = names[i].Substring(names[i].Length - 3, 3);
                    row.SetValues(new string[] { n, x8, values[i] });
                    dataGridView.Rows.Add(row);
                }
            }
            //flow loc 4; 60 Size: 192; 105 Anchor: Top, Left, Right
            //bool isSecond = false;
            //TopAccountListEntry headEntry = new TopAccountListEntry(names[0], values[0], false, true);
            //headEntry.Width = flow.Width;
            //flow.Controls.Add(headEntry);
            //flow.SetFlowBreak(headEntry, true);
            //int entries = 1;
            //for (int i = 1; i < names.Length; i++)
            //{
            //    TopAccountListEntry entry = new TopAccountListEntry(names[i], values[i], isSecond);
            //    entry.Width = flow.Width;
            //    flow.Controls.Add(entry);
            //    flow.SetFlowBreak(entry, true);
            //    isSecond = !isSecond;
            //    entries++;
            //}

            //flow.Height = entries * (headEntry.Height + flow.Margin.Top + flow.Margin.Bottom);
            int h = 0;
            for (int i = 0; i < dataGridView.Rows.Count; i++)
                h += dataGridView.Rows[i].Height;

            dataGridView.Height = dataGridView.ColumnHeadersHeight + h;
            this.Height = dataGridView.Bottom + 10;

            ApplyTheme(isDarkmode);
        }

        public void ApplyTheme(bool useDarkmode)
        {
            isDarkmode = useDarkmode;

            Color def = Color.FromArgb(253, 253, 253);
            Color second = Color.FromArgb(250, 250, 250);
            Color third = Color.FromArgb(230, 230, 230);
            Color font = Color.Black;

            if (useDarkmode)
            {
                def = Color.FromArgb(30, 30, 30);
                second = Color.FromArgb(23, 23, 23);
                third = Color.FromArgb(0, 0, 0);
                font = Color.White;
            }

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            try
            {
                pMain.PanelColor = pMain.PanelColor2 = def;
                pMain.ShadowColor = useDarkmode ? Color.FromArgb(45, 20, 20, 20) : Color.FromArgb(25, 0, 0, 0);
                this.ForeColor = font;

                dataGridView.BackgroundColor = def;

                dataGridView.GridColor = font;
                dataGridView.ForeColor = font;
                dataGridView.CurrentTheme.BackColor = def;

                dataGridView.CurrentTheme.HeaderStyle.BackColor = third;
                dataGridView.CurrentTheme.HeaderStyle.SelectionBackColor = third;
                dataGridView.CurrentTheme.HeaderStyle.ForeColor = font;
                dataGridView.CurrentTheme.HeaderStyle.SelectionForeColor = font;

                dataGridView.CurrentTheme.AlternatingRowsStyle.BackColor = second;
                dataGridView.CurrentTheme.AlternatingRowsStyle.SelectionBackColor = third;
                dataGridView.CurrentTheme.AlternatingRowsStyle.ForeColor = font;
                dataGridView.CurrentTheme.AlternatingRowsStyle.SelectionForeColor = font;

                dataGridView.CurrentTheme.RowsStyle.BackColor = def;
                dataGridView.CurrentTheme.RowsStyle.SelectionBackColor = third;
                dataGridView.CurrentTheme.RowsStyle.ForeColor = font;
                dataGridView.CurrentTheme.RowsStyle.SelectionForeColor = font;
                dataGridView.ApplyTheme(dataGridView.CurrentTheme);

                for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                    dataGridView.SelectedRows[i].Selected = false;
            }
            catch { }

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        public event DataGridViewCellMouseEventHandler RowClicked;
        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {            
            if (this.RowClicked != null)
                this.RowClicked(this, e);
        }
    }
}
