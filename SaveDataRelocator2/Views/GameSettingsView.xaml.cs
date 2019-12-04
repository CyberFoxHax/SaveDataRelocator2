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
        }

        private void CreateShortcut_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
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
            _dataModel.Filename = Filename.Text;
            _dataModel.RemoteDirectory = SaveDataPath.Text;
            _dataModel.BackupDirectory = BackupDataPath.Text;
            _dataModel.ExecutablePath = ExecutablePath.Text;
            if (string.IsNullOrEmpty(_dataModel.Filename))
                _dataModel.Filename = Path.GetFileName(_dataModel.ExecutablePath).Replace(".exe", "");

            DataContext = _dataModel;
            ConfigManager.SaveGameConfig(_dataModel);


            SaveClicked?.Invoke(_dataModel);
        }
    }
}
