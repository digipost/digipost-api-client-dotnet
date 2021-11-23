using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Extensions;
using Digipost.Api.Client.Send;
using Microsoft.Extensions.Logging;

namespace Digipost.Api.Client.Api
{
    internal class SendMessageApi
    {
        private const int MinimumSearchLength = 3;
        private ILogger<SendMessageApi> _logger;

        public SendMessageApi(SendRequestHelper requestHelper, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SendMessageApi>();
            RequestHelper = requestHelper;
        }

        public SendRequestHelper RequestHelper { get; }

        public IMessageDeliveryResult SendMessage(IMessage message, bool skipMetaDataValidation = false)
        {
            var messageDelivery = SendMessageAsync(message, skipMetaDataValidation);

            if (messageDelivery.IsFaulted && messageDelivery.Exception != null)
                throw messageDelivery.Exception.InnerException;

            return messageDelivery.Result;
        }

        public async Task<IMessageDeliveryResult> SendMessageAsync(IMessage message, bool skipMetaDataValidation = false)
        {
            _logger.LogDebug($"Outgoing Digipost message to Recipient: {message.DigipostRecipient}");

            var uri = new Uri("messages", UriKind.Relative);

            var messageDeliveryResultTask = RequestHelper.PostMessage<V7.Message_Delivery>(message, uri, skipMetaDataValidation);

            if (messageDeliveryResultTask.IsFaulted && messageDeliveryResultTask.Exception != null)
                throw messageDeliveryResultTask.Exception?.InnerException;

            var messageDeliveryResult = SendDataTransferObjectConverter.FromDataTransferObject(await messageDeliveryResultTask.ConfigureAwait(false));

            _logger.LogDebug($"Response received for message to recipient, {message.DigipostRecipient}: '{messageDeliveryResult.Status}'. Will be available to Recipient at {messageDeliveryResult.DeliveryTime}.");

            return messageDeliveryResult;
        }

        public IIdentificationResult Identify(IIdentification identification)
        {
            return IdentifyAsync(identification).Result;
        }

        public async Task<IIdentificationResult> IdentifyAsync(IIdentification identification)
        {
            _logger.LogDebug($"Outgoing identification request: {identification}");

            var uri = new Uri("identification", UriKind.Relative);

            var identifyResponse = RequestHelper.PostIdentification<V7.Identification_Result>(identification, uri);

            if (identifyResponse.IsFaulted)
            {
                var exception = identifyResponse.Exception?.InnerException;

                _logger.LogWarning($"Identification failed, {exception}");

                if (identifyResponse.Exception != null)
                    throw identifyResponse.Exception.InnerException;
            }

            var identificationResultDataTransferObject = await identifyResponse.ConfigureAwait(false);
            var identificationResult = DataTransferObjectConverter.FromDataTransferObject(identificationResultDataTransferObject);

            _logger.LogDebug($"Response received for identification to recipient, ResultType '{identificationResult.ResultType}', Data '{identificationResult.Data}'.");

            return identificationResult;
        }

        public ISearchDetailsResult Search(string search)
        {
            return SearchAsync(search).Result;
        }

        public async Task<ISearchDetailsResult> SearchAsync(string search)
        {
            _logger.LogDebug($"Outgoing search request, term: '{search}'.");

            search = search.RemoveReservedUriCharacters();
            var uri = new Uri($"recipients/search/{Uri.EscapeUriString(search)}", UriKind.Relative);

            if (search.Length < MinimumSearchLength)
            {
                var emptyResult = new SearchDetailsResult {PersonDetails = new List<SearchDetails>()};

                var taskSource = new TaskCompletionSource<ISearchDetailsResult>();
                taskSource.SetResult(emptyResult);
                return await taskSource.Task.ConfigureAwait(false);
            }

            var searchDetailsResultDataTransferObject = await RequestHelper.Get<V7.Recipients>(uri).ConfigureAwait(false);

            var searchDetailsResult = DataTransferObjectConverter.FromDataTransferObject(searchDetailsResultDataTransferObject);

            _logger.LogDebug($"Response received for search with term '{search}' retrieved.");

            return searchDetailsResult;
        }
    }
}
