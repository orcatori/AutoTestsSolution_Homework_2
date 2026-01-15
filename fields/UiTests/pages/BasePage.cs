using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace UiTests.pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver => UiTests.Infrastructure.DriverSingleton.Instance;
        protected WebDriverWait Wait => new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

        protected IWebElement WaitAndFind(By by)
        {
            return Wait.Until(d => d.FindElement(by));
        }

        protected bool WaitUntilElementInvisible(By by, int timeoutSeconds = 5)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d =>
                {
                    try
                    {
                        var el = d.FindElement(by);
                        return !el.Displayed;
                    }
                    catch (NoSuchElementException) { return true; }
                });
            }
            catch { return false; }
        }
    }
}