using System;
using System.Windows;
using System.Windows.Controls;

namespace SaveDataRelocator2.Views
{
    public partial class ConfigView : UserControl {
        public ConfigView() {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            _dataModel = ConfigManager.LoadAppConfig() ?? new DataModels.ApplicationConfig();
            BackupPath.Text = "";
            Loaded += OnLoaded;
            ButtonSave.Click += ButtonSave_Click;
        }

        public event Action<DataModels.ApplicationConfig> SaveClicked;
        private DataModels.ApplicationConfig _dataModel;

        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (_dataModel == null)
                return;
            BackupPath.Text = _dataModel.BackupDataPath;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            _dataModel.BackupDataPath = BackupPath.Text;
            ConfigManager.SaveAppConfig(_dataModel);
            SaveClicked?.Invoke(_dataModel);
        }
    }
}
