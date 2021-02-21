using System;
using Leopotam.Ecs;
using Rollin.LeoEcs.Extensions;
using Scellecs.Collections;

namespace Rollin.LeoEcs
{
    internal class SystemGroup
    {
        public Enum GroupType { get; }

        private FastList<IEcsInitSystem> initSystems;

        private FastList<IEcsRunSystem> runSystems;
        private FastList<IEcsRunSystem> handlerRunSystems;
        private FastList<IEcsRunSystem> handlerSystems;

        public SystemGroup(Enum groupType)
        {
            GroupType = groupType;

            initSystems = new FastList<IEcsInitSystem>();
            runSystems = new FastList<IEcsRunSystem>();
            handlerRunSystems = new FastList<IEcsRunSystem>();
            handlerSystems = new FastList<IEcsRunSystem>();
        }

        public void AddSystem<T>(T system)
        {
            switch (system)
            {
                case IEcsInitSystem initSystem:
                    initSystems.Add(initSystem);
                    return;
                case IEcsRunSystem runSystem when system.GetType().IsDefined(typeof(HandlerSystemAttribute), false):
                    handlerSystems.Add(runSystem);
                    return;
                case IEcsRunSystem runSystem when system.GetType().IsDefined(typeof(HandlerRunSystemAttribute), false):
                    handlerRunSystems.Add(runSystem);
                    return;
                case IEcsRunSystem runSystem:
                    runSystems.Add(runSystem);
                    return;
                default:
                    throw new Exception($"{typeof(T).Name} is not inherited from Init or Run system");
            }
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