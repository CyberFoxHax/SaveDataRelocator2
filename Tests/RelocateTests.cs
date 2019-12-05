using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaveDataRelocator2.DataModels;

namespace Tests
{
    [TestClass]
    public class RelocateTests {
        /*
        start no data
        start with existing backup data
        start with existing remote data
        start with existing remote and backup data

        end with backup deta deleted then rmote data written in place, then remote data deleted
        */

        private const string _remoteDirectory = "C:\\TestRemoteDirectory";
        private const string _backupDirectory = "C:\\TestBackupDirectory";
        private const string _gameName = "MyTestGame";

        [TestMethod]
        public void CleanFolders() {
            if (Directory.Exists(_remoteDirectory))
                Directory.Delete(_remoteDirectory, true);
            if(Directory.Exists(_backupDirectory))
                Directory.Delete(_backupDirectory, true);
            if (Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)))
                Directory.Delete(Path.Combine(_remoteDirectory, "_" + _gameName), true);
            if (Directory.Exists(SaveDataRelocator2.ConfigManager.ConfigFolder))
                Directory.Delete(SaveDataRelocator2.ConfigManager.ConfigFolder, true);
            if (File.Exists(SaveDataRelocator2.ConfigManager.RecentsData))
                File.Delete(SaveDataRelocator2.ConfigManager.RecentsData);
            if (File.Exists(SaveDataRelocator2.ConfigManager.GlobalConfig))
                File.Delete(SaveDataRelocator2.ConfigManager.GlobalConfig);

            Assert.IsTrue(Directory.Exists(SaveDataRelocator2.ConfigManager.ConfigFolder) == false);
            Assert.IsTrue(File.Exists(SaveDataRelocator2.ConfigManager.RecentsData) == false);
            Assert.IsTrue(File.Exists(SaveDataRelocator2.ConfigManager.GlobalConfig) == false);
            Assert.IsTrue(Directory.Exists(_backupDirectory) == false);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)) == false);
        }

        public static void LaunchEmptyApplication(bool insertDelay = false) {
            if (insertDelay) {
                var process = System.Diagnostics.Process.Start("ping", "localhost");
                process.WaitForExit();
            }
            Directory.CreateDirectory(_remoteDirectory);
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
        }

        [TestMethod]
        public void NoData() {
            CleanFolders();
            var config = new GameRelocationConfig {
                BackupDirectory = Path.Combine(_backupDirectory, _gameName),
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName)
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            CleanFolders();
        }

        [TestMethod]
        public void ExistingBackup() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName, "Empty"));
            var config = new GameRelocationConfig {
                BackupDirectory = Path.Combine(_backupDirectory, _gameName),
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName)
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, _gameName)) == false);
            Assert.IsTrue(Directory.Exists(Path.Combine(_backupDirectory, _gameName)));
            CleanFolders();
        }

        [TestMethod]
        public void ExistingRemote() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName, "Empty"));
            var config = new GameRelocationConfig {
                BackupDirectory = Path.Combine(_backupDirectory, _gameName),
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName)
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_"+_gameName)));
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)) == false);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, _gameName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(_backupDirectory, _gameName)));
            CleanFolders();
        }

        [TestMethod]
        public void ExistingBackupAndRemote() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName, "Empty"));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName, "Empty"));
            var config = new GameRelocationConfig {
                BackupDirectory = Path.Combine(_backupDirectory, _gameName),
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName)
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)));
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)) == false);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, _gameName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(_backupDirectory, _gameName)));
            CleanFolders();
        }

        [TestMethod]
        public void ExistingRemoteAltPath() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
            var config = new GameRelocationConfig { // check with broken backshlashes
                BackupDirectory = Path.Combine(_backupDirectory, _gameName) + "\\",
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName) + "\\"
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)));
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)) == false);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, _gameName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(_backupDirectory, _gameName)));
            CleanFolders();
        }

        [TestMethod]
        public void ExistingTempRemote() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, "_" + _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, "_" + _gameName, "Empty"));
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
            var config = new GameRelocationConfig {
                BackupDirectory = Path.Combine(_backupDirectory, _gameName),
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName)
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)));
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)) == false);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, _gameName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(_backupDirectory, _gameName)));
            CleanFolders();
        }

        [TestMethod]
        public void RelativeBackupFolder() {
            CleanFolders();
            SaveDataRelocator2.ConfigManager.SaveGlobalConfig(new ApplicationConfig {
                BackupDefaultPath = _backupDirectory
            });
            var config = new GameRelocationConfig {
                BackupDirectory = _gameName,
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName)
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            CleanFolders();
        }
    }
}
