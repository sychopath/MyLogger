namespace MyLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LogManager logManager = LogManager.Instance;
            Logger logger = logManager.GetLogger();

            logger.Log(LogLevel.Info, "This is a info test message");
            logger.Log(LogLevel.Warn, "This is a warning test message");
            logger.Log(LogLevel.Error, "This is a error test message");
            logger.Log(LogLevel.Debug, "This is a debug test message");

            Console.WriteLine("This is done");
            Console.Read();

        }
    }
}