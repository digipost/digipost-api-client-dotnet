﻿using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Test.Utilities;
using Xunit;

namespace Digipost.Api.Client.Test.Smoke
{
    public class ClientSmokeTests
    {
        private static TestHelper _t;

        public ClientSmokeTests()
        {
            var sender = SenderUtility.GetSender(Environment.Qa);
            _t = new TestHelper(sender);
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
    }
}