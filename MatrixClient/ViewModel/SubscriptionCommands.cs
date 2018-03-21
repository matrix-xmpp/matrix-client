namespace MatrixClient.ViewModel
{
    using Matrix.Extensions.Client.Roster;
    using Matrix.Extensions.Client.Subscription;
    using MatrixClient.Xmpp;
    using ReactiveUI;
    using System.Reactive.Linq;    

    public class SubscriptionCommands
    {
        public ReactiveCommand ApproveSubscriptionRequest { get; }
        public ReactiveCommand DenySubscriptionRequest { get; }

        MyViewModel viewModel;
        Contacts contacts;
        XmppClientEx xmppClient;
        SubscriptionRequests SubscriptionRequests;

        public SubscriptionCommands(
            Contacts contacts,
            XmppClientEx xmppClient,
            MyViewModel viewModel,
            SubscriptionRequests subscriptionRequests)
        {
            this.contacts = contacts;
            this.xmppClient = xmppClient;
            this.viewModel = viewModel;
            this.SubscriptionRequests = subscriptionRequests;

            ApproveSubscriptionRequest = ReactiveCommand.Create<SubscriptionRequest>(
                async sr =>
                {                    
                    await xmppClient.ApproveSubscriptionRequestAsync(sr.Jid);
                    if (!contacts.Contains(sr.Jid))
                    {
                        // we approved a contact which is not on our contact list
                        // => add it now
                        await xmppClient.AddRosterItemAsync(sr.Jid, sr.Name);
                        await xmppClient.SubscribeAsync(sr.Jid);
                    }
                    else
                    {
                        // handle from and none
                        var contact = contacts[sr.Jid];
                        if (contact.Subscription == Subscription.From
                            || contact.Subscription == Subscription.None)
                        {
                            await xmppClient.SubscribeAsync(sr.Jid);
                        }
                    }
                    this.SubscriptionRequests.Remove(sr);                    
                }
                , this.viewModel.IsConnectedObervalble
            );

            DenySubscriptionRequest = ReactiveCommand.Create<SubscriptionRequest>(
                 async sr =>
                 {                     
                    await xmppClient.DenySubscriptionRequestAsync(sr.Jid);
                    SubscriptionRequests.Remove(sr);                 
                 }
                 , this.viewModel.IsConnectedObervalble
            );
        }
    }
}
