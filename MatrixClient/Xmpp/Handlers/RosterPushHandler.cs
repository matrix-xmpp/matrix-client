namespace MatrixClient.Xmpp.Handlers
{
    using AutoMapper;    

    using Matrix.Attributes;
    using Matrix.Network.Handlers;
    using Matrix.Xml;
    using Matrix.Xmpp;    
    using Matrix.Xmpp.Client;
    using Matrix.Xmpp.Roster;

    using ReactiveUI;

    using System.Linq;
    using System.Reactive.Linq;

    /// <summary>
    /// This handler handles incoming roster items (roster pushes)
    /// </summary>    
    [Name("RosterPush-Handler")]
    public class RosterPushHandler : XmppStanzaHandler
    {
        public RosterPushHandler(ViewModel.Contacts contacts, IMapper mapper)
        {
            // handle incoming subscription requests
            Handle(
                el =>
                    el.OfType<Iq>()
                    && el.Cast<Iq>().Query.OfType<Roster>()
                    && el.Cast<Iq>().Type == IqType.Set,                    

                async (context, xmppXElement) =>
                {
                    var iq = xmppXElement.Cast<Iq>();
                    var roster = iq.Query.Cast<Roster>();
                    var rItems = roster.GetRoster();
                    
                    await Observable.Start(() =>
                    {
                        foreach (var ri in rItems)
                        {
                            if (ri.Subscription == Subscription.Remove)
                            {
                                // remove
                                contacts.Remove(c => c.Jid == ri.Jid);
                            }
                            else
                            { 
                                // add or update
                                var contact = mapper.Map<ViewModel.Contact>(ri);
                                contacts.AddOrReplace(contact, c => c.Jid == contact.Jid);
                            }
                        }
                    }
                    ,RxApp.MainThreadScheduler);
                });
        }
    }
}
