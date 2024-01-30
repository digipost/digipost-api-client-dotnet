﻿using Digipost.Api.Client.Tests.Utilities;
using Xunit;
using Xunit.Sdk;

namespace Digipost.Api.Client.Inbox.Tests.Smoke
{
    public class InboxSmokeTests
    {
        private readonly InboxSmokeTestsHelper _t = new InboxSmokeTestsHelper(SenderUtility.GetSender(TestEnvironment.Qa));

        // To test this, log on to the account you are using and upload a document to the inbox.
        [Fact(Skip = "Skipping due to missing inbox for test users")]
        public void Get_inbox_and_read_document()
        {
            _t
                .Get_inbox()
                .Expect_inbox_to_have_documents()
                .Fetch_document_data()
                .Delete_document();
        }
    }
}
