using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Action
{
    internal class DigipostActionFactory
    {
        public static DigipostAction CreateClass(Type type, ClientConfig clientConfig,
            X509Certificate2 privateCertificate, string uri)
        {
            if (type == typeof(Message))
            {
                return new MessageAction(clientConfig, privateCertificate, uri);
            }

            if (type == typeof(Identification))
            {
                return new IdentificationAction(clientConfig, privateCertificate, uri);
            }

            throw new Exception(string.Format("Could not create class with type{0}", type.Name));
        }
    }
}
