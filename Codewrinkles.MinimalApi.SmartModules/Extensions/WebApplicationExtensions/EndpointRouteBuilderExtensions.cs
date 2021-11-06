using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions
{
    public static class EndpointRouteBuilderExtensions
    {
        private static readonly string[] HeadVerb = new[] { "HEAD" };
        private static readonly string[] ConnectVerb = new[] { "CONNECT" };
        private static readonly string[] OptionsVerb = new[] { "OPTIONS" };
        private static readonly string[] TraceVerb = new[] { "TRACE" };
        public static IEndpointConventionBuilder MapHead(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
        {
            return Map(endpoints, pattern, HeadVerb, handler);
        }

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