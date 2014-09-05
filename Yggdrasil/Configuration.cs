using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Nini.Config;

using Yggdrasil.Helpers;

namespace Yggdrasil
{
    /* Based on http://stackoverflow.com/a/15869868 */

    static class Configuration
    {
        static readonly string ConfigName = "Main";

        static string configPath;
        static string configFilename;

        static IConfigSource source;

        public static string FullConfigFilename { get { return (Path.Combine(configPath, configFilename)); } }

        public static string UpdateServer
        {
            get { return (source.Configs[ConfigName].GetString("UpdateServer", string.Format("http://magicstone.de/dzd/progupdates/{0}.txt", System.Windows.Forms.Application.ProductName))); }
            set { source.Configs[ConfigName].Set("UpdateServer", value); }
        }

        public static string LastDataPath
        {
            get { return (source.Configs[ConfigName].GetString("LastDataPath", string.Empty)); }
            set { source.Configs[ConfigName].Set("LastDataPath", value); }
        }

        public static GameDataManager.Languages Language
        {
            get { return ((GameDataManager.Languages)Enum.Parse(typeof(GameDataManager.Languages), (source.Configs[ConfigName].GetString("Language", "English")))); }
            set { source.Configs[ConfigName].Set("Language", value); }
        }

        public static Logger.Level LogLevel
        {
            get { return ((Logger.Level)Enum.Parse(typeof(Logger.Level), (source.Configs[ConfigName].GetString("LogLevel", "Info")))); }
            set { source.Configs[ConfigName].Set("LogLevel", value); }
        }

        static Configuration()
        {
            PrepareConfig();

            source = new XmlConfigSource(FullConfigFilename);
            source.AutoSave = true;

            CreateConfigSections();
        }

        static void CreateConfigSections()
        {
            if (source.Configs[ConfigName] == null) source.AddConfig(ConfigName);
        }

        static void PrepareConfig()
        {
            configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.Windows.Forms.Application.ProductName);
            configFilename = String.Format("{0}.xml", ConfigName);
            Directory.CreateDirectory(configPath);

            if (!File.Exists(FullConfigFilename)) File.WriteAllText(FullConfigFilename, "<Nini>\n</Nini>\n");
        }
    }
}
