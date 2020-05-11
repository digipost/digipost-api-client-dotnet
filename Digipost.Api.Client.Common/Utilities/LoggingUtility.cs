using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Digipost.Api.Client.Common.Utilities
{
    public static class LoggingUtility
    {
        internal static IServiceProvider CreateServiceProviderAndSetUpLogging()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);

                builder.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
                NLog.LogManager.LoadConfiguration("./../../../../Digipost.Api.Client.Common/nlog.config");
            });

            return services.BuildServiceProvider();
        }
    }
}
