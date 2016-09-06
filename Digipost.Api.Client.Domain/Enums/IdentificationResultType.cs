using System;
using System.Xml.Serialization;

namespace Digipost.Api.Client.Domain.Enums
{
    public enum IdentificationResultType
    {
        None,

        DigipostAddress,

        InvalidReason,

        Personalias,

        UnidentifiedReason
    }
}