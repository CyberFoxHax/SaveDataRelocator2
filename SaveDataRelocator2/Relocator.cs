using System.IO;

namespace SaveDataRelocator2
{
    public static class Relocator {

        private static string GetTempDirectoryPath(string original) {
            var info = new DirectoryInfo(original);
            return Path.Combine(
                info.Parent.FullName,
                "_" + info.Name
            );
        }

        public static void CopyBackupToRemote(DataModels.GameRelocationConfig config) {
            var remoteDir = System.Environment.ExpandEnvironmentVariables(config.RemoteDirectory);
            var backupDir = System.Environment.ExpandEnvironmentVariables(config.BackupDirectory);

            var appConfig = ConfigManager.LoadGlobalConfig();
            if (Path.IsPathRooted(backupDir) == false) {
                if (appConfig?.BackupDefaultPath != null)
                    backupDir = Path.Combine(appConfig.BackupDefaultPath, backupDir);
                else
                    throw new System.Exception("Relative path falls back to default config. Which is missing.");
            }

            var tempDir = GetTempDirectoryPath(remoteDir);
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);

            if (Directory.Exists(remoteDir)) 
                Directory.Move(
                    remoteDir,
                    tempDir
                );

            if(Directory.Exists(backupDir))
                DirectoryUtils.DirectoryCopy(backupDir, remoteDir);
        }

        public static void CopyRemoteToBackup(DataModels.GameRelocationConfig config) {
            var remoteDir = System.Environment.ExpandEnvironmentVariables(config.RemoteDirectory);
            var backupDir = System.Environment.ExpandEnvironmentVariables(config.BackupDirectory);

            var appConfig = ConfigManager.LoadGlobalConfig();
            if (Path.IsPathRooted(backupDir) == false) {
                if (appConfig?.BackupDefaultPath != null)
                    backupDir = Path.Combine(appConfig.BackupDefaultPath, backupDir);
                else
                    throw new System.Exception("Relative path falls back to default config. Which is missing.");
            }
            if (Directory.Exists(remoteDir) == false) {
                System.Windows.MessageBox.Show("Remote data is still absent after execution has completed.\nYou've most likely typed the wrong \"Save Data Path\" for \"" + config.Filename + "\"", "SaveDataRelocator2");
                return;
            }

            if (Directory.Exists(backupDir))
                Directory.Delete(backupDir, true);

            DirectoryUtils.DirectoryCopy(remoteDir, backupDir);
            Directory.Delete(remoteDir, true);
            var tempPath = GetTempDirectoryPath(remoteDir);
            if (Directory.Exists(tempPath)) {
                Directory.Move(
                    tempPath,
                    remoteDir
                );
            }
        }

    }
}
