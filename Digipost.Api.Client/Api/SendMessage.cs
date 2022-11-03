using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Extensions;
using Digipost.Api.Client.Send;
using Microsoft.Extensions.Logging;

namespace Digipost.Api.Client.Api
{
    internal class SendMessageApi
    {
        private readonly Root _root;
        private const int MinimumSearchLength = 3;
        private readonly ILogger<SendMessageApi> _logger;

        public SendMessageApi(SendRequestHelper requestHelper, ILoggerFactory loggerFactory, Root root)
        {
            _root = root;
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

            var uri = new Uri(_root.FindByRelationName("CREATE_MESSAGE").Uri, UriKind.Absolute);

            var messageDeliveryResultTask = RequestHelper.PostMessage<V8.Message_Delivery>(message, uri, skipMetaDataValidation);

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

            var uri = new Uri(_root.FindByRelationName("IDENTIFY_RECIPIENT").Uri, UriKind.Absolute);

            var identifyResponse = RequestHelper.PostIdentification<V8.Identification_Result>(identification, uri);

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
            var uri = new Uri($"{_root.FindByRelationName("SEARCH").Uri}/{Uri.EscapeUriString(search)}", UriKind.Absolute);

            if (search.Length < MinimumSearchLength)
            {
                var emptyResult = new SearchDetailsResult {PersonDetails = new List<SearchDetails>()};

                var taskSource = new TaskCompletionSource<ISearchDetailsResult>();
                taskSource.SetResult(emptyResult);
                return await taskSource.Task.ConfigureAwait(false);
            }

            var searchDetailsResultDataTransferObject = await RequestHelper.Get<V8.Recipients>(uri).ConfigureAwait(false);

            var searchDetailsResult = DataTransferObjectConverter.FromDataTransferObject(searchDetailsResultDataTransferObject);

            _logger.LogDebug($"Response received for search with term '{search}' retrieved.");

            return searchDetailsResult;
        }
    }
}
