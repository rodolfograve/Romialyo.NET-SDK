using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Romialyo.Messaging
{
    public interface IMessageSerializer
    {

        void Serialize(IList<IMessage> messages, Stream outputStream);

        IList<IMessage> Deserialize(Stream inputStream);

    }
}
