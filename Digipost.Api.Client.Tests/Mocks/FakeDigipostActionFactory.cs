using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;

namespace Digipost.Api.Client.Tests.Mocks
{
    internal class FakeDigipostActionFactory : IDigipostActionFactory 
    {
        public DigipostAction CreateClass(Type type, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            var action = new MessageAction(clientConfig, businessCertificate, uri);
            
            
            action.HttpClient = new HttpClient(new FakeMessageResponseHandler());

            return null;
        }
    }
}
