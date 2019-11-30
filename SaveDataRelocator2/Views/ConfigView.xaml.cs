using System;
using System.Windows;
using System.Windows.Controls;

namespace SaveDataRelocator2.Views
{
    public partial class ConfigView : UserControl {
        public ConfigView() {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false) {
                BackupPath.Text = "";
            }
            Loaded += OnLoaded;
            ButtonSave.Click += ButtonSave_Click;
        }

        public event Action<DataModels.ApplicationConfig> SaveClicked;
        private DataModels.ApplicationConfig _dataModel;

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _dataModel = DataContext as DataModels.ApplicationConfig;
            if (_dataModel == null)
                return;
            BackupPath.Text = _dataModel.BackupDataPath;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            _dataModel.BackupDataPath = BackupPath.Text;
            DataContext = _dataModel;
            SaveClicked?.Invoke(_dataModel);
        }
    }
}
