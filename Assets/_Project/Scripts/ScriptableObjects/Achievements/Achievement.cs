using System;
using System.Linq;
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

        [HideInInspector]
        public string m_guid;

        public bool IsAchievementUnlocked(GameData _gameData)
        {
            return m_conditions.All(condition => condition.IsVerified(_gameData));
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
