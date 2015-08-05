using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client
{
    internal class GetByUriAction : DigipostAction
    {
        public GetByUriAction(RequestContent requestContent, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
            : base(requestContent, clientConfig, businessCertificate, uri)
        {

        }

        protected override HttpContent Content(RequestContent requestContent)
        {
            return null;
        }

        protected override string Serialize(RequestContent requestContent)
        {
            return null;
        }
    }
}
