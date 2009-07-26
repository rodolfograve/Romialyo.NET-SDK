using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace Romialyo.Xml.Serialization
{
    public class SerializableString : IXmlSerializable
    {

        public SerializableString(string text)
        {
            Text = text;
        }

        protected string Text { get; set; }

        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Text = reader.ReadString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteCData(Text);
        }

        public static implicit operator SerializableString(string text)
        {
            return new SerializableString(text);
        }

        public static implicit operator string(SerializableString cdata)
        {
            return cdata.Text;
        }
    }
}
