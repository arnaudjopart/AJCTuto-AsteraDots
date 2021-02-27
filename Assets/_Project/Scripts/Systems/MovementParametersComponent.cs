using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementParametersComponent : IComponentData
{
    public float3 m_previousPosition;
    public float3 m_directionOfMove;
    public float m_angularImpulse;
    public float m_linearImpulse;
}