using System;
using System.Threading.Tasks;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Send
{
    internal class SendRequestHelper
    {
        private readonly RequestHelper _requestHelper;

        internal SendRequestHelper(RequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        internal Task<T> Get<T>(Uri uri)
        {
            return _requestHelper.Get<T>(uri);
        }

        internal Task<T> PostMessage<T>(IMessage message, Uri uri)
        {
            var messageAction = new MessageAction(message);
            var httpContent = messageAction.Content(message);

            return _requestHelper.Post<T>(httpContent, messageAction.RequestContent, uri);
        }

        internal Task<T> PostIdentification<T>(IIdentification identification, Uri uri)
        {
            var messageAction = new IdentificationAction(identification);
            var httpContent = messageAction.Content(identification);

            return _requestHelper.Post<T>(httpContent, messageAction.RequestContent, uri);
        }
    }
}