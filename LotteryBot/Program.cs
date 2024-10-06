﻿using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using System.Threading;


namespace LotteryBot
{
    class Program
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private static AndroidDriver _driver;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        static void Main(string[] args)
        {
            _driver = SetUp();
            
            TodayTixSignUp();

            TodayTixSearchEvent();

            TearDown();
        }


        static AndroidDriver SetUp()
        {
            var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
            var driverOptions = new AppiumOptions() {
                AutomationName = AutomationName.AndroidUIAutomator2,
                PlatformName = "Android",
                DeviceName = "Android Emulator",
            };



            driverOptions.AddAdditionalAppiumOption("appPackage", "com.todaytix.TodayTix");
            driverOptions.AddAdditionalAppiumOption("appActivity", ".activity.MainActivity");

            // NoReset assumes the app com.google.android is preinstalled on the emulator
            driverOptions.AddAdditionalAppiumOption("noReset", true);

            var driver = new AndroidDriver(serverUri, driverOptions, TimeSpan.FromSeconds(180));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        static void PrintAllElements() {
            if(_driver.FindElements(By.XPath("//*")) != null)
            {
                var elements = _driver.FindElements(By.XPath("//*"));
                foreach(var element in elements)
                {
                    Console.WriteLine(element.Text);
                }
            }
        }

        static void PrintVisibleWidgets()
        {
            // Find all elements on the screen (using '//*' for all elements)
            var allElements = _driver.FindElements(By.XPath("//*"));

            Console.WriteLine("Visible Widgets:");
            
            // Loop through each element and print relevant attributes
            foreach (var element in allElements)
            {
                // Check if the element is visible
                if (element.Displayed)
                {
                    // Print relevant attributes (you can add more if needed)
                    var className = element.GetAttribute("class");
                    var resourceId = element.GetAttribute("resource-id");
                    var text = element.GetAttribute("text");
                    var contentDesc = element.GetAttribute("content-desc");

                    // Print the details
                    Console.WriteLine($"Class: {className}, Resource-ID: {resourceId}, Text: {text}, Content-Description: {contentDesc}");
                }
            }
        }



        static void TodayTixSignUp()
        {
            //Click on TodayTix app
            _driver.StartActivity("com.todaytix.TodayTix", ".activity.MainActivity");

            //Click on Accont tab
            _driver.FindElement(By.XPath("//*[@text='Account']")).Click();
            
            //Find Email address field, type in email, and click continue
            if(_driver.FindElement(By.XPath("(//android.widget.EditText)[1]")) != null)
            {
                var editText = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
                editText.Click();
                editText.SendKeys("usvienaspirmas+test001@gmail.com");

                var continueButton = _driver.FindElement(By.XPath("//*[@text='Continue']"));
                continueButton.Click();
            }

            //Find name test field, type in first name and second name, and click send link
            if(_driver.FindElement(By.XPath("(//android.widget.EditText)[1]")) != null)
            {
                var firstName = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
                firstName.Click();
                firstName.SendKeys("John");

                var lastName = _driver.FindElement(By.XPath("(//android.widget.EditText)[2]"));
                lastName.Click();
                lastName.SendKeys("Smith");

                var sendLinkButton = _driver.FindElement(By.XPath("//*[@text='Send link']"));
                sendLinkButton.Click();
            }

            //TODO: get code from email and type it in
            Console.WriteLine("Sleep for 20sec");
            Thread.Sleep(20000);

            //Click on Reject all for app activity usage
            try
            {
                if(_driver.FindElement(By.XPath("(//*[@text='Reject all'])")) != null)
                {
                    var rejectButton = _driver.FindElement(By.XPath("(//*[@text='Reject all'])"));
                    rejectButton.Click();
                }
            }
            catch
            {
                Console.WriteLine("No reject button found");
            }
            
        }


        

        static public void TearDown()
        {
            _driver.Dispose();
        }
    }
}