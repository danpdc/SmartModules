using Codewrinkles.MinimalApi.SmartModules;
using Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Sample.Modules
{
    public class TestModule : SmartModule
    {
        private readonly ILogger<TestModule> _logger;
        private readonly DummyService _dummyService;
        public TestModule(ILogger<TestModule> logger, DummyService dummyService)
        {
            _logger = logger;
            _dummyService = dummyService;
        }
        public override IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app)
        {
            app.MapHead("/api", () => "Response to HEAD method")
                .WithName("Head")
                .WithDisplayName("Sample tests");
            _logger.LogInformation("Registered HEAD endpoint");
            app.MapOptions("/api",() => "Response to OPTIONS method")
                .WithName("Options")
                .WithDisplayName("Sample tests");
            return app;
        }
    }
}