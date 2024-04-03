using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking
{
    public class BatchToLaunch
    {
        public static void TryLaunchRestream()
        {
            if (ChatHackerConfiguration.Instance.m_launchRestreamWithTheApp)
            {
               

                    Console.WriteLine("Try Auto-Launch Restream.");
                LaunchBat("StartRestreamChat");
                

                
            }
        }
        private static void LaunchBat(string batName)
        {
            string path = Environment.CurrentDirectory + "\\" + batName + ".bat";
            if (File.Exists(path))
                System.Diagnostics.Process.Start(path);
        }

        public static void LaunchStreaming(RestreamChatMessage message)
        {
            if (ChatHackerConfiguration.Instance.m_allowSteamLaunchingFromChat)
            {
                if (message.Message.Contains(ChatHackerConfiguration.Instance.m_startStreamKeyword))
                {

                    Console.WriteLine("Action: Try to launch streaming");

                    LaunchBat("StartStreamingOBS");
                    
                }
            }
        }

        public static void StopStreaming(RestreamChatMessage message)
        {
            if (ChatHackerConfiguration.Instance.m_allowSteamKillingFromChat)
            {

                if (message.Message.Contains(ChatHackerConfiguration.Instance.m_killStreamWord))
                {
                    Console.WriteLine("Action: Try to stop streaming");
                    LaunchBat("StopStreaming");
                }
            }
        }

    }
}
