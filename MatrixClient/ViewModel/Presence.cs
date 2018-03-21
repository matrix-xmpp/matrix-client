namespace MatrixClient.ViewModel
{
    using ReactiveUI;

    /// <summary>
    /// Represents a XMPP presence resource object
    /// </summary>
    public class Presence : ReactiveObject
    {
        private OnlineStatus onlineStatus = OnlineStatus.Offline;
        private string resource;
        private string statusText;
        private int priority;
        
        /// <summary>
        /// Gets or sets the resource for this presence
        /// </summary>
        public string Resource
        {
            get { return resource; }
            set { this.RaiseAndSetIfChanged(ref this.resource, value); }
        }

        /// <summary>
        /// Gets or sets onine status for this resource
        /// </summary>
        public OnlineStatus OnlineStatus
        {
            get { return onlineStatus; }
            set { this.RaiseAndSetIfChanged(ref this.onlineStatus, value); }
        }

        /// <summary>
        /// Gets or sets the status text for this resource
        /// </summary>
        public string StatusText
        {
            get { return statusText; }
            set { this.RaiseAndSetIfChanged(ref this.statusText, value); }
        }

        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        public int Priority
        {
            get { return priority; }
            set { this.RaiseAndSetIfChanged(ref this.priority, value); }
        }
    }
}
