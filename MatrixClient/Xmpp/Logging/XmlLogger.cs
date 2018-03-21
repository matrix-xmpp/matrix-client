using Microsoft.Extensions.Logging;
using System;

namespace MatrixClient.Xmpp.Logging
{
    public class XmlLogger : ILogger
    {
        private readonly string name;
        private readonly XmlLoggerConfiguration config;

        public XmlLogger(string name, XmlLoggerConfiguration config)
        {
            this.name = name;
            this.config = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            
            if (config.Enabled)
            {               
                config.logMessagesSubject.OnNext($"{formatter(state, exception)}");               
            }
        }
    }
}
