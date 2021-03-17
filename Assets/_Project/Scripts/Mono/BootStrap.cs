using System;
using System.Collections;
using _Project.Scripts.Components;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.ScriptableObjects.Achievements;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Mono
{
    public class BootStrap : MonoBehaviour
    {
        
        public int m_lives;
        public int m_startLives=3;
        public GameEvent m_onLevelCompletedEvent;
        public IntGameEvent m_onLivesChangeEvent;
        public IntGameEvent m_onLevelStartsEvent;
        public LevelDataScriptableObject[] m_levelDataScriptableObjects;
        private LevelDataScriptableObject m_currentLevelData;

        public int m_hits;

        public int m_currentLevelIndex=-1;
        
        private Camera m_camera;
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

            var jsonGameData = DataSaveLoadUtils.GetGameData();
            m_gameData = string.IsNullOrEmpty(jsonGameData) ? new GameData() : JsonUtility.FromJson<GameData>(jsonGameData);

            m_gameData.CurrentScore = 0;
            m_gameData.CurrentLevel = 0;
            
            m_camera = FindObjectOfType<Camera>().GetComponent<Camera>();

            m_spawnPositionsVectors = new Vector3[m_spawnPositions.Length];
            for (var i = 0; i < m_spawnPositionsVectors.Length; i++)
            {
                m_spawnPositionsVectors[i] = m_spawnPositions[i].position;
            }
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            var sys = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<OffScreenDetectionSystem>();

            var singletonAlreadyExists = sys.TryGetSingleton(out ScreenDataComponent component);
            if (singletonAlreadyExists) return;
            var screenInfoEntity = m_entityManager.CreateEntity(ComponentType.ReadOnly<ScreenDataComponent>());

            var screenSize = GetScreenSize();

            
            sys.SetSingleton(new ScreenDataComponent()
            {
                m_width = screenSize.x,
                m_height = screenSize.y,
            });
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.2f);
            StartGame();
        }

        public void StartGame()
        {
            m_lives = m_startLives;
            StartNextLevel();
            m_onLivesChangeEvent.Raise(m_lives);    
            m_entityManager.CreateEntity(typeof(InputComponentData));
            m_gameState = m_entityManager.CreateEntity(typeof(GameStateDataComponent));
            SpawnPlayerAt(float3.zero);
        }

        private void Update()
        {
            m_gameData.TimeOfPlay += Time.deltaTime;
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
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

            if (m_currentLevelData == null) return;
            if (m_nbAsteroidAlreadySpawnedInThisLevel >= m_currentLevelData.m_nbOfAsteroids) return;
            
            m_currentTimer += Time.deltaTime;
            if (m_currentTimer > m_asteroidSpawnFrequency)
            {
                m_nbAsteroidAlreadySpawnedInThisLevel++;
                
                m_currentTimer = 0;
                SpawnAsteroid();
            }
            
        }

        private void SpawnAsteroid()
        {
            var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_asteroidEntityLibrary);
            var newAsteroid = m_entityManager.Instantiate(buffer[2].m_entity);

            var spawnPositionRandomIndex = UnityEngine.Random.Range(0, m_spawnPositionsVectors.Length);
            var spawnPosition = m_spawnPositionsVectors[spawnPositionRandomIndex];
            
            m_entityManager.SetComponentData(newAsteroid, new Translation()
            {
                Value = spawnPosition
            });
            
            m_entityManager.SetComponentData(newAsteroid, new AsteroidTagComponent()
            {
                m_size = 3,
                m_nbChildren = m_currentLevelData.m_nbOfChildren,
                m_points = 100
            });
            
            var randomMoveDirection = math.normalize(new float3(Random.Range(-.8f,.8f), Random.Range(-.8f,.8f), 0));
            var randomRotation = math.normalize(new float3(Random.value, Random.value, 0));
            
            m_instance.m_entityManager.SetComponentData(newAsteroid, new MovementCommandsComponentData()
            {
                m_previousPosition = spawnPosition,
                m_currentDirectionOfMove = randomMoveDirection,
                m_currentlinearCommand = 1,
                m_angularImpulse = 1,
                m_currentAngularCommand = randomRotation
            });
        }

        public void SignalPlayerDeath()
        {
            m_lives -= 1;
            if (m_lives < 0)
            {
                GameOver();
                return;
            }
            m_onLivesChangeEvent.Raise(m_lives);    
            Invoke(nameof(LookForPlayerSpawnPosition),1);
            
        }

        private void GameOver()
        {
            SceneManager.LoadScene(0);
        }

        public void SaveGameData()
        {
            var json = JsonUtility.ToJson(m_gameData);
            DataSaveLoadUtils.SaveGameData(json);
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
        
        public void StartNextLevel()
        {
            m_currentLevelIndex++;
            m_gameData.CurrentLevel = m_currentLevelIndex;
            
            m_onLevelStartsEvent.Raise(m_currentLevelIndex+1);
            m_currentLevelData = m_levelDataScriptableObjects[m_currentLevelIndex];
            m_hits = 0;
            m_nbAsteroidAlreadySpawnedInThisLevel = 0;
            print("Hits to complete: "+m_currentLevelData.NbOfHits);
        }

        public void AddHit(int _score)
        {
            m_gameData.CurrentScore += _score;
            if (m_gameData.CurrentScore > m_gameData.SavedHighScore)
            {
                // New High Score
                m_gameData.SavedHighScore = m_gameData.CurrentScore;
            }
            m_hits++;
            if (m_hits < m_currentLevelData.NbOfHits) return;
            m_onLevelCompletedEvent.Raise();
            StartNextLevel();
        }

        public void AddWrapHit()
        {
            m_gameData.TotalNumberOfWrapHit++;
        }
        
        public void AddShoot()
        {
            m_gameData.TotalShot++;
        }

        private void OnDestroy()
        {
            SaveGameData();
            DestroyAllEntities();
        }
        
        void DestroyAllEntities() {
            m_entityManager.DestroyEntity(m_entityManager.UniversalQuery);
        }

        private static BootStrap m_instance;
        private Vector3[] m_spawnPositionsVectors;
        private int m_nbAsteroidAlreadySpawnedInThisLevel;
        public GameData m_gameData;
    }
    
    

}


public struct ScreenDataComponent : IComponentData
{
    public float m_height;
    public float m_width;
}
