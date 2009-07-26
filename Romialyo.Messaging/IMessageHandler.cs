using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    public interface IMessageHandler<TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}
