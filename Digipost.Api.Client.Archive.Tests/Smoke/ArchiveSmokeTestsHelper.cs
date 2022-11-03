using System;
using System.Collections.Generic;
using System.Linq;
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
        private ArchiveApi _archiveApi;
        private IEnumerable<Archive> _archives;

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
        }

        public ArchiveSmokeTestsHelper Get_archives()
        {
            _archiveApi = _client.GetArchive(new Sender(_testSender.Id));
            _archives = _archiveApi.FetchArchives().Result;

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
