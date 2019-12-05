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
            if (Directory.Exists("C:\\TestMySharedGameData"))
                Directory.Delete("C:\\TestMySharedGameData", true);

            Assert.IsTrue(Directory.Exists(backupDir) == false);
            Assert.IsTrue(Directory.Exists(remoteDir) == false);
            Assert.IsTrue(Directory.Exists("C:\\TestMySharedGameData") == false);
        }

        [TestMethod]
        public void TestSimple() { 
            ConfigManager.SaveGlobalConfig(new SaveDataRelocator2.DataModels.ApplicationConfig {
                BackupDefaultPath = "C:\\TestMySharedGameData"
            });
            var currentPath = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            var config = new SaveDataRelocator2.DataModels.GameRelocationConfig {
                BackupDirectory = "C:\\TestMySharedGameData\\TestApplication",
                ExecutablePath = Path.Combine(currentPath, "..\\..\\..\\TestApplication\\bin\\Debug\\TestApplication.exe"),
                RemoteDirectory = "%appdata%\\TestApplication",
                Filename = "TestApplication"
            };
            ConfigManager.Initialize();
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
            ConfigManager.SaveGlobalConfig(new SaveDataRelocator2.DataModels.ApplicationConfig {
                BackupDefaultPath = "C:\\TestMySharedGameData"
            });
            var config = new SaveDataRelocator2.DataModels.GameRelocationConfig {
                BackupDirectory = "C:\\TestMySharedGameData\\TestApplication",
                ExecutablePath = "..\\..\\..\\TestApplication\\bin\\Debug\\TestApplication.exe",
                RemoteDirectory = "%appdata%\\TestApplication",
                Filename = "TestApplication"
            };
            ConfigManager.Initialize();
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
            ConfigManager.SaveGlobalConfig(new SaveDataRelocator2.DataModels.ApplicationConfig {
                BackupDefaultPath = "C:\\TestMySharedGameData"
            });
            var config = new SaveDataRelocator2.DataModels.GameRelocationConfig {
                BackupDirectory = "C:\\TestMySharedGameData\\TestApplication",
                ExecutablePath = "..\\..\\..\\TestApplication\\bin\\Debug\\TestApplication.exe",
                RemoteDirectory = "%appdata%\\TestApplication",
                Filename = "TestApplication"
            };
            ConfigManager.Initialize();
            ConfigManager.SaveGameConfig(config);
            var remoteDir = System.Environment.ExpandEnvironmentVariables(config.RemoteDirectory);
            var backupDir = System.Environment.ExpandEnvironmentVariables(config.BackupDirectory);
            CleanFolders(remoteDir, backupDir);
            Directory.CreateDirectory("C:\\TestMySharedGameData");
            CommandLineEntry.Entry(new[] { "SaveDataRelocator2.exe", "-launch", "TestApplication" });
            Assert.IsTrue(Directory.Exists(remoteDir) == false);
            Assert.IsTrue(Directory.Exists(backupDir));
            CleanFolders(remoteDir, backupDir);
        }
    }
}
