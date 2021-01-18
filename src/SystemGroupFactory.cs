using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Leopotam.Ecs;
using Rollin.LeoEcs.Extensions;

namespace Rollin.LeoEcs
{
    public static class SystemGroupFactory
    {
        public static IEnumerable<ISystemGroup> GetSystemGroups(params Type[] includedGroups)
        {
            var groupsDictionary = new Dictionary<Enum, ISystemGroup>();

            CreateSystemGroups(groupsDictionary, includedGroups);

            CreateAndSortSystems(groupsDictionary);

            var groups = new ISystemGroup[groupsDictionary.Count];

            var itr = 0;
            foreach (var keyValuePair in groupsDictionary)
            {
                groups[itr] = keyValuePair.Value;
                itr++;
            }

            return groupsDictionary.Select(x => x.Value);
        }

        private static void CreateSystemGroups(IDictionary<Enum, ISystemGroup> groupsDictionary,
            params Type[] includedTypes)
        {
            foreach (var includedType in includedTypes)
            {
                var systemGroupsTypes = includedType.GetEnumNames();

                foreach (var type in systemGroupsTypes)
                {
                    var enumValue = Enum.Parse(includedType, type) as Enum;
                    groupsDictionary.Add(enumValue, new SystemGroup(enumValue));
                }
            }
        }

        private static void CreateAndSortSystems(IReadOnlyDictionary<Enum, ISystemGroup> groupsDictionary)
        {
            CreateSystems<IEcsInitSystem>(groupsDictionary);
            CreateSystems<IEcsRunSystem>(groupsDictionary);

            foreach (var keyValuePair in groupsDictionary)
            {
                keyValuePair.Value.SortSystems();
            }
        }

        private static void CreateSystems<T>(IReadOnlyDictionary<Enum, ISystemGroup> groupsDictionary)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var systemTypes = executingAssembly.GetTypesWithFilter(typeof(T),
                AssemblyExtensions.AssemblyTypesFilter.IncludeAbstractClasses);

            foreach (var type in systemTypes)
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