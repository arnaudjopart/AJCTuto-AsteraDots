using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.Achievements;
using UnityEngine;

namespace _Project.Scripts.Mono
{
    public class AchievementManager : MonoBehaviour
    {
        public UIAchievementDisplay m_achievementDisplay;
        private Queue<Achievement> m_queue;
        public AchievementCollection m_achievementCollection;
        public BootStrap m_bootStrap;

        private void Awake()
        {
            m_queue = new Queue<Achievement>();
            m_achievementDisplay = FindObjectOfType<UIAchievementDisplay>();
        }

        public void Update()
        {
            GameData gameData = m_bootStrap.m_gameData;
            
            foreach (var achievement in m_achievementCollection.m_achievements)
            {
                if (achievement.m_isUnlocked) continue;
                var isUnlockThisFrame = achievement.IsAchievementUnlocked(gameData);
                achievement.m_isUnlocked = isUnlockThisFrame;
                if(isUnlockThisFrame) m_queue.Enqueue(achievement);
            }

            HandleAchievementQueue();
        }

        private void HandleAchievementQueue()
        {
            if (m_achievementDisplay.IsCurrentlyDisplayingAchievementNotification) return;
            if (m_queue.Count == 0) return;
            var currentAchievement = m_queue.Dequeue();
            m_achievementDisplay.DisplayAchievementAlert(currentAchievement);
        }
        
    }
}