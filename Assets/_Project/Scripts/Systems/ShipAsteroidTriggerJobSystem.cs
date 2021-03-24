using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;


public class ShipAsteroidTriggerJobSystem : JobComponentSystem
{
    private BuildPhysicsWorld m_physicsWorld;
    private StepPhysicsWorld m_stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem m_endSimulationEntityCommandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_physicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        m_stepPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();

        m_endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var shipComponents = GetComponentDataFromEntity<PlayerTagComponent>();
        var asteroidsComponents = GetComponentDataFromEntity<AsteroidTagComponent>();
        var destroyables = GetComponentDataFromEntity<DestroyableComponentData>();
        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
        
        var job = new TriggerJob
        {
            m_ecb = ecb,
            m_ships = shipComponents,
            m_asteroids = asteroidsComponents,
            m_destroyables = destroyables
        };

        var jobHandle = job.Schedule(m_stepPhysicsWorld.Simulation, ref m_physicsWorld.PhysicsWorld, inputDeps);

        jobHandle.Complete();

        m_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);
        
        return jobHandle;
    }
    
    [BurstCompile]
    private struct TriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PlayerTagComponent> m_ships;
        [ReadOnly] public ComponentDataFromEntity<AsteroidTagComponent> m_asteroids;
        public ComponentDataFromEntity<DestroyableComponentData> m_destroyables;

        public EntityCommandBuffer m_ecb;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            /*var entityAIsAShip = m_ships.HasComponent(triggerEvent.EntityA);
            var entityBIsAShip = m_ships.HasComponent(triggerEvent.EntityB);
            
            var entityAIsAnAsteroid = m_asteroids.HasComponent(triggerEvent.EntityA);
            var entityBIsAnAsteroid = m_asteroids.HasComponent(triggerEvent.EntityB);

            if (entityAIsAnAsteroid && entityBIsAShip)
            {
                m_ecb.SetComponent(triggerEvent.EntityB, new DestroyableComponentData()
                {
                    m_mustBeDestroyed = true
                });
                
                m_ecb.SetComponent(triggerEvent.EntityA, new DestroyableComponentData()
                {
                    m_mustBeDestroyed = true
                });
            }

            if (entityAIsAShip && entityBIsAnAsteroid)
            {
                m_ecb.SetComponent(triggerEvent.EntityA, new DestroyableComponentData()
                {
                    m_mustBeDestroyed = true
                });
                
                m_ecb.SetComponent(triggerEvent.EntityB, new DestroyableComponentData()
                {
                    m_mustBeDestroyed = true
                });
            }*/
        }
    }
}