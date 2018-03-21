namespace MatrixClient.ViewModel
{
    using ReactiveUI;
    using System.Reactive.Linq;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Linq;

    public class MenuItems : ObservableCollection<IMenu>
    {
        public MenuItems()
        {
            Add(new RoomsHeaderItem());
            Add(new ChatsHeaderItem());


            Add(new GroupChatItem() { Text = "jdev" });
            Add(new GroupChatItem() { Text = "test", CountUnread = 11 });
            Add(new GroupChatItem() { Text = "jabber" });
        }

        //IMenu selectedChat;

        //public IMenu SelectedChat
        //{
        //    get { return selectedChat; }
        //    set
        //    {
        //        if (value is ChatItemBase)
        //        {
        //            ChatsVisible = true;
        //            this.RaiseAndSetIfChanged(ref this.selectedChat, value);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Adds a chatmenu item for the seleted contact
        ///// </summary>
        ///// <param name="contact"></param>
        //public void AddChatMenu(Contact contact, bool select = true)
        //{
        //    var newChat = ChatItem.CreateFromContact(contact);
        //    Add(newChat);

        //    if (select)
        //    {
        //        SelectedChat = newChat;
        //    }
        //}

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
        public IReactiveDerivedList<IMenu> Conversations => this.CreateDerivedCollection(c => c, c => c is ChatItemBase);

        /// <summary>
        /// Gets all active Chats (1:1)
        /// </summary>
        public IReactiveDerivedList<ChatItem> Chats => this.CreateDerivedCollection(c => c as ChatItem, c => c is ChatItem);

        /// <summary>
        /// Gets all active ChatRooms
        /// </summary>
        public IReactiveDerivedList<GroupChatItem> Rooms => this.CreateDerivedCollection(c => c as GroupChatItem, c => c is GroupChatItem);

        public IReactiveDerivedList<IMenu> SortedMenuItems => this.CreateDerivedCollection(c => c, c => true, (x, y) => x.SortString.CompareTo(y.SortString));
    }
}
