﻿using System;
using System.CodeDom;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Api.Client.Action;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Autocomplete;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.XmlValidation;

namespace Digipost.Api.Client.Api
{
    internal class DigipostApi : IDigipostApi
    {
        private IDigipostActionFactory _digipostActionFactory;

        private ClientConfig ClientConfig { get; set; }
        
        private X509Certificate2 BusinessCertificate { get; set; }

        public DigipostApi(ClientConfig clientConfig, X509Certificate2 businessCertificate)
        {
            ClientConfig = clientConfig;
            BusinessCertificate = businessCertificate;
        }

        public DigipostApi(ClientConfig clientConfig, string thumbprint)
        {
            ClientConfig = clientConfig;
            BusinessCertificate = CertificateUtility.SenderCertificate(thumbprint, Language.English);
        }

        public IDigipostActionFactory DigipostActionFactory
        {
            get { return _digipostActionFactory ?? (_digipostActionFactory = new DigipostActionFactory()); }
            set { _digipostActionFactory = value; }
        }

        public MessageDeliveryResult SendMessage(Message message)
        {
            var messageDelivery = SendMessageAsync(message);

            if (messageDelivery.IsFaulted && messageDelivery.Exception != null) 
                    throw messageDelivery.Exception.InnerException;
            
            return messageDelivery.Result;
        }

        public async Task<MessageDeliveryResult> SendMessageAsync(Message message)
        {
            const string uri = "messages";
            var result = await GenericPostAsync<MessageDeliveryResult>(message, uri);

            return result;
        }

        public IdentificationResult Identify(Identification identification)
        {
            var identifyResponse = IdentifyAsync(identification);

            if (identifyResponse.IsFaulted)
            {
                if (identifyResponse.Exception != null) throw identifyResponse.Exception.InnerException;
            }
            return identifyResponse.Result;
        }

        public Task<IdentificationResult> IdentifyAsync(Identification identification)
        {
            const string uri = "identification";
            return GenericPostAsync<IdentificationResult>(identification, uri);
        }

        public AutocompleteResult Autocomplete(string search)
        {
            var uri = string.Format("recipients/suggest/{0}", Uri.EscapeUriString(search));
            return GenericGetAsync<AutocompleteResult>(uri).Result;
        }

        public IdentificationResult Person(Link link)
        {
            var ret = GenericGetAsync<AutocompleteResult>(link.SubUri).Result;

            return GenericGetAsync<IdentificationResult>(link.SubUri).Result;
        }

        private Task<T> GenericPostAsync<T>(RequestContent content, string uri)
        {
            var action = DigipostActionFactory.CreateClass(content, ClientConfig, BusinessCertificate, uri);

            ValidateXml(action.RequestContent);

            var responseTask = action.PostAsync(content);
            return GenericSendAsync<T>(responseTask);
        }

        private Task<T> GenericGetAsync<T>(string uri)
        {
            var action = DigipostActionFactory.CreateClass(ClientConfig, BusinessCertificate, uri);
            
            Task<HttpResponseMessage> responseTask = action.GetAsync();
            
            
            //var v = responseTask.Content.ReadAsStringAsync();
            //Logging.Log(TraceEventType.Information, v.Result);

            //return null;
            return GenericSendAsync<T>(responseTask);
        }

        private async Task<T> GenericSendAsync<T>(Task<HttpResponseMessage> responseTask)
        {
            var responseTaskResult = responseTask.Result;
            var responseContent = await ReadResponse(responseTaskResult);

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
            if (document.InnerXml.Length == 0) { return; }

            var xmlValidator = new ApiClientXmlValidator();
            var isValidXml = xmlValidator.ValiderDokumentMotXsd(document.InnerXml);

            if (!isValidXml)
            {
                throw new XmlException("Xml was invalid. Stopped sending message. Feilmelding:" + xmlValidator.ValideringsVarsler);
            }
        }

        private static async Task<string> ReadResponse(HttpResponseMessage requestResult)
        {
            var contentResult = await requestResult.Content.ReadAsStringAsync();
            return contentResult;
        }

        private static void ThrowNotEmptyResponseError(string responseContent)
        {
            var error = SerializeUtil.Deserialize<Error>(responseContent);
            throw new ClientResponseException("Error occured, check inner Error object for more information.", error);
        }

        private static void ThrowEmptyResponseError(HttpStatusCode httpStatusCode)
        {
            throw new ClientResponseException((int)httpStatusCode + ": " + httpStatusCode);
        }

        private static T HandleSuccessResponse<T>(string responseContent)
        {
            return SerializeUtil.Deserialize<T>(responseContent);
        }

       
    }
}