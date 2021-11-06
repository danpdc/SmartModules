using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseSmartModules(this WebApplication app, Type type)
        {
            app.MapEndpoints();
            return app;
        }
    }
}