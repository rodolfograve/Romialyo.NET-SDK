using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Loggers
{
    public class CompoundLogger : ILogger
    {

        public CompoundLogger()
        {
            Loggers = new List<ILogger>();
        }

        public CompoundLogger(params ILogger[] loggers)
        {
            Loggers = new List<ILogger>(loggers);
        }

        protected readonly IList<ILogger> Loggers;
        protected readonly object LoggersLocker = new object();

        public void AddLogger(ILogger logger)
        {
            lock (LoggersLocker)
            {
                Loggers.Add(logger);
            }
        }

        public LogLevel LogLevel { get; protected set; }

        public void ChangeLogLevel(LogLevel newLevel)
        {
            LogLevel = newLevel;
        }

        public void Info(string informationMessage)
        {
            LoggersLocker.Lock(() => Loggers.ForEach(l => l.Info(informationMessage)));
        }

        public void Debug(string debugMessage)
        {
            LoggersLocker.Lock(() => Loggers.ForEach(l => l.Info(debugMessage)));
        }

        public void Error(string errorMessage)
        {
            LoggersLocker.Lock(() => Loggers.ForEach(l => l.Error(errorMessage)));
        }

        public void Error(string errorMessage, Exception ex)
        {
            LoggersLocker.Lock(() => Loggers.ForEach(l => l.Error(errorMessage, ex)));
        }
    }
}
