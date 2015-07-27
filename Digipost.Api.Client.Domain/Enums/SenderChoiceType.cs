using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum SenderChoiceType
    {
        [XmlEnum("sender-id")]
        SenderId,

        [XmlEnum("sender-organization")]
        SenderOrganization
    }
}
