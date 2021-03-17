using System;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.Achievements
{
    [CreateAssetMenu(menuName = "Create AchievementCollection", fileName = "AchievementCollection", order = 0)]
    public class AchievementCollection : ScriptableObject
    {
        public Achievement[] m_achievements;
        private void OnValidate()
        {
            var guids = AssetDatabase.FindAssets("t:Achievement");
            Debug.Log(guids.Length);

            m_achievements = new Achievement[guids.Length];
            
            for(var i =0;i<m_achievements.Length;i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                m_achievements[i] = AssetDatabase.LoadAssetAtPath<Achievement>(path);
                m_achievements[i].m_guid = guids[i];
            }
        }
    }
}