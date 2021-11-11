using Codewrinkles.MinimalApi.SmartModules;

namespace Sample.Modules
{
    public class ModuleBaseTestModule : SmartModule
    {
        public override IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/smartmodulebase", () => "Response to HEAD method")
                .WithName("something")
                .WithDisplayName("Sample tests");
            return app;
        }

        public override IServiceCollection RegisterModuleServices(IServiceCollection services)
        {
            services.AddSingleton<DummyService>();
            return base.RegisterModuleServices(services);
        }
    }
}
