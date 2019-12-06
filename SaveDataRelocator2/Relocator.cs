using System.IO;

namespace SaveDataRelocator2
{
    public static class Relocator {

        public static bool CopyBackupToRemote(DataModels.GameRelocationConfig config) {
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
            try {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
            catch {
                if (Directory.Exists(remoteDir)) {
                    System.Windows.MessageBox.Show("Unable to delete temp directory, Unable to continue", "SaveDataRelocator2");
                    return false;
                }
            }

            try {
                if (Directory.Exists(remoteDir)) 
                    Directory.Move(
                        remoteDir,
                        tempDir
                    );
            }
            catch {
                System.Windows.MessageBox.Show("Remote directory already exists and is in use. Unable to continue.", "SaveDataRelocator2");
                return false;
            }

            if(Directory.Exists(backupDir))
                DirectoryUtils.DirectoryCopy(backupDir, remoteDir);
            return true;
        }

        public static bool CopyRemoteToBackup(DataModels.GameRelocationConfig config) {
            var remoteDir = System.Environment.ExpandEnvironmentVariables(config.RemoteDirectory);
            var backupDir = System.Environment.ExpandEnvironmentVariables(config.BackupDirectory);
            var tempPath = GetTempDirectoryPath(remoteDir);

            var appConfig = ConfigManager.LoadGlobalConfig();
            if (Path.IsPathRooted(backupDir) == false) {
                if (appConfig?.BackupDefaultPath != null)
                    backupDir = Path.Combine(appConfig.BackupDefaultPath, backupDir);
                else
                    throw new System.Exception("Relative path falls back to default config. Which is missing.");
            }
            if (Directory.Exists(remoteDir) == false) {
                System.Windows.MessageBox.Show("Remote data is still absent after execution has completed.\nYou've most likely typed the wrong \"Save Data Path\" for \"" + config.Filename + "\"", "SaveDataRelocator2");
                if (Directory.Exists(tempPath))
                    Directory.Move(
                        tempPath,
                        remoteDir
                    );
                return false;
            }

            try {
                if (Directory.Exists(backupDir))
                    Directory.Delete(backupDir, true);
            }
            catch {
                System.Windows.MessageBox.Show("Unable to delete the backup folder.\nRemote data was not backed up, please resolve this by yourself", "SaveDataRelocator2");
                System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + backupDir + "\"");
                System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + remoteDir + "\"");
                return false;
            }
            

            DirectoryUtils.DirectoryCopy(remoteDir, backupDir);
            Directory.Delete(remoteDir, true);
            if (Directory.Exists(tempPath))
                Directory.Move(
                    tempPath,
                    remoteDir
                );
            return true;
        }

        private static string GetTempDirectoryPath(string original) {
            var info = new DirectoryInfo(original);
            return Path.Combine(
                info.Parent.FullName,
                "_" + info.Name
            );
        }

    }
}
