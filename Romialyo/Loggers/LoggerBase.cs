using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Loggers
{
    public abstract class LoggerBase : ILogger
    {

        public LoggerBase()
        {
            LogLevel = LogLevel.Debug;
        }

        public LoggerBase(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

        public LogLevel LogLevel { get; protected set; }

        public void ChangeLogLevel(LogLevel newLevel)
        {
            LogLevel = newLevel;
        }

        protected abstract void Write(string message, LogLevel messageLevel);

        public void Info(string informationMessage)
        {
            if (LogLevel == LogLevel.Debug || LogLevel == LogLevel.Info)
            {
                Write(informationMessage, LogLevel.Info);
            }
        }

        public void Debug(string debugMessage)
        {
            if (LogLevel == LogLevel.Debug)
            {
                Write(debugMessage, LogLevel.Debug);
            }
        }

        public void Error(string errorMessage)
        {
            if (LogLevel != LogLevel.Off)
            {
                Write(errorMessage, LogLevel.Error);
            }
        }

        public void Error(string errorMessage, Exception ex)
        {
            if (LogLevel != LogLevel.Off)
            {
                Write(errorMessage + " {" + ex.ToString() + "}", LogLevel.Error);
            }
        }

    }
}
