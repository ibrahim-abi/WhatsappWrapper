using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WhatsappWrapper.Models.Internal;

namespace WhatsappWrapper.Processor
{
    public class HealthProcessor
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthProcessor(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        public async Task<GetHealthModel> DoAction(CancellationToken cancellationToken)
        {
            try
            {
                var hostName = Dns.GetHostName();
                var version = typeof(Startup).Assembly.GetName().Version.ToString();

                var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);

                var response = new GetHealthModel
                {
                    HostName = hostName,
                    Version = version,
                    Status = healthReport.Status.ToString(),
                    Description = "Good"
                };

                return response;
            }
            catch (Exception ex)
            {
                var response = new GetHealthModel
                {
                    HostName = Dns.GetHostName(),
                    Version = typeof(Startup).Assembly.GetName().Version.ToString(),
                    Status = "Error",
                    Description = $"Health check failed: {ex.Message}"
                };

                return response;
            }
        }
    }
}
