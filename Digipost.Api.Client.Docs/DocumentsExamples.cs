using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client.Docs
{
    public class DocumentsExamples
    {
#pragma warning disable 0649
        private static readonly DigipostClient Client;
        private static readonly Sender Sender;
#pragma warning restore 0649

        public void Hent_document_status()
        {
            DocumentStatus documentStatus = Client.GetDocumentStatus(Sender)
                .GetDocumentStatus(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c"))
                .Result;

            // example information:
            // documentStatus.DeliveryStatus => DELIVERED
            // documentStatus.DeliveryMethod => PRINT
        }
    }
}
