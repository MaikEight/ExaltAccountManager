using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MK_EAM_Updater_Lib
{
    public static class Updater
    {
        private static string logPath = string.Empty;
        public static string tempFilePath { get; internal set; } = Path.Combine(Path.GetTempPath(), "ExaltAccountManager");
        private static string appPath = string.Empty;

        /// <summary>
        /// Perform an Update of EAM.
        /// </summary>
        /// <param name="applicationPath">Path to the current EAM-Installation.</param>
        /// <param name="latestVersionDownloadLink">Link to the .zip file of the new version.</param>
        /// <returns>True if a restart of the update-process is needed,</returns>
        public static bool PerformUpdate(string applicationPath, string latestVersionDownloadLink)
        {
            appPath = applicationPath;

            Log("Starting update process...");
            Log("Application path: " + applicationPath);
            Log("Latest version download link: " + latestVersionDownloadLink);

            DownloadLatestVersion(latestVersionDownloadLink);

            bool restartRequired = MoveUpdateFiles(applicationPath, tempFilePath);

            return restartRequired;
        }

        public static bool MoveUpdateFiles(string applicationPath, string updatePath)
        {
            #region Moving files

            Log("Moving update files...");

            List<string> failedFiles = new List<string>();

            if (string.IsNullOrEmpty(updatePath) || !Directory.Exists(updatePath))
            {
                return false;
            }

            string mainUpdateFileFolder = Directory.GetDirectories(updatePath).Where(d => d.Replace(updatePath, "").TrimStart('\\').StartsWith("ExaltAccountManager")).FirstOrDefault();
            
            if (string.IsNullOrEmpty(mainUpdateFileFolder))
            {
                return false;
            }
            
            string[] files = Directory.GetFiles(updatePath, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                try
                {
                    string relativePath = file.Replace(mainUpdateFileFolder, "");
                    relativePath = relativePath.TrimStart('\\');
                    string destinationPath = Path.Combine(applicationPath, relativePath);
                    if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                    File.Copy(file, destinationPath, true);
                }
                catch 
                {
                    Log("Failed to move file: " + file);
                    failedFiles.Add(file);
                }
            }

            #endregion

            #region Delete update files

            Log("Deleting update files...");

            foreach (string file in files)
            {
                try
                {
                    if (!failedFiles.Contains(file))
                    {
                        File.Delete(file);
                    }
                }
                catch
                {
                    Log("Failed to delete file: " + file);
                }
            }

            #endregion

            if (failedFiles.Count == 0)
            {
                return CleanupTemp(updatePath);
            }
            return false;
        }

        public static bool CleanupTemp(string tempPath)
        {
            try
            {
                Directory.Delete(tempPath, true);
                return true;
            }
            catch (Exception e)
            {
                Log("Failed to delete temp path: " + tempPath);
                Log(e.ToString());
            }
            return false;
        }

        private static void CreateTempFileFolder()
        {
            Log("Creating temp file folder...");

            if (Directory.Exists(tempFilePath))
            {
                Log("Deleting old temp files...");
                try
                {
                    Directory.Delete(tempFilePath, true);
                }
                catch (Exception e)
                {
                    Log("Failed to delete old temp files...");
                    Log(e.ToString());

                    tempFilePath = Path.Combine(Path.GetTempPath(), "ExaltAccountManager_Update_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                }
            }

            Directory.CreateDirectory(tempFilePath);
        }

        private static void DownloadLatestVersion(string latestVersionDownloadLink)
        {
            CreateTempFileFolder();

            Log("Downloading latest version...");

            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    string filePath = Path.Combine(tempFilePath, "EAM.zip");

                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    client.DownloadFile(latestVersionDownloadLink, filePath);
                    if (File.Exists(filePath))
                    {
                        System.IO.Compression.ZipFile.ExtractToDirectory(filePath, tempFilePath);
                        File.Delete(filePath);
                    }
                    else
                    {
                        throw new FileNotFoundException("Failed to find the local update files...");
                    }
                }
            }
            catch (Exception e)
            {
                Log("Failed to download latest version, see reason below.");
                Log(e.ToString());
                Log("Aborting update...");

                if (MessageBox.Show("Failed to download latest version, see log file for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    try
                    {
                        Process.Start("notepad.exe", logPath);
                    }
                    catch { }
                }
                Environment.Exit(1);
            }

        }

        private static void Log(string message)
        {
            if (logPath == null || string.IsNullOrEmpty(logPath))
            {
                logPath = Path.Combine(appPath, $"_updatelog_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log");
                if (File.Exists(logPath))
                    File.Delete(logPath);
            }

            string logMessage = string.Format("[{0}] {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message, Environment.NewLine);
            File.AppendAllText(logPath, logMessage);
        }
    }
}
