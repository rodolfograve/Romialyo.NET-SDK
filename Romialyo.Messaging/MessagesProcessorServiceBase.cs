using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    public abstract class MessagesProcessorServiceBase : ServiceBase
    {

        public MessagesProcessorServiceBase(ILogger logger, string serviceName, IReceivingTransport receivingTransport, int numberOfWorkingThreads, int waitForMessageTimeoutMilliseconds)
            : base (logger, serviceName)
        {
            ReceivingTransport = receivingTransport;
            NumberOfWorkingThreads = numberOfWorkingThreads;
            WaitForMessageTimeoutMilliseconds = waitForMessageTimeoutMilliseconds;
            ReceivingThreads = new List<WorkerThread>();
        }

        protected readonly IReceivingTransport ReceivingTransport;
        public readonly int NumberOfWorkingThreads;
        public readonly int WaitForMessageTimeoutMilliseconds;

        protected readonly IList<WorkerThread> ReceivingThreads;

        /// <summary>
        /// Blocks until all the ReceivingThreads are started.
        /// </summary>
        public override void Start()
        {
            Logger.Info("Starting service '" + ServiceName + "'.");
            Logger.Info("Configuration: " + this.PrettyFormat());
            for (int i = 0; i < NumberOfWorkingThreads; i++)
            {
                WorkerThread t = new WorkerThread("Receiving thread " + i.ToString("00"), ReceiveOneMessageFromTransport);
                t.Stopped += (x, e) =>
                {
                    if (e.Error != null)
                    {
                        WorkerThread worker = (WorkerThread)x;
                        Logger.Error(worker.Id + " threw an exception", e.Error);
                    }
                };
                ReceivingThreads.Add(t);
                t.Start();
            }
        }

        /// <summary>
        /// Blocks until all Receiving threads are stopped, then stops the bus.
        /// </summary>
        public override void Stop()
        {
            Logger.Info("Stopping service '" + ServiceName + "'. Some working threads may delay up to '" + WaitForMessageTimeoutMilliseconds.ToString() + "' milliseconds.");
            foreach (var receivingThread in ReceivingThreads)
            {
                receivingThread.Stop();
            }
            Logger.Info("Bus endpoint stopped.");
        }

        protected void ReceiveOneMessageFromTransport()
        {
            TimeSpan timeout = TimeSpan.FromMilliseconds(WaitForMessageTimeoutMilliseconds);
            IList<IMessage> message = ReceivingTransport.WaitForMessage(timeout);
            if (message == null)
            {
                Logger.Info("No message received in " + timeout.TotalMilliseconds + " milliseconds");
            }
            else
            {
                HandleMessage(message);
            }
        }

        public abstract void HandleMessage(IList<IMessage> message);

    }
}
