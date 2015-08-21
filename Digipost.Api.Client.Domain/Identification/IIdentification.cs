using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identification
{
    public interface IIdentification : IRequestContent
    {
        object Data{ get; }

        IdentificationChoice IdentificationChoice { get; } 
    }
}