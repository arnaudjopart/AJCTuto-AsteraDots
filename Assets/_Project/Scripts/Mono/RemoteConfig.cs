using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects;
using Unity.RemoteConfig;
using UnityEngine;

namespace _Project.Scripts.Mono
{
    public class RemoteConfig : MonoBehaviour
    {
        public bool m_fetchRemoteData;
        private LevelsData m_defaultLevelsData;
        public LevelDataScriptableObject[] m_levels;

        public struct UserAttributes {

        }

        public struct AppAttributes {

        }
    
    
        private void Awake()
        {
        
            m_defaultLevelsData = new LevelsData
            {
                m_levels = new[]
                {
                    new LevelRemoteData() {m_nbOfAsteroids = 1, m_nbOfChildren = 1},
                    new LevelRemoteData() {m_nbOfAsteroids = 2, m_nbOfChildren = 3},
                    new LevelRemoteData() {m_nbOfAsteroids = 3, m_nbOfChildren = 3},
                    new LevelRemoteData() {m_nbOfAsteroids = 5, m_nbOfChildren = 2},
                    new LevelRemoteData() {m_nbOfAsteroids = 6, m_nbOfChildren = 3},
                    new LevelRemoteData() {m_nbOfAsteroids = 6, m_nbOfChildren = 4},
                }
            };

            LoadLevelData(m_defaultLevelsData);

            if (!m_fetchRemoteData) return;
            
            ConfigManager.FetchCompleted += ApplyRemoteSettings;
            ConfigManager.SetEnvironmentID("d5558a49-7760-4c64-a9ba-f773d2582ad9");
            ConfigManager.FetchConfigs(new UserAttributes(), new AppAttributes());
        }

        private void LoadLevelData(LevelsData defaultLevelsData)
        {
            for (var i = 0; i < m_levels.Length; i++)
            {
                m_levels[i].m_nbOfAsteroids = defaultLevelsData.m_levels[i].m_nbOfAsteroids;
                m_levels[i].m_nbOfChildren = defaultLevelsData.m_levels[i].m_nbOfChildren;
            }
        }

        private void ApplyRemoteSettings(ConfigResponse obj)
        {
            switch (obj.requestOrigin)
            {
                case ConfigOrigin.Default:
                    break;
                case ConfigOrigin.Cached:
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log ("New settings loaded this session; update values accordingly.");
                    var jsonData = ConfigManager.appConfig.GetJson("levelConfig");
                    
                    var levelsData = JsonUtility.FromJson<LevelsData>(jsonData);
                    
                    LoadLevelData(levelsData);
           
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
    [Serializable]
    public class LevelRemoteData
    {
        public int m_nbOfAsteroids;
        public int m_nbOfChildren;
    }

    public class LevelsData
    {
        public LevelRemoteData[] m_levels;
    }
}