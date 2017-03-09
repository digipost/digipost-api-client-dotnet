using System;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Domain.Mailbox;
using Digipost.Api.Client.Test.Utilities;
using Xunit;

namespace Digipost.Api.Client.Mailbox.Tests.Smoke
{
    public class MailboxSmokeTestsHelper
    {
        private readonly string _senderId;
        private readonly DigipostClient _client;

        //Stepwise built state
        private Inbox _inbox;
        private Mailbox _mailbox;
        private InboxDocument _inboxDocument;

        internal MailboxSmokeTestsHelper(Sender sender)
        {
            _senderId = sender.Id;
            _client = new DigipostClient(
                new ClientConfig(sender.Id, sender.Environment), 
                sender.Certificate
            );
        }

        public MailboxSmokeTestsHelper Get_inbox()
        {
            _mailbox = _client.Mailbox(_senderId);
            _inbox = _mailbox.FetchInbox().Result;

            return this;
        }

        public MailboxSmokeTestsHelper Expect_inbox_to_have_documents()
        {
            Assert_state(_inbox);

            Assert.True(_inbox.Documents.Any());

            _inboxDocument = _inbox.Documents.First();

            return this;
        }

        public MailboxSmokeTestsHelper Fetch_document_data()
        {
            Assert_state(_inboxDocument);

            var documentStream = _mailbox.FetchDocument(_inboxDocument).Result;

            Assert.Equal(true, documentStream.CanRead);
            Assert.True(documentStream.Length > 500);

            return this;
        }

        public MailboxSmokeTestsHelper Delete_document()
        {
            Assert_state(_inboxDocument);

            var deleted = _mailbox.DeleteDocument(_inboxDocument);

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
