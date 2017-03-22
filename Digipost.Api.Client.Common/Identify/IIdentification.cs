using Digipost.Api.Client.Common.Recipient;

namespace Digipost.Api.Client.Common.Identify
{
    public interface IIdentification : IRequestContent
    {
        DigipostRecipient DigipostRecipient { get; set; }
    }
}