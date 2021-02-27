using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementParametersComponentData : IComponentData
{
    public float m_physicsLinearImpulse;
    public float m_maxLinearVelocity;
    public float m_physicsAngularImpulse;
    public float m_maxAngularVelocity;
}