using System.Threading.Tasks;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.PersonDetails;

namespace Digipost.Api.Client.Api
{
    internal interface IDigipostApi
    {
        MessageDeliveryResult SendMessage(Message message);

        Task<MessageDeliveryResult> SendMessageAsync(Message message);

        IdentificationResult Identify(Identification identification);

        Task<IdentificationResult> IdentifyAsync(Identification identification);

        Task<PersonDetailsResult> SearchAsync(string search);

        PersonDetailsResult Search(string search);

        IDigipostActionFactory DigipostActionFactory { get; set; }
    }
}