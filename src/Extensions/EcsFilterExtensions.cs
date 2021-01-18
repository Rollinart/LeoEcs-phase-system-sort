using Leopotam.Ecs;

namespace Rollin.LeoEcs.Extensions
{
    public static class EcsFilterExtensions
    {
        public static EcsEntity FindEntity<T>(this EcsFilter<T> filter) where T : struct
        {
            foreach (var idx in filter)
            {
                return filter.GetEntity(idx);
            }

            return EcsEntity.Null;
        }

        public static EcsEntity FindEntity<T1, T2>(this EcsFilter<T1, T2> filter) where T1 : struct where T2 : struct
        {
            foreach (var idx in filter)
            {
                return filter.GetEntity(idx);
            }

            return EcsEntity.Null;
        }
    }
}