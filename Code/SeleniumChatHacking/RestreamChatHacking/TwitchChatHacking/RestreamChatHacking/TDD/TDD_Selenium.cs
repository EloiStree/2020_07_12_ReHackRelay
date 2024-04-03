using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestreamChatHacking;

    public class TDD_Selenium
    {
    public static SeleniumAccessToRestreamChat restreamChat;
        public static void Main(string[] args)
    {
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

        StartSeleniumAndOpenPage();

        bool stopTheTest = false;
        while (!stopTheTest)
        {
            Console.Out.WriteLine("----- PAGE ------");
            if (!restreamChat.IsDriverInstanciated())
                Console.Out.WriteLine("Diver seems not crated xD");
            else if (!restreamChat.IsNavigatorOpen()) { 
                Console.Out.WriteLine("Navigator seems closed");
                Console.Out.WriteLine("Do you want to restart (y) ? ");
                if (Console.In.ReadLine().Trim().ToLower() == "y")
                    StartSeleniumAndOpenPage();
                else stopTheTest = true;
            }
            else Console.Out.WriteLine(restreamChat.GetRestreamHtmlPageInformation());

            Thread.Sleep(3000);
        }

    }

    private static void StartSeleniumAndOpenPage()
    {
        restreamChat = new SeleniumAccessToRestreamChat();
        restreamChat.SetupSeleniumDriver(false, SeleniumAccessToRestreamChat.NavigatorType.Chrome);
        restreamChat.OpenPage("https://www.youtube.com/watch?v=hJgQCbRsq-I");
    }

    static void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
        restreamChat.TeardownRunningDriver();
    }
}

