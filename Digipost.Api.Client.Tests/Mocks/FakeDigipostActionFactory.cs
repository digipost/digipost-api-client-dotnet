using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Tests.Mocks
{
    internal class FakeDigipostActionFactory : IDigipostActionFactory
    {
        public DigipostAction CreateClass(Type type, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {

            if (type == typeof(Message))
            {
                return new FakeMessageAction(clientConfig, businessCertificate, uri)
                {
                    HttpClient = new HttpClient(new FakeMessageResponseHandler())
                    {
                        BaseAddress = new Uri("http://fake.uri.for.tests.no") 
                    }
                    
                };
            }

            if (type == typeof(Identification))
            {
                return new IdentificationAction(clientConfig, businessCertificate, uri)
                {
                    HttpClient = new HttpClient(new FakeIdentificationResponseHandler())
                    {
                        BaseAddress = new Uri("http://fake.uri.for.tests.no")
                    }
                };
            }

            throw new Exception(string.Format("Could not create class with type {0}", type.Name));
        }
    }
}
