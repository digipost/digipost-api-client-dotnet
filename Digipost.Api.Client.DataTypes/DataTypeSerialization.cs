using System;
using System.Xml;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.DataTypes
{
    internal class DataTypeSerialization
    {

        internal static XmlElement Serialize<T>(T data)
        {
            var document = new XmlDocument();
            var serialized = SerializeUtil.Serialize(data);
            document.LoadXml(serialized);
            return document.DocumentElement;
        }

    }
}
