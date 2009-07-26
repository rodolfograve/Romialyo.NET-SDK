using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging.SqlBroker
{
    public class SqlBrokerSendingTransport : ISendingTransport
    {
        public void Send(IMessage message)
        {
            throw new NotImplementedException();
        }

        public void Send(IMessage[] messages)
        {
            throw new NotImplementedException();
        }
    }
}
