using Digipost.Api.Client.Domain.Enums;

namespace Digipost.Api.Client.Domain.Identify
{
    public interface IIdentification : IRequestContent
    {
        DigipostRecipient DigipostRecipient { get; set; }
        
    }
}