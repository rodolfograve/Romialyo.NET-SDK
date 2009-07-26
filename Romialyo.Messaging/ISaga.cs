using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    public interface ISaga : IMessage
    {
        Guid CorrelationId { get; set; }

        bool IsCompleted { get; }
    }
}
