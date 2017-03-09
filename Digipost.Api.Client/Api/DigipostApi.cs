using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiClientShared;
using ApiClientShared.Enums;
using Common.Logging;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.Extensions;

namespace Digipost.Api.Client.Api
{
    internal class DigipostApi : IDigipostApi
    {
        private const int MinimumSearchLength = 3;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RequestHelper _requestHelper;

        public DigipostApi(ClientConfig clientConfig, X509Certificate2 businessCertificate, RequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
            ClientConfig = clientConfig;
            BusinessCertificate = businessCertificate;
        }

        public DigipostApi(ClientConfig clientConfig, string thumbprint)
        {
            ClientConfig = clientConfig;
            BusinessCertificate = CertificateUtility.SenderCertificate(thumbprint, Language.English);
        }

        private ClientConfig ClientConfig { get; }

        private X509Certificate2 BusinessCertificate { get; }

        [Obsolete("Not in use, set factory on RequestHelper instead.")]
        public IDigipostActionFactory DigipostActionFactory
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IMessageDeliveryResult SendMessage(IMessage message)
        {
            var messageDelivery = SendMessageAsync(message);

            if (messageDelivery.IsFaulted && (messageDelivery.Exception != null))
                throw messageDelivery.Exception.InnerException;

            return messageDelivery.Result;
        }

        public async Task<IMessageDeliveryResult> SendMessageAsync(IMessage message)
        {
            Log.Debug($"Outgoing Digipost message to Recipient: {message.DigipostRecipient}");

            var uri = new Uri("messages", UriKind.Relative);

            var messageDeliveryResultTask = _requestHelper.Post<messagedelivery>(message, uri);

            if (messageDeliveryResultTask.IsFaulted && (messageDeliveryResultTask.Exception != null))
                throw messageDeliveryResultTask.Exception.InnerException;

            var messageDeliveryResult = DataTransferObjectConverter.FromDataTransferObject(await messageDeliveryResultTask.ConfigureAwait(false));

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

            var identifyResponse = _requestHelper.Post<identificationresult>(identification, uri);

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

            var searchDetailsResultDataTransferObject = await _requestHelper.Get<recipients>(uri).ConfigureAwait(false);

            var searchDetailsResult = DataTransferObjectConverter.FromDataTransferObject(searchDetailsResultDataTransferObject);

            Log.Debug($"Response received for search with term '{search}' retrieved.");

            return searchDetailsResult;
        }
    }
}