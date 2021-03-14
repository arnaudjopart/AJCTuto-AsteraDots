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
        public bool m_isUnlocked;
        public RewardScriptableObject[] m_rewards;
        public AchievementCondition[] m_conditions;
        
        public bool IsAchievementUnlocked(AchievementManager _achievementManager)
        {
            foreach (var condition in m_conditions)
            {
                if (!condition.IsVerified(_achievementManager)) return false;
            }

            return true;
        }
        
        public bool IsAchievementUnlocked(GameData _gameData)
        {
            foreach (var condition in m_conditions)
            {
                if (!condition.IsVerified(_gameData)) return false;
            }

            return true;
        }
    }

    public struct GameData
    {
        public float TimeOfPlay;
        public double CurrentScore;
        public double CurrentLevel;
        public double CurrentLaserShots;
    }
}
