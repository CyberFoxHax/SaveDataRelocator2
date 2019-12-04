using System;
using System.IO;

namespace TestApplication
{
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            var appData = new DirectoryInfo(Environment.ExpandEnvironmentVariables("%appdata%"));
            var folder = Path.Combine(appData.FullName, "TestApplication");
            if (Directory.Exists(folder) == false) {
                Console.WriteLine("Creating new data");
                Directory.CreateDirectory(folder);
                File.WriteAllText(Path.Combine(folder, "HelloWorld.txt"), "This is initial data");
            }
            else {
                var str = File.ReadAllText(Path.Combine(folder, "HelloWorld.txt"));
                str += "\r\nHello!";
                File.WriteAllText(Path.Combine(folder, "HelloWorld.txt"), str);
                Console.WriteLine(str);
            }

        }
    }
}
