
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

        public delegate void SignalMessage(RestreamTchatMessage message);
        public SignalMessage _onMessageDetected;
        public Queue<RestreamTchatMessage> _lastMessages = new Queue<RestreamTchatMessage>();
        public float _maxQueueSize=300;
        public int _maxMessageInPage = 1000;//1000;

        public void SetupTest()
        {
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
            driver.Navigate().GoToUrl("http://restream.io/webchat?id=338979&guid=67248fed28fb4894a626c041e7d9fa3d");
            //http://restream.io/webchat?id=338979&guid=67248fed28fb4894a626c041e7d9fa3d
            //http://restream.io/webchat?id=338979&guid=67248fed28fb4894a626c041e7d9fa3d


           int iChecKCount = 0;
            string firstDisplay = driver.PageSource;

            //while (i > 0)
             while (true)
            {
                iChecKCount++;




                Console.WriteLine("---------------------------LOADING PAGE-------------------------");
                Console.WriteLine("################################################################");

                string pageCode = driver.PageSource;
                File.WriteAllText(Environment.CurrentDirectory + "/Restream.html", pageCode);
                var element = driver.FindElements(By.XPath("//*[contains(@class, 'message-item')]"));
                int messagesFound = element.Count;
                Console.WriteLine("Message count:"+element.Count);

                for (int i = 0; i < element.Count; i++)
                {
                    string messageRaw = "<A>" + element[i].GetAttribute("innerHTML") + "</A>";

                    RestreamTchatMessage message = new RestreamTchatMessage();
                    message.UserName = GetValueOf(messageRaw, "message-sender");
                    message.When = GetValueOf(messageRaw, "message-time");
                    message.Message = GetValueOf(messageRaw, "message-text");
                    message.SetPlatform(GetPlatformId(messageRaw));

                    bool isMessageNew=  IsMessageNew(message.Id);
                    if (isMessageNew) {
                        Console.WriteLine("Element[" + i + "]:" + message.ToString());
                        _lastMessages.Enqueue(message);
                        if (_lastMessages.Count > _maxQueueSize)
                            _lastMessages.Dequeue();
                        if (_onMessageDetected != null)
                            _onMessageDetected(message);
                    }


                    //var sender = element[i].FindElement(By.XPath("//div[contains(@class, 'message-sender')]"));
                    //if(sender!=null)
                    //Console.WriteLine("Sender:" + sender.Text);

                }



                Console.WriteLine("Refresh page in " + (_maxMessageInPage - messagesFound) + " messages");
                if (messagesFound > _maxMessageInPage)
                    driver.Navigate().Refresh();


                Console.WriteLine("");
                Console.WriteLine("");

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                System.Threading.Thread.Sleep(5000);
                //i--;

            }
            
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div/div[2]")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div[2]")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[3]/div[2]")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[3]/div[2]")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div/span[3]")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div/span[2]")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div[2]")).Click();
            //driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div/span")).Click();

        }

        public bool IsMessageNew(string messageId)
        {
            var result = _lastMessages.Where(p => p.Id == messageId);
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
