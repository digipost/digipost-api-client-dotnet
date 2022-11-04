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
        public static readonly string ArchiveName = "SmokeTestArchive";
        private readonly TestSender _testSender;
        private readonly ArchiveApi _archiveApi;
        private Archive _archivesWithDocuments;
        private Archive _archive;

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
                new ClientConfig(broker, testSender.Environment){LogRequestAndResponse = true},
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
            _archive = _archiveApi.FetchArchives().Result.Where(a => a.Name == null).First();
            return this;
        }

        public ArchiveSmokeTestsHelper Get_All_Documents()
        {
            Assert_state(_archive);

            _archivesWithDocuments = _archiveApi.FetchArchiveDocuments(_archive);

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

            var byAttribute = _archiveApi.FetchArchiveDocuments(_archive, searchBy);

            Assert.NotEmpty(byAttribute.ArchiveDocuments);
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

        public ArchiveSmokeTestsHelper Delete_All_Documents()
        {
            Assert_state(_archivesWithDocuments);
            Assert.NotEmpty(_archivesWithDocuments.ArchiveDocuments);
            foreach (var archiveDocument in _archivesWithDocuments.ArchiveDocuments)
            {
                _archiveApi.DeleteDocument(archiveDocument).Wait();
            }

            return this;
        }

        public ArchiveSmokeTestsHelper ArchiveAFile(string archiveName = null)
        {
            string content = $"Smoketested with .net api client on {DateTime.Now.ToString(CultureInfo.CurrentCulture)}";
            var withArchiveDocument = new Archive(archiveName, new Sender(_testSender.Id))
                .WithArchiveDocument(
                    new ArchiveDocument(Guid.NewGuid(), "smoketest.txt", "txt", "text/plain", FileContentBytes(content))
                        .WithAttribute("smoke", "test")
                );

            _archive = _archiveApi.ArchiveDocuments(withArchiveDocument);

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
