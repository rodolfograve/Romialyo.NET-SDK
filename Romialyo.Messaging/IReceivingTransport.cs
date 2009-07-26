using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    /// <summary>
    /// Transport used to receive messages.
    /// </summary>
    public interface IReceivingTransport
    {

        IList<IMessage> WaitForMessageWithoutTimeout();

        IList<IMessage> WaitForMessage(TimeSpan timeout);

    }
}
