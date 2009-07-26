using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    public interface ILogger
    {

        LogLevel LogLevel { get; }

        void ChangeLogLevel(LogLevel newLevel);

        void Debug(string debugMessage);

        void Info(string informationMessage);

        void Error(string errorMessage);

        void Error(string errorMessage, Exception ex);

    }
}
