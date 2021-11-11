namespace Codewrinkles.MinimalApi.SmartModules
{
    /// <summary>
    /// Abstract base class that defines a smart module
    /// </summary>
    public abstract class SmartModule : SmartModuleBase, IModule
    {
        /// <summary>
        /// Abstract method meant to automatically register endpoint definitions
        /// </summary>
        /// <param name="app">The <see cref="IEndpointRouteBuilder"/> instance where endpoint registrations happen.
        /// </param>
        /// <returns>Returns back instance of <see cref="IEndpointRouteBuilder"/> passed in, after all registrations have been applied</returns>
        public abstract IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app);
        
    }
}
