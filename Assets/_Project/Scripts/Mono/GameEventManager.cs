using UnityEngine;

namespace _Project.Scripts.Mono
{
    public class GameEventManager : MonoBehaviour
    {
        private void Awake()
        {
            m_instance = this;
        }

        public GameEvent m_onPlayerDeathEvent;
        public IntGameEvent m_onAsteroidDestroyedEvent;
        
        private static GameEventManager m_instance;

        public static void RaisePlayerDeathEvent()
        {
            m_instance.m_onPlayerDeathEvent.Raise();
        }
        
        public static void RaiseAsteroidDestroyedEvent(int _score)
        {
            m_instance.m_onAsteroidDestroyedEvent.Raise(_score);
        }
    }
}