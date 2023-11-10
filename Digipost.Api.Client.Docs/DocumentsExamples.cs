using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client.Docs
{
    public class DocumentsExamples
    {
#pragma warning disable 0649
        private static readonly DigipostClient Client;
        private static readonly Sender Sender;
#pragma warning restore 0649

        public async void Hent_document_status()
        {
            DocumentStatus documentStatus = await Client.DocumentsApi()
                .GetDocumentStatus(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c"));

            // example information:
            // documentStatus.DeliveryStatus => DELIVERED
            // documentStatus.DeliveryMethod => PRINT
        }

        public async void Hent_document_events_last_5_minutes_max100()
        {
            // Fetch max 100 events last 5 minutes
            // Beware that there might be more events. So you must fetch more events by adding to the offset
            // until you get 0 or less than 100 events.
            // Then you can start again from the previous `to` datetime as `from`
            DocumentEvents events = await Client.DocumentsApi(Sender)
                .GetDocumentEvents(from: DateTime.Now.Subtract(TimeSpan.FromMinutes(5)), to: DateTime.Now, offset: 0, maxResults: 100);

            IEnumerable<DocumentEventType> documentEventTypes = events.Select(aEvent => aEvent.EventType);
        }
    }
}
