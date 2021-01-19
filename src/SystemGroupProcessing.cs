using System;
using Leopotam.Ecs;

namespace Rollin.LeoEcs
{
    public static class SystemGroupProcessing
    {
        public static void PrepareSystems(EcsSystems systems, Type[] includedSystemGroups)
        {
            var groups = SystemGroupFactory.GetSystemGroups(includedSystemGroups);
            groups = groups.Sort();
            groups.RegisterToEcsSystems(systems);
        }
    }
}