using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;

namespace Digipost.Api.Client.Action
{
    internal class DigipostActionFactory : IDigipostActionFactory
    {
        public virtual DigipostAction CreateClass(IRequestContent content, ClientConfig clientConfig, 
            X509Certificate2 businessCertificate, string uri)
        {
            var type = content.GetType();

            if (type == typeof(Message))
            {
                return new MessageAction(content as Message, clientConfig, businessCertificate, uri);
            }

            if (typeof(IIdentification).IsAssignableFrom(type))
            {
                return new IdentificationAction(content as Identification, clientConfig, businessCertificate, uri);
            }

            throw new ConfigException(string.Format("Could not create class with type {0}", content.GetType().Name));
        }

        public virtual DigipostAction CreateClass(ClientConfig clientConfig, X509Certificate2 businessCertificate,
            string uri)
        {
            return new GetByUriAction(null, clientConfig, businessCertificate, uri);
        }
    }
}
