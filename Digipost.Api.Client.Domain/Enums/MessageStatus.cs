using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    [Serializable]
    [XmlType(TypeName = "message-status", Namespace = "http://api.digipost.no/schema/v6")]
    [XmlRoot("message-status", Namespace = "http://api.digipost.no/schema/v6", IsNullable = false)]
    public enum MessageStatus
    {
        /// <summary>
        ///     The message resource is not complete. Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("NOT_COMPLETE")] NotComplete,

        /// <summary>
        ///     The message resource is complete, and can be sent. Note that you can also tweak the message before sending it.
        ///     Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("COMPLETE")] Complete,

        /// <summary>
        ///     The message is delivered. Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("DELIVERED")] Delivered,

        /// <summary>
        ///     The message is delivered to print. Consult the provided links to see what options are availiable.
        /// </summary>
        [XmlEnum("DELIVERED_TO_PRINT")] DeliveredToPrint
    }
}