using System.Data;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Interactions;



namespace LotteryBot
{
    enum SignUpStage
    {
        StartTodayTix,
        ClickOnAccountTabOnSignUpScreen,
        TypeInEmailAddressOnSignUpScreen,
        ClickOnContinueOnSignUpScreen,
        TypeInFirstNameOnSignUpScreen,
        TypeInLastNameOnSignUpScreen,
        ClickSendLinkButton,
        GetSignUpCodeFromEmail,
        TypeInSignUpCode,
        ClickOnSearchTab,
        TypeInShowNameIntoSearchBar,
        ClickOnShowSearchResult,
        ScrollDownToLotteryButton,
        ClickOnEnterLottery,
        SelectNumberOfTickets,
        UntickFirstDay,
        UntickSecondDay,
        ClickOnNext,
        EnterEmailOnLotteryScreen,
        ClickOnCountryCode,
        SelectCountryCode,
        EnterPhoneNumber,
        ClickOnEnterLotteryInEnterLotteryScreen,
        ClickOnClosePopUpMenu,
        ScrollUpOnShowScreen,
        ClickOnBackButton,
        HideKeyboardOnSearchScreen,
        ClickOnAccountAfterSignUp,
        ClickOnAccountSettings,
        ScrollDownAccountSettingsPage,
        ClickOnLogOut,
        FinishedLotterySignUp,
    }

    class Program
    {
        private static AndroidDriver _driver;
        private static GmailMonitor _gmailMonitor;

        private static int _targetSingUpAmount = 10;
        private static int _currentSingUpAmount = 0;
        private static int _attemptEmailNum = 130;
        private static string _originalEmailAddress = "usvienaspirmas+test";
        private static string _modifiedEmailAddress = "";
        private static string _logInCode;

        private static SignUpStage _nextAction = SignUpStage.StartTodayTix;

        private static string _showName = "Harry Potter";
        private static string _searchScrollKeyword = "£40 Friday Forty";
        private static string _numberOfTicketsToWin = "2";

        static void Main(string[] args)
        {
            _gmailMonitor = new GmailMonitor();

            // Appium driver crashes sometimes so we need to put it in a loop to reconnect
            // when it crashes and continue from where where we left off
            while (_currentSingUpAmount < _targetSingUpAmount)
            {
                try
                {
                    _driver = SetUp();
                    SignUpForTicketsLoop();
                    TearDown();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Appium client crashed with error: {e.Message}");
                    Console.WriteLine($"Restarting Appium client");
                }
            }
        }

