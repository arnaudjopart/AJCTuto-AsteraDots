using Unity.Entities;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct MovementParametersComponentData : IComponentData
    {
        public float m_linearVelocity;
        public float m_maxLinearVelocity;
        public float m_angularVelocity;
        public float m_maxAngularVelocity;
    }
}