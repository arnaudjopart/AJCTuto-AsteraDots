using Unity.Entities;
using Unity.Mathematics;

namespace _Project.Scripts.Components
{
    [GenerateAuthoringComponent]
    public struct MovementCommandsComponentData : IComponentData
    {
        public float3 m_previousPosition;
        public float3 m_currentDirectionOfMove;
        public float3 m_currentAngularCommand;
        public float m_angularImpulse;
        public float m_currentlinearCommand;
    }
}