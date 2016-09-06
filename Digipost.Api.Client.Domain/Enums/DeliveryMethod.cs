using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum DeliveryMethod
    {
        /// <summary>
        ///     Delivered through physical print and postal service.
        /// </summary>
        Print,

        /// <summary>
        ///     Delivered digitally in Digipost
        /// </summary>
        Digipost
    }
}