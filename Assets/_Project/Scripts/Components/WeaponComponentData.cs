using Unity.Entities;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct WeaponComponentData : IComponentData
    {
        public Entity m_projectilePrefab;
        public bool m_isFiring;
        public float m_fireRate;
        public float m_timer;
    }
}
