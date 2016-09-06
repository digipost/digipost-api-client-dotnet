using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum NondeliverableHandling
    {
        /// <summary>
        ///     If mail is undeliverable the mail will be returned to the return address.
        /// </summary>
        ReturnToSender
    }
}