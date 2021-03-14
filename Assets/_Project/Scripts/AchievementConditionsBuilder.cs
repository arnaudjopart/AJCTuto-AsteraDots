using _Project.Scripts.ScriptableObjects.Achievements;
using UnityEngine;

namespace _Project.Test
{
    public class AchievementConditionsBuilder
    {
        private AchievementCondition.TYPE m_type;
        private float m_threshold;

        public AchievementConditionsBuilder WithType(AchievementCondition.TYPE _type)
        {
            m_type = _type;
            return this;
        }
        public AchievementConditionsBuilder WithThreshold(float _threshold)
        {
            m_threshold = _threshold;
            return this;
        }
        public AchievementCondition Build()
        {
            var achievementCondition = ScriptableObject.CreateInstance<AchievementCondition>();
            achievementCondition.m_type = m_type;
            achievementCondition.m_threshold = m_threshold;

            return achievementCondition;
        }

        public static implicit operator AchievementCondition(AchievementConditionsBuilder builder)
        {
            return builder.Build();
        }
    }
}