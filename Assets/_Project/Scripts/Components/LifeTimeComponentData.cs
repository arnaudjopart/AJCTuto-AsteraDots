using Unity.Entities;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct LifeTimeComponentData : IComponentData
    {
        public float m_maxLifeTime;
        public float m_currentLifeTime;
    }
}
