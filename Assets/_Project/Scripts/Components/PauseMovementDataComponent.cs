using Unity.Entities;
using Unity.Mathematics;

public struct PauseMovementDataComponent : ISystemStateComponentData
{
    public float3 m_savedLinearVelocity;
    public float3 m_savedAngularVelocity;
    public float m_savedLinearImpulse;
    public float m_savedAngularImpulse;
}