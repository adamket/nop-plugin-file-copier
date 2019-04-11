using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NopPluginFileCopier
{
    class Program
    {
        static List<PluginWatcher> _pluginWatchers;
        static void Main(string[] args)
        {

            Console.WriteLine("\r\n\r\n --- Nop Plugin File Copier --- ");


            _pluginWatchers = new List<PluginWatcher>();
            
            if (File.Exists("config.json"))
            {
                var text = File.ReadAllText("config.json");
                try
                {
                    var items = JsonConvert.DeserializeObject<List<PluginWatcher>>(@text);
                    foreach (var item in items)
                    {
                        _pluginWatchers.Add(item);
                        item.AddWatch();
                    }
                }
                catch (Exception e)
                {
                    Console.Write("Error reading config: " + e);
                }
            }

            while (true)
            {
                Console.Write("\r\n\r\nEnter source plugin directory: ");
                var projectRoot = Console.ReadLine();

                if (!Directory.Exists(projectRoot))
                {
                    Console.WriteLine("Could not locate directory.");
                    continue;
                }


                Console.Write("Enter plugin output directory: ");

                var copyTo = Console.ReadLine();

                if (!Directory.Exists(copyTo))
                {
                    Console.WriteLine("Could not locate directory.");
                    continue;
                }

                var item = new PluginWatcher(projectRoot.ToLower(), copyTo.ToLower());
                _pluginWatchers.Add(item);

                item.AddWatch();
            }

        }
    }

    class PluginWatcher
    {
        public PluginWatcher()
        { }
        public PluginWatcher(string from, string to)
        {
            from = from.TrimEnd('\\');
            to = to.TrimEnd('\\');

            From = from;
            To = to;
        }

        public string From { get; set; }
        public string To { get; set; }

        public void AddWatch()
        {
            var watcher = new FileSystemWatcher(this.From)
            {
                IncludeSubdirectories = true,
                Filter = ""
            };

            watcher.Changed += CopyFile;
            watcher.Renamed += CopyFile;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"-- Watcher added --\r\nProject root: {this.From} \r\nCopies to: {this.To}");
        }


        private void CopyFile(object sender, FileSystemEventArgs e)
        {
            if (!ShouldCopy(e.FullPath))
            {
                return;
            }

            var info = $"[{DateTime.Now}: {e.ChangeType} {e.FullPath}]";

            var from = this.From.ToLower();
            var to = this.To.ToLower();

            var relativePath = e.FullPath.ToLower().Replace(from, string.Empty);
            var copyTo = to + relativePath;

            try
            {
                File.SetAttributes(e.FullPath, FileAttributes.Normal);
                File.SetAttributes(copyTo, FileAttributes.Normal);

                File.Copy(e.FullPath, copyTo, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine($"\r\n{info}: File copied from {@from} to {to}");
            Console.Write("\r\n\r\nEnter source plugin directory: ");
        }


        private static bool ShouldCopy(string path)
        {
            var ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }

            switch (ext.ToLower())
            {
                case ".cshtml":  
                case ".js":
                case ".css":
                case ".xml":
                    return true;
                default:
                    return false;
            }
        }

    }
}
