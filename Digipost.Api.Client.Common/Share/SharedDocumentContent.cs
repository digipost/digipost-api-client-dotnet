using System;

namespace Digipost.Api.Client.Common.Share
{
    public class SharedDocumentContent
    {
        public SharedDocumentContent(string contentType, Uri uri)
        {
            ContentType = contentType;
            Uri = uri;
        }

        public string ContentType { get; }

        public Uri Uri { get; }
    }
}
