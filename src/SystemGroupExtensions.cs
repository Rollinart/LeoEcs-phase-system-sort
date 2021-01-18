using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;

namespace Rollin.LeoEcs
{
    public static class SystemGroupExtensions
    {
        public static IEnumerable<ISystemGroup> Sort(this IEnumerable<ISystemGroup> groups)
        {
            var groupsDictionary = new Dictionary<Type, List<ISystemGroup>>();

            foreach (var systemGroup in groups)
            {
                if (!groupsDictionary.TryGetValue(systemGroup.GroupType.GetType(), out var list))
                {
                    list = new List<ISystemGroup>();
                    groupsDictionary.Add(systemGroup.GroupType.GetType(), list);
                }

                list.Add(systemGroup);
            }

            foreach (var keyValuePair in groupsDictionary)
            {
                groupsDictionary[keyValuePair.Key].Sort((group, group2) => group.GroupType.CompareTo(group2.GroupType));
            }

            return groupsDictionary.SelectMany(x => x.Value);
        }

        public static IEnumerable<ISystemGroup> RegisterToEcsSystems(this IEnumerable<ISystemGroup> groups, EcsSystems systems)
        {
            foreach (var systemGroup in groups)
            {
                systemGroup.RegisterInit(systems);
            }

            foreach (var systemGroup in groups)
            {
                systemGroup.RegisterRun(systems);
            }

            foreach (var systemGroup in groups)
            {
                systemGroup.RegisterHandlerRun(systems);
            }

            foreach (var systemGroup in groups)
            {
                systemGroup.RegisterHandler(systems);
            }

            return groups;
        }
    }
}