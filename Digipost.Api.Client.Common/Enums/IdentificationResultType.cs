using System;

namespace Digipost.Api.Client.Common.Enums
{
    public enum IdentificationResultType
    {
        DigipostAddress,

        InvalidReason,

        [Obsolete("No longer returned by the API.")]
        Personalias,

        UnidentifiedReason
    }
}
