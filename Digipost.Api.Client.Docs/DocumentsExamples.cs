using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client.Docs
{
    public class DocumentsExamples
    {
        private static readonly DigipostClient client;
        private static readonly Sender sender;

        public void Hent_document_status()
        {
            DocumentStatus documentStatus = client.GetDocumentStatus(sender)
                .GetDocumentStatus(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c"))
                .Result;

            // example information:
            // documentStatus.DeliveryStatus => DELIVERED
            // documentStatus.DeliveryMethod => PRINT
        }
    }
}
