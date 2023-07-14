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
using System.Reactive.Disposables;
using System.Security.Cryptography.X509Certificates;
using RestreamChatHacking;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Newtonsoft.Json.Converters;

namespace RestreamChatHacking
{

    class Program
    {
        public static SeleniumThreadRunning m_instanceRunning;
        



        public static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProcessExit);
            BatchToLaunch.TryLaunchRestream();
            RestreamAppData.CheckForFilesPresence();
            RestreamAppData.LoadConfigurationFile();
            ConsoleCommunication.HelloWorldAndCredit();
            if (ChatHackerConfiguration.Instance.IsFirstTimeForUser())
            {
                Console.Out.WriteLine("Hello There :) ... ");
                Console.Out.WriteLine("I see it is your first time.");
                Console.Out.WriteLine("You can edit Config.json in AppData to set your preference:");
                Console.Out.WriteLine(RestreamAppData.ConfigurationPath);

                Console.Out.WriteLine("To work as UDP (by default), the app need: Ip(s) to target, Port, Restream Chat link.");
                Console.Out.WriteLine("When providing, a chrome browser will open and Hide");
                Console.Out.WriteLine("The app only work if:\n- This console is open\n-Resteam Chat application is open\n- Good Restream link was given\n-The launched chome window is open");

                Console.Out.WriteLine("\nAs you are new, could you give the following basic information?\n");
                ConsoleCommunication.AskForTargetUdpPort();
                ConsoleCommunication.AskForTargetUdpIps();
                ChatHackerConfiguration.Instance.m_isFirstTimeTheAppIsLaunch = false;
            }
            ConsoleCommunication.AskForRestreamEmbedLink();
            RestreamAppData.SaveConfigurationFile();

            if (ChatHackerConfiguration.Instance.IsRequestingFakeMessagesToDebug())
            LaunchDirtyMockupSystemOnThread();

            

            AddListenersToMessagesExport();
            LaunchRestreamChatOberver();


            if (ChatHackerConfiguration.Instance.IsUserRequestToHideInterface())
            {
               User32Utility.HideTheInterfaceWithUser32DLL();
            }


