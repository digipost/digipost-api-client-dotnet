using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.DataTransferObject;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Utilities;

namespace Digipost.Api.Client.Action
{
    internal class IdentificationAction : DigipostAction
    {
        public IdentificationAction(IIdentification identification, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
            : base(identification, clientConfig, businessCertificate, uri)
        {
        }

        protected override HttpContent Content(IRequestContent requestContent)
        {
            var xmlMessage = Serialize(requestContent);
            var messageContent = new StringContent(xmlMessage);

            var boundary = Guid.NewGuid().ToString();
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(DigipostVersion.V6);
            mediaTypeHeaderValue.Parameters.Add(new NameValueWithParametersHeaderValue("boundary", boundary));
            messageContent.Headers.ContentType = mediaTypeHeaderValue;

            return messageContent;
        }

        protected override string Serialize(IRequestContent requestContent)
        {
            IdentificationDataTransferObject identificationDto = DataTransferObjectConverter.ToDataTransferObject((Identification)requestContent);
            return SerializeUtil.Serialize(identificationDto);
        }
    }
}