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
        public Entity m_playerShipEntity;
        private EntityManager m_entityManager;

        public Transform[] m_asteroidsSpawnPositions;
        private Vector3[] m_spawnPositionsVectors;

        private float m_currentTimer;
        public float m_asteroidSpawnFrequency = 2f;

        private ValidateSpawnPositionJob m_validateSpawnPositionJob;
        private JobHandle m_jobHandle;

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
            m_validateSpawnPositionJob = new ValidateSpawnPositionJob();
            
            m_entityManager.CreateEntity(typeof(InputComponentData));
            SpawnShipAtPosition(Vector3.zero);
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
            var screenInfoQuery = m_entityManager.CreateEntityQuery(typeof(ScreenInfoComponentData));
            var screenInfoEntity = screenInfoQuery.GetSingletonEntity();
            var screenInfoComponent = m_entityManager.GetComponentData<ScreenInfoComponentData>(screenInfoEntity);

            var screenHalfWidth = screenInfoComponent.m_width * .5f;
            var screenHalfHeight = screenInfoComponent.m_height * .5f;

            var asteroidsQuery = m_entityManager.CreateEntityQuery(typeof(AsteroidTagComponent), ComponentType.ReadOnly<Translation>());
            var translationComponentsOfAllAsteroids = asteroidsQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
            
            var isSpawnPositionValid = new NativeArray<bool>(1,Allocator.TempJob);

            var possibleSpawnPosition = new Vector3(Random.Range(-screenHalfWidth,screenHalfWidth),Random.Range(-screenHalfHeight,screenHalfHeight),0);

            while (stillLookingForPosition)
            {
                m_validateSpawnPositionJob.m_translations = translationComponentsOfAllAsteroids;
                m_validateSpawnPositionJob.m_possibleSpawnPosition = possibleSpawnPosition;
                m_validateSpawnPositionJob.m_minimalSpawnDistance = 5f;
                m_validateSpawnPositionJob.m_result = isSpawnPositionValid;

                m_jobHandle = m_validateSpawnPositionJob.Schedule();
                m_jobHandle.Complete();

                if (isSpawnPositionValid[0])
                {
                    stillLookingForPosition = false;
                }
                else
                {
                    possibleSpawnPosition = new Vector3(Random.Range(-screenHalfWidth,screenHalfWidth),Random.Range(-screenHalfHeight,screenHalfHeight),0);
                }
            }

            isSpawnPositionValid.Dispose();
            translationComponentsOfAllAsteroids.Dispose();
            
            SpawnShipAtPosition(possibleSpawnPosition);
        }
        
        
        private void SpawnShipAtPosition(Vector3 _spawnPosition)
        {
            var shipLibraryReference =
                m_entityManager.GetComponentData<ShipLibraryElementComponentData>(m_playerShipEntity);
            var playerShip = m_entityManager.Instantiate(shipLibraryReference.m_ship);
            m_entityManager.SetComponentData(playerShip, new Translation
            {
                Value = _spawnPosition
            });
        }
    }


    public struct ValidateSpawnPositionJob: IJob
    {
        public NativeArray<Translation> m_translations;
        public float3 m_possibleSpawnPosition;
        public float m_minimalSpawnDistance;
        public NativeArray<bool> m_result;
        
        public void Execute()
        {
            var result = true;
            foreach (var translation in m_translations)
            {
                if(math.distancesq(translation.Value,m_possibleSpawnPosition)<m_minimalSpawnDistance*m_minimalSpawnDistance)
                {
                    result = false;
                    break;
                }
            }

            m_result[0] = result;
        }
    }
    
    
    
}