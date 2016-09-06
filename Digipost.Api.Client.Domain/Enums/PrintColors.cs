using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum PrintColors
    {
        /// <summary>
        ///     Prints the document in black/white.
        /// </summary>
        Monochrome,

        /// <summary>
        ///     Prints the document in colors. Note: additional charges may apply.
        /// </summary>
        Colors
    }
}