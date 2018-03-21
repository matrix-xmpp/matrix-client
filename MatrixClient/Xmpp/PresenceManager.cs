namespace MatrixClient.Xmpp
{
    using AutoMapper;
    using Matrix;
    using Matrix.Extensions.Client.Presence;
    using Matrix.Xmpp.Client;
    
    using System.Threading.Tasks;

    public class PresenceManager : IPresenceManager
    {
        ICaps clientCaps;
        XmppClient xmppClient;
        IMapper mapper;

        public PresenceManager(XmppClientEx xmppClient, ICaps clientCaps, IMapper mapper)
        {
            this.clientCaps = clientCaps;
            this.xmppClient = xmppClient;
            this.mapper = mapper;
        }

        public async Task SendPresenceAsync()
        {
            var pres = new Presence();
            pres.Add(clientCaps.ClientCapabilities);

            await xmppClient.SendPresenceAsync(pres);
        }

        public async Task SendPresenceAsync(ViewModel.OnlineStatus status)
        {
            var pres = new Presence
            {
                Show = mapper.Map<Matrix.Xmpp.Show>(status)
            };
            pres.Add(clientCaps.ClientCapabilities);
            await xmppClient.SendPresenceAsync(pres);
        }
    }
}
