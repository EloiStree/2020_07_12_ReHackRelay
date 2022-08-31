using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking
{
    class ConsoleCommunication
    {
        public static void HelloWorldAndCredit()
        {
            ///HELLO
            Console.Out.WriteLine("#######################################################");
            Console.Out.WriteLine("######  Hello & welcome to Restream Chat Relay  ######");
            Console.Out.WriteLine("#######################################################");
            Console.Out.WriteLine("> Config stored in " + RestreamAppData.RestreamAppDataPath);
            Console.Out.WriteLine("> Code Restream Hack: " + "https://gitlab.com/eloistree/2017_12_23_RestreamChatHacking");
            Console.Out.WriteLine("> Code Unity GitHub & Manual: " + "https://github.com/EloiStree/2020_07_12_ReHackRelay");
            Console.Out.WriteLine("> Issue to report: " + "https://eloistree.page.link/discord");
            Console.Out.WriteLine("> License: " + "https://eloistree.page.link/license");
            Console.Out.WriteLine("> Credit: " + "Strée Eloi - https://ko-fi.com/eloistree");
            Console.Out.WriteLine("#######################################################");

            Console.Out.WriteLine(" ");
            Console.Out.WriteLine(" ");
            Console.Out.WriteLine(" ");
        }

        internal static void WelcomeTheNewUser()
        {
            Console.Out.WriteLine("Hello There :) ... ");
            Console.Out.WriteLine("I see it is your first time.");
            Console.Out.WriteLine("You can edit Config.json in AppData to set your preference:");
            Console.Out.WriteLine(RestreamAppData.ConfigurationPath);

            Console.Out.WriteLine("To work the app need: Ip(s) to target(s), Port, Twitch Chat link.");
            Console.Out.WriteLine("When providing, a chrome browser will open and Hide");
            Console.Out.WriteLine("The app only work if:\n- This console is open\n- Good Restream link was given\n-The launched chome window is open");
            Console.Out.WriteLine("Nothing happens... (o_O)");
            Console.Out.WriteLine("Yeah, I know. It is because you can't see UDP message with your eyes, you need an app to see them.");
            
            Console.Out.WriteLine("You can download my default one here: ");

            Console.Out.WriteLine("\nAs you are new, could you give the following basic information?\n");
            ConsoleCommunication.AskForTargetUdpPort();
            ConsoleCommunication.AskForTargetUdpIps();
        }

        internal static void AskForTargetUdpIps()
        {
            List<string> ips = new List<string>();
            ips.AddRange(ChatHackerConfiguration.Instance.m_udpAddressListeners);
            string answer = "";
            while (answer != "stop") { 
                answer = AskQuestion("Could you enter the Ips to target ?", "- 127.0.0.1 Is alway targeted.\n- Type 'stop' to stop entering targets\n- 'clear' to remove all except default");
                if (answer == "clear")
                    ips.Clear();
                else if (answer == "stop") { }
                else ips.Add(answer.Trim());
            }
            ChatHackerConfiguration.Instance.m_udpAddressListeners = ips.ToArray();
        }

        internal static void AskForTargetUdpPort()
        {
          string port=  AskQuestion("Could you enter the UDP port you want to use ?", "Current:" + ChatHackerConfiguration.Instance.m_udpPort);
            int portInt;
            if (int.TryParse(port, out portInt))
                ChatHackerConfiguration.Instance.m_udpPort = portInt;
        }

        public static void AskForRestreamEmbedLink()
        {
            string answer = "";
            // Is user want to define a new restream ?
            //    !Empty = yes => store it in config
            //    Save the new config  

            if (ChatHackerConfiguration.Instance.IsRestreamDefined())
            {
                answer = AskQuestion("Do you want to use this link for Twitch IRC ?\n"
                    + ChatHackerConfiguration.Instance.GetRestreamChatURL()
                    , "(N)o or any keys to continue");

                if (IsNo(answer))
                {
                    DefinedRestreamChatURL();

                }
            }
            else
            {

                DefinedRestreamChatURL();

            }
        }
        public static void DefinedRestreamChatURL()
        {
            string answer = "";
            do
            {
                answer = AskQuestion("Could you enter the Restream IRC embed link ?",
                   "Enter to the Restream IRC link to continue");
            } while (!(answer.Trim().ToLower().StartsWith("http")));
            //QUESTION TO ME:  SHOUD I CHECK IF THE ANSWER IS A  URL ???
            ChatHackerConfiguration.Instance.SetRestreamChatURL(answer);
        }
        public static string AskQuestion(string question, string proposition)
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
        public static bool IsNo(string answer)
        {
            return answer.ToLower().Trim().StartsWith("n");
        }
        public static bool IsYes(string answer)
        {
            return answer.ToLower().Trim().StartsWith("y");
        }
        public static void DisplayMessage(RestreamChatMessage message)
        {
            Console.WriteLine(string.Format(">> {3} | {0},{1}:{2}", message.UserName, message.When, message.Message, message.Platform));
        }
    }
}
