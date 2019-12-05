using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SaveDataRelocator2.Views
{
    public partial class ConfigView : UserControl {
        public ConfigView() {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            _dataModel = ConfigManager.LoadGlobalConfig() ?? new DataModels.ApplicationConfig();
            BackupPath.Text = "";
            Loaded += OnLoaded;
            ButtonSave.Click += ButtonSave_Click;
            ButtonBackupPathBrowse.Click += ButtonBackupPathBrowse_Click;
            ButtonGamesPathBrowse.Click += ButtonGamesPathBrowse_Click;
        }

        private void ButtonGamesPathBrowse_Click(object sender, RoutedEventArgs e) {
            var path = FileDialogHelper.CreateDialog("Select Default Games Directory", true);
            if(path != null && Path.IsPathRooted(path))
                GamesPath.Text = path;
        }

        private void ButtonBackupPathBrowse_Click(object sender, RoutedEventArgs e) {
            var path = FileDialogHelper.CreateDialog("Select Default Backup Directory", true);
            if (path != null && Path.IsPathRooted(path))
                BackupPath.Text = path;
        }

        public event Action<DataModels.ApplicationConfig> SaveClicked;
        private DataModels.ApplicationConfig _dataModel;

        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (_dataModel == null)
                return;
            BackupPath.Text = _dataModel.BackupDefaultPath;
            GamesPath.Text = _dataModel.GamesDefaultPath;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            var error = false;
            if (Path.IsPathRooted(BackupPath.Text) == false) {
                BackupPath.Text = "";
                error = true;
            }
            if (Path.IsPathRooted(GamesPath.Text) == false) {
                GamesPath.Text = "";
                error = true;
            }
            if (error)
                return;

            _dataModel.BackupDefaultPath = BackupPath.Text;
            _dataModel.GamesDefaultPath = GamesPath.Text;
            ConfigManager.SaveGlobalConfig(_dataModel);
            SaveClicked?.Invoke(_dataModel);
        }
    }
}
