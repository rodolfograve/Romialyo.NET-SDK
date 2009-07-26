using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    /// <summary>
    /// Transport used to send messages.
    /// </summary>
    public interface ISendingTransport
    {

        void Send(IMessage message);

        void Send(IMessage[] messages);

    }
}
