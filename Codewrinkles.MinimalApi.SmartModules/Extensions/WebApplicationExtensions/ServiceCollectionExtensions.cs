using System;
using Codewrinkles.MinimalApi.SmartModules.ModuleRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
    /// <summary>
    /// Extension methods to add required SmartModuleServices
    /// </summary>
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
            try
            {
                var registrar = new ModuleRegistrar(type);
                registrar.AddModulesToDi(services);
                services.AddSingleton(typeof(ModuleRegistrar), (provider) => { return registrar; });
                return services;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}