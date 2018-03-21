namespace MatrixClient.ViewModel
{
    using ReactiveUI;   

    using MoreLinq;

    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    /// <summary>
    /// Represents a Contact aka xmpp roster item
    /// </summary>
    public class Contact : ReactiveObject
    {        
        string name;
        string jid;
        string avatarUri;
        byte[] avatarBytes;

        public Contact()
        {
            Presences = new ObservableCollection<Presence>();

            Observable
               .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                       cc => Presences.CollectionChanged += cc,
                       cc => Presences.CollectionChanged -= cc)
                 .Subscribe(cc =>
                 {
                     this.RaisePropertyChanged(nameof(OnlineStatus));
                     this.RaisePropertyChanged(nameof(StatusText));
                 });
        }        

        public Contact(string jid)
        {
            Jid = jid;
        }

        public Contact(string jid, string name) : this(jid, name, false)
        { 
        }

        public Contact(string jid, string name, bool isConference) : this()
        {
            Jid = jid;
            Name = name;
            IsConference = isConference;
        }

        /// <summary>
        /// Gets the collection of presences for this contact
        /// </summary>
        public ObservableCollection<Presence> Presences { get; }

        /// <summary>
        /// Gets the online status of the most available resource
        /// </summary>
        public OnlineStatus OnlineStatus => MostAvailablePresence?.OnlineStatus ?? OnlineStatus.Offline;

        /// <summary>
        /// Gets the status text of the most available resource
        /// </summary>
        public string StatusText => MostAvailablePresence?.StatusText ?? string.Empty;

        /// <summary>
        /// Gets the "most available presence" based on max priority
        /// </summary>
        public Presence MostAvailablePresence => Presences.Any() ? Presences.MaxBy(p => p.Priority) : null;

        /// <summary>
        /// Gets or sets the resource we got the last message from for this contact
        /// </summary>
        public string LastMessageResource { get; set; } = null;

        /// <summary>
        /// Gets the resource messages should go to
        /// </summary>
        public string MessageResource
        {
            get
            {
                if (LastMessageResource != null)
                {
                    if (Presences.Any(p => p.Resource == LastMessageResource))
                    {
                        return LastMessageResource;
                    }
                    else
                    {
                        return null;
                    }
                        
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get { return name ?? jid; }
            set { this.RaiseAndSetIfChanged(ref this.name, value); }
        }

        /// <summary>
        /// Gets or sets the Jabber id
        /// </summary>
        public string Jid
        {
            get { return jid; }
            set { this.RaiseAndSetIfChanged(ref this.jid, value); }
        }

        /// <summary>
        /// Gets or sets the Avatar Uri
        /// </summary>
        public string AvatarUri
        {
            get { return avatarUri; }
            set { this.RaiseAndSetIfChanged(ref this.avatarUri, value); }
        }

        /// <summary>
        /// Gets or set the bytes of the avatar image
        /// </summary>
        public byte[] AvatarBytes
        {
            get { return avatarBytes; }
            set { this.RaiseAndSetIfChanged(ref this.avatarBytes, value); }
        }
      
        public bool IsConference { get; private set; }

        /// <summary>
        /// Gets or sets the subscription state
        /// </summary>
        public string Subscription { get; set; }
    }
}