            AvoidUserToQuit();
        }
        static void ProcessExit(object sender, EventArgs e)
        {
            m_instanceRunning.StopAll();

        }
       


        private static void LaunchDirtyMockupSystemOnThread()
        {
            SimulateFakeMessage.LaunchThread();
        }

       
       
        private static void LaunchRestreamChatOberver()
        {
            RamMemoryRegisterOfMessages memory = new RamMemoryRegisterOfMessages();
            SeleniumAccessToRestreamChat restreamChat = new SeleniumAccessToRestreamChat();
            restreamChat.OpenPage(ChatHackerConfiguration.Instance.GetRestreamChatURL());
            // memory.SetMaximumMessageSizeTo(ChatHackerConfiguration.Instance.MaximumMessageSize);
            Thread t = new Thread(new ThreadStart(CheckForNewMessages));
            m_instanceRunning = new SeleniumThreadRunning();
            m_instanceRunning.m_messagesInMemory = memory;
            m_instanceRunning.m_selenium = restreamChat;
            m_instanceRunning.m_runningThread = t;
            m_instanceRunning.m_timeBetweenFrameInMs = ChatHackerConfiguration.Instance.GetTimeBetweenChatCheckRequrested();
            t.Start();

        }
        public static NewMessageObserver m_newMessageObserver = new NewMessageObserver();
        private static void CheckForNewMessages()
        {
            string htmlCode="";
            List<string> messagesAsHtml;
            List<RestreamChatMessage> foundMessageInPage= new List<RestreamChatMessage>();
            List<RestreamChatMessage> newMessageInPage = new List<RestreamChatMessage>() ;
            long tick=0;
           
            while (m_instanceRunning!=null && m_instanceRunning.IsAllowToRun()) {

       
                if (!m_instanceRunning.m_selenium.IsNavigatorOpen())
                {
                    m_instanceRunning.StopRunning();
                    Environment.Exit(0);
                }
                else
                {
                    Console.Out.WriteLine("\n\nTick:\n" + tick);
                   // Console.Out.WriteLine("HTML:\n" + htmlCode);

                    htmlCode =  m_instanceRunning.m_selenium.GetRestreamHtmlPageInformation();
                    string p = Environment.CurrentDirectory + "\\LastDownloaded.txt";
                    File.WriteAllText(p, htmlCode);

                    
                    HTML2Messages.GetMessagesInHTML(htmlCode, out messagesAsHtml, out foundMessageInPage);
                    m_newMessageObserver.SetWithLastRefresh( foundMessageInPage);
                    m_newMessageObserver.GetNewMessage(out  newMessageInPage);

                    //if (foundMessageInPage.Count > 0) {
                    //   m_instanceRunning.m_messagesInMemory.AddMessagesAndRecovertNewInList(ref foundMessageInPage, out newMessageInPage);
                       // m_instanceRunning.m_messagesInMemory.Add(newMessageInPage);
                        ExportToOtherApps.Push(newMessageInPage);
                    //}
                    Console.Out.WriteLine(
                        string.Format("In Page: {0}html - {1}msg - {2}new ", messagesAsHtml.Count, foundMessageInPage.Count, newMessageInPage.Count));
                    
                }
                Thread.Sleep(m_instanceRunning.GetDelayBetweenFrame());
                tick++;
            }
        }


    private static void AddListenersToMessagesExport()
        {
            // Save in files
            ExportToOtherApps.AddListener(ThrowMessageToListeners);
            // Launch server from the chat
            ////ExportToOtherApps.AddListener(LaunchStreaming);
            // Stop server from the chat
            //ExportToOtherApps.AddListener(StopStreaming);
            //// Debug incoming message
            ExportToOtherApps.AddListener(ConsoleCommunication.DisplayMessage);
        }

       

        private static void AvoidUserToQuit()
        {
            string answer = "";
            while (answer != "q")
            {

                Console.WriteLine("Press any key to leave...");
                answer = Console.ReadLine();
              // mockUpAccess.FakeMessage("RestreamHacking", answer, ChatPlatform.Mockup);


            }
            
        }

        private static MessageCommunication.IThrow sendOSCMessages = null;
        //private static MessageCommunication.IThrow sendMQTTMessages = null;
        private static MessageCommunication.IThrow [] sendUDPListeners = null;
         private static void ThrowMessageToListeners(RestreamChatMessage message)
        {
           
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


    }

    public class SeleniumThreadRunning : IDisposable
    {
        public RamMemoryRegisterOfMessages m_messagesInMemory;
        public SeleniumAccessToRestreamChat m_selenium;
        public Thread m_runningThread;
        public bool m_requestThreadToStop = false;
        public int m_timeBetweenFrameInMs = 1000;

        public void Dispose()
        {
            m_requestThreadToStop = false;
        }

        public bool IsAllowToRun()
        {
            return !m_requestThreadToStop;
        }

        public int GetDelayBetweenFrame()
        {
            return m_timeBetweenFrameInMs;
        }
        public void StopAll()
        {

            m_runningThread.Abort();
            m_runningThread = null;
            m_selenium.TeardownRunningDriver();
            m_selenium = null;
            StopRunning();
        }

        public void StopRunning()
        {
            m_requestThreadToStop = true;
        }
    }

    [System.Serializable]
    public class ChatHackerConfiguration
    {
        public static ChatHackerConfiguration Instance = new ChatHackerConfiguration();

        [System.Serializable]
        public class Debug
        {
            public bool DisplayMessage = true;

        }

        private Debug m_debug;
        public Debug DebugOption
        {
            get
            {
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
        private int m_maximumMessageSize = 1024;
        public bool m_useOsc = true;
        public string m_oscAddress = "127.0.0.1";
        public int m_oscPort = 2513;
        public bool m_useUdp = true;
        public string[] m_udpAddressListeners = new string[] { "127.0.0.1" };
        public int m_udpPort = 2512;
        public bool m_useAllMessagesFile = true;
        public bool m_useLastMessagesFile = false;
        public bool m_allowSteamLaunchingFromChat = false;
        public bool m_allowSteamKillingFromChat = false;
        public string m_killStreamWord = "!stopstreaming";
        public string m_startStreamKeyword = "!startstreaming";
        public bool m_sendMockingMessages = false;
        public bool m_launchRestreamWithTheApp=true;
        public int m_timeBetweenChatCheck=500;
        public bool m_hideInterfaceAtStart=true;
        public bool m_isFirstTimeTheAppIsLaunch=true;
        public int m_windowWidth=400;
        public int m_windowHeight=1000;
        public int MaximumMessageSize
        {
            get { return m_maximumMessageSize; }
            set { m_maximumMessageSize = value; }
        }




        #endregion
        #region SAVE AND LOAD
        public static string GetJson()
        {
            return JsonConvert.SerializeObject(Instance ,Formatting.Indented,
           new JsonConverter[] { new StringEnumConverter() });
        }

        public static void SetFromJson(string json)
        {
            ChatHackerConfiguration config = null;
            try
            {
                config = JsonConvert.DeserializeObject<ChatHackerConfiguration>(json);

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }

            if (config != null)
                Instance = config;
        }


        public void SetRestreamChatURL(string url)
        {
            m_restreamEmbedURL = url;
        }

        public bool IsRequestingFakeMessagesToDebug()
        {
            return m_sendMockingMessages;
        }

        public int GetTimeBetweenChatCheckRequrested()
        {
            return m_timeBetweenChatCheck;
        }

        internal bool IsUserRequestToHideInterface()
        {
            return m_hideInterfaceAtStart;
        }

        internal bool IsFirstTimeForUser()
        {
            return m_isFirstTimeTheAppIsLaunch;
        }
        #endregion

    }
}
