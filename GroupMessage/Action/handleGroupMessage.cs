using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Reflection.Metadata;
using Utilities;


namespace GroupMessage.Action
{
    public class handleGroupMessage
    {
        public static List<string> Error = new List<string>();
        public static async Task sendGroupMessage(ChromeDriver driver, string groupCode, string message, string path)
        {
            string url = $"{GroupUrl()}{groupCode}";
            try
            {
                IWebElement groupLinkElement = driver.FindElement(By.XPath($"//a[contains(@href,'{url}')]"));
                groupLinkElement.Click();
            }
            catch (NoSuchElementException)
            {
                if(Error.Count<0)
                Error.Add("E1 not found");


                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript(@"
            var newLink = document.createElement('a');
            newLink.setAttribute('id', 'dynamicLink');
            newLink.setAttribute('href', arguments[0]);
            newLink.textContent = 'Click Me';
            document.body.appendChild(newLink);                                                                     
        ", url);

                    IWebElement newLinkElement = driver.FindElement(By.XPath("//a[@id='dynamicLink']"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", newLinkElement);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", newLinkElement);
                    Thread.Sleep(500);
                    bool joinedSuccessfully = CheckIfInvalidGroup(driver);

                    if (joinedSuccessfully)
                    {
                        throw new Exception("Couldn't join this group. Please try again.");
                    }
                    try
                    {
                        IWebElement dynamicLinkElement = driver.FindElement(By.XPath("//a[@id='dynamicLink']"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].remove();", dynamicLinkElement);
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Dynamically added link was Unsuccessfully clicked and removed.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (ElementClickInterceptedException)
            {
                Console.WriteLine("ElementClickInterceptedException occurred.");
            }

            try
            {
                IWebElement footer = await WaitForElement.WaitForElementAsync(driver, By.XPath("//*[@id='main']/footer/div[1]/div/span[2]/div/div[2]/div[1]/div[2]/div[1]/p"), TimeSpan.FromSeconds(50));
                Thread.Sleep(500);
                if (footer != null)
                {
                    Console.WriteLine("Footer element found.");
                    footer.Click();

                    // Click the attachment button
                    IWebElement attachButton = driver.FindElement(By.XPath("//*[@id='main']/footer/div[1]/div/span[2]/div/div[1]/div/div/div/div/span"));
                    if (attachButton != null)
                    {
                        attachButton.Click();

                        // Wait for the file input element
                        IWebElement fileInput = await WaitForElement.WaitForElementAsync(driver, By.XPath("//input[@type='file']"), TimeSpan.FromSeconds(30));
                        if (fileInput != null)
                        {
                            // Send file path to the file input element
                            fileInput.SendKeys(path);
                            Console.WriteLine("File path added.");

                            // Send message
                            IWebElement messageInput = await WaitForElement.WaitForElementAsync(driver, By.XPath("//*[@id='app']/div/div[2]/div[2]/div[2]/span/div/span/div/div/div[2]/div/div[1]/div[3]/div/div/div[1]/div[1]"), TimeSpan.FromSeconds(30));
                            if (messageInput != null)
                            {
                                messageInput.SendKeys(message + Keys.Enter);
                                Console.WriteLine("Message sent.");
                            }
                            else
                            {
                                throw new Exception("Message input field not found.");
                            }
                        }
                        else
                        {
                            throw new Exception("File input element not found.");
                        }
                    }
                    else
                    {
                        throw new Exception("Attachment button not found or empty.");
                    }
                }
                else
                {
                    Console.WriteLine("Footer element not found within the specified timeout.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

        }
        private static string GroupUrl()
        {
            return Configuration.GetWhatsappChatUrl();
        }
        public static bool CheckIfInvalidGroup(IWebDriver driver)
        {
            try
            {
                string script = @"
                var xpath = ""//*[@id='app']/div/span[2]/div/div/div/div/div/div/div[2]/div/button"";
                var cancelButton = document.evaluate(xpath, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
                if (cancelButton) {
                    cancelButton.click();
                    return true;
                } else {
                    return false; 
                }
            ";
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                bool result = (bool)js.ExecuteScript(script);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling join group result: {ex.Message}");
                return false;
            }
        }
    }
}