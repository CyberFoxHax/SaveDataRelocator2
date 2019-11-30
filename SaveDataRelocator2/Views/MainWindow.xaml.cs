using System.Windows;

namespace SaveDataRelocator2.Views
{
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            ContentPresenter.Content = null;

            ButtonConfig.Click += ButtonNavConfig_Click;
            ButtonNew.Click += ButtonNavNew_Click;
            ButtonDelete.Click += ButtonNavDelete_Click;
            GamesList.ItemClicked += ButtonNavItem_Click;
            ButtonDelete.Visibility = Visibility.Collapsed;
        }

        private void ButtonNavConfig_Click(object sender, RoutedEventArgs e) {
            var view = new ConfigView();
            view.SaveClicked += AppConfigSaved;
            GamesList.Selection = null;
            GamesList.Refresh();
            ContentPresenter.Content = view;
            ButtonDelete.Visibility = Visibility.Collapsed;
        }

        private void ButtonNavNew_Click(object sender, RoutedEventArgs e) {
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

        private void ButtonNavItem_Click(DataModels.GameRelocationConfig obj) {
            var view = new GameSettingsView();
            view.DataContext = obj;
            view.SaveClicked += OnSaveClick;
            ContentPresenter.Content = view;
            ButtonDelete.Visibility = Visibility.Visible;
        }

        private void ButtonNavDelete_Click(object sender, RoutedEventArgs e) {
            var view = new DeleteGameView();
            var selection = GamesList.Selection;
            if (selection == null)
                return;
            GamesList.Refresh();
            GamesList.MarkForDeletion(selection);
            view.ViewCompleted += OnDeleted;
            view.DataContext = selection;
            ContentPresenter.Content = view;
        }

        private void OnDeleted(DeleteGameView obj) {
            ContentPresenter.Content = null;
            GamesList.RemoveItem((DataModels.GameRelocationConfig)obj.DataContext);
            GamesList.Refresh();
        }

        private void OnSaveClick(DataModels.GameRelocationConfig obj) {
            GamesList.Refresh();
            ContentPresenter.Content = null;
        }

        private void AppConfigSaved(DataModels.ApplicationConfig obj) {
            ContentPresenter.Content = null;
        }
    }
}
