using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Archive.Actions
{
    internal class ArchiveDocumentAction : DigipostAction<ArchiveDocument>
    {
        public ArchiveDocumentAction(ArchiveDocument archiveDocument)
            : base(archiveDocument)
        {
        }

        internal override HttpContent Content(ArchiveDocument requestContent)
        {
            var xmlMessage = Serialize(requestContent);
            var messageContent = new StringContent(xmlMessage);

            var boundary = Guid.NewGuid().ToString();
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V8);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;

            return messageContent;
        }

        protected override string Serialize(ArchiveDocument requestContent)
        {
            var messageDataTransferObject = requestContent.ToDataTransferObject();
            return SerializeUtil.Serialize(messageDataTransferObject);
        }
    }
}
