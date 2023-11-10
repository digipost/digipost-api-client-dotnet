using System;
using Digipost.Api.Client.Common.Relations;
using Digipost.Api.Client.Common.Utilities;

namespace Digipost.Api.Client.Common.Entrypoint
{
    /// <summary>
    /// The Root entrypoint is the starting point of the REST-api of Digipost.
    /// </summary>
    public class Root : RestLinkable
    {
        public Root(string certificate)
        {
            Certificate = certificate;
        }

        /// <summary>
        /// The element certificate contains Digipost's current public key. It can be used to verify
        /// each response from Digipost. See the documentation for more information.
        /// </summary>
        public string Certificate { get; set; }

        public ApiRootUri GetApiRootUri(Sender sender = null)
        {
            return new ApiRootUri(sender);
        }

        public SenderInformationUri GetSenderInformationUri(Sender sender)
        {
            return new SenderInformationUri(Links["GET_SENDER_INFORMATION"], sender);
        }
        public SenderInformationUri GetSenderInformationUri(string organisationNumber, string partId)
        {
            return new SenderInformationUri(Links["GET_SENDER_INFORMATION"], organisationNumber, partId);
        }

        public GetInboxUri GetGetInboxUri(int offset = 0, int limit = 100)
        {
            return new GetInboxUri(Links["GET_INBOX"], offset, limit);
        }

        public SendMessageUri GetSendMessageUri()
        {
            return new SendMessageUri(Links["CREATE_MESSAGE"]);
        }

        public IdentifyRecipientUri GetIdentifyRecipientUri()
        {
            return new IdentifyRecipientUri(Links["IDENTIFY_RECIPIENT"]);
        }

        public RecipientSearchUri GetRecipientSearchUri(string search)
        {
            return new RecipientSearchUri(Links["SEARCH"], search);
        }

        public GetArchivesUri GetGetArchivesUri()
        {
            return new GetArchivesUri(Links["GET_ARCHIVES"]);
        }

        public GetArchiveDocumentsUri GetArchiveDocumentsUri()
        {
            return new GetArchiveDocumentsUri(Links["ARCHIVE_DOCUMENTS"]);
        }

        public GetArchiveDocumentByReferenceIdUri GetGetArchiveDocumentsReferenceIdUri(string referenceId)
        {
            return new GetArchiveDocumentByReferenceIdUri(Links["GET_ARCHIVE_DOCUMENTS_BY_REFERENCEID"], referenceId);
        }
        public GetArchiveDocumentByUuidUri GetGetArchiveDocumentsByUuidUri(string externalId)
        {
            var nameUuidFromBytes = UuidInterop.NameUuidFromBytes(externalId);

            return new GetArchiveDocumentByUuidUri(Links["GET_ARCHIVE_DOCUMENT_BY_UUID"], Guid.Parse(nameUuidFromBytes));
        }

        public GetArchiveDocumentByUuidUri GetGetArchiveDocumentsByUuidUri(Guid guid)
        {
            return new GetArchiveDocumentByUuidUri(Links["GET_ARCHIVE_DOCUMENT_BY_UUID"], guid);
        }

        public DocumentEventsUri GetDocumentEventsUri(Sender sender, DateTime from, DateTime to, int offset, int maxResults)
        {
            return new DocumentEventsUri(Links["DOCUMENT_EVENTS"], sender, from, to, offset, maxResults);
        }

        public DocumentStatusUri GetDocumentStatusUri(Guid guid)
        {
            return new DocumentStatusUri(Links["DOCUMENT_STATUS"], guid);
        }

        public ShareDocumentsRequestStateUri GetShareDocumentsRequestStateUri(Guid guid)
        {
            return new ShareDocumentsRequestStateUri(Links["GET_SHARE_DOCUMENTS_REQUEST_STATE"], guid);
        }
    }

    public class Link
    {
        public Link(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// The actual uri for the resource
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// A string constant representing the Relation of the uri. The rel is Optional
        /// but is in practice always present.
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// An optional media type describing the resource
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// the Uri as a typed absolute Uri
        /// </summary>
        /// <returns></returns>
        public Uri AbsoluteUri()
        {
            return new Uri(Uri, UriKind.Absolute);
        }
    }
}
