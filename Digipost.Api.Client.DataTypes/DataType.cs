using System.Xml;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.DataTypes
{
    public interface IDataType
    {
        XmlElement Serialize();

    }
}
