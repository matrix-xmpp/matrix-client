namespace MatrixClient.ViewModel
{
    using ReactiveUI;

    using System.Collections.ObjectModel;    

    public abstract class ChatItemBase : ReactiveObject, IMenu
    {
        public ChatItemBase()
        {          
            this.Messages = new ObservableCollection<Message>();
        }

        private string text;
        private string jid;
        private int countUnread;
        string composeMessage;

        /// <inheritdoc/>
        public string SortString => $"{GroupId}{this.Text.ToLower()}";

        /// <inheritdoc/>
        public string Text
        {
            get { return text; }
            set { this.RaiseAndSetIfChanged(ref this.text, value); }
        }

        /// <inheritdoc/>        
        public int GroupId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the count of iunread messages
        /// </summary>
        public int CountUnread
        {
            get { return countUnread; }
            set
            {
                this.RaiseAndSetIfChanged(ref this.countUnread, value);
                this.RaisePropertyChanged(nameof(ShowUnreadCounter));
            }
        }

        /// <summary>
        /// Gets a boolean which indicated whether the unread count shjould be shows or not.
        /// </summary>
        public bool ShowUnreadCounter => CountUnread > 0;

        /// <summary>
        /// Gets or sets the JabberId
        /// </summary>
        public string Jid
        {
            get { return jid; }
            set { this.RaiseAndSetIfChanged(ref this.jid, value); }
        }

        /// <summary>
        /// Gets or sets the text which is composed
        /// </summary>
        public string ComposeMessage
        {
            get { return composeMessage; }
            set { this.RaiseAndSetIfChanged(ref this.composeMessage, value); }
        }

        /// <inhertitdoc />
        public bool Focusable => true;
        
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();        
    }
}
