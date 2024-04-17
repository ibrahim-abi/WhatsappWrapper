using NLog;

namespace LoggerService
{
    public class LoggerManager: ILoggerManager
    {
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarning(string message)
        {
            logger.Warn(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogException(Exception ex, string message)
        {
            logger.Error(ex, message);
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
    }
}
