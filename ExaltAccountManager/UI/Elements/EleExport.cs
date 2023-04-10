using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleExport : UserControl
    {
        private FrmMain frm;
        private UIImportExport uiImportExport;

        private EleCustomExport eleCustomExport;

        private BindingSource bindingSource = new BindingSource();

        private string fileName = "Export";

        private bool isInit = true;
        /*
            Encrypted EAM-Save file
            EAM-Save file
            CSV-Formated with passwords
            CSV-Formated without passwords
            Custom formated file
         */
        private readonly string[] descriptions = new string[]
        {
            "A custom format, that is AES-128 encrypted using your own password. It includes all selected accounts and statistics data.",
            "A custom format, that is AES-128 encrypted using a default IV & Key. It includes all selected accounts and statistics data.",
            "A Comma-Seperated-Value list of all selected accounts, including accountname, password and group.",
            "A Comma-Seperated-Value list of all selected accounts, including accountname and group.",
            "A list, formated to your likings, containing all selected accounts."
        };

        public EleExport(FrmMain _frm, UIImportExport _uiImportExport)
        {
            InitializeComponent();
            frm = _frm;
            uiImportExport = _uiImportExport;

            dataGridView.MouseWheel += dataGridView_MouseWheel;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);

            AddAccountsToDatagrid();

            tbPath.Text = System.IO.Path.Combine(Application.StartupPath, $"{fileName}.EAMexport");
            dropExport.SelectedIndex = 0;
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            tbPassword.ResetColors();
            tbPath.ResetColors();

            this.BackColor =
            pExportFinisher.BackColor = lChoose.BackColor = lExportAs.BackColor = lDescription.BackColor = pExport.BackgroundColor = lPW.BackColor =
            tbPassword.BackColor = tbPassword.FillColor = tbPassword.OnIdleState.FillColor = tbPassword.OnHoverState.FillColor = tbPassword.OnActiveState.FillColor = tbPassword.OnDisabledState.FillColor =
            tbPath.BackColor = tbPath.FillColor = tbPath.OnIdleState.FillColor = tbPath.OnHoverState.FillColor = tbPath.OnActiveState.FillColor = tbPath.OnDisabledState.FillColor =
            def;
            this.ForeColor =
            tbPassword.ForeColor = tbPassword.OnActiveState.ForeColor = tbPassword.OnDisabledState.ForeColor = tbPassword.OnHoverState.ForeColor = tbPassword.OnIdleState.ForeColor =
            tbPath.ForeColor = tbPath.OnActiveState.ForeColor = tbPath.OnDisabledState.ForeColor = tbPath.OnHoverState.ForeColor = tbPath.OnIdleState.ForeColor =
            font;

            bunifuCards.BackColor = pExport.BackColor = second;

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

            dropExport.BackgroundColor =
            dropExport.ItemBackColor = def;
            dropExport.ForeColor =
            dropExport.ItemBorderColor =
            dropExport.ItemForeColor = font;
            dropExport.BorderColor = third;

            toolTip.BackColor = def;
            toolTip.TitleForeColor = font;
            toolTip.TextForeColor = frm.UseDarkmode ? Color.WhiteSmoke : Color.FromArgb(64, 64, 64);

            this.Invalidate();
        }

        private void dropExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            lDescription.Text = descriptions[dropExport.SelectedIndex];

            switch (dropExport.SelectedIndex)
            {
                case 0:
                    tbPassword.Clear();
                    lPW.Visible = tbPassword.Visible = btnExport.Visible = true;
                    btnExport.Enabled = false;
                    btnShowCustomizer.Visible = false;
                    break;
                case 1:
                    lPW.Visible = tbPassword.Visible =
                    btnShowCustomizer.Visible = false;
                    btnExport.Visible =
                    btnExport.Enabled = true;
                    break;
                case 2:
                    lPW.Visible = tbPassword.Visible =
                    btnShowCustomizer.Visible = false;
                    btnExport.Visible =
                    btnExport.Enabled = true;
                    break;
                case 3:
                    lPW.Visible = tbPassword.Visible =
                    btnShowCustomizer.Visible = false;
                    btnExport.Visible =
                    btnExport.Enabled = true;
                    break;
                case 4:
                    lPW.Visible = tbPassword.Visible =
                    btnExport.Visible = false;
                    btnShowCustomizer.Visible =
                    btnShowCustomizer.Enabled = true;
                    break;
                default:
                    break;
            }
            try
            {
                tbPath.Text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(tbPath.Text), $"{fileName}{GetFileExtension()}");
            }
            catch { tbPath.Text = System.IO.Path.Combine(Application.StartupPath, $"{fileName}.EAMexport"); }
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (isInit || dropExport.SelectedIndex == 4)
                return;

            List<string> emails = new List<string>();
            for (int i = 0; i < dataGridView.Rows.Count; i++)
                if ((bool)dataGridView.Rows[i].Cells[0].Value)
                    emails.Add(dataGridView.Rows[i].Cells[3].Value.ToString());

            List<MK_EAM_Lib.AccountInfo> accounts = frm.accounts.Where(a => emails.Contains(a.Email)).ToList();

            switch (dropExport.SelectedIndex)
            {
                case 0: //Encrypted EAM-Save file
                    try
                    {
                        ExportSaveFile saveFile = new ExportSaveFile()
                        {
                            version = 1,
                            exportType = ExportType.AccountsWithPassword
                        };
                        ExportAccounts accs = new ExportAccounts()
                        {
                            accounts = accounts,
                            statsFileName = new List<string>(),
                            statsFileData = new List<byte[]>()
                        };

                        for (int i = 0; i < accs.accounts.Count; i++)
                            accs.accounts[i].accessToken = null;

                        foreach (string f in Directory.GetFiles(frm.accountStatsPath))
                        {
                            accs.statsFileName.Add(Path.GetFileName(f));
                            accs.statsFileData.Add(File.ReadAllBytes(f));
                        }

                        AesCryptographyService aes = new AesCryptographyService();
                        System.Tuple<byte[], byte[]> keyIV = aes.GetKeyAndIV(tbPassword.Text);
                        byte[] enc = aes.Encrypt(frm.ObjectToByteArray(accs), keyIV.Item1, keyIV.Item2);
                        saveFile.data = enc;

                        File.WriteAllBytes(tbPath.Text, frm.ObjectToByteArray(saveFile));

                        frm.ShowSnackbar("Successfully exported selected accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                    }
                    catch { frm.ShowSnackbar("Failed to export accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000); }
                    break;
                case 1: //EAM-Save file
                    try
                    {
                        ExportSaveFile saveFile = new ExportSaveFile()
                        {
                            version = 1,
                            exportType = ExportType.AccountsNoPassword
                        };
                        ExportAccounts accs = new ExportAccounts()
                        {
                            accounts = accounts,
                            statsFileName = new List<string>(),
                            statsFileData = new List<byte[]>()
                        };

                        for (int i = 0; i < accs.accounts.Count; i++)
                            accs.accounts[i].accessToken = null;

                        foreach (string f in Directory.GetFiles(frm.accountStatsPath))
                        {
                            accs.statsFileName.Add(Path.GetFileName(f));
                            accs.statsFileData.Add(File.ReadAllBytes(f));
                        }

                        AesCryptographyService aes = new AesCryptographyService();
                        byte[] enc = aes.Encrypt(frm.ObjectToByteArray(accs));
                        saveFile.data = enc;

                        File.WriteAllBytes(tbPath.Text, frm.ObjectToByteArray(saveFile));

                        frm.ShowSnackbar("Successfully exported selected accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                    }
                    catch { frm.ShowSnackbar("Failed to export accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000); }
                    break;
                case 2: //CSV-Formated with passwords
                    try
                    {
                        List<ExportCSVAccount> export = new List<ExportCSVAccount>();

                        for (int i = 0; i < accounts.Count; i++)
                            export.Add(new ExportCSVAccount(frm.accounts[i], true));

                        using (var writer = new StreamWriter(tbPath.Text))
                        using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            csv.Context.RegisterClassMap<ExportCSVAccountMap>();
                            csv.WriteRecords(export);
                        }

                        frm.ShowSnackbar("Successfully exported selected accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                    }
                    catch { frm.ShowSnackbar("Failed to export accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000); }
                    break;
                case 3: //CSV-Formated without passwords
                    try
                    {
                        List<ExportCSVAccount> export = new List<ExportCSVAccount>();

                        for (int i = 0; i < accounts.Count; i++)
                            export.Add(new ExportCSVAccount(accounts[i]));

                        using (var writer = new StreamWriter(tbPath.Text))
                        using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            csv.Context.RegisterClassMap<ExportCSVAccountMap>();
                            csv.WriteRecords(export);
                        }

                        frm.ShowSnackbar("Successfully exported selected accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                    }
                    catch { frm.ShowSnackbar("Failed to export accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000); }
                    break;
                default:
                    break;
            }
        }

        private void btnShowCustomizer_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> emails = new List<string>();
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                    if ((bool)dataGridView.Rows[i].Cells[0].Value)
                        emails.Add(dataGridView.Rows[i].Cells[3].Value.ToString());

                List<MK_EAM_Lib.AccountInfo> accs = frm.accounts.Where(a => emails.Contains(a.Email)).ToList();

                eleCustomExport = new EleCustomExport(frm, this, accs);
                bunifuCards.Controls.Add(eleCustomExport);
                eleCustomExport.Location = new System.Drawing.Point(20, 58);
                eleCustomExport.BringToFront();
            }
            catch { }
        }

        public void CloseEleCustomImport()
        {
            if (eleCustomExport != null && bunifuCards.Controls.Contains(eleCustomExport))
            {
                bunifuCards.Controls.Remove(eleCustomExport);
                eleCustomExport.Dispose();
                eleCustomExport = null;
            }
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
                dataGridView.Rows[i].Cells[0].Value = true;

            dataGridView.Columns[1].Width = 60;
            dataGridView.Columns[1].DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding() { Left = 20, Top = 0, Right = 0, Bottom = 0 } };

            UpdateScrollbarValue();
        }

        public void AddAccountsToDatagrid()
        {
            if (frm.accounts.Count > 0)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnF2;
                dataGridView.Columns.Clear();
                dataGridView.Columns.Add(new DataGridViewCheckBoxColumn());
                dataGridView.Columns[0].HeaderText = "Export";
                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[0].Width = 60;
                dataGridView.Columns[0].ValueType = typeof(bool);
                dataGridView.Columns[0].CellTemplate.Value = true;

                dataGridView.DataBindings.Clear();
                bindingSource.DataSource = frm.accounts;
                dataGridView.DataSource = bindingSource;
            }
        }

        private void EleExport_Load(object sender, EventArgs e)
        {
            isInit = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pExport.Visible = false;
            btnNext.Visible = true;
            //btnClose.Top = btnNext.Top;
            btnClose.Visible = false;
            btnSelectAll.Visible =
            btnDeselectAll.Visible = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pExport.Visible = true;
            btnNext.Visible = false;
            btnSelectAll.Visible =
            btnDeselectAll.Visible = false;
            btnClose.Top = btnNext.Top;
            btnClose.Visible = true;
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (isInit)
                return;

            btnExport.Enabled = tbPassword.Text.Length > 3;
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Title = "Export accounts to...";
            diag.Filter = "All files (*.*)|*.*";

            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(tbPath.Text)))
                diag.InitialDirectory = System.IO.Path.GetDirectoryName(tbPath.Text);
            else
                diag.InitialDirectory = Application.StartupPath;
            diag.FileName = $"{fileName}{GetFileExtension()}";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                fileName = System.IO.Path.GetFileNameWithoutExtension(diag.FileName);
                tbPath.Text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(diag.FileName), $"{fileName}{GetFileExtension()}");
            }
        }

        private string GetFileExtension()
        {
            switch (dropExport.SelectedIndex)
            {
                case 0:
                    return ".EAMexport";
                case 1:
                    return ".EAMexport";
                case 2:
                    return ".CSV";
                case 3:
                    return ".CSV";
                case 4:
                    return ".txt";
                default:
                    return string.Empty;
            }
        }

        public void WriteStringToFile(string exportString)
        {
            try
            {
                System.IO.File.WriteAllText(tbPath.Text, exportString);
                frm.ShowSnackbar("Accounts exported successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
            }
            catch (Exception)
            {
                frm.ShowSnackbar("Failed to export accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            SetExportState(true);
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            SetExportState(false);
        }

        private void SetExportState(bool state)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
                dataGridView.Rows[i].Cells[0].Value = state;
        }
    }
}
