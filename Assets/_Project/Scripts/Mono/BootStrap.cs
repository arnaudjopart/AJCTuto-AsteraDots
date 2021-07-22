using System;
using _Project.Scripts.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Mono
{
    public class BootStrap : MonoBehaviour
    {

        public Entity m_asteroidLibrary;
        private EntityManager m_entityManager;

        public Transform[] m_asteroidsSpawnPositions;
        private Vector3[] m_spawnPositionsVectors;

        private float m_currentTimer;
        public float m_asteroidSpawnFrequency = 2f;
        private JobHandle m_jobHandle;
        public Entity m_playerEntity;
        public static BootStrap m_instance;

        private void Awake()
        {
            m_instance = this;
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            m_spawnPositionsVectors = new Vector3[m_asteroidsSpawnPositions.Length];
            for (int i = 0; i < m_spawnPositionsVectors.Length; i++)
            {
                m_spawnPositionsVectors[i] = m_asteroidsSpawnPositions[i].position;
            }
        }


        void Start()
        {
            m_jobHandle = new JobHandle();
            m_entityManager.CreateEntity(typeof(InputComponentData));
        }

        private void SpawnAsteroid()
        {
            var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_asteroidLibrary);
            var lengthOfBuffer = buffer.Length;
            var randomAsteroidIndex = Random.Range(0, lengthOfBuffer);
            var newAsteroid = m_entityManager.Instantiate(buffer[randomAsteroidIndex].m_entity);

            var randomSpawnPositionIndex = Random.Range(0, m_spawnPositionsVectors.Length);
            var spawnPosition = m_spawnPositionsVectors[randomSpawnPositionIndex];
            
            m_entityManager.SetComponentData(newAsteroid, new Translation()
            {
                Value = spawnPosition
            });

            var randomMoveDirection = math.normalize(new float3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0));
            var randomRotation = math.normalize(new float3(Random.value, Random.value, 0));
            
            m_entityManager.SetComponentData(newAsteroid, new MovementCommandsComponentData()
            {
                m_previousPosition = spawnPosition,
                m_currentDirectionOfMove = randomMoveDirection,
                m_currentlinearCommand = 1,
                m_currentAngularCommand = randomRotation
            });
        }

        private void Update()
        {
            m_currentTimer += Time.deltaTime;

            if (m_currentTimer > m_asteroidSpawnFrequency)
            {
                m_currentTimer = 0;
                SpawnAsteroid();
            }

        }

        public void LookForPlayerSpawnPosition()
        {
            var stillLookingForPosition = true;
            var possiblePosition = new Vector3(Random.Range(-10, 10), Random.Range(-5, 5), 0);
            while (stillLookingForPosition)
            {
                var query =
                    m_entityManager.CreateEntityQuery(typeof(AsteroidTagComponent),
                        ComponentType.ReadOnly<Translation>());
            
                var length = query.ToComponentDataArray<Translation>(Allocator.TempJob);
                var result = new NativeArray<bool>(1, Allocator.TempJob);
            
                print(length.Length);
            
                var job = new ValidateSpawnPositionJob()
                {
                    m_translations = length,
                    m_minSpawnDistance = 5,
                    m_possibleSpawnPosition = possiblePosition,
                    m_result = result
                };

                m_jobHandle = job.Schedule();
                m_jobHandle.Complete();

                if (result[0])
                {
                    stillLookingForPosition = false;
                }
                else
                {
                    possiblePosition = new Vector3(Random.Range(-10, 10), Random.Range(-5, 5), 0);
                }
            
                result.Dispose();
                length.Dispose();
            }

            var ship = m_entityManager.GetComponentData<ShipReferenceInBoostrapComponentData>(m_playerEntity);

            var player = m_entityManager.Instantiate(ship.m_ship);
            m_entityManager.SetComponentData(player, new Translation
            {
                Value = possiblePosition
            });
            
        }

        public struct ValidateSpawnPositionJob : IJob
        {
            public NativeArray<Translation> m_translations;
            public float3 m_possibleSpawnPosition;
            public float m_minSpawnDistance;
            public NativeArray<bool> m_result;

            public void Execute()
            {
                var result = true;
                foreach (var translation in m_translations)
                {
                    if (math.distance(translation.Value,m_possibleSpawnPosition) < m_minSpawnDistance)
                    {
                        result = false;
                        break;
                    }
                }

                m_result[0] = result;
            }
        }
        
        
    }
}