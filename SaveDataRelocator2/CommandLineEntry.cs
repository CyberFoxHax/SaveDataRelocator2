using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SaveDataRelocator2 {
    public static class CommandLineEntry {
        public static void Entry(string[] args) {
            ConfigManager.Initialize();

            if (args.Length > 1 && args.Length % 2 == 0) {
                if (Debugger.IsAttached)
                    throw new Exception("Invalid arguments");
                else
                    Console.WriteLine(App.HelpString);
                return;
            }

            var argsDict = new Dictionary<string, string>();
            for (var i = 1; i < args.Length; i += 2)
                argsDict[args[i + 0]] = args[i + 1];

            if (args[1] == "-launch") {
                var configFilename = args[2];
                Launcher.Launch(configFilename);
            }
        }
    }
}
