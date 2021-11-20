using Codewrinkles.MinimalApi.SmartModules;
using Codewrinkles.MinimalApi.SmartModules.Extensions.SmartEndpointsExtensions;

namespace Sample.Modules
{
    public class SmartEndpointsModule : SmartModule
    {
        public override IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app)
        {
            app.MapSmartGet("/api/smart", () => "Return from smart GET")
                .WithDisplayName("Smart endpoints");
            app.MapSmartPost("/api/smart", () => "Return from smart POST")
                .WithDisplayName("Smart endpoints");
            app.MapSmartPut("/api/smart", () => "Return from smart PUT")
                .WithDisplayName("Smart endpoints");
            app.MapSmartDelete("/api/smart", () => "Return from smart DELETE")
                .WithDisplayName("Smart endpoints");
            app.MapSmartPatch("/api/smart", () => "Return from smart PATCH")
                .WithDisplayName("Smart endpoints");
            app.MapSmartHead("/api/smart", () => "Return from smart HEAD")
                .WithDisplayName("Smart endpoints");
            app.MapSmartOptions("/api/smart", () => "Return from smart OPTIONS")
                .WithDisplayName("Smart endpoints");
            return app;
        }
    }
}
