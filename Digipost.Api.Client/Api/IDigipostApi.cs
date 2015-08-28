using System.Threading.Tasks;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Api
{
    internal interface IDigipostApi
    {
        MessageDeliveryResultDataTransferObject SendMessage(IMessage messageDataTransferObject);

        Task<MessageDeliveryResultDataTransferObject> SendMessageAsync(IMessage messageDataTransferObject);

        IdentificationResult Identify(IIdentification identification);

        Task<IdentificationResult> IdentifyAsync(IIdentification identification);

        Task<SearchDetailsResult> SearchAsync(string search);

        SearchDetailsResult Search(string search);

        IDigipostActionFactory DigipostActionFactory { get; set; }
    }
}