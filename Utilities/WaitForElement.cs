using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Utilities
{
    public static class WaitForElement
    {
        public static async Task<IWebElement> WaitForElementAsync(ChromeDriver driver, By by, TimeSpan timeout)
        {
            try
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, timeout);
                return await Task.Run(() => wait.Until(drv => drv.FindElement(by)));
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }
        public static async Task<IWebElement> Wait(IWebDriver driver, By locator, TimeSpan timeout)
        {
            DateTime endTime = DateTime.Now.Add(timeout);

            while (DateTime.Now < endTime)
            {
                try
                {
                    var element = await Task.Run(() => driver.FindElement(locator));
                    if (element.Displayed)
                        return element;
                }
                catch (NoSuchElementException)
                {
                }
                Thread.Sleep(500);
            }
            return null;
        }
    }
}
