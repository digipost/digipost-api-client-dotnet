using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Exceptions;

namespace Digipost.Api.Client.Action
{
    internal class DigipostActionFactory : IDigipostActionFactory
    {
        public virtual DigipostAction CreateClass(RequestContent content, ClientConfig clientConfig, X509Certificate2 businessCertificate, string uri)
        {
            var type = content.GetType();

            if (type == typeof(Message))
            {
                return new MessageAction(content as Message, clientConfig, businessCertificate, uri);
            }

            if (type == typeof(Identification))
            {
                return new IdentificationAction(content as Identification, clientConfig, businessCertificate, uri);
            }

            throw new ConfigException(string.Format("Could not create class with type {0}", content.GetType().Name));
        }
    }
}
