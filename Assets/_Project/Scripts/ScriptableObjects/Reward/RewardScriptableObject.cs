using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.Reward
{
    [CreateAssetMenu(menuName = "Create RewardScriptableObject", fileName = "RewardScriptableObject", order = 0)]
    public class RewardScriptableObject : ScriptableObject
    {
        public Material m_material;
    }
}