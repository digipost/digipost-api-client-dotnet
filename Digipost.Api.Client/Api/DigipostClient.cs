using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Api
{
    public class DigipostClient 
    {

        private readonly DigipostApi _api;
        public DigipostClient(ClientConfig clientConfig, X509Certificate2 businessCertificate)
        {
            _api = new DigipostApi(clientConfig,businessCertificate);
        }

        public DigipostClient(ClientConfig clientConfig, string thumbprint)
        {
            _api = new DigipostApi(clientConfig,thumbprint);
        }

        public IdentificationResult Identify(Identification identification)
        {
            return _api.Identify(identification);
        }

        public Task<IdentificationResult> IdentifyAsync(Identification identification)
        {
            return _api.IdentifyAsync(identification);
        }

        public MessageDeliveryResult SendMessage(Message message)
        {
            return _api.SendMessage(message);
        }

        public Task<MessageDeliveryResult> SendMessageAsync(Message message)
        {
            return _api.SendMessageAsync(message);
        }
    }
}