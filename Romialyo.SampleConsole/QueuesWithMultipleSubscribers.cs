using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace Romialyo.SampleConsole
{
    class QueuesWithMultipleSubscribers
    {

        public static void Run()
        {
            string queuePath = ".\\Private$\\Testing";
            if (MessageQueue.Exists(queuePath))
            {
                MessageQueue.Delete(queuePath);
            }
            Queue = MessageQueue.Create(queuePath, false);

            BackgroundWorker w1 = new BackgroundWorker();
            w1.DoWork += (x, e) =>
            {
                MessageQueue queue1 = new MessageQueue(queuePath, QueueAccessMode.Receive);
                Console.WriteLine("w1 receiving");
                while (!Stop)
                {
                    Message message = queue1.Receive();
                    object body = Formatter.Deserialize(message.BodyStream);
                    Console.WriteLine("w1-Message received: " + body.ToString());
                }
            };

            BackgroundWorker w2 = new BackgroundWorker();
            w2.DoWork += (x, e) =>
            {
                MessageQueue queue2 = new MessageQueue(queuePath, QueueAccessMode.Receive);
                Console.WriteLine("w2 receiving");
                while (!Stop)
                {
                    Message message = queue2.Receive();
                    object body = Formatter.Deserialize(message.BodyStream);
                    Console.WriteLine("w2-Message received: " + body.ToString());
                }
            };

            w1.RunWorkerAsync();
            w2.RunWorkerAsync();

            for (int i = 0; i < 10; i++)
            {
                Send("Message " + i.ToString());
            }

            Console.WriteLine("Press ENTER to Stop");
            Console.ReadLine();
            Stop = true;
        }
        private static void Send(string message)
        {
            Message toSend = new Message();
            Formatter.Serialize(toSend.BodyStream, message);
            Queue.Send(toSend);
            Console.WriteLine("Message sent: " + message);
        }

        private static bool Stop;

        private static MessageQueue Queue;

        private static IFormatter Formatter = new BinaryFormatter();
    }
}
