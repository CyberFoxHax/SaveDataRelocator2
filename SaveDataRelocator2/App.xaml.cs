using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;

namespace SaveDataRelocator2
{
    public partial class App : Application {

        public const string HelpString = "Usage: -launch [file]\nfilename being the filename of the json config in the Configurations folder in the application directory";

        public App() : this(Environment.GetCommandLineArgs()) {}

        public App(string[] args) {
            if (args.Length == 1) {
                ConfigManager.Initialize();
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                StartupUri = new Uri("Views/MainWindow.xaml", UriKind.Relative);
                return;
            }
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            CommandLineEntry.Entry(args);

            Shutdown();
        }

    }
}
