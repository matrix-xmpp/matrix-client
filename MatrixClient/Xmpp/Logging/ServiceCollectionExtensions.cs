namespace MatrixClient.Xmpp.Logging
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds XMPP logger and logger configuration to the Avalonia DI container
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static IServiceCollection AddXmppLogging(this IServiceCollection serviceCollection)
        {
            var loggerFactory = new LoggerFactory();
            var xmlLoggerConfig = new XmlLoggerConfiguration()
            {
                LogLevel = LogLevel.Information,
                Enabled = false,
            };

            loggerFactory.AddProvider(new XmlLoggerProvider(xmlLoggerConfig));

            serviceCollection.AddSingleton<XmlLoggerConfiguration>(xmlLoggerConfig);
            serviceCollection.AddSingleton<ILogger>(loggerFactory.CreateLogger(nameof(MatrixClient)));

            return serviceCollection;
        }
    }
}
