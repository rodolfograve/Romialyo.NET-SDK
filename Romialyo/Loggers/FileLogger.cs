using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Romialyo.Loggers
{

    /// <summary>
    /// Logger that output messages to a file. The base name of the file is prefixed
    /// with the date to improve readability (each file contains log for a single day).
    /// Error messages are ALSO logged in another file to ease the task of finding errors.
    /// </summary>
    public class FileLogger : LoggerBase
    {
        public FileLogger(string rootDirectoryPath, string logName)
        {
            RootDirectoryPath = rootDirectoryPath;
            LogName = logName;
        }

        protected readonly string RootDirectoryPath;
        protected readonly string LogName;

        protected readonly object WriteLocker = new object();

        protected override void Write(string message, LogLevel messageLevel)
        {
            // Get the time of invocation, before we try to get the lock
            DateTime now = DateTime.Now;
            lock (WriteLocker)
            {
                string formattedMessage = now.ToShortDateString() + "-" + now.ToShortTimeString() + "-" + messageLevel.ToString() + ": " + message + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(GetLogPath(now), formattedMessage);
                if (LogLevel == LogLevel.Error)
                {
                    File.AppendAllText(GetErrorsLogPath(now), formattedMessage);
                }
            }
        }

        protected string GetLogFileName(DateTime entryTime)
        {
            return entryTime.ToString("yyyy-MM-dd") + "-" + LogName;
        }

        protected string GetLogPath(DateTime entryTime)
        {
            return Path.Combine(RootDirectoryPath, GetLogFileName(entryTime) + ".log");
        }

        protected string GetErrorsLogPath(DateTime entryTime)
        {
            return Path.Combine(RootDirectoryPath, GetLogFileName(entryTime) + ".error");
        }

    }
}
