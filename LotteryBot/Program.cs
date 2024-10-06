using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

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

            TestBattery();

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

            driverOptions.AddAdditionalAppiumOption("appPackage", "com.android.settings");
            driverOptions.AddAdditionalAppiumOption("appActivity", ".Settings");
            // NoReset assumes the app com.google.android is preinstalled on the emulator
            driverOptions.AddAdditionalAppiumOption("noReset", true);

            var driver = new AndroidDriver(serverUri, driverOptions, TimeSpan.FromSeconds(180));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        static void TestBattery()
        {
            _driver.StartActivity("com.android.settings", ".Settings");
            _driver.FindElement(By.XPath("//*[@text='Battery']")).Click();
        }

        static public void TearDown()
        {
            _driver.Dispose();
        }
    }
}