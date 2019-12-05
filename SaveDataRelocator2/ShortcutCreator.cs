using IWshRuntimeLibrary;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using SaveDataRelocator2.DataModels;
using System;
using System.IO;
using System.Reflection;

namespace SaveDataRelocator2 {
    public static class ShortcutCreator {

        public static string CreateShortcutDialog(GameRelocationConfig dataModel) {
            var dialog = new CommonOpenFileDialog {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Multiselect = false,
                IsFolderPicker = true,
                Title = "Pick a folder",
            };

            string dialogFileName;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                dialogFileName = dialog.FileName;
            else
                return null;

            var shortcutPath = Path.Combine(dialogFileName, dataModel.Filename) + ".lnk";
            CreateShortcut(shortcutPath, dataModel);
            return shortcutPath;
        }

        public static void CreateShortcut(string shortcutPath, GameRelocationConfig dataModel) {
            if (System.IO.File.Exists(shortcutPath))
                System.IO.File.Delete(shortcutPath);
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.IconLocation = dataModel.ExecutablePath;
            shortcut.Description = "SaveDataRelocator2 shortcut";
            shortcut.TargetPath = Assembly.GetExecutingAssembly().Location;
            shortcut.WorkingDirectory = Directory.GetParent(shortcut.TargetPath).FullName;
            shortcut.Arguments = "-launch " + dataModel.Filename;
            shortcut.Save();
        }
    }
}
