namespace Codewrinkles.MinimalApi.SmartModules
{
    /// <summary>
    /// Abstract class that defines base functionality for all module types
    /// </summary>
    public abstract class SmartModuleBase
    {
        /// <summary>
        /// Registers module specific services. Override this method if you want to add your module specific services
        /// to your module
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual IServiceCollection RegisterModuleServices(IServiceCollection services)
        {
            return services;
        }
    }
}
