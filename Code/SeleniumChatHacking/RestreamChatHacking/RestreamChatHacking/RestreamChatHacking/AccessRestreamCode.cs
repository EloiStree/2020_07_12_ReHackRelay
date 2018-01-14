
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Linq;
namespace RestreamChatHacking
{
    public class AccessRestreamCode
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        public delegate void SignalMessage(RestreamChatMessage message);
        public SignalMessage _onMessageDetected;
        public Queue<RestreamChatMessage> _lastMessages = new Queue<RestreamChatMessage>();
        public float _maxQueueSize=300;
        public int _maxMessageInPage = 500;//1000;

        public int _frameTiming=500;

        public bool _useDebug = false;

        public void SetupTest(bool useDebug )
        {
            _useDebug = useDebug;
            driver = new ChromeDriver();
            baseURL = "https://www.katalon.com/";
            verificationErrors = new StringBuilder();

            
        }
        
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public void TheAccessRestreamCodeTest()
        {
            //            driver.Navigate().GoToUrl("https://restream.io/chat-app/v1/?theme=boxed&aligment=top&msgOpacity=15&chatOpacity=100&scale=150&timeout=60&hideMessages=false&userId=338979&token=SNNrJr2M8VvSJXZnCjxG");
            driver.Navigate().GoToUrl("https://restream.io/embed-chat?id=338979&guid=67248fed28fb4894a626c041e7d9fa3d");
            //https://restream.io/embed-chat?id=338979&guid=67248fed28fb4894a626c041e7d9fa3d
            //http://restream.io/webchat?id=338979&guid=67248fed28fb4894a626c041e7d9fa3d
            //http://restream.io/webchat?id=338979&guid=67248fed28fb4894a626c041e7d9fa3d


            int iChecKCount = 0;
            string firstDisplay = driver.PageSource;

            //while (i > 0)
            while (true)
            {
                try
                {
                    iChecKCount++;



                    if (_useDebug)
                        Console.WriteLine("---------------------------LOADING PAGE-------------------------");

                    if (_useDebug) Console.WriteLine("################################################################");

                    string pageCode = driver.PageSource;
                    File.WriteAllText(Environment.CurrentDirectory + "/Restream.html", pageCode);
                    var element = driver.FindElements(By.XPath("//*[contains(@class, 'message-item')]"));
                    int messagesFound = element.Count;

                    if (_useDebug)
                        Console.WriteLine("Message count:" + element.Count);

                    for (int i = 0; i < element.Count; i++)
                    {
                        string messageRaw = "<A>" + element[i].GetAttribute("innerHTML") + "</A>";

                        RestreamChatMessage message = new RestreamChatMessage();
                        message.SetDateToNow();
                        message.UserName = GetValueOf(messageRaw, "message-sender");
                        message.When = GetValueOf(messageRaw, "message-time");
                        message.Message = GetValueOf(messageRaw, "message-text");
                        message.SetPlatform(GetPlatformId(messageRaw));


                        bool isMessageNew = IsMessageNew(message);
                        if (isMessageNew)
                        {

                            if (_useDebug)
                                Console.WriteLine("Element[" + i + "]:" + message.ToString());
                            _lastMessages.Enqueue(message);
                            if (_lastMessages.Count > _maxQueueSize)
                                _lastMessages.Dequeue();
                            if (_onMessageDetected != null)
                                _onMessageDetected(message);
                        }

                        

                    }




                    if (_useDebug)
                        Console.WriteLine("Refresh page in " + (_maxMessageInPage - messagesFound) + " messages");
                    if (messagesFound > _maxMessageInPage)
                        driver.Navigate().Refresh();



                    if (_useDebug)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("");
                    }
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    System.Threading.Thread.Sleep(_frameTiming);
                    //i--;

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
             
            }
        }

        public bool IsMessageNew(string messageId)
        {
            var result = _lastMessages.Where(p => p.Message == messageId);
            return result.Count() <= 0;
        }
        public bool IsMessageNew(RestreamChatMessage messageId)
        {
            var result = _lastMessages.Where(p => p.Equals(messageId));
            return result.Count() <= 0;
        }

        private static  int GetNumberIn(string text)
        {
            return Int32.Parse(Regex.Match(text, @"\d+").Value);
        }

        private static string GetValueOf(string messageRaw, string attribute)
        {
            string value;
            var xPathDoc = new XPathDocument(new StringReader(messageRaw));
            var nav = xPathDoc.CreateNavigator();
            var sender = nav.Select("//*[contains(@class, '" + attribute + "')]");
            sender.MoveNext();
            value = sender.Current.Value;
            return value;
        }
        private static int GetPlatformId(string messageRaw)
        {
            string value;
            var xPathDoc = new XPathDocument(new StringReader(messageRaw));
            var nav = xPathDoc.CreateNavigator();
            var sender = nav.Select("//*[contains(@class, 'icon-platform')]");
            sender.MoveNext();
            sender.Current.MoveToFirstAttribute();
          
            return GetNumberIn(sender.Current.Value);
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
