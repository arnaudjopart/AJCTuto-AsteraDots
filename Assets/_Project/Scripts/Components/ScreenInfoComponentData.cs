using Unity.Entities;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]  
    public struct ScreenInfoComponentData : IComponentData
    {
        public float m_height;
        public float m_width;
    }
}