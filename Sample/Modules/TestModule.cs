using Codewrinkles.MinimalApi.SmartModules;
using Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Sample.Modules
{
    public class TestModule : IModule
    {
        public IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app)
        {
            app.MapHead("/api", () => "Response to HEAD method").WithName("Head").WithDisplayName("Sample tests");
            app.MapOptions("/api",() => "Response to OPTIONS method").WithName("Options").WithDisplayName("Sample tests");
            app.MapTrace("/api", () => "Response to TRACE method").WithName("Trace").WithDisplayName("Sample tests");
            return app;
        }
    }
}