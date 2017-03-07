using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Domain;

namespace Digipost.Api.Client.Action
{
    internal interface IDigipostActionFactory
    {
        DigipostAction CreateClass(IRequestContent requestContent, ClientConfig clientConfig, X509Certificate2 businessCertificate,
            Uri uri);

        DigipostAction CreateClass(ClientConfig clientConfig, X509Certificate2 businessCertificate,
            Uri uri);
    }
}