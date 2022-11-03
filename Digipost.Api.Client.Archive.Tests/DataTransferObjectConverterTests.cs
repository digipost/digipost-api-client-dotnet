using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Tests.CompareObjects;
using V8;
using Xunit;
using Link = Digipost.Api.Client.Common.Entrypoint.Link;

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
                    Sender_Id = 1010,
                };

                var expected = new Archive("per", new Sender(1010));

                var actual = ArchiveDataTransferObjectConverter.FromDataTransferObject(source);

                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void ArchiveDocumentBasicState()
            {
                var newGuid = Guid.NewGuid();
                var source = new V8.Archive_Document()
                {
                    Uuid = newGuid.ToString(),
                    File_Name = "per.txt",
                    File_Type = "txt",
                    Content_Type = "text/plain",
                    Attributes = { },
                    Link = {new V8.Link() {Rel = $"{BaseUri}/relations/get_something", Uri = $"{BaseUri}/something", Media_Type = "text/plain"}}
                };

                var expected = new ArchiveDocument(newGuid, "per.txt", "txt", "text/plain");
                expected.Links.Add("get_something", new Link($"{BaseUri}/something") {MediaType = "text/plain", Rel = $"{BaseUri}/relations/get_something"});

                var actual = ArchiveDataTransferObjectConverter.FromDataTransferObject(source);

                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void ArchiveDocumentToDto()
            {
                var newGuid = Guid.NewGuid();
                var source = new ArchiveDocument(newGuid, "per.txt", "txt", "text/plain").WithAttribute("test", "val");

                var expected = new V8.Archive_Document()
                {
                    Uuid = newGuid.ToString(),
                    File_Name = "per.txt",
                    File_Type = "txt",
                    Content_Type = "text/plain",
                    Attributes = { new Archive_Document_Attribute(){Key = "test", Value = "val"} },
                };

                Comparator.AssertEqual(expected, ArchiveDataTransferObjectConverter.ToDataTransferObject(source));
            }
        }
    }
}
