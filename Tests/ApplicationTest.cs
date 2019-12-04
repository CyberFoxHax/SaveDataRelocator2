using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaveDataRelocator2;
using System.IO;

namespace Tests {
    [TestClass]
    public class ApplicationTest {

        private static void CleanFolders(string remoteDir, string backupDir) {
            if (Directory.Exists(remoteDir))
                Directory.Delete(remoteDir, true);
            if (Directory.Exists(backupDir))
                Directory.Delete(backupDir, true);

            Assert.IsTrue(Directory.Exists(backupDir) == false);
            Assert.IsTrue(Directory.Exists(remoteDir) == false);
        }

        [TestMethod]
        public void TestSimple() { 
            ConfigManager.SaveAppConfig(new SaveDataRelocator2.DataModels.ApplicationConfig {
                BackupDataPath = "C:\\MySharedGameData"
            });
            var config = new SaveDataRelocator2.DataModels.GameRelocationConfig {
                BackupDirectory = "C:\\MySharedGameData\\TestApplication",
                ExecutablePath = "..\\..\\..\\TestApplication\\bin\\Debug\\TestApplication.exe",
                RemoteDirectory = "%appdata%\\TestApplication",
                Filename = "TestApplication"
            };
            ConfigManager.SaveGameConfig(config);
            var remoteDir = System.Environment.ExpandEnvironmentVariables(config.RemoteDirectory);
            var backupDir = System.Environment.ExpandEnvironmentVariables(config.BackupDirectory);
            CleanFolders(remoteDir, backupDir);
            CommandLineEntry.Entry(new[] { "SaveDataRelocator2.exe", "-launch", "TestApplication" });
            Assert.IsTrue(Directory.Exists(remoteDir) == false);
            Assert.IsTrue(Directory.Exists(backupDir));
            CleanFolders(remoteDir, backupDir);
        }

        [TestMethod]
        public void TestSimpleRemoteExists() {
            ConfigManager.SaveAppConfig(new SaveDataRelocator2.DataModels.ApplicationConfig {
                BackupDataPath = "C:\\MySharedGameData"
            });
            var config = new SaveDataRelocator2.DataModels.GameRelocationConfig {
                BackupDirectory = "C:\\MySharedGameData\\TestApplication",
                ExecutablePath = "..\\..\\..\\TestApplication\\bin\\Debug\\TestApplication.exe",
                RemoteDirectory = "%appdata%\\TestApplication",
                Filename = "TestApplication"
            };
            ConfigManager.SaveGameConfig(config);
            var remoteDir = System.Environment.ExpandEnvironmentVariables(config.RemoteDirectory);
            var backupDir = System.Environment.ExpandEnvironmentVariables(config.BackupDirectory);
            CleanFolders(remoteDir, backupDir);
            Directory.CreateDirectory(remoteDir);
            CommandLineEntry.Entry(new[] { "SaveDataRelocator2.exe", "-launch", "TestApplication" });
            Assert.IsTrue(Directory.Exists(remoteDir));
            Assert.IsTrue(Directory.Exists(backupDir));
            CleanFolders(remoteDir, backupDir);
        }

        [TestMethod]
        public void TestSimpleBackupExists() {
            ConfigManager.SaveAppConfig(new SaveDataRelocator2.DataModels.ApplicationConfig {
                BackupDataPath = "C:\\MySharedGameData"
            });
            var config = new SaveDataRelocator2.DataModels.GameRelocationConfig {
                BackupDirectory = "C:\\MySharedGameData\\TestApplication",
                ExecutablePath = "..\\..\\..\\TestApplication\\bin\\Debug\\TestApplication.exe",
                RemoteDirectory = "%appdata%\\TestApplication",
                Filename = "TestApplication"
            };
            ConfigManager.SaveGameConfig(config);
            var remoteDir = System.Environment.ExpandEnvironmentVariables(config.RemoteDirectory);
            var backupDir = System.Environment.ExpandEnvironmentVariables(config.BackupDirectory);
            CleanFolders(remoteDir, backupDir);
            Directory.CreateDirectory("C:\\MySharedGameData");
            CommandLineEntry.Entry(new[] { "SaveDataRelocator2.exe", "-launch", "TestApplication" });
            Assert.IsTrue(Directory.Exists(remoteDir) == false);
            Assert.IsTrue(Directory.Exists(backupDir));
            CleanFolders(remoteDir, backupDir);
        }
    }
}
