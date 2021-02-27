using Unity.Entities;
using Unity.Mathematics;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct MovementInfoComponent : IComponentData
    {
        public float3 m_previousPosition;
        public float3 m_directionOfMove;
        public float m_angularImpulse;
        public float m_linearImpulse;
    }
}