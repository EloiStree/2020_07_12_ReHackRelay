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
using OpenQA.Selenium.IE;
using System.Drawing;

namespace RestreamChatHacking
{
    public class SeleniumAccessToRestreamChat
    {


        private IWebDriver m_driver; 
        public enum NavigatorType { Firefox, Chrome, InternetExplorer }


        ////chat-settings-timestamp
        //public void ClickOnLabel(string name)
        //{
        //    m_driver.FindElement(By.Id(name)).Click();

        //}
        //public void ClickOnTimeStampLabel() {
        //    ClickOnLabel("chat-settings-timestamp");
        //}


        ~SeleniumAccessToRestreamChat() {
            if (m_driver!=null) { 
                m_driver.Close();
                m_driver.Dispose();
            }
        }
        public void SetupSeleniumDriver(bool useDebug, NavigatorType navigatorType)
        {
            if (m_driver == null) {

                if (navigatorType == NavigatorType.Firefox)
                    m_driver = new FirefoxDriver();
                else
                if (navigatorType == NavigatorType.InternetExplorer)
                    m_driver = new InternetExplorerDriver();
                else {
                    string pathDriver = Environment.CurrentDirectory+ "\\GoogleDriver";
                    Directory.CreateDirectory(pathDriver);
                    Console.WriteLine("Driver:" +pathDriver);
                    ChromeOptions options = new ChromeOptions();
                    options.AddArguments(@"user-data-dir="+ pathDriver);
                    m_driver = new ChromeDriver(pathDriver, options);
                 }

                //specify location for profile creation/ access


                Thread.Sleep(2000);
                if (ChatHackerConfiguration.Instance.IsUserRequestToHideInterface())
                {
                    m_driver.Manage().Window.Size = new System.Drawing.Size(0, 0);
                    m_driver.Manage().Window.Position = new System.Drawing.Point(-2000, 0);
                }
                else
                {
                    m_driver.Manage().Window.Size = new System.Drawing.Size(ChatHackerConfiguration.Instance.m_windowWidth, ChatHackerConfiguration.Instance.m_windowHeight);
                }

             
            }


        }
        public void OpenPage(string url) {
            if (m_driver == null)
                SetupSeleniumDriver(false, NavigatorType.Chrome);
            m_driver.Navigate().GoToUrl(url);
        }


        public bool IsNavigatorOpen()
        {
            bool isOpen = true;
            try
            {
                m_driver.WindowHandles.Count();
            }
            catch (InvalidOperationException e)
            {
                isOpen = false;
            }

            return isOpen;
        }


        public bool IsDriverInstanciated()
        {
            return m_driver != null;
        }
       

        public string GetRestreamHtmlPageInformation()
        {
            if (m_driver == null)
                return "";
            return m_driver.PageSource;
        }
        public void TeardownRunningDriver()
        {
            if (m_driver != null)
                m_driver.Quit();
            //try
            //{
            //    m_driver.Quit();
            //}
            //catch (Exception)
            //{

            //    // Ignore errors if unable to close the browser
            //}
        }

        /// <summary>
        /// I keep this code just in case I need it later. Was used in the V0 of the project.
        /// </summary>
        public class Archived {

            public void StartToListenAtRestreamEmbedUrl(string embedUrl)
            {
                //m_driver.Navigate().GoToUrl(url);
                while (true)
                {
                    //ARCHIVED WHILE LISTENING
                    //m_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    //System.Threading.Thread.Sleep(m_frameTiming);
                }

            }
            private bool IsAlertPresent()
            {
                try
                {
                   // m_driver.SwitchTo().Alert();
                    return true;
                }
                catch (NoAlertPresentException)
                {
                    return false;
                }
            }

            public bool m_acceptNextAlert;
            private string CloseAlertAndGetItsText()
            {
                try
                {
                    IAlert alert = null;// m_driver.SwitchTo().Alert();
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


}