using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "print-colors", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("print-colors", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum PrintColors
    {
        /// <summary>
        ///     Prints the document in black/white.
        /// </summary>
        [XmlEnum("MONOCHROME")]
        Monochrome,

        /// <summary>
        ///     Prints the document in colors. Note: additional charges may apply.
        /// </summary>
        [XmlEnum("COLORS")]
        Colors
    }
}