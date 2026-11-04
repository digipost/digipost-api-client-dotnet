using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Digipost.Api.Client.Tests.TestSupport
{
    internal sealed class RecordingLoggerFactory : ILoggerFactory
    {
        private readonly ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

        public IReadOnlyList<string> Messages => _messages.ToArray();

        public ILogger CreateLogger(string categoryName)
        {
            return new RecordingLogger(_messages);
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
        }

        private sealed class RecordingLogger : ILogger
        {
            private readonly ConcurrentQueue<string> _messages;

            public RecordingLogger(ConcurrentQueue<string> messages)
            {
                _messages = messages;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= LogLevel.Debug;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                _messages.Enqueue(formatter(state, exception));
            }
        }
    }
}
