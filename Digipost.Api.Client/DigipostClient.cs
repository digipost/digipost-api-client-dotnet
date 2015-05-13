using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public ClientResponse SendMessage(Message message)
        {
            return SendMessageAsync(message).Result;
        }

        public async Task<ClientResponse> SendMessageAsync(Message message)
        {
            DigipostAction action = new MessageAction(ClientConfig,PrivateCertificate,"messages");
            var requestResult = action.SendAsync(message).Result;
            var contentResult = await ReadResponse(requestResult);
            return CreateSendResponse(contentResult, requestResult.StatusCode);
        }

        //public async Task<IdentificationResult> Identify(Identification identification)
        //{
        //    const string uri = "identification";
        //    DigipostAction action = new IdentificationAction(ClientConfig, PrivateCertificate, "messages");

        //    var requestResult = client.PostAsync(uri, messageContent).Result;
        //    var contentResult = await ReadResponse(requestResult);
        //    var identificationResult = SerializeUtil.Deserialize<IdentificationResult>(contentResult);

        //    return identificationResult;

        //}

        private static StringContent CreateMessageContent(Identification identification)
        {
            var xmlMessage = SerializeUtil.Serialize(identification);
            Logging.Log(TraceEventType.Information, string.Format("   -  XML-body \n [{0}]", xmlMessage));
            var messageContent = new StringContent(xmlMessage);
            return messageContent;
        }

        private static void AddHeaderToContent(string boundary, StringContent messageContent)
        {
            var mediaTypeHeaderValue = new MediaTypeHeaderValue("application/vnd.digipost-v6+xml");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;
        }

        private static async Task<string> ReadResponse(HttpResponseMessage requestResult)
        {
            var contentResult = await requestResult.Content.ReadAsStringAsync();
            Logging.Log(TraceEventType.Information, string.Format("  - Result \n [{0}]", contentResult));
            return contentResult;
        }

        private static void AddHeaderToContent(string boundary, MultipartFormDataContent content)
        {
            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            content.Headers.ContentType = mediaTypeHeaderValue;
        }


        private static void AddBodyToContent(Message message, MultipartFormDataContent content)
        {
            {
                Logging.Log(TraceEventType.Information, "  - Creating XML-body");
                var xmlMessage = SerializeUtil.Serialize(message);

                Logging.Log(TraceEventType.Information, string.Format("   -  XML-body \n [{0}]", xmlMessage));
                var messageContent = new StringContent(xmlMessage);
                messageContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.digipost-v6+xml");
                messageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "\"message\""
                };
                content.Add(messageContent);
            }
        }
       

        private static void AddPrimaryDocumentToContent(Message message, MultipartFormDataContent content)
        {
            Logging.Log(TraceEventType.Information, "  - Adding primary document");
            var documentContent = new ByteArrayContent(message.PrimaryDocument.ContentBytes);
            documentContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            documentContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = message.PrimaryDocument.Guid
            };
            content.Add(documentContent);
        }

        private static void AddAttachmentsToContent(Message message, MultipartFormDataContent content)
        {
            foreach (var attachment in message.Attachments)
            {
                Logging.Log(TraceEventType.Information, "  - Adding attachment");
                var attachmentContent = new ByteArrayContent(attachment.ContentBytes);
                attachmentContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                attachmentContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = attachment.Guid
                    };
                content.Add(attachmentContent);
            }
        }

        private static ClientResponse CreateSendResponse(string contentResult, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                var messagedelivery = SerializeUtil.Deserialize<MessageDelivery>(contentResult);
                return new ClientResponse(messagedelivery, contentResult);
            }
            var error = SerializeUtil.Deserialize<Error>(contentResult);
            return new ClientResponse(error, contentResult);
        }

       
    }
}