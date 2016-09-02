using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;

namespace Digipost.Api.Client.Action
{
    internal class DigipostActionFactory : IDigipostActionFactory
    {
        public virtual DigipostAction CreateClass(IRequestContent content, ClientConfig clientConfig,
            X509Certificate2 businessCertificate, Uri uri)
        {
            var type = content.GetType();

            if (typeof (IMessage).IsAssignableFrom(type))
            {
                return new MessageAction((IMessage) content, clientConfig, businessCertificate, uri);
            }

            if (typeof (IIdentification).IsAssignableFrom(type))
            {
                return new IdentificationAction((IIdentification) content, clientConfig, businessCertificate, uri);
            }

            throw new ConfigException(string.Format("Could not create class with type {0}", content.GetType().Name));
        }

        public virtual DigipostAction CreateClass(ClientConfig clientConfig, X509Certificate2 businessCertificate,
            Uri uri)
        {
            return new GetByUriAction(null, clientConfig, businessCertificate, uri);
        }
    }
}