using Codewrinkles.MinimalApi.SmartModules.ModuleRegistration;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
    /// <summary>
    /// WebApplication extensions to register smart modules
    /// </summary>
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Adds all the endpoints discovered in modules end endpoint definitions to the <see cref="WebApplication"/> pipeline.
        /// It also adds smart modules middleware to the middleware pipeline
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication UseSmartModules(this WebApplication app)
        {
            var registrar = app.Services.GetRequiredService(typeof(ModuleRegistrar)) as ModuleRegistrar;
            registrar?.MapEndpoints(app);
            return app;
        }
    }
}