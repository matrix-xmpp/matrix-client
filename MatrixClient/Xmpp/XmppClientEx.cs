namespace MatrixClient.Xmpp
{
    using System;

    using DotNetty.Transport.Channels;

    using Matrix;
    using Matrix.Network.Handlers;
    using MatrixClient.Xmpp.Handlers;
    using MatrixClient.Xmpp.Logging;

    using Microsoft.Extensions.Logging;    
    
    public class XmppClientEx : XmppClient
    {
        public XmppClientEx(ILogger logger,
                            DiscoHandler discoHandler, 
                            CapsCollectionHandler capsCollectionHandler,
                            AvatarCollectionHandler avatarCollectionHandler,
                            PresenceCollectionHandler presenceCollectionHandler,
                            PresenceSubscriptionHandler presenceSubscriptionHandler,
                            RosterPushHandler rosterPushHandler)
            : base(new Action<IChannelPipeline>(
                pipeline =>
                    pipeline
                        .AddFirst(new XmlLoggingHandler(logger))
                        .AddBefore<XmppStanzaHandler>(discoHandler)
                        .AddBefore<XmppStanzaHandler>(capsCollectionHandler)
                        .AddBefore<XmppStanzaHandler>(avatarCollectionHandler)
                        .AddBefore<XmppStanzaHandler>(presenceCollectionHandler)
                        .AddBefore<XmppStanzaHandler>(presenceSubscriptionHandler)
                        .AddBefore<XmppStanzaHandler>(rosterPushHandler)
                ))
        {
        }
    }
}