using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Components;
using _Project.Scripts.Mono;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class AsteroidDestructionSystem : SystemBase
{

    protected override void OnCreate()
    {
        base.OnCreate();
        
    }

    protected override void OnUpdate()
    {
        var entityManager = World.EntityManager;

        var query = GetEntityQuery(typeof(EntityBufferElement));
        var array = query.ToEntityArray(Allocator.TempJob);

        var buffer = entityManager.GetBuffer<EntityBufferElement>(array[0]);

        var nativeArrayOfAsteroids = new NativeArray<Entity>(3, Allocator.TempJob);

        for (var i = 0; i < nativeArrayOfAsteroids.Length; i++)
        {
            nativeArrayOfAsteroids[i] = buffer[i].m_entity;
        }
        Entities.WithoutBurst().WithStructuralChanges().WithAll<AsteroidTagComponent>().ForEach((Entity _entity,
            in DestroyableComponentData _destroyable,
            in Translation _translation, 
            in AsteroidTagComponent _asteroid) =>
        {
            if (_destroyable.m_mustBeDestroyed)
            {
                if (_asteroid.m_size > 1)
                {
                    var newSize = _asteroid.m_size - 1;
                    var nbChildren = _asteroid.m_nbChildren;
                    var spawnPosition = _translation.Value;

                    for (int i = 0; i < nbChildren; i++)
                    {
                        var child = entityManager.Instantiate(nativeArrayOfAsteroids[newSize-1]);
                        
                        entityManager.SetComponentData(child,new Translation()
                        {
                            Value = spawnPosition
                        });
                        var points = i == 2 ? 200 : 300;
                       
                        entityManager.SetComponentData(child, new AsteroidTagComponent()
                        {
                            m_size = newSize,
                            m_nbChildren = nbChildren,
                            m_points = points
                        });

                        var randomMoveDirection = math.normalize(new float3(UnityEngine.Random.Range(-.8f,.8f), UnityEngine.Random.Range(-.8f,.8f), 0));
                        var randomRotation = math.normalize(new float3(UnityEngine.Random.value, UnityEngine.Random.value, 0));
            
                        entityManager.SetComponentData(child, new MovementCommandsComponentData()
                        {
                            m_previousPosition = spawnPosition,
                            m_directionOfMove = randomMoveDirection,
                            m_linearImpulseCommand = 1,
                            m_angularImpulse = 1,
                            m_angularVector = randomRotation
                        });
                    }
                    
                }

                GameEventManager.RaiseAsteroidDestroyedEvent(_asteroid.m_points);
                entityManager.DestroyEntity(_entity);
                var fx = FXPool.GetExplosionFx();
                fx.transform.position = _translation.Value;
            }
        }).Run();
        array.Dispose();
        nativeArrayOfAsteroids.Dispose();
    }
}