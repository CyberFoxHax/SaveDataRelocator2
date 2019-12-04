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

        private const string _remoteDirectory = "RemoteDirectory";
        private const string _backupDirectory = "BackupDirectory";
        private const string _gameName = "MyTestGame";

        [TestMethod]
        public void CleanFolders() {
            if(Directory.Exists(_remoteDirectory))
                Directory.Delete(_remoteDirectory, true);
            if(Directory.Exists(_backupDirectory))
                Directory.Delete(_backupDirectory, true);
            if (Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)))
                Directory.Delete(Path.Combine(_remoteDirectory, "_" + _gameName), true);

            Assert.IsTrue(Directory.Exists(_remoteDirectory) == false);
            Assert.IsTrue(Directory.Exists(_backupDirectory) == false);
            Assert.IsTrue(Directory.Exists(Path.Combine(_remoteDirectory, "_" + _gameName)) == false);
        }

        public static void LaunchEmptyApplication() {
            //var process = System.Diagnostics.Process.Start("ping", "localhost");
            //process.WaitForExit();
            // intentionally no checks, like a real application would.
            Directory.CreateDirectory(_remoteDirectory);
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
        }

        [TestMethod]
        public void NoDataStartup() {
            CleanFolders();
            var config = new GameRelocationConfig { // check with broken backshlashes
                BackupDirectory = Path.Combine(_backupDirectory, _gameName),
                RemoteDirectory = Path.Combine(_remoteDirectory, _gameName)
            };
            SaveDataRelocator2.Relocator.CopyBackupToRemote(config);
            LaunchEmptyApplication();
            SaveDataRelocator2.Relocator.CopyRemoteToBackup(config);
            CleanFolders();
        }

        [TestMethod]
        public void ExistingBackupStartup() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName, "Empty"));
            var config = new GameRelocationConfig { // check with broken backshlashes
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
        public void ExistingRemoteStartup() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName, "Empty"));
            var config = new GameRelocationConfig { // check with broken backshlashes
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
        public void ExistingBackupAndRemoteStartup() {
            CleanFolders();

            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName, "Empty"));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName, "Empty"));
            var config = new GameRelocationConfig { // check with broken backshlashes
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
        public void ExistingRemoteStartupAltPath() {
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
        public void ExistingTempRemoteStartup() {
            CleanFolders();
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, "_" + _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, "_" + _gameName, "Empty"));
            Directory.CreateDirectory(Path.Combine(_backupDirectory, _gameName));
            Directory.CreateDirectory(Path.Combine(_remoteDirectory, _gameName));
            var config = new GameRelocationConfig { // check with broken backshlashes
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
    }
}
