using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Scripts.Xsd.XsdToCode.Code;
using Digipost.Api.Client.Tests.CompareObjects;
using Xunit;

namespace Digipost.Api.Client.Inbox.Tests
{
    public class DataTransferObjectConverterTests
    {
        public class FromDataTransferObject : DataTransferObjectConverterTests
        {
            [Fact]
            public void InboxDocument()
            {
                const string contentType = "txt";
                const string content = "http://contenturi.no";
                const string deleteUri = "http://deletecontenturi.no";
                var firstAccessed = DateTime.Today.AddDays(2);
                var deliveryTime = DateTime.Today;
                const int id = 123456789;
                const string sender = "sender";

                var source = new inboxdocument
                {
                    attachment = new inboxdocument[0],
                    authenticationlevel = authenticationlevel.PASSWORD,
                    contenttype = contentType,
                    contenturi = content,
                    deleteuri = deleteUri,
                    deliverytime = deliveryTime,
                    firstaccessed = firstAccessed,
                    firstaccessedSpecified = true,
                    id = id,
                    sender = sender
                };

                var expected = new InboxDocument
                {
                    AuthenticationLevel = AuthenticationLevel.Password,
                    ContentType = contentType,
                    Content = new Uri(content),
                    Delete = new Uri(deleteUri),
                    DeliveryTime = deliveryTime,
                    FirstAccessed = firstAccessed,
                    Id = id,
                    Sender = sender
                };

                var actual = InboxDataTransferObjectConverter.FromDataTransferObject(source);

                IEnumerable<IDifference> differences;
                var comparator = new Comparator {ComparisonConfiguration = new ComparisonConfiguration {IgnoreObjectTypes = true}};
                comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void InboxWithEmptyListOnNullResult()
            {
                var source = new inbox();
                var expected = new List<InboxDocument>();

                var actual = InboxDataTransferObjectConverter.FromDataTransferObject(source);

                IEnumerable<IDifference> differences;
                var comparator = new Comparator {ComparisonConfiguration = new ComparisonConfiguration {IgnoreObjectTypes = true}};
                comparator.Equal(expected, actual, out differences);
                Assert.Equal(0, differences.Count());
            }
        }
    }
}