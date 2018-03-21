namespace MatrixClient.Xmpp
{
    using AutoMapper;

    using Matrix;
    using Matrix.Xml;
    using Matrix.Xmpp.Roster;

    using MatrixClient;
    using MatrixClient.ViewModel;
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using MatrixJid = Matrix.Jid;
    using MatrixSrvResolver = Matrix.Srv.SrvNameResolver;
    using MatrixPresenceType = Matrix.Xmpp.PresenceType;
    using MatrixPresence = Matrix.Xmpp.Client.Presence;
    using MatrixMessage = Matrix.Xmpp.Client.Message;
    using MatriXMessageType = Matrix.Xmpp.MessageType;
    using ReactiveUI;

    using Microsoft.Extensions.DependencyInjection;
    using Matrix.Network.Resolver;
    using Matrix.Extensions.Client.Roster;

    public class XmppSession
    {
        private XmppClientEx xmppClient;
        private IMapper mapper;
        public MyViewModel viewModel;        
        private Contacts contacts;
        private Account account;
        private IPresenceManager presenceManager;
        private IAvatarStorage avatarStorage;
        private IUserAvatarStorage userAvatarStorage;

        public SessionState SessionState = SessionState.Disconnected;
        
        public XmppSession(
            XmppClientEx xmppClient,
            IMapper mapper,
            MyViewModel myViewModel,
            Contacts contacts, 
            IPresenceManager presenceManager,
            IAvatarStorage avatarStorage, 
            IUserAvatarStorage userAvatarStorage)
        {
            this.xmppClient = xmppClient;
            this.mapper = mapper;
            this.viewModel = myViewModel;            
            this.contacts = contacts;
            this.presenceManager = presenceManager;
            this.avatarStorage = avatarStorage;
            this.userAvatarStorage = userAvatarStorage;

            xmppClient.XmppSessionStateObserver.Subscribe(async state =>
            {
                if (state == SessionState.Disconnected && SessionState != SessionState.Disconnected)
                {
                    //Task.Run(() => DoReconnectAsync(Connect, TimeSpan.FromSeconds(30), 100));
                }

                await SetSessionState(state);                
                
                System.Diagnostics.Debug.WriteLine(state);
            });

            xmppClient
                .XmppXElementStreamObserver
                    .Where(el =>
                        el.OfType<MatrixPresence>()
                        && el.Cast<MatrixPresence>().Type == MatrixPresenceType.Available
                        && contacts.Any(c => c.Jid == el.Cast<MatrixPresence>().From.Bare)
                    )
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(el =>
                {
                    // received online presence from a contact
                    System.Diagnostics.Debug.WriteLine($"online presence from: {el.Cast<MatrixPresence>().From}");
                    var pres = el.Cast<MatrixPresence>();
                    var contact = contacts.FirstOrDefault(ct => ct.Jid == pres.From.Bare);
                    var viewPres = mapper.Map<ViewModel.Presence>(pres);
                    contact.Presences.AddOrReplace(viewPres, p => p.Resource == viewPres.Resource);
                });
            

            xmppClient
                .XmppXElementStreamObserver
                    .Where(el =>
                        el.OfType<MatrixPresence>()
                        && el.Cast<MatrixPresence>().Type == MatrixPresenceType.Unavailable
                        && el.Cast<MatrixPresence>().From != null
                        && el.Cast<MatrixPresence>().From.Resource != null
                        && contacts.Any(c => c.Jid == el.Cast<MatrixPresence>().From.Bare)
                    )
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(el =>
                {
                    // received offline presence from a contact
                    System.Diagnostics.Debug.WriteLine($"offline presence from: {el.Cast<MatrixPresence>().From}");
                    var pres = el.Cast<MatrixPresence>();
                    var contact = contacts.FirstOrDefault(ct => ct.Jid == pres.From.Bare);

                    contact.Presences.Remove(p => p.Resource == pres.From.Resource);
                });


            xmppClient
                .XmppXElementStreamObserver
                    .Where(el =>
                        el.OfType<MatrixMessage>()
                        && el.Cast<MatrixMessage>().Type == MatriXMessageType.Chat
                        && el.Cast<MatrixMessage>().From != null
                        && el.Cast<MatrixMessage>().Body != null
                    )
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(el =>
                {
                    var msg = el.Cast<MatrixMessage>();
                    Contact contact;

                    // handle incoming chat message
                    if (!viewModel.Chats.Any(c => c.Jid == msg.From.Bare))
                    {
                        if (contacts.Contains(msg.From.Bare))
                        {
                            contact = contacts[msg.From.Bare];
                        }
                        else
                        {
                            contact = new Contact(msg.From.Bare);
                        }
                        viewModel.AddChatMenu(contact, false);
                    }
                    else
                    {
                        contact = contacts[msg.From.Bare];
                    }

                    contact.LastMessageResource = msg.From.Resource;
                    var viewMessage = mapper.Map<Message>(msg);
                    viewMessage.Nickname = contact.Name;
                    viewMessage.IsSelf = false;
                  
                    if (msg.Id != null)
                    {
                        viewMessage.Id = msg.Id;
                    }

                    viewModel.GetChatMenu(msg.From.Bare).Messages.Add(viewMessage);

                });

            
            /*
             * <presence>
                <show>away</show>
                <status>Kann nicht chatten. Muss arbeiten.</status>
                <priority>40</priority>
                <c xmlns="http://jabber.org/protocol/caps" ver="r75qsgpbcZtkumlpddBcZZCeDco=" hash="sha-1" node="http://psi-im.org"/>
                <x xmlns="vcard-temp:x:update">
                    <photo>9b33b369b378be852b8487e23105dd4bd880a0f0</photo>
                </x>
                </presence>

             */
        }

        public async Task SetStatus(OnlineStatus status)
        {            
            await presenceManager.SendPresenceAsync(status);
        }

        public async Task Connect(Account account)
        {
            try
            {
                this.account = account;

                var jid = new MatrixJid(account.XmppId);

                // set username, xmpp domain and password
                xmppClient.Username = jid.User;
                xmppClient.XmppDomain = jid.Server;
                xmppClient.Password = account.Password;
                xmppClient.Resource = account.Resource;
                //xmppClient.Priority = 10;

                if (!account.SpecifyHostnameAndPort)
                {
                    xmppClient.HostnameResolver = new MatrixSrvResolver();
                }
                else
                {
                    xmppClient.HostnameResolver = new StaticNameResolver(account.Hostname, account.Port);
                }                

                await SetConnectionStatus("Connecting...");
                // Connect the XMPP session
                await xmppClient.ConnectAsync();
                

                await SetConnectionStatus("Getting contacts...");
                // request the contact list from the server
                await RequestRosterAsync();

                await SetConnectionStatus("Setting presence...");
                // send our presence to the server
                await presenceManager.SendPresenceAsync();

                await ResetConnectionStatus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task SetConnectionStatus(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);

            await Observable.Start(() => viewModel.ConnectionStatus = msg, RxApp.MainThreadScheduler);
        }

        private async Task SetSessionState(SessionState state)
        {
            this.SessionState = state;
            await Observable.Start(() => viewModel.IsConnected = (state != SessionState.Disconnected), RxApp.MainThreadScheduler);
        }

        private async Task ResetConnectionStatus()
        {
            await Observable.Start(() => viewModel.ConnectionStatus = String.Empty, RxApp.MainThreadScheduler);            
        }

        public async Task RequestRosterAsync()
        {
            var rosterIq = await xmppClient.RequestRosterAsync();
            var rosterItems
                = rosterIq
                   .Query
                   .Cast<Roster>()
                   .GetRoster();

            await Observable.Start(() =>
            {
                foreach (var ri in rosterItems)
                {
                    var contact = mapper.Map<Contact>(ri);

                    if (userAvatarStorage.HasUserAvatar(contact.Jid))
                    {
                        var hash = userAvatarStorage.GetUserAvatar(contact.Jid).Hash;
                        contact.AvatarBytes = avatarStorage.GetAvatar(hash)?.ImageBytes;
                    }
                   
                    contacts.AddOrReplace(contact, c => c.Jid == contact.Jid);
                }
            }, RxApp.MainThreadScheduler);
        }

     
        public async Task SendChatMessage(ChatItem chatItem)
        {
            var msg = new Message(true, true)
            {
                Text = chatItem.ComposeMessage,
                Nickname = account?.Nickname ?? "Foo",
                IsSelf = true
            };

            var toJid = new Matrix.Jid(chatItem.Contact.Jid);
            if (chatItem.Contact.MessageResource != null)
            {
                toJid.Resource = chatItem.Contact.MessageResource;
            }

            var xmppMsg = new MatrixMessage
            {
                Type = MatriXMessageType.Chat,
                To = toJid,
                Id = msg.Id,
                Body = chatItem.ComposeMessage
            };

            await xmppClient.SendAsync(xmppMsg);
            
            chatItem.Messages.Add(msg);

            // clear the textbox
            chatItem.ComposeMessage = string.Empty;          
        }

        public async Task Disconnect()
        {
            if (SessionState != SessionState.Disconnected)
            {
                try
                {
                    await xmppClient.DisconnectAsync();
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }

        public async Task DoReconnectAsync(Func<Task> action, TimeSpan sleepPeriod, int tryCount = 3)
        {
            // maybe we should use Polly for this: https://github.com/App-vNext/Polly/

            bool exit = false;

            while (!exit)
            {
                try
                {
                    await SetConnectionStatus("Trying to reconnect...");

                    await action();
                    return; // success!
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine($"reconnect failed: {tryCount}");
                    if (--tryCount == 0)
                    {
                        await SetConnectionStatus("Failed to reconnect, check your connection ...");
                        exit = true;
                        // Exceeded reconnect count
                        //throw;
                    }
                    else
                    {
                        await Task.Delay(sleepPeriod);
                    }
                }
            }
        }
    }
}
