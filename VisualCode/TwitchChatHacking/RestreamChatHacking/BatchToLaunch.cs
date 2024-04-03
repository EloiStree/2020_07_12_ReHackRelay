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
            if (ChatHackerConfigurationByJson.Instance.m_launchRestreamWithTheApp)
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
            if (ChatHackerConfigurationByJson.Instance.m_allowSteamLaunchingFromChat)
            {
                if (message.Message.Contains(ChatHackerConfigurationByJson.Instance.m_startStreamKeyword))
                {

                    Console.WriteLine("Action: Try to launch streaming");

                    LaunchBat("StartStreamingOBS");
                    
                }
            }
        }

        public static void StopStreaming(RestreamChatMessage message)
        {
            if (ChatHackerConfigurationByJson.Instance.m_allowSteamKillingFromChat)
            {

                if (message.Message.Contains(ChatHackerConfigurationByJson.Instance.m_killStreamWord))
                {
                    Console.WriteLine("Action: Try to stop streaming");
                    LaunchBat("StopStreaming");
                }
            }
        }

    }
}
