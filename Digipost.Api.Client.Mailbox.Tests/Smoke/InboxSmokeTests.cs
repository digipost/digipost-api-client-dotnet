using Digipost.Api.Client.Test.Utilities;
using Xunit;

namespace Digipost.Api.Client.Inbox.Tests.Smoke
{
    public class InboxSmokeTests
    {
        private readonly InboxSmokeTestsHelper _t;

        public InboxSmokeTests()
        {
            _t = new InboxSmokeTestsHelper(SenderUtility.GetSender(TestEnvironment.Qa));
        }

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
