namespace MatrixClient.ViewModel
{
    using ReactiveUI;    

    public class SubscriptionRequest : ReactiveObject
    {       
        string jid;
        string name;
        string message;

        public SubscriptionRequest()
        {            
        }

        /// <summary>
        /// Gets or sets the XMPP id
        /// </summary>
        public string Jid
        {
            get { return jid; }
            set { this.RaiseAndSetIfChanged(ref this.jid, value); }
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref this.name, value); }
        }

        /// <summary>
        /// Gets or sets a message which can be included with a subscription request
        /// </summary>
        public string Message
        {
            get { return message; }
            set { this.RaiseAndSetIfChanged(ref this.message, value); }
        }
    }
}
