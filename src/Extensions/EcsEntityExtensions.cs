using Leopotam.Ecs;

namespace Rollin.LeoEcs.Extensions
{
    public static class EcsEntityExtensions
    {
        public static object[] GetComponents(this in EcsEntity entity)
        {
            var components = new object[0];
            entity.GetComponentValues(ref components);
            return components;
        }

        public static bool IsValid(this EcsEntity entity)
        {
            return !entity.IsNull() && entity.IsAlive();
        }
    }
}