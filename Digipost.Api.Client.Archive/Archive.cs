using System;
using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Relations;

namespace Digipost.Api.Client.Archive
{
    public class Archive : RestLinkable,IRequestContent
    {

        public Archive(Sender sender, string name = null)
            : this(sender, new List<ArchiveDocument>(), name)
        {
        }

        public Archive(Sender sender, IEnumerable<ArchiveDocument> archiveDocuments, string name = null)
        {
            Name = name;
            Sender = sender;
            ArchiveDocuments = new List<ArchiveDocument>();
            ArchiveDocuments.AddRange(archiveDocuments);
        }

        public Archive WithArchiveDocument(ArchiveDocument archiveDocument)
        {
            ArchiveDocuments.Add(archiveDocument);
            return this;
        }

        public Archive WithArchiveDocuments(IEnumerable<ArchiveDocument> archiveDocuments)
        {
            ArchiveDocuments.AddRange(archiveDocuments);
            return this;
        }

        public string Name { get; }

        public Sender Sender { get; }

        public List<ArchiveDocument> ArchiveDocuments { get; set; }

        public ArchiveDocument One()
        {
            return ArchiveDocuments[0];
        }

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
        public ArchiveNextDocumentsUri GetNextDocumentsUri()
        {
            return new ArchiveNextDocumentsUri(Links["NEXT_DOCUMENTS"]);
        }

        public ArchiveNextDocumentsUri GetNextDocumentsUri(Dictionary<string, string> searchBy)
        {
            return new ArchiveNextDocumentsUri(Links["NEXT_DOCUMENTS"], searchBy);
        }

        public ArchiveNextDocumentsUri GetNextDocumentsUri(DateTime from, DateTime to)
        {
            return new ArchiveNextDocumentsUri(Links["NEXT_DOCUMENTS"], from, to);
        }

        public ArchiveNextDocumentsUri GetNextDocumentsUri(Dictionary<string,string> searchBy, DateTime from, DateTime to)
        {
            return new ArchiveNextDocumentsUri(Links["NEXT_DOCUMENTS"], searchBy, from, to);
        }
    }
}
