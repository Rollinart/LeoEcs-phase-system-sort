using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Rollin.LeoEcs.Extensions;

namespace Rollin.LeoEcs
{
    public class SystemGroup : ISystemGroup
    {
        public Enum GroupType { get; }

        private ICollection<IEcsInitSystem> initSystems;

        private ICollection<IEcsRunSystem> runSystems;
        private ICollection<IEcsRunSystem> handlerRunSystems;
        private ICollection<IEcsRunSystem> handlerSystems;

        public SystemGroup(Enum groupType)
        {
            GroupType = groupType;

            initSystems = new List<IEcsInitSystem>();
            runSystems = new List<IEcsRunSystem>();
            handlerRunSystems = new List<IEcsRunSystem>();
            handlerSystems = new List<IEcsRunSystem>();
        }

        public void AddSystem<T>(T system)
        {
            if (system is IEcsInitSystem initSystem)
            {
                initSystems.Add(initSystem);
                return;
            }

            if (system is IEcsRunSystem runSystem)
            {
                if (system.GetType().IsDefined(typeof(HandlerSystemAttribute), true))
                {
                    handlerSystems.Add(runSystem);
                    return;
                }

                if (system.GetType().IsDefined(typeof(HandlerRunSystemAttribute), true))
                {
                    handlerRunSystems.Add(runSystem);
                    return;
                }

                runSystems.Add(runSystem);
                return;
            }

            throw new Exception($"{typeof(T).Name} is not inherited from Init or Run system");
        }

        public void SortSystems()
        {
            initSystems = initSystems.SortSystems();
            runSystems = runSystems.SortSystems();
            handlerRunSystems = handlerRunSystems.SortSystems();
            handlerSystems = handlerSystems.SortSystems();
        }

        public void RegisterInit(EcsSystems systems)
        {
            foreach (var system in initSystems)
            {
                systems.Add(system);
            }
        }

        public void RegisterRun(EcsSystems systems)
        {
            foreach (var system in runSystems)
            {
                systems.Add(system);
            }
        }

        public void RegisterHandlerRun(EcsSystems systems)
        {
            foreach (var system in handlerRunSystems)
            {
                systems.Add(system);
            }
        }

        public void RegisterHandler(EcsSystems systems)
        {
            foreach (var system in handlerSystems)
            {
                systems.Add(system);
            }
        }
    }
}