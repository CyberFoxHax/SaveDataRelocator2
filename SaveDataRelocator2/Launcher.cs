using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SaveDataRelocator2
{
    public class Launcher {
        public static void Launch(string configFilename) {
            var config = ConfigManager.LoadGameConfigFromName(configFilename);
            if (config == null) {
                Console.WriteLine("Configuration: \"" + configFilename + "\" was not found");
                Console.ReadLine();
                return;
            }
            Relocator.CopyBackupToRemote(config);
            var localDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var process = new Process {
                StartInfo = {
                    FileName = config.ExecutablePath,
                    WorkingDirectory = localDir,
                    Domain = localDir,
                    UseShellExecute = true
                }
            };
            process.Start();
            process.WaitForExit();
            Relocator.CopyRemoteToBackup(config);
        }
    }
}