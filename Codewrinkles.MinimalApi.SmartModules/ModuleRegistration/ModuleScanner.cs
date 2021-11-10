namespace Codewrinkles.MinimalApi.SmartModules.ModuleRegistration
{
    internal class ModuleScanner
    {
        internal ModuleScanner(Type scanInput)
        {
            ScanInput = scanInput;
        }

        private Type ScanInput { get; init; }

        internal IEnumerable<Type> DiscoverModules()
        {
            return ScanInput.Assembly.GetTypes()
                .Where(m => m.IsClass
                && (m.IsAssignableTo(typeof(IModule)) || m.IsAssignableTo(typeof(IAsyncModule))));
        }

        internal IEnumerable<Type> DiscoverDerivedModules()
        {
            return ScanInput.Assembly.GetTypes()
                .Where(m => m.IsClass && (m.IsAssignableTo(typeof(SmartModule))));
        }

        internal IEnumerable<Type> DiscoverEndpointDefinitions()
        {
            return ScanInput.Assembly.GetTypes()
                .Where(ed => ed.IsClass && ed.IsAssignableTo(typeof(IEndpointDefinition)));
        }
    }
}
