using System;
using System.Reflection;
using Scellecs.Collections;

namespace Rollin.LeoEcs.Extensions
{
    public static class AssemblyExtensions
    {
        [Flags]
        public enum AssemblyTypesFilter
        {
            None = 1 << 0,
            IncludeInterfaces = 1 << 1,
            IncludeAbstractClasses = 1 << 2,
            IncludeGeneric = 1 << 3,
            All = IncludeInterfaces | IncludeAbstractClasses | IncludeGeneric,
        }

        public static FastList<Type> GetTypesWithFilter(this AppDomain appDomain, Type type,
            AssemblyTypesFilter filter = AssemblyTypesFilter.All)
        {
            var result = new FastList<Type>(10);
            var assemblies = appDomain.GetAssemblies();

            for (var index = 0; index < assemblies.Length; index++)
            {
                var assembly = appDomain.GetAssemblies()[index];
                var types = assembly.GetTypes();
                for (var i = 0; i < types.Length; i++)
                {
                    var assemblyType = types[i];
                    if (assemblyType.IsAssignableFromWithFilter(type, filter))
                        result.Add(assemblyType);
                }
            }

            return result;
        }

        public static FastList<Type> GetTypesWithFilter(this Assembly assembly, Type type,
            AssemblyTypesFilter filter = AssemblyTypesFilter.All)
        {
            var result = new FastList<Type>(10);
            var types = assembly.GetTypes();
            for (var index = 0; index < types.Length; index++)
            {
                var assemblyType = types[index];
                if (assemblyType.IsAssignableFromWithFilter(type, filter))
                    result.Add(assemblyType);
            }

            return result;
        }

        private static bool IsAssignableFromWithFilter(this Type type, Type assignedFrom, AssemblyTypesFilter filter)
        {
            return assignedFrom.IsAssignableFrom(type) &&
                   (type.IsInterface && filter.IsSet(AssemblyTypesFilter.IncludeInterfaces) ||
                    type.IsAbstract && filter.IsSet(AssemblyTypesFilter.IncludeAbstractClasses) &&
                    !type.IsInterface || !type.IsInterface && !type.IsAbstract && !type.IsGenericType ||
                    type.IsGenericType && filter.IsSet(AssemblyTypesFilter.IncludeGeneric));
        }

        private static bool IsSet(this AssemblyTypesFilter filter, AssemblyTypesFilter mask)
        {
            return (filter & mask) == mask;
        }
    }
}