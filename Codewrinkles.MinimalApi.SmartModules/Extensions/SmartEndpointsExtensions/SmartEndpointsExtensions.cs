using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.SmartEndpointsExtensions
{
    /// <summary>
    /// Extensions methods over the .NET 6 minimal API endoiunt registration methods
    /// These smart endpoint methods wrap the .NET minimal API functionality in order to
    /// offer a mechanism for looking into the registration process and allowing the SmartModules
    /// library to extend the .NET6 base functionality.
    /// </summary>
    public static class SmartEndpointsExtensions
    {
        private static readonly string[] HeadVerb = new[] { "HEAD" };
        private static readonly string[] OptionsVerb = new[] { "OPTIONS" };
        private static readonly string[] PatchVerb = new[] { "Patch" };
        private static readonly string[] GetVerb = new[] { "GET" };
        private static readonly string[] PostVerb = new[] { "POST" };
        private static readonly string[] PutVerb = new[] { "PUT" };
        private static readonly string[] DeleteVerb = new[] { "DELETE" };

        /// <summary>
        /// Wrapper extensions method for .NET 6 minimal API app.MapGet()
        /// Registers and endpoint for HTTP GET, for a specific pattern and providing a handler
        /// </summary>
        /// <param name="app">The .NET 6 <see cref="IEndpointRouteBuilder"/></param>
        /// <param name="pattern">A pattern that defines an API route</param>
        /// <param name="handler">A handler to be executed each time there is an HTTP cal
        /// matchung the HTTP GET method and the provided pattern</param>
        /// <returns>The <see cref="IEndpointConventionBuilder"/> containing the registered endpoint</returns>
        public static IEndpointConventionBuilder MapSmartGet(this IEndpointRouteBuilder app, 
            string pattern, Delegate handler)
        {
            return Map(app, pattern, GetVerb, handler);
        }

        /// <summary>
        /// Wrapper extensions method for .NET 6 minimal API app.MapPost()
        /// Registers and endpoint for HTTP POST, for a specific pattern and providing a handler
        /// </summary>
        /// <param name="app">The .NET 6 <see cref="IEndpointRouteBuilder"/></param>
        /// <param name="pattern">A pattern that defines an API route</param>
        /// <param name="handler">A handler to be executed each time there is an HTTP call
        /// matchung the HTTP POST method and the provided pattern</param>
        /// <returns>The <see cref="IEndpointConventionBuilder"/> containing the registered endpoint</returns>
        public static IEndpointConventionBuilder MapSmartPost(this IEndpointRouteBuilder app, 
            string pattern, Delegate handler)
        {
            return Map(app, pattern, PostVerb, handler);
        }

        /// <summary>
        /// Wrapper extensions method for .NET 6 minimal API app.MapPut()
        /// Registers and endpoint for HTTP PUT, for a specific pattern and providing a handler
        /// </summary>
        /// <param name="app">The .NET 6 <see cref="IEndpointRouteBuilder"/></param>
        /// <param name="pattern">A pattern that defines an API route</param>
        /// <param name="handler">A handler to be executed each time there is an HTTP call
        /// matchung the HTTP PUT method and the provided pattern</param>
        /// <returns>The <see cref="IEndpointConventionBuilder"/> containing the registered endpoint</returns>
        public static IEndpointConventionBuilder MapSmartPut(this IEndpointRouteBuilder app,
            string pattern, Delegate handler)
        {
            return Map(app, pattern, PutVerb, handler);
        }

        /// <summary>
        /// Wrapper extensions method for .NET 6 minimal API app.MapDelete()
        /// Registers and endpoint for HTTP DELETE, for a specific pattern and providing a handler
        /// </summary>
        /// <param name="app">The .NET 6 <see cref="IEndpointRouteBuilder"/></param>
        /// <param name="pattern">A pattern that defines an API route</param>
        /// <param name="handler">A handler to be executed each time there is an HTTP call
        /// matchung the HTTP PUT method and the provided pattern</param>
        /// <returns>The <see cref="IEndpointConventionBuilder"/> containing the registered endpoint</returns>
        public static IEndpointConventionBuilder MapSmartDelete(this IEndpointRouteBuilder app,
            string pattern, Delegate handler)
        {
            return Map(app, pattern, DeleteVerb, handler);
        }

        /// <summary>
        /// Maps a HEAD request.
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="pattern">Pattern that described the desired URL. E.g. "/api/products/{id}"</param>
        /// <param name="handler">The handler responsible to service the request and generate a response</param>
        /// <returns></returns>
        public static IEndpointConventionBuilder MapSmartHead(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
        {
            return Map(endpoints, pattern, HeadVerb, handler);
        }

        /// <summary>
        /// Automatically maps an OPTIONS HTTP request
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="pattern">Pattern that described the desired URL. E.g. "/api/products/{id}"</param>
        /// <param name="handler">The handler responsible to service the request and generate a response</param>
        /// <returns></returns>
        public static IEndpointConventionBuilder MapSmartOptions(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)

        {
            return Map(endpoints, pattern, OptionsVerb, handler);
        }

        /// <summary>
        /// Automatically maps a PATCH HTTP request
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pattern">Pattern that described the desired URL. E.g. "/api/products/{id}"</param>
        /// <param name="handler">The handler responsible to service the request and generate a response</param>
        /// <returns></returns>
        public static IEndpointConventionBuilder MapSmartPatch(this IEndpointRouteBuilder app,
            string pattern, Delegate handler)
        {
            return Map(app, pattern, PatchVerb, handler);
        }

        private static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints, string pattern,
            IEnumerable<string> methods, Delegate handler)
        {
            return endpoints.MapMethods(pattern, methods, handler);
        }
    }
}
