using GroupMessage.Action;
using OpenQA.Selenium.Chrome;
using WhatsappWrapper.Models.Internal;

namespace WhatsappWrapper.Processor
{
    public class PostFileProcessor
    {
        private ChromeDriver _chromeDriver;
        public PostFileProcessor(ChromeDriver chromeDriver)
        {
            _chromeDriver = chromeDriver;
        }
        public async Task<CustomMessage> AsyncProcess(FileGroupModel fileGroupModel)
        {
            try
            {
                await handleGroupMessage.sendGroupMessage(_chromeDriver, fileGroupModel.groupcode, fileGroupModel.message, fileGroupModel.path);
                return new CustomMessage { statuscode = StatusCodes.Status200OK.ToString(), message = "ok" };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
                throw;
            }
        }
    }
}
