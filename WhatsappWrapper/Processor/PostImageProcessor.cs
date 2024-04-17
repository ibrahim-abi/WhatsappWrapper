namespace WhatsappWrapper.Processor
{
    public class PostImageProcessor
    {
        public async Task AsyncProcess(string filePath)
        {
            try
            {
                // Simulate processing the file
                Console.WriteLine($"Processing file at '{filePath}'...");
                // Your actual processing logic here
                Console.WriteLine("File processing completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
                throw; // Rethrow the exception for higher-level handling
            }
        }
    }
}
