using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;

namespace SaveDataRelocator2
{
    public partial class App : Application {

        public const string ConfigFolder = "SaveLocations";
        public const string AppConfig = "Config.json";
        public const string ShortcutsCache = "ShortcutsCache.json";

        public App() {
            if (Directory.Exists(ConfigFolder) == false)
                Directory.CreateDirectory(ConfigFolder);
            if (File.Exists(AppConfig) == false)
                File.CreateText(AppConfig).Dispose();
            if (File.Exists(ShortcutsCache) == false)
                File.CreateText(ShortcutsCache).Dispose();

            var args = Environment.GetCommandLineArgs();

            if (args.Length == 1) {
                StartupUri = new Uri("Views/MainWindow.xaml", UriKind.Relative);
                return;
            }

            if (args.Length % 2 == 1) {
                if (Debugger.IsAttached)
                    throw new Exception("Invalid arguments");
                else
                    Console.WriteLine("Invalid arguments");
                return;
            }

            var argsDict = new Dictionary<string, string>();
            for (var i = 0; i < args.Length; i+=2)
                argsDict[args[i + 0]] = args[i + 1];

            if (args[1] == "-launch") {
                var key = args[2];

            }

            Shutdown();
        }

    }
}
