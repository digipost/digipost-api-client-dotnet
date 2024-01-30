using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Send.Actions
{
    internal class AddAdditionalDataAction : DigipostAction<IAdditionalData>
    {
        public AddAdditionalDataAction(IAdditionalData additionalData)
            : base(additionalData)
        {
        }

        internal override HttpContent Content(IAdditionalData requestContent)
        {
            var xmlMessage = Serialize(requestContent);
            var messageContent = new StringContent(xmlMessage);

            var boundary = Guid.NewGuid().ToString();
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V8);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;

            return messageContent;
        }

        protected override string Serialize(IAdditionalData requestContent)
        {
            return SerializeUtil.Serialize(requestContent.ToDataTransferObject());
        }
    }
}
