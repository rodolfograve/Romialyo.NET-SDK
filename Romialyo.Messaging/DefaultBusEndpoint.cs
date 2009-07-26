using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    public class DefaultBusEndpoint : IBusEndpoint
    {
        public DefaultBusEndpoint(ILogger logger, int numberOfWorkingThreads, int waitingForMessageTimeoutMilliseconds, bool logNoMessageReceived, IReceivingTransport receivingTransport, ISagasRepository sagasRepository)
        {
            Logger = logger;
            WaitingForMessageTimeoutMilliseconds = waitingForMessageTimeoutMilliseconds;
            LogNoMessageReceived = logNoMessageReceived;
            ReceivingTransport = receivingTransport;
            SagasRepository = sagasRepository;

            Subscriptions = new Dictionary<Type, Predicate<IMessage>>();
            NumberOfReceivingThreads = numberOfWorkingThreads;
            ReceivingThreads = new List<WorkerThread>();
        }

        /// <summary>
        /// This must match the name of the method defined in the IMessageHandler interface.
        /// </summary>
        protected const string HandleMethodName = "Handle";

        protected readonly ILogger Logger;
        protected readonly IReceivingTransport ReceivingTransport;
        protected IDependencyInjectionContainer MessageHandlersContainer { get; set; }
        protected readonly ISagasRepository SagasRepository;
        protected readonly IList<WorkerThread> ReceivingThreads;

        protected HashSet<Type> MessagesWithHandlersCache { get; set; }
        protected readonly IDictionary<Type, Predicate<IMessage>> Subscriptions;

        public string InstanceId { get; protected set; }

        public readonly int NumberOfReceivingThreads;
        public readonly int WaitingForMessageTimeoutMilliseconds;
        public readonly bool LogNoMessageReceived;

        protected IEnumerable<Type> GetMessageTypesWithHandlersIn(IEnumerable<Type> registeredTypes)
        {
            Type genericHandlerType = typeof(IMessageHandler<>);
            string genericHandlerTypeName = genericHandlerType.Name;//"IMessageHandler`1"
            foreach (var handlerType in registeredTypes)
            {
                Type[] handlerInterfaceTypes = handlerType.GetInterfaces();
                foreach (Type interfaceType in handlerInterfaceTypes)
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericHandlerType)
                    {
                        Type messageType = interfaceType.GetGenericArguments()[0];
                        yield return messageType;
                    }
                }
            }
        }

        /// <summary>
        /// Use the types registered in the MessageHandlersContainer and subscribe
        /// to all messages with a known handler.
        /// </summary>
        public void SetMessageHandlersContainer(IDependencyInjectionContainer messageHandlersContainer)
        {
            MessageHandlersContainer = messageHandlersContainer;
            MessagesWithHandlersCache = new HashSet<Type>(GetMessageTypesWithHandlersIn(MessageHandlersContainer.RegisteredTypes));
            foreach (Type messageType in MessagesWithHandlersCache)
            {
                Subscribe(messageType);
            }
        }

        public void Subscribe(Type messageType)
        {
            Subscriptions.Add(messageType, null);
        }

        public void Unsubscribe<TMessage>() where TMessage : IMessage
        {
            Subscriptions.Remove(typeof(TMessage));
        }

        protected Predicate<IMessage> GetHandlerForMessage(IMessage message)
        {
            Predicate<IMessage> result = null;
            Type messageType = message.GetType();
            if (!Subscriptions.TryGetValue(message.GetType(), out result))
            {
                result = null;
            }
            return result;
        }

        protected readonly object handlingLocker = new object();
        public void HandleMessage(IList<IMessage> messages)
        {
            lock (handlingLocker)
            {
                foreach (var message in messages)
                {
                    Logger.Info("Handling " + message.PrettyFormat());
                    try
                    {
                        Type messageType = message.GetType();
                        if (Subscriptions.ContainsKey(messageType))
                        {
                            Type handlerType = typeof(IMessageHandler<>).MakeGenericType(message.GetType());

                            object handlerInstance = GetOrCreateHandlerFor(message, handlerType);
                            CallHandleMethodOnHandler(handlerType, handlerInstance, message);

                            if (handlerInstance is ISaga)
                            {
                                SagasRepository.Save((ISaga)handlerInstance);
                            }
                        }
                        else
                        {
                            Logger.Debug("No handler for message '" + message.ToString() + "'. Maybe this message had to be handled by a Saga that hasn't been created?");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Exception reached the bus while processing message '" + message.ToString() + "'. Exception: '" + ex.ToString() + "'. Sending message to Bus Poison.");
                    }
                    finally
                    {
                        Logger.Info("Handled " + message.PrettyFormat());
                    }
                }
            }
        }

        protected object GetOrCreateHandlerFor(IMessage message, Type handlerType)
        {
            object result = null;
            ICorrelatedMessage correlatedMessage = message as ICorrelatedMessage;
            if (correlatedMessage == null)
            {
                result = MessageHandlersContainer.Get(handlerType);
            }
            else
            {
                result = SagasRepository.WithId(correlatedMessage.CorrelationId);
                if (result == null)
                {
                    result = MessageHandlersContainer.Get(handlerType);
                    ISaga saga = result as ISaga;
                    // It is a correlated message, but maybe the handlerInstance is not a Saga
                    if (saga != null)
                    {
                        Logger.Debug("Created a new Saga for message {" + correlatedMessage.ToString() + "}");
                        // We are creating a Saga so we assign it the message's CorrelationId
                        saga.CorrelationId = correlatedMessage.CorrelationId;
                        SagasRepository.Save(saga);
                    }
                }
                else
                {
                    Logger.Debug("Loaded Saga {" + result.ToString() + "} for message {" + correlatedMessage.ToString() + "}");
                }
            }
            return result;
        }

        protected void CallHandleMethodOnHandler(Type handlerType, object handlerInstance, IMessage message)
        {
            handlerType.GetMethod(HandleMethodName).Invoke(handlerInstance, new object[1] { message });
        }

        protected void ReceiveOneMessageFromTransport()
        {
            TimeSpan timeout = TimeSpan.FromMilliseconds(WaitingForMessageTimeoutMilliseconds);
            IList<IMessage> message = ReceivingTransport.WaitForMessage(timeout);
            if (message == null)
            {
                if (LogNoMessageReceived)
                {
                    Logger.Info("No message received in " + timeout.TotalMilliseconds + " milliseconds");
                }
            }
            else
            {
                HandleMessage(message);
            }
        }

        /// <summary>
        /// Blocks until all the ReceivingThreads are started.
        /// </summary>
        public void Start()
        {
            Logger.Info("Starting Bus Endpoint " + InstanceId + ".");
            for (int i = 0; i < NumberOfReceivingThreads; i++)
            {
                WorkerThread t = new WorkerThread("Receiving thread " + i.ToString("00"),  ReceiveOneMessageFromTransport);
                t.Stopped += (x, e) =>
                {
                    if (e.Error != null)
                    {
                        WorkerThread worker = (WorkerThread)x;
                        Logger.Error(worker.Id + " threw an exception.", e.Error);
                    }
                };
                ReceivingThreads.Add(t);
                t.Start();
            }
        }

        /// <summary>
        /// Blocks until all Receiving threads are stopped, then stops the bus.
        /// </summary>
        public void Stop()
        {
            Logger.Info("Stopping the bus endpoint. Some working threads may delay up to " + WaitingForMessageTimeoutMilliseconds.ToString() + " milliseconds.");
            foreach (var receivingThread in ReceivingThreads)
            {
                receivingThread.Stop();
            }
            Logger.Info("Bus endpoint stopped.");
        }

    }
}
