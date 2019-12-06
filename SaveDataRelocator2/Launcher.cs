using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SaveDataRelocator2
{
    public class Launcher {
        public static void Launch(string configFilename) {
            var config = ConfigManager.LoadGameConfigFromName(configFilename);
            if (config == null) {
                System.Windows.MessageBox.Show("Config \""+configFilename+"\" was not found", "SaveDataRelocator2");
                return;
            }
            var exec = config.ExecutablePath;
            var appConfig = ConfigManager.LoadGlobalConfig();
            if(appConfig?.GamesDefaultPath != null && Path.IsPathRooted(exec) == false)
                exec = Path.Combine(appConfig.GamesDefaultPath, exec);

            if (File.Exists(exec) == false) {
                System.Windows.MessageBox.Show("Executable \"" + exec + "\" was not found", "SaveDataRelocator2");
                return;
            }

            if (Relocator.CopyBackupToRemote(config) == false)
                return;
            var localDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var process = new Process {
                StartInfo = {
                    FileName = exec,
                    WorkingDirectory = localDir,
                    Domain = localDir,
                    UseShellExecute = true
                }
            };
            process.Start();
            process.WaitForExit();
            if (Relocator.CopyRemoteToBackup(config) == false)
                return;
        }
    }
}