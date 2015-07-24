using System;
using System.Diagnostics;
using ConcurrencyTester;
using Digipost.Api.Client.ConcurrencyTest.Enums;

namespace Digipost.Api.Client.ConcurrencyTest
{
    public class Initializer
    {
        private const ProcessingType ProcessingType = Enums.ProcessingType.Parallel;
        private const RequestType RequestType = Enums.RequestType.Message;
        private const string HttpsQa2ApiDigipostNo = "https://qa2.api.digipost.no";
        private const string Thumbprint = "29 7e 44 24 f2 8d ed 2c 9a a7 3d 9b 22 7c 73 48 f1 8a 1b 9b";
        private const string SenderId = "106824802"; //"779052"; 
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

            var clientConfig = new ClientConfig(SenderId)
            {
                ApiUrl = new Uri(HttpsQa2ApiDigipostNo),
                Logger = (severity, konversasjonsId, metode, melding) =>
                {
                    
                }
            };

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