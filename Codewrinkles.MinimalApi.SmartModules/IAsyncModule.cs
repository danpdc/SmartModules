namespace Codewrinkles.MinimalApi.SmartModules
{
    /// <summary>
    /// Base interface for async smart module registration
    /// </summary>
    public interface IAsyncModule
    {
        /// <summary>
        /// Automatically and asynchronousely registers endpoints in the discovered modules
        /// </summary>
        /// <param name="app">The <see cref="IEndpointRouteBuilder"/> instance where endpoint registrations happen.
        /// </param>
        /// <returns>Returns back instance of <see cref="IEndpointRouteBuilder"/> passed in, after all registrations have been applied</returns>
        Task<IEndpointRouteBuilder> MapEndpointDefinitionsAsync(IEndpointRouteBuilder app);
    }
}
