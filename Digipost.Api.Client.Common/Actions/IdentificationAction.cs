using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Utilities;

namespace Digipost.Api.Client.Common.Actions
{
    internal class IdentificationAction : DigipostAction
    {
        public IdentificationAction(IIdentification identification, ClientConfig clientConfig, X509Certificate2 businessCertificate)
            : base(identification)
        {
        }

        internal override HttpContent Content(IRequestContent requestContent)
        {
            var xmlMessage = Serialize(requestContent);
            var messageContent = new StringContent(xmlMessage);

            var boundary = Guid.NewGuid().ToString();
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V7);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;

            return messageContent;
        }

        protected override string Serialize(IRequestContent requestContent)
        {
            var identificationDto = DataTransferObjectConverter.ToDataTransferObject((IIdentification) requestContent);
            return SerializeUtil.Serialize(identificationDto);
        }
    }
}