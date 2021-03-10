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
        public Camera m_camera;
        private EntityManager m_entityManager;
        public Entity m_asteroidEntityLibrary;
        public Entity m_shipReference;
        private float m_currentTimer;
        public float m_asteroidSpawnFrequency;
        private Entity m_gameState;
        public static bool NeedToSpawnPlayer { get; set; }

        public Transform[] m_spawnPositions;
        
        private void Awake()
        {
            m_instance = this;

            m_spawnPositionsVectors = new Vector3[m_spawnPositions.Length];
            for (var i = 0; i < m_spawnPositionsVectors.Length; i++)
            {
                m_spawnPositionsVectors[i] = m_spawnPositions[i].position;
            }
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            var screenInfoEntity = m_entityManager.CreateEntity(ComponentType.ReadOnly<ScreenDataComponent>());

            var screenSize = GetScreenSize();

            var sys = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<OffScreenDetectionSystem>();
            
            sys.SetSingleton(new ScreenDataComponent()
            {
                m_width = screenSize.x,
                m_height = screenSize.y,
            });
        }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            m_entityManager.CreateEntity(typeof(InputComponentData));
            m_gameState = m_entityManager.CreateEntity(typeof(GameStateDataComponent));
            SpawnPlayerAt(float3.zero);
        }

        private void Update()
        {
            m_currentTimer += Time.deltaTime;
            if (m_currentTimer > m_asteroidSpawnFrequency)
            {
                m_currentTimer = 0;
                SpawnAsteroid();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var state = m_entityManager.GetComponentData<GameStateDataComponent>(m_gameState);

                var pause = !state.m_isOnPause;
                m_entityManager.SetComponentData(m_gameState, new GameStateDataComponent()
                {
                    m_isOnPause = pause
                });

                Time.timeScale = pause ? 0 : 1;
            }
        }

        private void SpawnAsteroid()
        {
            var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_asteroidEntityLibrary);
            var newAsteroid = m_entityManager.Instantiate(buffer[0].m_entity);

            var spawnPositionRandomIndex = UnityEngine.Random.Range(0, m_spawnPositionsVectors.Length);
            var spawnPosition = m_spawnPositionsVectors[spawnPositionRandomIndex];
            
            m_entityManager.SetComponentData(newAsteroid, new Translation()
            {
                Value = spawnPosition
            });
            
            m_entityManager.SetComponentData(newAsteroid, new AsteroidTagComponent()
            {
                m_size = 3,
                m_nbChildren = 2
            });
            
            var randomMoveDirection = math.normalize(new float3(Random.Range(-.8f,.8f), Random.Range(-.8f,.8f), 0));
            var randomRotation = math.normalize(new float3(Random.value, Random.value, 0));
            
            m_instance.m_entityManager.SetComponentData(newAsteroid, new MovementCommandsComponentData()
            {
                m_previousPosition = spawnPosition,
                m_directionOfMove = randomMoveDirection,
                m_linearImpulseCommand = 1,
                m_angularImpulse = 1,
                m_angularVector = randomRotation
            });
        }

        public void SignalPlayerDeath()
        {
            Invoke(nameof(LookForPlayerSpawnPosition),3);
            
        }

        public void LookForPlayerSpawnPosition()
        { 
            m_entityManager.CreateEntity(typeof(SpawnSignal));
        }

        private float2 GetScreenSize()
        {
            var bottomLeftCornerPosition = m_camera.ViewportToWorldPoint(new Vector3(0, 0));
            var topRightCornerPosition = m_camera.ViewportToWorldPoint(new Vector3(1, 1));
            var heightOfScreen = topRightCornerPosition.y - bottomLeftCornerPosition.y;
            var widthOfScreen = topRightCornerPosition.x - bottomLeftCornerPosition.x;
            var size = new float2(widthOfScreen, heightOfScreen);
            return size;
        }

        public static void SpawnPlayerAt(float3 _float3)
        {
            
            var ship = m_instance.m_entityManager.GetComponentData<ShipReferenceInBoostrapComponentData>(m_instance.m_shipReference);

            
            var newShipEntity = m_instance.m_entityManager.Instantiate(ship.m_ship);
            m_instance.m_entityManager.SetComponentData(newShipEntity, new Translation()
            {
                Value = _float3
            });
            
            
        }

        private static BootStrap m_instance;
        private Vector3[] m_spawnPositionsVectors;
    }

    internal struct SizeComponentData : IComponentData
    {
        public float Value;
    }
}


public struct ScreenDataComponent : IComponentData
{
    public float m_height;
    public float m_width;
}


[CreateAssetMenu(menuName = "Level/Level Data")]
public class LevelDatScriptableObject : ScriptableObject
{
    public int m_nbOfAsteroids;
    public int m_nbOfChildren;

    public int NbOfHits
    {
        get { return m_nbOfAsteroids*(1+m_nbOfChildren+(m_nbOfChildren*m_nbOfChildren)); }
    }
}