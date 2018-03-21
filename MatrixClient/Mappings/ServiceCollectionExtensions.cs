namespace MatrixClient.Mappings
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;

    static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the AutoMapperConfiguration to the Avalonia DI container
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IMapper>(AutoMapperConfiguration.BuildMapperConfiguration());
        }
    }
}
