using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identify
{
    public interface IIdentification : IRequestContent
    {
        object Data{ get; }

        IdentificationChoiceType IdentificationChoiceType { get; } 
    }
}