using System.Collections.Generic;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Relations;

namespace Digipost.Api.Client.Archive
{
    public class Archive : RestLinkable,IRequestContent
    {

        internal Archive(Sender sender)
            : this(null, sender, new List<ArchiveDocument>())
        {
        }

        internal Archive(string name, Sender sender)
            : this(name, sender, new List<ArchiveDocument>())
        {
        }

        public Archive(string name, Sender sender, IEnumerable<ArchiveDocument> archiveDocuments)
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
    }
}
