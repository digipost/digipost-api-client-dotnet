using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "nondeliverable-handling", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("nondeliverable-handling", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum NondeliverableHandling
    {
        /// <summary>
        ///     If mail is undeliverable the mail will be returned to the return address.
        /// </summary>
        [XmlEnum("RETURN_TO_SENDER")] ReturnToSender
    }
}