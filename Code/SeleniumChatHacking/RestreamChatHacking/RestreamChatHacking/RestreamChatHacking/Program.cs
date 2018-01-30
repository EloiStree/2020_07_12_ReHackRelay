using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Mail;

namespace RestreamChatHacking
{

    class Program
    {
        public class AppData {
            public static string RestreamAppDataPath
            {get
                {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RestreamChatHacking";
                }
            }
            public static string ConfigurationPath
            {
                get
                {
                    return RestreamAppDataPath+"\\config.json";
                }
            }
            public static string LastMessagesPath
            {
                get
                {
                    return RestreamAppDataPath + "\\lastmessages.json";
                }
            }
            public static string AllMessagesPath
            {
                get
                {
                    return RestreamAppDataPath + "\\allmessages.json";
                }
            }



        }

        public static void CreateReastreamFolder()
        {
            Directory.CreateDirectory(AppData.RestreamAppDataPath);
        }
        public static void CreateRestreamConfigFile()
        {
            if(!File.Exists(AppData.ConfigurationPath))
                File.Create(AppData.ConfigurationPath);
        }
        public static void SaveConfigurationFile()
        {
            File.WriteAllText(AppData.ConfigurationPath, ChatHackerConfiguration.GetJson());
        }
        public static void LoadConfigurationFile()
        {
            ChatHackerConfiguration.SetFromJson(File.ReadAllText(AppData.ConfigurationPath));
        }

        public static void CreateAndOverrideFile(string path, string text) {
            File.WriteAllText(path, text);
        }
        public static void AppendFile(string path, string text) {
            CheckFilePresence(path);
            File.AppendAllText(path, text);
        }

        public static void CheckFilePresence(string path) {
            if (!File.Exists(path))
                File.Create(path);
        }




        public static bool IsRestreamAppDataDefined() { return Directory.Exists(AppData.RestreamAppDataPath); }
        public static bool IsConfigurationFileDefined() { return File.Exists(AppData.ConfigurationPath); }
        public static bool IsLastMessagesFileDefined() { return File.Exists(AppData.LastMessagesPath); }
        public static bool IsAllMessagesFileDefined() { return File.Exists(AppData.AllMessagesPath); }


        static void Main(string[] args)
        {
            CheckForFilesPresence();
            LoadConfigurationFile();
            HelloWorldAndCredit();

            string answer = "";
            AskForRestreamEmbedLink();
            SaveConfigurationFile();
            ConfigureUserOutput();

            LaunchRestreamChatOberver();

            AvoidUserToQuit();
            SayGoodBye();
        }

        private static void LaunchRestreamChatOberver()
        {
            AccessRestreamCode restreamChat = new AccessRestreamCode();
            AddListenersToRestreamChat(restreamChat);
            LaunchRestreamChatObserver(restreamChat);
        }

        private static void LaunchRestreamChatObserver(AccessRestreamCode d)
        {
            d.Setup(false);
            d.StartToListenAtRestreamEmbedUrl(ChatHackerConfiguration.Instance.GetRestreamChatURL());
            d.Teardown();
        }

        private static void AddListenersToRestreamChat(AccessRestreamCode d)
        {
            // Save in files
            d._onMessageDetected += SaveMessagesToFiles;
            // Launch server from the chat
            d._onMessageDetected += LaunchStreaming;
            // Stop server from the chat
            d._onMessageDetected += StopStreaming;
            // Debug incoming message
            d._onMessageDetected += DisplayMessage;
        }

        private static void SayGoodBye()
        {
            HelloWorldAndCredit();
        }

        private static void AvoidUserToQuit()
        {
            string answer = "";
            while (answer != "q")
            {

                Console.WriteLine("Press any key to leave...");
                answer = Console.ReadLine();
            }
            
        }

        private static void ConfigureUserOutput()
        {

            //TO ADD TO ARGS
            // Config file path with
            // Limite of message to store in memory
            // File Path where to store new messages
            // File Path where to store all messages
            // Filter / Mails messages ex: @Eloi -> mail with the message
            // OSC xor UDP addresses to send new messages info
            // Webhock addresses to send messages info
            //Console.Out.WriteLine(JsonConvert.SerializeObject(ChatHackerConfiguration.Instance));

        }
        
