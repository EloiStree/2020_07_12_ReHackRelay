
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
        private IWebDriver m_driver;
        private StringBuilder m_verificationErrors;
        private string m_baseURL;
        private bool m_acceptNextAlert = true;

        public delegate void SignalMessage(RestreamChatMessage message);
        public SignalMessage m_onMessageDetected;
        public Queue<RestreamChatMessage> m_lastMessages = new Queue<RestreamChatMessage>();
        public float m_maxQueueSize=300;
        public int m_maxMessageInPage = 500;

        public int m_frameTiming=250;

        public bool m_useDebug = false;
        private bool m_allowAllSize = true;
        public int m_maxMessageSize = 1014;

        public void Setup(bool useDebug )
        {
            m_useDebug = useDebug;
            m_driver = new ChromeDriver();
            m_baseURL = "https://ko-fi.com/eloistree";
            m_driver.Manage().Window.Size =new System.Drawing.Size(300, 200);
            m_verificationErrors = new StringBuilder();

            
        }
        
        public void TeardownRunningServer()
        {
            try
            {
                m_driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public void StartToListenAtRestreamEmbedUrl(string embedUrl)
        {   
            m_driver.Navigate().GoToUrl(embedUrl);

            int iChecKCount = 0;
            string firstDisplay = m_driver.PageSource;

            //while (i > 0)
            while (true)
            {
                string pageCode ="";
                //try
                //{
                    iChecKCount++;



                    if (m_useDebug)
                        Console.WriteLine("---------------------------LOADING PAGE-------------------------");

                    if (m_useDebug) Console.WriteLine("################################################################");

                    pageCode = m_driver.PageSource;
                    //File.WriteAllText(Program.AppData.RestreamAppDataPath + "/Restream.html", pageCode);
                    var element = m_driver.FindElements(By.XPath("//*[contains(@class, 'message-item')]"));
                    int messagesFound = element.Count;

                    if (m_useDebug)
                        Console.WriteLine("Message count:" + element.Count);

                    for (int i = 0; i < element.Count; i++)
                    {
                        string messageRaw = "<div>" + element[i].GetAttribute("innerHTML") + "</div>";

                        RestreamChatMessage message = new RestreamChatMessage();
                        message.SetDateToNow();
                        string messageRecovered = GetValueOf(messageRaw, "message-text");
                        messageRecovered = CutIfAskedToWantedSize(messageRecovered);
                        message.Message = messageRecovered;
                        message.UserName = GetValueOf(messageRaw, "message-sender");
                        message.When = GetValueOf(messageRaw, "message-time");
                        message.SetPlatform(GetPlatformId(messageRaw));

                        if (m_useDebug)
                            Console.WriteLine("Element[" + i + "]:" + message.ToString());
                        NotifyNewMessageIfNew(message);

                    }




                    if (m_useDebug)
                        Console.WriteLine("Refresh page in " + (m_maxMessageInPage - messagesFound) + " messages");
                    if (messagesFound > m_maxMessageInPage)
                        m_driver.Navigate().Refresh();



                    if (m_useDebug)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("");
                    }
                    m_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    System.Threading.Thread.Sleep(m_frameTiming);
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

        private string CutIfAskedToWantedSize(string messageRecovered)
        {
            if (messageRecovered != null && !m_allowAllSize)
                messageRecovered = messageRecovered.Substring(0, m_maxMessageSize);
            return messageRecovered;
        }

        public void SetMaximumMessageSizeTo(int maximumMessageSize)
        {
            m_maxMessageSize = maximumMessageSize;
        }

        public void SetAllowingAllSize(bool allowAllSize)
        {
            m_allowAllSize = allowAllSize;
        }

        private void NotifyNewMessageIfNew( RestreamChatMessage message)
        {
            bool isMessageNew = IsMessageNew(message);
            if (isMessageNew)
            {

                m_lastMessages.Enqueue(message);
                if (m_lastMessages.Count > m_maxQueueSize)
                    m_lastMessages.Dequeue();
                if (m_onMessageDetected != null)
                    m_onMessageDetected(message);
            }
        }

        public void FakeMessage(string userName, string message, ChatPlatform mockup)
        {
            message = CutIfAskedToWantedSize(message);
            RestreamChatMessage chatMessage =  new RestreamChatMessage(userName, message);
            chatMessage.SetPlatform(mockup);
            chatMessage.SetDateToNow();
            NotifyNewMessageIfNew(chatMessage);
        }

        //public bool IsMessageNew(string messageId)
        //{
        //    var result = m_lastMessages.Where(p => p.Message == messageId);
        //    return result.Count() <= 0;
        //}
        public bool IsMessageNew(RestreamChatMessage messageId)
        {
            var result = m_lastMessages.Where(p => p.Equals(messageId));
            return result.Count() <= 0;
        }

        private static ChatPlatform GetNumberIn(string text)
        {
            text = text.ToLower();
            if ( text.IndexOf("restream-icon-orange.svg") >=0)
                return ChatPlatform.Restream;
            if ( text.IndexOf("platform-1001.png") >= 0)
                return ChatPlatform.Discord;
            if (text.IndexOf("platform-37.png") >= 0)
                return ChatPlatform.Facebook;
            if (text.IndexOf("platform-38.png") >= 0)
                return ChatPlatform.Periscope;
            if (text.IndexOf("platform-57.png") >= 0)
                return ChatPlatform.DLive;
            //if (text.IndexOf("platform-xxx.png") >= 0)
            //    return RestreamChatMessage.ChatPlatform.LinkedIn;
            if ( text.IndexOf("platform-1.png") >= 0)
                return ChatPlatform.Twitch;
            if (text.IndexOf("platform-25.png") >= 0 ||
                text.IndexOf("platform-5.png") >= 0)
                return ChatPlatform.Youtube;
            return ChatPlatform.Unknow;
        }

        private static string GetValueOf(string messageRaw, string attribute)
        {
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

        private static ChatPlatform GetPlatformId(string messageRaw)
        {
            string messageRawWithoutImage = RemoveImagesFrom(messageRaw, "");
            return GetNumberIn(messageRaw);
            //
            ///"<div><div class=\"message-info\">
            /// <img class=\"icon-platform\" alt=\"Restream.io\" src=\"/assets/icon-platform/restream-icon-orange.svg\">
            /// <span class=\"message-sender\"><span></span>Restream.io</span>
            /// <div class=\"message-time\">21:23:13</div></div>
            /// <div class=\"message-text\"><span class=\"jss8\">The chat is ready to display messages.</span> </div>
            ///</div>"
            //var xPathDoc = new XPathDocument(new StringReader(messageRawWithoutImage));
            //var nav = xPathDoc.CreateNavigator();
            //var sender = nav.Select("//*[contains(@class, 'message-sender')]");
            //sender.MoveNext();
            ////sender.Current.MoveToFirstAttribute();
            //return GetNumberIn(sender.Current.Value);
        }

        public static string urlInSrcPatter = "src\\s*=\\s*\"(.+?)\"";
//        public static string htmlImageTagPattern = "<img\\s[^>]*?src\\s*=\\s*['\\\"]([^'\\\"]*?)['\\\"][^>]*?>";
        public static string htmlImageTagPattern = "<img[^>]*>";
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
                m_driver.FindElement(by);
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
                m_driver.SwitchTo().Alert();
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
                IAlert alert = m_driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (m_acceptNextAlert)
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
                m_acceptNextAlert = true;
            }
        }
    }
}
