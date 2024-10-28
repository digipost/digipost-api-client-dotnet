using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Utilities;
using Xunit;

namespace Digipost.Api.Client.Common.Tests.Utilities
{
    public class ApiRelationsTests
    {
        private static readonly Uri BaseUri = new Uri("https://api.io/api/archive/search?id=123");

        [Fact]
        public void should_produce_archive_search_url_query()
        {
            Assert.Equal("https://api.io:443/api/archive/search?id=123",
                ArchiveNextDocumentsUri.ToUri(BaseUri, new Dictionary<string, string>(), null, null));

            Assert.Equal("https://api.io:443/api/archive/search?id=123&attributes=aWQsMTIz",
                ArchiveNextDocumentsUri.ToUri(BaseUri, new Dictionary<string, string>()
                    {
                        ["id"] = "123",
                    },
                    null, null
                ));

            var url = ArchiveNextDocumentsUri.ToUri(BaseUri, new Dictionary<string, string>(),
                DateTime.Parse("2024-10-26 14:50:14Z"),
                DateTime.Parse("2024-10-28 14:50:14Z")
            );
            Assert.Contains("fromDate=2024-10-26T", url);
            Assert.Contains("toDate=2024-10-28T", url);
        }
    }
}
