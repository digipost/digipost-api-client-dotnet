using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Send
{
    internal class SendRequestHelper : RequestHelper
    {
        internal SendRequestHelper(HttpClient httpClient, ClientConfig clientConfig, X509Certificate2 businessCertificate)
            : base(httpClient, clientConfig, businessCertificate)
        {
        }

        internal Task<T> PostMessage<T>(IMessage message, Uri uri)
        {
            var messageAction = new MessageAction(message);
            var httpContent = messageAction.Content(message);

            return Post<T>(httpContent, messageAction.RequestContent, uri);
        }
    }
}