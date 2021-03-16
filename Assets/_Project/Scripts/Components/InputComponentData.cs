using Unity.Entities;

namespace _Project.Scripts.Components
{
    public struct InputComponentData : IComponentData
    {
        public bool m_inputLeft;
        public bool m_inputRight;
        public bool m_inputForward;
    }
}