namespace MatrixClient.ViewModel
{    
    using Microsoft.Extensions.DependencyInjection;

    static class ServiceCollectionExtensions
    {
     
        /// <summary>
        /// Adds the view model classes to the Avalonia DI container
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
        {
            return
                serviceCollection
                    .AddSingleton<Account>()
                    .AddSingleton<MyViewModel>()
                    .AddSingleton<Commands>()
                    .AddSingleton<Contacts>()
                    .AddSingleton<SubscriptionRequests>()
                    .AddSingleton<AddUser>()
                    .AddSingleton<RosterCommands>()
                    .AddSingleton<SubscriptionCommands>();
        }
    }
}
