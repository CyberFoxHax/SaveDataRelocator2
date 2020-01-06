using System;
using System.Diagnostics;

namespace SaveDataRelocator2 {
    public static class CommandLineEntry {
        public static void Entry(string[] args) {
            ConfigManager.Initialize();

            if (args.Length > 1 && string.IsNullOrEmpty(args[1])) {
                if (Debugger.IsAttached)
                    throw new Exception("Invalid arguments");
                else
                    Console.WriteLine(App.HelpString);
                return;
            }

            if (args[1] == "-launch") {
                var configFilename = args[2];
                Launcher.Launch(configFilename);
            }
        }
    }
}
