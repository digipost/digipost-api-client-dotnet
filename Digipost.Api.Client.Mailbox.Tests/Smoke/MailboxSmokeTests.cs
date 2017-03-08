using Digipost.Api.Client.Test.Utilities;
using Xunit;

namespace Digipost.Api.Client.Mailbox.Tests.Smoke
{
    public class MailboxSmokeTests
    {
        private MailboxSmokeTestsHelper _t;

        public MailboxSmokeTests()
        {
            _t = new MailboxSmokeTestsHelper(SenderUtility.GetSender(TestEnvironment.Qa));
        }

        [Fact]
        public void Get_inbox_and_read_document()
        {
            _t
                .Get_inbox()
                .Expect_inbox_to_have_documents()
                .Fetch_document_data();
        }
    }
}
