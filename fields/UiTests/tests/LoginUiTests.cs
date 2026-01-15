using UiTests.Helpers;
using UiTests.Infrastructure;
using UiTests.pages;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace UiTests.tests
{
    [TestFixture]
    public class LoginUiTests
    {
        private LoginPage _login;
        private IWebDriver _driver;
        private WebDriverWait _wait;
        
        private const string ValidEmail = "testuser@example.com";
        private const string ValidPassword = "testpassword";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _driver = DriverSingleton.Instance;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [SetUp]
        public void Setup()
        {
            _login = new LoginPage();
            _login.Open();
        }

        [TearDown]
        public void TearDown()
        {
            var s = TestContext.CurrentContext.Result.Outcome.Status;
            if (s == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var file = ScreenshotHelper.TakeScreenshot(TestContext.CurrentContext.Test.Name);
                SimpleLogger.Error($"Test failed. Screenshot saved to: {file}");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DriverSingleton.Quit();
        }

        [Test]
        public void Login_Valid_ShowsDashboard()
        {
            _login.FillEmail(ValidEmail);
            _login.FillPassword(ValidPassword);
            _login.Submit();
            
            var redirected = _login.WaitForLoginRedirect();
            Assert.IsTrue(redirected, "Expected to be redirected from login page after valid credentials.");
        }

        [Test]
        public void Login_EmptyEmail_ShowsError()
        {
            _login.FillEmail("");
            _login.FillPassword("anypassword");
            _login.Submit();
            
            WaitForElementCondition(driver => _login.IsEmailRequiredVisible(),
                "Email required error should be visible");
            
            Assert.IsTrue(_login.IsEmailRequiredVisible(),
                "Expected 'required field' error for empty email.");
        }

        [Test]
        public void Login_EmptyPassword_ShowsError()
        {
            _login.FillEmail(ValidEmail);
            _login.FillPassword("");
            _login.Submit();
            
            WaitForElementCondition(driver => _login.IsPasswordRequiredVisible(),
                "Password required error should be visible");
            
            Assert.IsTrue(_login.IsPasswordRequiredVisible(),
                "Expected 'required field' error for empty password.");
        }

        [TestCase("wrong@example.com", "testpassword", "wrong email")]
        [TestCase("testuser@example.com", "wrongpass", "wrong password")]
        public void Login_InvalidCredentials_ShowsError(string email, string pass, string description)
        {
            _login.FillEmail(email);
            _login.FillPassword(pass);
            _login.Submit();
            
            WaitForElementCondition(driver => _login.IsErrorVisible(),
                $"General login error should be visible for {description}");
            
            Assert.IsTrue(_login.IsErrorVisible(), 
                $"Expected general login error for {description}.");
        }

        private void WaitForElementCondition(Func<IWebDriver, bool> condition, string timeoutMessage)
        {
            try
            {
                _wait.Until(condition);
            }
            catch (WebDriverTimeoutException)
            {
                throw new AssertionException($"Timeout waiting for: {timeoutMessage}");
            }
        }
    }
}