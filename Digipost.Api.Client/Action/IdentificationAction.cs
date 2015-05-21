using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Action
{
    internal class IdentificationAction : DigipostAction
    {
        public IdentificationAction(ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
            : base(clientConfig, businessCertificate, uri)
        {
        }

        protected override HttpContent Content(RequestContent requestContent)
        {
            var identification = requestContent as Identification;
            var xmlMessage = SerializeUtil.Serialize(identification);
            var messageContent = new StringContent(xmlMessage);

            var boundary = Guid.NewGuid().ToString();
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V6);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;

            return messageContent;
        }
    }
}