using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace SaveDataRelocator2.Views
{
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            ContentPresenter.Content = null;

            ButtonConfig.Click += ButtonConfig_Click;
            ButtonNew.Click += ButtonNew_Click;
            ButtonDelete.Click += ButtonDelete_Click;
            GamesList.ItemClicked += ConfigList_ItemClicked;
            ButtonDelete.Visibility = Visibility.Collapsed;
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e) {
            var view = new ConfigView();
            view.DataContext = new DataModels.ApplicationConfig();
            view.SaveClicked += View_SaveClicked;
            GamesList.Selection = null;
            GamesList.Refresh();
            ContentPresenter.Content = view;
            ButtonDelete.Visibility = Visibility.Collapsed;
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e) {
            var view = new GameSettingsView();
            var newConfig = new DataModels.GameRelocationConfig();
            GamesList.AddItem(newConfig);
            view.DataContext = newConfig;
            GamesList.Refresh();
            GamesList.Selection = newConfig;
            view.SaveClicked += OnSaveClick;
            ContentPresenter.Content = view;
            ButtonDelete.Visibility = Visibility.Visible;
        }

        private void ConfigList_ItemClicked(DataModels.GameRelocationConfig obj) {
            var view = new GameSettingsView();
            view.DataContext = obj;
            view.SaveClicked += OnSaveClick;
            ContentPresenter.Content = view;
            ButtonDelete.Visibility = Visibility.Visible;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e) {
            var view = new DeleteGameView();
            var selection = GamesList.Selection;
            if (selection == null)
                return;
            GamesList.Refresh();
            GamesList.MarkForDeletion(selection);
            view.DataContext = selection;
            ContentPresenter.Content = view;
        }

        private void OnSaveClick(DataModels.GameRelocationConfig obj) {
            GamesList.Refresh();
            ContentPresenter.Content = null;
        }

        private void View_SaveClicked(DataModels.ApplicationConfig obj) {
            throw new System.NotImplementedException();
        }
    }
}
