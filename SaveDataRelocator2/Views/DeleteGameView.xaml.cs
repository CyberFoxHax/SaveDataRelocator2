using System;
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

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _dataModel = DataContext as DataModels.GameRelocationConfig;
            if (_dataModel == null)
                return;
            Filename.Text = _dataModel.Filename;
            SaveDataPath.Text = _dataModel.SaveDataPath;
            BackupDataPath.Text = _dataModel.BackupDataPath;
            ExecutablePath.Text = _dataModel.ExecutablePath;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e) {
            _dataModel.Filename = Filename.Text;
            _dataModel.SaveDataPath = SaveDataPath.Text;
            _dataModel.BackupDataPath = BackupDataPath.Text;
            _dataModel.ExecutablePath = ExecutablePath.Text;
            DataContext = _dataModel;

            ConfigManager.DeleteGameConfig(_dataModel);

            ViewCompleted?.Invoke(this);
        }

        private void ButtonOpenFolder_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void ButtonDeleteShortcut_Click(object sender, RoutedEventArgs e) {
            ShortcutWrapper.Visibility = Visibility.Collapsed;
            //throw new NotImplementedException();
        }
    }
}
