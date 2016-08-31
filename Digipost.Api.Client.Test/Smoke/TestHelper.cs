using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using Digipost.Api.Client.Test.Utilities;
using Xunit;
using Environment = System.Environment;

namespace Digipost.Api.Client.Test.Smoke
{
    internal class TestHelper
    {
        private readonly List<IDocument> _attachments = new List<IDocument>();
        private readonly DigipostClient _digipostClient;

        //Gradually built state, identification
        private Identification _identification;
        private IIdentificationResult _identificationResult;
        private IMessageDeliveryResult _messageDeliveryResult;

        //Gradually built state, message
        private IDocument _primary;
        private DigipostRecipient _recipient;

        //Gradually built state, search
        private ISearchDetailsResult _searchResult;

        public TestHelper(Sender sender)
        {
            var actualSender = OverrideSenderIfOnBuildServer(sender);

            _digipostClient = new DigipostClient(new ClientConfig(actualSender.Id, actualSender.Environment), actualSender.Certificate);
        }

        private Sender OverrideSenderIfOnBuildServer(Sender sender)
        {
            const string buildServerUser = "administrator";
            var currentUser = Environment.UserName.ToLower();
            var isCurrentUserBuildServer = currentUser.Contains(buildServerUser);

            if (isCurrentUserBuildServer)
            {
                return SenderUtility.GetSender(Utilities.Environment.DifiTest);
            }

            return sender;
        }

        public TestHelper Create_message_with_primary_document()
        {
            _primary = DomainUtility.GetDocument();

            return this;
        }

        public TestHelper Create_message_with_primary_invoice()
        {
            _primary = DomainUtility.GetInvoice();

            return this;
        }

        public TestHelper Add_Attachments(params IDocument[] attachments)
        {
            _attachments.AddRange(attachments);

            return this;
        }

        public TestHelper To_Digital_Recipient()
        {
            Assert_state(_primary);

            _recipient = new RecipientById(IdentificationType.PersonalIdentificationNumber, "04036125433");

            return this;
        }

        public TestHelper SendMessage()
        {
            Assert_state(_recipient);

            _messageDeliveryResult = _digipostClient.SendMessage(
                new Message(_recipient, _primary)
                {
                    Attachments = _attachments
                });

            return this;
        }

        public void Expect_message_to_have_status(MessageStatus messageStatus)
        {
            Assert_state(_messageDeliveryResult);

            Assert.Equal(_messageDeliveryResult.Status, messageStatus);
        }

        private static void Assert_state(object obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Requires gradually built state. Make sure you use functions in the correct order.");
            }
        }

        public TestHelper Create_identification_request()
        {
            _identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "04036125433"));

            return this;
        }

        public TestHelper SendIdentification()
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

        public TestHelper Create_search_request()
        {
            _searchResult = _digipostClient.Search("Jarand");

            return this;
        }

        public void Expect_search_to_have_result()
        {
            Assert_state(_searchResult);

            Assert.InRange(_searchResult.PersonDetails.Count, 1, 11);
        }
    }
}