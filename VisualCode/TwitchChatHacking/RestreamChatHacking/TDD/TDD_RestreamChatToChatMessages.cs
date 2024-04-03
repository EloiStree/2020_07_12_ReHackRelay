using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking.TDD
{
    class TDD_RestreamChatToChatMessages
    {
        public static SeleniumAccessToRestreamChat restreamChat;
        public static void Main(string[] args)
        {
            string path = Environment.CurrentDirectory + "/RestreamChatExample.html";
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            Console.Out.WriteLine("Project Path:"+Environment.CurrentDirectory);
            string htmlCode = File.ReadAllText(path);

            List<string> messagesAsHtml;
            List<RestreamChatMessage> foundMessageInPage;
            HTML2Messages.GetMessagesInHTML(htmlCode, out messagesAsHtml, out foundMessageInPage);

            for (int i = 0; i < messagesAsHtml.Count; i++)
            {
                Console.Out.WriteLine("Found:\n" + messagesAsHtml[i]);

            }
            for (int i = 0; i < foundMessageInPage.Count; i++)
            {
                Console.Out.WriteLine("One Line:\n" + foundMessageInPage[i].GetAsOneLiner());

            }
            Console.In.ReadLine();

        }


        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {

        }
    }
}

