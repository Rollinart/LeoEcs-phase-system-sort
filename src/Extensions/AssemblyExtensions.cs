using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rollin.LeoEcs.Extensions
{
    public static class AssemblyExtensions
    {
        [Flags]
        public enum AssemblyTypesFilter
        {
            None,
            IncludeInterfaces,
            IncludeAbstractClasses,
            IncludeGeneric,
            All = IncludeInterfaces | IncludeAbstractClasses | IncludeGeneric,
        }

        public static List<Type> GetTypesWithFilter(this AppDomain appDomain, Type type,
            AssemblyTypesFilter filter = AssemblyTypesFilter.All)
        {
            var result = new List<Type>();

            foreach (var assembly in appDomain.GetAssemblies())
            {
                var types = assembly.GetTypes();
                foreach (var assemblyType in types)
                {
                    if (assemblyType.IsAssignableFromWithFilter(type, filter))
                        result.Add(assemblyType);
                }
            }

            return result;
        }

        public static List<Type> GetTypesWithFilter(this Assembly assembly, Type type,
            AssemblyTypesFilter filter = AssemblyTypesFilter.All)
        {
            var result = new List<Type>();
            var types = assembly.GetTypes();
            foreach (var assemblyType in types)
            {
                if (assemblyType.IsAssignableFromWithFilter(type, filter))
                    result.Add(assemblyType);
            }

            return result;
        }

        private static bool IsAssignableFromWithFilter(this Type type, Type assignedFrom, AssemblyTypesFilter filter)
        {
            return assignedFrom.IsAssignableFrom(type) &&
                   (type.IsInterface && filter.HasFlag(AssemblyTypesFilter.IncludeInterfaces) ||
                    type.IsAbstract && filter.HasFlag(AssemblyTypesFilter.IncludeAbstractClasses) &&
                    !type.IsInterface || !type.IsInterface && !type.IsAbstract && !type.IsGenericType ||
                    type.IsGenericType && filter.HasFlag(AssemblyTypesFilter.IncludeGeneric));
        }
    }
}