using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Api.Client.Common.Actions;
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
            var action = DigipostActionFactory.CreateClass(_clientConfig, _businessCertificate, uri);
            var responseTask = action.GetAsync();

            return Send<T>(responseTask);
        }

        internal Task<string> Delete(Uri uri)
        {
            var action = DigipostActionFactory.CreateClass(_clientConfig, _businessCertificate, uri);
            var responseTask = action.DeleteAsync();

            return Send<string>(responseTask);
        }

        internal async Task<Stream> GetStream(Uri uri)
        {
            var action = DigipostActionFactory.CreateClass(_clientConfig, _businessCertificate, uri);
            var responseTask = action.GetAsync();

            var documentStream = await (await responseTask.ConfigureAwait(false)).Content.ReadAsStreamAsync();

            return documentStream;
        }

        private static async Task<T> Send<T>(Task<HttpResponseMessage> responseTask)
        {
            var responseTaskResult = await responseTask.ConfigureAwait(false);

            var responseContent = await ReadResponse(responseTaskResult).ConfigureAwait(false);

            if (responseTaskResult.IsSuccessStatusCode)
                return HandleSuccessResponse<T>(responseContent);

            var emptyResponse = string.IsNullOrEmpty(responseContent);

            if (!emptyResponse)
                ThrowNotEmptyResponseError(responseContent);
            else
            {
                ThrowEmptyResponseError(responseTaskResult.StatusCode);
            }
            return HandleSuccessResponse<T>(responseContent);
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