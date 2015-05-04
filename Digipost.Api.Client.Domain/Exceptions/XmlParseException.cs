using System;

namespace Digipost.Api.Client.Domain.Exceptions
{
    public class XmlParseException : Exception
    {
        private static readonly string XmlException = "Could not parse parse XML.";

        public XmlParseException(string xmlRawData) : base (XmlException)
        {
            XmlRawData = xmlRawData;
        }

        public XmlParseException(string message, Exception inner,string xmlRawData)
            : base(message, inner)
        {
            XmlRawData = xmlRawData;
        }

        public string XmlRawData{get; private set; }
    }
}
