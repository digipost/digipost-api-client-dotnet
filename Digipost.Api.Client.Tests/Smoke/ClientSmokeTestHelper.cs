using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Print;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Search;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Send;
using Digipost.Api.Client.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Digipost.Api.Client.Tests.Smoke
{
    internal class ClientSmokeTestHelper
    {
        private readonly List<IDocument> _attachments = new List<IDocument>();
        private readonly DigipostClient _digipostClient;
        private readonly TestSender _testSender;

        private readonly Root _root;

        //Gradually built state, identification
        private Identification _identification;

        private IIdentificationResult _identificationResult;
        private IMessageDeliveryResult _messageDeliveryResult;

        //Gradually built state, message
        private IDocument _primary;

        private DigipostRecipient _recipient;

        //Gradually built state, search
        private ISearchDetailsResult _searchResult;

        //Gradually built state, printMessage
        private IPrintDetails _printDetails;

        //Gradually built state, requestForRegistration
        private RequestForRegistration _requestForRegistration;

        private DocumentStatus _documentStatus;

        public ClientSmokeTestHelper(TestSender testSender, bool withoutDataTypesProject = false)
        {
            var broker = new Broker(testSender.Id);
            _testSender = testSender;

            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();

            _digipostClient = new DigipostClient(new ClientConfig(broker, testSender.Environment) {TimeoutMilliseconds = 900000000, LogRequestAndResponse = true, SkipMetaDataValidation = withoutDataTypesProject}, testSender.Certificate, serviceProvider.GetService<ILoggerFactory>());

            _root = _digipostClient.GetRoot(new ApiRootUri());
        }

        public ClientSmokeTestHelper HasRoot()
        {
            Assert_state(_root);
            return this;
        }

        public ClientSmokeTestHelper Create_message_with_primary_document()
        {
            _primary = DomainUtility.GetDocument();
            return this;
        }

        public ClientSmokeTestHelper CreateMessageWithPrimaryDataTypeDocument(string dataType)
        {
            _primary = DomainUtility.GetDocument(dataType);
            return this;
        }

        public ClientSmokeTestHelper Add_Attachments(params IDocument[] attachments)
        {
            _attachments.AddRange(attachments);

            return this;
        }

        public ClientSmokeTestHelper To_Digital_Recipient()
        {
            Assert_state(_primary);

            _recipient = _testSender.Recipient;

            return this;
        }

        public ClientSmokeTestHelper To_Physical_Recipient()
        {
            Assert_state(_primary);

            _recipient = _testSender.Recipient;

            return this;
        }

        public ClientSmokeTestHelper SendMessage()
        {
            Assert_state(_recipient);

            _messageDeliveryResult = _digipostClient.SendMessage(
                new Message(new Sender(_testSender.Id), _recipient, _primary)
                {
                    Attachments = _attachments
                });

            return this;
        }

        public ClientSmokeTestHelper SendPrintMessage()
        {
            Assert_state(_printDetails);

            _messageDeliveryResult = _digipostClient.SendMessage(
                new PrintMessage(new Sender(_testSender.Id), _printDetails, _primary)
                {
                    Attachments = _attachments
                });

            return this;
        }
        public ClientSmokeTestHelper SendRequestForRegistration(String personalIdentificationNumber)
        {
            Assert_state(_requestForRegistration);

            _messageDeliveryResult = _digipostClient.SendMessage(
                new Message(new Sender(_testSender.Id),
                    new RecipientById(IdentificationType.PersonalIdentificationNumber, personalIdentificationNumber),
                    _primary
                )
                {
                    RequestForRegistration = _requestForRegistration
                });

            return this;
        }

        public void Expect_message_to_have_status(MessageStatus messageStatus)
        {
            Assert_state(_messageDeliveryResult);

            Assert.Equal(_messageDeliveryResult.Status, messageStatus);
        }

        public void Expect_message_to_have_method(DeliveryMethod deliveryMethod)
        {
            Assert_state(_messageDeliveryResult);

            Assert.Equal(_messageDeliveryResult.DeliveryMethod, deliveryMethod);
        }


        private static void Assert_state(object obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Requires gradually built state. Make sure you use functions in the correct order.");
            }
        }

        public ClientSmokeTestHelper Create_identification_request()
        {
            _identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "01013300001"));

            return this;
        }

        public ClientSmokeTestHelper SendIdentification()
        {
            Assert_state(_identification);

            _identificationResult = _digipostClient.Identify(_identification);

            return this;
        }

        public void Expect_identification_to_have_status(IdentificationResultType identificationResultType)
        {
            Assert_state(_identificationResult);

            Assert.Equal(identificationResultType, _identificationResult.ResultType);
        }

        public ClientSmokeTestHelper Create_search_request()
        {
            _searchResult = _digipostClient.Search("Børre");

            return this;
        }

        public ClientSmokeTestHelper ToPrintDirectly()
        {
            _printDetails = new PrintDetails(
                printRecipient: new PrintRecipient(
                    "Ola Nordmann",
                    new NorwegianAddress("0460", "Oslo", "Prinsensveien 123")),
                printReturnRecipient: new PrintReturnRecipient(
                    "Kari Nordmann",
                    new NorwegianAddress("0400", "Oslo", "Akers Àle 2"))
            );
            return this;
        }

        public ClientSmokeTestHelper RequestForRegistration(string phonenumber)
        {
            ToPrintDirectly();
            _requestForRegistration = new RequestForRegistration(DateTime.Now.AddDays(6), phonenumber, null, _printDetails);
            return this;
        }

        public void Expect_search_to_have_result()
        {
            Assert_state(_searchResult);

            Assert.InRange(_searchResult.PersonDetails.ToList().Count, 1, 11);
        }

        public ClientSmokeTestHelper FetchDocumentStatus()
        {
            Assert_state(_messageDeliveryResult);
            Assert_state(_primary);

            _documentStatus = _digipostClient.GetDocumentStatus(new Sender(_testSender.Id)).GetDocumentStatus(Guid.Parse(_primary.Guid)).Result;
            return this;
        }
        public ClientSmokeTestHelper FetchDocumentStatus(Guid guid)
        {
            _documentStatus = _digipostClient.GetDocumentStatus(new Sender(_testSender.Id)).GetDocumentStatus(guid).Result;
            return this;
        }

        public void Expect_document_status_to_be(DocumentStatus.DocumentDeliveryStatus deliveryStatus, DeliveryMethod deliveryMethod)
        {
            Assert_state(_documentStatus);
            Assert.Equal(_documentStatus.DeliveryMethod, deliveryMethod);
            Assert.Equal(_documentStatus.DeliveryStatus, deliveryStatus);
        }
    }
}
