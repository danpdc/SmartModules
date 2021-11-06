using System;
using Microsoft.Extensions.DependencyInjection;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartModules(this IServiceCollection services, Type type)
        {
            services.AddModulesToDi(type);
            return services;
        }
    }
}