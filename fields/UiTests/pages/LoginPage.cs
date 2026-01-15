using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace UiTests.pages
{
    public class LoginPage : BasePage
    {
        private By EmailInput => By.CssSelector("input[data-testid='email-input']");
        private By PasswordInput => By.CssSelector("input[data-testid='password-input']");
        private By SubmitButton => By.CssSelector("button[type='submit']");
        private By Toasts => By.CssSelector("[data-sonner-toast], [data-sonner-toaster], .sonner-toast, .toast, .alert, .error");
        private By EmailError => By.XPath("//input[@data-testid='email-input']/following-sibling::p[contains(@class,'text-destructive')]");
        private By PasswordError => By.XPath("//input[@data-testid='password-input']/following-sibling::p[contains(@class,'text-destructive')]");

        public void Open()
        {
            Driver.Navigate().GoToUrl(UiTests.Infrastructure.AppConfig.LoginUrl);
        }

        public void FillEmail(string email)
        {
            var el = WaitAndFind(EmailInput);
            el.Clear();
            el.SendKeys(email);
        }

        public void FillPassword(string password)
        {
            var el = WaitAndFind(PasswordInput);
            el.Clear();
            el.SendKeys(password);
        }

        public void Submit()
        {
            var btn = WaitAndFind(SubmitButton);
            btn.Click();
        }

        public bool IsEmailRequiredVisible()
        {
            try
            {
                var emailError = Driver.FindElement(By.Id("_r_2_-form-item-message"));
                return emailError.Displayed && emailError.Text.Contains("Invalid input");
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsPasswordRequiredVisible()
        {
            try
            {
                var passwordError = Driver.FindElement(By.Id("_r_3_-form-item-message"));
                return passwordError.Displayed && passwordError.Text.Contains("Password is required");
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsErrorVisible()
        {
            try
            {
                var generalError = Driver.FindElement(By.CssSelector("[data-sonner-toast][data-type='error']"));
                return generalError.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool WaitForLoginRedirect(int timeoutSec = 8)
        {
            return WaitUntilElementInvisible(EmailInput, timeoutSec);
        }

        private IWebElement WaitAndFind(By by, int timeoutSec = 5)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSec));
            return wait.Until(d => d.FindElement(by));
        }
    }
}
