using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    /// <summary>
    /// Authentication-levels you may require the receiver to be authenticated with to read documents.
    /// Note that IDPORTEN_3 and IDPORTEN_4 needs an explicit permission to use, and are intended for
    /// government agencies. Using those levels while not being permitted to, will result in an error
    /// response.
    /// </summary>
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [XmlType(TypeName = "authentication-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum AuthenticationLevel
    {
        /// <summary>
        /// Default. Password is required to read the message.
        /// </summary>
        [XmlEnum("PASSWORD")]
        Password,

        /// <summary>
        /// Two factor authentication will be required to read it.
        /// </summary>
        [XmlEnum("TWO_FACTOR")]
        TwoFactor,

        /// <summary>
        /// ID-porten level 3. Can only be used by government agencies.
        /// </summary>
        [XmlEnum("IDPORTEN_3")]
        IdPorten3,

        /// <summary>
        /// ID-porten level 4. Can only be used by government agencies.
        /// </summary>
        [XmlEnum("IDPORTEN_4")]
        IdPorten4,
    }
}
