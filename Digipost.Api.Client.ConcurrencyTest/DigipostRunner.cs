using System;
using System.Diagnostics;
using System.Threading;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Common.Enums;
using Digipost.Api.Client.Common.Identify;
using Digipost.Api.Client.Common.Recipient;
using Digipost.Api.Client.ConcurrencyTest.Enums;
using Digipost.Api.Client.Resources.Content;
using Digipost.Api.Client.Send;

namespace Digipost.Api.Client.ConcurrencyTest
{
    internal abstract class DigipostRunner
    {
        private readonly Lazy<DigipostClient> _client;
        private readonly object _lock = new object();
        private byte[] _documentBytes;
        private int _failedCalls;
        private IIdentification _identification;
        private int _itemsLeft;
        private Message _message;
        private int _successfulCalls;
        public Stopwatch Stopwatch;

        protected DigipostRunner(ClientConfig clientConfig, string thumbprint, int numOfRuns)
        {
            _client = new Lazy<DigipostClient>(() => new DigipostClient(clientConfig, thumbprint));
            Stopwatch = new Stopwatch();
            _itemsLeft = numOfRuns + 1; //Fordi vi decrementer teller før return
        }

        public DigipostClient Client => _client.Value;

        public int RunsLeft()
        {
            return Interlocked.Decrement(ref _itemsLeft);
        }

        public abstract void Run(RequestType requestType);

        public IMessage GetMessage()
        {
            lock (_lock)
            {
                if (_message != null) return _message;

                var primaryDocument = new Document("document subject", "txt", GetDocumentBytes());
                var digitalRecipientWithFallbackPrint = new RecipientByNameAndAddress("Ola Nordmann", "0460",
                    "Oslo", "Collettsgate 68");
                _message = new Message(new Sender(1010), digitalRecipientWithFallbackPrint, primaryDocument);
            }

            return _message;
        }

        public IIdentification GetIdentification()
        {
            lock (_lock)
            {
                if (_identification != null) return _identification;
                _identification = new Identification(new RecipientById(IdentificationType.PersonalIdentificationNumber, "01013300001"));
            }

            return _identification;
        }

        private byte[] GetDocumentBytes()
        {
            return _documentBytes ?? (_documentBytes = ContentResource.Hoveddokument.PlainText());
        }

        public async void Send(DigipostClient digipostClient, RequestType requestType)
        {
            try
            {
                switch (requestType)
                {
                    case RequestType.Message:
                        await digipostClient.SendMessageAsync(GetMessage()).ConfigureAwait(false);
                        break;
                    case RequestType.Identify:
                        await digipostClient.IdentifyAsync(GetIdentification()).ConfigureAwait(false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
                }

                Interlocked.Increment(ref _successfulCalls);
            }
            catch (Exception e)
            {
                Interlocked.Increment(ref _failedCalls);
                Console.WriteLine("Request failed. Are you connected to VPN? Reason{0}. Inner: {1}", e.Message,
                    e.InnerException.Message);
                Console.WriteLine(e.InnerException.InnerException);
            }

            Console.Write(".");
        }

        protected void DisplayTestResults()
        {
            var performanceAllWork = _successfulCalls / (Stopwatch.ElapsedMilliseconds / 1000d);

            Console.WriteLine();
            Console.WriteLine(
                "Success:" + _successfulCalls + ", \n" +
                "Failed:" + _failedCalls + " \n" +
                "Duration:" + Stopwatch.ElapsedMilliseconds + " \n" +
                "Performance full run:" + performanceAllWork.ToString("#.###") + " req/sec");
        }
    }
}
