namespace MatrixClient.ViewModel
{
    using ReactiveUI;

    /// <summary>
    /// Represemts an object for adding a new User
    /// </summary>
    public class AddUser : ReactiveObject
    {
        string xmppId;
        string name;
        string message;

        public ReactiveCommand AddUserCommand { get; }

        /// <summary>
        /// Gets or sets the XMPP id
        /// </summary>
        public string XmppId
        {
            get { return xmppId; }
            set { this.RaiseAndSetIfChanged(ref this.xmppId, value); }
        }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref this.name, value); }
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message
        {
            get { return message; }
            set { this.RaiseAndSetIfChanged(ref this.message, value); }
        }

        /// <summary>
        /// Resets teh object to defaults
        /// </summary>
        public void Reset()
        {
            this.XmppId = string.Empty;
            this.Name = string.Empty;
            this.Message = string.Empty;
        }
    }
}
