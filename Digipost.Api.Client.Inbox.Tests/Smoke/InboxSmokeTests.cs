using Digipost.Api.Client.Tests.Utilities;
using Xunit;

namespace Digipost.Api.Client.Inbox.Tests.Smoke
{
    public class InboxSmokeTests
    {
        public InboxSmokeTests()
        {
            _t = new InboxSmokeTestsHelper(SenderUtility.GetSender(TestEnvironment.Qa));
        }

        private readonly InboxSmokeTestsHelper _t;

        [Fact]
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