using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    public interface ICorrelatedMessage : IMessage
    {
        Guid CorrelationId { get; }
    }
}
