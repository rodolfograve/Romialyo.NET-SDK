using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romialyo.Loggers;

namespace Romialyo.SampleConsole
{
    class TestFileLogger
    {

        public static void Run()
        {
            FileLogger logger = new FileLogger(".\\", "TestFileLogger");
            logger.Debug("Debug message");
            logger.Info("Info message");
            logger.Error("Error message without exception");
            logger.Error("Error message with exception", new Exception("Test exception"));
        }

    }
}
