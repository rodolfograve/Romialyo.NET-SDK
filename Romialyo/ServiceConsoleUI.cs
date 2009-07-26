using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romialyo.Loggers;
using System.IO;

namespace Romialyo
{
    public abstract class ServiceConsoleUI : ConsoleApplication
    {
        public ServiceConsoleUI(ConsoleColor color, string serviceName)
            : base(color, serviceName + " Service")
        {
            ServiceName = serviceName;
        }

        protected readonly string ServiceName;

        protected abstract IService BuildService();

        protected override void RunInternal()
        {
            IService service = BuildService();
            service.Start();
            WriteMessage("Press ENTER to stop the service and exit.");
            ReadLine();
            service.Stop();
        }

        protected CompoundLogger LoggerInternal;
        protected override ILogger Logger
        {
            get
            {
                if (LoggerInternal == null)
                {
                    string logsPath = Path.GetFullPath("..\\");
                    Console.WriteLine("Creating log at " + logsPath);
                    LoggerInternal = new CompoundLogger();
                    LoggerInternal.AddLogger(new ConsoleLogger());
                    LoggerInternal.AddLogger(new FileLogger(logsPath, ServiceName));
                    Console.WriteLine("Log created");
                }
                return LoggerInternal;
            }
        }

    }
}
