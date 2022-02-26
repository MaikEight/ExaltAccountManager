using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    public class GameUpdater
    {
        public static GameUpdater Instance { get; private set; }
        bool _disposed = false;
        public GameUpdater(string _exePath, string _lastUpdateCheckPath)
        {
            if (GameUpdater.Instance != null)
            {
                _disposed = true;
                return;
            }
            GameUpdater.Instance = this;
            exePath = _exePath;
            lastUpdateCheckPath = _lastUpdateCheckPath;
        }

        public bool UpdateRequired
        {
            get => updateRequired;
            private set
            {
                updateRequired = value;
                if (!suppressUpdateRequiredEvent && !updateInProgress && OnUpdateRequired != null)
                    OnUpdateRequired(this, new EventArgs());
            }
        }
        private bool updateRequired = false;

        public bool CheckedForUpdate { get; private set; } = false;

        public int UpdateProgress
        {
            get => updateProgress;
            private set
            {
                updateProgress = value;
                if (OnUpdateProgressChanged != null)
                    OnUpdateProgressChanged(this, new EventArgs());
            }
        }
        private int updateProgress = 0;

        public event EventHandler OnUpdateRequired;
        public event EventHandler OnUpdateProgressChanged;
        public event EventHandler OnUpdateFinished;

        private string[] updateWebequestURLs = new string[]
            {
                "https://www.realmofthemadgod.com/app/init?platform=standalonewindows64&key=9KnJFxtTvLu2frXv",
                "https://rotmg-build.decagames.com/build-release/{0}/rotmg-exalt-win-64/checksum.json",
                "https://rotmg-build.decagames.com/build-release/{0}/rotmg-exalt-win-64/{1}.gz"
            };

        private string exePath = string.Empty;
        private string lastUpdateCheckPath = string.Empty;

        private string pathTemp = string.Empty;
        private string checksumJson = string.Empty;
        private string buildHash = string.Empty;
        private List<ChecksumFile> outdatedFiles;

        private bool checkInProgress = false;
        private bool updateInProgress = false;

        public bool CheckForUpdate()
        {
            if (_disposed || checkInProgress || updateInProgress) return false;
            Task.Run(() => UpdateCheck());
            return true;
        }

        public bool PerformGameUpdate()
        {
            if (_disposed || checkInProgress || updateInProgress) return false;
            Task.Run(() => UpdateGame());
            return true;
        }

        bool suppressUpdateRequiredEvent = false;
        private void UpdateCheck(bool _suppressUpdateRequiredEvent = false)
        {
            try
            {
                checkInProgress = true;
                suppressUpdateRequiredEvent = _suppressUpdateRequiredEvent;

                buildHash = PerformWebrequest(updateWebequestURLs[0]);
                int index = buildHash.IndexOf("<BuildHash>") + 11;
                buildHash = buildHash.Substring(index, ((buildHash.IndexOf("</BuildHash>") - index)));

                checksumJson = PerformWebrequest(string.Format(updateWebequestURLs[1], buildHash));

                ChecksumFiles checksumFile = ChecksumFiles.FromJson(checksumJson);
                outdatedFiles = new List<ChecksumFile>();

                string rootPath = Path.GetDirectoryName(exePath);
                if (Directory.Exists(rootPath))
                {
                    foreach (ChecksumFile f in checksumFile.Files)
                    {
                        if (string.IsNullOrWhiteSpace(f.FileName))
                            Console.WriteLine("Unnamed file found in list!");
                        else if (!File.Exists(Path.Combine(rootPath, f.FileName)) || !f.Checksum.Equals(GetMD5AsString(Path.Combine(rootPath, f.FileName))))
                            outdatedFiles.Add(f);
                    }

                    checkInProgress = false;
                    if (!suppressUpdateRequiredEvent)
                        UpdateRequired = outdatedFiles.Count > 0;
                }
                else
                {
                    Directory.CreateDirectory(rootPath);
                    outdatedFiles.AddRange(checksumFile.Files);

                    checkInProgress = false;
                    if (!suppressUpdateRequiredEvent)
                        UpdateRequired = true;
                }

                if (!suppressUpdateRequiredEvent)
                {
                    string[] rows = new string[] { DateTime.Now.Ticks.ToString(), UpdateRequired.ToString() };
                    if (File.Exists(lastUpdateCheckPath))
                        File.Delete(lastUpdateCheckPath);
                    File.WriteAllLines(lastUpdateCheckPath, rows);
                }
            }
            catch
            {
                if (!suppressUpdateRequiredEvent && OnUpdateRequired != null)
                    OnUpdateRequired(this, null);
            }
            finally
            {
                checkInProgress = false;
                suppressUpdateRequiredEvent = false;
            }
        }


        private void UpdateGame()
        {
            UpdateProgress = 0;
            updateInProgress = true;

            if (string.IsNullOrEmpty(buildHash) || string.IsNullOrEmpty(checksumJson) || outdatedFiles == null)
                UpdateCheck(true);

            UpdateProgress = 15;

            if (updateRequired && outdatedFiles.Count > 0)
            {
                pathTemp = Path.Combine(Path.GetTempPath(), "EAM_Update");
                if (Directory.Exists(pathTemp))
                    Directory.Delete(pathTemp, true);
                Directory.CreateDirectory(pathTemp);

                string rootPath = Path.GetDirectoryName(exePath);
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
                        UpdateProgress = (v >= 100 ? 100 : (int)v);
                    }
                }
                UpdateProgress = 100;
                UpdateRequired = false;
                updateProgress = 0;

                string[] rows = new string[] { DateTime.Now.Ticks.ToString(), updateRequired.ToString() };
                if (File.Exists(lastUpdateCheckPath))
                    File.Delete(lastUpdateCheckPath);
                File.WriteAllLines(lastUpdateCheckPath, rows);
                updateInProgress = false;

                if (OnUpdateRequired != null)
                    OnUpdateRequired(this, new EventArgs());

                if (OnUpdateFinished != null)
                    OnUpdateFinished(this, new EventArgs());
            }
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
    }

    public partial class ChecksumFiles
    {
        [JsonProperty("files")]
        public ChecksumFile[] Files { get; set; }

        public static ChecksumFiles FromJson(string json) => JsonConvert.DeserializeObject<ChecksumFiles>(json, Converter.Settings);
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

    public static class Serialize
    {
        public static string ToJson(this ChecksumFiles self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal }
            }
        };
    }
}
