using System.Xml;

namespace Digipost.Api.Client.DataTypes
{
    public interface IDataType
    {
        XmlElement Serialize();
    }
}
