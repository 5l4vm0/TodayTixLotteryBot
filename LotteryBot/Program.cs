using OpenQA.Selenium;
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
            
            TodayTix();

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


        static void TodayTix()
        {
            _driver.StartActivity("com.todaytix.TodayTix", ".activity.MainActivity");


            _driver.FindElement(By.XPath("//*[@text='Account']")).Click();

            if(_driver.FindElement(By.XPath("//*[@text='Email address']"))!= null)
            {
                var email = _driver.FindElement(By.XPath("//*[@text='Email address']"));
                email.Click();  
                email.Clear();  
                Console.WriteLine("Sleep for 2 seconds.");
                Thread.Sleep(2000);
                email.SendKeys("usvienaspirmas@gmail.com");
            }
            
            
        }

        static public void TearDown()
        {
            _driver.Dispose();
        }
    }
}