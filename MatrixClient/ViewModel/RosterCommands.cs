namespace MatrixClient.ViewModel
{
    using System;
    using MatrixClient.Xmpp;
    using ReactiveUI;
    using System.Reactive.Linq;
    using Matrix.Extensions.Client.Roster;
    using Matrix.Extensions.Client.Subscription;

    public class RosterCommands
    {
        public ReactiveCommand RemoveContact { get; }
        public ReactiveCommand AddContact { get; }

        MyViewModel viewModel;        
        AddUser addUser;
        XmppClientEx xmppClient;

        public RosterCommands(
            XmppClientEx xmppClient,
            MyViewModel viewModel,
            AddUser addUser)
        {
            this.xmppClient = xmppClient;
            this.viewModel = viewModel;
            this.addUser = addUser;            

            RegexUtilities regexUtils = new RegexUtilities();

            var canAddUser = this
                          .WhenAnyValue(x => x.addUser.XmppId, x => x.viewModel.IsConnected,
                              (xmppId, isConnected) =>
                                  !String.IsNullOrWhiteSpace(xmppId)
                                  && regexUtils.IsValidXmppId(xmppId)
                                  && isConnected)
                          .DistinctUntilChanged();

            RemoveContact = ReactiveCommand.Create<Contact>(
                async contact =>
                {
                    await this.xmppClient.RemoveRosterItemAsync(new Matrix.Jid(contact.Jid));
                    await this.xmppClient.CancelSubscriptionAsync(contact.Jid);
                    await this.xmppClient.UnsubscribeAsync(contact.Jid);
                }
                , this.viewModel.IsConnectedObervalble
            );

            AddContact = ReactiveCommand.Create<AddUser>(
                 async user =>
                 {                     
                     await xmppClient.AddRosterItemAsync(user.XmppId, user.Name);
                     await xmppClient.SubscribeAsync(user.XmppId, user.Message);

                     this.addUser.Reset();
                 }
                 , canAddUser
            );
        }
    }
}
