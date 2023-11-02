using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Send.Actions
{
    internal class IdentificationAction : DigipostAction<IIdentification>
    {
        public IdentificationAction(IIdentification identification)
            : base(identification)
        {
        }

        internal override HttpContent Content(IIdentification requestContent)
        {
            var xmlMessage = Serialize(requestContent);
            var messageContent = new StringContent(xmlMessage);

            var boundary = Guid.NewGuid().ToString();
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V8);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;

            return messageContent;
        }

        protected override string Serialize(IIdentification requestContent)
        {
            var identificationDto = requestContent.ToDataTransferObject();
            return SerializeUtil.Serialize(identificationDto);
        }
    }
}
