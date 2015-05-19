using System;
using System.Security.Cryptography.X509Certificates;

namespace Digipost.Api.Client.Action
{
    internal interface IDigipostActionFactory
    {
        DigipostAction CreateClass(Type type, ClientConfig clientConfig, X509Certificate2 businessCertificate,
            string uri);
    }
}