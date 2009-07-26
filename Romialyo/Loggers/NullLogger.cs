using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Loggers
{
    public class NullLogger : LoggerBase
    {
        protected override void Write(string message, LogLevel messageLevel)
        {
        }
    }
}
