using Unity.Entities;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct AsteroidTagComponent : IComponentData
    {
        public int m_size;
        public int m_nbChildren;
        public int m_points;
    }
}