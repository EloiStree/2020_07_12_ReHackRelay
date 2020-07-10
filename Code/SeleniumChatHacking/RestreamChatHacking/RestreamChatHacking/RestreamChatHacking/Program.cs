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
using System.Threading;

namespace RestreamChatHacking
{

    //TODOLIST
    //IF the user close the selenium window, close the stream softly and propose to reenter a new link.

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
            if (!File.Exists(AppData.ConfigurationPath))
                SaveConfigurationFile();
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

            if (ChatHackerConfiguration.Instance.m_sendMockingMessages)
            LaunchDirtyMockupSystemOnThread();

            LaunchRestreamChatOberver();


            AvoidUserToQuit();
            SayGoodBye();
        }

      
        private static void LaunchDirtyMockupSystemOnThread()
        {
            Thread newThread = new Thread(UseMockUData);
            newThread.IsBackground = true;
            newThread.Start();
        }

        private static void UseMockUData()
        {
            int count=0;
            while (true) {
                if(mockUpAccess!=null)
                mockUpAccess.FakeMessage("MockUp User", "Ha ha ha " + count++,ChatPlatform.Mockup);
    
                Thread.Sleep(1000);
            }

        }

        private static void LaunchRestreamChatOberver()
        {
            AccessRestreamCode restreamChat = new AccessRestreamCode();
            restreamChat.m_useDebug = true;
            restreamChat.SetAllowingAllSize(!ChatHackerConfiguration.Instance.m_useMaxiumMessageSize);
            restreamChat.SetMaximumMessageSizeTo(ChatHackerConfiguration.Instance.MaximumMessageSize);
            AddMockUpSystem(restreamChat);
            AddListenersToRestreamChat(restreamChat);
            LaunchRestreamChatObserver(restreamChat);
        }

        private static AccessRestreamCode mockUpAccess;
        private static void AddMockUpSystem(AccessRestreamCode restreamChat)
        {
            mockUpAccess = restreamChat;
        }

        private static void LaunchRestreamChatObserver(AccessRestreamCode d)
        {
            d.Setup(false);
            d.StartToListenAtRestreamEmbedUrl(ChatHackerConfiguration.Instance.GetRestreamChatURL());
            d.TeardownRunningServer();
        }

        private static void AddListenersToRestreamChat(AccessRestreamCode d)
        {
            // Save in files
            d.m_onMessageDetected += ThrowMessageToListeners;
            // Launch server from the chat
            d.m_onMessageDetected += LaunchStreaming;
            // Stop server from the chat
            d.m_onMessageDetected += StopStreaming;
            // Debug incoming message
            d.m_onMessageDetected += DisplayMessage;
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
                mockUpAccess.FakeMessage("RestreamHacking", answer, ChatPlatform.Mockup);


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
            Console.Out.WriteLine("######  Hello & welcome to Restream Chat Relay  ######");
            Console.Out.WriteLine("#######################################################");
            Console.Out.WriteLine("> Config stored in " + AppData.RestreamAppDataPath);
            Console.Out.WriteLine("> Last messages stored in " + AppData.LastMessagesPath);
            Console.Out.WriteLine("> All messages stored in " + AppData.AllMessagesPath);
            Console.Out.WriteLine("> Code GitHub & Manual: " + "https://gitlab.com/eloistree/2017_12_23_RestreamChatHacking");
            Console.Out.WriteLine("> Issue to report: " + "https://eloistree.page.link/discord");
            Console.Out.WriteLine("> License: " + "https://gist.github.com/EloiStree/b4e1520d068c23af7234755036adc170");
            Console.Out.WriteLine("> Credit: " + "Strée Eloi - https://ko-fi.com/eloistree");
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
            if (ChatHackerConfiguration.Instance.m_allowSteamLaunchingFromChat) { 
            if (message.Message.Contains(ChatHackerConfiguration.Instance.m_startStreamKeyword ))
            {

                Console.WriteLine("Action: Try to launch streaming");
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\StartStreaming.bat");

            }
            }
        }

        private static void StopStreaming(RestreamChatMessage message)
        {
            if (ChatHackerConfiguration.Instance.m_allowSteamKillingFromChat)
            {
               
            if (message.Message.Contains(ChatHackerConfiguration.Instance.m_killStreamWord ))
            {
                Console.WriteLine("Action: Try to stop streaming");
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\StopStreaming.bat");

            }
            }
        }



        private static MessageCommunication.IThrow recentMessagesFile = null;
        private static MessageCommunication.IThrow allMessagesFile = null;
        private static MessageCommunication.IThrow sendOSCMessages = null;
        private static MessageCommunication.IThrow [] sendUDPListeners = null;
        private static Queue<RestreamChatMessage> m_lastMessages = new Queue<RestreamChatMessage>();
        private static void ThrowMessageToListeners(RestreamChatMessage message)
        {
           
           
           
           
            if (recentMessagesFile ==null &&
                ChatHackerConfiguration.Instance.m_useLastMessagesFile)
                recentMessagesFile = new MessageCommunication.ThrowFile()
            {
                WriteBy = MessageCommunication.ThrowFile.WriteType.Overriding,
                FilePath = AppData.LastMessagesPath,
                UseRelativePath = false
            };

            if (allMessagesFile == null &&
                ChatHackerConfiguration.Instance.m_useAllMessagesFile)
                allMessagesFile = new MessageCommunication.ThrowFile()
            {
                WriteBy = MessageCommunication.ThrowFile.WriteType.Appending,
                FilePath = AppData.AllMessagesPath,
                UseRelativePath = false
            };

             if(sendOSCMessages == null &&
                ChatHackerConfiguration.Instance.m_useOsc)
            sendOSCMessages = new ThrowOSC(
                ChatHackerConfiguration.Instance.m_oscAddress,
                ChatHackerConfiguration.Instance.m_oscPort );

            if (sendUDPListeners == null &&
                ChatHackerConfiguration.Instance.m_useUdp) {
                string[] ips = ChatHackerConfiguration.Instance.m_udpAddressListeners;
                sendUDPListeners = new MessageCommunication.IThrow[ips.Length];
                for (int i = 0; i < ips.Length; i++)
                {
                    sendUDPListeners[i] = new ThrowUDP(
                    ips[i],
                    ChatHackerConfiguration.Instance.m_udpPort);
                }
                
            }

            EnqueueWithMaximumBoundery(message);

            if (recentMessagesFile != null)
                recentMessagesFile.SendChatMessage(message);
            if (allMessagesFile != null)
                allMessagesFile.SendChatMessage(message);
            if (sendOSCMessages != null)
                sendOSCMessages.SendChatMessage(message);
            if (sendUDPListeners != null)
            {
                for (int i = 0; i < sendUDPListeners.Length; i++)
                {
                    sendUDPListeners[i].SendChatMessage(message);
                }
            }


        }

        private static void EnqueueWithMaximumBoundery(RestreamChatMessage message)
        {
            while (m_lastMessages.Count > ChatHackerConfiguration.Instance.MaximumMessagesTracked)
                m_lastMessages.Dequeue();
            m_lastMessages.Enqueue(message);
        }

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

    private Debug m_debug;
    public Debug DebugOption { get {
            if (m_debug == null)
                m_debug = new Debug();
            return m_debug;
        }
    }
    #region RESTREAM IRC
    public bool IsRestreamDefined() { return !string.IsNullOrEmpty(m_restreamEmbedURL); }
    public string GetRestreamChatURL() { return m_restreamEmbedURL; }
    public string m_restreamEmbedURL;
    #endregion


    //#region FACEBOOK LINKS
    //public bool IsFacebookPostDefined() { return _facebookPostURL.Length > 0; }
    //public string [] GetFacebokPostURL() { return _facebookPostURL; }
    //public string [] _facebookPostURL;

    //#endregion

    #region STORAGE INFO
    public int m_maxMessagesTracked = 30;
    public int MaximumMessagesTracked { get { return m_maxMessagesTracked; } }
    #endregion

    #region FILTER


    public bool m_useMaxiumMessageSize = false;
    private int m_maximumMessageSize  =1024 ;
    public bool m_useOsc=true;
    public string m_oscAddress="127.0.0.1";
    public int m_oscPort=2513;
    public bool m_useUdp=true;
    public string [] m_udpAddressListeners = new string[] {"127.0.0.1","192.168.137.103" };
    public int m_udpPort=2512;
    public bool m_useAllMessagesFile=true;
    public bool m_useLastMessagesFile=false;
    public bool m_allowSteamLaunchingFromChat=false;
    public bool m_allowSteamKillingFromChat=false;
    public string m_killStreamWord = "!stopstreaming";
    public string m_startStreamKeyword= "!startstreaming";
    public bool m_sendMockingMessages=false;

    public int MaximumMessageSize
    {
        get { return m_maximumMessageSize; }
        set { m_maximumMessageSize = value; }
    }




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
        m_restreamEmbedURL = url;
    }
    #endregion

}