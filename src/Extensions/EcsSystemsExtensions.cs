using System.Reflection;
using Leopotam.Ecs;
using Scellecs.Collections;

namespace Rollin.LeoEcs.Extensions
{
    public static class EcsSystemsExtensions
    {
        public static FastList<T> SortSystems<T>(this FastList<T> systems) where T : IEcsSystem
        {
            if (systems.length <= 1)
            {
                return systems;
            }

            var graph = new Graph<T>(systems.length);

            foreach (var ecsSystem in systems)
            {
                graph.AddVertex(ecsSystem);
            }

            foreach (var ecsSystem in systems)
            {
                var systemType = ecsSystem.GetType();

                if (systemType.IsDefined(typeof(UpdateAfterAttribute), false))
                {
                    graph.AddEdge(ecsSystem,
                        graph.FindVertex(system =>
                            system.GetType() == ecsSystem.GetType().GetCustomAttribute<UpdateAfterAttribute>().Type));
                }

                if (systemType.IsDefined(typeof(UpdateBeforeAttribute), false))
                {
                    graph.AddEdge(
                        graph.FindVertex(system =>
                            system.GetType() == ecsSystem.GetType().GetCustomAttribute<UpdateBeforeAttribute>().Type),
                        ecsSystem);
                }
            }

            var hash = graph.DeepFirstSort();

            return hash.ToFastList();
        }
    }
}