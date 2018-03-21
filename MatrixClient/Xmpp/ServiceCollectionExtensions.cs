namespace MatrixClient.Xmpp
{
    using MatrixClient.Xmpp.Handlers;
    using Microsoft.Extensions.DependencyInjection;   

    static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a XMPP client instance to the Avalonia DI container
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static IServiceCollection AddXmppClient(this IServiceCollection serviceCollection)
        {
            return
                serviceCollection                    
                    .AddSingleton<ICapsStorage, CapsStorage>()
                    .AddSingleton<IAvatarStorage, AvatarStorage>()
                    .AddSingleton<IUserAvatarStorage, UserAvatarStorage>()
                    .AddSingleton<ICaps, ClientCaps>()
                    .AddSingleton<IPresenceManager, PresenceManager>()
                    .AddSingleton<DiscoHandler>()
                    .AddSingleton<CapsCollectionHandler>()
                    .AddSingleton<AvatarCollectionHandler>()
                    .AddSingleton<PresenceCollectionHandler>()
                    .AddSingleton<PresenceSubscriptionHandler>()
                    .AddSingleton<RosterPushHandler>()
                    .AddSingleton<XmppClientEx>()
                    .AddSingleton<XmppSession>();
        }
    }
}
