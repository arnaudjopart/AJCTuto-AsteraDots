using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Project.Scripts.ScriptableObjects.Achievements;
using UnityEngine;

namespace _Project.Scripts.Mono
{
    public class AchievementManager : MonoBehaviour
    {
        public UIAchievementDisplay m_achievementDisplay;
        private Queue<Achievement> m_queue;
        public AchievementCollection m_achievementCollection;
        public List<Achievement> m_achievementsToUnlockList = new List<Achievement>();
        public BootStrap m_bootStrap;
        
        public AchievementGuids m_achievementsUnlockGuids;

        private void Awake()
        {
            m_queue = new Queue<Achievement>();
            m_achievementDisplay = FindObjectOfType<UIAchievementDisplay>();
            m_achievementsToUnlockList = m_achievementCollection.m_achievements.ToList();
        }

        private void Start()
        {
            var sortingSystem = new AchievementSortingSystemFromPlayerPrefs();
            var json = DataSaveLoadUtils.LoadAchievementData();

            m_achievementsUnlockGuids = !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<AchievementGuids>(json) : new AchievementGuids();

            sortingSystem.m_data = m_achievementsUnlockGuids;
            sortingSystem.Sort(ref m_achievementsToUnlockList );
            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteAchievementProgress();
            }
            for (var i = m_achievementsToUnlockList.Count - 1; i >= 0; i--)
            {
                var achievement = m_achievementsToUnlockList[i];
                
                var isUnlockThisFrame = achievement.IsAchievementUnlocked(m_bootStrap.m_gameData);

                if (!isUnlockThisFrame) continue;
                
                m_achievementsUnlockGuids.m_achievementGuids.Add(achievement.m_guid);
                m_queue.Enqueue(achievement);
                m_achievementsToUnlockList.RemoveAt(i);
                SaveAchievementProgress();
                m_bootStrap.SaveGameData();
            }
            
            HandleAchievementQueue();
        }

        private void DeleteAchievementProgress()
        {
            DataSaveLoadUtils.SaveAchievementData("");
        }

        private void SaveAchievementProgress()
        {
            var json = JsonUtility.ToJson(m_achievementsUnlockGuids);
            DataSaveLoadUtils.SaveAchievementData(json);
        }

        private void HandleAchievementQueue()
        {
            if (m_achievementDisplay.IsCurrentlyDisplayingAchievementNotification) return;
            if (m_queue.Count == 0) return;
            var currentAchievement = m_queue.Dequeue();
            m_achievementDisplay.DisplayAchievementAlert(currentAchievement);
            
        }
        
    }

    internal abstract class AchievementSortingSystemBase
    {
        public AchievementGuids m_data;
        public abstract void Sort(ref List<Achievement> _achievements);
        
    }

    internal class AchievementSortingSystemFromPlayerPrefs : AchievementSortingSystemBase
    {
        public override void Sort(ref List<Achievement> _achievements)
        {
            
            
            for(var i = _achievements.Count-1;i>=0;i--)
            {
                foreach (var guid in m_data.m_achievementGuids)
                {
                    if (_achievements[i].m_guid != guid) continue;
                    Debug.Log(_achievements[i].m_name + " is already unlocked");
                    _achievements.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class DataSaveLoadUtils
    {
        private const string ACHIEVEMENT_KEY = "achievements";
        private const string BEST_SCORE_KEY = "bestScore";
        private const string GAME_DATA = "gameData";

        public static string LoadAchievementData()
        {
            var path = Application.persistentDataPath +"/"+ ACHIEVEMENT_KEY+".txt";
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }
        
        public static void SaveAchievementData(string _data)
        {
            var path = Application.persistentDataPath +"/"+ ACHIEVEMENT_KEY+".txt";
            File.WriteAllText(path, _data);
            //PlayerPrefs.SetString(ACHIEVEMENT_KEY,_data);
        }
        
        public static int LoadBestScore()
        {
            return PlayerPrefs.GetInt(BEST_SCORE_KEY);
        }
        
        public static void SaveBestScore(int _data)
        {
            PlayerPrefs.SetInt(BEST_SCORE_KEY,_data);
        }

        public static void SaveGameData(string _data)
        {
            var path = Application.persistentDataPath +"/"+ GAME_DATA+".txt";
            File.WriteAllText(path, _data);
        }
        
        public static string GetGameData()
        {
            var path = Application.persistentDataPath +"/"+ GAME_DATA+".txt";
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }
    }

    [System.Serializable]
    public class AchievementGuids
    {
        public AchievementGuids()
        {
            m_achievementGuids = new List<string>();
        }
        public List<string> m_achievementGuids;
    }
}