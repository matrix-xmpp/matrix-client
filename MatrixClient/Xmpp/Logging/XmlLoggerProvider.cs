using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace MatrixClient.Xmpp.Logging
{
    public class XmlLoggerProvider : ILoggerProvider
    {
        public XmlLoggerConfiguration Configuration { get; set; }
        private readonly ConcurrentDictionary<string, XmlLogger> _loggers = new ConcurrentDictionary<string, XmlLogger>();

        public XmlLoggerProvider(XmlLoggerConfiguration config)
        {
            Configuration = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new XmlLogger(name, Configuration));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
