using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Romialyo.Messaging.InProcess
{
    public class InProcessTransport : ISendingTransport, IReceivingTransport
    {
        public InProcessTransport()
        {
            ReceptionQueue = new Queue<IList<IMessage>>();
            WaitForMessageHandles = new List<AutoResetEvent>();
        }

        protected readonly Queue<IList<IMessage>> ReceptionQueue;
        protected readonly IList<AutoResetEvent> WaitForMessageHandles;
        protected AutoResetEvent NoPendingMessageHandle;

        public void EnsurePendingMessagesAreDelivered()
        {
            if (ReceptionQueue.Count > 0)
            {
                NoPendingMessageHandle = new AutoResetEvent(false);
                NoPendingMessageHandle.WaitOne();
            }
        }

        public void Send(IMessage message)
        {
            Send(new IMessage[1] { message });
        }

        public void Send(IMessage[] messages)
        {
            ReceptionQueue.Enqueue(messages);
            lock (WaitForMessageHandles)
            {
                if (WaitForMessageHandles.Count > 0)
                {
                    WaitForMessageHandles[0].Set();
                    WaitForMessageHandles.RemoveAt(0);
                }
            }
        }

        public IList<IMessage> WaitForMessageWithoutTimeout()
        {
            return WaitForMessage(Timeout.Infinite);
        }

        protected IList<IMessage> WaitForMessage(int timeoutMilliseconds)
        {
            IList<IMessage> result = null;
            if (ReceptionQueue.Count > 0)
            {
                result = ReceptionQueue.Dequeue();
            }
            else
            {
                AutoResetEvent handle = new AutoResetEvent(false);
                WaitForMessageHandles.Add(handle);

                bool signaled = handle.WaitOne(timeoutMilliseconds);

                if (signaled)
                {
                    result = ReceptionQueue.Dequeue();
                }
                else
                {
                    lock (WaitForMessageHandles)
                    {
                        WaitForMessageHandles.Remove(handle);
                    }
                }
            }
            if (NoPendingMessageHandle != null && ReceptionQueue.Count == 0)
            {
                NoPendingMessageHandle.Set();
            }
            return result;
        }

        public IList<IMessage> WaitForMessage(TimeSpan timeout)
        {
            return WaitForMessage((int)timeout.TotalMilliseconds);
        }
    }
}
