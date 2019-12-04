using System;
using SaveDataRelocator2.DataModels;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SaveDataRelocator2 {
    public static class ConfigManager {
        public const string ConfigFolder = "Configurations";
        public const string AppConfig = "Config.json";
        public const string ShortcutsCache = "ShortcutsCache.json";

        private static Dictionary<string, GameRelocationConfig> _loadedGameConfigs;

        public static void Initialize() {
            if (Directory.Exists(ConfigFolder) == false)
                Directory.CreateDirectory(ConfigFolder);
            if (File.Exists(AppConfig) == false)
                File.CreateText(AppConfig).Dispose();
            if (File.Exists(ShortcutsCache) == false)
                File.CreateText(ShortcutsCache).Dispose();
        }

        public static ApplicationConfig LoadAppConfig() {
            return JsonConvert.DeserializeObject<ApplicationConfig>(File.ReadAllText(AppConfig));
        }

        public static void SaveAppConfig(ApplicationConfig config) {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(AppConfig, json);
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
            return LoadGameConfigFromPath(Path.Combine(ConfigFolder, name + ".json"));
        }

        public static GameRelocationConfig LoadGameConfigFromPath(string path) {
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

        public static ShortcutsCacheModel LoadShortcutsCache() {
            return JsonConvert.DeserializeObject<ShortcutsCacheModel>(File.ReadAllText(AppConfig));
        }

        public static void SaveShortcutsCache(ShortcutsCacheModel config) {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(AppConfig, json);
        }

    }
}