using System;
using System.Collections.Generic;
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
        private readonly DigipostClient _client;
        private readonly TestSender _testSender;
        private readonly ArchiveApi _archiveApi;
        private IEnumerable<Archive> _archives;
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

            _client = new DigipostClient(
                new ClientConfig(broker, testSender.Environment),
                testSender.Certificate,
                serviceProvider.GetService<ILoggerFactory>()
            );

            _archiveApi = _client.GetArchive(new Sender(_testSender.Id));
        }

        public ArchiveSmokeTestsHelper Get_archives()
        {
            Assert_state(_archive);
            _archives = _archiveApi.FetchArchives().Result;
            Assert.True(_archives.Any());
            return this;
        }

        public ArchiveSmokeTestsHelper ArchiveAFile()
        {
            string content = $"Smoketested with .net api client on {DateTime.Now.ToString()}";
            var withArchiveDocument = new Archive("MittArkiv", new Sender(_testSender.Id))
                .WithArchiveDocument(
                    new ArchiveDocument(Guid.NewGuid(), "smoketest.txt", "txt", "text/plain", FileContentBytes(content))
                        .WithAttribute("smoke", "test")
                );

            _archive = _archiveApi.ArchiveDocuments(withArchiveDocument);

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
