using Microsoft.AspNetCore.Routing;

namespace Codewrinkles.MinimalApi.SmartModules
{
    /// <summary>
    /// Base interface that defines what a smart module is
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Automatically registers endpoints in the discovered modules
        /// </summary>
        /// <param name="app">The <see cref="IEndpointRouteBuilder"/> instance where endpoint registrations happen.
        /// </param>
        /// <returns>Returns back instance of <see cref="IEndpointRouteBuilder"/> passed in, after all registrations have been applied</returns>
        IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app);
    }
}
