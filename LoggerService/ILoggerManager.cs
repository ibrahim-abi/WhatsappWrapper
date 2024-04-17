namespace LoggerService
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception ex, string message);
        void LogDebug(string message);
    }
}
