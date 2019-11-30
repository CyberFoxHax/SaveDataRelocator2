﻿using System;
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
            Filename.Text = _dataModel.Filename;
            SaveDataPath.Text = _dataModel.SaveDataPath;
            BackupDataPath.Text = _dataModel.BackupDataPath;
            ExecutablePath.Text = _dataModel.ExecutablePath;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            _dataModel.Filename = Filename.Text;
            _dataModel.SaveDataPath = SaveDataPath.Text;
            _dataModel.BackupDataPath = BackupDataPath.Text;
            _dataModel.ExecutablePath = ExecutablePath.Text;
            DataContext = _dataModel;
            SaveClicked?.Invoke(_dataModel);
        }
    }
}