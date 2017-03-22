using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Common.Logging;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Extensions;
using Digipost.Api.Client.Scripts.Xsd.XsdToCode.Code;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client.Api
{
    internal class SendMessageApi
    {
        private const int MinimumSearchLength = 3;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SendMessageApi(SendRequestHelper requestHelper)
        {
            RequestHelper = requestHelper;
        }

        public SendRequestHelper RequestHelper { get; }

        public IMessageDeliveryResult SendMessage(IMessage message)
        {
            var messageDelivery = SendMessageAsync(message);

            if (messageDelivery.IsFaulted && messageDelivery.Exception != null)
                throw messageDelivery.Exception.InnerException;

            return messageDelivery.Result;
        }

        public async Task<IMessageDeliveryResult> SendMessageAsync(IMessage message)
        {
            Log.Debug($"Outgoing Digipost message to Recipient: {message.DigipostRecipient}");

            var uri = new Uri("messages", UriKind.Relative);

            var messageDeliveryResultTask = RequestHelper.PostMessage<messagedelivery>(message, uri);

            if (messageDeliveryResultTask.IsFaulted && messageDeliveryResultTask.Exception != null)
                throw messageDeliveryResultTask.Exception?.InnerException;

            var messageDeliveryResult = SendDataTransferObjectConverter.FromDataTransferObject(await messageDeliveryResultTask.ConfigureAwait(false));

            Log.Debug($"Response received for message to recipient, {message.DigipostRecipient}: '{messageDeliveryResult.Status}'. Will be available to Recipient at {messageDeliveryResult.DeliveryTime}.");

            return messageDeliveryResult;
        }

        public IIdentificationResult Identify(IIdentification identification)
        {
            return IdentifyAsync(identification).Result;
        }

        public async Task<IIdentificationResult> IdentifyAsync(IIdentification identification)
        {
            Log.Debug($"Outgoing identification request: {identification}");

            var uri = new Uri("identification", UriKind.Relative);

            var identifyResponse = RequestHelper.PostIdentification<identificationresult>(identification, uri);

            if (identifyResponse.IsFaulted)
            {
                var exception = identifyResponse.Exception?.InnerException;

                Log.Warn($"Identification failed, {exception}");

                if (identifyResponse.Exception != null)
                    throw identifyResponse.Exception.InnerException;
            }

            var identificationResultDataTransferObject = await identifyResponse.ConfigureAwait(false);
            var identificationResult = DataTransferObjectConverter.FromDataTransferObject(identificationResultDataTransferObject);

            Log.Debug($"Response received for identification to recipient, ResultType '{identificationResult.ResultType}', Data '{identificationResult.Data}'.");

            return identificationResult;
        }

        public ISearchDetailsResult Search(string search)
        {
            return SearchAsync(search).Result;
        }

        public async Task<ISearchDetailsResult> SearchAsync(string search)
        {
            Log.Debug($"Outgoing search request, term: '{search}'.");

            search = search.RemoveReservedUriCharacters();
            var uri = new Uri($"recipients/search/{Uri.EscapeUriString(search)}", UriKind.Relative);

            if (search.Length < MinimumSearchLength)
            {
                var emptyResult = new SearchDetailsResult {PersonDetails = new List<SearchDetails>()};

                var taskSource = new TaskCompletionSource<ISearchDetailsResult>();
                taskSource.SetResult(emptyResult);
                return await taskSource.Task.ConfigureAwait(false);
            }

            var searchDetailsResultDataTransferObject = await RequestHelper.Get<recipients>(uri).ConfigureAwait(false);

            var searchDetailsResult = DataTransferObjectConverter.FromDataTransferObject(searchDetailsResultDataTransferObject);

            Log.Debug($"Response received for search with term '{search}' retrieved.");

            return searchDetailsResult;
        }
    }
}