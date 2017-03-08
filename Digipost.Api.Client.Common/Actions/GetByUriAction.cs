using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Common.Actions
{
    internal class GetByUriAction : DigipostAction
    {
        public GetByUriAction(IRequestContent requestContent, ClientConfig clientConfig, X509Certificate2 businessCertificate, Uri uri)
            : base(requestContent, clientConfig, businessCertificate, uri)
        {
        }

        protected override HttpContent Content(IRequestContent requestContent)
        {
            return null;
        }

        protected override string Serialize(IRequestContent requestContent)
        {
            return null;
        }
    }
}