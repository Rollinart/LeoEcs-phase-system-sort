using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Leopotam.Ecs;

namespace Rollin.LeoEcs.Extensions
{
    public static class EcsSystemsExtensions
    {
        public static ICollection<T> SortSystems<T>(this ICollection<T> systems) where T : IEcsSystem
        {
            if (systems.Count <= 1)
            {
                return systems;
            }

            var graph = new Graph<T>(systems.Count);

            foreach (var ecsSystem in systems)
            {
                graph.AddVertex(ecsSystem);
            }

            foreach (var ecsSystem in systems)
            {
                var systemType = ecsSystem.GetType();

                if (systemType.IsDefined(typeof(UpdateAfterAttribute), false))
                {
                    var updateAfterType = ecsSystem.GetType().GetCustomAttribute<UpdateAfterAttribute>().Type;

                    graph.AddEdge(ecsSystem, graph.FindVertex(system => system.GetType() == updateAfterType));
                }

                if (systemType.IsDefined(typeof(UpdateBeforeAttribute), false))
                {
                    var updateBeforeType = ecsSystem.GetType().GetCustomAttribute<UpdateBeforeAttribute>()?.Type;
                    graph.AddEdge(graph.FindVertex(system => system.GetType() == updateBeforeType), ecsSystem);
                }
            }

            var hash = graph.DeepFirstSort();

            return hash.ToList();
        }
    }
}