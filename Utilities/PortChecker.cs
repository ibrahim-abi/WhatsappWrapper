using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public static class PortChecker
{
    public static bool IsPortInUse(int port)
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpConnections = ipGlobalProperties.GetActiveTcpListeners();
        return tcpConnections.Any(endpoint => endpoint.Port == port);
    }
    public static void ClosePort(int port)
    {
        ProcessStartInfo psi = new ProcessStartInfo("cmd", $"/c netstat -aon | findstr :{port} | findstr LISTENING");
        psi.RedirectStandardOutput = true;
        psi.UseShellExecute = false;
        Process process = Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd();
        string[] lines = output.Split('\n');
        if (lines.Length > 0)
        {
            string[] parts = lines[0].Trim().Split(' ');
            int pid = int.Parse(parts[parts.Length - 1]);
            Process.GetProcessById(pid).Kill();
        }
    }
    public static void CloseSession(int port)
    {
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        service.Port = port;

        try
        {
            using (var driver = new ChromeDriver(service))
            {
                driver.Quit();
                Console.WriteLine($"Closed the ChromeDriver session on port {port}.");
            }
        }
        catch (WebDriverException ex)
        {
            Console.WriteLine($"Failed to close the ChromeDriver session on port {port}: {ex.Message}");
        }
    }

    public static bool IsSessionCreated(int port)
    {
        // Attempt to connect to the specified port
        using (var tcpClient = new TcpClient())
        {
            try
            {
                tcpClient.Connect(IPAddress.Loopback, port);
                return true; // Connection successful, session is created
            }
            catch (SocketException)
            {
                return false; // Connection failed, session is not created
            }
        }
    }
}
