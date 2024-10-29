using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Utilities;
using Digipost.Api.Client.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Digipost.Api.Client.Archive.Tests.Smoke
{
    public class ArchiveSmokeTestsHelper
    {
        internal static readonly string ArchiveName = "SmokeTestArchive";
        private readonly TestSender _testSender;
        private readonly IArchiveApi _archiveApi;
        private Archive _archivesWithDocuments;
        private Archive _archive;
        private Archive _byAttribute;

        private static readonly Func<string, string> FileContent = identifyingText => $@"
_____ __  _______  __ __ _____________________________
/ ___//  |/  / __ \/ //_// ____/_  __/ ____/ ___/_  __/
\__ \/ /|_/ / / / / ,<  / __/   / / / __/  \__ \ / /
___/ / /  / / /_/ / /| |/ /___  / / / /___ ___/ // /
/____/_/  /_/\____/_/ |_/_____/ /_/ /_____//____//_/
{identifyingText}
";

        private static readonly Func<string, byte[]> FileContentBytes = str => Encoding.UTF8.GetBytes(FileContent(str));

        internal ArchiveSmokeTestsHelper(TestSender testSender)
        {
            _testSender = testSender;
            var broker = new Broker(testSender.Id);

            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();

            var client = new DigipostClient(
                new ClientConfig(broker, testSender.Environment) {LogRequestAndResponse = true},
                testSender.Certificate,
                serviceProvider.GetService<ILoggerFactory>()
            );

            _archiveApi = client.GetArchive(new Sender(_testSender.Id));
        }

        public ArchiveSmokeTestsHelper Get_Archive(string archiveName)
        {
            _archive = _archiveApi.FetchArchives().Result.Where(a => archiveName.Equals(a.Name)).First();
            Assert.True(_archive.Name.Equals(archiveName));
            return this;
        }

        public ArchiveSmokeTestsHelper Get_Default_Archive()
        {
            _archive = _archiveApi.FetchArchives().Result.First(a => a.Name == string.Empty);
            return this;
        }

        public ArchiveSmokeTestsHelper Get_All_Documents()
        {
            Assert_state(_archive);

            _archivesWithDocuments = _archiveApi.FetchArchiveDocuments(_archive.GetNextDocumentsUri()).Result;

            Assert.NotEmpty(_archivesWithDocuments.ArchiveDocuments);
            return this;
        }

        public ArchiveSmokeTestsHelper Get_All_DocumentsWithAttributes()
        {
            Assert_state(_archive);

            var searchBy = new Dictionary<string, string>
            {
                ["smoke"] = "test"
            };

            _byAttribute = _archiveApi.FetchArchiveDocuments(_archive.GetNextDocumentsUri(searchBy)).Result;

            Assert.NotEmpty(_byAttribute.ArchiveDocuments);
            return this;
        }

        public ArchiveSmokeTestsHelper Get_For_Timeinterval(DateTime start, DateTime end, int expectCount = 0)
        {
            _byAttribute = _archiveApi.FetchArchiveDocuments(_archive.GetNextDocumentsUri(start, end)).Result;
            
            Assert.Equal(expectCount, _byAttribute.ArchiveDocuments.Count);
            
            return this;
        }

        public ArchiveSmokeTestsHelper Get_For_TimeintervalWithAttributes(Dictionary<string, string> searchBy, DateTime start, DateTime end, int expectCount = 0)
        {
            _byAttribute = _archiveApi.FetchArchiveDocuments(_archive.GetNextDocumentsUri(searchBy, start, end)).Result;
            
            Assert.Equal(expectCount, _byAttribute.ArchiveDocuments.Count);
            
            return this;
        }

        public ArchiveSmokeTestsHelper Update_attritbutesOnFirst()
        {
            Assert_state(_byAttribute);

            var documentToUpdate = _byAttribute.ArchiveDocuments[0];
            documentToUpdate.ReferenceId = "TheBoss";
            documentToUpdate.WithAttribute("nr", "007");

            var updateDocument = _archiveApi.UpdateDocument(documentToUpdate, documentToUpdate.GetUpdateUri());

            Assert.Equal("TheBoss", updateDocument.Result.ReferenceId);
            Assert.True(updateDocument.Result.Attributes.ContainsKey("nr"));
            Assert.True(updateDocument.Result.Attributes.ContainsValue("007"));
            Assert.True(updateDocument.Result.Attributes.ContainsKey("smoke"));
            Assert.True(updateDocument.Result.Attributes.ContainsValue("test"));

            return this;
        }

        public ArchiveSmokeTestsHelper Get_Documents()
        {
            Assert_state(_archivesWithDocuments);
            Assert.NotEmpty(_archivesWithDocuments.ArchiveDocuments);

            foreach (var archiveDocument in _archivesWithDocuments.ArchiveDocuments)
            {
                var documentStream = _archiveApi.StreamDocumentFromExternalId(archiveDocument.Id).Result;
                Assert.Equal(true, documentStream.CanRead);
                Assert.True(documentStream.Length > 100);
            }

            return this;
        }

        public ArchiveSmokeTestsHelper Get_DocumentsByReferenceId()
        {
            Assert_state(_archivesWithDocuments);
            Assert.NotEmpty(_archivesWithDocuments.ArchiveDocuments);

            var list = _archiveApi.FetchArchiveDocumentsByReferenceId("TheBoss").Result;
            Assert.Equal("TheBoss", list.First().ArchiveDocuments[0].ReferenceId);

            return this;
        }

        public ArchiveSmokeTestsHelper Delete_All_Documents()
        {
            Assert_state(_archivesWithDocuments);
            Assert.NotEmpty(_archivesWithDocuments.ArchiveDocuments);
            foreach (var archiveDocument in _archivesWithDocuments.ArchiveDocuments)
            {
                _archiveApi.DeleteDocument(archiveDocument.GetDeleteUri()).Wait();
            }

            return this;
        }

        public ArchiveSmokeTestsHelper ArchiveAFile(string archiveName = null)
        {
            string content = $"Smoketested with .net api client on {DateTime.Now.ToString(CultureInfo.CurrentCulture)}";
            var newGuid = Guid.NewGuid();

            var withArchiveDocument = new Archive(new Sender(_testSender.Id), archiveName)
                .WithArchiveDocument(
                    new ArchiveDocument(newGuid, "smoketest.txt", "txt", "text/plain", FileContentBytes(content))
                        .WithAttribute("smoke", "test")
                );

            _archive = _archiveApi.ArchiveDocuments(withArchiveDocument).Result;

            var archiveDocument = _archiveApi.FetchDocumentFromExternalId(newGuid).Result;
            Assert.NotEmpty(_archive.ArchiveDocuments);

            return this;
        }

        private static void Assert_state(object obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Requires gradually built state. Make sure you use functions in the correct order.");
            }
        }
    }
}
