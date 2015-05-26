using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Action
{
    internal interface IDigipostActionFactory
    {
        DigipostAction CreateClass(RequestContent requestContent, ClientConfig clientConfig, X509Certificate2 businessCertificate,
            string uri);
    }
}