using System.Net;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.ConcurrencyTest.Enums;

namespace Digipost.Api.Client.ConcurrencyTest
{
    internal class DigipostAsync : DigipostRunner
    {
        private readonly int _defaultConnectionLimit;

        public DigipostAsync(int numberOfRequests, int defaultConnectionLimit, ClientConfig clientconfig,
            string thumbprint)
            :base(clientconfig, thumbprint, numberOfRequests)
        {
            _defaultConnectionLimit = defaultConnectionLimit;
        }

        public override void Run(RequestType requestType)
        {
            Stopwatch.Start();
            ServicePointManager.DefaultConnectionLimit = _defaultConnectionLimit;

            while (RunsLeft() > 0)
            {
                Send(Client, requestType);
            }

            Stopwatch.Stop();
            DisplayTestResults();
        }
    }
}