using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    /// <summary>
    /// Base class to for services which use a Logger.
    /// </summary>
    public abstract class ServiceBase : IService
    {
        public ServiceBase(ILogger logger, string serviceName)
        {
            Logger = logger;
            ServiceName = serviceName;
        }

        public readonly string ServiceName;

        protected readonly ILogger Logger;

        public abstract void Start();

        public abstract void Stop();
    }

}
