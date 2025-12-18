using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Extensions.DependencyInjection;

/// <summary>
/// Helper to register validators by scanning assemblies and namespaces.
/// </summary>
internal static class AssemblyScanningExtensions
{
    /// <summary>
    /// Registers all concrete types implementing any of the provided interfaces that live in the given namespace prefix.
    /// Allows for an optional action to perform additional registrations (e.g. adapters) for each type.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="assembly">Assembly to scan.</param>
    /// <param name="namespacePrefix">Namespace prefix to filter types (e.g. "Finova.Countries.Europe").</param>
    /// <param name="extraRegistration">Optional action to perform extra registrations for a found type.</param>
    /// <param name="interfacesToRegister">Interfaces to look for on concrete types.</param>
    public static void RegisterValidatorsFromNamespace(
        this IServiceCollection services, 
        Assembly assembly, 
        string namespacePrefix, 
        Action<IServiceCollection, Type>? extraRegistration,
        params Type[] interfacesToRegister)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null && t.Namespace.StartsWith(namespacePrefix, StringComparison.Ordinal));

        foreach (var implType in types)
        {
            var implemented = implType.GetInterfaces().Intersect(interfacesToRegister).ToArray();
            if (implemented.Length == 0)
            {
                continue;
            }

            // Register concrete type
            services.AddSingleton(implType);

            // Register each matching interface to resolve to the concrete instance
            foreach (var iface in implemented)
            {
                services.AddSingleton(iface, sp => sp.GetRequiredService(implType));
            }

            // Perform extra registrations (e.g. Adapters)
            extraRegistration?.Invoke(services, implType);
        }
    }
}
