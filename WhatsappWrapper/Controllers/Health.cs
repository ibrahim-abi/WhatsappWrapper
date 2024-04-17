using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;
using WhatsappWrapper.Processor;

namespace WhatsappWrapper.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class Health : Controller
    {
        private readonly HealthCheckService _healthCheckService;

        public Health(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
        }
        [Route("Health")]
        [HttpGet]
        public  async Task<IActionResult> HealthSummary(CancellationToken cancellationToken)
        {
            try
            {

                HealthProcessor healths = new HealthProcessor(_healthCheckService);
                var response = await healths.DoAction(cancellationToken);
                return Ok(response);
            }
            catch (Exception)
            {

                throw;
        
            }
        }
    }
}
