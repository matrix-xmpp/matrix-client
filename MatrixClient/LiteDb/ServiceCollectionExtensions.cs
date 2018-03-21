namespace MatrixClient.LiteDb
{
    using Microsoft.Extensions.DependencyInjection;
   
    static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the LiteDb Context to the DI container
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static IServiceCollection AddLiteDb(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddLiteDb("MatriXClient");
        }

        /// <summary>
        /// /// Adds the LiteDb Context and configuration to the DI container
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="databaseName">the databaseName used in the <see cref="LiteDbConfiguration"/> </param>
        /// <returns></returns>
        public static IServiceCollection AddLiteDb(this IServiceCollection serviceCollection, string databaseName)
        {
            return serviceCollection
                    .AddSingleton<IDbConfiguration>(new LiteDbConfiguration { DatabaseName = databaseName})
                    .AddSingleton<LiteDbContext>();
        }
    }
}
