using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging.SqlBroker
{
    public class SqlBrokerReceivingTransport : IReceivingTransport
    {
        public IList<IMessage> WaitForMessageWithoutTimeout()
        {
            throw new NotImplementedException();
        }

        public IList<IMessage> WaitForMessage(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}
