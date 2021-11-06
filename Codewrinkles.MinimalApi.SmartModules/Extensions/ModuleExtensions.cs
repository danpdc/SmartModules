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
            var logger = loggerFactory!.CreateLogger("SMartModulesEndpointRegistration");
            foreach (var module in _modules!)
            {
                logger.LogDebug("Registering module: {Module}", module.UnderlyingSystemType.Name);
                var serviceType = module.UnderlyingSystemType;
                var instance = app.Services.GetRequiredService(serviceType) as IModule;

                if (instance is null)
                {
                    var exception = new NullReferenceException($"Unable to resolve module service {module.UnderlyingSystemType}");
                    logger.LogError(exception, "Unable to resolve module service for {Module}", new object[] {module.UnderlyingSystemType});
                    throw exception;
                }
                
                instance!.MapEndpointDefinitions(app);
            }
            
            return app;
        }

        internal static IServiceCollection AddModulesToDi(this IServiceCollection services, Type assembly)
        {
            var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("SmartModulesServiceRegistrations");
            DiscoverModules(assembly);
            DiscoverEndpointDefinitions(assembly);

            if (_modules is null)
            {
                var exception = new NullReferenceException("Module discover failed.");
                logger.LogError(exception, exception.Message);
                throw exception;
            }
            
            if (!_modules.Any()) logger.LogWarning("No smart modules were discovered in assembly: {Assembly}." +
                                                   "Make sure that intended modules inherit the IModule interface", 
                new object[] {assembly.Name});

            try
            {
                foreach (var type in _modules!)
                {
                    services.AddTransient(type.UnderlyingSystemType);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while trying to add smart module services to the DI container" +
                                   "See the inner stack trace for more details!");
                throw;
            }
            
            if (_endpointDef is null)
            {
                var exception = new NullReferenceException("Endpoint definition discovery failed.");
                logger.LogError(exception, exception.Message);
                throw exception;
            }

            try
            {
                foreach (var ed in _endpointDef!)
                {
                    services.AddTransient(ed.UnderlyingSystemType);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while trying to add endpoint definitions to the DI container" +
                                   "See the inner stack trace for more details!");
                throw;
            }

            return services;
        }
    }
}