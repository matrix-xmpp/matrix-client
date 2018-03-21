namespace MatrixClient.ViewModel
{
    using MatrixClient.Xmpp;
    using ReactiveUI;
    using System;
    using System.Reactive.Linq;
    using System.Security;

    public class Commands
    {
        public ReactiveCommand ConnectCommand { get; }
        public ReactiveCommand SetStatusCommand { get; }

        MyViewModel viewModel;
        Account account;
        XmppSession xmppSession;        

        public Commands(XmppSession xmppSession, MyViewModel viewModel, Account account)
        {
            this.viewModel = viewModel;
            this.account = account;
            this.xmppSession = xmppSession;

            RegexUtilities regexUtils = new RegexUtilities();

            // create an observable which is used to enable/disable the login command
            // based on the user input for the credentials and the xmpp connection status
            var canLogin = this
                            .WhenAnyValue(x => x.account.XmppId, x => x.account.SecurePassword, x => x.viewModel.IsConnected,
                                (xmppId, pass, isConnected) =>
                                    !String.IsNullOrWhiteSpace(xmppId)
                                    && regexUtils.IsValidXmppId(xmppId)
                                    && pass?.Length > 0
                                    && !isConnected)
                            .DistinctUntilChanged();

           
            ConnectCommand = ReactiveCommand.Create<Account>(
                async a =>
                {
                    await Observable.Start(async () => await xmppSession.Connect(a),
                    RxApp.TaskpoolScheduler);
                }
                ,canLogin
            );

            SetStatusCommand = ReactiveCommand.Create<OnlineStatus>(
                 async status => await xmppSession.SetStatus(status),
                 this.viewModel.IsConnectedObervalble
            );
        }
    }
}
