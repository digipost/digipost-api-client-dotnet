using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Api.Client.Common.Exceptions;
using Digipost.Api.Client.Scripts.Xsd.XsdToCode.Code;

namespace Digipost.Api.Client.Common.Utilities
{
    internal class RequestHelper
    {
        internal RequestHelper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        internal HttpClient HttpClient { get; set; }

        internal Task<T> Post<T>(HttpContent httpContent, XmlDocument messageActionRequestContent, Uri uri)
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