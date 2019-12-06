using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;

namespace SaveDataRelocator2 {
    public class FileDialogHelper {

        private static readonly string specialFolderPath = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..");
        private static readonly string userProfilePath = Environment.ExpandEnvironmentVariables("%userprofile%");
        private static readonly string documentsPath = Path.Combine(userProfilePath, "Documents");
        public const string MyComputerCSIDPath = "::{20d04fe0-3aea-1069-a2d8-08002b30309d}";

        public static readonly string[] SaveDataCommonLocations = {
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            specialFolderPath,
            Path.Combine(specialFolderPath, "Roaming"),
            Path.Combine(specialFolderPath, "Local"),
            Path.Combine(specialFolderPath, "LocalLow"),
            documentsPath,
            Path.Combine(documentsPath, "My Games"),
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games"),
            Path.Combine(userProfilePath, "Saved Games")
        };

        public static string CreateSaveDataDialog() {
            var dialog = new CommonOpenFileDialog {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Multiselect = false,
                IsFolderPicker = true,
                Title = "Pick a save data folder",
            };

            Action<string> addPlace = p => {
                if (Directory.Exists(p))
                    dialog.AddPlace(p, Microsoft.WindowsAPICodePack.Shell.FileDialogAddPlaceLocation.Top);
            };

            foreach (var item in SaveDataCommonLocations)
                addPlace(item);

            string dialogResult;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                dialogResult = dialog.FileName;
            else
                return null;

            return dialogResult;
        }

        public static string CreateExecutableDialog() {
            var dialog = new CommonOpenFileDialog {
                InitialDirectory = MyComputerCSIDPath,
                Multiselect = false,
                IsFolderPicker = false,
                Filters = {
                    new CommonFileDialogFilter {
                        DisplayName = "Executable",
                        Extensions = { "exe" }
                    }
                },
                Title = "Pick a Game executable",
            };

            string dialogResult;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                dialogResult = dialog.FileName;
            else
                return null;

            return dialogResult;
        }

        public static string CreateBackupDialog() {
            var dialog = new CommonOpenFileDialog {
                InitialDirectory = MyComputerCSIDPath,
                Multiselect = false,
                IsFolderPicker = true,
                Title = "Pick a Backup Folder",
            };

            string dialogResult;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                dialogResult = dialog.FileName;
            else
                return null;

            return dialogResult;
        }

        public static string CreateDialog(string title, bool isFolder = true, string initialPath=MyComputerCSIDPath) {
            var dialog = new CommonOpenFileDialog {
                InitialDirectory = initialPath,
                Multiselect = false,
                IsFolderPicker = isFolder,
                Title = title,
            };

            string dialogResult;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                dialogResult = dialog.FileName;
            else
                return null;

            return dialogResult;
        }
    }
}
