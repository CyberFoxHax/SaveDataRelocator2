using Newtonsoft.Json;

namespace SaveDataRelocator2.DataModels
{
    public class GameRelocationConfig {
        [JsonIgnore]
        public string Filename { get; set; }
        public string SaveDataPath { get; set; }
        public string ExecutablePath { get; set; }
        public string BackupDataPath { get; set; }
    }
}
