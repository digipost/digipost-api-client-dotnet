using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Tests.CompareObjects;
using V8;
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

                var source = new V8.Inbox_Document()
                {
                    Authentication_Level = Authentication_Level.PASSWORD,
                    Content_Type = contentType,
                    Content_Uri = content,
                    Delete_Uri = deleteUri,
                    Delivery_Time = deliveryTime,
                    First_Accessed = firstAccessed,
                    First_AccessedSpecified = true,
                    Id = id,
                    Sender = sender
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

                Comparator.AssertEqual(expected, actual);
            }

            [Fact]
            public void InboxWithEmptyListOnNullResult()
            {
                var source = new V8.Inbox();
                var expected = new List<InboxDocument>();

                var actual = InboxDataTransferObjectConverter.FromDataTransferObject(source);

                Comparator.AssertEqual(expected, actual);
            }
        }
    }
}
