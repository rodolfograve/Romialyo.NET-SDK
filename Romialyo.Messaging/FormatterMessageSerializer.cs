using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Romialyo.Messaging
{
    public class FormatterMessagesSerializer : IMessageSerializer
    {

        public FormatterMessagesSerializer(IFormatter formatter)
        {
            Formatter = formatter;
        }

        protected readonly IFormatter Formatter;

        public void Serialize(IList<IMessage> messages, Stream outputStream)
        {
            Formatter.Serialize(outputStream, messages);
        }

        public IList<IMessage> Deserialize(Stream inputStream)
        {
            IList<IMessage> result = Formatter.Deserialize(inputStream) as IList<IMessage>;
            return result;
        }
    }
}
