using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Exceptions;

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
            var messageDelivery = SendMessageAsync(message);

            if (messageDelivery.IsFaulted)
            {
                throw messageDelivery.Exception.InnerException;
            }

            return messageDelivery.Result;
        }

        public IdentificationResult Identify(Identification identification)
        {
            var identifyResponse =  IdentifyAsync(identification);
            
            if (identifyResponse.IsFaulted)
            {
                throw identifyResponse.Exception.InnerException;
            }
            return identifyResponse.Result;
        }

        public async Task<MessageDeliveryResult> SendMessageAsync(Message message)
        {
            const string uri = "messages";
            var result = await GenericSendAsync<MessageDeliveryResult>(message, uri);

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
            var response = action.SendAsync(message).Result;
            var responseContent = await ReadResponse(response);


            try
            {
                return SerializeUtil.Deserialize<T>(responseContent);
            }
            catch (InvalidOperationException exception)
            {
                var error=  SerializeUtil.Deserialize<Error>(responseContent);
                throw  new ClientResponseException("Failed to deserialize response object." +
                                                   "Check inner Error object for more information.",error,exception);
            }
        }
        
        private static async Task<string> ReadResponse(HttpResponseMessage requestResult)
        {
            var contentResult = await requestResult.Content.ReadAsStringAsync();
            return contentResult;
        }

    }
}