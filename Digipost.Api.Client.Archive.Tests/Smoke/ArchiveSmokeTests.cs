using Digipost.Api.Client.Tests.Utilities;
using Xunit;

namespace Digipost.Api.Client.Archive.Tests.Smoke
{
    public class InboxSmokeTests
    {
        public InboxSmokeTests()
        {
            _t = new ArchiveSmokeTestsHelper(SenderUtility.GetSender(TestEnvironment.Qa));
        }

        private readonly ArchiveSmokeTestsHelper _t;

        [Fact(Skip = "Smoketest")]
        public void Get_inbox_and_read_document()
        {
            _t.ArchiveAFile()
                .Get_archives();
        }
    }
}
