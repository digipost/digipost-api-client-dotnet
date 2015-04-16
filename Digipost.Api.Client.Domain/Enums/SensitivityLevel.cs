using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    /// <summary>
    /// Defines if the message is sensitive or not.
    /// </summary>
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.33440")]
    [XmlType(TypeName = "sensitivity-level", Namespace = "http://api.digipost.no/schema/v6")]
    public enum SensitivityLevel
    {
        /// <summary>
        /// Default. Non sensitive message. Metadata about the message, like the sender and subject,
        /// will be revealed in user notifications (eg. email and SMS), and can also be seen when logged in at a
        /// security level below the one specified for the message.
        /// </summary>
        [XmlEnum("NORMAL")] 
        Normal,

        /// <summary>
        /// Sensitive message. Metadata about the message, like the sender and subject, will be hidden
        /// until logged in at the appropriate security level specified for the message.
        /// </summary>
        [XmlEnum("SENSITIVE")] 
        Sensitive
    }
}