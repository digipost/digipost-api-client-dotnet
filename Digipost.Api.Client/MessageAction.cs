using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Api.Client
{
    class MessageAction : DigipostAction
    {
        public MessageAction(ClientConfig clientConfig, X509Certificate2 privateCertificate, string uri) : base(clientConfig, privateCertificate, uri)
        {
        }
    }
}
