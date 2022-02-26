using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager.UI
{
    public partial class UILogs : UserControl
    {
        FrmMain frm;
        BindingList<LogData> logs = new BindingList<LogData>();

        bool isInit = true;

        public UILogs(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;

            frm.ThemeChanged += ApplyTheme;
            ApplyTheme(frm, null);
        }

        private void UILogs_Load(object sender, EventArgs e)
        {
            dataGridView.MouseWheel += dataGridView_MouseWheel;
            dropExportMode.SelectedIndex = 0;
            LoadLogs();

            pDatagrid.Controls.Add(pLoader);
            pLoader.Dock = DockStyle.Fill;
            pLoader.SendToBack();

            isInit = false;
        }

        private void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            MK_EAM_Lib.FormsUtils.SuspendDrawing(this);

            this.BackColor = def;

            this.ForeColor = font;

            bunifuCards.BackColor = pLoader.BackColor = second;

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

            dropExportMode.BackgroundColor = def;
            dropExportMode.ForeColor = font;
            dropExportMode.ItemBackColor = def;
            dropExportMode.ItemBorderColor = font;
            dropExportMode.ItemForeColor = font;
            dropExportMode.BorderColor = third;
            dropExportMode.Invalidate();

            MK_EAM_Lib.FormsUtils.ResumeDrawing(this);
        }

        private void LoadLogs()
        {
            try
            {
                if (System.IO.File.Exists(frm.pathLogs))
                {
                    logs = new BindingList<LogData>(((List<LogData>)frm.ByteArrayToObject(System.IO.File.ReadAllBytes(frm.pathLogs))).OrderByDescending(l => l.time).ToList());
                    dataGridView.CurrentCell = null;
                    dataGridView.DataBindings.Clear();
                    dataGridView.DataSource = logs;

                    if (!isInit)
                        frm.ShowSnackbar("Log-enries reloaded successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success);
                }
                else
                {
                    frm.ShowSnackbar("No logfile found.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 5000);
                    return;
                }

                if (dataGridView.Rows.Count == 0)
                    return;

                dataGridView.Columns[0].Width = 70;
                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[1].Width = 60;
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[2].Width = 100;
                dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[3].Width = 100;
                dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                UpdateScrollbarValues();
            }
            catch
            {
                frm.ShowSnackbar("Failed to read logfile.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void UpdateScrollbarValues()
        {
            scrollbar.SmallChange = 1;
            scrollbar.LargeChange = 2;
            scrollbar.Value = 0;
            scrollbar.Maximum = 1;
            if (logs.Count > 1)
                scrollbar.Maximum = (logs.Count - 1) / 2;
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogs();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Select the path to save the exported data to.";

                switch (dropExportMode.SelectedIndex)
                {
                    case 0: //To Clipboard
                        {
                            StringBuilder sb = new StringBuilder();
                            List<int> indices = new List<int>();
                            for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                                indices.Add(dataGridView.SelectedRows[i].Index);
                            indices.Sort();

                            for (int i = 0; i < indices.Count; i++)
                                sb.AppendLine($"{dataGridView.Rows[indices[i]].Cells[0].Value} {dataGridView.Rows[indices[i]].Cells[1].Value}\t{dataGridView.Rows[indices[i]].Cells[2].Value}\t{dataGridView.Rows[indices[i]].Cells[3].Value}\t{dataGridView.Rows[indices[i]].Cells[4].Value}");

                            Clipboard.SetText(sb.ToString());
                            frm.ShowSnackbar($"Log-{(indices.Count == 1 ? "entry" : "entries")} copied to clipboard.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success);
                        }
                        break;
                    case 1: //As PNG
                        saveFileDialog.Filter = "PNG (.png)| *.png";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            int firstDisplayedRow = dataGridView.FirstDisplayedScrollingRowIndex;
                            List<Bitmap> bmps = new List<Bitmap>();
                            Size dgvSz = dataGridView.ClientSize;
                            List<int> indices = new List<int>();
                            for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                                indices.Add(dataGridView.SelectedRows[i].Index);
                            indices.Sort();

                            for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                                dataGridView.SelectedRows[i].Selected = false;

                            pLoader.Visible = true;
                            pLoader.BringToFront();

                            for (int i = 0; i < indices.Count; i++)
                            {
                                dataGridView.FirstDisplayedScrollingRowIndex = indices[i];
                                dataGridView.Update();

                                Rectangle RowRect = dataGridView.GetRowDisplayRectangle(indices[i], true);
                                Bitmap bmp = new Bitmap(RowRect.Width, RowRect.Height);
                                using (Bitmap bmpDgv = new Bitmap(dgvSz.Width, dgvSz.Height))
                                {
                                    dataGridView.DrawToBitmap(bmpDgv, new Rectangle(Point.Empty, dgvSz));
                                    using (Graphics G = Graphics.FromImage(bmp))
                                        G.DrawImage(bmpDgv, new Rectangle(Point.Empty,
                                                    RowRect.Size), RowRect, GraphicsUnit.Pixel);
                                    bmps.Add(bmp);
                                }
                            }

                            int mWidth = 0;
                            int mHeight = 0;
                            for (int i = 0; i < bmps.Count; i++)
                            {
                                mWidth = Math.Max(bmps[i].Width, mWidth);
                                mHeight += bmps[i].Height;
                            }
                            Bitmap result = new Bitmap(mWidth, mHeight);
                            Point p = new Point();
                            using (Graphics g = Graphics.FromImage(result))
                            {
                                for (int i = 0; i < bmps.Count; i++)
                                {
                                    g.DrawImage(bmps[i], p);
                                    p.Y += bmps[i].Height;
                                }
                            }
                            result.Save(saveFileDialog.FileName);
                            dataGridView.FirstDisplayedScrollingRowIndex = firstDisplayedRow;

                            frm.ShowSnackbar($"Export selected rows as png.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);

                            if (System.IO.File.Exists(saveFileDialog.FileName))                            
                                System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + saveFileDialog.FileName + "\"");                            
                            else
                                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(saveFileDialog.FileName));

                            pLoader.Visible = false;
                            pLoader.SendToBack();
                        }
                        break;
                    case 2: //As CSV
                        saveFileDialog.Filter = "Comma separated values (.csv)| *.csv";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            List<CSVLogData> export = new List<CSVLogData>();

                            List<int> indices = new List<int>();
                            for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                                indices.Add(dataGridView.SelectedRows[i].Index);
                            indices.Sort();

                            for (int i = 0; i < indices.Count; i++)
                                export.Add(new CSVLogData()
                                {
                                    Date = dataGridView.Rows[indices[i]].Cells[0].Value != null ? dataGridView.Rows[indices[i]].Cells[0].Value.ToString() : string.Empty,
                                    Time = dataGridView.Rows[indices[i]].Cells[1].Value != null ? dataGridView.Rows[indices[i]].Cells[1].Value.ToString() : string.Empty,
                                    Sender = dataGridView.Rows[indices[i]].Cells[2].Value != null ? dataGridView.Rows[indices[i]].Cells[2].Value.ToString() : string.Empty,
                                    EventType = dataGridView.Rows[indices[i]].Cells[3].Value != null ? dataGridView.Rows[indices[i]].Cells[3].Value.ToString() : string.Empty,
                                    Entry = dataGridView.Rows[indices[i]].Cells[4].Value != null ? dataGridView.Rows[indices[i]].Cells[4].Value.ToString() : string.Empty
                                });

                            using (var writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                            using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))                            
                                csv.WriteRecords(export);                            

                            frm.ShowSnackbar($"Export selected rows as .CSV.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);

                            if (System.IO.File.Exists(saveFileDialog.FileName))                            
                                System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + saveFileDialog.FileName + "\"");                            
                            else
                                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(saveFileDialog.FileName));
                        }
                        break;
                    default:
                        break;
                }
                saveFileDialog.Dispose();
            }
            catch
            {
                frm.ShowSnackbar($"Export to {dropExportMode.SelectedItem.ToString()} failed.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
                pLoader.SendToBack();
                pLoader.Visible = false;
            }
        }

        private void dropExportMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (dropExportMode.SelectedIndex)
            {
                case 0:
                    btnSave.Image = Properties.Resources.send_file_white_32px;
                    break;
                case 1:
                    btnSave.Image = Properties.Resources.png_white_32px;
                    break;
                case 2:
                    btnSave.Image = Properties.Resources.export_csv_white_32px;
                    break;
                default:
                    break;
            }
        }
    }
}
