using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Runtime.Serialization.Formatters.Binary;

namespace Romialyo.Messaging.Msmq
{
    public class MsmqSendingTransport : MsmqTransportBase, ISendingTransport
    {

        public MsmqSendingTransport(ILogger logger, string outputQueuePath)
            : base(logger, outputQueuePath)
        {
        }

        public void Send(IMessage message)
        {
            Send(new IMessage[1] { message });
        }

        public void Send(IMessage[] messages)
        {
            IMessageSerializer serializer = new FormatterMessagesSerializer(new BinaryFormatter());
            Message toSend = new Message();
            serializer.Serialize(messages, toSend.BodyStream);
            Queue.Send(toSend);
        }

        protected override MessageQueue CreateQueue(string queuePath)
        {
            return new MessageQueue(queuePath, QueueAccessMode.Send);
        }
    }
}
