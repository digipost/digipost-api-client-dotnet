using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Api
{
    public class DigipostClient 
    {

        private DigipostApi api;
        public DigipostClient(ClientConfig clientConfig, X509Certificate2 businessCertificate)
        {
            api = new DigipostApi(clientConfig,businessCertificate);
            Logging.Initialize(clientConfig);
        }

        public DigipostClient(ClientConfig clientConfig, string thumbprint)
        {
            api = new DigipostApi(clientConfig,thumbprint);
            Logging.Initialize(clientConfig);
        }

        public IdentificationResult Identify(IIdentification identification)
        {
            return api.Identify(identification);
        }

        public Task<IdentificationResult> IdentifyAsync(IIdentification identification)
        {
            return api.IdentifyAsync(identification);
        }

        public MessageDeliveryResultDataTransferObject SendMessage(IMessage messageDataTransferObject)
        {
            return api.SendMessage(messageDataTransferObject);
        }

        public Task<MessageDeliveryResultDataTransferObject> SendMessageAsync(IMessage messageDataTransferObject)
        {
            return api.SendMessageAsync(messageDataTransferObject);
        }

        public SearchDetailsResult Search(string query)
        {
            return api.Search(query);
        }

        public Task<SearchDetailsResult> SearchAsync(string query)
        {
            return api.SearchAsync(query);
        }
    }
}