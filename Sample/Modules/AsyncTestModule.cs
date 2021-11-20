using Codewrinkles.MinimalApi.SmartModules;
using Codewrinkles.MinimalApi.SmartModules.Extensions.SmartEndpointsExtensions;

namespace Sample.Modules
{
    public class AsyncTestModule : SmartAsyncModule
    {
        private readonly DummyService _dummyService;
        public AsyncTestModule(DummyService dummyService)
        {
            _dummyService = dummyService;
        }
        public override async Task<IEndpointRouteBuilder> MapEndpointDefinitionsAsync(IEndpointRouteBuilder app)
        {
            app.MapSmartHead("/api/async", () => "Response to HEAD method").WithName("HeadAsync").WithDisplayName("Sample tests");
            app.MapSmartOptions("/api/async", () => "Response to OPTIONS method").WithName("OptionsAsync").WithDisplayName("Sample tests");
            app.MapSmartPatch("/api/smartpatch", () => "Response from smart patch");
            await Task.Delay(100);
            return app;
        }
    }
}
