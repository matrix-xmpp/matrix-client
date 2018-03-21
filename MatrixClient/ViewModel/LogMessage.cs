
namespace MatrixClient.ViewModel
{
    using System;

    /// <summary>
    /// Represents a log message for the debug view
    /// </summary>
    public class LogMessage
    {
        DateTime dateTime;

        public LogMessage(string msg)
        {
            Message = msg;
            this.dateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the timestamp of the message
        /// </summary>
        public string TimeStamp => dateTime.ToShortTimeString();
    }
}
