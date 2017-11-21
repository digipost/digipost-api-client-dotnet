using System.Xml;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.DataTypes
{
    public abstract class DataType
    {
        internal XmlElement Serialize()
        {
            var document = new XmlDocument();
            var serialized = SerializeUtil.Serialize(AsDataTransferObject());
            document.LoadXml(serialized);
            return document.DocumentElement;
        }

        protected abstract object AsDataTransferObject();


    }
}
