using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Api.Client.Archive;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Relations;
using Xunit;

#pragma warning disable 0169
#pragma warning disable 0649

namespace Digipost.Api.Client.Docs
{
    public class ArchiveExamples
    {
        private static readonly DigipostClient client;
        private static readonly Sender sender;

        private async Task ArchiveADocument()
        {
            var archive = new Archive.Archive(new Sender(1234), new List<ArchiveDocument>
            {
                new ArchiveDocument(Guid.NewGuid(), "invoice_123123.pdf", "pdf", "application/psd", readFileFromDisk("invoice_123123.pdf")),
                new ArchiveDocument(Guid.NewGuid(), "attachment_123123.pdf", "pdf", "application/psd", readFileFromDisk("attachment_123123.pdf"))
            });

            var savedArchive = await client.GetArchive().ArchiveDocuments(archive);
        }

        private async Task ArchiveADocumentWithAutoDelete()
        {
            var archive = new Archive.Archive(sender, new List<ArchiveDocument>
            {
                new ArchiveDocument(Guid.NewGuid(), "invoice_123123.pdf", "pdf", "application/psd", readFileFromDisk("invoice_123123.pdf"))
                    .WithDeletionTime(DateTime.Today.AddYears(5))
            });

            var savedArchive = await client.GetArchive().ArchiveDocuments(archive);
        }

        private async Task ArchiveADocumentToANamedArchive()
        {
            var archive = new Archive.Archive(sender, new List<ArchiveDocument>(), "MyArchiveName");

            var savedArchive = await client.GetArchive().ArchiveDocuments(archive);
        }

        private async Task<IEnumerable<Archive.Archive>> FetchArchives()
        {
            IEnumerable<Archive.Archive> fetchedArchives = await client.GetArchive().FetchArchives();
            return fetchedArchives;
        }

        private async void FetchAllArchiveDocuments()
        {
            var current = (await client.GetArchive().FetchArchives()).First();
            var documents = new List<ArchiveDocument>();

            while (current.HasMoreDocuments())
            {
                var fetchArchiveDocuments = client.GetArchive().FetchArchiveDocuments(current.GetNextDocumentsUri()).Result;
                documents.AddRange(fetchArchiveDocuments.ArchiveDocuments);
                current = fetchArchiveDocuments;
            }

            // documents now have all ArchiveDocuments in the archive
        }

        private async void FetchArchiveDocumentsWithAttribute()
        {
            var archive = (await client.GetArchive().FetchArchives()).First();
            var searchBy = new Dictionary<string, string>
            {
                ["key"] = "val"
            };

            var fetchArchiveDocuments = client.GetArchive().FetchArchiveDocuments(archive.GetNextDocumentsUri(searchBy)).Result;
            var documents = fetchArchiveDocuments.ArchiveDocuments;

            // documents now have the first 100 ArchiveDocuments in the archive that have invoicenumber=123123
        }
        private async void FetchAllArchiveDocumentsWithAttribute()
        {
            var current = (await client.GetArchive().FetchArchives()).First();
            var documents = new List<ArchiveDocument>();

            var searchBy = new Dictionary<string, string>
            {
                ["key"] = "val"
            };

            while (current.HasMoreDocuments())
            {
                var fetchArchiveDocuments = client.GetArchive().FetchArchiveDocuments(current.GetNextDocumentsUri(searchBy)).Result;
                documents.AddRange(fetchArchiveDocuments.ArchiveDocuments);
                current = fetchArchiveDocuments;
            }

            // documents now have all ArchiveDocuments in the archive that have invoicenumber=123123
        }

        private async Task FetchArchiveDocumentByGuid()
        {
            ArchiveDocument archiveDocument = await client.GetArchive(sender).FetchArchiveDocument(client.GetRoot(new ApiRootUri()).GetGetArchiveDocumentsByUuidUri(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c")));

            ArchiveDocumentContent archiveDocumentContent = await client.GetArchive().GetDocumentContent(archiveDocument.DocumentContentUri());
            Uri uri = archiveDocumentContent.Uri;

            Stream streamDocumentFromExternalId = await client.GetArchive(sender).StreamDocumentFromExternalId("My unique reference");
        }

        private void ArchiveDocumentAttributes()
        {
            var archiveDocument = new ArchiveDocument(Guid.NewGuid(), "invoice_123123.pdf", "pdf", "application/psd", readFileFromDisk("invoice_123123.pdf"))
            {
                Attributes = {["invoicenumber"] = "123123"}
            };
        }

        private async void FetchArchiveDocumentsByReferenceId()
        {
            IEnumerable<Archive.Archive> fetchArchiveDocumentsByReferenceId = await client.GetArchive().FetchArchiveDocumentsByReferenceId("MyProcessId[No12341234]");
        }

        private async void ChangeAttributesReferenceIdOnArchiveDocument()
        {
            ArchiveDocument archiveDocument = (await client.GetArchive(sender).FetchDocumentFromExternalId(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c"))).One();
            archiveDocument.WithAttribute("newKey", "foobar")
                .WithReferenceId("MyProcessId[No12341234]Done");

            ArchiveDocument updatedArchiveDocument = await client.GetArchive().UpdateDocument(archiveDocument, archiveDocument.GetUpdateUri());
        }

        private void AsBroker()
        {
            client.GetArchive(new Sender(111111)).FetchArchives();
        }

        private byte[] readFileFromDisk(string invoicePdf)
        {
            throw new NotImplementedException();
        }
    }
}
