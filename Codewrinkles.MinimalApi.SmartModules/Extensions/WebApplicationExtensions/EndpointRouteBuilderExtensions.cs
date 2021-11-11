namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
    /// <summary>
    /// Extension methods for <see cref="IEndpointRouteBuilder"></see>
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        private static readonly string[] HeadVerb = new[] { "HEAD" };
        private static readonly string[] OptionsVerb = new[] { "OPTIONS" };

        /// <summary>
        /// Maps a HEAD request.
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="pattern">Pattern that described the desired URL. E.g. "/api/products/{id}"</param>
        /// <param name="handler">The handler responsible to service the request and generate a response</param>
        /// <returns></returns>
        public static IEndpointConventionBuilder MapHead(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
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
        public static IEndpointConventionBuilder MapOptions(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)

        {
            return Map(endpoints, pattern, OptionsVerb,handler);
        }

        private static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints, string pattern, 
            IEnumerable<string> methods, Delegate handler)
        {
            return endpoints.MapMethods(pattern, methods, handler);
        }
    }
}