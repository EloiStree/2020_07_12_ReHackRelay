using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Collections;
using System.Net.Mail;
using System.Threading;
using System.Reactive.Disposables;
using System.Security.Cryptography.X509Certificates;
using RestreamChatHacking;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RestreamChatHacking
{
    public static class IsProgramAlive { 
    public static DateTime IsAlive = DateTime.UtcNow;
    }
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
            if (ChatHackerConfigurationByJson.Instance.IsFirstTimeForUser())
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
                ChatHackerConfigurationByJson.Instance.m_isFirstTimeTheAppIsLaunch = false;
            }
            ConsoleCommunication.AskForRestreamEmbedLink();
            RestreamAppData.SaveConfigurationFile();

            if (ChatHackerConfigurationByJson.Instance.IsRequestingFakeMessagesToDebug())
            LaunchDirtyMockupSystemOnThread();

            

            AddListenersToMessagesExport();
            LaunchRestreamChatOberver();


            if (ChatHackerConfigurationByJson.Instance.IsUserRequestToHideInterface())
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
            restreamChat.OpenPage(ChatHackerConfigurationByJson.Instance.GetRestreamChatURL());
            // memory.SetMaximumMessageSizeTo(ChatHackerConfiguration.Instance.MaximumMessageSize);
            Thread t = new Thread(new ThreadStart(CheckForNewMessages));
            m_instanceRunning = new SeleniumThreadRunning();
            m_instanceRunning.m_messagesInMemory = memory;
            m_instanceRunning.m_selenium = restreamChat;
            m_instanceRunning.m_runningThread = t;
            m_instanceRunning.m_timeBetweenFrameInMs = ChatHackerConfigurationByJson.Instance.GetTimeBetweenChatCheckRequrested();
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
                ChatHackerConfigurationByJson.Instance.m_useOsc)
            sendOSCMessages = new ThrowOSC(
                ChatHackerConfigurationByJson.Instance.m_oscAddress,
                ChatHackerConfigurationByJson.Instance.m_oscPort );

            if (sendUDPListeners == null &&
                ChatHackerConfigurationByJson.Instance.m_useUdp) {
                string[] ips = ChatHackerConfigurationByJson.Instance.m_udpAddressListeners;
                sendUDPListeners = new MessageCommunication.IThrow[ips.Length];
                for (int i = 0; i < ips.Length; i++)
                {
                    sendUDPListeners[i] = new ThrowUDP(
                    ips[i],
                    ChatHackerConfigurationByJson.Instance.m_udpPort);
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
}
