using System.Collections.Generic;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Extensions;
using Digipost.Api.Client.Send;
using Microsoft.Extensions.Logging;
using V8 = Digipost.Api.Client.Common.Generated.V8;

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
            _logger.LogDebug("Outgoing Digipost message to Recipient: {message}", message);

            var messageDeliveryResultTask = RequestHelper.PostMessage<V8.MessageDelivery>(message, _root.GetSendMessageUri(), skipMetaDataValidation);

            if (messageDeliveryResultTask.IsFaulted && messageDeliveryResultTask.Exception != null)
                throw messageDeliveryResultTask.Exception?.InnerException;

            var messageDeliveryResult = (await messageDeliveryResultTask.ConfigureAwait(false)).FromDataTransferObject();

            _logger.LogDebug("Response received for message to recipient, {message}: '{status}'. Will be available to Recipient at {deliverytime}.", message, messageDeliveryResult.Status, messageDeliveryResult.DeliveryTime);

            return messageDeliveryResult;
        }

        public IIdentificationResult Identify(IIdentification identification)
        {
            return IdentifyAsync(identification).Result;
        }

        public async Task<IIdentificationResult> IdentifyAsync(IIdentification identification)
        {
            _logger.LogDebug("Outgoing identification request: {identification}", identification);

            var identifyResponse = RequestHelper.PostIdentification<V8.IdentificationResult>(identification, _root.GetIdentifyRecipientUri());

            if (identifyResponse.IsFaulted)
            {
                var exception = identifyResponse.Exception?.InnerException;

                _logger.LogWarning("Identification failed, {exception}", exception);

                if (identifyResponse.Exception != null)
                    throw identifyResponse.Exception.InnerException;
            }

            var identificationResultDataTransferObject = await identifyResponse.ConfigureAwait(false);
            var identificationResult = identificationResultDataTransferObject.FromDataTransferObject();

            _logger.LogDebug("Response received for identification to recipient, ResultType '{resultType}', Data '{data}'.", identificationResult.ResultType, identificationResult.Data);

            return identificationResult;
        }

        public ISearchDetailsResult Search(string search)
        {
            return SearchAsync(search).Result;
        }

        public async Task<ISearchDetailsResult> SearchAsync(string search)
        {
            _logger.LogDebug("Outgoing search request, term: '{search}'.", search);

            search = search.RemoveReservedUriCharacters();
            var uri = _root.GetRecipientSearchUri(search);

            if (search.Length < MinimumSearchLength)
            {
                var emptyResult = new SearchDetailsResult {PersonDetails = new List<SearchDetails>()};

                var taskSource = new TaskCompletionSource<ISearchDetailsResult>();
                taskSource.SetResult(emptyResult);
                return await taskSource.Task.ConfigureAwait(false);
            }

            var searchDetailsResultDataTransferObject = await RequestHelper.Get<V8.Recipients>(uri).ConfigureAwait(false);

            var searchDetailsResult = searchDetailsResultDataTransferObject.FromDataTransferObject();

            _logger.LogDebug("Response received for search with term '{search}' retrieved.", search);

            return searchDetailsResult;
        }

        public void SendAdditionalData(IAdditionalData additionalData, AddAdditionalDataUri uri)
        {
            SendAdditionalDataAsync(additionalData, uri).GetAwaiter().GetResult();
        }

        public async Task SendAdditionalDataAsync(IAdditionalData additionalData, AddAdditionalDataUri uri)
        {
            _logger.LogDebug("Sending additional data '{uri}'", uri);

            await RequestHelper.PostAdditionalData<string>(additionalData, uri).ConfigureAwait(false);

            _logger.LogDebug("Additional data added to '{uri}'", uri);
        }
    }
}
