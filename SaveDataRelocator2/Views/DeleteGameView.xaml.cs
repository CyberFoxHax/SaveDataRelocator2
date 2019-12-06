using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SaveDataRelocator2.Views
{
    public partial class DeleteGameView : UserControl {
        public DeleteGameView() {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false) {
                Filename.Text = "";
                SaveDataPath.Text = "";
                BackupDataPath.Text = "";
                ExecutablePath.Text = "";
            }
            Loaded += OnLoaded;
            ButtonDelete.Click += ButtonDelete_Click;
            ButtonDeleteShortcut.Click += ButtonDeleteShortcut_Click;
            ButtonOpenFolder.Click += ButtonOpenFolder_Click;
        }

        public event Action<DeleteGameView> ViewCompleted;

        private DataModels.GameRelocationConfig _dataModel;
        private string _detectedShortcutPath;

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _dataModel = DataContext as DataModels.GameRelocationConfig;
            if (_dataModel == null)
                return;
            Filename.Text = _dataModel.Filename;
            SaveDataPath.Text = _dataModel.RemoteDirectory;
            BackupDataPath.Text = _dataModel.BackupDirectory;
            ExecutablePath.Text = _dataModel.ExecutablePath;

            ShortcutWrapper.Visibility = Visibility.Collapsed;
            var recents = ConfigManager.LoadRecents();
            var missing = new List<string>();
            foreach (var shortcutPath in recents.Paths) {
                if (System.IO.File.Exists(shortcutPath) == false) {
                    missing.Add(shortcutPath);
                    continue;
                }
                var shell = new IWshRuntimeLibrary.WshShell();
                var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
                if (shortcut.Arguments == "-launch " + _dataModel.Filename) {
                    ShortcutWrapper.Visibility = Visibility.Visible;
                    _detectedShortcutPath = shortcutPath;
                    break;
                }
            }
            foreach (var item in missing)
                recents.Paths.Remove(item);
            ConfigManager.SaveRecents(recents);
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e) {
            _dataModel.Filename = Filename.Text;
            _dataModel.RemoteDirectory = SaveDataPath.Text;
            _dataModel.BackupDirectory = BackupDataPath.Text;
            _dataModel.ExecutablePath = ExecutablePath.Text;
            DataContext = _dataModel;

            ConfigManager.DeleteGameConfig(_dataModel);

            ViewCompleted?.Invoke(this);
        }

        private void ButtonOpenFolder_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", "/select,\""+ _detectedShortcutPath + "\"");
        }

        private void ButtonDeleteShortcut_Click(object sender, RoutedEventArgs e) {
            ShortcutWrapper.Visibility = Visibility.Collapsed;

            var recents = ConfigManager.LoadRecents();
            recents.Paths.Remove(_detectedShortcutPath);
            ConfigManager.SaveRecents(recents);

            if (File.Exists(_detectedShortcutPath))
                File.Delete(_detectedShortcutPath);
        }
    }
}
