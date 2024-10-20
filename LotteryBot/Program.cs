using System.Data;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Interactions;



namespace LotteryBot
{
    class Program
    {
        private static AndroidDriver _driver;
        private static GmailMonitor _gmailMonitor;

        private static int _singUpAmount = 10;
        private static int _attemptEmailNum = 45;
        private static string _emailAddress = "usvienaspirmas+test";
        private static string _code;

        static void Main(string[] args)
        {
            _gmailMonitor = new GmailMonitor();


            _driver = SetUp();

            SignUpForTicketsLoop();

            TearDown();
        }

        static void SignUpForTicketsLoop()
        {
            for (int i = 0; i < _singUpAmount; i++)
            {
                _emailAddress = "usvienaspirmas+test";
                _emailAddress = $"{_emailAddress}{_attemptEmailNum:000}";
                Console.WriteLine($"StartingSignUP, Attemp:{i + 1}, email:{_attemptEmailNum}");
                TodayTixSignUp();

                TodayTixSearchEvent();

                EnterLottery();

                LogOut();
                Console.WriteLine($"FinsishedSignUP, Attemp:{i + 1}, email:{_attemptEmailNum}");
                _attemptEmailNum++;
            }
        }

        static AndroidDriver SetUp()
        {
            var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
            var driverOptions = new AppiumOptions()
            {
                AutomationName = AutomationName.AndroidUIAutomator2,
                PlatformName = "Android",
                DeviceName = "Android Emulator",
            };



            driverOptions.AddAdditionalAppiumOption("appPackage", "com.todaytix.TodayTix");
            driverOptions.AddAdditionalAppiumOption("appActivity", ".activity.MainActivity");

            // NoReset assumes the app com.google.android is preinstalled on the emulator
            driverOptions.AddAdditionalAppiumOption("noReset", true);

            var driver = new AndroidDriver(serverUri, driverOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        static void PrintAllElements()
        {
            if (_driver.FindElements(By.XPath("//*")) != null)
            {
                var elements = _driver.FindElements(By.XPath("//*"));
                foreach (var element in elements)
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
            Thread.Sleep(1000);
            //Click on Accont tab
            _driver.FindElement(By.XPath("//*[@text='Account']")).Click();

            //Find Email address field, type in email, and click continue
            if (_driver.FindElement(By.XPath("(//android.widget.EditText)[1]")) != null)
            {
                var editText = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
                editText.Click();
                editText.SendKeys(_emailAddress + "@gmail.com");

                var continueButton = _driver.FindElement(By.XPath("//*[@text='Continue']"));
                continueButton.Click();
            }

            //Find name test field, type in first name and second name, and click send link
            if (_driver.FindElement(By.XPath("(//android.widget.EditText)[1]")) != null)
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

            Console.WriteLine("Sleep for 5sec");
            Thread.Sleep(10000);
            GetSignUpCode().Wait();
            TypeInSignUpCode();

            //Click on Reject all for app activity usage
            try
            {
                if (_driver.FindElement(By.XPath("(//*[@text='Reject all'])")) != null)
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


        static void TodayTixSearchEvent()
        {
            //Click on Accont tab
            _driver.FindElement(By.XPath("//*[@text='Search']")).Click();

            //Type in Harry Potter in search bar
            if (_driver.FindElement(By.XPath("(//android.widget.EditText)[1]")) != null)
            {
                var searchBar = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
                searchBar.SendKeys("Harry Potter");
            }

            try
            {
                if (_driver.FindElement(By.Id("com.todaytix.TodayTix:id/show_name")) != null)
                {
                    var show = _driver.FindElement(By.Id("com.todaytix.TodayTix:id/show_name"));
                    show.Click();
                }
            }
            catch
            {
                Console.WriteLine("No show found");
            }


            ScrollUntilElementIsFound(_driver, "£40 Friday Forty", -500);
            try
            {
                if (_driver.FindElement(By.XPath("//*[@text='Enter Lottery']")) != null)
                {
                    _driver.FindElement(By.XPath("//*[@text='Enter Lottery']")).Click();
                }

                if (_driver.FindElement(By.XPath("//*[@text='Set alert']")) != null)
                {
                    _driver.FindElement(By.XPath("//*[@text='Set alert']")).Click();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //Scroll down a bit
        static public void ScrollUntilElementIsFound(AndroidDriver driver, string elementText, int scrollDistance)
        {
            bool elementFound = false;
            int attempt = 0;

            int maxAttempts = 5;

            while (!elementFound && attempt < maxAttempts)
            {

                try
                {
                    var element = driver.FindElement(By.XPath($"//*[@text='{elementText}']"));
                    Console.WriteLine($"Element found: {elementText}");
                    elementFound = true;
                    Actions action = new Actions(driver);
                    action.DragAndDropToOffset(element, 0, scrollDistance).Perform();
                }
                catch
                {
                    Console.WriteLine($"Element not found, scrolling down");

                }

                attempt++;
                Thread.Sleep(1000);
            }
        }

        static public async Task GetSignUpCode()
        {
            _code = await _gmailMonitor.GetCodeFromEmail(_emailAddress);
        }

        static public void TypeInSignUpCode()
        {
            if (_driver.FindElement(By.XPath("(//android.widget.EditText)[1]")) != null)
            {
                var editText = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
                editText.Click();
                editText.SendKeys(_code);
            }
        }

        static public void EnterLottery()
        {

            //Select 2 tickets
            if (_driver.FindElement(By.XPath("(//*[@text='2'])")) != null)
            {
                var twoTickets = _driver.FindElement(By.XPath("(//*[@text='2'])"));
                twoTickets.Click();
            }

            //Untick weekdays
            for (int i = 0; i <= 1; i++)
            {
                _driver.FindElements(By.Id("com.todaytix.TodayTix:id/date_label"))[i].Click();
            }

            _driver.FindElement(By.XPath("//*[@text='Next']")).Click();

            //Enter email address
            var emailAddress = _driver.FindElement(By.XPath("(//android.widget.EditText)[2]"));
            emailAddress.Click();
            emailAddress.SendKeys(_emailAddress + "@gmail.com");

            //Enter phone country code
            _driver.FindElement(By.XPath("(//android.widget.Spinner)[1]")).Click();
            var phoneCode = _driver.FindElement(By.XPath("(//*[@text='GB +44'])"));
            phoneCode.Click();

            //Enter phone number
            var phoneNumber = _driver.FindElement(By.XPath("(//android.widget.EditText)[3]"));
            phoneNumber.Click();
            long basePhoneNum = 7891234567;
            phoneNumber.SendKeys("0" + (basePhoneNum + _attemptEmailNum).ToString());

            //Click on enter lottery
            _driver.FindElement(By.XPath("(//*[@text='Enter'])")).Click();

            //Click on close pop up menu
            _driver.FindElement(By.Id("com.todaytix.TodayTix:id/close_button")).Click();
        }

        static public void LogOut()
        {
            //Drag text up 1000 pixels
            ScrollUntilElementIsFound(_driver, "£40 Friday Forty", 1000);

            _driver.FindElement(By.Id("com.todaytix.TodayTix:id/back_button")).Click();
            Thread.Sleep(2000);
            _driver.HideKeyboard();
            _driver.FindElement(By.XPath("//*[@text='Account']")).Click();

            //Click on Account Settings
            _driver.FindElement(By.XPath("//*[@content-desc='Account Settings icon']")).Click();

            ScrollUntilElementIsFound(_driver, "Full name", -1000);

            _driver.FindElement(By.XPath("//*[@text='Log out']")).Click();

            Thread.Sleep(2000);
        }
        static public void TearDown()
        {
            _driver.Dispose();
        }
    }
}