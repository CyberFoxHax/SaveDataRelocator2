using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace SaveDataRelocator2.Views
{
    public partial class GamesList : UserControl {
        public GamesList() {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            ListView.Items.Clear();
            ListView.ItemsSource = ConfigManager.LoadAllGameConfigs().Select(p=>new GamesListItemViewModel(p)).ToList();
            ListView.MouseUp += ListView_MouseUp;
        }

        private void ListView_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var listView = sender as ListView;
            if (listView == null)
                return;

            var item = listView.SelectedItem as GamesListItemViewModel;
            if (item == null)
                return;


            item.MarkedForDeletion = false;
            var selection = ListView.SelectedItem;
            Refresh();
            ListView.SelectedItem = selection;
            ItemClicked?.Invoke(item.BackingData);
        }

        public event Action<DataModels.GameRelocationConfig> ItemClicked;        

        public void Refresh() {
            var source = (List<GamesListItemViewModel>)ListView.ItemsSource;
            ListView.ItemsSource = null;
            if(source != null)
                foreach (var item in source)
                    item.MarkedForDeletion = false;            
            ListView.ItemsSource = source;
        }

        public DataModels.GameRelocationConfig Selection {
            get {
                var selectedViewModel = (GamesListItemViewModel)ListView.SelectedItem;
                if (selectedViewModel == null)
                    return null;
                return selectedViewModel.BackingData;
            }
            set {
                if (value == null) {
                    ListView.SelectedItem = null;
                    return;
                }    
                foreach (GamesListItemViewModel item in ListView.ItemsSource) {
                    if (item.BackingData != value)
                        continue;
                    ListView.SelectedItem = item;
                    return;
                }
            }
        }

        public void MarkForDeletion(DataModels.GameRelocationConfig config) {
            var item = ListView.ItemsSource
                .Cast<GamesListItemViewModel>()
                .FirstOrDefault(p=>p.BackingData == config);
            if (item == null)
                return;
            item.MarkedForDeletion = true;
        }

        public void AddItem(DataModels.GameRelocationConfig config) {
            var source = (List<GamesListItemViewModel>)ListView.ItemsSource;
            source.Add(new GamesListItemViewModel(config));
            ListView.ItemsSource = source;
        }

        public void RemoveItem(DataModels.GameRelocationConfig config) {
            if (config == null)
                throw new Exception("Paramter can't be null");
            var source = (List<GamesListItemViewModel>)ListView.ItemsSource;
            var item = source.FirstOrDefault(p => p.BackingData == config);
            if (item == null)
                return;
            source.Remove(item);
            ListView.ItemsSource = source;
        }
    }
}
