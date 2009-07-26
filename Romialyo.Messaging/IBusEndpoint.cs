using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    /// <summary>
    /// An endpoint for messages.
    /// </summary>
    public interface IBusEndpoint
    {

        void HandleMessage(IList<IMessage> messages);

        string InstanceId { get; }

        void Start();

        void Stop();

        /// <summary>
        /// Sets the DI to get the message handlers from, and subscribes all the
        /// IMessageHandler found in it to their messages.
        /// </summary>
        /// <param name="messageHandlersContainer"></param>
        void SetMessageHandlersContainer(IDependencyInjectionContainer messageHandlersContainer);
    }
}
