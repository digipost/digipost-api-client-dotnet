using System.Threading.Tasks;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client.Api
{
    internal interface IDigipostApi
    {
        IMessageDeliveryResult SendMessage(SendRequestHelper sendRequestHelper, IMessage messageDataTransferObject);

        Task<IMessageDeliveryResult> SendMessageAsync(SendRequestHelper sendRequestHelper, IMessage messageDataTransferObject);

        IIdentificationResult Identify(IIdentification identification);

        Task<IIdentificationResult> IdentifyAsync(IIdentification identification);

        Task<ISearchDetailsResult> SearchAsync(string search);

        ISearchDetailsResult Search(string search);
    }
}