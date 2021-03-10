using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Level/Level Data")]
    public class LevelDataScriptableObject : ScriptableObject
    {
        public int m_nbOfAsteroids;
        public int m_nbOfChildren;

        public int NbOfHits
        {
            get { return m_nbOfAsteroids*(1+m_nbOfChildren+(m_nbOfChildren*m_nbOfChildren)); }
        }
    }
}