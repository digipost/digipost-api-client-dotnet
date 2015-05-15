using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "delivery-method", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("delivery-method", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum DeliveryMethod
    {
        /// <summary>
        ///     Delivered through fysical print and postal service.
        /// </summary>
        [XmlEnum("PRINT")] 
        Print,

        /// <summary>
        ///     Delivered digitally in Digipost
        /// </summary>
        [XmlEnum("DIGIPOST")] 
        Digipost
    }
}