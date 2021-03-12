using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.Mono;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerSpawnSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem m_endSimulationEntityCommandBufferSystem;
    protected override void OnCreate()
    {
        m_endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        base.OnCreate();
    }

    protected override void OnUpdate()
    {
        var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        var asteroids = GetEntityQuery(typeof(AsteroidTagComponent),typeof(Translation));

        var spawnPosition = new NativeArray<float3>(1,Allocator.TempJob);
        var foundPosition = new NativeArray<bool>(1,Allocator.TempJob);
        
        var asteroidTranslations = asteroids.ToComponentDataArray<Translation>(Allocator.TempJob);
        var positionArray = new NativeArray<float3>(asteroidTranslations.Length, Allocator.TempJob);

        for (var i = 0; i < positionArray.Length; i++)
        {
            positionArray[i] = asteroidTranslations[i].Value;
        }
        
        Entities.WithNativeDisableParallelForRestriction(randomArray).WithReadOnly(positionArray).ForEach((
            Entity _entity, 
            int entityInQueryIndex,
            int nativeThreadIndex,ref SpawnSignal _signal) =>
        {
            var validatePosition = true;
            var random = randomArray[nativeThreadIndex];

            var valueX = random.NextFloat(-10, 10);
            var valueY = random.NextFloat(-10, 10);

            var possiblePosition = new float3(valueX, valueY, 0);
            randomArray[nativeThreadIndex] = random;

            foreach (var VARIABLE in positionArray)
            {
                if (math.length(VARIABLE - possiblePosition) < 10)
                {
                    validatePosition = false;
                    break;
                }
            }

            if (validatePosition)
            {
                Debug.Log("Found Position");
                    spawnPosition[0] = possiblePosition;
                    foundPosition[0] = true;
                    ecb.DestroyEntity(entityInQueryIndex, _entity);
                    
            }
            else
            {
                Debug.Log("Wrong Position");
                
            }


        }).ScheduleParallel();
        Dependency.Complete();

        if (BootStrap.NeedToSpawnPlayer && foundPosition[0])
        {
            BootStrap.SpawnPlayerAt(spawnPosition[0]);
        }
        m_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        
        asteroidTranslations.Dispose();
        positionArray.Dispose();
        
        spawnPosition.Dispose();
        foundPosition.Dispose();

    }
}