using System;
using System.CodeDom;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Action
{
    internal class DigipostActionFactory : IDigipostActionFactory
    {
        public virtual DigipostAction CreateClass(Type type, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            if (type == typeof(Message))
            {
                return new MessageAction(clientConfig, businessCertificate, uri);
            }

            if (type == typeof(Identification))
            {
                return new IdentificationAction(clientConfig, businessCertificate, uri);
            }

            throw new Exception(string.Format("Could not create class with type {0}", type.Name));
        }
    }
}
