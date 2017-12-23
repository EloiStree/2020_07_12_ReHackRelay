using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class AccessRestreamCode
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "https://www.katalon.com/";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
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
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheAccessRestreamCodeTest()
        {
            driver.Navigate().GoToUrl("https://restream.io/chat-app/v1/?theme=boxed&aligment=top&msgOpacity=15&chatOpacity=100&scale=150&timeout=60&hideMessages=false&userId=338979&token=SNNrJr2M8VvSJXZnCjxG");
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div/div[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[3]/div[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[3]/div[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div/span[3]")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div/span[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div[2]")).Click();
            driver.FindElement(By.XPath("//div[@id='jsMessagesBlock']/div[2]/div/span")).Click();
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
