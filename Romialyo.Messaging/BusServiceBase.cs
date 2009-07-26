using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Romialyo.Messaging
{

    /// <summary>
    /// Base class for services that use an IBus for comunication.
    /// </summary>
    public abstract class BusServiceBase : ServiceBase
    {
        public BusServiceBase(ILogger logger, string serviceName, IBusEndpoint receivingEndpoint)
            : base (logger, serviceName)
        {
            ReceivingEndpoint = receivingEndpoint;
        }

        protected readonly IBusEndpoint ReceivingEndpoint;

        protected IDependencyInjectionContainer MessageHandlersContainer;
        
        public override void Start()
        {
            Logger.Info("Starting Service '" + ServiceName + "' v" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            MessageHandlersContainer = CreateMessageHandlersContainer();
            ReceivingEndpoint.SetMessageHandlersContainer(MessageHandlersContainer);
            ReceivingEndpoint.Start();
            Logger.Info("Service Started");
        }

        public override void Stop()
        {
            Logger.Info("Stopping Service '" + ServiceName + "'");
            ReceivingEndpoint.Stop();
            if (MessageHandlersContainer != null)
            {
                MessageHandlersContainer.Dispose();
            }
            Logger.Info("Stopped Service '" + ServiceName + "'");
        }

        /// <summary>
        /// Must create the DependencyInjectionSession configured to provide all the message handlers
        /// of this service and all the artifacts required by them.
        /// </summary>
        /// <returns></returns>
        protected abstract IDependencyInjectionContainer CreateMessageHandlersContainer();
    }
}
