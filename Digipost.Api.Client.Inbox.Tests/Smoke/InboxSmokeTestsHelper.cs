using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Test.Utilities;
using Xunit;

namespace Digipost.Api.Client.Inbox.Tests.Smoke
{
    public class InboxSmokeTestsHelper
    {
        private readonly DigipostClient _client;
        private Inbox _inbox;
        private InboxDocument _inboxDocument;
        private IEnumerable<InboxDocument> _inboxDocuments;

        internal InboxSmokeTestsHelper(Sender sender)
        {
            _client = new DigipostClient(
                new ClientConfig(sender.Id, sender.Environment),
                sender.Certificate
            );
        }

        public InboxSmokeTestsHelper Get_inbox()
        {
            _inbox = _client.Inbox;
            _inboxDocuments = _inbox.Fetch().Result;

            return this;
        }

        public InboxSmokeTestsHelper Expect_inbox_to_have_documents()
        {
            Assert_state(_inboxDocuments);

            Assert.True(_inboxDocuments.Any());

            _inboxDocument = _inboxDocuments.First();

            return this;
        }

        public InboxSmokeTestsHelper Fetch_document_data()
        {
            Assert_state(_inboxDocument);

            var documentStream = _inbox.FetchDocument(_inboxDocument).Result;

            Assert.Equal(true, documentStream.CanRead);
            Assert.True(documentStream.Length > 500);

            return this;
        }

        public InboxSmokeTestsHelper Delete_document()
        {
            Assert_state(_inboxDocument);

            _inbox.DeleteDocument(_inboxDocument).RunSynchronously();

            return this;
        }

        private static void Assert_state(object obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Requires gradually built state. Make sure you use functions in the correct order.");
            }
        }
    }
}