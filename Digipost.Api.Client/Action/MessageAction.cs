using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client
{
    internal class MessageAction : DigipostAction
    {
        public MessageAction(ClientConfig clientConfig, X509Certificate2 privateCertificate, string uri)
            : base(clientConfig, privateCertificate, uri)
        {
        }

        protected override HttpContent Content(XmlBodyContent xmlBodyContent)
        {
            var message = xmlBodyContent as Message;
            var boundary = Guid.NewGuid().ToString();

            var content = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            content.Headers.ContentType = mediaTypeHeaderValue;

            AddBodyToContent(message, content);
            AddPrimaryDocumentToContent(message, content);
            AddAttachmentsToContent(message, content);

            return content;
        }

        private static void AddBodyToContent(Message message, MultipartFormDataContent content)
        {
            Logging.Log(TraceEventType.Information, "  - Creating XML-body");
            var xmlMessage = SerializeUtil.Serialize(message);

            Logging.Log(TraceEventType.Information, string.Format("   -  XML-body \n [{0}]", xmlMessage));
            var messageContent = new StringContent(xmlMessage);
            messageContent.Headers.ContentType = new MediaTypeHeaderValue(DigipostVersion.V6);
            messageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "\"message\""
            };
            content.Add(messageContent);
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
    }
}