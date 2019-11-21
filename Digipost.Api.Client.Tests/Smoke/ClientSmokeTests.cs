using System;
using DataTypes;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.DataTypes.Utils;
using Digipost.Api.Client.Tests.Utilities;
using Xunit;

namespace Digipost.Api.Client.Tests.Smoke
{
    public class ClientSmokeTests
    {
        public ClientSmokeTests()
        {
            var sender = SenderUtility.GetSender(TestEnvironment.Qa);
            _t = new ClientSmokeTestHelper(sender);
        }

        private static ClientSmokeTestHelper _t;
        
        [Fact]
        public void Can_identify_user()
        {
            _t
                .Create_identification_request()
                .SendIdentification()
                .Expect_identification_to_have_status(IdentificationResultType.DigipostAddress);
        }

        [Fact]
        public void Can_Search()
        {
            _t
                .Create_search_request()
                .Expect_search_to_have_result();
        }

        [Fact]
        public void Can_send_document_digipost_user()
        {
            _t
                .Create_message_with_primary_document()
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }
        
        [Fact]
        public void Can_send_datatype_document_digipost_user()
        {
            ExternalLink externalLink = new ExternalLink {Url = "https://www.test.no", Description = "This is a link"};
            string linkXml = SerializeUtil.Serialize(externalLink);
            
            _t 
                .CreateMessageWithPrimaryDataTypeDocument(linkXml)
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }
        
        [Fact]
        public void Can_send_generic_datatype_document_digipost_user()
        {
            dynamic test = DataTypeUtil.GetDatatypeObject("externalLink");
            test.url = "https://www.test.no";
            test.description = "This was a generic datatype";

            string linkXml = SerializeUtil.Serialize<DynamicDataType>(test);
            
            _t 
                .CreateMessageWithPrimaryDataTypeDocument(linkXml)
                .To_Digital_Recipient()
                .SendMessage()
                .Expect_message_to_have_status(MessageStatus.Delivered);
        }
    }
}
