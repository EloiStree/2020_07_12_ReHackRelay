using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking
{
    public class RestreamAppData
    {
        public static string RestreamAppDataPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RestreamChatHacking";
            }
        }
        public static string ConfigurationPath
        {
            get
            {
                return RestreamAppDataPath + "\\config.json";
            }
        }
        public static void CreateReastreamFolder()
        {
            Directory.CreateDirectory(RestreamAppData.RestreamAppDataPath);
        }
        public static void CreateRestreamConfigFile()
        {
            if (!File.Exists(RestreamAppData.ConfigurationPath))
                SaveConfigurationFile();
        }
        public static void SaveConfigurationFile()
        {
            File.WriteAllText(RestreamAppData.ConfigurationPath, ChatHackerConfiguration.GetJson());
        }
        public static void LoadConfigurationFile()
        {
            ChatHackerConfiguration.SetFromJson(File.ReadAllText(RestreamAppData.ConfigurationPath));
        }

        public static void CreateAndOverrideFile(string path, string text)
        {
            File.WriteAllText(path, text);
        }
        public static void AppendFile(string path, string text)
        {
            CheckFilePresence(path);
            File.AppendAllText(path, text);
        }

        public static void CheckFilePresence(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
        }
        public static bool IsRestreamAppDataDefined() { return Directory.Exists(RestreamAppData.RestreamAppDataPath); }
        public static bool IsConfigurationFileDefined() { return File.Exists(RestreamAppData.ConfigurationPath); }
        public static void CheckForFilesPresence()
        {

            if (!RestreamAppData.IsRestreamAppDataDefined())
                RestreamAppData.CreateReastreamFolder();
            if (!RestreamAppData.IsConfigurationFileDefined())
                RestreamAppData.CreateRestreamConfigFile();

        }
    }
}
