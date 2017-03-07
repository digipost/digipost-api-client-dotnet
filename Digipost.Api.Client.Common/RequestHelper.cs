using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Utilities;
using Digipost.Api.Client.XmlValidation;

namespace Digipost.Api.Client.Api
{
    public class RequestHelper
    {
        private readonly X509Certificate2 _businessCertificate;
        private readonly ClientConfig _clientConfig;
        private IDigipostActionFactory _digipostActionFactory;

        public RequestHelper(ClientConfig clientConfig, X509Certificate2 businessCertificate)
        {
            _clientConfig = clientConfig;
            _businessCertificate = businessCertificate;
        }

        internal IDigipostActionFactory DigipostActionFactory
        {
            get { return _digipostActionFactory ?? (_digipostActionFactory = new DigipostActionFactory()); }
            set { _digipostActionFactory = value; }
        }

        internal Task<T> GenericPostAsync<T>(IRequestContent content, Uri uri)
        {
            var action = DigipostActionFactory.CreateClass(content, _clientConfig, _businessCertificate, uri);

            ValidateXml(action.RequestContent);

            var responseTask = action.PostAsync(content);
            return GenericSendAsync<T>(responseTask);
        }

        internal Task<T> GenericGetAsync<T>(Uri uri)
        {
            var action = DigipostActionFactory.CreateClass(_clientConfig, _businessCertificate, uri);
            var responseTask = action.GetAsync();

            return GenericSendAsync<T>(responseTask);
        }

        public async Task<T> GenericSendAsync<T>(Task<HttpResponseMessage> responseTask)
        {
            var responseTaskResult = await responseTask.ConfigureAwait(false);

            var responseContent = await ReadResponse(responseTaskResult).ConfigureAwait(false);

            if (!responseTaskResult.IsSuccessStatusCode)
            {
                var emptyResponse = string.IsNullOrEmpty(responseContent);

                if (!emptyResponse)
                    ThrowNotEmptyResponseError(responseContent);
                else
                {
                    ThrowEmptyResponseError(responseTaskResult.StatusCode);
                }
            }
            return HandleSuccessResponse<T>(responseContent);
        }

        internal static void ValidateXml(XmlDocument document)
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