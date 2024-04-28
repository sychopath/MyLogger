namespace MyLogger
{
    internal class LogManager
    {
        private static readonly Lazy<LogManager> lazyInstance = new Lazy<LogManager>(() => new LogManager());
        private readonly Logger logger;

        private LogManager()
        {
            DebugLogger debugLogger = new DebugLogger(null);
            ErrorLogger errorLogger = new ErrorLogger(debugLogger);
            WarningLogger warningLogger = new WarningLogger(errorLogger);
            InfoLogger infoLogger = new InfoLogger(warningLogger);
            
            logger = infoLogger;
        }

        public static LogManager Instance => lazyInstance.Value;

        public Logger GetLogger()
        {
            return logger;
        }
    }
}
