using CsvHelper;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements
{
    public sealed partial class EleImport : UserControl
    {
        private FrmMain frm;
        private BindingList<MK_EAM_Lib.AccountInfo> importAccounts = new BindingList<MK_EAM_Lib.AccountInfo>();
        private BindingSource bindingSource = new BindingSource();
        private bool isInit = true;
        private EleCustomImport eleCustomImport = null;

        private UIStates UIState
        {
            get => uiStateValue;
            set
            {
                uiStateValue = value;
                switch (uiStateValue)
                {
                    case UIStates.WaitForFile:
                        pDatagrid.Visible =
                        pImportPassword.Visible = false;
                        pDragDrop.Location = new Point(175, 58);
                        pDragDrop.Visible =
                        pbImport.Visible =
                        btnChangePath.Enabled =
                        pFilterParent.Visible = true;

                        pbImport.Image = frm.UseDarkmode ? Properties.Resources.ic_filter_none_white_48dp : Properties.Resources.ic_filter_none_black_48dp;

                        lDND.Text = "Drag and Drop here";
                        lDND.ForeColor = this.ForeColor;

                        pFilterParent.AllowDrop =
                        pDragDrop.AllowDrop =
                        pImport.AllowDrop =
                        lDND.AllowDrop = true;
                        break;
                    case UIStates.WaitForPassword:
                        pDatagrid.Visible =
                        btnChangePath.Enabled = false;
                        pDragDrop.Location = new Point(175, 28);
                        pImportPassword.Visible =
                        pFilterParent.Visible = true;

                        lDND.Text = "Pleas enter the Password";
                        lDND.ForeColor = Color.SeaGreen;

                        tbPassword.Text = String.Empty;
                        tbPassword.Clear();

                        pFilterParent.AllowDrop =
                        pDragDrop.AllowDrop =
                        pImport.AllowDrop =
                        lDND.AllowDrop = false;
                        break;
                    case UIStates.ShowAccounts:
                        pbImport.Visible =
                        pFilterParent.Visible = false;
                        pDatagrid.Visible = true;

                        pFilterParent.AllowDrop =
                        pDragDrop.AllowDrop =
                        pImport.AllowDrop =
                        lDND.AllowDrop = false;
                        break;
                    case UIStates.ShowImportFailed:
                        pDatagrid.Visible =
                        pImportPassword.Visible = false;
                        pDragDrop.Location = new Point(175, 58);
                        pDragDrop.Visible =
                        pbImport.Visible =
                        btnChangePath.Enabled =
                        pFilterParent.Visible = true;

                        lDND.Text = "Import failed!";
                        lDND.ForeColor = Color.Crimson;

                        timerResetOnError.Start();
                        break;
                    default:
                        break;
                }
            }
        }
        private UIStates uiStateValue = UIStates.None;

        public EleImport(FrmMain _frm)
        {
            InitializeComponent();
            frm = _frm;
            UIState = UIStates.WaitForFile;
            isInit = false;

            dataGridView.MouseWheel += dataGridView_MouseWheel;

            frm.ThemeChanged += ApplyTheme;
            this.Disposed += (object sender, EventArgs e) => frm.ThemeChanged -= ApplyTheme;

            ApplyTheme(frm, null);
        }

        public void ApplyTheme(object sender, EventArgs e)
        {
            Color def = ColorScheme.GetColorDef(frm.UseDarkmode);
            Color second = ColorScheme.GetColorSecond(frm.UseDarkmode);
            Color third = ColorScheme.GetColorThird(frm.UseDarkmode);
            Color font = ColorScheme.GetColorFont(frm.UseDarkmode);

            tbPassword.ResetColors();
            this.BackColor =
            lPW.BackColor = pDragDrop.BackColor = pImport.BackgroundColor = pImportPassword.BackColor = lDND.BackColor = pbImport.BackColor =
            tbPassword.BackColor = tbPassword.FillColor = tbPassword.OnIdleState.FillColor = tbPassword.OnHoverState.FillColor = tbPassword.OnActiveState.FillColor = tbPassword.OnDisabledState.FillColor =
            def;
            this.ForeColor = lDND.ForeColor =
            tbPassword.ForeColor = tbPassword.OnActiveState.ForeColor = tbPassword.OnDisabledState.ForeColor = tbPassword.OnHoverState.ForeColor = tbPassword.OnIdleState.ForeColor =
            font;

            bunifuCards.BackColor = pFilterParent.BackColor = second;

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

            pbImport.Image = frm.UseDarkmode ? Properties.Resources.ic_filter_none_white_48dp : Properties.Resources.ic_filter_none_black_48dp;
            this.Invalidate();
        }

        private void pDragDrop_DragEnter(object sender, DragEventArgs e)
        {
            pbImport.Image = frm.UseDarkmode ? Properties.Resources.ic_add_to_photos_white_48dp : Properties.Resources.ic_add_to_photos_black_48dp;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void pDragDrop_DragLeave(object sender, EventArgs e)
        {
            pbImport.Image = frm.UseDarkmode ? Properties.Resources.ic_filter_none_white_48dp : Properties.Resources.ic_filter_none_black_48dp;

        }

        private void pDragDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                ImportFile(files[0]);
            }

            pDragDrop_DragLeave(sender, null);
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select the file you want to import.";
            diag.Filter = "All files (*.*)|*.*";
            diag.InitialDirectory = Application.StartupPath;
            diag.Multiselect = false;

            if (diag.ShowDialog() == DialogResult.OK)
            {
                ImportFile(diag.FileName);
            }
        }

        private void ImportFile(string path, string _password = "")
        {
            importFilePath = path;
            try
            {
                //if (timerNotSupported.Enabled)
                //    timerNotSupported.Stop();

                lDND.Text = "Importing, please wait...";
                lDND.ForeColor = Color.SeaGreen;
                pbImport.Image = frm.UseDarkmode ? Properties.Resources.ic_hourglass_empty_white_36dp : Properties.Resources.ic_hourglass_empty_black_36dp;
                lDND.Update();
                pbImport.Update();

                string fileExtension = Path.GetExtension(path);
                bool supported = true;

                switch (fileExtension)
                {
                    case ".EAMexport":
                        try
                        {
                            ExportSaveFile saveFile = (ExportSaveFile)frm.ByteArrayToObject(File.ReadAllBytes(path));
                            if (saveFile.version == 1)
                            {
                                switch (saveFile.exportType)
                                {
                                    case ExportType.AccountsWithPassword:
                                        {
                                            if (_password.Length == 0)
                                            {
                                                UIState = UIStates.WaitForPassword;
                                                return;
                                            }
                                            else
                                            {
                                                AesCryptographyService aes = new AesCryptographyService();
                                                System.Tuple<byte[], byte[]> keyIV = aes.GetKeyAndIV(_password);

                                                ExportAccounts accs = (ExportAccounts)frm.ByteArrayToObject(aes.Decrypt(saveFile.data, keyIV.Item1, keyIV.Item2));

                                                AddAccountsToDatagrid(accs.accounts);
                                                frm.ShowSnackbar($"Loading successfull.{Environment.NewLine}Please select the accounts you want to import.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                                            }
                                        }
                                        break;
                                    case ExportType.AccountsNoPassword:
                                        {
                                            AesCryptographyService aes = new AesCryptographyService();
                                            ExportAccounts accs = (ExportAccounts)frm.ByteArrayToObject(aes.Decrypt(saveFile.data));

                                            AddAccountsToDatagrid(accs.accounts);
                                            frm.ShowSnackbar($"Loading successfull.{Environment.NewLine}Please select the accounts you want to import.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                                        }
                                        break;
                                    case ExportType.CompleteSaveFileWithPassword:
                                        {
                                            if (_password.Length == 0)
                                            {
                                                UIState = UIStates.WaitForPassword;
                                                return;
                                            }
                                            else
                                            {
                                                AesCryptographyService aes = new AesCryptographyService();
                                                System.Tuple<byte[], byte[]> keyIV = aes.GetKeyAndIV(_password);

                                                ExportAll all = (ExportAll)frm.ByteArrayToObject(aes.Decrypt(saveFile.data, keyIV.Item1, keyIV.Item2));

                                                if (all.dailyLogins != null)
                                                    File.WriteAllBytes(frm.dailyLoginsPath, frm.ObjectToByteArray(all.dailyLogins));
                                                if (all.options != null && all.options.Length > 0)
                                                    File.WriteAllBytes(frm.optionsPath, all.options);
                                                if (all.pingSaveFile != null)
                                                    File.WriteAllBytes(Path.Combine(FrmMainOLD.saveFilePath, "EAM.PingSaveFile"), frm.ObjectToByteArray(all.pingSaveFile));
                                                if (all.notificationOptions != null)
                                                    File.WriteAllBytes(frm.notificationOptionsPath, frm.ObjectToByteArray(all.notificationOptions));

                                                AddAccountsToDatagrid(all.exportAccounts.accounts);
                                                frm.ShowSnackbar($"Loading successfull.{Environment.NewLine}Please select the accounts you want to import.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        catch { UIState = UIStates.ShowImportFailed; }
                        break;
                    case ".csv":
                        try
                        {
                            using (var reader = new StreamReader(path))
                            using (var csv = new CsvHelper.CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) { MissingFieldFound = null}))
                            {
                                csv.Read();
                                csv.ReadHeader();
                                
                                if (!csv.HeaderRecord.Contains("username") || !csv.HeaderRecord.Contains("color") || !csv.HeaderRecord.Contains("orderID"))
                                {
                                    reader.BaseStream.Position = 0;
                                }

                                List<ExportCSVAccount> records = csv.GetRecords<ExportCSVAccount>().ToList();
                                int filtered = 0;
                                int skippedAccs = 0;
                                List<MK_EAM_Lib.AccountInfo> accs = new List<MK_EAM_Lib.AccountInfo>();                               
                                records = records.OrderBy(r => r.orderID).ToList();

                                List<ExportCSVAccount> toRemove = records.Where(r => r.email.ToLower().Equals("email") || r.email.ToLower().Equals("username")).ToList();
                                toRemove.ForEach(r => { records.Remove(r); });

                                List<string> emails = frm.accounts.Select(a => a.email).ToList();

                                for (int i = 0; i < records.Count; i++)
                                {
                                    if (string.IsNullOrEmpty(records[i].password) || string.IsNullOrEmpty(records[i].email))
                                    {
                                        records.RemoveAt(i);
                                        i--;
                                        filtered++;
                                    }
                                    else
                                    {
                                        if (!emails.Contains(records[i].email))
                                            accs.Add(new MK_EAM_Lib.AccountInfo(records[i]));
                                    }
                                }
                                AddAccountsToDatagrid(accs);
                                if (skippedAccs > 0)
                                    frm.ShowSnackbar($"Loading successfull, but skipped {skippedAccs} accounts.{Environment.NewLine}Please select the accounts you want to import.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                                else
                                    frm.ShowSnackbar($"Loading successfull.{Environment.NewLine}Please select the accounts you want to import.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                            }
                        }
                        catch { UIState = UIStates.ShowImportFailed; }
                        break;
                    case ".js": //muledump
                        try
                        {
                            string[] rows = File.ReadAllLines(path);
                            bool active = false;
                            char splitChar = 'a';
                            List<MuledumpAccounts> accs = new List<MuledumpAccounts>();

                            for (int i = 0; i < rows.Length; i++)
                            {
                                if (rows[i].TrimStart().StartsWith("accounts = {"))
                                    active = true;
                                else if (active)
                                {
                                    if (rows[i].TrimStart().StartsWith("}"))
                                        break;
                                    if (rows[i].TrimStart().StartsWith("'") || rows[i].TrimStart().StartsWith("\""))
                                    {
                                        if (splitChar.Equals('a'))
                                            splitChar = rows[i].TrimStart().StartsWith("'") ? '\'' : '"';

                                        string[] data = rows[i].Split(splitChar);
                                        MuledumpAccounts acc = new MuledumpAccounts
                                        {
                                            mail = data[1],
                                            password = data[3]
                                        };
                                        accs.Add(acc);
                                    }
                                }
                            }

                            int amnt = 0;
                            int skippedAccs = 0;
                            List<string> emails = frm.accounts.Select(a => a.email).ToList();
                            List<MK_EAM_Lib.AccountInfo> listAccs = new List<MK_EAM_Lib.AccountInfo>();
                            foreach (MuledumpAccounts a in accs)
                            {
                                MK_EAM_Lib.AccountInfo i = new MK_EAM_Lib.AccountInfo(a);
                                if (!emails.Contains(i.Email))
                                {
                                    if (amnt < 3)
                                    {
                                        try
                                        {
                                            amnt++;
                                            i.PerformWebrequest(this, frm.LogEvent, "EAM Importer", frm.accountStatsPath, frm.itemsSaveFilePath, frm.GetDeviceUniqueIdentifier(), true, true);
                                        }
                                        catch { }
                                    }

                                    listAccs.Add(i);
                                    emails.Add(i.Email);
                                }
                                else
                                    skippedAccs++;
                            }
                            AddAccountsToDatagrid(listAccs);

                            if (skippedAccs > 0)
                                frm.ShowSnackbar($"Loading successfull, but skipped {skippedAccs} accounts.{Environment.NewLine}Please select the accounts you want to import.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                            else
                                frm.ShowSnackbar($"Loading successfull.{Environment.NewLine}Please select the accounts you want to import.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);
                        }
                        catch { UIState = UIStates.ShowImportFailed; }
                        break;
                    case ".txt":
                        {
                            eleCustomImport = new EleCustomImport(frm, this, path);
                            bunifuCards.Controls.Add(eleCustomImport);
                            eleCustomImport.Location = new System.Drawing.Point(20, 58);
                            eleCustomImport.BringToFront();
                            return;
                        }
                    default:
                        {
                            try
                            {
                                FileInfo fileInfo = new FileInfo(path);
                                if (fileInfo.Length > 50000000)
                                {
                                    supported = false;
                                    frm.ShowSnackbar("Filesize not supported!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                                    importFilePath = string.Empty;
                                    _password = string.Empty;
                                    UIState = UIStates.WaitForFile;
                                    return;
                                }
                                if (!File.ReadAllText(path).Any(ch => char.IsControl(ch) && ch != '\r' && ch != '\n'))
                                {
                                    eleCustomImport = new EleCustomImport(frm, this, path);
                                    bunifuCards.Controls.Add(eleCustomImport);
                                    eleCustomImport.Location = new System.Drawing.Point(20, 58);
                                    eleCustomImport.BringToFront();
                                }
                                else
                                {
                                    supported = false;
                                    frm.ShowSnackbar("Filetype not supported!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                                    importFilePath = string.Empty;
                                    _password = string.Empty;
                                    UIState = UIStates.WaitForFile;
                                }
                            }
                            catch
                            {
                                supported = false;
                                frm.ShowSnackbar("Filetype not supported!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                                importFilePath = string.Empty;
                                _password = string.Empty;
                                UIState = UIStates.WaitForFile;
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                frm.ShowSnackbar("File not supported!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000);
                importFilePath = string.Empty;
                _password = string.Empty;

                UIState = UIStates.ShowImportFailed;
            }
        }

        public void AddAccountsToDatagrid(List<MK_EAM_Lib.AccountInfo> accs)
        {
            List<string> emails = frm.accounts.Select(a => a.Email).ToList();
            importAccounts = new BindingList<MK_EAM_Lib.AccountInfo>(accs.Where(a => !emails.Contains(a.Email))
                                                                         .GroupBy(a => a.Email)
                                                                         .Select(a => a.First())
                                                                         .OrderBy(a => a.orderID)
                                                                         .ToList());

            if (importAccounts.Count > 0)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnF2;
                dataGridView.Columns.Clear();
                dataGridView.Columns.Add(new DataGridViewCheckBoxColumn());
                dataGridView.Columns[0].HeaderText = "Import";
                dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[0].Width = 60;
                dataGridView.Columns[0].ValueType = typeof(bool);
                dataGridView.Columns[0].CellTemplate.Value = true;

                dataGridView.DataBindings.Clear();
                bindingSource.DataSource = importAccounts;
                dataGridView.DataSource = bindingSource;
            }
            else
            {
                frm.ShowSnackbar("No new account found.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 5000);
                btnClose_Click(null, null);
            }
        }

        public void CloseEleCustomImport(bool closeOnly = false)
        {
            if (eleCustomImport != null && bunifuCards.Controls.Contains(eleCustomImport))
            {
                bunifuCards.Controls.Remove(eleCustomImport);
                eleCustomImport.Dispose();
                eleCustomImport = null;
            }
            if (closeOnly)
            {
                UIState = UIStates.None;
                UIState = UIStates.WaitForFile;
            }
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

        internal enum UIStates
        {
            None,
            WaitForFile,
            WaitForPassword,
            ShowAccounts,
            ShowImportFailed
        }

        private string importFilePath = string.Empty;
        private void btnPasswordOK_Click(object sender, EventArgs e)
        {
            btnPasswordOK.Enabled = false;

            if (!string.IsNullOrEmpty(importFilePath) && tbPassword.Text.Length >= 3)
            {
                ImportFile(importFilePath, tbPassword.Text);
            }
            else
            {
                importFilePath = string.Empty;
                tbPassword.Clear();
                UIState = UIStates.WaitForFile;
            }

            btnPasswordOK.Enabled = true;
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
                dataGridView.Rows[i].Cells[0].Value = true;

            dataGridView.Columns[1].Width = 60;
            dataGridView.Columns[1].DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding() { Left = 20, Top = 0, Right = 0, Bottom = 0 } };

            UpdateScrollbarValue();

            UIState = UIStates.ShowAccounts;
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                importAccounts.RemoveAt(dataGridView.SelectedRows[0].Index);

            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            bool foundImportData = false;
            frm.accounts.RaiseListChangedEvents = false;

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {                
                if ((bool)dataGridView.Rows[i].Cells[0].Value)
                {
                    frm.accounts.Add(importAccounts[i]);
                    foundImportData = true;
                }
            }

            frm.accounts.RaiseListChangedEvents = true;

            if (foundImportData)
            {
                frm.SaveAndUpdateAccounts();
                frm.ShowSnackbar("Import successfull.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000);

                btnClose_Click(btnClose, e);
            }
            else
            {
                frm.ShowSnackbar("No accounts selected for import!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 5000);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            importAccounts = new BindingList<MK_EAM_Lib.AccountInfo>();
            dataGridView.DataBindings.Clear();
            UpdateScrollbarValue();

            UIState = UIStates.WaitForFile;
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            btnPasswordOK.Enabled = tbPassword.Text.Length >= 3;
        }

        private void timerResetOnError_Tick(object sender, EventArgs e)
        {
            timerResetOnError.Stop();

            UIState = UIStates.WaitForFile;
        }
    }
}
