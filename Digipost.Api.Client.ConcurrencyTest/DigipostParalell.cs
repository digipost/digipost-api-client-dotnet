using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Digipost.Api.Client;
using Digipost.Api.Client.ConcurrencyTest;
using Digipost.Api.Client.ConcurrencyTest.Enums;
using Digipost.Api.Client.Domain;

namespace ConcurrencyTester
{
    internal class DigipostParalell : DigipostRunner
    {
        private readonly int _defaultConnectionLimit;
        private readonly int _degreeOfParallelism;

        public DigipostParalell(int numberOfRequests, int defaultConnectionLimit, int degreeOfParallelism,
            ClientConfig clientConfig, string thumbprint) :
                base(clientConfig, thumbprint, numberOfRequests)
        {
            _defaultConnectionLimit = defaultConnectionLimit;
            _degreeOfParallelism = degreeOfParallelism;
        }

        public override void Run(RequestType requestType)
        {
            Stopwatch.Start();
            ServicePointManager.DefaultConnectionLimit = _defaultConnectionLimit;

            var messages = new List<Message>();
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