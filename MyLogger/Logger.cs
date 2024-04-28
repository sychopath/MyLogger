using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLogger
{
    enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error
    }

    abstract class Logger
    {
        ISinks sinks;
        protected Logger nextLogLevel;

        protected void setNextLevel(Logger logger)
        {
            this.nextLogLevel = logger;
        }

        public abstract void Log(LogLevel level, string msg);

        protected void LogInFile(string msg)
        {
            sinks = FileSink.GetInstance();
            Task.Run(() => sinks.ProcessMessage(msg));
        }

        protected void LogInDB(string msg)
        {
            sinks = DBSink.GetInstance();
            Task.Run(() => sinks.ProcessMessage(msg));
        }

        protected async void LogInConsole(string msg)
        {
            sinks = ConsoleSink.GetInstance();
            sinks.ProcessMessage(msg);
        }
    }

    class DebugLogger : Logger
    {
        public DebugLogger(Logger nextLevel)
        {
            setNextLevel(nextLevel);
        }

        public override void Log(LogLevel level, string msg)
        {
            if(level == LogLevel.Debug)
            {
                LogInFile(msg);
                LogInDB(msg);
                LogInConsole(msg);
            }
        }
    }

    class InfoLogger : Logger
    {
        public InfoLogger(Logger nextLevel)
        {
            setNextLevel(nextLevel);
        }

        public override void Log(LogLevel level, string msg)
        {
            if (level == LogLevel.Info)
            {
                LogInConsole(msg);
            }
            else
                nextLogLevel.Log(level, msg);
        }
    }

    class WarningLogger : Logger
    {
        public WarningLogger(Logger nextLevel)
        {
            setNextLevel(nextLevel);
        }

        public override void Log(LogLevel level, string msg)
        {
            if (level == LogLevel.Warn)
            {
                LogInFile(msg);
                LogInDB(msg);
            }
            else
                nextLogLevel.Log(level, msg);
        }
    }

    class ErrorLogger : Logger
    {
        public ErrorLogger(Logger nextLevel)
        {
            setNextLevel(nextLevel);
        }

        public override void Log(LogLevel level, string msg)
        {
            if (level == LogLevel.Debug)
            {
                LogInConsole(msg);
                LogInDB(msg);
            }
            else
                nextLogLevel.Log(level, msg);
        }
    }
}
