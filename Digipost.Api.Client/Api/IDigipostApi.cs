using System.Threading.Tasks;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Api
{
    internal interface IDigipostApi
    {
        MessageDeliveryResult SendMessage(Message message);

        IdentificationResult Identify(Identification identification);

        Task<MessageDeliveryResult> SendMessageAsync(Message message);

        Task<IdentificationResult> IdentifyAsync(Identification identification);
    }
}