using CsvHelper;
using CsvHelper.Configuration;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExaltAccountManager
{
    public partial class FrmImExport : Form
    {
        FrmMainOLD frm;
        public FrmImExport(FrmMainOLD _frm)
        {
            InitializeComponent();
            frm = _frm;
            dropExports.SelectedIndex = 0;

            ApplyTheme(frm.useDarkmode);
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.ForeColor =
                pImport.BorderColor =
                seperator.LineColor =
                tbPassword.ForeColor = dropExports.ForeColor =
                btnExport.ForeColor = font;

                this.BackColor = second;
                pTop.BackColor =
                pBox.BackColor =
                btnImport.BackColor =
                tbPassword.BackColor = tbPassword.OnIdleState.FillColor = tbPassword.OnActiveState.FillColor = tbPassword.OnDisabledState.FillColor = tbPassword.OnHoverState.FillColor = dropExports.BackgroundColor = def;
                tbPassword.OnIdleState.BorderColor = font;
                tbPassword.Update();
                dropExports.ItemBackColor = def;
                dropExports.ItemForeColor = font;

                pImport.BackgroundColor = pbImport.BackColor = lImport.BackColor = second;
                pbImport.Image = Properties.Resources.ic_filter_none_white_48dp;

                btnExport.Image = Properties.Resources.save_18p;

                btnImport.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 225, 225, 225);
                btnImport.Image = Properties.Resources.ic_folder_open_white_18dp;
                pbLogo.Image = Properties.Resources.ic_import_export_white_48dp;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                p = new Pen(Color.White);

                toggleAcceptRisk.ForeColor =
                toggleAcceptRisk.ToggleStateOn.BorderColor =
                toggleAcceptRisk.ToggleStateOn.BackColor = font;

                toggleAcceptRisk.ToggleStateOn.BorderColorInner =
                toggleAcceptRisk.ToggleStateOn.BackColorInner = def;
            }
        }

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }

        #region Paint

        Pen p = new Pen(Color.Black);
        private void FmImExport_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        private void pTop_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
        }

        private void pbClose_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, topRight, lowerRight);
        }

        private void pbMinimize_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);

            e.Graphics.DrawLine(p, topLeft, topRight);
        }

        private void pbLogo_Paint(object sender, PaintEventArgs e)
        {
            Control s = sender as Control;
            Point topLeft = new Point();
            Point topRight = new Point(s.Width - 1, 0);
            Point lowerLeft = new Point(0, s.Height - 1);
            Point lowerRight = new Point(s.Width - 1, s.Height - 1);

            e.Graphics.DrawLine(p, topLeft, topRight);
            e.Graphics.DrawLine(p, lowerLeft, lowerRight);
            e.Graphics.DrawLine(p, lowerLeft, topLeft);
        }

        #endregion

        #region Drag Form

        private Point MouseDownLocation;
        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDownLocation = e.Location;
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                this.Location = new Point(e.X + this.Left - MouseDownLocation.X, e.Y + this.Top - MouseDownLocation.Y);
        }

        #endregion

        #region Button Close / Minimize

        private void pbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbClose.BackColor = Color.FromArgb(225, 50, 50);
            else
                pbClose.BackColor = Color.IndianRed;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            if (frm.useDarkmode)
                pbMinimize.BackColor = Color.DimGray;
            else
                pbMinimize.BackColor = Color.DarkGray;
        }

        private void pbMinimize_MouseLeave(object sender, EventArgs e)
        {
            pbMinimize.BackColor = Color.Transparent;
        }

        #endregion

        private void btnImport_MouseEnter(object sender, EventArgs e)
        {
            btnImport.Image = frm.useDarkmode ? Properties.Resources.ic_folder_white_18dp : Properties.Resources.ic_folder_black_18dp;
        }

        private void btnImport_MouseLeave(object sender, EventArgs e)
        {
            btnImport.Image = frm.useDarkmode ? Properties.Resources.ic_folder_open_white_18dp : Properties.Resources.ic_folder_open_black_18dp;
        }

        private void pImport_DragEnter(object sender, DragEventArgs e)
        {
            pbImport.Image = frm.useDarkmode ? Properties.Resources.ic_add_to_photos_white_48dp : Properties.Resources.ic_add_to_photos_black_48dp;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void pImport_DragLeave(object sender, EventArgs e)
        {
            pbImport.Image = frm.useDarkmode ? Properties.Resources.ic_filter_none_white_48dp : Properties.Resources.ic_filter_none_black_48dp;
        }

        private void pImport_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                ImportFile(files[0]);
            }

            pImport_DragLeave(sender, null);
        }

        private void ImportFile(string path)
        {
            try
            {
                if (timerNotSupported.Enabled)
                    timerNotSupported.Stop();

                lImport.Text = "Importing, please wait...";
                lImport.ForeColor = Color.SeaGreen;
                lImport.Update();

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
                                            FrmImportAskPass ask = new FrmImportAskPass();
                                            ask.ApplyTheme(frm.useDarkmode);
                                            ask.StartPosition = FormStartPosition.Manual;
                                            ask.Location = new Point(this.Left + pImport.Left, this.Top + pImport.Top);
                                            if (ask.ShowDialog() == DialogResult.OK)
                                            {
                                                AesCryptographyService aes = new AesCryptographyService();
                                                System.Tuple<byte[], byte[]> keyIV = aes.GetKeyAndIV(ask.password);

                                                ExportAccounts accs = (ExportAccounts)frm.ByteArrayToObject(aes.Decrypt(saveFile.data, keyIV.Item1, keyIV.Item2));

                                                ImportExportAccounts(accs);

                                                this.Close();
                                            }
                                            else
                                            {
                                                //Cancel
                                                frm.snackbar.Show(frm, $"Import canceled.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                                            }
                                        }
                                        break;
                                    case ExportType.AccountsNoPassword:
                                        {
                                            AesCryptographyService aes = new AesCryptographyService();
                                            ExportAccounts accs = (ExportAccounts)frm.ByteArrayToObject(aes.Decrypt(saveFile.data));

                                            ImportExportAccounts(accs);

                                            this.Close();
                                        }
                                        break;
                                    case ExportType.CompleteSaveFileWithPassword:
                                        {
                                            FrmImportAskPass ask = new FrmImportAskPass();
                                            ask.ApplyTheme(frm.useDarkmode);
                                            ask.StartPosition = FormStartPosition.Manual;
                                            ask.Location = new Point(this.Left + pImport.Left, this.Top + pImport.Top);
                                            if (ask.ShowDialog() == DialogResult.OK)
                                            {
                                                AesCryptographyService aes = new AesCryptographyService();
                                                System.Tuple<byte[], byte[]> keyIV = aes.GetKeyAndIV(ask.password);

                                                ExportAll all = (ExportAll)frm.ByteArrayToObject(aes.Decrypt(saveFile.data, keyIV.Item1, keyIV.Item2));

                                                if (all.dailyLogins != null)
                                                    File.WriteAllBytes(frm.dailyLoginsPath, frm.ObjectToByteArray(all.dailyLogins));
                                                if (all.options != null && all.options.Length > 0)
                                                    File.WriteAllBytes(frm.optionsPath, all.options);
                                                if (all.pingSaveFile != null)
                                                    File.WriteAllBytes(Path.Combine(FrmMainOLD.saveFilePath, "EAM.PingSaveFile"), frm.ObjectToByteArray(all.pingSaveFile));
                                                if (all.notificationOptions != null)
                                                    File.WriteAllBytes(frm.notificationOptionsPath, frm.ObjectToByteArray(all.notificationOptions));

                                                ImportExportAccounts(all.exportAccounts, false);

                                                Application.Restart();
                                            }
                                            else
                                            {
                                                //Cancel
                                                frm.snackbar.Show(frm, $"Import canceled.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        catch { }
                        break;
                    case ".csv":
                        try
                        {
                            using (var reader = new StreamReader(path))
                            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                            {
                                List<ExportCSVAccount> records = csv.GetRecords<ExportCSVAccount>().ToList();
                                int filtered = 0;
                                int skippedAccs = 0;
                                List<MK_EAM_Lib.AccountInfo> accs = new List<MK_EAM_Lib.AccountInfo>();
                                records = records.OrderBy(r => r.orderID).ToList();

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
                                        accs.Add(new MK_EAM_Lib.AccountInfo(records[i]));
                                    }
                                }

                                List<string> emails = frm.accounts.Select(a => a.email).ToList();
                                for (int i = 0; i < accs.Count; i++)
                                {
                                    if (emails.Contains(accs[i].email))
                                    {
                                        skippedAccs++;
                                        continue;
                                    }
                                    else
                                    {
                                        frm.accounts.Add(accs[i]);
                                        frm.AddAccountToOrders(accs[i].email);
                                        emails.Add(accs[i].email);
                                    }
                                }
                                frm.SaveAccounts();
                                frm.SaveAccountOrders();
                                frm.UpdateAccountInfos();

                                if (skippedAccs > 0 || filtered > 0)
                                    frm.snackbar.Show(frm, $"Import successfull, but skipped {skippedAccs + filtered} accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                                else
                                    frm.snackbar.Show(frm, $"Import successfull.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);

                                this.Close();
                            }
                        }
                        catch { }
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

                            foreach (MuledumpAccounts a in accs)
                            {
                                MK_EAM_Lib.AccountInfo i = new MK_EAM_Lib.AccountInfo(a);
                                if (!emails.Contains(i.email))
                                {
                                    if (amnt < 3)
                                    {
                                        try
                                        {
                                            amnt++;
                                            i = frm.GetAccountData(i);
                                        }
                                        catch { }
                                    }

                                    frm.accounts.Add(i);
                                    frm.AddAccountToOrders(i.email);
                                    emails.Add(i.email);
                                }
                                else
                                    skippedAccs++;
                            }

                            frm.UpdateAccountInfos();
                            if (skippedAccs > 0)
                                frm.snackbar.Show(frm, $"Import successfull, but skipped {skippedAccs} accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                            else
                                frm.snackbar.Show(frm, $"Import successfull.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);

                            this.Close();
                        }
                        catch { }
                        break;
                    default:
                        supported = false;
                        lImport.Text = "Filetype not supported!";
                        lImport.ForeColor = Color.Crimson;
                        timerNotSupported.Start();
                        break;
                }
                if (supported)
                {
                    lImport.Text = "Drag & drop to import";
                    lImport.ForeColor = this.ForeColor;
                }
            }
            catch { }
        }

        private void ImportExportAccounts(ExportAccounts accs, bool loadUI = true)
        {
            int skippedAccs = 0;
            if (frm.accounts.Count == 0)
            {
                frm.accounts = accs.accounts;
                frm.accountOrders = accs.accountOrders;
            }
            else
            {
                for (int i = 0; i < accs.accounts.Count; i++)
                {
                    try
                    {
                        var q = frm.accounts.Where(a => a.email.Equals(accs.accounts[i].email));
                        if (q.Count() > 0)
                        {
                            skippedAccs++;
                        }
                        else
                        {
                            frm.accounts.Add(accs.accounts[i]);
                            frm.AddAccountToOrders(accs.accounts[i].email);
                        }
                    }
                    catch { skippedAccs++; }
                }
            }

            frm.SaveAccounts();
            frm.SaveAccountOrders();
            if (loadUI)
                frm.UpdateAccountInfos();

            for (int i = 0; i < accs.statsFileName.Count; i++)
            {
                try
                {
                    if (File.Exists(Path.Combine(frm.accountStatsPath, accs.statsFileName[i])))
                    {
                        StatsMain file = (StatsMain)frm.ByteArrayToObject(File.ReadAllBytes(Path.Combine(frm.accountStatsPath, accs.statsFileName[i])));
                        StatsMain exported = (StatsMain)frm.ByteArrayToObject(accs.statsFileData[i]);
                        if (file.logins.Count == 0)
                        {
                            File.WriteAllBytes(Path.Combine(frm.accountStatsPath, accs.statsFileName[i]), accs.statsFileData[i]);
                        }
                        else if (exported.logins.Count > 0 && file.logins[file.logins.Count - 1].time < exported.logins[exported.logins.Count - 1].time)
                        {
                            File.WriteAllBytes(Path.Combine(frm.accountStatsPath, accs.statsFileName[i]), accs.statsFileData[i]);
                        }
                    }
                    else
                        File.WriteAllBytes(Path.Combine(frm.accountStatsPath, accs.statsFileName[i]), accs.statsFileData[i]);
                }
                catch { }
            }
            if (skippedAccs > 0)
                frm.snackbar.Show(frm, $"Import successfull, but skipped {skippedAccs} accounts.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
            else
                frm.snackbar.Show(frm, $"Import successfull.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
        }

        private void btnImport_Click(object sender, EventArgs e)
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

        private void btnExport_MouseEnter(object sender, EventArgs e)
        {
            btnExport.Image = frm.useDarkmode ? Properties.Resources.ic_save_white_18dp : Properties.Resources.ic_save_black_18dp;
        }

        private void btnExport_MouseLeave(object sender, EventArgs e)
        {
            btnExport.Image = frm.useDarkmode ? Properties.Resources.save_18p : Properties.Resources.outline_save_black_18dp;
        }

        bool isInit = false;
        private void dropExports_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInit = true;
            switch (dropExports.SelectedIndex)
            {
                case 0: //Accounts encrypted with a password
                    pTbPassword.Visible = true;
                    pToggleRisk.Visible = false;
                    tbPassword.Clear();
                    toggleAcceptRisk.Checked = false;
                    btnExport.Visible = false;
                    break;
                case 1: //Accounts NOT encrypted (carefull!)
                    pTbPassword.Visible = false;
                    pToggleRisk.Visible = true;
                    tbPassword.Clear();
                    toggleAcceptRisk.Checked = false;
                    btnExport.Visible = false;
                    break;
                case 2: //Accounts as CSV. without passwords
                    pTbPassword.Visible = false;
                    pToggleRisk.Visible = false;
                    tbPassword.Clear();
                    toggleAcceptRisk.Checked = false;
                    btnExport.Visible = true;
                    break;
                case 3: //Accounts as CSV. with passwords (carefull!)
                    pTbPassword.Visible = false;
                    pToggleRisk.Visible = true;
                    tbPassword.Clear();
                    toggleAcceptRisk.Checked = false;
                    btnExport.Visible = false;
                    break;
                case 4: //Complete savefile encrypted with a password
                    pTbPassword.Visible = true;
                    pToggleRisk.Visible = false;
                    tbPassword.Clear();
                    toggleAcceptRisk.Checked = false;
                    btnExport.Visible = false;
                    break;
                default:
                    break;
            }
            isInit = false;
        }

        private void toggleAcceptRisk_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (isInit) return;

            btnExport.Visible = toggleAcceptRisk.Checked;
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (isInit) return;

            btnExport.Visible = tbPassword.TextLength > 3;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            switch (dropExports.SelectedIndex)
            {
                case 0: //Accounts encrypted with a password
                    try
                    {
                        SaveFileDialog diag = new SaveFileDialog();
                        diag.Title = "Export accounts to...";
                        diag.Filter = "All files (*.*)|*.*";
                        diag.InitialDirectory = Application.StartupPath;

                        if (diag.ShowDialog() == DialogResult.OK)
                        {
                            ExportSaveFile saveFile = new ExportSaveFile()
                            {
                                version = 1,
                                exportType = ExportType.AccountsWithPassword
                            };
                            ExportAccounts accs = new ExportAccounts()
                            {
                                accounts = frm.accounts,
                                accountOrders = frm.accountOrders,
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

                            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(diag.FileName), Path.GetFileNameWithoutExtension(diag.FileName) + ".EAMexport"), frm.ObjectToByteArray(saveFile));
                        }
                        diag.Dispose();
                        frm.snackbar.Show(frm, $"Exported accounts successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                        this.Close();
                    }
                    catch { }
                    break;
                case 1: //Accounts NOT encrypted (carefull!)
                    try
                    {
                        SaveFileDialog diag = new SaveFileDialog();
                        diag.Title = "Export accounts to...";
                        diag.Filter = "All files (*.*)|*.*";
                        diag.InitialDirectory = Application.StartupPath;

                        if (diag.ShowDialog() == DialogResult.OK)
                        {
                            ExportSaveFile saveFile = new ExportSaveFile()
                            {
                                version = 1,
                                exportType = ExportType.AccountsNoPassword
                            };
                            ExportAccounts accs = new ExportAccounts()
                            {
                                accounts = frm.accounts,
                                accountOrders = frm.accountOrders,
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

                            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(diag.FileName), Path.GetFileNameWithoutExtension(diag.FileName) + ".EAMexport"), frm.ObjectToByteArray(saveFile));
                        }
                        diag.Dispose();
                        frm.snackbar.Show(frm, $"Exported accounts successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                        this.Close();
                    }
                    catch { }
                    break;
                case 2: //Accounts as CSV. without passwords
                    try
                    {
                        SaveFileDialog diag = new SaveFileDialog();
                        diag.Title = "Export accounts as .csv to...";
                        diag.Filter = "csv (*.csv)|*.csv";
                        diag.InitialDirectory = Application.StartupPath;

                        if (diag.ShowDialog() == DialogResult.OK)
                        {
                            List<ExportCSVAccount> export = new List<ExportCSVAccount>();

                            for (int i = 0; i < frm.accounts.Count; i++)
                                export.Add(new ExportCSVAccount(frm.accounts[i]));

                            using (var writer = new StreamWriter(diag.FileName))
                            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(export);
                            }
                        }
                        diag.Dispose();
                        frm.snackbar.Show(frm, $"Exported accounts as .csv successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                        this.Close();
                    }
                    catch { }
                    break;
                case 3: //Accounts as CSV. with passwords (carefull!)
                    try
                    {
                        SaveFileDialog diag = new SaveFileDialog();
                        diag.Title = "Export accounts as .csv to...";
                        diag.Filter = "csv (*.csv)|*.csv";
                        diag.InitialDirectory = Application.StartupPath;

                        if (diag.ShowDialog() == DialogResult.OK)
                        {
                            List<ExportCSVAccount> export = new List<ExportCSVAccount>();

                            for (int i = 0; i < frm.accounts.Count; i++)
                            {
                                int index = -1;
                                OrderData q = frm.accountOrders.orderData.Where(o => o.email.Equals(frm.accounts[i].email)).First();
                                if (q != null)
                                    index = q.index;

                                export.Add(new ExportCSVAccount(frm.accounts[i], true, index));
                            }

                            using (var writer = new StreamWriter(diag.FileName))
                            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                csv.WriteRecords(export);
                            }
                        }
                        diag.Dispose();
                        frm.snackbar.Show(frm, $"Exported accounts as .csv successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                        this.Close();
                    }
                    catch { }
                    break;
                case 4: //Complete savefile encrypted with a password
                    try
                    {
                        SaveFileDialog diag = new SaveFileDialog();
                        diag.Title = "Export all configurations and accounts to...";
                        diag.Filter = "All files (*.*)|*.*";
                        diag.InitialDirectory = Application.StartupPath;

                        if (diag.ShowDialog() == DialogResult.OK)
                        {
                            ExportSaveFile saveFile = new ExportSaveFile()
                            {
                                version = 1,
                                exportType = ExportType.CompleteSaveFileWithPassword
                            };
                            ExportAll all = new ExportAll()
                            {
                                exportAccounts = new ExportAccounts()
                                {
                                    accounts = frm.accounts,
                                    accountOrders = frm.accountOrders,
                                    statsFileName = new List<string>(),
                                    statsFileData = new List<byte[]>()
                                },
                                notificationOptions = frm.notOpt,
                                dailyLogins = File.Exists(frm.dailyLoginsPath) ? (DailyLoginsOLD)frm.ByteArrayToObject(File.ReadAllBytes(frm.dailyLoginsPath)) : null,
                                options = File.Exists(frm.optionsPath) ? File.ReadAllBytes(frm.optionsPath) : new byte[0],
                                pingSaveFile = File.Exists(Path.Combine(FrmMainOLD.saveFilePath, "EAM.PingSaveFile")) ? (PingSaveFile)frm.ByteArrayToObject(File.ReadAllBytes(Path.Combine(FrmMainOLD.saveFilePath, "EAM.PingSaveFile"))) : null,
                            };

                            for (int i = 0; i < all.exportAccounts.accounts.Count; i++)
                                all.exportAccounts.accounts[i].accessToken = null;

                            foreach (string f in Directory.GetFiles(frm.accountStatsPath))
                            {
                                all.exportAccounts.statsFileName.Add(Path.GetFileName(f));
                                all.exportAccounts.statsFileData.Add(File.ReadAllBytes(f));
                            }

                            AesCryptographyService aes = new AesCryptographyService();
                            System.Tuple<byte[], byte[]> keyIV = aes.GetKeyAndIV(tbPassword.Text);
                            byte[] enc = aes.Encrypt(frm.ObjectToByteArray(all), keyIV.Item1, keyIV.Item2);
                            saveFile.data = enc;

                            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(diag.FileName), Path.GetFileNameWithoutExtension(diag.FileName) + ".EAMexport"), frm.ObjectToByteArray(saveFile));
                        }
                        diag.Dispose();
                        frm.snackbar.Show(frm, $"Exported complete configuration successfully.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, "X", Bunifu.UI.WinForms.BunifuSnackbar.Positions.BottomRight);
                        this.Close();
                    }
                    catch { }
                    break;
                default:
                    break;
            }

        }

        private void timerNotSupported_Tick(object sender, EventArgs e)
        {
            lImport.Text = "Darg & drop to import";
            lImport.ForeColor = this.ForeColor;
            timerNotSupported.Stop();
        }
    }
}
