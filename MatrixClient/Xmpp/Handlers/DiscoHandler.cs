namespace MatrixClient.Xmpp.Handlers
{
    using Matrix.Attributes;
    using Matrix.Network.Handlers;
    using Matrix.Xml;
    using Matrix.Xmpp;
    using Matrix.Xmpp.Client;
    using Matrix.Xmpp.Disco;

    /// <summary>
    /// This handler automatically replies to incoming XMPP Pings from clients or servers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Name("Disco-Handler")]
    public class DiscoHandler : XmppStanzaHandler
    {
        public DiscoHandler(ICaps caps, ICaps clientCaps)
        {
            Handle(
                el =>
                    el.OfType<Iq>()
                    && el.Cast<Iq>().Type == IqType.Get
                    && el.Cast<Iq>().Query.OfType<Info>(),
                   
                async (context, xmppXElement) =>
                {
                    var iq = xmppXElement.Cast<Iq>();
                    var info = xmppXElement.Cast<Iq>().Query.Cast<Info>();

                    if (info.Node == null || info.Node == caps.Node + "#" + clientCaps.CapsHash)
                    {
                        var resIq = new Iq()
                        {
                            Id = iq.Id,
                            To = iq.From,
                            Type = IqType.Result
                        };

                        resIq.Add(clientCaps.DiscoInfo);

                        await SendAsync(resIq);
                    }
                    else
                    {
                        var dIq = new DiscoInfoIq
                        {
                            Type = IqType.Error,
                            To = iq.From,
                            Id = iq.Id,
                            Info = { Node = info.Node },
                            Error = new Error(Matrix.Xmpp.Base.ErrorCondition.ItemNotFound)
                        };

                        await SendAsync(dIq);
                    }
                });
        }
    }
}