namespace Codewrinkles.MinimalApi.SmartModules
{
    public abstract class SmartModule : IModule
    {
        public abstract IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app);
        public virtual IServiceCollection RegisterModuleServices(IServiceCollection services)
        {
            return services;
        }
    }
}
