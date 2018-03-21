namespace MatrixClient.Xmpp.Handlers
{
    using Matrix;
    using Matrix.Attributes;
    using Matrix.Crypt;
    using Matrix.Network.Handlers;
    using Matrix.Xml;
    using Matrix.Xmpp;
    using Matrix.Xmpp.Avatar;
    using Matrix.Xmpp.Client;
    using Matrix.Xmpp.PubSub.Event;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// This handler collects caps information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Name("AvatarCollection-Handler")]
    public class AvatarCollectionHandler : XmppStanzaHandler
    {
        private IAvatarStorage avatarStorage;
        private IUserAvatarStorage userAvatarStorage;
        private ViewModel.Contacts contacts;

        public AvatarCollectionHandler(
            IAvatarStorage avatarStorage,
            IUserAvatarStorage userAvatarStorage, 
            ViewModel.Account account, 
            ViewModel.Contacts contacts)
        {
            this.avatarStorage = avatarStorage;
            this.userAvatarStorage = userAvatarStorage;
            this.contacts = contacts;

            Handle(
                el =>
                    el.OfType <Message>()
                    && el.HasDescendants<Event>()
                    && el.HasDescendants<Metadata>(),

                async (context, el) =>
                {
                    var msg = el.Cast<Message>();
                    var metaData = el.Descendants<Metadata>().First();
                    if (metaData.Info != null)
                    {
                        var id = metaData.Info.Id;
                        var from = msg.From.Bare;

                        if (!avatarStorage.HasAvatar(id))
                        {
                            await RequestAvatar(from, id);
                        }
                        
                        if (contacts.Contains(from))
                        {
                            // avatar of a contact
                            contacts[from].AvatarBytes = avatarStorage.GetAvatar(id)?.ImageBytes;
                        }
                        else if(account.XmppId == from)
                        {
                            //our own avatar
                            account.AvatarBytes = avatarStorage.GetAvatar(id)?.ImageBytes;
                        }
                    }
                });
        }

        private async Task RequestAvatar(Jid from, string id)
        {
            try
            {
                var pIq = AvatarManager.CreateRequestAvatarStanza(from, id);

                var ret = await SendIqAsync<Iq>(pIq);
                if (ret.Type == IqType.Result)
                {
                    if (ret.HasDescendants<Data>())
                    {
                        var data = ret.Descendants<Data>().FirstOrDefault();
                        var avatarBytes = data.Bytes;
                        var hash = Hash.Sha1HashHex(avatarBytes);
                        // check if the Hash is correct
                        if (hash == id)
                        {
                            StoreAvatar(id, avatarBytes);
                            StoreUserAvatar(from, id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// Store the avatar in the database
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="bytes"></param>
        private void StoreAvatar(string hash, byte[] bytes)
        {
            avatarStorage.AddAvatar(hash, bytes);
        }        

        /// <summary>
        /// Store the avatar hash for the given jid in the database
        /// </summary>
        /// <param name="from"></param>
        /// <param name="hash"></param>
        private void StoreUserAvatar(Jid from, string hash)
        {
            if (userAvatarStorage.GetUserAvatar(from) == null)
            {
                // no records for Jid => store new record
                userAvatarStorage.AddUserAvatar(from, hash);
            }
            else
            {
                if (userAvatarStorage.GetUserAvatar(from).Hash != hash)
                {
                    // records exists, but with a different hash => update
                    userAvatarStorage.ReplaceUserAvatar(from, hash);
                }
            }
        }
    }
}
 