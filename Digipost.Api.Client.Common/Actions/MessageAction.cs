using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;

namespace Digipost.Api.Client.Action
{
    internal class MessageAction : DigipostAction
    {
        public MessageAction(IMessage message, ClientConfig clientConfig, X509Certificate2 businessCertificate, Uri uri)
            : base(message, clientConfig, businessCertificate, uri)
        {
        }

        protected override HttpContent Content(IRequestContent requestContent)
        {
            var message = requestContent as IMessage;
            var boundary = Guid.NewGuid().ToString();

            var multipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue("multipart/vnd.digipost-v7+xml");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            AddBodyToContent(message, multipartFormDataContent);
            AddPrimaryDocumentToContent(message, multipartFormDataContent);
            AddAttachmentsToContent(message, multipartFormDataContent);

            return multipartFormDataContent;
        }

        protected override string Serialize(IRequestContent requestContent)
        {
            var messageDataTransferObject =
                DataTransferObjectConverter.ToDataTransferObject((Message) requestContent);
            return SerializeUtil.Serialize(messageDataTransferObject);
        }

        private static void AddBodyToContent(IMessage message, MultipartFormDataContent content)
        {
            var messageDataTransferObject = DataTransferObjectConverter.ToDataTransferObject(message);
            var xmlMessage = SerializeUtil.Serialize(messageDataTransferObject);

            var messageContent = new StringContent(xmlMessage);
            messageContent.Headers.ContentType = new MediaTypeHeaderValue(DigipostVersion.V7);
            messageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "\"message\""
            };
            content.Add(messageContent);
        }

        private static void AddPrimaryDocumentToContent(IMessage message, MultipartFormDataContent content)
        {
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