using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Messaging;
using System.Runtime.Serialization.Formatters.Binary;

namespace Romialyo.Messaging.Msmq
{
    public class MsmqReceivingTransport : MsmqTransportBase, IReceivingTransport
    {
        public MsmqReceivingTransport(ILogger logger, string inputQueuePath)
            : base(logger, inputQueuePath)
        {
        }

        protected object receivingLocker = new object();

        public IList<IMessage> WaitForMessageWithoutTimeout()
        {
            return WaitForMessage(Timeout.Infinite);
        }

        public IList<IMessage> WaitForMessage(TimeSpan timeout)
        {
            return WaitForMessage((int)timeout.TotalMilliseconds);
        }

        protected IList<IMessage> WaitForMessage(int timeoutMilliseconds)
        {
            lock (receivingLocker)
            {
                return ReceiveFromQueue(timeoutMilliseconds);
            }
        }

        protected IList<IMessage> ReceiveFromQueue(int timeoutMilliseconds)
        {
            IList<IMessage> result = null;
            Message msmqMessage = null;

            if (Timeout.Infinite == timeoutMilliseconds)
            {
                msmqMessage = Queue.Receive();
            }
            else
            {
                try
                {
                    msmqMessage = Queue.Receive(TimeSpan.FromMilliseconds(timeoutMilliseconds));
                }
                catch (MessageQueueException ex)
                {
                    switch (ex.MessageQueueErrorCode)
                    {
                        case MessageQueueErrorCode.QueueDeleted:
                            {
                                Queue.Close(); break;
                            }
                        case MessageQueueErrorCode.IOTimeout:
                            {
                                //Logger.Info("StarvationTimeSpan passed.");
                                break;
                            }
                        default:
                            {
                                throw;
                            }
                    }
                }
            }

            if (msmqMessage != null)
            {
                IMessageSerializer serializer = new FormatterMessagesSerializer(new BinaryFormatter());
                IList<IMessage> messages = serializer.Deserialize(msmqMessage.BodyStream);
                result = messages;
            }
            return result;
        }

        protected override MessageQueue CreateQueue(string queuePath)
        {
            return new MessageQueue(queuePath, QueueAccessMode.Receive);
        }
    }
}
