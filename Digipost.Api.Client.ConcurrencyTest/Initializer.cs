using System;
using ConcurrencyTester;
using Digipost.Api.Client.ConcurrencyTest.Enums;

namespace Digipost.Api.Client.ConcurrencyTest
{
    public class Initializer
    {
        private const ProcessingType ProcessingType = Enums.ProcessingType.Parallel;
        private const RequestType RequestType = Enums.RequestType.Identify;
        private const string HttpsQa2ApiDigipostNo = "https://qa2.api.digipost.no";
        private const string Thumbprint = "7d fc c9 8b 88 55 16 4d 03 a3 64 a4 90 98 26 9d 23 31 4d 0f";
        private const string SenderId = "106799002"; //"779052"; 
        private const int DegreeOfParallelism = 4;
        private const int NumberOfRequests = 100;
        private const int ThreadsActive = 4;

        
        public static void Run()
        {
            Console.WriteLine("Starting program ...");
            Digipost(NumberOfRequests, ThreadsActive, ProcessingType);
        }

        private static void Digipost(int numberOfRequests, int connectionLimit, ProcessingType processingType)
        {
            Console.WriteLine("Starting to send digipost: {0}, with requests: {1}, poolcount: {2}", processingType,
                numberOfRequests, connectionLimit);

            var clientConfig = new ClientConfig(SenderId) {ApiUrl = new Uri(HttpsQa2ApiDigipostNo)};

            switch (processingType)
            {
                case ProcessingType.Parallel:
                    new DigipostParalell(numberOfRequests, connectionLimit, DegreeOfParallelism, clientConfig,
                        Thumbprint).Run(RequestType);
                    break;
                case ProcessingType.Async:
                    new DigipostAsync(numberOfRequests, connectionLimit, clientConfig, Thumbprint).Run(RequestType);
                    break;
            }
        }
    }
}