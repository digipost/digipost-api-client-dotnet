using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain;

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

        public IdentificationResult Identify(Identification identification)
        {
            return api.Identify(identification);
        }

        public Task<IdentificationResult> IdentifyAsync(Identification identification)
        {
            return api.IdentifyAsync(identification);
        }

        public MessageDeliveryResult SendMessage(Message message)
        {
            return api.SendMessage(message);
        }

        public Task<MessageDeliveryResult> SendMessageAsync(Message message)
        {
            return api.SendMessageAsync(message);
        }
    }
}