using System;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Send
{
    public class DocumentStatus
    {
        public DocumentStatus(
            string guid,
            long senderId,
            DateTime created,
            DocumentDeliveryStatus documentDeliveryStatus,
            Read? read,
            DeliveryMethod deliveryMethod,
            string contentHash,
            DateTime? delivered,
            Boolean? isPrimaryDocument,
            HashAlgoritm? contentHashAlgoritm
        )
        {
            Guid = guid;
            Sender = new Sender(senderId);
            Created = created;
            DeliveryStatus = documentDeliveryStatus;
            DocumentRead = read;
            DeliveryMethod = deliveryMethod;
            ContentHash = contentHash;
            Delivered = delivered;
            IsPrimaryDocument = isPrimaryDocument;
            ContentHashAlgoritm = contentHashAlgoritm;
        }

        public string Guid { get; }

        public Sender Sender { get; }

        public DateTime Created { get; }

        /**
         * If DeliveryStatus is NOT_DELIVERED, Delivered will not have a value
         */
        public DateTime? Delivered { get; }

        public DocumentDeliveryStatus DeliveryStatus { get; }

        public Read? DocumentRead { get; }

        public DeliveryMethod DeliveryMethod { get; }

        public String ContentHash { get; }

        public HashAlgoritm? ContentHashAlgoritm { get; }

        /**
         * isPrimaryDocument has value only if you ask api are the actual sender asking for DocumentStatus.
         * If you are, then this will be true for the primary document else false.
         */
        public Boolean? IsPrimaryDocument { get; }

        public enum DocumentDeliveryStatus
        {
            /**
             * The document has been delivered
             */
            DELIVERED,

            /**
             * The document is still being processed
             */
            NOT_DELIVERED
        }

        /**
         * Indicates whether the document is read or not
         */
        public enum Read
        {
            YES,
            NO
        }
    }
}
