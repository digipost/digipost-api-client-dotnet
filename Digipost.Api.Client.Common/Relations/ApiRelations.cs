using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Digipost.Api.Client.Common.Entrypoint;
using static System.Convert;

namespace Digipost.Api.Client.Common.Relations
{
    /// <summary>
    /// A guid-based uri is typically one that is supposed to be appended by a guid that only the sender/broker can know.
    /// eg.
    /// api/1234/document/uuid/{append-guid-here}
    /// </summary>
    public abstract class GuidBasedUri : Uri
    {
        protected GuidBasedUri(Link link, Guid? guid)
            : base($"{link.Uri}{guid.ToString()}", UriKind.Absolute)
        {
        }
    }

    public class ApiRootUri : Uri
    {
        public ApiRootUri(Sender senderId = null)
            : base($"/{(senderId == null ? "" : senderId.Id.ToString())}", UriKind.Relative)
        {
        }
    }

    public class SenderInformationUri : Uri
    {
        public SenderInformationUri(Link link, Sender sender)
            : base($"{link.Uri}/{sender.Id}", UriKind.Absolute)
        {
        }

        public SenderInformationUri(Link link, string organisationNumber, string partId)
            : base($"{link.Uri}?org_id={organisationNumber}&part_id={partId}", UriKind.Absolute)
        {
        }
    }

    public class SendMessageUri : Uri
    {
        public SendMessageUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class IdentifyRecipientUri : Uri
    {
        public IdentifyRecipientUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class RecipientSearchUri : Uri
    {
        public RecipientSearchUri(Link link, string search)
            : base($"{link.Uri}/{EscapeUriString(search)}", UriKind.Absolute)
        {
        }
    }

    public class AddAdditionalDataUri : Uri
    {
        public AddAdditionalDataUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class GetInboxUri : Uri
    {
        public GetInboxUri(Link link, int offset = 0, int limit = 100)
            : base($"{link.Uri}?offset={offset}&limit={limit}", UriKind.Absolute)
        {
        }
    }

    public class GetInboxDocumentContentUri : Uri
    {
        public GetInboxDocumentContentUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class InboxDocumentDeleteUri : Uri
    {
        public InboxDocumentDeleteUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class GetArchiveDocumentsUri : Uri
    {
        public GetArchiveDocumentsUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class GetArchiveDocumentByReferenceIdUri : Uri
    {
        public GetArchiveDocumentByReferenceIdUri(Link link, string referenceId)
            : base($"{link.Uri}{ToBase64String(Encoding.UTF8.GetBytes(referenceId))}", UriKind.Absolute)
        {
        }
    }

    public class GetArchiveDocumentByUuidUri : GuidBasedUri
    {
        public GetArchiveDocumentByUuidUri(Link link, Guid guid)
            : base(link, guid)
        {
        }

        public GetArchiveDocumentByUuidUri(Link link)
            : base(link, null)
        {
        }
    }

    public class GetArchivesUri : Uri
    {
        public GetArchivesUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class ArchiveNextDocumentsUri : Uri
    {
        public ArchiveNextDocumentsUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }

        public ArchiveNextDocumentsUri(Link link, Dictionary<string, string> searchBy)
            : base(ToUri(new Uri(link.Uri, UriKind.Absolute), searchBy, null, null), UriKind.Absolute)
        {
        }

        public ArchiveNextDocumentsUri(Link link, DateTime from, DateTime to)
            : base(ToUri(new Uri(link.Uri, UriKind.Absolute), new Dictionary<string, string>(), from, to), UriKind.Absolute)
        {
        }

        public ArchiveNextDocumentsUri(Link link, Dictionary<string, string> searchBy, DateTime from, DateTime to)
            : base(ToUri(new Uri(link.Uri, UriKind.Absolute), searchBy, from, to), UriKind.Absolute)
        {
            
        }

        internal static string ToUri(Uri nextDocumentsUri, Dictionary<string, string> searchBy, DateTime? from, DateTime? to)
        {
            var query = HttpUtility.ParseQueryString(nextDocumentsUri.Query);
            if (searchBy.Count > 0)
            {
                var commaSeparated = string.Join(",", searchBy.Select(x => x.Key + "," + x.Value).ToArray());
                var base64 = ToBase64String(Encoding.UTF8.GetBytes(commaSeparated));

                query["attributes"] = base64;
            }

            if (from != null && to != null)
            {
                query["fromDate"] = from.Value.ToString("o");
                query["toDate"] = to.Value.ToString("o");
            }

            var uriBuilder = new UriBuilder(nextDocumentsUri)
            {
                Query = query.ToString()
            };
            return uriBuilder.ToString();
        }
    }

    public class ArchiveDocumentContentStreamUri : Uri
    {
        public ArchiveDocumentContentStreamUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class ArchiveDocumentContentUri : Uri
    {
        public ArchiveDocumentContentUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class ArchiveDocumentUpdateUri : Uri
    {
        public ArchiveDocumentUpdateUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class ArchiveDocumentDeleteUri : Uri
    {
        public ArchiveDocumentDeleteUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class DocumentEventsUri : Uri
    {
        public DocumentEventsUri(Link link, Sender sender, DateTime from, DateTime to, int offset, int maxResults)
            : base($"{link.Uri}?sender={sender.Id}&from={DatetimeFormatter(from)}&to={DatetimeFormatter(to)}&offset={offset}&maxResults={maxResults}", UriKind.Absolute)
        {
        }

        private static string DatetimeFormatter(DateTime? dt)
        {
            return HttpUtility.UrlEncode(dt?.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK"));
        }
    }

    public class DocumentStatusUri : GuidBasedUri
    {
        public DocumentStatusUri(Link link, Guid guid)
            : base(link, guid)
        {
        }
    }

    public class ShareDocumentsRequestStateUri : GuidBasedUri
    {
        public ShareDocumentsRequestStateUri(Link link, Guid guid)
            : base(link, guid)
        {
        }
    }

    public class GetSharedDocumentContentStreamUri : Uri
    {
        public GetSharedDocumentContentStreamUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }

    public class GetSharedDocumentContentUri : Uri
    {
        public GetSharedDocumentContentUri(Link link)
            : base(link.Uri, UriKind.Absolute)
        {
        }
    }
}
