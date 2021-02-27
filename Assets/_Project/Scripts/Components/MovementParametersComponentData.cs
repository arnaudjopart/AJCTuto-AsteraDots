using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementParametersComponentData : IComponentData
{
    public float m_linearSpeed;
    public float m_maxLinearVelocity;
    public float m_angularSpeed;
    public float m_maxAngularVelocity;
}