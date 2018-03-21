namespace MatrixClient
{
    using MatrixClient.LiteDb;
    using MatrixClient.Mappings;
    using MatrixClient.ViewModel;
    using MatrixClient.Xmpp;
    using MatrixClient.Xmpp.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public class ServiceLocator
    {
        static ServiceProvider serviceProvider;
        static IServiceCollection serviceCollection;

        public static ServiceProvider Current
        {
            get
            {
                if (serviceProvider == null)
                    InitServices();

                return serviceProvider;
            }
        }

        public static IServiceCollection ServiceCollection
        {
            get
            {
                if (serviceProvider == null)
                    InitServices();

                return serviceCollection;
            }
        }

        private static void InitServices()
        {
            serviceCollection = new ServiceCollection()
                    .AddAutoMapper()
                    .AddViewModels()
                    .AddXmppLogging()
                    .AddXmppClient()
                    .AddLiteDb();            

            serviceProvider = serviceCollection.BuildServiceProvider();           
        }
    }
}
