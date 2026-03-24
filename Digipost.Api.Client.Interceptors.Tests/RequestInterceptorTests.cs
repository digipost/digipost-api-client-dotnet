using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Api.Client.Common;
using Digipost.Api.Client.Internal;
using Xunit;

namespace Digipost.Api.Client.Tests.Handlers
{
    public class RequestInterceptorTests
    {
        private static HttpClient BuildClient(IList<IRequestInterceptor> interceptors, HttpMessageHandler innerHandler = null)
        {
            var interceptorHandler = new InterceptorHandler(interceptors)
            {
                InnerHandler = innerHandler ?? new OkResponseHandler()
            };
            return new HttpClient(interceptorHandler);
        }

        private sealed class OkResponseHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        private sealed class RecordingInterceptor : IRequestInterceptor
        {
            public readonly List<string> Events = new List<string>();
            private readonly string _name;

            public RecordingInterceptor(string name = "A") => _name = name;

            public Task OnBeforeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
            {
                Events.Add($"before:{_name}");
                return Task.CompletedTask;
            }

            public Task OnAfterResponseAsync(HttpResponseMessage response, HttpRequestMessage request, CancellationToken cancellationToken = default)
            {
                Events.Add($"after:{_name}");
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task PreRequestInterceptor_IsCalledBeforeRequest()
        {
            var interceptor = new RecordingInterceptor();
            var client = BuildClient(new List<IRequestInterceptor> { interceptor });

            await client.GetAsync("http://localhost/test");

            Assert.Contains("before:A", interceptor.Events);
        }

        [Fact]
        public async Task PostResponseInterceptor_IsCalledAfterResponse()
        {
            var interceptor = new RecordingInterceptor();
            var client = BuildClient(new List<IRequestInterceptor> { interceptor });

            await client.GetAsync("http://localhost/test");

            Assert.Contains("after:A", interceptor.Events);
        }

        [Fact]
        public async Task MultipleInterceptors_PreRunsFifo_PostRunsLifo()
        {
            var allEvents = new List<string>();

            var a = new CallbackInterceptor(
                before: _ => { allEvents.Add("before:A"); return Task.CompletedTask; },
                after: (_, __) => { allEvents.Add("after:A"); return Task.CompletedTask; });
            var b = new CallbackInterceptor(
                before: _ => { allEvents.Add("before:B"); return Task.CompletedTask; },
                after: (_, __) => { allEvents.Add("after:B"); return Task.CompletedTask; });

            var client = BuildClient(new List<IRequestInterceptor> { a, b });
            await client.GetAsync("http://localhost/test");

            Assert.Equal(new[] { "before:A", "before:B", "after:B", "after:A" }, allEvents);
        }

        [Fact]
        public async Task Interceptor_CanModifyRequestHeaders()
        {
            HttpRequestMessage capturedRequest = null;
            var interceptor = new CallbackInterceptor(
                before: req =>
                {
                    req.Headers.Add("X-Test-Header", "intercepted");
                    capturedRequest = req;
                    return Task.CompletedTask;
                });

            var client = BuildClient(new List<IRequestInterceptor> { interceptor });
            await client.GetAsync("http://localhost/test");

            Assert.NotNull(capturedRequest);
            Assert.True(capturedRequest.Headers.Contains("X-Test-Header"));
            Assert.Equal("intercepted", string.Join(",", capturedRequest.Headers.GetValues("X-Test-Header")));
        }

        [Fact]
        public async Task Interceptor_CanInspectResponse()
        {
            HttpResponseMessage capturedResponse = null;
            var inner = new FixedStatusHandler(HttpStatusCode.Accepted);
            var interceptor = new CallbackInterceptor(
                after: (response, _) =>
                {
                    capturedResponse = response;
                    return Task.CompletedTask;
                });

            var client = BuildClient(new List<IRequestInterceptor> { interceptor }, inner);
            await client.GetAsync("http://localhost/test");

            Assert.NotNull(capturedResponse);
            Assert.Equal(HttpStatusCode.Accepted, capturedResponse.StatusCode);
        }

        [Fact]
        public async Task BaseClass_WithOnlyBeforeOverridden_DoesNotThrow()
        {
            var beforeCalled = false;
            var interceptor = new BeforeOnlyInterceptor(() => beforeCalled = true);
            var client = BuildClient(new List<IRequestInterceptor> { interceptor });

            await client.GetAsync("http://localhost/test");

            Assert.True(beforeCalled);
        }

        [Fact]
        public async Task BaseClass_WithOnlyAfterOverridden_DoesNotThrow()
        {
            var afterCalled = false;
            var interceptor = new AfterOnlyInterceptor(() => afterCalled = true);
            var client = BuildClient(new List<IRequestInterceptor> { interceptor });

            await client.GetAsync("http://localhost/test");

            Assert.True(afterCalled);
        }

        // --- Helpers ---

        private sealed class CallbackInterceptor : IRequestInterceptor
        {
            private readonly System.Func<HttpRequestMessage, Task> _before;
            private readonly System.Func<HttpResponseMessage, HttpRequestMessage, Task> _after;

            public CallbackInterceptor(
                System.Func<HttpRequestMessage, Task> before = null,
                System.Func<HttpResponseMessage, HttpRequestMessage, Task> after = null)
            {
                _before = before ?? (_ => Task.CompletedTask);
                _after = after ?? ((_, __) => Task.CompletedTask);
            }

            public Task OnBeforeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
                => _before(request);

            public Task OnAfterResponseAsync(HttpResponseMessage response, HttpRequestMessage request, CancellationToken cancellationToken = default)
                => _after(response, request);
        }

        private sealed class FixedStatusHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode _statusCode;
            public FixedStatusHandler(HttpStatusCode statusCode) => _statusCode = statusCode;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(new HttpResponseMessage(_statusCode));
        }

        private sealed class BeforeOnlyInterceptor : RequestInterceptorBase
        {
            private readonly System.Action _callback;
            public BeforeOnlyInterceptor(System.Action callback) => _callback = callback;

            public override Task OnBeforeRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
            {
                _callback();
                return Task.CompletedTask;
            }
        }

        private sealed class AfterOnlyInterceptor : RequestInterceptorBase
        {
            private readonly System.Action _callback;
            public AfterOnlyInterceptor(System.Action callback) => _callback = callback;

            public override Task OnAfterResponseAsync(HttpResponseMessage response, HttpRequestMessage request, CancellationToken cancellationToken = default)
            {
                _callback();
                return Task.CompletedTask;
            }
        }
    }
}
