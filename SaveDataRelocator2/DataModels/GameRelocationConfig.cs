using Newtonsoft.Json;

namespace SaveDataRelocator2.DataModels
{
    public class GameRelocationConfig {
        [JsonIgnore]
        public string Filename { get; set; }
        public string RemoteDirectory { get; set; }
        public string ExecutablePath { get; set; }
        public string BackupDirectory { get; set; }
    }
}
