using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixClient.Tests
{
    public static class Extensions
    {
        public static IServiceCollection Replace<TService, TImplementation>(
                this IServiceCollection services,
                ServiceLifetime lifetime)
                where TService : class
                where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);

            services.Add(descriptorToAdd);
            //services.BuildServiceProvider();
            return services;
        }

        public static IServiceCollection ReplaceSingleton<TService>(
                this IServiceCollection services,
                TService implementation)
                where TService : class                
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);
            
            var descriptorToAdd = new ServiceDescriptor(typeof(TService), implementation);

            services.Add(descriptorToAdd);
            //services.BuildServiceProvider();
            return services;
        }
    }
}
