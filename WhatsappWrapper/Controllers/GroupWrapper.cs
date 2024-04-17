using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using WhatsappWrapper.Models.Internal;
using WhatsappWrapper.Processor;

namespace WhatsappWrapper.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    public class GroupWrapper : ControllerBase
    {
        private readonly ChromeDriver _driver;

        public GroupWrapper(ChromeDriver driver)
        {
            _driver = driver;
        }
        [HttpPost("wpfile")]
        public async Task<IActionResult> PostFile([FromBody] FileGroupModel fileGroupModel)
        {
            try
            {
                PostFileProcessor postFileProcessor = new PostFileProcessor(_driver);
                var result = await postFileProcessor.AsyncProcess(fileGroupModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("wpmessage")]
        public IActionResult PostMessage([FromBody] string message)
        {
            try
            {
                return Ok($"Message received: {message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("wpimage")]
        public IActionResult PostImage(IFormFile image)
        {
            try
            {
                return Ok("Image received successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
