using System;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.DataTypes.Core;
using Digipost.Api.Client.Tests.Utilities;
using Xunit;

namespace Digipost.Api.Client.Tests.Smoke
{
    public class ClientSmokeTests
    {
        private static ClientSmokeTestHelper client;
        private static ClientSmokeTestHelper clientWithoutDataTypes;
        
        public ClientSmokeTests()
        {
            var sender = SenderUtility.GetSender(TestEnvironment.Qa);
            client = new ClientSmokeTestHelper(sender);
            clientWithoutDataTypes = new ClientSmokeTestHelper(sender, true);
        }
        
        [Fact(Skip = "SmokeTest")]
        public void Can_identify_user()
        {
            client
                .Create_identification_request()
                .SendIdentification()
                .Expect_identification_to_have_status(IdentificationResultType.DigipostAddress);
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_Search()
        {
            client
                .Create_search_request()
                .Expect_search_to_have_result();
        }

        [Fact(Skip = "SmokeTest")]
        public void Can_send_document_digipost_user()
        {
            client
                .Create_message_with_primary_document()
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }
        
        [Fact(Skip = "SmokeTest")]
        public void Can_send_document_with_raw_datatype_to_digipost_user()
        {
            var raw = "<?xml version=\"1.0\" encoding=\"utf-8\"?><externalLink xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://api.digipost.no/schema/datatypes\"><url>https://www.test.no</url><description>This was raw string</description></externalLink>";
            
            clientWithoutDataTypes
                .CreateMessageWithPrimaryDataTypeDocument(raw)
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }
        
        [Fact(Skip = "SmokeTest")]
        public void Can_send_document_with_object_datatype_to_digipost_user()
        {
            
            ExternalLink externalLink = new ExternalLink {Url = "https://www.test.no", Description = "This is a link"};
            string linkXml = SerializeUtil.Serialize(externalLink);

            client 
                .CreateMessageWithPrimaryDataTypeDocument(linkXml)
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }
    }
}
