using System;
using _Project.Scripts.ScriptableObjects.Reward;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace _Project.Scripts.ScriptableObjects.Achievements
{
    [CreateAssetMenu(menuName = "Create AchievementScriptableObject", fileName = "AchievementScriptableObject", order = 0)]
    public class Achievement : ScriptableObject
    {
        public string m_name;
        public string m_description;
        public RewardScriptableObject[] m_rewards;
        public AchievementCondition[] m_conditions;

        public bool m_override;
        //[HideInInspector]
        public string m_guid;

        public bool IsAchievementUnlocked(GameData _gameData)
        {
            if (m_override) return true;
            
            foreach (var condition in m_conditions)
            {
                if (!condition.IsVerified(_gameData)) return false;
            }

            return true;
        }

        private void OnValidate()
        {
            name = m_name;
        }
    }

    public struct GameData
    {
        public float TimeOfPlay;
        public double CurrentScore;
        public double SavedHighScore;
        public int TotalShot;
        public int TotalNumberOfWrapHit;
        public double CurrentLevel;
    }
}
