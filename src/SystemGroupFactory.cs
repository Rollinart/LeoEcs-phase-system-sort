using System;
using System.Collections.Generic;
using System.Reflection;
using Leopotam.Ecs;
using Rollin.LeoEcs.Extensions;

namespace Rollin.LeoEcs
{
    internal static class SystemGroupFactory
    {
        public static Dictionary<Enum, SystemGroup> GetSystemGroups(params Type[] includedGroups)
        {
            var groupsDictionary = new Dictionary<Enum, SystemGroup>();

            CreateSystemGroups(groupsDictionary, includedGroups);
            CreateAndSortSystems(groupsDictionary);

            return groupsDictionary;
        }

        private static void CreateSystemGroups(IDictionary<Enum, SystemGroup> groupsDictionary,
            params Type[] includedTypes)
        {
            for (var index = 0; index < includedTypes.Length; index++)
            {
                var includedType = includedTypes[index];
                var systemGroupsTypes = includedType.GetEnumNames();

                for (var i = 0; i < systemGroupsTypes.Length; i++)
                {
                    var type = systemGroupsTypes[i];
                    var groupType = Enum.Parse(includedType, type) as Enum;
                    groupsDictionary.Add(groupType, new SystemGroup(groupType));
                }
            }
        }

        private static void CreateAndSortSystems(IReadOnlyDictionary<Enum, SystemGroup> groupsDictionary)
        {
            CreateSystems<IEcsInitSystem>(groupsDictionary);
            CreateSystems<IEcsRunSystem>(groupsDictionary);

            foreach (var keyValuePair in groupsDictionary)
            {
                keyValuePair.Value.SortSystems();
            }
        }

        private static void CreateSystems<T>(IReadOnlyDictionary<Enum, SystemGroup> groupsDictionary)
        {
            var usedAssemblies = new HashSet<string>();

            foreach (var key in groupsDictionary.Keys)
            {
                var groupType = key.GetType();
                var groupAssembly = groupType.Assembly;

                if (usedAssemblies.Contains(groupAssembly.FullName))
                {
                    continue;
                }

                usedAssemblies.Add(groupAssembly.FullName);

                var systemTypes = groupAssembly.GetTypesWithFilter(typeof(T),
                    AssemblyExtensions.AssemblyTypesFilter.IncludeAbstractClasses);
                
                foreach (Type type in systemTypes)
                {
                    if (type.IsInterface || type.IsAbstract)
                    {
                        continue;
                    }

                    var groupDependency = type.GetCustomAttribute<SystemGroupAttribute>(false);

                    if (groupDependency == null)
                        continue;

                    var system = (T) Activator.CreateInstance(type);

                    if (groupsDictionary.TryGetValue(groupDependency.GroupType, out var systemGroup))
                        systemGroup.AddSystem(system);
                }
            }
        }
    }
}