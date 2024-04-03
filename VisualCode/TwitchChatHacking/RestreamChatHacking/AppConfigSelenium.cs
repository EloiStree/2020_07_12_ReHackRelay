using System.IO;

namespace RestreamChatHacking
{
    public class AppConfigSelenium
    {
        public static AppConfigSelenium Instance = null;


        public static void ReloadFile() {

            string fileName = "AppConfigSelenium.json";
            string folderPath = Directory.GetCurrentDirectory() + "\\Config\\";
            string filePath = folderPath + fileName ;


            if (!File.Exists(filePath)) {
                Directory.CreateDirectory(folderPath);
                File.WriteAllText(filePath, Newtonsoft.Json.JsonConvert.SerializeObject(new AppConfigSelenium()));
            }


            string textJson = File.ReadAllText(filePath);
            Instance = Newtonsoft.Json.JsonConvert.DeserializeObject<AppConfigSelenium>(textJson);
            Instance.m_firefoxDriverAbsoluteFolderPath = (Directory.GetCurrentDirectory() + "\\" + Instance.m_firefoxDriverRelativeFolderPath);
            Instance.m_googleDriverAbsoluteFolderPath = (Directory.GetCurrentDirectory() + "\\" + Instance.m_googleDriverRelativeFolderPath);

        }

        public string m_firefoxExePath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        public string m_googleExePath = "C:\\Program Files\\Google\\Chrome Beta\\Application\\chrome.exe";

        public string m_firefoxDriverRelativeFolderPath = "FirefoxDriver";
        public string m_googleDriverRelativeFolderPath = "ChromeDriver";
        public string m_firefoxDriverAbsoluteFolderPath = "";
        public string m_googleDriverAbsoluteFolderPath = "";
    }


}