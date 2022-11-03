using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;

namespace Digipost.Api.Client.Archive
{
    public class Archive : RestLinkable
    {
        public Archive(string name, Sender sender)
        {
            Name = name;
            Sender = sender;
            ArchiveDocuments = new List<ArchiveDocument>();
        }

        public string Name { get; }
        public Sender Sender { get; }

        public List<ArchiveDocument> ArchiveDocuments { get; set; }

        /// <summary>
        /// True if you can fetch more documents from the archive
        /// </summary>
        /// <returns></returns>
        public bool HasMoreDocuments()
        {
            return Links.ContainsKey("NEXT_DOCUMENTS");
        }

        /// <summary>
        /// Url to fetch documents. This Url is also used to fetch the first documents. Looping
        /// over documents in an archive means looping until the relation hasMoreDocuments() => false
        /// </summary>
        /// <returns></returns>
        public Uri NextDocumentsUri()
        {
            return Links["NEXT_DOCUMENTS"].AbsoluteUri();
        }
    }
}
