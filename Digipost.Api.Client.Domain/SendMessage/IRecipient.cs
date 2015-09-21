using System;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Print;

namespace Digipost.Api.Client.Domain.SendMessage
{
    public interface IRecipient
    {
        object IdentificationValue { get; set; }

        IPrintDetails PrintDetails{ get; set; }

        IdentificationChoiceType? IdentificationType { get; set; }

    }
}