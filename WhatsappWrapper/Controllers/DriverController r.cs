using Microsoft.AspNetCore.Mvc;

namespace WhatsappWrapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : Controller
    {
        [HttpPost("stopdriver")]
        public  IActionResult StopDriver()
        {
            DriverConfiguration.Dispose();
            DriverConfiguration.GetDriver();
            return Ok("Driver stopped successfully");
        }
    }
}
