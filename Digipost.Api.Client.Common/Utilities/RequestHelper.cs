using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Handlers;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Domain.Utilities;

namespace Digipost.Api.Client.Common.Utilities
{
    internal class RequestHelper
    {
        private readonly X509Certificate2 _businessCertificate;
        private readonly ClientConfig _clientConfig;

        internal RequestHelper(ClientConfig clientConfig, X509Certificate2 businessCertificate)
        {
            _clientConfig = clientConfig;
            _businessCertificate = businessCertificate;
            HttpClient = GetHttpClient();
        }

        internal HttpClient HttpClient { get; set; }

        private HttpClient GetHttpClient()
        {
            var loggingHandler = new LoggingHandler(
                new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate},
                _clientConfig
            );

            var authenticationHandler = new AuthenticationHandler(_clientConfig, _businessCertificate, loggingHandler);

            var httpClient = new HttpClient(authenticationHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(_clientConfig.TimeoutMilliseconds),
                BaseAddress = new Uri(_clientConfig.Environment.Url.AbsoluteUri)
            };

            return httpClient;
        }

        internal Task<T> PostMessage<T>(IMessage message, Uri uri)
        {
            var messageAction = new MessageAction(message);
            var httpContent = messageAction.Content(message);

            return NewPost<T>(httpContent, messageAction.RequestContent, uri);
        }

        internal Task<T> PostIdentification<T>(IIdentification identification, Uri uri)
        {
            var messageAction = new IdentificationAction(identification, _clientConfig, _businessCertificate);
            var httpContent = messageAction.Content(identification);

            return NewPost<T>(httpContent, messageAction.RequestContent, uri);
        }

        internal Task<T> NewPost<T>(HttpContent httpContent, XmlDocument messageActionRequestContent, Uri uri)
        {
            ValidateXml(messageActionRequestContent);

            var postAsync = HttpClient.PostAsync(uri, httpContent);

            return Send<T>(postAsync);
        }

        internal Task<T> Get<T>(Uri uri)
        {
            return Send<T>(HttpClient.GetAsync(uri));
        }

        internal Task<string> Delete(Uri uri)
        {
            return Send<string>(HttpClient.DeleteAsync(uri));
        }

        internal async Task<Stream> GetStream(Uri uri)
        {
            var responseTask = HttpClient.GetAsync(uri);
            var httpResponseMessage = await responseTask.ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var responseContent = await ReadResponse(httpResponseMessage).ConfigureAwait(false);
                HandleResponseErrorAndThrow(responseContent, httpResponseMessage.StatusCode);
            }

            return await httpResponseMessage.Content.ReadAsStreamAsync();
        }

        private static async Task<T> Send<T>(Task<HttpResponseMessage> responseTask)
        {
            var httpResponseMessage = await responseTask.ConfigureAwait(false);

            var responseContent = await ReadResponse(httpResponseMessage).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
                HandleResponseErrorAndThrow(responseContent, httpResponseMessage.StatusCode);

            return HandleSuccessResponse<T>(responseContent);
        }

        private static void HandleResponseErrorAndThrow(string responseContent, HttpStatusCode statusCode)
        {
            var emptyResponse = string.IsNullOrEmpty(responseContent);

            if (!emptyResponse)
                ThrowNotEmptyResponseError(responseContent);
            else
            {
                ThrowEmptyResponseError(statusCode);
            }
        }

        private static void ValidateXml(XmlDocument document)
        {
            if (document.InnerXml.Length == 0)
            {
                return;
            }

            var xmlValidator = new ApiClientXmlValidator();
            string validationMessages;
            var isValidXml = xmlValidator.Validate(document.InnerXml, out validationMessages);

            if (!isValidXml)
            {
                throw new XmlException($"Xml was invalid. Stopped sending message. Feilmelding: '{validationMessages}'");
            }
        }

        private static async Task<string> ReadResponse(HttpResponseMessage requestResult)
        {
            var contentResult = await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            return contentResult;
        }

        private static void ThrowNotEmptyResponseError(string responseContent)
        {
            var errorDataTransferObject = SerializeUtil.Deserialize<error>(responseContent);
            var error = DataTransferObjectConverter.FromDataTransferObject(errorDataTransferObject);

            throw new ClientResponseException("Error occured, check inner Error object for more information.", error);
        }

        private static void ThrowEmptyResponseError(HttpStatusCode httpStatusCode)
        {
            throw new ClientResponseException((int) httpStatusCode + ": " + httpStatusCode);
        }

        private static T HandleSuccessResponse<T>(string responseContent)
        {
            return SerializeUtil.Deserialize<T>(responseContent);
        }
    }
}