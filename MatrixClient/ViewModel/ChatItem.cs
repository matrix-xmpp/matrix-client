namespace MatrixClient.ViewModel
{
    /// <summary>
    /// Represents a 1:1 chat
    /// </summary>
    public class ChatItem : ChatItemBase
    {    
        /// <summary>
        /// Construct a new <see cref="ChatItem"/> with the given contact
        /// </summary>
        /// <param name="contact"></param>
        public ChatItem()
        {          
        }

        public static ChatItem CreateFromContact(Contact contact)
        {
            var ci = new ChatItem();
            ci.Contact = contact;
            ci.Text = contact.Name;
            ci.Jid = contact.Jid;

            return ci;
        }


        public Contact Contact { get; internal set; }       
    }
}
