using _Project.Scripts.Mono;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.Achievements
{
    public class AchievementManager : MonoBehaviour
    {
        public BootStrap m_bootStrap;
        
        public double TimeOfPlay => Time.timeSinceLevelLoad;
        public double CurrentScore { get
        {
            return m_bootStrap.m_currentScore;
        } }
        public double CurrentLevel { get; set; }
        public double CurrentLaserShots { get; set; }
    }
}