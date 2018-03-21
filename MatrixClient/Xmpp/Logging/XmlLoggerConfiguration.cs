using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Subjects;

namespace MatrixClient.Xmpp.Logging
{
    public class XmlLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;
        //public IList<string> LogMessages { get; set; }

        public bool Enabled { get; set; }

        internal readonly ISubject<string> logMessagesSubject = new Subject<string>();

        public IObservable<string> LogMessages => logMessagesSubject;
    }
}