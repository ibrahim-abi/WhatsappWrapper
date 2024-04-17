using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;

public static class Configuration
{
    private static IConfiguration _configuration;

    static Configuration()
    {
        _configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();
    }

    public static ChromeOptions GetChromeOptions()
    {
        string exePath = _configuration["Appsettingkeys:ExePath"];
        string userDataDir = _configuration["Appsettingkeys:UserDataDir"];
        bool headless = Convert.ToBoolean(_configuration["Appsettingkeys:headless"]);

        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument($"--user-data-dir={userDataDir}");
        if (headless)
        {
            chromeOptions.AddArgument("--headless=new");
        }
        chromeOptions.AddArgument("--log-level=silent");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        //chromeOptions.AddArgument("log-level=3");
        chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument("--disable-extensions");
        chromeOptions.AddArgument("--disable-popup-blocking");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--window-size=1920,1080");
        chromeOptions.AddArgument("--disable-infobars");
        chromeOptions.AddArgument("--disable-notifications");
        chromeOptions.AddArgument("--mute-audio");
        chromeOptions.AddArgument("--blink-settings=imagesEnabled=false");

        return (chromeOptions);
    }

    public static int GetPortNo()
    {
        return Convert.ToInt32(_configuration["Appsettingkeys:PortNo"]);
    }
    public static string GetWhatsappUrl()
    {
        return _configuration["Appsettingkeys:whatsapp"]; ;
    }
    public static string GetWhatsappChatUrl()
    {
        return _configuration["Appsettingkeys:whatsappChat"]; ;
    }
}
