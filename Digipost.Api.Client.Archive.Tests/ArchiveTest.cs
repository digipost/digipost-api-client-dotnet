using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Entrypoint;
using Xunit;

namespace Digipost.Api.Client.Archive.Tests
{
    public class ArchiveTest
    {
        [Fact]
        public void BuildCorrectNextDocumentsUri()
        {
            var archive = new Archive(new Sender(1010))
            {
                Links = new Dictionary<string, Link>
                {
                    ["NEXT_DOCUMENTS"] = new Link("https://www.testing.no/1010/archive/1000/document?limit=100&offset=0")
                    {
                        Rel = "https://www.testing.no/relation/next_document"
                    }
                }
            };

            Assert.True(archive.HasMoreDocuments());
            Assert.Equal("https://www.testing.no/1010/archive/1000/document?limit=100&offset=0", archive.GetNextDocumentsUri().ToString());

            var searchBy = new Dictionary<string, string>
            {
                ["key"] = "val"
            };
            Assert.Equal("https://www.testing.no/1010/archive/1000/document?limit=100&offset=0&attributes=a2V5LHZhbA==", archive.GetNextDocumentsUri(searchBy).ToString());
        }
    }
}
