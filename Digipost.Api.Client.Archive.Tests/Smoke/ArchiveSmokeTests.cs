using Digipost.Api.Client.Tests.Utilities;
using Xunit;

namespace Digipost.Api.Client.Archive.Tests.Smoke
{
    public class ArchiveSmokeTests
    {
        public ArchiveSmokeTests()
        {
            _t = new ArchiveSmokeTestsHelper(SenderUtility.GetSender(TestEnvironment.Qa));
        }

        private readonly ArchiveSmokeTestsHelper _t;

        [Fact(Skip = "SmokeTest")]
        public void ArchiveDocuments_read_and_delete()
        {
            _t.ArchiveAFile(ArchiveSmokeTestsHelper.ArchiveName)
                .Get_Archive(ArchiveSmokeTestsHelper.ArchiveName)
                .Get_All_Documents()
                .Get_Documents()
                .Delete_All_Documents();
        }

        [Fact(Skip = "SmokeTest")]
        public void ArchiveADocumentWithAttributes()
        {
            _t.ArchiveAFile()
                .Get_Default_Archive()
                .Get_All_DocumentsWithAttributes();
        }
    }
}
