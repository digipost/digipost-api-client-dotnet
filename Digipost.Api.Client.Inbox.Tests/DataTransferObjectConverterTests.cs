using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Tests.CompareObjects;
using V8 = Digipost.Api.Client.Common.Generated.V8;
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
                var firstAccessed = DateTime.Today.AddDays(2);
                var deliveryTime = DateTime.Today;
                const int id = 123456789;
                const string sender = "sender";

                var source = new V8.InboxDocument()
                {
                    AuthenticationLevel = V8.AuthenticationLevel.Password,
                    ContentType = contentType,
                    DeliveryTime = deliveryTime,
                    FirstAccessed = firstAccessed,
                    FirstAccessedSpecified = true,
                    Id = id,
                    Sender = sender
                };

                var expected = new InboxDocument
                {
                    AuthenticationLevel = AuthenticationLevel.Password,
                    ContentType = contentType,
                    DeliveryTime = deliveryTime,
                    FirstAccessed = firstAccessed,
                    Id = id,
                    Sender = sender
                };

                var actual = source.FromDataTransferObject();

                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void InboxWithEmptyListOnNullResult()
            {
                var source = new V8.Inbox();
                var expected = new List<InboxDocument>();

                var actual = source.FromDataTransferObject();

                Comparator.AssertEqual(expected, actual);
            }
        }
    }
}
