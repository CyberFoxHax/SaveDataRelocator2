using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SaveDataRelocator2.Views
{
    public partial class GameSettingsView : UserControl {
        public GameSettingsView() {
            InitializeComponent();

            if(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false) {
                Filename.Text = "";
                SaveDataPath.Text = "";
                BackupDataPath.Text = "";
                ExecutablePath.Text = "";
            }
            Loaded += OnLoaded;
            ButtonSave.Click += ButtonSave_Click;
            CreateShortcut.Click += CreateShortcut_Click;
            ButtonBackupLocationBrowse.Click += ButtonBackupLocationBrowse_Click;
            ButtonExecutablePathBrowse.Click += ButtonExecutablePathBrowse_Click;
            ButtonSaveDataPathBrowse.Click += ButtonSaveDataPathBrowse_Click;
        }

        private void ButtonSaveDataPathBrowse_Click(object sender, RoutedEventArgs e) {
            var path = FileDialogHelper.CreateSaveDataDialog();
            if (path == null)
                return;


            var appdataPath = Environment.ExpandEnvironmentVariables("%appdata%");
            if (path.Contains(appdataPath))
                path = path.Replace(appdataPath, "%appdata%");

            var local = new DirectoryInfo(appdataPath + "\\..\\Local").FullName;
            if (path.Contains(local))
                path = path.Replace(local, "%appdata%\\..\\Local");

            var localLow = new DirectoryInfo(appdataPath + "\\..\\LocalLow").FullName;
            if (path.Contains(localLow))
                path = path.Replace(localLow, "%appdata%\\..\\LocalLow");

            var userprofilePath = Environment.ExpandEnvironmentVariables("%userprofile%");
            if (path.Contains(userprofilePath))
                path = path.Replace(userprofilePath, "%userprofile%");

            SaveDataPath.Text = path;
        }

        private void ButtonExecutablePathBrowse_Click(object sender, RoutedEventArgs e) {
            var path = FileDialogHelper.CreateExecutableDialog();
            if (path == null)
                return;

            var defaults = ConfigManager.LoadGlobalConfig();
            if (path == defaults.GamesDefaultPath)
                return;
            if (path.Contains(defaults.GamesDefaultPath))
                path = path.Replace(defaults.GamesDefaultPath+"\\", "");

            ExecutablePath.Text = path;
        }

        private void ButtonBackupLocationBrowse_Click(object sender, RoutedEventArgs e) {
            var path = FileDialogHelper.CreateBackupDialog();
            if (path == null)
                return;

            var defaults = ConfigManager.LoadGlobalConfig();
            if (path == defaults.BackupDefaultPath)
                return;
            if (path.Contains(defaults.BackupDefaultPath))
                path = path.Replace(defaults.BackupDefaultPath+"\\", "");

            BackupDataPath.Text = path;
        }

        private void CreateShortcut_Click(object sender, RoutedEventArgs e) {
            GuiToData();
            var shortcut = ShortcutCreator.CreateShortcutDialog(_dataModel);
            if (shortcut != null) {
                var recents = ConfigManager.LoadRecents();
                if (recents?.Paths.Contains(shortcut) == false) {
                    recents.Paths.Add(shortcut);
                }
                else
                    recents = new DataModels.RecentModel {
                        Paths = new System.Collections.Generic.List<string> {
                            shortcut
                        }
                    };
                ConfigManager.SaveRecents(recents);
            }
        }

        public event Action<DataModels.GameRelocationConfig> SaveClicked;
        private DataModels.GameRelocationConfig _dataModel;

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _dataModel = DataContext as DataModels.GameRelocationConfig;
            if (_dataModel == null)
                return;
            if (_dataModel.Filename == null)
                return;
            if(_dataModel.Filename != Path.GetFileName(_dataModel.ExecutablePath).Replace(".exe", ""))
                Filename.Text = _dataModel.Filename;
            SaveDataPath.Text = _dataModel.RemoteDirectory;
            BackupDataPath.Text = _dataModel.BackupDirectory;
            ExecutablePath.Text = _dataModel.ExecutablePath;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            if (Path.IsPathRooted(Environment.ExpandEnvironmentVariables(SaveDataPath.Text)) == false) {
                SaveDataPath.Text = "";
                return;
            }
            GuiToData();
            DataContext = _dataModel;
            ConfigManager.SaveGameConfig(_dataModel);
            SaveClicked?.Invoke(_dataModel);
        }

        private void GuiToData() {
            _dataModel.Filename = Filename.Text;
            _dataModel.RemoteDirectory = SaveDataPath.Text;
            _dataModel.BackupDirectory = BackupDataPath.Text;
            _dataModel.ExecutablePath = ExecutablePath.Text;
            if (string.IsNullOrEmpty(_dataModel.Filename))
                _dataModel.Filename = Path.GetFileName(_dataModel.ExecutablePath).Replace(".exe", "");
        }
    }
}
