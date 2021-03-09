using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

public class LaserAsteroidTriggerJobSystem : JobComponentSystem
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
        var laserComponents = GetComponentDataFromEntity<ProjectileTagComponent>();
        var asteroidsComponents = GetComponentDataFromEntity<AsteroidTagComponent>();
        var destroyables = GetComponentDataFromEntity<DestroyableComponentData>();
        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
        var job = new TriggerJob
        {
            m_ecb = ecb,
            m_projectiles = laserComponents,
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
        [ReadOnly] public ComponentDataFromEntity<ProjectileTagComponent> m_projectiles;
        [ReadOnly] public ComponentDataFromEntity<AsteroidTagComponent> m_asteroids;
        public ComponentDataFromEntity<DestroyableComponentData> m_destroyables;

        public EntityCommandBuffer m_ecb;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            var entityAIsAProjectile = m_projectiles.HasComponent(triggerEvent.EntityA);
            var entityBIsAProjectile = m_projectiles.HasComponent(triggerEvent.EntityB);
            
            var entityAIsAnAsteroid = m_asteroids.HasComponent(triggerEvent.EntityA);
            var entityBIsAnAsteroid = m_asteroids.HasComponent(triggerEvent.EntityB);

            if (entityAIsAnAsteroid && entityBIsAProjectile)
            {
                Debug.Log("entityAIsAnAsteroid");
               
                if (m_destroyables.HasComponent(triggerEvent.EntityA))
                {
                    Debug.Log("Found destroy");
                    var a =m_destroyables[triggerEvent.EntityA];
                    a.m_mustBeDestroyed = true;

                    m_destroyables[triggerEvent.EntityA] = a;
                }
            }

            if (entityAIsAProjectile && entityBIsAnAsteroid)
            {
                Debug.Log("entityAIsAProjectile");
                if (m_destroyables.HasComponent(triggerEvent.EntityB))
                {
                    
                    var a =m_destroyables[triggerEvent.EntityB];
                    a.m_mustBeDestroyed = true;
                    m_destroyables[triggerEvent.EntityB] = a;
                }
                else
                {
                    Debug.Log("Not found destroy");
                }
                
            }
        }
    }
}