using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Romialyo.Loggers
{
    /// <summary>
    /// Logger that outputs messages to a log4net logger
    /// </summary>
    public class Log4NetLoggerAdapter : ILogger
    {

        public Log4NetLoggerAdapter(ILog adapted)
        {
            if (adapted == null)
            {
                throw new ArgumentNullException("adapted", "Can not adapt a null instance.");
            }
            Adapted = adapted;
        }

        protected readonly ILog Adapted;

        public LogLevel LogLevel { get; protected set; }

        public void ChangeLogLevel(LogLevel newLevel)
        {
            LogLevel = newLevel;
        }

        public void Info(string informationMessage)
        {
            Adapted.Info(informationMessage);
        }

        public void Debug(string debugMessage)
        {
            Adapted.Debug(debugMessage);
        }

        public void Error(string errorMessage)
        {
            Adapted.Error(errorMessage);
        }

        public void Error(string errorMessage, Exception ex)
        {
            Adapted.Error(errorMessage, ex);
        }

    }
}
