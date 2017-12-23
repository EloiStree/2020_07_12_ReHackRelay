using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;


namespace RestreamChatHacking
{
    class Program
    {
        static void Main(string[] args)
        {

            AccessRestreamCode d = new AccessRestreamCode();

            d.SetupTest();
            d.TheAccessRestreamCodeTest();
            d.TeardownTest();

            string answer = "";
            while (answer != "q") {
                Console.WriteLine("Do you want to (q)uit");
                answer =  Console.ReadLine();
            }

        }
    }
}
