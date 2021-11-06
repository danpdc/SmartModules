using Microsoft.AspNetCore.Builder;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
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
            app.MapEndpoints();
            return app;
        }
    }
}