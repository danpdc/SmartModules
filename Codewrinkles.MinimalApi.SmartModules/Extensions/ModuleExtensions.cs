using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions
{
    public static class ModuleExtensions
    {
        private static IEnumerable<Type>? _modules;
        private static IEnumerable<Type>? _endpointDef;

        private static void DiscoverModules(Type type)
        {
            _modules = type.Assembly.GetTypes()
                .Where(m => m.IsClass && m.IsAssignableTo((typeof(IModule))));
        }

        private static void DiscoverEndpointDefinitions(Type type)
        {
            _endpointDef = type.Assembly.GetTypes()
                .Where(ed => ed.IsClass && ed.IsAssignableTo(typeof(IEndpointDefinition)));
        }
        
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            var loggerFactory = app.Services.GetService<ILoggerFactory>();
            var logger = loggerFactory!.CreateLogger(typeof(WebApplicationExtensions.WebApplicationExtensions));
            foreach (var module in _modules!)
            {
                logger.LogInformation("Registering module: {Module}", module.UnderlyingSystemType.Name);
                var serviceType = module.UnderlyingSystemType;
                var instance = app.Services.GetRequiredService(serviceType) as IModule;
                instance!.MapEndpointDefinitions(app);
            }
            
            return app;
        }

        internal static IServiceCollection AddModulesToDi(this IServiceCollection services, Type assembly)
        {
            DiscoverModules(assembly);
            DiscoverEndpointDefinitions(assembly);
            foreach (var type in _modules!)
            {
                services.AddTransient(type.UnderlyingSystemType);
            }

            foreach (var ed in _endpointDef!)
            {
                services.AddTransient(ed.UnderlyingSystemType);
            }
            return services;
        }
    }
}