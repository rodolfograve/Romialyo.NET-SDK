using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Loggers
{

    /// <summary>
    /// Logger that outputs messages to the Console.
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {

        protected override void Write(string message, LogLevel messageLevel)
        {
            string prefix = messageLevel.ToString();
            Console.WriteLine(DateTime.Now.ToString() + "(" + prefix + "): " + message);
            Console.WriteLine();
        }

    }
}
