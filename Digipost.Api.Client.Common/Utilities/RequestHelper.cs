using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Api.Client.Common.Actions;
using Digipost.Api.Client.Common.Handlers;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Utilities;

namespace Digipost.Api.Client.Common.Utilities
{
    internal class RequestHelper
    {
        private readonly X509Certificate2 _businessCertificate;
        private readonly ClientConfig _clientConfig;
        private IDigipostActionFactory _digipostActionFactory;

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
                new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate },
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

        internal Task<HttpResponseMessage> PostAsync(IRequestContent requestContent, Uri uri)
        {
            return null;
            //return HttpClient.PostAsync(uri, Content(requestContent));
        }

        internal Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return HttpClient.GetAsync(uri); //Todo: ToString()?
        }

        internal Task<HttpResponseMessage> DeleteAsync(Uri uri)
        {
            return HttpClient.DeleteAsync(uri); //Todo: ToString()?
        }

        internal IDigipostActionFactory DigipostActionFactory
        {
            get { return _digipostActionFactory ?? (_digipostActionFactory = new DigipostActionFactory()); }
            set { _digipostActionFactory = value; }
        }

        internal Task<T> Post<T>(IRequestContent content, Uri uri)
        {
            var action = DigipostActionFactory.CreateClass(content, _clientConfig, _businessCertificate, uri);

            ValidateXml(action.RequestContent);

            var responseTask = action.PostAsync(content);
            return Send<T>(responseTask);
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