using Microsoft.AspNetCore.Routing;

namespace Codewrinkles.MinimalApi.SmartModules
{
    public interface IModule
    {
        IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app);
    }
}
