using System;
using Microsoft.Extensions.DependencyInjection;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds smart module services to the DI container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type">Any type from the assembly where Smart Modules should scan for your modules</param>
        /// <returns></returns>
        public static IServiceCollection AddSmartModules(this IServiceCollection services, Type type)
        {
            services.AddModulesToDi(type);
            return services;
        }
    }
}