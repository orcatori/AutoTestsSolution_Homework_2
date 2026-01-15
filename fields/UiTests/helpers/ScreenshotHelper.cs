using OpenQA.Selenium;
using System;
using System.IO;
using UiTests.Infrastructure;

namespace UiTests.Helpers
{
    public static class ScreenshotHelper
    {
        public static string TakeScreenshot(string testName)
        {
            try
            {
                var driver = DriverSingleton.Instance;
                var ss = ((ITakesScreenshot)driver).GetScreenshot();
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "ui-screenshots");
                Directory.CreateDirectory(dir);

                var file = Path.Combine(dir, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                ss.SaveAsFile(file);

                return file;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Screenshot failed: " + ex.Message);
                return null;
            }
        }
    }
}