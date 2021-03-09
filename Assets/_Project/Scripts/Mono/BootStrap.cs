using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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

        private void Awake()
        {
            m_instance = this;
            
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
            m_entityManager.Instantiate(buffer[0].m_entity);
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
    }
    
   
}


public struct ScreenDataComponent : IComponentData
{
    public float m_height;
    public float m_width;
}