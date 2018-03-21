namespace MatrixClient.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Linq;
    using MatrixClient.Xmpp;
    using ReactiveUI;
    using Microsoft.Extensions.DependencyInjection;

    public class MyViewModel : ReactiveObject
    {        
        public ReactiveCommand ShowContacts { get; }
        public ReactiveCommand ShowAddContact { get; }
        public ReactiveCommand ShowJoinRoom { get; }        
        public ReactiveCommand ShowLogin { get; }
        public ReactiveCommand HideContacts { get; }
        public ReactiveCommand CloseChat { get; }
        public ReactiveCommand CloseGroupChat { get; }        
        public ReactiveCommand ShowMyProfile { get; }

        public ReactiveCommand SendChat { get; }

        public MyViewModel()
        {
            MenuItems = new ObservableCollection<IMenu>();

            MenuItems.Add(new RoomsHeaderItem());
            MenuItems.Add(new ChatsHeaderItem());


            MenuItems.Add(new GroupChatItem() { Text = "jdev" });
            MenuItems.Add(new GroupChatItem() { Text = "test" , CountUnread = 11});
            MenuItems.Add(new GroupChatItem() { Text = "jabber" });
      

            // setup commands
            ShowContacts = ReactiveCommand.Create(() => {
                ChatsVisible = false;
                SelectedPageIndex = ContentPages.ContactsPage;
                }
                , this.IsConnectedObervalble
            );

            ShowAddContact = ReactiveCommand.Create(() => {
                ChatsVisible = false;
                SelectedPageIndex = ContentPages.AddContactPage;
            }
           //, this.WhenAnyValue(x => x.IsConnected)
           );

            ShowJoinRoom = ReactiveCommand.Create(() => {
                    ChatsVisible = false;
                    SelectedPageIndex = ContentPages.JoinRoomPage;
                }
                , this.IsConnectedObervalble
            );

            HideContacts = ReactiveCommand.Create(() => ChatsVisible = true);
            CloseChat = ReactiveCommand.Create<ChatItem>(r => MenuItems.Remove(r));
            CloseGroupChat = ReactiveCommand.Create<GroupChatItem>(r => MenuItems.Remove(r));

            ShowLogin = ReactiveCommand.Create(() => {
                ChatsVisible = false;
                SelectedPageIndex = ContentPages.LoginPage;
            });

            ShowMyProfile = ReactiveCommand.Create(() => {
                ChatsVisible = false;
                SelectedPageIndex = ContentPages.ProfilePage;
            });

          

            SendChat = ReactiveCommand.Create<ChatItem>(async ci => {                
                await ServiceLocator.Current.GetService<XmppSession>().SendChatMessage(ci);
            });
        }

       
        public ObservableCollection<IMenu> MenuItems { get; }

        bool isConnected = false;
        bool chatsVisible = false;
        IMenu selectedChat;
        int selectedPageIndex = ContentPages.LoginPage;
        string connectionStatus = string.Empty;
        bool showConnectionStatus;

        
        public IObservable<bool> IsConnectedObervalble => this.WhenAnyValue(x => x.IsConnected, (connected) => connected == true).DistinctUntilChanged();

        public bool IsConnected
        {
            get { return isConnected; }
            set { this.RaiseAndSetIfChanged(ref this.isConnected, value); }
        }       
        

        public bool ChatsVisible
        {
            get { return chatsVisible; }
            set { this.RaiseAndSetIfChanged(ref this.chatsVisible, value); }
        }
       
        public IMenu SelectedChat
        {
            get { return selectedChat; }
            set
            {
                if (value is ChatItemBase)
                {
                    ChatsVisible = true;
                    this.RaiseAndSetIfChanged(ref this.selectedChat, value);
                }                              
            }
        }

        public int SelectedPageIndex
        {
            get { return selectedPageIndex; }
            set { this.RaiseAndSetIfChanged(ref this.selectedPageIndex, value); }
        }

        public string ConnectionStatus
        {
            get { return connectionStatus; }
            set
            {
                this.RaiseAndSetIfChanged(ref this.connectionStatus, value);
                ShowConnectionStatus = !string.IsNullOrEmpty(value);
            }
        }

        public bool ShowConnectionStatus
        {
            get { return showConnectionStatus; }
            set { this.RaiseAndSetIfChanged(ref this.showConnectionStatus, value); }
        }        

        /// <summary>
        /// Adds a chatmenu item for the seleted contact
        /// </summary>
        /// <param name="contact"></param>
        public void AddChatMenu(Contact contact, bool select = true)
        {
            var newChat = ChatItem.CreateFromContact(contact);
            MenuItems.Add(newChat);

            if (select)
            {
                SelectedChat = newChat;
            }
        }

        /// <summary>
        /// Gets a chat menuitem byjid
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public ChatItem GetChatMenu(string jid)
        {            
            return Chats.FirstOrDefault(c => c.Jid == jid);            
        }

        /// <summary>
        /// Gets all active conversation
        /// </summary>
        public IReactiveDerivedList<IMenu> Conversations => MenuItems.CreateDerivedCollection(c => c, c => c is ChatItemBase);

        /// <summary>
        /// Gets all active Chats (1:1)
        /// </summary>
        public IReactiveDerivedList<ChatItem> Chats => MenuItems.CreateDerivedCollection(c => c as ChatItem, c => c is ChatItem);

        /// <summary>
        /// Gets all active ChatRooms
        /// </summary>
        public IReactiveDerivedList<GroupChatItem> Rooms => MenuItems.CreateDerivedCollection(c => c as GroupChatItem, c => c is GroupChatItem);

        public IReactiveDerivedList<IMenu> SortedMenuItems => MenuItems.CreateDerivedCollection(c => c, c => true, (x,y) => x.SortString.CompareTo(y.SortString));        
    }
}
