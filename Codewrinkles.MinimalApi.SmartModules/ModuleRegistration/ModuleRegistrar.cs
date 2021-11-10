using System.Runtime.Serialization;

namespace Codewrinkles.MinimalApi.SmartModules.ModuleRegistration
{
    internal class ModuleRegistrar
    {
        private IEnumerable<Type>? _modules;
        private IEnumerable<Type>? _endpointDef;
        private IEnumerable<Type>? _derivedModules;
        internal ModuleRegistrar(Type scanInput)
        {
            var scanner = new ModuleScanner(scanInput);
            _modules = scanner.DiscoverModules();
            _endpointDef = scanner.DiscoverEndpointDefinitions();
            _derivedModules = scanner.DiscoverDerivedModules();
        }

        internal IServiceCollection AddModulesToDi(IServiceCollection services)
        {
            if (_modules is null) throw new NullReferenceException("Module discovery failed.");

            if (_endpointDef is null) throw new NullReferenceException("Endpoint definition discovery failed.");

            AddModuleServices(services);
            AddEndpointDefinitionServices(services);

            return services;
        }

        internal WebApplication MapEndpoints(WebApplication app)
        {
            var modules = _modules ?? Enumerable.Empty<Type>();
            var derived = _derivedModules ?? Enumerable.Empty<Type>();
            var distinct = modules.Concat(derived).Distinct();

            RegisterEndpointsFromModuleEnumerable(distinct!, app);

            return app;
        }
        private void RegisterEndpointsFromModuleEnumerable(IEnumerable<Type> types, WebApplication app)
        
        {
            foreach (var module in types)
            {
                var serviceType = module.UnderlyingSystemType;
                var method = serviceType.GetMethod("MapEndpointDefinitionsAsync");

                if (method is null)
                {
                    var instance = app.Services.GetRequiredService(serviceType) as IModule;

                    if (instance is null) throw new NullReferenceException();

                    instance!.MapEndpointDefinitions(app);
                }
                else
                {
                    var instance = app.Services.GetRequiredService(serviceType) as IAsyncModule;

                    if (instance is null) throw new NullReferenceException();

                    instance!.MapEndpointDefinitionsAsync(app).Wait();
                }
            }
        }

        private void AddModuleServices(IServiceCollection services)
        {
            foreach (var type in _modules!)
            {
                if (type.IsAssignableTo(typeof(SmartModule)))
                {
                    var instance = FormatterServices.GetUninitializedObject(type) as SmartModule;
                    instance!.RegisterModuleServices(services);
                }

                services.AddTransient(type.UnderlyingSystemType);
            }
        }

        private void AddEndpointDefinitionServices(IServiceCollection services)
        {
            foreach (var ed in _endpointDef!)
            {
                services.AddTransient(ed.UnderlyingSystemType);
            }
        }
    }
}
