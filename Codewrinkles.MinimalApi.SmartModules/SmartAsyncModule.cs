using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codewrinkles.MinimalApi.SmartModules
{
    /// <summary>
    /// Abstract base class that defines an async SmartModul
    /// </summary>
    public abstract class SmartAsyncModule : SmartModuleBase, IAsyncModule
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public abstract Task<IEndpointRouteBuilder> MapEndpointDefinitionsAsync(IEndpointRouteBuilder app);
    }
}
