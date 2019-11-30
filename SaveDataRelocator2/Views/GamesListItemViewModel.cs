using SaveDataRelocator2.DataModels;

namespace SaveDataRelocator2.Views
{
    public class GamesListItemViewModel {
        public GamesListItemViewModel() {}
        public GamesListItemViewModel(GameRelocationConfig backingData) {
            BackingData = backingData;
        }

        public GameRelocationConfig BackingData { get; }

        private string _filename;
        public string Filename {
            get { return BackingData?.Filename ?? _filename; }
            set { _filename = value; }
        }

        public bool MarkedForDeletion { get; set; } = false;
    }
}
