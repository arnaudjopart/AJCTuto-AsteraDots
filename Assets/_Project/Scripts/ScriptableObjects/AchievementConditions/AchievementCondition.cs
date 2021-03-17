using System;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.Achievements
{
    [CreateAssetMenu(menuName = "Create AchievementConditionScriptableObject", fileName = "AchievementConditionScriptableObject", order = 0)]
    public class AchievementCondition : ScriptableObject
    {
        public enum TYPE{TIME, SCORE, LEVEL, SHOTS,WARP}

        public TYPE m_type;
        public float m_threshold;
        
        
        public bool IsVerified(GameData _data)
        {
            return m_type switch
            {
                TYPE.TIME => _data.TimeOfPlay >= m_threshold,
                TYPE.SCORE => _data.CurrentScore >= m_threshold,
                TYPE.LEVEL => _data.CurrentLevel >= m_threshold,
                TYPE.SHOTS => _data.TotalShot>= m_threshold,
                TYPE.WARP => _data.TotalNumberOfWrapHit>= m_threshold,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}