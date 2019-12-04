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