        private static void CheckForFilesPresence()
        {

            //Is folder RestreamChatHacking existing ?
            //    NO: Create it
            //Is File config exist ?
            //    NO: Create file config
            if (!IsRestreamAppDataDefined())
                CreateReastreamFolder();
            if (!IsConfigurationFileDefined())
                CreateRestreamConfigFile();

            // Is all messages exist ?
            //    Create all messages json
            // Is recent messages exist ?
            //    Create all messages json
            CheckFilePresence(AppData.LastMessagesPath);
            CheckFilePresence(AppData.AllMessagesPath);
        }

        private static void HelloWorldAndCredit()
        {
            ///HELLO
            Console.Out.WriteLine("#######################################################");
            Console.Out.WriteLine("######  Hello & welcome to Restream Chat Hacker  ######");
            Console.Out.WriteLine("#######################################################");
            Console.Out.WriteLine("> Config stored in " + AppData.RestreamAppDataPath);
            Console.Out.WriteLine("> Last messages stored in " + AppData.LastMessagesPath);
            Console.Out.WriteLine("> All messages stored in " + AppData.AllMessagesPath);
            Console.Out.WriteLine("> Code GitHub & Manual: " + "https://github.com/JamsCenter/2017_12_23_RestreamChatHacking");
            Console.Out.WriteLine("> Issue to report: " + "https://github.com/JamsCenter/2017_12_23_RestreamChatHacking/issues");
            Console.Out.WriteLine("> License: " + "https://github.com/JamsCenter/2017_12_23_RestreamChatHacking/wiki/License");
            Console.Out.WriteLine("> Credit: " + "Strée Eloi - http://jams.center");
            Console.Out.WriteLine("#######################################################");

            Console.Out.WriteLine(" ");
            Console.Out.WriteLine(" ");
            Console.Out.WriteLine(" ");
        }

        private static void AskForRestreamEmbedLink()
        {
            string answer = "";
            // Is user want to define a new restream ?
            //    !Empty = yes => store it in config
            //    Save the new config  

            if (ChatHackerConfiguration.Instance.IsRestreamDefined())
            {
                answer = AskQuestion("Do you want to use this link for Restream IRC ?\n"
                    + ChatHackerConfiguration.Instance.GetRestreamChatURL()
                    , "(N)o or enter to continue");

                if (IsNo(answer))
                {
                    DefinedRestreamChatURL();

                }
            }
            else {

                DefinedRestreamChatURL();

            }
          

        }

        private static void DefinedRestreamChatURL()
        {
            string answer="";
            do
            {
                answer = AskQuestion("Would you enter the Restream IRC embed link ?",
                   "Enter to the Restream IRC link to continue");
            } while (  !(answer.Trim().ToLower().StartsWith("http"))  );
            //QUESTION TO ME:  SHOUD I CHECK IF THE ANSWER IS A  URL ???
            ChatHackerConfiguration.Instance.SetRestreamChatURL(answer);
        }

        private static bool IsNo(string answer)
        {
            return answer.ToLower().StartsWith("n");
        }
        private static bool IsYes(string answer)
        {
            return answer.ToLower().StartsWith("y");
        }

        private static string AskQuestion(string question, string proposition)
        {
            string answer = "";
            do
            {
                Console.Out.WriteLine(question);
                Console.Out.WriteLine(proposition);
                answer = Console.In.ReadLine();
            } while (string.IsNullOrEmpty(answer));
            Console.Out.WriteLine(">> " + answer);
            return answer;
        }

        private static void DisplayMessage(RestreamChatMessage message)
        {

            Console.WriteLine(string.Format("{3} | {0},{1}:{2}", message.UserName, message.When, message.Message, message.Platform));
        }

