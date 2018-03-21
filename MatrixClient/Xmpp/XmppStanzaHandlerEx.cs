namespace MatrixClient.Xmpp.Handlers
{
    using Matrix;
    using Matrix.Network.Handlers;
    using Matrix.Xmpp.Client;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A XmppStanzaHandler which implement the <see cref="IClientIqSender"/> interface.
    /// This allows us to send packets from a stanza handler with the helper in Matrix.Extensions
    /// </summary>
    /// <typeparam name="T"></typeparam>  
    public abstract class XmppStanzaHandlerEx : XmppStanzaHandler, IClientIqSender
    {
        public async Task<Iq> SendIqAsync(Iq iq)
        {
            return await SendIqAsync(iq, XmppStanzaHandler.DefaultTimeout, CancellationToken.None);
        }

        public async Task<Iq> SendIqAsync(Iq iq, int timeout)
        {
            return await SendIqAsync(iq, timeout, CancellationToken.None);
        }

        public async Task<Iq> SendIqAsync(Iq iq, CancellationToken cancellationToken)
        {
            return await SendIqAsync(iq, XmppStanzaHandler.DefaultTimeout, cancellationToken);
        }

        public async Task<Iq> SendIqAsync(Iq iq, int timeout, CancellationToken cancellationToken)
        {
            return await SendIqAsync<Iq>(iq, timeout, cancellationToken);
        }
    }
}