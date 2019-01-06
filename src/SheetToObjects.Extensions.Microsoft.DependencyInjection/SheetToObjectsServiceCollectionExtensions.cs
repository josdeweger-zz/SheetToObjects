using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Extensions.Microsoft.DependencyInjection
{
    public static class SheetToObjectsServiceCollectionExtensions
    {
        /// <summary>Adds services required for using SheetToObjects by scanning all assemblies starting with the given string for SheetToObjectConfig files</summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="assembliesStartingWith">Scans all assemblies starting with this string for SheetToObjectConfigs and adds them to the SheetMapper</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSheetToObjects(this IServiceCollection services, string assembliesStartingWith)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var assemblies = GetAssembliesStartingWith(assembliesStartingWith);

            return AddSheetToObjects(services, assemblies.ToArray());
        }

        /// <summary>Adds services required for using SheetToObjects by scanning the assembly containing the given type for SheetToObjectConfig files</summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSheetToObjects(this IServiceCollection services, Type type)
        {
            return AddSheetToObjects(services, new[] { type.Assembly });
        }

        /// <summary>Adds services required for using SheetToObjects by scanning the assemblies containing the given types for SheetToObjectConfig files</summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="types"></param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSheetToObjects(this IServiceCollection services, Type[] types)
        {
            return AddSheetToObjects(services, types.Select(t => t.Assembly).ToArray());
        }

        /// <summary>Adds services required for using SheetToObjects by scanning the given assembly for SheetToObjectConfig files</summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="assembly"></param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSheetToObjects(this IServiceCollection services, Assembly assembly)
        {
            return AddSheetToObjects(services, new[] {assembly});
        }

        /// <summary>Adds services required for using SheetToObjects by scanning the given assemblies for SheetToObjectConfig files</summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="assemblies"></param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSheetToObjects(this IServiceCollection services, Assembly[] assemblies)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if(assemblies == null || !assemblies.Any())
                throw new ArgumentNullException(nameof(assemblies));

            var sheetToObjectConfigTypes = GetTypesFromAssemblies<SheetToObjectConfig>(assemblies);

            var sheetToObjectConfigs = CreateInstances<SheetToObjectConfig>(sheetToObjectConfigTypes).ToList();

            var sheetMapper = new SheetMapper();

            foreach (var config in sheetToObjectConfigs)
                sheetMapper.AddSheetToObjectConfig(config);

            services.TryAdd(ServiceDescriptor.Singleton<IMapSheetToObjects>(sheetMapper));

            return services;
        }

        private static IEnumerable<T> CreateInstances<T>(IEnumerable<Type> types)
        {
            return types.Select(p => (T)Activator.CreateInstance(p));
        }

        private static IEnumerable<Type> GetTypesFromAssemblies<T>(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(a => a.GetTypes())
                .Where(p => typeof(T).IsAssignableFrom(p) && p.IsPublic && !p.IsAbstract)
                .Distinct();
        }

        private static IEnumerable<Assembly> GetAssembliesStartingWith(string start)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith(start, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