        static void SignUpForTicketsLoop()
        {
            for (; _currentSingUpAmount < _targetSingUpAmount; _currentSingUpAmount++)
            {
                while (_nextAction != SignUpStage.FinishedLotterySignUp)
                {
                    switch (_nextAction)
                    {
                        case SignUpStage.StartTodayTix:
                            _modifiedEmailAddress = $"{_originalEmailAddress}{_attemptEmailNum:00000}";
                            Console.WriteLine($"StartingSignUP, Attemp:{_currentSingUpAmount + 1}, email:{_attemptEmailNum}");
                            StartTodayTix();
                            _nextAction = SignUpStage.ClickOnAccountTabOnSignUpScreen;
                            break;
                        case SignUpStage.ClickOnAccountTabOnSignUpScreen:
                            ClickOnAccountTab();
                            _nextAction = SignUpStage.TypeInEmailAddressOnSignUpScreen;
                            break;
                        case SignUpStage.TypeInEmailAddressOnSignUpScreen:
                            TypeInEmailAddressOnSignUpScreen();
                            _nextAction = SignUpStage.ClickOnContinueOnSignUpScreen;
                            break;
                        case SignUpStage.ClickOnContinueOnSignUpScreen:
                            ClickContinueOnEmailAdddressScreen();
                            _nextAction = SignUpStage.TypeInFirstNameOnSignUpScreen;
                            break;
                        case SignUpStage.TypeInFirstNameOnSignUpScreen:
                            TypeInFirstName();
                            _nextAction = SignUpStage.TypeInLastNameOnSignUpScreen;
                            break;
                        case SignUpStage.TypeInLastNameOnSignUpScreen:
                            TypeInLastName();
                            _nextAction = SignUpStage.ClickSendLinkButton;
                            break;
                        case SignUpStage.ClickSendLinkButton:
                            ClickSendLinkButton();
                            _nextAction = SignUpStage.GetSignUpCodeFromEmail;
                            break;
                        case SignUpStage.GetSignUpCodeFromEmail:
                            GetSignUpCodeFromEmail();
                            _nextAction = SignUpStage.TypeInSignUpCode;
                            break;
                        case SignUpStage.TypeInSignUpCode:
                            TypeInSignUpCode();
                            _nextAction = SignUpStage.ClickOnSearchTab;
                            break;
                        case SignUpStage.ClickOnSearchTab:
                            ClickOnSearchTab();
                            _nextAction = SignUpStage.TypeInShowNameIntoSearchBar;
                            break;
                        case SignUpStage.TypeInShowNameIntoSearchBar:
                            TypeInShowNameIntoSearchBar();
                            _nextAction = SignUpStage.ClickOnShowSearchResult;
                            break;
                        case SignUpStage.ClickOnShowSearchResult:
                            ClickOnShowSearchResult();
                            _nextAction = SignUpStage.ScrollDownToLotteryButton;
                            break;
                        case SignUpStage.ScrollDownToLotteryButton:
                            ScrollDownToLotteryButton();
                            _nextAction = SignUpStage.ClickOnEnterLottery;
                            break;
                        case SignUpStage.ClickOnEnterLottery:
                            ClickOnEnterLottery();
                            _nextAction = SignUpStage.SelectNumberOfTickets;
                            break;
                        case SignUpStage.SelectNumberOfTickets:
                            SelectNumberOfTickers();
                            _nextAction = SignUpStage.UntickFirstDay;
                            break;
                        case SignUpStage.UntickFirstDay:
                            UntickFirstDay();
                            _nextAction = SignUpStage.UntickSecondDay;
                            break;
                        case SignUpStage.UntickSecondDay:
                            UntickSecondDay();
                            _nextAction = SignUpStage.ClickOnNext;
                            break;
                        case SignUpStage.ClickOnNext:
                            ClickOnNext();
                            _nextAction = SignUpStage.EnterEmailOnLotteryScreen;
                            break;
                        case SignUpStage.EnterEmailOnLotteryScreen:
                            EnterEmailOnEnterLotteryScreen();
                            _nextAction = SignUpStage.ClickOnCountryCode;
                            break;
                        case SignUpStage.ClickOnCountryCode:
                            ClickOnCountryCode();
                            _nextAction = SignUpStage.SelectCountryCode;
                            break;
                        case SignUpStage.SelectCountryCode:
                            SelectCountryCode();
                            _nextAction = SignUpStage.EnterPhoneNumber;
                            break;
                        case SignUpStage.EnterPhoneNumber:
                            EnterPhoneNumber();
                            _nextAction = SignUpStage.ClickOnEnterLotteryInEnterLotteryScreen;
                            break;
                        case SignUpStage.ClickOnEnterLotteryInEnterLotteryScreen:
                            ClickOnEnterLotteryInEnterLotteryScreen();
                            _nextAction = SignUpStage.ClickOnClosePopUpMenu;
                            break;
                        case SignUpStage.ClickOnClosePopUpMenu:
                            ClickOnClosePopUpMenu();
                            _nextAction = SignUpStage.ScrollUpOnShowScreen;
                            break;
                        case SignUpStage.ScrollUpOnShowScreen:
                            ScrollUpOnShowScreen();
                            _nextAction = SignUpStage.ClickOnBackButton;
                            break;
                        case SignUpStage.ClickOnBackButton:
                            ClickOnBackButton();
                            _nextAction = SignUpStage.HideKeyboardOnSearchScreen;
                            break;
                        case SignUpStage.HideKeyboardOnSearchScreen:
                            _driver.HideKeyboard();
                            _nextAction = SignUpStage.ClickOnAccountAfterSignUp;
                            break;
                        case SignUpStage.ClickOnAccountAfterSignUp:
                            ClickOnAccountTab();
                            _nextAction = SignUpStage.ClickOnAccountSettings;
                            break;
                        case SignUpStage.ClickOnAccountSettings:
                            ClickOnAccountSettings();
                            _nextAction = SignUpStage.ScrollDownAccountSettingsPage;
                            break;
                        case SignUpStage.ScrollDownAccountSettingsPage:
                            ScrollDownAccountSettingsPage();
                            _nextAction = SignUpStage.ClickOnLogOut;
                            break;
                        case SignUpStage.ClickOnLogOut:
                            ClickOnLogOut();
                            _nextAction = SignUpStage.FinishedLotterySignUp;
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine($"FinsishedSignUP, Attemp:{_currentSingUpAmount + 1}, email:{_attemptEmailNum}");
                _nextAction = SignUpStage.StartTodayTix;
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

        static void StartTodayTix()
        {
            _driver.StartActivity("com.todaytix.TodayTix", ".activity.MainActivity");
            Thread.Sleep(1000);
        }

        static void ClickOnAccountTab()
        {
            _driver.FindElement(By.XPath("//*[@text='Account']")).Click();
        }

        static void TypeInEmailAddressOnSignUpScreen()
        {
            var editText = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
            editText.Click();
            editText.Clear();
            editText.SendKeys(_modifiedEmailAddress + "@gmail.com");
        }

        static void ClickContinueOnEmailAdddressScreen()
        {
            var continueButton = _driver.FindElement(By.XPath("//*[@text='Continue']"));
            continueButton.Click();
        }

        static void TypeInFirstName()
        {
            var firstName = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
            firstName.Click();
            firstName.SendKeys("John");
        }

        static void TypeInLastName()
        {
            var lastName = _driver.FindElement(By.XPath("(//android.widget.EditText)[2]"));
            lastName.Click();
            lastName.SendKeys("Smith");
        }

        static void ClickSendLinkButton()
        {
            var sendLinkButton = _driver.FindElement(By.XPath("//*[@text='Send link']"));
            sendLinkButton.Click();
        }

        static void GetSignUpCodeFromEmail()
        {
            Console.WriteLine("Sleep for 10 secs");
            Thread.Sleep(10000);
            GetSignUpCode().Wait();
        }

        static void TypeInSignUpCode()
        {
            if (_driver.FindElement(By.XPath("(//android.widget.EditText)[1]")) != null)
            {
                var editText = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
                editText.Click();
                editText.Clear();
                editText.SendKeys(_logInCode);
            }
        }

        static void ClickOnSearchTab()
        {
            _driver.FindElement(By.XPath("//*[@text='Search']")).Click();
        }

        static void TypeInShowNameIntoSearchBar()
        {
            var searchBar = _driver.FindElement(By.XPath("(//android.widget.EditText)[1]"));
            searchBar.Clear();
            searchBar.SendKeys(_showName);
        }

        static void ClickOnShowSearchResult()
        {
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
        }

        static void ScrollDownToLotteryButton()
        {
            ScrollUntilElementIsFound(_searchScrollKeyword, -500);
        }

        static void ClickOnEnterLottery()
        {
            _driver.FindElement(By.XPath("//*[@text='Enter Lottery']")).Click();
        }

        static void SelectNumberOfTickers()
        {
            var numberOfTickets = _driver.FindElement(By.XPath($"(//*[@text='{_numberOfTicketsToWin}'])"));
            numberOfTickets.Click();
        }

        static void UntickFirstDay()
        {
            _driver.FindElements(By.Id("com.todaytix.TodayTix:id/date_label"))[0].Click();
        }

        static void UntickSecondDay()
        {
            _driver.FindElements(By.Id("com.todaytix.TodayTix:id/date_label"))[1].Click();
        }

        static void ClickOnNext()
        {
            _driver.FindElement(By.XPath("//*[@text='Next']")).Click();
        }

        static void EnterEmailOnEnterLotteryScreen()
        {
            var emailAddress = _driver.FindElement(By.XPath("(//android.widget.EditText)[2]"));
            emailAddress.Click();
            emailAddress.SendKeys(_modifiedEmailAddress + "@gmail.com");
        }

        static void ClickOnCountryCode()
        {
            _driver.FindElement(By.XPath("(//android.widget.Spinner)[1]")).Click();
        }

        static void SelectCountryCode()
        {
            var phoneCode = _driver.FindElement(By.XPath("(//*[@text='GB +44'])"));
            phoneCode.Click();
        }

        static void EnterPhoneNumber()
        {
            var phoneNumber = _driver.FindElement(By.XPath("(//android.widget.EditText)[3]"));
            phoneNumber.Click();
            long basePhoneNum = 7891234567;
            phoneNumber.Clear();
            phoneNumber.SendKeys("0" + (basePhoneNum + _attemptEmailNum).ToString());
        }

        static void ClickOnEnterLotteryInEnterLotteryScreen()
        {
            _driver.FindElement(By.XPath("(//*[@text='Enter'])")).Click();
        }

        static void ClickOnClosePopUpMenu()
        {
            _driver.FindElement(By.Id("com.todaytix.TodayTix:id/close_button")).Click();
        }

        static void ScrollUpOnShowScreen()
        {
            ScrollUntilElementIsFound(_searchScrollKeyword, 1000);
        }

        static void ClickOnBackButton()
        {
            _driver.FindElement(By.Id("com.todaytix.TodayTix:id/back_button")).Click();
            Thread.Sleep(2000);
        }

        static void ClickOnAccountSettings()
        {
            _driver.FindElement(By.XPath("//*[@content-desc='Account Settings icon']")).Click();
        }

        static void ScrollDownAccountSettingsPage()
        {
            ScrollUntilElementIsFound("Full name", -1000);
        }

        static void ClickOnLogOut()
        {
            _driver.FindElement(By.XPath("//*[@text='Log out']")).Click();
            Thread.Sleep(2000);
        }

        static public void ScrollUntilElementIsFound(string elementText, int scrollDistance)
        {
            bool elementFound = false;
            int attempt = 0;

            int maxAttempts = 5;

            while (!elementFound && attempt < maxAttempts)
            {

                try
                {
                    var element = _driver.FindElement(By.XPath($"//*[@text='{elementText}']"));
                    Console.WriteLine($"Element found: {elementText}");
                    elementFound = true;
                    Actions action = new Actions(_driver);
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
            _logInCode = await _gmailMonitor.GetCodeFromEmail(_modifiedEmailAddress);
        }

        static public void TearDown()
        {
            _driver.Dispose();
        }
    }
}