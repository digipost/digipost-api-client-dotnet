using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Digipost.Api.Client.Common;
using static System.Convert;

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

        public Uri NextDocumentsUri(Dictionary<string, string> searchBy)
        {
            var nextDocumentsUri = Links["NEXT_DOCUMENTS"].AbsoluteUri();

            var query = HttpUtility.ParseQueryString(nextDocumentsUri.Query);
            var commaSeparated = string.Join(",", searchBy.Select(x => x.Key + "," + x.Value).ToArray());
            var base64 = ToBase64String(Encoding.UTF8.GetBytes(commaSeparated));

            query["attributes"] = "";
            var uriBuilder = new UriBuilder(nextDocumentsUri)
            {
                Query = query + base64
            };

            return new Uri(uriBuilder.ToString());
        }
    }
}
