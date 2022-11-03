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

        [Fact(Skip = "SmokeTest")]
        public void Get_inbox_and_read_document()
        {
            _t.ArchiveAFile(ArchiveSmokeTestsHelper.ArchiveName)
                .Get_Archive(ArchiveSmokeTestsHelper.ArchiveName)
                .Get_All_Documents()
                .Get_Documents()
                .Delete_All_Documents();
        }

        [Fact(Skip = "SmokeTest")]
        public void ArchiveADocument()
        {
            _t.ArchiveAFile();
        }
    }
}
