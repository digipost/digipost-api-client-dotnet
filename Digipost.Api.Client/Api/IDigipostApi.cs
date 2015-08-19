using System.Threading.Tasks;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Identification;
using Digipost.Api.Client.Domain.PersonDetails;

namespace Digipost.Api.Client.Api
{
    internal interface IDigipostApi
    {
        MessageDeliveryResult SendMessage(Message message);

        Task<MessageDeliveryResult> SendMessageAsync(Message message);

        IdentificationResult Identify(IIdentification identification);

        Task<IdentificationResult> IdentifyAsync(IIdentification identification);

        Task<PersonDetailsResult> SearchAsync(string search);

        PersonDetailsResult Search(string search);

        IDigipostActionFactory DigipostActionFactory { get; set; }
    }
}