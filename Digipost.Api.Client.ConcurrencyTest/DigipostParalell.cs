using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.ConcurrencyTest.Enums;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client.ConcurrencyTest
{
    internal class DigipostParalell : DigipostRunner
    {
        private readonly int _defaultConnectionLimit;
        private readonly int _degreeOfParallelism;

        public DigipostParalell(int numberOfRequests, int defaultConnectionLimit, int degreeOfParallelism,
            ClientConfig clientConfig, string thumbprint)
            : base(clientConfig, thumbprint, numberOfRequests)
        {
            _defaultConnectionLimit = defaultConnectionLimit;
            _degreeOfParallelism = degreeOfParallelism;
        }

        public override void Run(RequestType requestType)
        {
            Stopwatch.Start();
            ServicePointManager.DefaultConnectionLimit = _defaultConnectionLimit;

            var messages = new List<IMessage>();
            while (RunsLeft() > 0)
            {
                messages.Add(GetMessage());
            }

            var options = new ParallelOptions {MaxDegreeOfParallelism = _degreeOfParallelism};
            Parallel.ForEach(messages, options, message => Send(Client, requestType));

            Stopwatch.Stop();
            DisplayTestResults();
        }
    }
}
