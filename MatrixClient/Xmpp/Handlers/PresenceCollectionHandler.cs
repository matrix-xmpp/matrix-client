namespace MatrixClient.Xmpp.Handlers
{
    using AutoMapper;
    using Matrix;
    using Matrix.Attributes;
    using Matrix.Network.Handlers;
    using Matrix.Xml;
    using Matrix.Xmpp;    
    using Matrix.Xmpp.Client;
    using ReactiveUI;
    using System.Linq;
    using System.Reactive.Linq;

    /// <summary>
    /// 
    /// </summary>    
    [Name("PresenceCollection-Handler")]
    public class PresenceCollectionHandler : XmppStanzaHandler
    {
        public PresenceCollectionHandler(ViewModel.Account account, IMapper mapper)
        {

            // handle self presence
            Handle(
                el =>
                    el.OfType<Presence>()
                    && el.Cast<Presence>().Type == PresenceType.Available
                    && el.Cast<Presence>().From.Equals(account.XmppId, new BareJidComparer()),

                async (context, xmppXElement) =>
                {
                    var pres = xmppXElement.Cast<Presence>();
                    if (pres.From.Resource == account.Resource)
                    {
                        // this is our client
                        await Observable.Start(() =>
                            account.Presence = mapper.Map<ViewModel.Presence>(pres),
                            RxApp.MainThreadScheduler);
                    }
                });
        }
    }
}
