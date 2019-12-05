using SaveDataRelocator2.DataModels;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SaveDataRelocator2 {
    public static class ConfigManager {
        public const string ConfigFolder = "Configurations";
        public const string GlobalConfig = "Config.json";
        public const string RecentsData = "Recents.json";

        private static Dictionary<string, GameRelocationConfig> _loadedGameConfigs;

        public static void Initialize() {
            if (Directory.Exists(ConfigFolder) == false)
                Directory.CreateDirectory(ConfigFolder);
            if (File.Exists(GlobalConfig) == false)
                File.CreateText(GlobalConfig).Dispose();
            if (File.Exists(RecentsData) == false)
                File.CreateText(RecentsData).Dispose();
        }

        public static ApplicationConfig LoadGlobalConfig() {
            if(File.Exists(GlobalConfig))
                return JsonConvert.DeserializeObject<ApplicationConfig>(File.ReadAllText(GlobalConfig));
            return null;
        }

        public static void SaveGlobalConfig(ApplicationConfig config) {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(GlobalConfig, json);
        }

        public static void SaveGameConfig(GameRelocationConfig dataModel) {
            var str = JsonConvert.SerializeObject(dataModel, Formatting.Indented);
            File.WriteAllText(Path.Combine(ConfigFolder, dataModel.Filename + ".json"), str);

            if (_loadedGameConfigs == null)
                return;
            foreach (var item in _loadedGameConfigs) {
                if (item.Value != dataModel)
                    continue;

                if (item.Key != dataModel.Filename) {
                    File.Delete(Path.Combine(ConfigFolder, item.Key + ".json"));
                    _loadedGameConfigs.Remove(item.Key);
                    _loadedGameConfigs.Add(dataModel.Filename, dataModel);
                }

                break;
            }
        }

        public static void DeleteGameConfig(GameRelocationConfig dataModel) {
            File.Delete(Path.Combine(ConfigFolder, dataModel.Filename + ".json"));
        }

        public static GameRelocationConfig LoadGameConfigFromName(string name) {
            var path = Path.Combine(ConfigFolder, name + ".json");
            if(File.Exists(path))
                return LoadGameConfigFromPath(Path.Combine(ConfigFolder, name + ".json"));
            return null;
        }

        public static GameRelocationConfig LoadGameConfigFromPath(string path) {
            if (File.Exists(path) == false)
                return null;
            var obj = JsonConvert.DeserializeObject<GameRelocationConfig>(File.ReadAllText(path));
            obj.Filename = Path.GetFileName(path).Replace(".json", "");
            return obj;
        }

        public static List<GameRelocationConfig> LoadAllGameConfigs() {
            _loadedGameConfigs = Directory.EnumerateFiles(ConfigFolder)
                .ToDictionary(
                    p=>Path.GetFileName(p).Replace(".json", ""),
                    LoadGameConfigFromPath
                );

            return _loadedGameConfigs.Values.ToList();
        }

        public static RecentModel LoadRecents() {
            if(File.Exists(RecentsData))
                return JsonConvert.DeserializeObject<RecentModel>(File.ReadAllText(RecentsData));
            return null;
        }

        public static void SaveRecents(RecentModel config) {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(RecentsData, json);
        }

    }
}