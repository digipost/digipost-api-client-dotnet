using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum PostType
    {
        /// <summary>
        ///     Increased delivery priority. Note: additional charges may apply.
        /// </summary>
        A,

        /// <summary>
        ///     Normal delivery priority.
        /// </summary>
        B
    }
}