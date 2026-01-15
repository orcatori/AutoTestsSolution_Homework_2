using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UiTests.Infrastructure
{
    public sealed class DriverSingleton
    {
        private static readonly Lazy<IWebDriver> lazyDriver = new Lazy<IWebDriver>(() =>
        {
            var options = new ChromeOptions();
            
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--window-size=1920,1080");
            
            return new ChromeDriver(options);
        });

        private DriverSingleton() { }

        public static IWebDriver Instance => lazyDriver.Value;

        public static void Quit()
        {
            try
            {
                if (lazyDriver.IsValueCreated)
                {
                    Instance.Quit();
                    Instance.Dispose();
                }
            }
            catch { /* ignore */ }
        }
    }
}