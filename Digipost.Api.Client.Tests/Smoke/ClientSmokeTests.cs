using System;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.DataTypes.Core;
using Digipost.Api.Client.Send;
using Digipost.Api.Client.Tests.Utilities;
using Xunit;

namespace Digipost.Api.Client.Tests.Smoke
{
    public class ClientSmokeTests
    {
        private static ClientSmokeTestHelper _client;
        private static ClientSmokeTestHelper _clientWithoutDataTypes;

        public ClientSmokeTests()
        {
            var sender = SenderUtility.GetSender(TestEnvironment.Qa);
            _client = new ClientSmokeTestHelper(sender);
            _clientWithoutDataTypes = new ClientSmokeTestHelper(sender, true);
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_identify_user()
        {
            _client
                .HasRoot()
                .Create_identification_request()
                .SendIdentification()
                .Expect_identification_to_have_status(IdentificationResultType.DigipostAddress);
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_Search()
        {
            _client
                .Create_search_request()
                .Expect_search_to_have_result();
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_send_document_digipost_user()
        {
            _client
                .Create_message_with_primary_document()
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_send_direct_to_print()
        {
            _client
                .Create_message_with_primary_document()
                .ToPrintDirectly()
                .SendPrintMessage()
                .Expect_message_to_have_status(MessageStatus.DeliveredToPrint);
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_send_request_for_registration()
        {
            _client
                .Create_message_with_primary_document()
                .RequestForRegistration("+4711223344")
                // Find a user with Tenor testdata to test registration. This is TYKKHUDET EKTEMANN
                .SendRequestForRegistration("05926398190")
                .Expect_message_to_have_method(DeliveryMethod.PENDING);
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_send_document_with_raw_datatype_to_digipost_user()
        {
            const string raw = "<?xml version=\"1.0\" encoding=\"utf-8\"?><externalLink xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://api.digipost.no/schema/datatypes\"><url>https://www.test.no</url><description>This was raw string</description></externalLink>";

            _clientWithoutDataTypes
                .CreateMessageWithPrimaryDataTypeDocument(raw)
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_send_document_with_object_datatype_to_digipost_user()
        {

            var externalLink = new ExternalLink {Url = "https://www.test.no", Description = "This is a link"};
            var linkXml = SerializeUtil.Serialize(externalLink);

            _client
                .CreateMessageWithPrimaryDataTypeDocument(linkXml)
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }
    }
}
