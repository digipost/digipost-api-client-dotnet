using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Archive.Actions
{
    internal class ArchiveAction : DigipostAction<Archive>
    {
        public ArchiveAction(Archive archive)
            : base(archive)
        {
        }

        internal override HttpContent Content(Archive archive)
        {
            var boundary = Guid.NewGuid().ToString();

            var multipartFormDataContent = new MultipartFormDataContent(boundary);

            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V8_MULTIPART);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            AddBodyToContent(archive, multipartFormDataContent);
            AddDocumentsToContent(archive, multipartFormDataContent);

            return multipartFormDataContent;
        }

        protected override string Serialize(Archive requestContent)
        {
            var messageDataTransferObject = ((Archive) requestContent).ToDataTransferObject();
            return SerializeUtil.Serialize(messageDataTransferObject);
        }

        private static void AddBodyToContent(Archive archive, MultipartContent content)
        {
            var messageDataTransferObject = archive.ToDataTransferObject();
            var xmlMessage = SerializeUtil.Serialize(messageDataTransferObject);

            var messageContent = new StringContent(xmlMessage);
            messageContent.Headers.ContentType = new MediaTypeHeaderValue(DigipostVersion.V8);
            messageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "\"archive\""
            };
            content.Add(messageContent);
        }

        private static void AddDocumentsToContent(Archive archive, MultipartContent content)
        {
            foreach (var document in archive.ArchiveDocuments)
            {
                var attachmentContent = new ByteArrayContent(document.ContentBytes);
                attachmentContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                attachmentContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = document.Id.ToString()
                    };
                content.Add(attachmentContent);
            }
        }
    }
}
