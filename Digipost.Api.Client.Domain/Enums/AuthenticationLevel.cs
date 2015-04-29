using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    /// <summary>
    /// Authentication-levels you may require the receiver to be authenticated with to read mail.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "authentication-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum AuthenticationLevel
    {
        /// <summary>
        /// Default. Social security number and password is required to read the mail.
        /// </summary>
        [XmlEnum("PASSWORD")]
        Password,

        /// <summary>
        /// Two factor authentication will be required to read the mail.
        /// </summary>
        [XmlEnum("TWO_FACTOR")]
        TwoFactor
    }
}
