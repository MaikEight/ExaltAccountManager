using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Security.Cryptography;

namespace ExaltAccountManager
{
    public partial class FrmUpdater : Form
    {
        private FrmMain frm;
        private bool updateRequired = false;

        private string[] updateWebequestURLs = new string[]
            {
                "https://www.realmofthemadgod.com/app/init?platform=standalonewindows64&key=9KnJFxtTvLu2frXv",
                "https://rotmg-build.decagames.com/build-release/{0}/rotmg-exalt-win-64/checksum.json",
                "https://rotmg-build.decagames.com/build-release/{0}/rotmg-exalt-win-64/{1}.gz"
            };

        private string pathTemp = string.Empty;
        private string checksumJson = string.Empty;
        private string buildHash = string.Empty;
        private List<ChecksumFile> outdatedFiles;
        private bool returnToForm = false;

        public FrmUpdater(FrmMain _frm, bool _returnToForm = false)
        {
            returnToForm = _returnToForm;
            frm = _frm;

            if (!returnToForm)
                InitializeComponent();


            if (!returnToForm)
                ApplyTheme(frm.useDarkmode);

            if (string.IsNullOrEmpty(frm.buildHash))
            {
                if (!returnToForm)
                    lStatus.Text = "Searching...";
                Task.Run(() => CheckForUpdate());
            }
            else
            {
                buildHash = frm.buildHash;
                checksumJson = frm.checksumJson;
                updateRequired = frm.updateRequired;

                if (!returnToForm)
                    UpdateInfoUI();
            }

            if (returnToForm)
            {
                this.FormClosing -= Frm_Closing;
                this.Dispose();
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (updateRequired)
            {
                progressbar.Value = 0;
                pages.SetPage("Update");
                this.Height = 300;
                this.Top -= 50;

                Application.DoEvents();
                PerformUpdate();
            }
            else
            { //Check again
                btnInfo.Enabled = false;

                lStatus.Text = "Searching...";
                pbState.Image = frm.useDarkmode ? Properties.Resources.ic_hourglass_empty_white_36dp : Properties.Resources.ic_hourglass_empty_black_36dp;
                Application.DoEvents();

                CheckForUpdate();

                btnInfo.Enabled = true;
            }
        }

        private void CheckForUpdate()
        {
            try
            {
                buildHash = PerformWebrequest(updateWebequestURLs[0]);
                int index = buildHash.IndexOf("<BuildHash>") + 11;
                frm.buildHash = buildHash = buildHash.Substring(index, ((buildHash.IndexOf("</BuildHash>") - index)));

                frm.checksumJson = checksumJson = PerformWebrequest(string.Format(updateWebequestURLs[1], buildHash));

                ChecksumFiles checksumFile = ChecksumFiles.FromJson(checksumJson);
                outdatedFiles = new List<ChecksumFile>();

                string rootPath = Path.GetDirectoryName(frm.exePath);
                if (Directory.Exists(rootPath))
                {
                    foreach (ChecksumFile f in checksumFile.Files)
                    {
                        if (string.IsNullOrWhiteSpace(f.FileName))
                        {
                            Console.WriteLine("Unnamed file found in list!");
                        }
                        else if (!File.Exists(Path.Combine(rootPath, f.FileName)) || !f.Checksum.Equals(GetMD5AsString(Path.Combine(rootPath, f.FileName))))
                        {
                            outdatedFiles.Add(f);
                        }
                    }
                    frm.updateRequired = updateRequired = outdatedFiles.Count > 0;
                }
                else
                {
                    Directory.CreateDirectory(rootPath);
                    outdatedFiles.AddRange(checksumFile.Files);
                    frm.updateRequired = updateRequired = true;
                }

                if (!returnToForm)
                    UpdateInfoUI();

                string[] rows = new string[] { DateTime.Now.Ticks.ToString(), updateRequired.ToString() };
                if (File.Exists(frm.lastUpdateCheckPath))
                    File.Delete(frm.lastUpdateCheckPath);
                File.WriteAllLines(frm.lastUpdateCheckPath, rows);

                frm.ShowGameUpdateRequired(updateRequired);
            }
            catch { }
        }

        private bool UpdateInfoUI()
        {
            if (this.InvokeRequired)
                return (bool)this.Invoke((Func<bool>)UpdateInfoUI);

            lStatus.Text = updateRequired ? $"Your game is{Environment.NewLine}   outdated!" : $"Your game is{Environment.NewLine}  up to date";
            pbState.Image = updateRequired ? (frm.useDarkmode ? Properties.Resources.ic_new_releases_white_36dp : Properties.Resources.ic_new_releases_black_36dp) : (frm.useDarkmode ? Properties.Resources.ic_beenhere_white_36dp : Properties.Resources.ic_beenhere_black_36dp);
            btnInfo.Text = updateRequired ? "Start update" : "Search for updates";
            btnInfo.Image = updateRequired ? (frm.useDarkmode ? Properties.Resources.below_white_24px : Properties.Resources.below_black_24px) : (frm.useDarkmode ? Properties.Resources.ic_search_white_24dp : Properties.Resources.ic_search_black_24dp);

            return false;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PerformUpdate()
        {
            progressbar.Value = 0;

            if (string.IsNullOrEmpty(buildHash) || string.IsNullOrEmpty(checksumJson) || outdatedFiles == null)
                CheckForUpdate();

            progressbar.Maximum = 100;
            progressbar.Value = 15;
            Application.DoEvents();

            if (updateRequired && outdatedFiles.Count > 0)
            {
                pathTemp = Path.Combine(Path.GetTempPath(), "EAM_Update");
                if (Directory.Exists(pathTemp))
                    Directory.Delete(pathTemp, true);
                Directory.CreateDirectory(pathTemp);

                string rootPath = Path.GetDirectoryName(frm.exePath);
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                using (WebClient client = new WebClient())
                {
                    for (int i = 0; i < outdatedFiles.Count; i++)
                    {
                        string url = string.Format(updateWebequestURLs[2], buildHash, outdatedFiles[i].FileName);

                        string filePath = Path.Combine(pathTemp, $"{outdatedFiles[i].FileName}.gz");

                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        client.DownloadFile(url, filePath);
                        if (File.Exists(filePath))
                        {
                            Decompress(new FileInfo(filePath), Path.Combine(rootPath, outdatedFiles[i].FileName));
                            File.Delete(filePath);
                        }
                        float v = (80f / outdatedFiles.Count) * (i + 1f) + 15f;
                        progressbar.Value = (v >= 100 ? 100 : (int)v);
                    }
                }

                lUpdateStatus.Text = "Update done!";
                lWait.Visible = false;
                progressbar.TransitionValue(100);
                progressbar.Top -= 8;
                btnDone.Visible = true;

                frm.updateRequired = updateRequired = false;

                string[] rows = new string[] { DateTime.Now.Ticks.ToString(), updateRequired.ToString() };
                if (File.Exists(frm.lastUpdateCheckPath))
                    File.Delete(frm.lastUpdateCheckPath);
                File.WriteAllLines(frm.lastUpdateCheckPath, rows);

                frm.ShowGameUpdateRequired(updateRequired);
            }
        }

        private string PerformWebrequest(string url)
        {
            string str = string.Empty;
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                str = reader.ReadToEnd();
            }
            response.Close();
            return str;
        }

        public void Decompress(FileInfo fileToDecompress, string destinationFileName)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;

                if (File.Exists(destinationFileName))
                    File.Delete(destinationFileName);
                if (!Directory.Exists(Path.GetDirectoryName(destinationFileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFileName));

                using (FileStream decompressedFileStream = File.Create(destinationFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        private async Task DownloadFileAsync(string url, string fn)
        {
            using (WebClient client = new WebClient())
            {
                await client.DownloadFileTaskAsync(new Uri(url), fn);
                if (File.Exists(fn))
                {
                    ZipFile.ExtractToDirectory(fn, fn.Substring(0, fn.Length - 3));
                    File.Delete(fn);
                }
            }
        }

        private string GetMD5AsString(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public void ApplyTheme(bool isDarkmode)
        {
            if (isDarkmode)
            {
                Color def = Color.FromArgb(32, 32, 32);
                Color second = Color.FromArgb(23, 23, 23);
                Color third = Color.FromArgb(0, 0, 0);
                Color font = Color.White;

                this.ForeColor = font;
                this.BackColor = second;
                pTop.BackColor =
                pBox.BackColor = def;

                pbLogo.Image = Properties.Resources.below_white_48px;
                pbClose.Image = Properties.Resources.ic_close_white_24dp;
                pbMinimize.Image = Properties.Resources.baseline_minimize_white_24dp;
                pbState.Image = string.IsNullOrEmpty(buildHash) ? Properties.Resources.ic_hourglass_empty_white_36dp : updateRequired ? Properties.Resources.ic_new_releases_white_36dp : Properties.Resources.ic_beenhere_white_36dp;
                btnInfo.Image = Properties.Resources.ic_search_white_24dp;
                btnDone.Image = Properties.Resources.ic_done_white_24dp;

                pageInfo.BackColor =
                pageUpdate.BackColor = second;

                progressbar.BackColor = second;
                progressbar.ForeColor = Color.Gainsboro;

                p = new Pen(Color.White);
            }
        }

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

        Pen p = new Pen(Color.Black);
        private void FrmUpdater_Paint(object sender, PaintEventArgs e)
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

        private void Frm_Closing(object sender, FormClosingEventArgs e)
        {
            frm.lockForm = false;
        }

        private void btnInfo_MouseEnter(object sender, EventArgs e)
        {
            btnInfo.Image = frm.useDarkmode ? (updateRequired ? Properties.Resources.below_white_24px : Properties.Resources.search_more_white_24px) : (updateRequired ? Properties.Resources.below_black_24px : Properties.Resources.search_more_black_24px_2);
        }

        private void btnInfo_MouseLeave(object sender, EventArgs e)
        {
            btnInfo.Image = frm.useDarkmode ? (updateRequired ? Properties.Resources.below_white_24px : Properties.Resources.ic_search_white_24dp) : (updateRequired ? Properties.Resources.below_black_24px : Properties.Resources.ic_search_black_24dp);
        }
    }

    public partial class ChecksumFiles
    {
        [JsonProperty("files")]
        public ChecksumFile[] Files { get; set; }

        public static ChecksumFiles FromJson(string json) => JsonConvert.DeserializeObject<ChecksumFiles>(json, ExaltAccountManager.Converter.Settings);
    }

    public partial class ChecksumFile
    {
        [JsonProperty("file")]
        public string FileName { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        [JsonProperty("permision")]
        public string Permision { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        internal static IDisposable OpenRead(string filename)
        {
            throw new NotImplementedException();
        }
    }

    //public partial class ChecksumFiles
    //{
    //    public static ChecksumFiles FromJson(string json) => JsonConvert.DeserializeObject<ChecksumFiles>(json, ExaltAccountManager.Converter.Settings);
    //}

    public static class Serialize
    {
        public static string ToJson(this ChecksumFiles self) => JsonConvert.SerializeObject(self, ExaltAccountManager.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
