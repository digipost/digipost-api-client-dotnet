using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Tests.Mocks
{
    public class FakeMessageAction : DigipostAction
    {
        public FakeMessageAction(ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri) : base(clientConfig, businessCertificate, uri)
        {
        }

        protected override HttpContent Content(RequestContent requestContent)
        {
            return new StringContent("Tullebeskjed som sendes");
        }
    }
}
