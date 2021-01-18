using System;
using Leopotam.Ecs;

namespace Rollin.LeoEcs
{
    public interface ISystemGroup
    {
        Enum GroupType { get; }

        void AddSystem<T>(T system);

        void SortSystems();

        void RegisterInit(EcsSystems systems);
        
        void RegisterRun(EcsSystems systems);
        
        void RegisterHandlerRun(EcsSystems systems);

        void RegisterHandler(EcsSystems systems);
    }
}