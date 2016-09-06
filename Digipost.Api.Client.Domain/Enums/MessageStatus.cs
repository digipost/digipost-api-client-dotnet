using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum MessageStatus
    {
        /// <summary>
        ///     The message resource is not complete. Consult the provided links to see what options are availiable.
        /// </summary>
        NotComplete,

        /// <summary>
        ///     The message resource is complete, and can be sent. Note that you can also tweak the message before sending it.
        ///     Consult the provided links to see what options are availiable.
        /// </summary>
        Complete,

        /// <summary>
        ///     The message is delivered. Consult the provided links to see what options are availiable.
        /// </summary>
        Delivered,

        /// <summary>
        ///     The message is delivered to print. Consult the provided links to see what options are availiable.
        /// </summary>
        DeliveredToPrint
    }
}