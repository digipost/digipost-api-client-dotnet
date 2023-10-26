using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Send.Actions
{
    internal class MessageAction : DigipostAction<IMessage>
    {
        public MessageAction(IMessage message)
            : base(message)
        {
        }

        internal override HttpContent Content(IMessage message)
        {
            var boundary = Guid.NewGuid().ToString();

            var multipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V8_MULTIPART);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            AddBodyToContent(message, multipartFormDataContent);
            AddDocumentsToContent(message, multipartFormDataContent);

            return multipartFormDataContent;
        }

        protected override string Serialize(IMessage requestContent)
        {
            var messageDataTransferObject = requestContent.ToDataTransferObject();
            return SerializeUtil.Serialize(messageDataTransferObject);
        }

        private static void AddBodyToContent(IMessage message, MultipartContent content)
        {
            var messageDataTransferObject = message.ToDataTransferObject();
            var xmlMessage = SerializeUtil.Serialize(messageDataTransferObject);

            var messageContent = new StringContent(xmlMessage);
            messageContent.Headers.ContentType = new MediaTypeHeaderValue(DigipostVersion.V8);
            messageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "\"message\""
            };
            content.Add(messageContent);
        }

        private static void AddDocumentsToContent(IMessage message, MultipartContent content)
        {
            var documents = new List<IDocument> {message.PrimaryDocument};
            documents.AddRange(message.Attachments);

            foreach (var document in documents)
            {
                var attachmentContent = new ByteArrayContent(document.ContentBytes);
                attachmentContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                attachmentContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = document.Guid
                    };
                content.Add(attachmentContent);
            }
        }
    }
}
