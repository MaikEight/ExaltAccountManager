using System;
using System.Collections.Generic;
using System.Linq;
using WixSharp;
using WixSharp.Controls;

namespace EAM_Installer
{
    internal sealed class Program
    {
        private const string productName = "Exalt Account Manager";
        private static string directoryPath = System.IO.Path.Combine("%ProgramFiles%", "Maik8", "ExaltAccountManager");
        private const string eamFiles = "..\\_Releases\\ExaltAccountManager_PRE_V3_1_0_R4";

        static void Main()
        {
            string[] allDirs = System.IO.Directory.GetDirectories(eamFiles, "*", System.IO.SearchOption.AllDirectories);
            List<WixEntity> dirs = new List<WixEntity>();
            dirs.Add(new DirFiles(eamFiles + @"\*.*"));

            foreach (string d in allDirs)
            {
                string target = d.Substring(eamFiles.Length).TrimStart('\\');
                dirs.Add(new Dir(target, new DirFiles(d + @"\*.*")));
            }

            Dir dir = new Dir(directoryPath,
                            dirs.ToArray());

            Project project = new Project(
            productName,
                    dir
                )
            {
                ProductId = Guid.NewGuid(),
                UpgradeCode = new Guid("45414D20-0000-6279-0000-204D61696B38"),
                Platform = Platform.x64,
                UI = WUI.WixUI_InstallDir,
            };

            // Defines the custom installation UI.
            project.CustomUI = new DialogSequence()
                                   .On(NativeDialogs.WelcomeDlg, Buttons.Next, new ShowDialog(NativeDialogs.InstallDirDlg))
                                   .On(NativeDialogs.InstallDirDlg, Buttons.Back, new ShowDialog(NativeDialogs.WelcomeDlg))
                                   .On(NativeDialogs.InstallDirDlg, Buttons.Next, new ShowDialog(NativeDialogs.VerifyReadyDlg))
                                   .On(NativeDialogs.VerifyReadyDlg, Buttons.Back, new ShowDialog(NativeDialogs.InstallDirDlg));

            // Defines logic to implement upgrades
            project.MajorUpgrade = new MajorUpgrade()
            {
                Schedule = UpgradeSchedule.afterInstallInitialize,                
                AllowDowngrades = true,
            };

            // Generate shortcuts
            project.ResolveWildCards()
               .FindFile(f => f.Name.EndsWith("ExaltAccountManager.exe"))
               .First()
               .Shortcuts = new[] {
                    new FileShortcut(productName, "%Desktop%"),
                    new FileShortcut(productName, "%ProgramMenu%")
                };

            // Builds your MSI.
            Compiler.BuildMsi(project);
        }       
    }
}