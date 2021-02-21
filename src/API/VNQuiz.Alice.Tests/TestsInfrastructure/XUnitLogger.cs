using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace VNQuiz.Alice.Tests.TestsInfrastructure
{
    internal class XUnitLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IMessageSink _messageSink;
        private readonly LoggerExternalScopeProvider _scopeProvider;

        public static ILogger CreateLogger(IMessageSink messageSink) 
            => new XUnitLogger(messageSink, new LoggerExternalScopeProvider(), "");
        public static ILogger<T> CreateLogger<T>(IMessageSink messageSink) 
            => new XUnitLogger<T>(messageSink, new LoggerExternalScopeProvider());

        public XUnitLogger(IMessageSink messageSink, LoggerExternalScopeProvider scopeProvider, string categoryName)
        {
            _messageSink = messageSink;
            _scopeProvider = scopeProvider;
            _categoryName = categoryName;
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var sb = new StringBuilder();
            sb.Append(GetLogLevelString(logLevel))
              .Append(" [").Append(_categoryName).Append("] ")
              .Append(formatter(state, exception));

            if (exception != null)
            {
                sb.Append('\n').Append(exception);
            }

            // Append scopes
            _scopeProvider.ForEachScope((scope, state) =>
            {
                state.Append("\n => ");
                state.Append(scope);
            }, sb);

            var message = new DiagnosticMessage(sb.ToString());
            _messageSink.OnMessage(message);
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "trce",
                LogLevel.Debug => "dbug",
                LogLevel.Information => "info",
                LogLevel.Warning => "warn",
                LogLevel.Error => "fail",
                LogLevel.Critical => "crit",
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
            };
        }
    }

    internal sealed class XUnitLogger<T> : XUnitLogger, ILogger<T>
    {
        public XUnitLogger(IMessageSink messageSink, LoggerExternalScopeProvider scopeProvider)
            : base(messageSink, scopeProvider, typeof(T).FullName)
        {
        }
    }

    internal sealed class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly LoggerExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();
        private readonly IMessageSink _messageSink;

        public XUnitLoggerProvider(IMessageSink messageSink)
        {
            _messageSink = messageSink;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(_messageSink, _scopeProvider, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
