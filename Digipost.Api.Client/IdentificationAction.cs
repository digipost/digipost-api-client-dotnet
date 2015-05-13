using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client
{
    public class IdentificationAction : DigipostAction
    {
        public IdentificationAction(ClientConfig clientConfig, X509Certificate2 privateCertificate, string uri) : base(clientConfig, privateCertificate, uri)
        {
        }

        protected override HttpContent Content(XmlBodyContent xmlBodyContent)
        {
            var identification = xmlBodyContent as Identification;
            var xmlMessage = SerializeUtil.Serialize(identification);
            var messageContent = new StringContent(xmlMessage);

            var boundary = Guid.NewGuid().ToString();
            var mediaTypeHeaderValue = new MediaTypeHeaderValue("application/vnd.digipost-v6+xml");
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;

            return messageContent;
        }
    }
}
