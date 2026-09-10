using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Digipost.Api.Client.Tests.TestSupport
{
    internal sealed class MtlsTestServer : IAsyncDisposable
    {
        private readonly WebApplication _app;
        private readonly ConcurrentQueue<RecordedRequest> _recordedRequests;

        private MtlsTestServer(WebApplication app, Uri baseAddress, ConcurrentQueue<RecordedRequest> recordedRequests)
        {
            _app = app;
            BaseAddress = baseAddress;
            _recordedRequests = recordedRequests;
        }

        public Uri BaseAddress { get; }

        public IReadOnlyList<RecordedRequest> RecordedRequests => _recordedRequests.ToList();

        public static async Task<MtlsTestServer> StartAsync(X509Certificate2 serverCertificate, Func<HttpContext, Task> respond, ClientCertificateMode clientCertificateMode = ClientCertificateMode.RequireCertificate)
        {
            var recordedRequests = new ConcurrentQueue<RecordedRequest>();

            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();
            builder.WebHost.UseKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 0, listenOptions =>
                {
                    listenOptions.UseHttps(serverCertificate, httpsOptions =>
                    {
                        httpsOptions.ClientCertificateMode = clientCertificateMode;
                        httpsOptions.ClientCertificateValidation = (_, _, _) => true;
                    });
                });
            });

            var app = builder.Build();

            app.Run(async context =>
            {
                var clientCertificate = await context.Connection.GetClientCertificateAsync();

                string body;
                using (var reader = new StreamReader(context.Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                var recorded = new RecordedRequest
                {
                    Method = context.Request.Method,
                    Path = context.Request.Path,
                    Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    Body = body,
                    ClientCertificate = clientCertificate
                };
                recordedRequests.Enqueue(recorded);

                await respond(context);
            });

            await app.StartAsync();

            var server = app.Services.GetRequiredService<IServer>();
            var addressesFeature = server.Features.Get<IServerAddressesFeature>();
            var address = addressesFeature.Addresses.First();
            if (!address.EndsWith("/"))
            {
                address += "/";
            }

            return new MtlsTestServer(app, new Uri(address), recordedRequests);
        }

        public async ValueTask DisposeAsync()
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}
