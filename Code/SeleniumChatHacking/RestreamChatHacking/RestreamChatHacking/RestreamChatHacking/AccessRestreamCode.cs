
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

namespace RestreamChatHacking
{
    public class AccessRestreamCode
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        
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
            driver.Navigate().GoToUrl("https://restream.io/chat-app/v1/?theme=boxed&aligment=top&msgOpacity=15&chatOpacity=100&scale=150&timeout=60&hideMessages=false&userId=338979&token=SNNrJr2M8VvSJXZnCjxG");

            //int i = 3;
             string firstDisplay = driver.PageSource;

            //while (i > 0)
             while (true)
            {


                Console.WriteLine("---------------------------LOADING PAGE-------------------------");
                Console.WriteLine("################################################################");
                string pageCode = driver.PageSource;
                File.WriteAllText(Environment.CurrentDirectory + "/Restream.html", pageCode);
                var element = driver.FindElements(By.XPath("//*[contains(@class, 'message-item')]"));
                Console.WriteLine("Message count:"+element.Count);

                for (int i = 0; i < element.Count; i++)
                {
                    string messageRaw = "<A>" + element[i].GetAttribute("innerHTML") + "</A>";
                    
                    RestreamTchatMessage message = new RestreamTchatMessage();
                    message.UserName = GetValueOf(messageRaw, "message-sender");
                    message.When = GetValueOf(messageRaw, "message-time");
                    message.Message = GetValueOf(messageRaw, "message-text");
                    message.SetPlatform( GetPlatformId(messageRaw));
                    Console.WriteLine("Element[" + i + "]:" + message.ToString());



                    //var sender = element[i].FindElement(By.XPath("//div[contains(@class, 'message-sender')]"));
                    //if(sender!=null)
                    //Console.WriteLine("Sender:" + sender.Text);

                }
                



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
