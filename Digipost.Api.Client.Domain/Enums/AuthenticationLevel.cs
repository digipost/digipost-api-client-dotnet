using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    /// <summary>
    ///     The authentication level you require that the recipient have to open the letter.
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "authentication-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum AuthenticationLevel
    {
        /// <summary>
        ///     Default. Social security number and password is required to open the letter.
        /// </summary>
        [XmlEnum("PASSWORD")]
        Password,

        /// <summary>
        ///     Two factor authentication will be required to open the letter.
        /// </summary>
        [XmlEnum("TWO_FACTOR")]
        TwoFactor
    }
}