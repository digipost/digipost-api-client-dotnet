using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Api.Client.Tests.TestSupport
{
    internal sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
        private int _invocationCount;

        public StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
        {
            _responder = responder;
        }

        public int InvocationCount => _invocationCount;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _invocationCount);

            return Task.FromResult(_responder(request));
        }
    }
}
