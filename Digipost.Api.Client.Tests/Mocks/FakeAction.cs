using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Tests.Mocks
{
    public class FakeAction : DigipostAction
    {
        public FakeAction(ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri) : base(clientConfig, businessCertificate, uri)
        {
        }

        internal FakeAction() : base(new ClientConfig("1337"), new X509Certificate2(), "http://fake.uri.com"  )
        {
            
        }

        protected override HttpContent Content(RequestContent requestContent)
        {
            return new StringContent("Tullebeskjed som sendes");
        }
    }
}
