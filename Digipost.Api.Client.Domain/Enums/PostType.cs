using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "post-type", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("post-type", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum PostType
    {
        /// <summary>
        ///     Increased delivery priority. Note: additional charges may apply.
        /// </summary>
        [XmlEnum("A")]
        A,

        /// <summary>
        ///     Normal delivery priority.
        /// </summary>
        [XmlEnum("B")]
        B
    }
}