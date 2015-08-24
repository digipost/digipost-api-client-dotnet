using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using IMessage = Digipost.Api.Client.Domain.SendMessage.IMessage;

namespace Digipost.Api.Client
{
    internal class MessageAction : DigipostAction
    {
        public MessageAction(IMessage message, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
            : base(message, clientConfig, businessCertificate, uri)
        {
        }

        protected override HttpContent Content(IRequestContent requestContent)
        {
            var message = requestContent as IMessage;
            var boundary = Guid.NewGuid().ToString();

            var multipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/mixed");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            AddBodyToContent(message, multipartFormDataContent);
            AddPrimaryDocumentToContent(message, multipartFormDataContent);
            AddAttachmentsToContent(message, multipartFormDataContent);

            return multipartFormDataContent;
        }

        protected override string Serialize(IRequestContent requestContent)
        {
            return SerializeUtil.Serialize((IMessage) requestContent);
        }

        private static void AddBodyToContent(IMessage message, MultipartFormDataContent content)
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

        private static void AddPrimaryDocumentToContent(IMessage message, MultipartFormDataContent content)
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

        private static void AddAttachmentsToContent(IMessage message, MultipartFormDataContent content)
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