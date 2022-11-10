using System;

namespace Digipost.Api.Client.Archive
{
    public class ArchiveDocumentContent
    {
        public ArchiveDocumentContent(string contentType, Uri uri)
        {
            ContentType = contentType;
            Uri = uri;
        }

        public string ContentType { get; }

        public Uri Uri { get; }
    }
}
