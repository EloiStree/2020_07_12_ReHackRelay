
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

        public int _frameTiming=250;

        public bool _useDebug = false;

        public void Setup(bool useDebug )
        {
            _useDebug = useDebug;
            driver = new ChromeDriver();
            baseURL = "https://www.jams.center/";
            verificationErrors = new StringBuilder();

            
        }
        
        public void Teardown()
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

        public void StartToListenAtRestreamEmbedUrl(string embedUrl)
        {   
            driver.Navigate().GoToUrl(embedUrl);

            int iChecKCount = 0;
            string firstDisplay = driver.PageSource;

            //while (i > 0)
            while (true)
            {
                string pageCode ="";
                //try
                //{
                    iChecKCount++;



                    if (_useDebug)
                        Console.WriteLine("---------------------------LOADING PAGE-------------------------");

                    if (_useDebug) Console.WriteLine("################################################################");

                    pageCode = driver.PageSource;
                    File.WriteAllText(Program.AppData.RestreamAppDataPath + "/Restream.html", pageCode);
                    var element = driver.FindElements(By.XPath("//*[contains(@class, 'message-item')]"));
                    int messagesFound = element.Count;

                    if (_useDebug)
                        Console.WriteLine("Message count:" + element.Count);

                    for (int i = 0; i < element.Count; i++)
                {
                    string messageRaw = "<div>" + element[i].GetAttribute("innerHTML") + "</div>";

                    RestreamChatMessage message = new RestreamChatMessage();
                    message.SetDateToNow();
                    message.Message = GetValueOf(messageRaw, "message-text");
                    message.UserName = GetValueOf(messageRaw, "message-sender");
                    message.When = GetValueOf(messageRaw, "message-time");
                    message.SetPlatform(GetPlatformId(messageRaw));

                    if (_useDebug)
                        Console.WriteLine("Element[" + i + "]:" + message.ToString());
                    NotifyNewMessageIfNew( message);

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

                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("############################# LOADED PAGE ###################################");
                //    Console.WriteLine(""+pageCode); 
                //    Console.WriteLine("############################# EXCEPTION ###################################");
                //    Console.Error.WriteLine(e);
                //}
             
            }
        }

        private void NotifyNewMessageIfNew( RestreamChatMessage message)
        {
            bool isMessageNew = IsMessageNew(message);
            if (isMessageNew)
            {

                _lastMessages.Enqueue(message);
                if (_lastMessages.Count > _maxQueueSize)
                    _lastMessages.Dequeue();
                if (_onMessageDetected != null)
                    _onMessageDetected(message);
            }
        }

        internal void FakeMessage(string userName, string message, RestreamChatMessage.ChatPlatform mockup)
        {
          RestreamChatMessage chatMessage =  new RestreamChatMessage(userName, message);
            chatMessage.SetPlatform(mockup);
            chatMessage.SetDateToNow();
            NotifyNewMessageIfNew(chatMessage);
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
            if (string.IsNullOrEmpty(text))
                return (int) RestreamChatMessage.ChatPlatform.Unknow;
            return Int32.Parse(Regex.Match(text, @"\d+").Value);
        }

        private static string GetValueOf(string messageRaw, string attribute)
        {


            //string[] images;
            //if(Find(messageRaw, out images));
            //{
            //    foreach (var image in images)
            //    {
            //        string url = FindUrlInImage(image);
            //        messageRaw.Replace(image, "![img]("+url+")");
            //    }
            //}

            string messageRawWithoutImage = RemoveImagesFrom(messageRaw,"");


            try
            {

                string value;
                var xPathDoc = new XPathDocument(new StringReader(messageRawWithoutImage));
                var nav = xPathDoc.CreateNavigator();
                var sender = nav.Select("//*[contains(@class, '" + attribute + "')]");
                sender.MoveNext();
                value = sender.Current.Value;
                return value;
            }
            catch (Exception e)
            {
                if (ChatHackerConfiguration.Instance.DebugOption.DisplayMessage)
                    Console.WriteLine("" + messageRaw);
                Console.WriteLine("############################# Message ###################################");
                Console.WriteLine("" + messageRaw);
                Console.WriteLine("############################# EXCEPTION ###################################");
                Console.Error.WriteLine(e);
                throw new MessageNotReadableException();
            }
            finally
            {
            }
        }

        private static int GetPlatformId(string messageRaw)
        {
            string messageRawWithoutImage = RemoveImagesFrom(messageRaw, "");
            
            var xPathDoc = new XPathDocument(new StringReader(messageRaw));
            var nav = xPathDoc.CreateNavigator();
            var sender = nav.Select("//*[contains(@class, 'icon-platform')]");
            sender.MoveNext();
            sender.Current.MoveToFirstAttribute();
            return GetNumberIn(sender.Current.Value);
        }

        public static string urlInSrcPatter = "src\\s*=\\s*\"(.+?)\"";
        public static string htmlImageTagPattern = "<img\\s[^>]*?src\\s*=\\s*['\\\"]([^'\\\"]*?)['\\\"][^>]*?>";
        private static string RemoveImagesFrom(string messageRaw,string replacement="[img]")
        {
            
            return Regex.Replace(messageRaw, htmlImageTagPattern, replacement);
        }
        private static bool Find(string messageRaw, out string [] imagesFound)
        {

            List<string> results = new List<string>();
            foreach (Match match in Regex.Matches(messageRaw, htmlImageTagPattern)) {
               // Console.WriteLine("Found '{0}' at position {1}",match.Value, match.Index);
                results.Add(match.Value);
            }
            imagesFound= results.ToArray<string>();
            return imagesFound.Length>0;
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
