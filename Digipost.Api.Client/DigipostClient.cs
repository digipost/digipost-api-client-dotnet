using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Handlers;

namespace Digipost.Api.Client
{
    public class DigipostClient
    {
        public DigipostClient(ClientConfig clientConfig, X509Certificate2 privateCertificate)
        {
            ClientConfig = clientConfig;
            PrivateCertificate = privateCertificate;
        }

        public DigipostClient(ClientConfig clientConfig, string thumbprint)
        {
            ClientConfig = clientConfig;
            PrivateCertificate = CertificateUtility.SenderCertificate(thumbprint, Language.English);
        }

        private ClientConfig ClientConfig { get; set; }
        private X509Certificate2 PrivateCertificate { get; set; }

        public MessageDeliveryResult SendMessage(Message message)
        {
            return SendMessageAsync(message).Result;
        }

        public IdentificationResult Identify(Identification identification)
        {
            return IdentifyAsync(identification).Result;
        }

        public async Task<MessageDeliveryResult> SendMessageAsync(Message message)
        {
            const string uri = "messages";
            var result = await GenericSendAsync<MessageDeliveryResult>(message, uri, message.GetType());

            return result;
        }

        public async Task<IdentificationResult> IdentifyAsync(Identification identification)
        {
            const string uri = "identification";
            var result = await GenericSendAsync<IdentificationResult>(identification, uri);

            return result;
        }

        private async Task<T> GenericSendAsync<T>(XmlBodyContent message, string uri)
        {
            DigipostAction action = DigipostAction.CreateClass(message.GetType(),ClientConfig, PrivateCertificate, uri);
            var requestResult = action.SendAsync(message).Result;
            var contentResult = await ReadResponse(requestResult);
            var result = SerializeUtil.Deserialize<T>(contentResult);
            return result;
        }
        

        private static async Task<string> ReadResponse(HttpResponseMessage requestResult)
        {
            var contentResult = await requestResult.Content.ReadAsStringAsync();
            Logging.Log(TraceEventType.Information, string.Format("  - Result \n [{0}]", contentResult));
            return contentResult;
        }

    }
}