using System;
using Leopotam.Ecs;

namespace Rollin.LeoEcs
{
    public static class SystemGroupProcessing
    {
        /// <summary>
        /// Create, sort and register systems to EcsSystems
        /// </summary>
        /// <param name="systems"></param>
        /// <param name="includedSystemGroups">
        /// Array of Enum types <code>new [] { typeof(EnumType_1), typeof(EnumType_2)}</code>
        /// </param>
        public static void PrepareSystems(EcsSystems systems, params Type[] includedSystemGroups)
        {
            var groups = SystemGroupFactory.GetSystemGroups(includedSystemGroups);

            foreach (var keyValuePair in groups)
            {
                keyValuePair.Value.RegisterInit(systems);
            }
            
            foreach (var keyValuePair in groups)
            {
                keyValuePair.Value.RegisterRun(systems);
            }
            
            foreach (var keyValuePair in groups)
            {
                keyValuePair.Value.RegisterHandlerRun(systems);
            }
            
            foreach (var keyValuePair in groups)
            {
                keyValuePair.Value.RegisterHandler(systems);
            }
        }
    }
}