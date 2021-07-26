using System;
using _Project.Scripts.Components;
using Unity.Entities;
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

        private void Awake()
        {
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            m_spawnPositionsVectors = new Vector3[m_asteroidsSpawnPositions.Length];
            for (int i = 0; i < m_spawnPositionsVectors.Length; i++)
            {
                m_spawnPositionsVectors[i] = m_asteroidsSpawnPositions[i].position;
            }
        }


        void Start()
        {
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
}