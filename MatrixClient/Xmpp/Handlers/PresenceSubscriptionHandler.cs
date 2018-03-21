namespace MatrixClient.Xmpp.Handlers
{
    using AutoMapper;    
    using Matrix.Attributes;
    using Matrix.Network.Handlers;
    using Matrix.Xml;
    using Matrix.Xmpp;    
    using Matrix.Xmpp.Client;
    using ReactiveUI;
    using System.Linq;
    using System.Reactive.Linq;

    /// <summary>
    /// This handler handles incoming presence subscription requests
    /// </summary>    
    [Name("PresenceSubscription-Handler")]
    public class PresenceSubscriptionHandler : XmppStanzaHandler
    {
        public PresenceSubscriptionHandler(ViewModel.SubscriptionRequests subscriptionRequests, IMapper mapper)
        {
            // handle incoming subscription requests
            Handle(
                el =>
                    el.OfType<Presence>()
                    && el.Cast<Presence>().From != null
                    && el.Cast<Presence>().Type == PresenceType.Subscribe,

                async (context, xmppXElement) =>
                {
                    var pres = xmppXElement.Cast<Presence>();
                    var subscriptionRequest = mapper.Map<ViewModel.SubscriptionRequest>(pres);
                    
                    await Observable.Start(() =>
                            subscriptionRequests.AddOrReplace(subscriptionRequest, sr => sr.Jid == subscriptionRequest.Jid),
                            RxApp.MainThreadScheduler);                    
                });
        }
    }
}
