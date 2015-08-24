using System.Threading.Tasks;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.PersonDetails;
using Digipost.Api.Client.Domain.SendMessage;
using IMessage = Digipost.Api.Client.Domain.SendMessage.IMessage;

namespace Digipost.Api.Client.Api
{
    internal interface IDigipostApi
    {
        MessageDeliveryResult SendMessage(IMessage messageDataTransferObject);

        Task<MessageDeliveryResult> SendMessageAsync(IMessage messageDataTransferObject);

        IdentificationResult Identify(IIdentification identification);

        Task<IdentificationResult> IdentifyAsync(IIdentification identification);

        Task<PersonDetailsResult> SearchAsync(string search);

        PersonDetailsResult Search(string search);

        IDigipostActionFactory DigipostActionFactory { get; set; }
    }
}