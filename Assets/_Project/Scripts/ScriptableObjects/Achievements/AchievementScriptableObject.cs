using _Project.Scripts.ScriptableObjects.Reward;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace _Project.Scripts.ScriptableObjects.Achievements
{
    [CreateAssetMenu(menuName = "Create AchievementScriptableObject", fileName = "AchievementScriptableObject", order = 0)]
    public class AchievementScriptableObject : ScriptableObject
    {
        public string m_name;
        public string m_description;
        public bool m_isUnlocked;
        public RewardScriptableObject[] m_rewards;
        public AchievementConditionScriptableObject[] m_conditions;
        
        public bool IsAchievementUnlocked(AchievementManager _achievementManager)
        {
            foreach (var condition in m_conditions)
            {
                if (!condition.IsVerified(_achievementManager)) return false;
            }

            return true;
        }
    }
}
