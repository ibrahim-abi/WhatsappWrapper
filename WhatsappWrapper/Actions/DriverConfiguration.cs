using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Management;
namespace WhatsappWrapper
{
    public sealed class DriverConfiguration
    {
        private static ChromeDriver _driverInstance;
        private static readonly object _lock = new object();

        private DriverConfiguration() { }

        public static ChromeDriver GetDriver()
        {
            lock (_lock)
            {
                if (_driverInstance == null)
                {
                    var chromeOptions = Configuration.GetChromeOptions();

                    try
                    {
                        try
                        {
                            _driverInstance = new ChromeDriver(chromeOptions);

                        }
                        catch
                        {
                        }

                    }
                    catch (InvalidOperationException)
                    {

                        throw;
                    }
                }
            _driverInstance.Navigate().GoToUrl(Configuration.GetWhatsappUrl());
            }
            return _driverInstance;
        }
        public static void Dispose()
        {
            lock (_lock)
            {
                if (_driverInstance != null)
                {
                    //_driverInstance.Dispose();
                    _driverInstance.Quit();
                    _driverInstance = null;
                }
            }
        }

    }
}
