using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Common.Utilities
{
    internal class SerializeUtil
    {
        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            var serializer = new XmlSerializer(value.GetType());

            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                ConformanceLevel = ConformanceLevel.Document,
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (var textWriter = new Utf8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlReaderSettings();

            using (var textReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T) serializer.Deserialize(xmlReader);
                }
            }
        }

        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
