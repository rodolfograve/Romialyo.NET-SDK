using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romialyo.Messaging;
using Romialyo.Loggers;
using Romialyo.Messaging.Msmq;
using System.Messaging;
using Romialyo.Messaging.InProcess;
using System.Threading;

namespace Romialyo.SampleConsole
{
    public static class CheckBusEndpoint
    {

        public static void Run()
        {
            ILogger logger = new ConsoleLogger();

            string queuePath = ".\\Private$\\Testing";
            if (MessageQueue.Exists(queuePath))
            {
                MessageQueue.Delete(queuePath);
            }
            MessageQueue.Create(queuePath, false);
            IReceivingTransport receivingTransport = new MsmqReceivingTransport(logger, queuePath);
            IBusEndpoint endpoint = new DefaultBusEndpoint(logger, 1, 150, true, receivingTransport, new InProcessSagasRepository());

            endpoint.Start();

            ISendingTransport sendingTransport = new MsmqSendingTransport(logger, queuePath);
            sendingTransport.Send(new TestMessage());

            Thread.Sleep(2000);
            endpoint.Stop();
        }
    }

    [Serializable]
    public class TestMessage : IMessage
    {
    }
}
