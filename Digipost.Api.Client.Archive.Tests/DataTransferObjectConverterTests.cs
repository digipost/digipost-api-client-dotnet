using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Tests.CompareObjects;
using V8 = Digipost.Api.Client.Common.Generated.V8;
using Xunit;

namespace Digipost.Api.Client.Archive.Tests
{
    public class DataTransferObjectConverterTests
    {
        public class FromDataTransferObject : DataTransferObjectConverterTests
        {
            private const string BaseUri = "https://contenturi.no";

            [Fact]
            public void Archive()
            {
                var source = new V8.Archive()
                {
                    Name = "per",
                    SenderId = 1010,
                };

                var expected = new Archive(new Sender(1010), "per");

                var actual = source.FromDataTransferObject();

                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void ArchiveDocumentBasicState()
            {
                var newGuid = Guid.NewGuid();
                var source = new V8.ArchiveDocument()
                {
                    Uuid = newGuid.ToString(),
                    FileName = "per.txt",
                    FileType = "txt",
                    ContentType = "text/plain",
                    Attributes = { },
                    Link = {new V8.Link() {Rel = $"{BaseUri}/relations/get_something", Uri = $"{BaseUri}/something", MediaType = "text/plain"}}
                };

                var expected = new ArchiveDocument(newGuid, "per.txt", "txt", "text/plain");
                expected.Links.Add("get_something", new Common.Entrypoint.Link($"{BaseUri}/something") {MediaType = "text/plain", Rel = $"{BaseUri}/relations/get_something"});

                var actual = source.FromDataTransferObject();

                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void ArchiveDocumentToDto()
            {
                var newGuid = Guid.NewGuid();
                var source = new ArchiveDocument(newGuid, "per.txt", "txt", "text/plain").WithAttribute("test", "val");

                var expected = new V8.ArchiveDocument()
                {
                    Uuid = newGuid.ToString(),
                    FileName = "per.txt",
                    FileType = "txt",
                    ContentType = "text/plain",
                    Attributes = { new V8.ArchiveDocumentAttribute(){Key = "test", Value = "val"} },
                };

                Comparator.AssertEqual(expected, source.ToDataTransferObject());
            }
        }
    }
}
