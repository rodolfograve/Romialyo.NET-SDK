using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace Romialyo.Messaging.Msmq
{
    public abstract class MsmqTransportBase : IDisposable
    {
        public MsmqTransportBase(ILogger logger, string queuePath)
        {
            Logger = logger;
            Queue = CreateQueue(queuePath);
        }

        protected readonly ILogger Logger;
        protected MessageQueue Queue;

        public string QueuePath { get { return Queue.Path; } }

        protected abstract MessageQueue CreateQueue(string queuePath);

        public void Dispose()
        {
            if (Queue != null)
            {
                Queue.Close();
            }
        }

    }
}
