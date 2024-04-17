using OpenQA.Selenium.Chrome;

public class WebDriverDisposeService : IHostedService
{
    private readonly ChromeDriver _driverManager;

    public WebDriverDisposeService(ChromeDriver driverManager)
    {
        _driverManager = driverManager;
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_driverManager != null)
        {
            try
            {

                _driverManager.Quit();
            }
            catch
            { }

        }
        await Task.CompletedTask; 
    }
}