        private static void LaunchStreaming (RestreamChatMessage message)
        {
            if (message.Message.Contains("!startstreaming"))
            {

                Console.WriteLine("Action: Try to launch streaming");
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\StartStreaming.bat");

            }
        }

        private static void StopStreaming(RestreamChatMessage message)
        {
            if (message.Message.Contains("!stopstreaming"))
            {
                Console.WriteLine("Action: Try to stop streaming");
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\StopStreaming.bat");

            }
        }


        //BAD CODE CHANGE LATER -->
        public static MessageCommunication.IThrow recentMessagesFile = new MessageCommunication.ThrowFile() { WriteBy = MessageCommunication.ThrowFile.WriteType.Overriding, FilePath = "LastMessages.json", UseRelativePath = true };
        public static MessageCommunication.IThrow allMessagesFile = new MessageCommunication.ThrowFile() { WriteBy = MessageCommunication.ThrowFile.WriteType.Appending, FilePath = "AllMessages.json", UseRelativePath = true };
        

        private static Queue<RestreamChatMessage> _lastMessages = new Queue<RestreamChatMessage>();
        private static void SaveMessagesToFiles(RestreamChatMessage message)
        {
           
            recentMessagesFile = new MessageCommunication.ThrowFile()
            {
                WriteBy = MessageCommunication.ThrowFile.WriteType.Overriding,
                FilePath = AppData.LastMessagesPath,
                UseRelativePath = false
            };

            allMessagesFile = new MessageCommunication.ThrowFile()
            {
                WriteBy = MessageCommunication.ThrowFile.WriteType.Appending,
                FilePath = AppData.AllMessagesPath,
                UseRelativePath = false
            };


            EnqueueWithMaximumBoundery(message);

            recentMessagesFile.Send(_lastMessages);
            allMessagesFile.Send(_lastMessages);
            // DONE BUT NEET TO BE LINKED TO EXTERNAL FILE WITH MAIL AND PASSWORD OUT OF THE GIT.
            //  if (message.Message.Contains("JamsCenter")) {
            //      mailMeIfTagged.Send(message);
            //  }
        }

        private static void EnqueueWithMaximumBoundery(RestreamChatMessage message)
        {
            while (_lastMessages.Count > ChatHackerConfiguration.Instance.MaximumMessagesTracked)
                _lastMessages.Dequeue();
            _lastMessages.Enqueue(message);
        }
        //<--BAD CODE CHANGE LATER

    }
   

}

[System.Serializable]
public class ChatHackerConfiguration {
    public static ChatHackerConfiguration Instance = new ChatHackerConfiguration();

    [System.Serializable]
    public class Debug
    {
       public  bool DisplayMessage=true;

    }

    private Debug _debug;
    public Debug DebugOption { get {
            if (_debug == null)
                _debug = new Debug();
            return _debug;
        }
    }
    #region RESTREAM IRC
    public bool IsRestreamDefined() { return !string.IsNullOrEmpty(_restreamEmbedURL); }
    public string GetRestreamChatURL() { return _restreamEmbedURL; }
    public string _restreamEmbedURL;
    #endregion


    #region FACEBOOK LINKS
    public bool IsFacebookPostDefined() { return _facebookPostURL.Length > 0; }
    public string [] GetFacebokPostURL() { return _facebookPostURL; }
    public string [] _facebookPostURL;

    #endregion

    #region STORAGE INFO
    public int _maxMessagesTracked = 30;
    public int MaximumMessagesTracked { get { return _maxMessagesTracked; } }
    #endregion
    #region SAVE AND LOAD
    public static string GetJson()
    {
        return JsonConvert.SerializeObject(Instance);
    }

    public static void SetFromJson(string json)
    {
        ChatHackerConfiguration config=null;
        try
        {
            config = JsonConvert.DeserializeObject<ChatHackerConfiguration>(json); 

        }
        catch (Exception e) {
            Console.Out.WriteLine(e);
        }

        if (config != null)
            Instance = config;
    }


    public void SetRestreamChatURL(string url)
    {
        _restreamEmbedURL = url;
    }
    #endregion

}